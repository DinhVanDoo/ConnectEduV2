using ConnectEduV2.Hubs;
using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Linq.Expressions;
using X.PagedList;

namespace ConnectEduV2.Pages.Home
{
    public class HomeModel : PageModel
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISchoolRepositoriy _schoolRepositoriy;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ConnectEduV1Context _context = new ConnectEduV1Context();
        private readonly IHubContext<ReportHub> _hubContext;

        public HomeModel(ISubjectRepository subjectRepository, ISchoolRepositoriy schoolRepositoriy, IDepartmentRepository departmentRepository, ISemesterRepository semesterRepository, IHubContext<ReportHub> hubContext)
        {
            _subjectRepository = subjectRepository;
            _schoolRepositoriy = schoolRepositoriy;
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _hubContext = hubContext;
        }

        public IPagedList<Subject> Subjects { get; set; }
        public List<School> Schools { get; set; }
        public List<Semester> Semesters { get; set; }
        public List<Department> Departments { get; set; }

        public string Key { get; set; }
        public int? SchoolID { get; set; }
        public int? SemesterID { get; set; }
        public int? DepartmentID { get; set; }
        public IActionResult OnGet(int? schoolID, int? departmentID, int? semesterID, string key, int pageNumber = 1, int pageSize = 6)
        {
            var includes = new string[] { "Semester", "School", "Derpartment" };
            var includes2 = new string[] { };

            Expression<Func<Subject, bool>> predicate = sub =>
                sub.DataStatusId == 1 &&
                ((schoolID == null || sub.SchoolId == schoolID) &&
                (departmentID == null || sub.DerpartmentId == departmentID) &&
                (semesterID == null || sub.SemesterId == semesterID) &&
                (key == null || sub.Name.Contains(key)));
           

            var listSub = _subjectRepository.GetMulti(predicate, includes).ToPagedList(pageNumber, pageSize);

            if (schoolID == null)
            {
                // Nếu chưa chọn trường thì lấy danh sách trường
                Schools = (List<School>)_schoolRepositoriy.GetAll();
            }
            else
            {
                SchoolID = schoolID;

                var c = _schoolRepositoriy.GetMulti(sch => sch.Id == (int)schoolID);
                Schools = (List<School>)c;

                if (departmentID == null)
                {
                    // Nếu đã chọn trường nhưng chưa chọn ngành thì lấy danh sách ngành của trường đó
                    Departments = (List<Department>)_departmentRepository.GetMulti(depart => depart.SchoolId == schoolID, includes2);
                    SemesterID = null;
                }
                else
                {
                    DepartmentID = departmentID;
                    var ok = _departmentRepository.GetSingleById((int)departmentID);
                    Departments = new List<Department>(); // Initialize the list
                    Departments.Add((Department)ok);

                    // Nếu đã chọn trường và ngành, thì lấy danh sách kỳ của ngành đó
                    Semesters = (List<Semester>)_semesterRepository.GetMulti(s => s.DepartmentId == departmentID, includes2);
                    SemesterID = semesterID;
                }
            }
            Key = key;
            Subjects = listSub;

            return Page();
        }

        public async void OnGetReport(string report)
        {
            Report rp = new Report
            {
                Contents = report
            };
            _context.Reports.Add(rp);
            _context.SaveChanges();
            await _hubContext.Clients.All.SendAsync("ReceiveReportNotification");
            // Do nothing
        }
    }
}
