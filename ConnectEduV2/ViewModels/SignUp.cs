using ConnectEduV2.Models;

namespace ConnectEduV2.ViewModels
{
    public class SignUp
    {
        public User? User { get; set; }
        public List<School>? SchoolList { get; set; }
        public List<Department>? DepartmentList { get; set; }
    }
}
