using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;

namespace RegistrationSystem.Controllers
{
    public class EnhancementController : Controller
    {
        private IEnhancementRepository repository;
        private ISystemRepository sys_repository;
        private IModuleRepository mod_repository;

        public EnhancementController(IEnhancementRepository repo,
                                     ISystemRepository sys_repo,
                                     IModuleRepository mod_repo) {
            repository = repo;
            sys_repository = sys_repo;
            mod_repository = mod_repo;
        }

        public IActionResult Index(Enhancement enh)
        {
            ViewBag.Sys_Code = enh.Enh_Sys_Code;
            ViewBag.Mod_Code = enh.Enh_Mod_Code;
            return View(repository.Enhancements.Where(r => r.Enh_Sys_Code == enh.Enh_Sys_Code &&
                                                           r.Enh_Mod_Code == enh.Enh_Mod_Code));
        }

        public IActionResult Enhancement(string Enh_Sys_Code, string Enh_Mod_Code)
        {
            ViewBag.Sys_Code = Enh_Sys_Code;
            ViewBag.Mod_Code = Enh_Mod_Code;
            return View();
        }

        public IActionResult EnhancementForm(Enhancement enh)
        {
            if (ModelState.IsValid)
            {
                repository.SaveEnhancement(enh);
                ModelState.Clear();
            }
            return RedirectToAction(nameof(EnhancementController.Index), enh);
        }

        public IActionResult SearchSystem(string Enh_Sys_Code)
        {
            ViewBag.ControllerName = "Enhancement";
            return View("Search", sys_repository.Systems.Where(r => r.Sys_Code.Contains(Enh_Sys_Code)));
        }

        public RedirectToActionResult Select(Models.System sys) => 
            RedirectToAction(nameof(EnhancementController.Index), new Enhancement{ Enh_Sys_Code = sys.Sys_Code });

        public RedirectToActionResult SelectModule(Models.Module mod) =>
            RedirectToAction(nameof(EnhancementController.Index), new Enhancement { Enh_Mod_Code = mod.Mod_Code,
                                                                                    Enh_Sys_Code  = mod.Mod_Sys_Code });

        public IActionResult SearchModule(string Enh_Sys_Code, string Enh_Mod_Code)
        {
            ViewBag.ControllerName = "Enhancement";
            return View(mod_repository.Modules.Where(r => r.Mod_Sys_Code == Enh_Sys_Code &&
                                                   r.Mod_Code.Contains(Enh_Mod_Code)));
        }

        public IActionResult Delete(Enhancement enh)
        {
            if (ModelState.IsValid)
            {
                repository.DeleteEnhancement(enh.Enh_Sys_Code, 
                                             enh.Enh_Mod_Code,
                                             enh.Enh_Code);
            }

            return RedirectToAction(nameof(VersionController.Index), enh);
        }

        public IActionResult Edit(Enhancement enh) =>
            View(nameof(EnhancementController.Enhancement), enh);

        public IActionResult Save(Enhancement enh) => 
            RedirectToAction(nameof(EnhancementController.Index), enh);

        [HttpGet]
        public IActionResult GetFirst()
        {
            ModelState.Clear();
            return RedirectToAction(nameof(EnhancementController.Index), repository.MoveFirst());
        }

        [HttpGet]   
        public IActionResult GetPrev(Models.Enhancement enh)
        {
            ModelState.Clear();
            return RedirectToAction(nameof(ModuleController.Index), 
                                    repository.MovePrev(string.Empty, enh.Enh_Mod_Code, enh.Enh_Sys_Code));
        }

        [HttpGet]
        public IActionResult GetNext(Models.Enhancement enh)
        {
            ModelState.Clear();
            return RedirectToAction(nameof(EnhancementController.Index), 
                                    repository.MoveNext(enh.Enh_Sys_Code, enh.Enh_Mod_Code, string.Empty));
        }

        [HttpGet]
        public IActionResult GetLast()
        {
            ModelState.Clear();
            return RedirectToAction(nameof(EnhancementController.Index), repository.MoveLast());
        }

    }
}