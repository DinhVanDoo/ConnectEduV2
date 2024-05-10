using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace ConnectEduV2.Areas.Admin.Pages.Edu
{
    public class ListDepartmentsModel : PageModel
    {
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IUserRepository _userRepository;

        public ListDepartmentsModel(ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository, IUserRepository userRepository)
        {
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _userRepository = userRepository;
        }
        public string? SearchKeyword { get; set; }
        public IPagedList<Department> Departments { get; set; }
        public IActionResult OnGet(int page = 1, int pageSize = 5, string searchKeyword = "")
        {
            var includes = new string[] { "DataStatusNavigation", "School" };
            var departs = _departmentRepository.GetMulti(
                          departs => string.IsNullOrEmpty(searchKeyword) || departs.Name.Contains(searchKeyword) || departs.School.Name.Contains(searchKeyword),
                         includes: includes).ToPagedList(page, pageSize);
            SearchKeyword = searchKeyword; // Lưu từ khóa tìm kiếm vào ViewBag
            Departments = departs;
            return Page();
        }
        public IActionResult OnPost(int id, int page)
        {
            var department = _departmentRepository.GetSingleById(id);
            if (department != null)
            {
                department.DataStatus = department.DataStatus == 1 ? 2 : 1;
                _departmentRepository.Update(department);
                _departmentRepository.SaveChanges();
            }
            return RedirectToPage("/Edu/ListDepartments", new { page = page });
        }
        
    }
}
