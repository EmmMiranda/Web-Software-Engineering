﻿using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace RegistrationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleController : Controller
    {
        private IModuleRepository repository;
        private ISystemRepository sys_repository;

        public ModuleController(IModuleRepository repo, ISystemRepository sys_repo) {
            repository = repo;
            sys_repository = sys_repo;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public RedirectToActionResult ModuleForm() => RedirectToAction("Index");

        [HttpPost]
        public ViewResult ModuleForm(RegistrationSystem.Models.Module mod)
        {
            if (ModelState.IsValid)
            {
                repository.SaveModule(mod);
                ModelState.Clear();
            }

            // if model state is not valid present error msg
            return View(nameof(ModuleController.Index));
        }

        public ViewResult SearchSystem(Models.Module mod)
        {
            ViewBag.ControllerName = "Module";
            return View("Search", sys_repository.Search(mod.Mod_Sys_Code));
        }

        public ViewResult SearchModule(Models.Module mod) =>
            View("SearchModule", repository.Search(mod.Mod_Sys_Code, mod.Mod_Code));
        
        public ViewResult Select(Models.System sys) => View(nameof(ModuleController.Index),
                                                            new Models.Module {Mod_Sys_Code = sys.Sys_Code});

        public ViewResult SelectModule(Models.Module mod) => View(nameof(ModuleController.Index),
                                                                  new Models.Module { Mod_Code = mod.Mod_Code,
                                                                                      Mod_Sys_Code = mod.Mod_Sys_Code });

        [HttpPost]
        public RedirectToActionResult Delete(Models.Module mod)
        {
            if (ModelState.IsValid)
            {
                repository.DeleteModule(mod.Mod_Code, mod.Mod_Sys_Code);
            }

            // ModelState.Clear();
            return RedirectToAction(nameof(ModuleController.Index));
        }

        [HttpGet]
        public IActionResult GetFirst()
        {
            ModelState.Clear();
            return View(nameof(ModuleController.Index), repository.MoveFirst());
        }

        [HttpGet]
        public ViewResult GetPrev(Models.Module mod)
        {
            ModelState.Clear();
            return View(nameof(ModuleController.Index), repository.MovePrev(mod.Mod_Code, mod.Mod_Sys_Code));
        }
        
        [HttpGet]
        public ActionResult GetNext(Models.Module mod)
        {
            ModelState.Clear();
            return View(nameof(ModuleController.Index), repository.MoveNext(mod.Mod_Code, mod.Mod_Sys_Code));
        }
        
        [HttpGet]
        public ViewResult GetLast()
        {
            ModelState.Clear();
            return View(nameof(ModuleController.Index), repository.MoveLast());
        }

    }
}