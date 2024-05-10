using ConnectEduV2.Models;
using X.PagedList;

namespace ConnectEduV2.ViewModels
{
    public class Home
    {
        public IPagedList<Subject>? Subjects { get; set; }
        public List<School>? Schools { get; set; }
        public List<Department>? Department { get; set; }
        public List<Semester>? Semesters { get; set; }
    }
}
