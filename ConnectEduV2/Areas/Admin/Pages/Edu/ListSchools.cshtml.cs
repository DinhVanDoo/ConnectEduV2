using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace ConnectEduV2.Areas.Admin.Pages.Edu
{
    public class ListSchoolsModel : PageModel
    {
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IUserRepository _userRepository;

        public ListSchoolsModel(ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository, IUserRepository userRepository)
        {
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _userRepository = userRepository;
        }
        public string? SearchKeyword { get; set; }
        public IPagedList<School> Schools { get; set; }
        public void OnGet(int page = 1, int pageSize = 5, string searchKeyword = "")
        {
            var includes = new string[] { "DataStatus" };
            var schools = _schoolRepositoriy.GetMulti(
                schools => string.IsNullOrEmpty(searchKeyword) || schools.Name.Contains(searchKeyword),
                includes: includes
            ).ToPagedList(page, pageSize);
            SearchKeyword = searchKeyword; // Lưu từ khóa tìm kiếm vào ViewBag
            Schools = schools;       
        }
        public IActionResult OnPost(int id, int page)
        {
            var school = _schoolRepositoriy.GetSingleById(id);
            if (school != null)
            {
                school.DataStatusId = school.DataStatusId == 1 ? 2 : 1;
                _schoolRepositoriy.Update(school);
                _schoolRepositoriy.SaveChanges();
            }           
            return RedirectToPage("/Edu/ListSchools", new {page = page});
        }

    }
}
