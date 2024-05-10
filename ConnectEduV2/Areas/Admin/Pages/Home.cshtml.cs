using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConnectEduV2.Areas.Admin.Pages
{
    public class HomeModel : PageModel
    {
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IDepositTransaction _depositTransactionRepository;
        private readonly IRevenueSharingRepository _revenueSharingRepository;
        private readonly ConnectEduV1Context _context = new ConnectEduV1Context();

        public HomeModel(IClassRepository classRepository, IUserRepository userRepository, IWalletRepository walletRepository, IDepositTransaction depositTransactionRepository, IRevenueSharingRepository revenueSharingRepository)
        {
            _classRepository = classRepository;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _depositTransactionRepository = depositTransactionRepository;
            _revenueSharingRepository = revenueSharingRepository;
        }

        public int TotalStudent { get; set; }
        public decimal ProjectWallet { get ; set; }
        public int TotalClass { get; set; }
        public int TotalMentor { get; set; }
        public List<Report> Reports { get; set; }
        public void OnGet()
        {
            TotalStudent = _userRepository.Count(c=>c.RoleId==2);
            TotalMentor = _userRepository.Count(c=>c.RoleId == 3);
            //ProjectWallet = _walletRepository.GetSingleById(9).Amount;
            TotalClass = _classRepository.Count(c=>c.ClassStatusId == 1);
            
            Reports = _context.Reports.ToList();
        }

        public IActionResult OnGetReport()
        {
            return new JsonResult(new { newReport = _context.Reports.ToList() });
        }

    }
}
