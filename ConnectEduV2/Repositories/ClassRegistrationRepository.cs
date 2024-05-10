using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;

namespace ConnectEduV2.Repositories
{
    public interface IClassRegistrationRepository : IRepository<ClassRegistration>
    {

    }
    public class ClassRegistrationRepository : RepositoryBase<ClassRegistration>, IClassRegistrationRepository
    {
        public ClassRegistrationRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
