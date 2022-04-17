using Microsoft.AspNetCore.Mvc;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;

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

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            TVShowDetailsViewModel show = new TVShowDetailsViewModel();
            try
            {
                show = await showService.GetTVShowDetails(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return View(show);
        }

        [HttpPost]
        public async Task<IActionResult> Details(TVShowDetailsViewModel model)
        {
            bool isSaved = false;

            string userId = await userService.GetUserId(User.Identity.Name);

            isSaved = await showService.RateShow(userId, model.Id, model.Rating);

            return RedirectToAction(nameof(LikedShows));
        }

        public async Task<IActionResult> LikedShows()
        {
            string userId = await userService.GetUserId(User.Identity.Name);
            var model = await showService.GetLikedShows(userId);

            return View(model);
        }
    }
}
