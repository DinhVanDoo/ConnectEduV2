using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;
using Microsoft.EntityFrameworkCore;

namespace ConnectEduV2.Repositories
{
    public interface IPurchaseTransactionRepository : IRepository<PurchaseTransaction>
    {
        public int GetNewID();
    }
    public class PurchaseTransactionRepository : RepositoryBase<PurchaseTransaction>, IPurchaseTransactionRepository
    {
        public PurchaseTransactionRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
     
            public int GetNewID()
            {
            using(ConnectEduV1Context context = new ConnectEduV1Context())
            { // Lấy ID lớn nhất từ bảng PurchaseTransaction
                int maxId = context.PurchaseTransactions.Max(pt => pt.Id);
            return maxId ;
            }
            }
    }
}
