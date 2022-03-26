using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyMovieList.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        
    }
}
