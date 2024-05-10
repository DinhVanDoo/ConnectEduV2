using ConnectEduV2.Models;
using ConnectEduV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConnectEduV2.Areas.Admin.Pages.Edu
{
    public class CreateSchoolModel : PageModel
    {
        private readonly ISchoolRepositoriy _schoolRepository;

        public CreateSchoolModel(ISchoolRepositoriy schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public string? Message { get; set; }
        public void OnPost(string? name, IFormFile? image)
        {
            if (!string.IsNullOrEmpty(name))
            {
                School school = new School();
                school.Name = name;
                school.DataStatusId = 1;
                if (image != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    var filePath = Path.Combine("wwwroot/img/Schools", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    school.Image = "/img/Schools/" + uniqueFileName;
                }
                _schoolRepository.Add(school);
                _schoolRepository.SaveChanges();
                Message = "Add success";
            }
            else
            {
                Message = "Please Enter Name";
            }
            
        }
    }
}
