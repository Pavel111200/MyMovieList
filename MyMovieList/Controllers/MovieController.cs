using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;

namespace MyMovieList.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;
        private readonly UserManager<IdentityUser> userManager;

        public MovieController(IMovieService movieService, UserManager<IdentityUser> userManager)
        {
            this.movieService = movieService;
            this.userManager = userManager;
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

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = await userManager.GetUserIdAsync(user);

            isSaved = await movieService.RateMovie(userId, model.Id, model.Rating);

            return RedirectToAction(nameof(LikedMovies));
        }

        public async Task<IActionResult> LikedMovies()
        {
            return View();
        }
    }
}
