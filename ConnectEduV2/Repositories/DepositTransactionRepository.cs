using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;

namespace ConnectEduV2.Repositories
{
    public interface IDepositTransaction : IRepository<DepositTransaction>
    {

    }
    public class DepositTransactionRepository : RepositoryBase<DepositTransaction>, IDepositTransaction
    {
        public DepositTransactionRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
