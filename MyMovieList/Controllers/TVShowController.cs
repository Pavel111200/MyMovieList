using Microsoft.AspNetCore.Mvc;
using MyMovieList.Core.Contracts;

namespace MyMovieList.Controllers
{
    public class TVShowController : BaseController
    {
        private readonly ITVShowService showService;
        private readonly IUserService userService;

        public TVShowController(ITVShowService showService, IUserService userService)
        {
            this.showService = showService;
            this.userService = userService;
        }
        public async Task<IActionResult> AllShows()
        {
            var model = await showService.GetAllTVShows();

            return View(model);
        }
    }
}
