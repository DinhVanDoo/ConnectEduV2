using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;

namespace ConnectEduV2.Repositories
{
    public interface IWalletRepository : IRepository<Wallet>
    {

    }
    public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
    {
        public WalletRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
