using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConnectEduV2.Areas.Admin.Pages.Edu
{
    public class CreateSubjectModel : PageModel
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;

        public CreateSubjectModel(ISubjectRepository subjectRepository, ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository)
        {
            _subjectRepository = subjectRepository;
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
        }
        public List<School> Schools { get; set; }
        public List<Department> Departments { get; set; }
        public List<Semester> Semesters { get; set; }

        public string Message { get; set; }
        public IActionResult OnGet()
        {
            string[] include = { "School" };
            string[] include2 = { "School" , "Department"};
            Schools = _schoolRepositoriy.GetAll().ToList();
            Departments = _departmentRepository.GetAll(include).ToList();
            Semesters = _semesterRepository.GetAll(include2).ToList();
            return Page();
        }
        public IActionResult OnPost(string name, int? schoolId, int? departmentId, int? semesterId)
        {
            if (!string.IsNullOrEmpty(name) && schoolId != null && departmentId != null)
            {
                Subject sub = new Subject();
                sub.Name = name;
                sub.SchoolId = schoolId;
                sub.DerpartmentId = departmentId;
                sub.SemesterId = semesterId;
                sub.DataStatusId = 1;
                _subjectRepository.Add(sub);
                _subjectRepository.SaveChanges();
                Message = "Add Success";
            }
            else
            {
                Message = "Please Enter Name and Choose School and Choose Department and Choose Semester";
            }
            return OnGet();
        }
    }
}
