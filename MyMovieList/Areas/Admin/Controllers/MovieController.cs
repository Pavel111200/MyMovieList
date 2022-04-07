using Microsoft.AspNetCore.Mvc;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;

namespace MyMovieList.Areas.Admin.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllMovies()
        {
            var model = await movieService.GetAllMovies();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var movie = await movieService.GetMovieDetails(id);

            return View(movie);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMovieViewModel model)
        {
            bool isSaved = false;
            if (ModelState.IsValid)
            {
                isSaved = await movieService.AddMovie(model);
            }

            if (isSaved)
            {
                return RedirectToAction(nameof(AllMovies));
            }
            return View(model);
        }
    }
}
