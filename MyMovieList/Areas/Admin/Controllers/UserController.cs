using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyMovieList.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "User"
            });

            return Ok();
        }
    }
}
