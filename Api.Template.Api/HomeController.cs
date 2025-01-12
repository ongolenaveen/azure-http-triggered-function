using Microsoft.AspNetCore.Mvc;

namespace Api.Template.Api
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return new RedirectResult("/swagger");
        }
    }
}
