using Microsoft.AspNetCore.Mvc;

namespace OneBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
   
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MailBox()
        {
            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult InVoice()
        {
            return View();
        }

        public IActionResult Table()
        {
            return View();
        }

        public IActionResult Forms()
        {
            return View();
        }
    }
}
        

