using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace ConnectEduV2.Areas.Admin.Pages.Edu
{
    public class ListSubjectsModel : PageModel
    {
       
        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository;

        public ListSubjectsModel(IUserRepository userRepository, ISubjectRepository subjectRepository)
        {
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
        }

        public string? SearchKeyword { get; set; }
        public IPagedList<Subject> Subjects { get; set; }

        public void OnGet(int pageNumber = 1, int pageSize = 5, string searchKeyword = "")
        {
            var includes = new string[] { "Semester", "School" , "Derpartment", "DataStatus" ,};
            var subjects = _subjectRepository.GetMulti(
                          subjects => string.IsNullOrEmpty(searchKeyword) 
                          || subjects.Name.Contains(searchKeyword) 
                          || subjects.School.Name.Contains(searchKeyword)
                          || subjects.Derpartment.Name.Contains(searchKeyword)
                          || subjects.Semester.Name.Contains(searchKeyword),
                         includes: includes).ToPagedList(pageNumber, pageSize);
            SearchKeyword = searchKeyword; // Lưu từ khóa tìm kiếm vào ViewBag
            Subjects = subjects;
        }
        public IActionResult OnPost(int pageNum, int id)
        {
            var subject = _subjectRepository.GetSingleById(id);
            if (subject != null)
            {
                subject.DataStatusId = subject.DataStatusId == 1 ? 2 : 1;
                _subjectRepository.Update(subject);
                _subjectRepository.SaveChanges();
            }
            return RedirectToPage("/Admin/Edu/ListSubjects", new { pageNumber = pageNum });
        }
    }
}
