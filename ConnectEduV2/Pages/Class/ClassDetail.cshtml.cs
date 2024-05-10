using ConnectEduV2.Filters;
using ConnectEduV2.Hubs;
using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using ConnectEduV2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ConnectEduV2.Pages.Class
{
    public class ClassDetailModel : PageModel
    {
        private readonly IClassRepository _classRepository;
        private readonly IClassRegistrationRepository _classRegistrationRepository;
        private readonly IRegistrationStatusRepository _classRegistrationStatusRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ConnectEduV1Context _context = new ConnectEduV1Context();
        private readonly IHubContext<ChatHub> _hubContext;

        public ClassDetailModel(IClassRepository classRepository, IClassRegistrationRepository classRegistrationRepository, IRegistrationStatusRepository classRegistrationStatusRepository, IFeedbackRepository feedbackRepository, IHubContext<ChatHub> hubContext)
        {
            _classRepository = classRepository;
            _classRegistrationRepository = classRegistrationRepository;
            _classRegistrationStatusRepository = classRegistrationStatusRepository;
            _feedbackRepository = feedbackRepository;
            _hubContext = hubContext;
        }

        public ConnectEduV2.Models.Class ClassDetail { get; set; }
        public List<ClassRegistration> classRegistrations { get; set; }
        public List<Feedback> feedbacks { get; set; }
        public List<ClassChat> ClassChats { get; set; }
        public List<Chats> Chats { get; set; } = new List<Chats>();

        public IActionResult OnGet(int? classID)
        {
            var includes = new string[] { "User", "Subject", "ClassStatus" };
            var includes2 = new string[] { "User", "RegistrationStatus" };
            var includes3 = new string[] { "User", "Registration", "Class" };
            var classdetail = _classRepository.GetSingleByCondition(c => c.Id == classID, includes);
            var regis = _classRegistrationRepository.GetMulti(o => o.ClassId == classID && o.RegistrationStatusId != 3, includes2);
            feedbacks = _feedbackRepository.GetMulti(fb => fb.ClassId == classID, includes3).OrderByDescending(x => x.Id).ToList();
            classRegistrations = regis.ToList();
            ClassDetail = classdetail;
            ClassChats = _context.ClassChats.Include(x => x.Class).ThenInclude(x => x.User).Include(x => x.ClassRegistration).ThenInclude(x => x.User).Where(c => c.ClassId == classID).ToList();
            foreach (var classChat in ClassChats)
            {
                Chats chat = new Chats
                {
                    Name = classChat.ClassRegistration == null ?  classChat.Class?.User.Name + " (Mentor)" : classChat.ClassRegistration.User.Name ,                   
                    Content = classChat.ChatContent,
                    Date = classChat.ChatDate?.ToString("dd/MM/yyyy HH:mm"),
                    Img = classChat.ClassRegistration == null ? classChat.Class?.User.Image  : classChat.ClassRegistration.User.Image,
                };
                Chats.Add(chat);
            }
            return Page();
        }

        public IActionResult OnGetEnroll(int? classID)
        {
            string? accJson = HttpContext.Session.GetString("User");

            if (accJson == null)
            {
                return RedirectToPage("/Login/Login");
            }
            else
            {
                User? acc = JsonConvert.DeserializeObject<User>(accJson);
                if (acc.RoleId == 2)
                {
                    var check = _classRegistrationRepository.GetMulti(check => check.UserId == acc.Id && check.ClassId == classID).OrderByDescending(c => c.Id).FirstOrDefault();
                    if (check == null)
                    {
                        ClassRegistration registration = new ClassRegistration();
                        registration.UserId = acc.Id;
                        registration.ClassId = classID;
                        registration.Date = DateTime.Now;
                        registration.RegistrationStatusId = 1;
                        registration.PaymentStatusId = 2;
                        _classRegistrationRepository.Add(registration);
                        _classRegistrationRepository.SaveChanges();
                    }
                    if (check != null && check.RegistrationStatusId == 3)
                    {
                        ClassRegistration registration = new ClassRegistration();
                        registration.UserId = acc.Id;
                        registration.ClassId = classID;
                        registration.Date = DateTime.Now;
                        registration.RegistrationStatusId = 1;
                        registration.PaymentStatusId = 2;
                        _classRegistrationRepository.Add(registration);
                        _classRegistrationRepository.SaveChanges();
                    }
                }
                return RedirectToPage("/Class/ClassRegistrationList");
            }

        }

        public async Task<IActionResult> OnGetSend(string? chatContent, int? classid)
        {
            string? accJson = HttpContext.Session.GetString("User");
            if (accJson == null )
            {
                return new StatusCodeResult(204);
            }
            else
            {
                User? acc = JsonConvert.DeserializeObject<User>(accJson);
                ClassRegistration? check = _classRegistrationRepository.GetSingleByCondition(c => c.UserId == acc.Id && c.ClassId == classid);
                var checkMentor = _classRepository.GetSingleByCondition(c => c.Id == classid && c.UserId == acc.Id);
                if (check == null && acc.RoleId ==2)
                {
                    
                    var responseData = new { errorChat = "You have to enroll this class to chat" };
                    return new JsonResult(responseData);
                    

                }else if (acc.RoleId == 3)
                {
                    if(checkMentor == null)
                    {
                       
                        var responseData = new { errorChat = "You have to be mentor of this class to chat" };
                        return new JsonResult(responseData);
                        
                    }
                    ClassChat chat = new ClassChat
                    {
                        ClassId = classid,
                       
                        ChatContent = chatContent,
                        ChatDate = DateTime.UtcNow
                    };
                    _context.ClassChats.Add(chat);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("ReceiveChat");
                    return OnGet(classid);
                }
                else
                {
                    ClassChat chat = new ClassChat
                    {
                        ClassId = classid,
                        ClassRegistrationId = check.Id,
                        ChatContent = chatContent,
                        ChatDate = DateTime.UtcNow
                    };
                    _context.ClassChats.Add(chat);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("ReceiveChat");
                    return OnGet(classid);
                }
            }
        }

        public IActionResult OnGetChat(int? classid)
        {
            ClassChats = _context.ClassChats
                .Include(x => x.Class)
                .ThenInclude(x => x.User)
                .Include(x => x.ClassRegistration)
                .ThenInclude(x => x.User).Where(c => c.ClassId == classid).ToList();

            Chats = ClassChats.Select(classChat => new Chats
            {
                Name = classChat.ClassRegistration == null ? classChat.Class?.User.Name + " (Mentor)" : classChat.ClassRegistration.User.Name,
                Content = classChat.ChatContent,
                Date = classChat.ChatDate?.ToString("dd/MM/yyyy HH:mm"),
                Img = classChat.ClassRegistration == null ? classChat.Class?.User.Image : classChat.ClassRegistration.User.Image,
            }).ToList();

            return new JsonResult(new { newChat = Chats });
        }

    }
}
    
