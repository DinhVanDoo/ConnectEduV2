using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;

namespace ConnectEduV2.Repositories
{
    public interface ISubjectRepository : IRepository<Subject>
    {

    }
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public SubjectRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
