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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            EditTVShowViewModel show = new EditTVShowViewModel();
            try
            {
                show = await showService.GetTVShowForEdit(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return View(show);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTVShowViewModel model)
        {
            bool isSaved = false;

            if (ModelState.IsValid)
            {
                isSaved = await showService.UpdateTVShow(model);
            }

            if (isSaved)
            {
                return RedirectToAction(nameof(AllTVShows));
            }
            return View(model);
        }
    }
}
