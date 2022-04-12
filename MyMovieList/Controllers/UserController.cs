using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;

namespace MyMovieList.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;

        public UserController(RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            this.roleManager = roleManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> ChangeName()
        //{
        //    string userId = await userService.GetUserId(User.Identity.Name);
        //    var model = await userService.GetUserForEdit(userId);

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> ChangeName(UserEditViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    if (await userService.UpdateUser(model))
        //    {
        //        ViewData["Succes"] = "Успешен запис!";
        //        return RedirectToAction("allmovies", "movie");
        //    }
        //    else
        //    {
        //        ViewData["Fail"] = "Възникна грешка!";
        //    }

        //    return View(model);
        //}
    }
}
