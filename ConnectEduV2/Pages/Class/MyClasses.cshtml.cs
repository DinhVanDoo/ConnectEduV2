using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using X.PagedList;

namespace ConnectEduV2.Pages.Class
{
    public class MyClassesModel : PageModel
    {
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassRegistrationRepository _classRegistrationRepository;
        private readonly IRegistrationStatusRepository _classRegistrationStatusRepository;

        public MyClassesModel(IClassRepository classRepository, ISubjectRepository subjectRepository, ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository, IClassRegistrationRepository classRegistrationRepository, IRegistrationStatusRepository classRegistrationStatusRepository)
        {
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _classRegistrationRepository = classRegistrationRepository;
            _classRegistrationStatusRepository = classRegistrationStatusRepository;
        }
        public IPagedList<ConnectEduV2.Models.Class> Classes { get; set; }
        public IActionResult OnGet(int pageNumber = 1, int pageSize = 3)
        {
            string? accJson = HttpContext.Session.GetString("User");
            ConnectEduV2.Models.User? acc = JsonConvert.DeserializeObject<ConnectEduV2.Models.User>(accJson);
            var includes = new string[] { "Subject", "ClassRegistrations", "ClassStatus", "User" };
            Classes = _classRepository.GetMulti(cl => cl.UserId == acc.Id, includes).ToPagedList(pageNumber, pageSize);
            return Page();
        }
    }


}

