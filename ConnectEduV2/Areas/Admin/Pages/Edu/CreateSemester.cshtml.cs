using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConnectEduV2.Areas.Admin.Pages.Edu
{
    public class CreateSemesterModel : PageModel
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISchoolRepositoriy _schoolRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public CreateSemesterModel(ISemesterRepository semesterRepository, ISchoolRepositoriy schoolRepository, IDepartmentRepository departmentRepository)
        {
            _semesterRepository = semesterRepository;
            _schoolRepository = schoolRepository;
            _departmentRepository = departmentRepository;
        }
        public List<School> Schools { get; set; }
        public List<Department> Departments { get; set; }
        public string Message { get; set; }

        public IActionResult OnGet()
        {
            string[] include = { "School" }; // {"School", "Department
            Schools = _schoolRepository.GetAll().ToList();
            Departments = _departmentRepository.GetAll(include).ToList();
            return Page();
        }
        public IActionResult OnPost(string name, int? schoolId, int? departmentId)
        {
            if (!string.IsNullOrEmpty(name) && schoolId != null && departmentId != null)
            {
                Semester semester = new Semester();
                semester.Name = name;
                semester.SchoolId = schoolId;
                semester.DepartmentId = departmentId;
                semester.DataStatusId = 1;
                _semesterRepository.Add(semester);
                _semesterRepository.SaveChanges();
                Message = "Add Success";
            }else
            {
                Message = "Please Enter Name and Choose School and Choose Department";
            }
            return OnGet();
        }
    }
}
