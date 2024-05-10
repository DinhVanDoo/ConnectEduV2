using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;

namespace ConnectEduV2.Repositories
{
    public interface IClassRepository : IRepository<Class>
    {

    }

    public class ClassRepository : RepositoryBase<Class>, IClassRepository
    {
        public ClassRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }

}
