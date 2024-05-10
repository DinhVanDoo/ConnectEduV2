using ConnectEduV2.Filters;
using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ConnectEduV2.Pages.Wallet
{
    [ServiceFilter(typeof(StudentAndMentorFillter))]
    public class MyWalletModel : PageModel
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepositTransaction _depositTransaction;

        public MyWalletModel(IWalletRepository walletRepository, IUserRepository userRepository, IDepositTransaction depositTransaction)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _depositTransaction = depositTransaction;
        }
        public ConnectEduV2.Models.Wallet Wallet { get; set; }
        public List<DepositTransaction> DepositTransactions { get; set; }

        public IActionResult OnGet()
        {
            string? accJson = HttpContext.Session.GetString("User");
            User? acc = JsonConvert.DeserializeObject<User>(accJson);
            var includes = new string[] { "User", "DepositTransactions" };
            ConnectEduV2.Models.Wallet wallet = _walletRepository.GetSingleByCondition(wallet => wallet.UserId == acc.Id);
            var list = _depositTransaction.GetMulti(list => list.WalletId == wallet.Id).OrderByDescending(list => list.Id);
            DepositTransactions = list.ToList();
            Wallet = wallet;
            return Page();
        }
        public IActionResult OnPost(decimal amount)
        {

            string? accJson = HttpContext.Session.GetString("User");
            User? acc = JsonConvert.DeserializeObject<User>(accJson);
            ConnectEduV2.Models.Wallet wallet = _walletRepository.GetSingleByCondition(wallet => wallet.UserId == acc.Id);
            wallet.Amount = wallet.Amount + amount;
            DepositTransaction trans = new DepositTransaction();
            trans.Amount = amount;
            trans.WalletId = wallet.Id;
            trans.PaymentStatusId = 1;
            trans.Date = DateTime.Now;
            _walletRepository.Update(wallet);
            _walletRepository.SaveChanges();
            _depositTransaction.Add(trans);
            _depositTransaction.SaveChanges();
            return OnGet();

        }

    }
}
