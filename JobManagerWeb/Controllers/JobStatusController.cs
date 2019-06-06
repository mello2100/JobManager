using Microsoft.AspNetCore.Mvc;

namespace JobManagerWeb.Controllers
{
    public class JobStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}