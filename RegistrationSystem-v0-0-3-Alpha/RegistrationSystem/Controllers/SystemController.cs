using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using System;

namespace RegistrationSystem.Controllers
{
    public class SystemController : Controller
    {
        private ISystemRepository repository;

        public SystemController(ISystemRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View("System");

        [HttpGet]
        public ViewResult SystemForm()
            => View("System");

        [HttpPost]
        public ViewResult SystemForm(RegistrationSystem.Models.System sys)
        {
            if (ModelState.IsValid)
            {
                sys.Sys_CreatedDate = DateTime.Now;
                sys.Sys_CreatedTime = DateTime.Now;
                repository.AddSystem(sys);
                return View("System");
            }
            else
            {
                //there is a validation error
                return View("System");
            }


        }

        public ViewResult Select(Models.System sys) => View("System");

        [HttpPost]
        public ViewResult Delete(Models.System sys)
        {
            if (ModelState.IsValid)
            {
                repository.DeleteSystem(sys.Sys_Code);
            }

            ModelState.Clear();
            return View("System");
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
