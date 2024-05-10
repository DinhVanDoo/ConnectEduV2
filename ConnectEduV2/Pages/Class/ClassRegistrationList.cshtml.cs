using ConnectEduV2.Filters;
using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using X.PagedList;

namespace ConnectEduV2.Pages.Class
{
    [ServiceFilter(typeof(StudentFillter))]
    public class ClassRegistrationListModel : PageModel
    {
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassRegistrationRepository _classRegistrationRepository;
        private readonly IRegistrationStatusRepository _classRegistrationStatusRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IRevenueSharingRepository _revenueSharingRepository;
        private readonly IPurchaseTransactionRepository _purchaseTransactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepositTransaction _depositTransactionRepository;
        private readonly IFeedbackRepository _feedbackRepository;

        public ClassRegistrationListModel(IClassRepository classRepository, ISubjectRepository subjectRepository, IClassRegistrationRepository classRegistrationRepository, IRegistrationStatusRepository classRegistrationStatusRepository, IWalletRepository walletRepository, IRevenueSharingRepository revenueSharingRepository, IPurchaseTransactionRepository purchaseTransactionRepository, IUserRepository userRepository, IDepositTransaction depositTransactionRepository, IFeedbackRepository feedbackRepository)
        {
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _classRegistrationRepository = classRegistrationRepository;
            _classRegistrationStatusRepository = classRegistrationStatusRepository;
            _walletRepository = walletRepository;
            _revenueSharingRepository = revenueSharingRepository;
            _purchaseTransactionRepository = purchaseTransactionRepository;
            _userRepository = userRepository;
            _depositTransactionRepository = depositTransactionRepository;
            _feedbackRepository = feedbackRepository;
        }

        public string? Password { get; set; }
        public IPagedList<ClassRegistration> ClassRegistrations { get; set; }
        public string? FeedbackValue { get; set; }
        public IActionResult OnGet(int pageNumber = 1, int pageSize = 3)
        {
            string? accJson = HttpContext.Session.GetString("User");
            User? acc = JsonConvert.DeserializeObject<User>(accJson);
            var includes = new string[] {  "PaymentStatus", "RegistrationStatus", "User" ,"Feedback" ,"Class"};
            var regitstrations = _classRegistrationRepository.GetMulti(
                regitstrations => regitstrations.UserId == acc.Id,
                includes: includes
            ).OrderByDescending(regitstrations => regitstrations.Id).ToPagedList(pageNumber, pageSize);
            ClassRegistrations = regitstrations;
            return Page();
        }
        public IActionResult OnGetComplete(int id,int pageNumber = 1, int pageSize = 3 )
        {
            string? accJson = HttpContext.Session.GetString("User");
            User? acc = JsonConvert.DeserializeObject<User>(accJson);
            var includes = new string[] { "Class", "PaymentStatus", "RegistrationStatus", "User"  };
            var regitstrations = _classRegistrationRepository.GetSingleById(id);
            regitstrations.RegistrationStatusId = 3;
            _classRegistrationRepository.Update(regitstrations);
            _classRegistrationRepository.SaveChanges();
            return OnGet(pageNumber, pageSize);
        }
        public IActionResult OnPost(int registrationID, decimal amount, string? password)
        {
            var includes = new string[] { "Class", "User" };
            var registration = _classRegistrationRepository.GetSingleByCondition(a => a.Id == registrationID, includes);

            if (registration.User.Password == password)
            {
                var walletOfStudent = _walletRepository.GetSingleByCondition(wallet => wallet.UserId == registration.User.Id);

                if (amount <= walletOfStudent.Amount)
                {
                    var mentor = _userRepository.GetSingleByCondition(mentor => mentor.Id == registration.Class.UserId);
                    var mentorWallet = _walletRepository.GetSingleByCondition(wallet => wallet.UserId == mentor.Id);
                    var adminWallet = _walletRepository.GetSingleByCondition(w=> w.UserId == 9);
                    decimal mentorAmount = amount * 70 / 100;

                    walletOfStudent.Amount -= amount;
                    _walletRepository.Update(walletOfStudent);
                    _walletRepository.SaveChanges();
                    mentorWallet.Amount += mentorAmount;
                    _walletRepository.Update(mentorWallet);
                    _walletRepository.SaveChanges();
                    adminWallet.Amount += (amount - mentorAmount);
                    _walletRepository.Update(adminWallet);
                    _walletRepository.SaveChanges();
                    registration.RegistrationStatusId = 2;
                    _classRegistrationRepository.Update(registration);
                    _classRegistrationRepository.SaveChanges();
                    var depositOfStudent = new DepositTransaction
                    {
                        PaymentStatusId = 1003,
                        Amount = amount,
                        Date = DateTime.UtcNow,
                        WalletId = walletOfStudent.Id
                    };
                    _depositTransactionRepository.Add(depositOfStudent);
                    _depositTransactionRepository.SaveChanges();
                    var depositOfMentor = new DepositTransaction
                    {
                        Amount = mentorAmount,
                        PaymentStatusId = 1002,
                        Date = DateTime.UtcNow,
                        WalletId = mentorWallet.Id
                    };
                    _depositTransactionRepository.Add(depositOfMentor);
                    _depositTransactionRepository.SaveChanges();
                    var depositOfAdmin = new DepositTransaction
                    {
                        Amount = amount - mentorAmount,
                        PaymentStatusId = 1002,
                        Date = DateTime.UtcNow,
                        WalletId = adminWallet.Id
                    };
                    _depositTransactionRepository.Add(depositOfAdmin);
                    _depositTransactionRepository.SaveChanges();

                    var purchase = new PurchaseTransaction
                    {
                        Amount = amount,
                        RegistrationId = registrationID,
                        WalletId = walletOfStudent.Id,
                        Date = DateTime.UtcNow,
                        PaymentStatus = 1
                    };
                    _purchaseTransactionRepository.Add(purchase);
                    _purchaseTransactionRepository.SaveChanges();
                    var revenue = new RevenueSharing
                    {
                        PurchaseId = _purchaseTransactionRepository.GetNewID(),
                        UserId = registration.User.Id,
                        MentorReceived = mentorAmount,
                        ProjectReceived = amount - mentorAmount
                    };
                    _revenueSharingRepository.Add(revenue);
                    _revenueSharingRepository.SaveChanges();
                    registration.PaymentStatusId = 1;
                    _classRegistrationRepository.Update(registration);
                    _classRegistrationRepository.SaveChanges();
                    return OnGet();
                }
                else
                {
                    ViewData["ErrorAmount"] = "Số tiền trong tài khoản không đủ.";  
                    return OnGet();
                }
            }

            return Page();
        }


        public IActionResult OnPostFeedback(int registrationID, int classID, string feedback)
        {
            string? accJson = HttpContext.Session.GetString("User");
            User? acc = JsonConvert.DeserializeObject<User>(accJson);
            var fb = _feedbackRepository.GetSingleByCondition(x => x.UserId == acc.Id && x.RegistrationId == registrationID && classID == x.ClassId);
            if(fb!= null)
            {
                FeedbackValue = fb.Comment;
            }
            else
            {
               Feedback newfb = new Feedback {
                  Comment = feedback,
                  UserId = acc.Id,
                  RegistrationId = registrationID,
                  ClassId = classID,
               };
                _feedbackRepository.Add(newfb);
                _feedbackRepository.SaveChanges();
            }
            return OnGet();
        }

    }
}
