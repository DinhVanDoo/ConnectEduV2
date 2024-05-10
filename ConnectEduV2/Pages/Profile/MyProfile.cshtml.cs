using ConnectEduV2.Filters;
using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ConnectEduV2.Pages.Profile
{
    [ServiceFilter(typeof(StudentAndMentorFillter))]
    public class MyProfileModel : PageModel
    {
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _validator;

        public MyProfileModel(ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, IUserRepository userRepository, IValidator<User> validator)
        {
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _validator = validator;
        }


        public User User { get; set; }
        public int? SchoolID { get; set; }
        public int? DepartmentID { get; set; }

        public List<School> schools { get; set; }
        public List<Department> departments { get; set; }
        public IActionResult OnGet(string? function, int? schoolID, int? departmentID)
        {

            string? user = HttpContext.Session.GetString("User");
            User = JsonConvert.DeserializeObject<User>(user);
            if (!string.IsNullOrEmpty(function) && function.Equals("UpdateProfile"))
            {
                ViewData["UpdateProfile"] = "On";
                List<School> listSchool = null;
                List<Department> listDepartment = null;
                if (schoolID == null)
                {
                    // Nếu chưa chọn trường thì lấy danh sách trường
                    listSchool = (List<School>)_schoolRepositoriy.GetAll();
                }
                else
                {
                    SchoolID = schoolID;
                    var c = _schoolRepositoriy.GetMulti(sch => sch.Id == (int)schoolID);
                    listSchool = (List<School>)c;

                    if (departmentID == null)
                    {
                        // Nếu đã chọn trường nhưng chưa chọn ngành thì lấy danh sách ngành của trường đó
                        listDepartment = (List<Department>)_departmentRepository.GetMulti(depart => depart.SchoolId == schoolID);

                    }
                    else
                    {
                        DepartmentID = departmentID;
                        var ok = _departmentRepository.GetSingleById((int)departmentID);
                        listDepartment = new List<Department>(); // Initialize the list
                        listDepartment.Add((Department)ok);


                    }
                }
                schools = listSchool;
                departments = listDepartment;

                return Page();
            }
            else if (!string.IsNullOrEmpty(function) && function.Equals("ChangePassword"))
            {
                ViewData["ChangePassword"] = "On";
                if (TempData["ErrorChangePass"] != null)
                {

                    ViewData["ErrorPassword"] = TempData["ErrorChangePass"];
                }


                return Page();
            }
            else if (!string.IsNullOrEmpty(function) && function.Equals("BecomMentor"))
            {
                ViewData["BecomMentor"] = "On";
                if (TempData["SaveFile"] != null)
                {

                    ViewData["SaveFile"] = TempData["SaveFile"];
                }
                if (TempData["SaveFileError"] != null)
                {

                    ViewData["SaveFileError"] = TempData["SaveFileError"];
                }


            }



            return Page();
        }

        public IActionResult OnPostChange(int? id, string? name, string? phone, string? facebook, int? schoolID, int? departID, IFormFile? image)
        {
            var includes = new string[] { "Wallet", "School", "Department", "Status" };
            User user = _userRepository.GetSingleByCondition(u => u.Id == id, includes);
            user.Name = name ?? user.Name;
            user.SchoolId = schoolID ?? user.SchoolId;
            user.DepartmentId = departID ?? user.DepartmentId;
            user.Phone = phone ?? user.Phone;
            user.FacebookPath = facebook ?? user.FacebookPath;

            if (image != null)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                var filePath = Path.Combine("wwwroot/img/uploads", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                user.Image = "/img/uploads/" + uniqueFileName;
            }
            else
            {
                user.Image = "/img/avatarmain.jpg";
            }

            string userJson = JsonConvert.SerializeObject(user, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            HttpContext.Session.SetString("User", userJson);
            _userRepository.Update(user);
            _userRepository.SaveChanges();

            return RedirectToPage("/Profile/MyProfile");
        }

        public IActionResult OnPostChangePassword(string? oldPass, string? newPass)
        {
            if (!string.IsNullOrEmpty(oldPass) && !string.IsNullOrEmpty(newPass))
            {
                User user = _userRepository.GetSingleByCondition(u => u.Password == oldPass);
                if (user == null)
                {
                    TempData["ErrorChangePass"] = "Your password is wrong";
                    return RedirectToPage("/Profile/MyProfile", new { function = "ChangePassword" });

                }
                else
                {
                    user.Password = newPass;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                    HttpContext.Session.Clear();
                    return RedirectToPage("/Login/Login");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult OnPostBecomMentor(string? email, IFormFile? image)
        {
            if (email != null && image != null)
            {
                User user = _userRepository.GetSingleByCondition(u => u.Email.Equals(email));
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                var filePath = Path.Combine("wwwroot/img/ScoreBoards", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                user.ScoreboardPhoto = "/img/ScoreBoards/" + uniqueFileName;
                _userRepository.Update(user);
                _userRepository.SaveChanges();
                TempData["SaveFile"] = "Application sent successfully";
            }
            else
            {
                TempData["SaveFileError"] = "An error occurred, please try again";
            }
            return RedirectToPage("/Profile/MyProfile", new { function = "BecomMentor" });
        }


    }
}
