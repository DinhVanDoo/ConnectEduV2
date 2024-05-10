using Azure;
using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using X.PagedList;

namespace ConnectEduV2.Pages.Class
{
    public class ClassesModel : PageModel
    {
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassRegistrationRepository _classRegistrationRepository;
        private readonly IRegistrationStatusRepository _classRegistrationStatusRepository;

        public ClassesModel(IClassRepository classRepository, ISubjectRepository subjectRepository, ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository, IClassRegistrationRepository classRegistrationRepository, IRegistrationStatusRepository classRegistrationStatusRepository)
        {
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _classRegistrationRepository = classRegistrationRepository;
            _classRegistrationStatusRepository = classRegistrationStatusRepository;
        }
        public int? SubjectID { get; set; }
        public DateTime? Start_date { get; set; }
        public DateTime? End_date { get; set; }
        public string Key { get; set; }
        public IPagedList<ConnectEduV2.Models.Class> Classes { get; set; }
        public IActionResult OnGet(DateTime? start_date, DateTime? end_date, int? subjectID, int pageNumber = 1, int pageSize = 5, string searchKeyword = "")
        {
            var includes = new string[] { "User", "Subject", "ClassStatus", "ClassRegistrations" };
            var classes = _classRepository.GetMulti(
                classes => classes.SubjectId == subjectID
                            && (string.IsNullOrEmpty(searchKeyword)
                            || classes.Name.Contains(searchKeyword)
                            || classes.User.Email.Contains(searchKeyword)
                            || classes.User.Name.Contains(searchKeyword))
                            && (start_date == null || classes.StartTime >= start_date || classes.EndTime >= start_date)
                            ,
            includes: includes
                ).ToPagedList(pageNumber, pageSize);
            Classes = classes;
            SubjectID = subjectID;
            Start_date = start_date;
            End_date = end_date;
            Key = searchKeyword;
            return Page();
        }
    }
}
