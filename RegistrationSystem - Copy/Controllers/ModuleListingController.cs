using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;

namespace RegistrationSystem.Controllers
{
    public class ModuleListingController : Controller
    {
        private IModuleRepository repository;

        public ModuleListingController(IModuleRepository repo) => repository = repo;

        public ViewResult Listing(Models.Module mod) =>
            View("ModuleListing", repository.Modules.Where(f => f.Mod_Sys_Code == mod.Mod_Sys_Code));


        public ViewResult SearchModule(Models.Module mod)
        {
            ViewBag.ControllerName = "Module";
            return View(repository.Modules.Where(r => r.Mod_Code.Contains(mod.Mod_Code) && 
                                                      r.Mod_Sys_Code == mod.Mod_Sys_Code));
        }

        [HttpGet]
        public RedirectToActionResult BackToModule() =>
            RedirectToAction(nameof(ModuleController.Index), "Module");

    }
}
