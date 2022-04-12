using Microsoft.AspNetCore.Mvc;
using MyMovieList.Core.Contracts;

namespace MyMovieList.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }
        public async Task<IActionResult> AllMovies()
        {
            var model = await movieService.GetAllMovies();

            return View(model);
        }
    }
}
