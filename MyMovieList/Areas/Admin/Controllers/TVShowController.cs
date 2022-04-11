using Microsoft.AspNetCore.Mvc;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;

namespace MyMovieList.Areas.Admin.Controllers
{
    public class TVShowController : BaseController
    {
        private readonly ITVShowService showService;

        public TVShowController(ITVShowService showService)
        {
            this.showService = showService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTVShowViewModel model)
        {
            bool isSaved = false;
            if (ModelState.IsValid)
            {
                isSaved = await showService.AddShow(model);
            }

            if (isSaved)
            {
                return RedirectToAction(nameof(AllTVShows));
            }
            return View(model);
        }

        public async Task<IActionResult> AllTVShows()
        {
            var model = await showService.GetAllTVShows();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var show = await showService.GetTVShowDetails(id);

            return View(show);
        }
    }
}
