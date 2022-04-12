using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;

namespace MyMovieList.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;
        private readonly IUserService userService;

        public MovieController(IMovieService movieService, IUserService userService)
        {
            this.movieService = movieService;
            this.userService = userService;
        }
        public async Task<IActionResult> AllMovies()
        {
            var model = await movieService.GetAllMovies();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var movie = await movieService.GetMovieDetails(id);

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Details(MovieDetailsViewModel model)
        {
            bool isSaved = false;

            string userId = await userService.GetUserId(User.Identity.Name);

            isSaved = await movieService.RateMovie(userId, model.Id, model.Rating);

            return RedirectToAction(nameof(LikedMovies));
        }

        public async Task<IActionResult> LikedMovies()
        {
            string userId = await userService.GetUserId(User.Identity.Name);
            var model = await movieService.GetLikedMovies(userId);

            return View(model);
        }

    }
}
