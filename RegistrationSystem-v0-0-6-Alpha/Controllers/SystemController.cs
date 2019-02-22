using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace RegistrationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemController : Controller
    {
        private ISystemRepository repository;

        public SystemController(ISystemRepository repo)
            => repository = repo;

        public ViewResult Index() => View("System");

        [HttpGet]
        public ViewResult SystemForm()
            => View("System");

        [HttpPost]
        public ViewResult SystemForm(RegistrationSystem.Models.System sys)
        {
            if (ModelState.IsValid)
            {
                repository.AddSystem(sys);
                ModelState.Clear();
            }

            // if model state is not valid present error msg
            return View("System");
        }

        public ViewResult Select(Models.System sys) => View("System");

        [HttpPost]
        public RedirectToActionResult Delete(Models.System sys)
        {
            if (ModelState.IsValid)
            {
                repository.DeleteSystem(sys.Sys_Code);
            }
                
           // ModelState.Clear();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ViewResult GetFirst()
        {
            ModelState.Clear();
            return View("System", repository.MoveFirst());
        }

        [HttpGet]
        public ViewResult GetPrev(Models.System sys)
        {
            ModelState.Clear();
            return View("System", repository.MovePrev(sys.Sys_Code));
        }

        [HttpGet]
        public ActionResult GetNext(Models.System sys)
        {
            ModelState.Clear();
            return View("System", repository.MoveNext(sys.Sys_Code));
        }

        [HttpGet]
        public ViewResult GetLast()
        {
            ModelState.Clear();
            return View("System", repository.MoveLast());
        }
    }
}
