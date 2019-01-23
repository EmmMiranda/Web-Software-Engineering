using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;

namespace RegistrationSystem.Controllers
{
    public class VersionController : Controller
    {
        private IVersionRepository repository;
        private ISystemRepository sys_repository;

        public VersionController(IVersionRepository repo, ISystemRepository sys_repo) {
            repository = repo;
            sys_repository = sys_repo;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public RedirectToActionResult VersionForm() => RedirectToAction("Index");

        [HttpPost]
        public ViewResult VersionForm(RegistrationSystem.Models.Version ver)
        {
            if (ModelState.IsValid)
            {
                repository.SaveVersion(ver);
                ModelState.Clear();
            }

            // if model state is not valid present error msg
            return View(nameof(VersionController.Index));
        }

        public ViewResult SystemSearch(Models.Version ver)
        {
            ViewBag.ControllerName = "Version";
            return View("Search", sys_repository.Systems.Where(r => r.Sys_Code.Contains(ver.Ver_Sys_Code)));
        }

        public ViewResult SearchVersion(Models.Version ver)
        {
            return View("SearchVersion", repository.Versions.Where(r => r.Ver_Sys_Code == ver.Ver_Sys_Code && 
                                                                        r.Ver_Code.Contains(ver.Ver_Code)));
        }
        
        public ViewResult Select(Models.System sys) => View(nameof(VersionController.Index),
                                                            new Models.Version {Ver_Sys_Code = sys.Sys_Code});

        public ViewResult SelectVersion(Models.Version ver) => View(nameof(VersionController.Index),
                                                                    new Models.Version { Ver_Sys_Code = ver.Ver_Sys_Code,
                                                                                         Ver_Code = ver.Ver_Code });

        public RedirectToActionResult Updates(Models.Version ver) =>
            RedirectToAction("Updates", "Updates", new Models.Version {
                                                   Ver_Sys_Code = ver.Ver_Sys_Code,
                                                   Ver_Code = ver.Ver_Code });

        [HttpPost]
        public RedirectToActionResult Delete(Models.Version ver)
        {
            if (ModelState.IsValid)
            {
                repository.DeleteVersion(ver.Ver_Sys_Code, ver.Ver_Code);
            }

            // ModelState.Clear();
            return RedirectToAction(nameof(VersionController.Index));
        }

        [HttpGet]
        public IActionResult GetFirst()
        {
            ModelState.Clear();
            return View(nameof(VersionController.Index), repository.MoveFirst());
        }

        [HttpGet]
        public ViewResult GetPrev(Models.Version ver)
        {
            ModelState.Clear();
            return View(nameof(VersionController.Index), repository.MovePrev(ver.Ver_Sys_Code, ver.Ver_Code));
        }
        
        [HttpGet]
        public ActionResult GetNext(Models.Version ver)
        {
            ModelState.Clear();
            return View(nameof(VersionController.Index), repository.MoveNext(ver.Ver_Sys_Code, ver.Ver_Code));
        }
        
        [HttpGet]
        public ViewResult GetLast()
        {
            ModelState.Clear();
            return View(nameof(VersionController.Index), repository.MoveLast());
        }

    }
}