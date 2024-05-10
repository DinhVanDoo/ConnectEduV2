using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;

namespace ConnectEduV2.Repositories
{
    public interface IRegistrationStatusRepository : IRepository<RegistrationStatus>
    {

    }
    public class RegistrationStatusRepository : RepositoryBase<RegistrationStatus>, IRegistrationStatusRepository
    {
        public RegistrationStatusRepository(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
