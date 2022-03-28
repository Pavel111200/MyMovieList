using Microsoft.AspNetCore.Mvc;

namespace MyMovieList.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
