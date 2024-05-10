using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConnectEduV2.Areas.Admin.Pages.Edu
{
    public class CreateDepartmentModel : PageModel
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchoolRepositoriy _schoolRepositoriy;

        public CreateDepartmentModel(IDepartmentRepository departmentRepository, ISchoolRepositoriy schoolRepositoriy)
        {
            _departmentRepository = departmentRepository;
            _schoolRepositoriy = schoolRepositoriy;
        }

        public List<School> Schools { get; set; }
        public string? Message { get; set; }
        public IActionResult OnGet()
        {
            Schools = _schoolRepositoriy.GetAll().ToList();
            return Page();
        }
        public IActionResult OnPost(string? name, int? schoolId)
        {
           
            if (!string.IsNullOrEmpty(name) && schoolId != null)
            {
                Department department = new Department();
                department.Name = name;
                department.SchoolId = schoolId;
                department.DataStatus = 1;
                _departmentRepository.Add(department);
                _departmentRepository.SaveChanges();
                Message = "Add Success";
            }
            else
            {
                Message = "Please Enter Name and Choose School";
            }
            return OnGet();
        }
    }
}
