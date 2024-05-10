using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;


namespace ConnectEduV2.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
    }
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
