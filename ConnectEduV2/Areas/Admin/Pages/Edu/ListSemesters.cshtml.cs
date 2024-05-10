using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace ConnectEduV2.Areas.Admin.Pages.Edu
{
    public class ListSemestersModel : PageModel
    {
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IUserRepository _userRepository;

        public ListSemestersModel(ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository, IUserRepository userRepository)
        {
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _userRepository = userRepository;
        }

        public string? SearchKeyword { get; set; }
        public IPagedList<Semester> Semesters { get; set; }
        public void OnGet(int pageNumber = 1, int pageSize = 5, string searchKeyword = "")
        {
            var includes = new string[] { "DataStatus", "School", "Department" };
            var semester = _semesterRepository.GetMulti(
                          semester => string.IsNullOrEmpty(searchKeyword) || semester.Name.Contains(searchKeyword) || semester.Department.Name.Contains(searchKeyword) || semester.School.Name.Contains(searchKeyword), 
                         includes: includes).ToPagedList(pageNumber, pageSize);
            SearchKeyword = searchKeyword; 
            Semesters = semester;
        }
        public IActionResult OnPost(int id, int page)
        {
            var semester = _semesterRepository.GetSingleById(id);
            if (semester != null)
            {
                semester.DataStatusId = semester.DataStatusId == 1 ? 2 : 1;
                _semesterRepository.Update(semester);
                _semesterRepository.SaveChanges();
            }
            return RedirectToPage("/Edu/ListSemesters", new { page = page });
        }
    }
}
