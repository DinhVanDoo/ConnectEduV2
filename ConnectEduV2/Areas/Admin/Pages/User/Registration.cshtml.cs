using ConnectEduV2.Filters;
using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace ConnectEduV2.Areas.Admin.Pages.User
{
   
    public class RegistrationModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        
        public RegistrationModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public string SearchKeyword { get; set; }
        public string Success { get; set; }
        public IPagedList<ConnectEduV2.Models.User> Users { get; set; }
        public IActionResult OnGet(int pageNumber = 1, int pageSize = 5, string searchKeyword = "")
        {
            var includes = new string[] { "Status" };
            var user = _userRepository.GetMulti(
                user => (string.IsNullOrEmpty(searchKeyword) || user.Name.Contains(searchKeyword) || user.Email.Contains(searchKeyword))
                && user.StatusId == 1
                && user.RoleId == 2
                && user.ScoreboardPhoto != null,
                includes: includes
            ).ToPagedList(TempData["page"] == null ? pageNumber : (int)TempData["page"], pageSize);
           SearchKeyword = searchKeyword; // Lưu từ khóa tìm kiếm vào ViewBag
            if (TempData["Success"] != null)
            {
                Success = (string)TempData["Success"];
            }
            Users = user;
            return Page();
            
        }
        public IActionResult OnPost(int pageNumber, int id)
        {
            if(pageNumber == 0)
            {
                pageNumber = 1;
            }
            var user = _userRepository.GetSingleById(id);
            if (user != null)
            {
                user.RoleId = 3;
                _userRepository.Update(user);
                _userRepository.SaveChanges();
               
                TempData["Success"] = user.Email + " Become to Mentor";

                return RedirectToPage("/User/Registration", new {pageNumber = pageNumber});
            }
            else
            {
                return RedirectToPage("/User/Registration", new { pageNumber = pageNumber });
            }
        }

    }
}
