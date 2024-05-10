using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;

namespace ConnectEduV2.Repositories
{
    public interface IRevenueSharingRepository : IRepository<RevenueSharing>
    {

    }
    public class RevenueSharingRepository : RepositoryBase<RevenueSharing>, IRevenueSharingRepository
    {
        public RevenueSharingRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
