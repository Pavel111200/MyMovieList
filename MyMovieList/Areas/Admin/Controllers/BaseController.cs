using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyMovieList.Areas.Admin.Controllers
{
    [Authorize(Roles ="Administrator")]
    [Area("Admin")]
    public class BaseController : Controller
    {
    }
}
