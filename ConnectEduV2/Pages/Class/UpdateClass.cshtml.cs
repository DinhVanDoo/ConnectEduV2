using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ConnectEduV2.Pages.Class
{
    public class UpdateClassModel : PageModel
    {
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassRegistrationRepository _classRegistrationRepository;
        private readonly IRegistrationStatusRepository _classRegistrationStatusRepository;

        public UpdateClassModel(IClassRepository classRepository, ISubjectRepository subjectRepository, ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository, IClassRegistrationRepository classRegistrationRepository, IRegistrationStatusRepository classRegistrationStatusRepository)
        {
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _classRegistrationRepository = classRegistrationRepository;
            _classRegistrationStatusRepository = classRegistrationStatusRepository;
        }
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
        public void OnGet(int id)
        {
            var classdetail = _classRepository.GetSingleById(id);
            StartDate = classdetail.StartTime; EndDate = classdetail.EndTime;
            ClassName = classdetail.Name;
            Description = classdetail.Describe;
            CoursePath = classdetail.CoursePath;
            SeatNumber = classdetail.SeatNumber;
            Price = classdetail.Price;
            TempData["classid"] = id;
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var classdetail = _classRepository.GetSingleById((int)TempData["classid"]);
                classdetail.StartTime = StartDate; classdetail.EndTime = EndDate;
                classdetail.Name = ClassName;
                classdetail.Describe = Description;
                classdetail.CoursePath = CoursePath;
                classdetail.SeatNumber = SeatNumber;
                classdetail.Price = Price;
                _classRepository.Update(classdetail);

                _classRepository.SaveChanges();
                ViewData["Success"] = "Class Update successfully";
            }
            
            return Page();
        }
    }
}
