using ConnectEduV2.Interfaces;
using ConnectEduV2.Models;


namespace ConnectEduV2.Repositories
{

    public interface ISchoolRepositoriy : IRepository<School>
    {
        // cái này sinh ra để ae có thể tạo thêm hàm nếu như hàm đó không có trong base (Custom fuiunction)
    }
    //còn đây là hàm để chiển khai hàm mới hoặc custom lại hàm base
    public class SchoolRepositoriy : RepositoryBase<School>, ISchoolRepositoriy
    {
        public SchoolRepositoriy(ConnectEduV1Context dbContext) : base(dbContext)
        {
        }
    }
}
