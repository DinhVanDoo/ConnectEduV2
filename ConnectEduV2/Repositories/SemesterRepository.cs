using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;

namespace ConnectEduV2.Repositories
{
    public interface ISemesterRepository : IRepository<Semester>
    {
    }
    public class SemesterRepository : RepositoryBase<Semester>, ISemesterRepository
    {
        public SemesterRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
