using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;

namespace RegistrationSystem.Controllers
{
    public class UpdatesController : Controller
    {
        private IUpdatesRepository repository;

        public UpdatesController(IUpdatesRepository repo) {
            repository = repo;
        }

        //public IActionResult Updates() => View();

        public IActionResult Updates(Models.Version ver)
        {
            ViewBag.Sys_Code = ver.Ver_Sys_Code;
            ViewBag.Ver_Code = ver.Ver_Code;
            return View(repository.Updates.Where(r => r.Upd_Sys_Code == ver.Ver_Sys_Code &&
                                                 r.Upd_Ver_Code == ver.Ver_Code));
        }

        /*
        [HttpGet]
        public IActionResult UpdatesForm() => RedirectToAction(nameof(UpdatesController.Updates),
                                                               new Models.Version
                                                               {
                                                                   Ver_Sys_Code = version_key.Ver_Sys_Code,
                                                                   Ver_Code = version_key.Ver_Sys_Code
                                                               });
                                                               */
        [HttpPost]
        public IActionResult UpdatesForm(RegistrationSystem.Models.Updates upd)
        {
            if (ModelState.IsValid)
            {
                repository.SaveUpdates(upd);
                ModelState.Clear();
            }

            // if model state is not valid present error msg
            return RedirectToAction(nameof(UpdatesController.Updates),
                                    new Models.Version { Ver_Sys_Code = upd.Upd_Sys_Code,
                                                         Ver_Code = upd.Upd_Ver_Code });
        }

      //  [HttpPost]
        public RedirectToActionResult Erase(Models.Updates upd)
        {
            if (ModelState.IsValid)
            {
                repository.DeleteUpdates(upd.Upd_Sys_Code, upd.Upd_Ver_Code,
                                         upd.Upd_Code);
            }

            // ModelState.Clear();
            return RedirectToAction(nameof(UpdatesController.Updates),
                                    new Models.Version
                                    {
                                        Ver_Sys_Code = upd.Upd_Sys_Code,
                                        Ver_Code = upd.Upd_Ver_Code
                                    });
        }

        [HttpGet]
        public RedirectToActionResult Delete(Models.Updates upd)
        {
            return RedirectToAction(nameof(UpdatesController.Erase), upd);
        }

    }
}