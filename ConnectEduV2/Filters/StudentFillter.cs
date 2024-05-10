using ConnectEduV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ConnectEduV2.Filters
{
    public class StudentFillter : IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userJson = context.HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
            {
                context.Result = new RedirectToPageResult("/Login/Login");
                return;
            }
            var user = JsonConvert.DeserializeObject<User>(userJson);
            if (user?.RoleId != 2)
            {
                context.Result = new RedirectToPageResult("/Login/Login");
                return;
            }
        }
    }

}
