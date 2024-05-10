using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;

namespace ConnectEduV2.Pages.SignUp
{
    public class VerifyRegisterModel : PageModel
    {
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _validator;
        private readonly IWalletRepository _walletRepository;

        public VerifyRegisterModel(ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, IUserRepository userRepository, IValidator<User> validator, IWalletRepository walletRepository)
        {
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _validator = validator;
            _walletRepository = walletRepository;
        }
        [BindProperty]
        public string VerifyCode { get; set; }

        public IActionResult OnGet()
        {
            var modValue = TempData["mod"];
            if (modValue != null)
            {
                return Page();
            }
            else
            {
                ViewData["ErrorMessage"] = "Verify email incorrect! Please input again.";
                return Page();
            }
        }
        public IActionResult OnPost()
        {

            int? storedRandomNumber = HttpContext.Session.GetInt32("RandomNumber");

            if (storedRandomNumber.HasValue && !string.IsNullOrEmpty(VerifyCode))
            {
                int userEnteredNumber;

                if (int.TryParse(VerifyCode, out userEnteredNumber) && userEnteredNumber == storedRandomNumber.Value)
                {
                    string email = HttpContext.Session.GetString("Email");
                    User user = _userRepository.GetSingleByCondition(u => u.Email == email);
                    user.StatusId = 1;
                    user.RoleId = 2;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                    ConnectEduV2.Models.Wallet wallet = new ConnectEduV2.Models.Wallet();
                    wallet.Amount = 0;
                    wallet.UserId = user.Id;
                    _walletRepository.Add(wallet);
                    _walletRepository.SaveChanges();
                    return RedirectToPage("/Login/Login", "OnGet");
                }
            }
            return RedirectToPage("/SignUp/VerifyRegister", "OnGet");
        }


        private int GenerateRandom5DigitNumber()
        {
            Random rand = new Random();
            int min = 10000;
            int max = 99999;
            return rand.Next(min, max);
        }

    }
}
