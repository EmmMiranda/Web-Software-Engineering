using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistrationSystem.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class HomeController : Controller
    {
        public ViewResult Index() {
            
            return View();
        }

        [HttpGet]
        public RedirectToActionResult SystemRegistration()
        {
            return RedirectToAction("Index", "System");
        }

        [HttpGet]
        public RedirectToActionResult ModuleSetup()
        {
            return RedirectToAction("Index", "Module");
        }

        [HttpGet]
        public RedirectToActionResult VersionSetup()
        {
            return RedirectToAction("Index", "Version");
        }

        [HttpGet]
        public RedirectToActionResult EnhancementSetup()
        {
            return RedirectToAction("Index", "Enhancement",
                new Models.Enhancement());
        }

        [HttpGet]
        public RedirectToActionResult CustomerSetup()
        {
            return RedirectToAction("Customer", "Customer");
        }

        [HttpGet]
        public RedirectToActionResult SysRegistration()
        {
            return RedirectToAction("Index", "SystemRegistration");
        }
    }
}
