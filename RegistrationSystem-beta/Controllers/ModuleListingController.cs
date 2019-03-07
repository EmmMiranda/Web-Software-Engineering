using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace RegistrationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleListingController : Controller
    {
        private IModuleRepository repository;

        public ModuleListingController(IModuleRepository repo) => repository = repo;

        public ViewResult Listing(Models.Module mod)
        {
            if (!string.IsNullOrWhiteSpace(mod.Mod_Sys_Code) &&
                !string.IsNullOrWhiteSpace(mod.Mod_Code))
                return View("ModuleListing", repository.Modules.Where(f => f.Mod_Code == mod.Mod_Code &&
                                                                           f.Mod_Sys_Code == mod.Mod_Sys_Code));
                
            return View("ModuleListing", repository.Modules.Where(f => f.Mod_Sys_Code == mod.Mod_Sys_Code));
        }


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
