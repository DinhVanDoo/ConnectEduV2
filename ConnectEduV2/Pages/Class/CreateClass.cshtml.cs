using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ConnectEduV2.Pages.Class
{
    public class CreateClassModel : PageModel
    {
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassRegistrationRepository _classRegistrationRepository;
        private readonly IRegistrationStatusRepository _classRegistrationStatusRepository;

        public CreateClassModel(IClassRepository classRepository, ISubjectRepository subjectRepository, ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository, IClassRegistrationRepository classRegistrationRepository, IRegistrationStatusRepository classRegistrationStatusRepository)
        {
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _classRegistrationRepository = classRegistrationRepository;
            _classRegistrationStatusRepository = classRegistrationStatusRepository;
        }

        public List<School> schools { get; set; }
        public List<Department> department { get; set; }
        public List<Semester> semesters { get; set; }
        public List<Subject> subjects { get; set; }
        public int? SchoolId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SemesterId { get; set; }
        public int? SubjectId { get; set; }

        [BindProperty]
        public DateTime? StartDate { get; set; }
        [BindProperty]
        public int? Subject { get; set; }
        [BindProperty]
        public DateTime? EndDate { get; set; }
        [BindProperty]
        public string? ClassName { get; set; }

        [BindProperty]
        [StringLength(5000, MinimumLength = 30, ErrorMessage = "Description must be between 30 and 2000 characters.")]
        public string? Description { get; set; }

        [BindProperty]
        [Url(ErrorMessage = "CoursePath must be a valid URL.")]
        public string? CoursePath { get; set; }

        [BindProperty]
        [Range(1, int.MaxValue, ErrorMessage = "SeatNumber must be greater than 0.")]
        public int? SeatNumber { get; set; }

        [BindProperty]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal? Price { get; set; }


        public IActionResult OnGet(int? schoolID, int? departmentID, int? semesterID, int? subjectID)
        {
            // Lấy danh sách trường nếu chưa chọn trường
            if (schoolID == null)
            {
                schools = _schoolRepositoriy.GetAll().ToList();
            }
            else
            {

                SchoolId = schoolID;
                // Lấy thông tin trường đã chọn
                var selectedSchool = _schoolRepositoriy.GetSingleById((int)SchoolId);
                schools = new List<School> { selectedSchool };
                if (departmentID == null)
                {
                    // Nếu đã chọn trường nhưng chưa chọn ngành, lấy danh sách ngành của trường đó
                    department = _departmentRepository.GetMulti(depart => depart.SchoolId == SchoolId).ToList();

                }
                else
                {
                    // Lưu departmentID vào ViewBag để sử dụng trong view
                    DepartmentId = departmentID;

                    // Lấy thông tin ngành đã chọn
                    var selectedDepartment = _departmentRepository.GetSingleById((int)DepartmentId);
                    department = new List<Department> { selectedDepartment };

                    if (semesterID == null)
                    {
                        // Nếu đã chọn trường và ngành nhưng chưa chọn kỳ học, lấy danh sách kỳ học
                        semesters = _semesterRepository.GetMulti(x=> x.DepartmentId == departmentID).ToList();
                    }
                    else
                    {
                        // Lưu semesterID vào ViewBag để sử dụng trong view
                        SemesterId = semesterID;

                        // Lấy thông tin kỳ học đã chọn
                        var selectedSemester = _semesterRepository.GetSingleById((int)SemesterId);
                        semesters = new List<Semester> { selectedSemester };

                        if (subjectID == null)
                        {
                            // Nếu đã chọn trường, ngành, và kỳ học nhưng chưa chọn môn học, lấy danh sách môn học
                            subjects = _subjectRepository.GetMulti(sub => sub.DerpartmentId == DepartmentId && sub.SemesterId == SemesterId).ToList();
                        }
                        else
                        {
                            // Lưu subjectID vào ViewBag để sử dụng trong view
                            SubjectId = subjectID;

                            // Lấy thông tin môn học đã chọn
                            var selectedSubject = _subjectRepository.GetSingleById((int)SubjectId);
                            subjects = new List<Subject> { selectedSubject };
                        }
                    }
                }
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                string? accJson = HttpContext.Session.GetString("User");
                User? acc = JsonConvert.DeserializeObject<User>(accJson);

                ConnectEduV2.Models.Class newClass = new ConnectEduV2.Models.Class();
                newClass.Name = ClassName;
                newClass.Describe = Description;
                newClass.StartTime = StartDate;
                newClass.EndTime = EndDate;
                newClass.Price = Price;
                newClass.CoursePath = CoursePath;
                newClass.SubjectId = Subject;
                newClass.ClassStatusId = 1;
                newClass.UserId = acc.Id;
                newClass.SeatNumber = SeatNumber;
                _classRepository.Add(newClass);
                _classRepository.SaveChanges();
                ViewData["Success"] = "Class created successfully";
            }
            else
            {
             
            ViewData["EmptyError"] = "Something error";
            }
           schools = _schoolRepositoriy.GetAll().ToList();
            return Page();
        }
    }
}
