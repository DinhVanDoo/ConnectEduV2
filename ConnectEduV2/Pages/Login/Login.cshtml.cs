using ConnectEdu.Validator;
using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using ConnectEduV2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ConnectEduV2.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [BindProperty]
        [Required(ErrorMessage = "Please Enter Gmail!")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please Enter Password!")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? Password { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }

        public ActionResult OnGetLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login/Login", "OnGet");
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var includes = new string[] { "Wallet", "School", "Department", "Status" };

                User user = _userRepository.GetSingleByCondition(u => u.Email == Email && u.Password == Password, includes);
                if (user != null)
                {
                    if (user.StatusId == 2)
                    {
                        HttpContext.Session.SetString("Email", user.Email.ToString());
                        return RedirectToPage("/SignUp/SignUp", "Send");
                    }
                    else
                    {
                        var settings = new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore    
                        };
                        string userJson = JsonConvert.SerializeObject(user, settings);
                        HttpContext.Session.SetString("User", userJson);

                        if (user.RoleId == 1)
                        {
                            return Redirect("/Admin/Home");
                        }
                        else
                        {
                            return RedirectToPage("/Home/Home");
                        }
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = "Invalid email or password.";
                    return Page();
                }
            }
            else
            {
                // Model state is not valid, return the page with validation errors
                return Page();
            }
        }


    }
}
