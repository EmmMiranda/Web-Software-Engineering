using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using RegistrationSystem.Controllers;

namespace RegistrationSystem.Components
{
    public class SelectedModules : ViewComponent {
        private IModuleRegistrationRepository repository;
        private IModuleRepository mod_repo;

        public SelectedModules(IModuleRegistrationRepository repo, IModuleRepository mrepo)
        { 
            repository = repo;
            mod_repo = mrepo;
        }

        public IViewComponentResult Invoke(SystemRegistration sr)
        {
            if (sr == null)
                return View();

            var module_regs = repository.ModuleRegistrations.Where(
                r => r.Mor_Cst_Code == sr.Syr_Cst_Code &&
                     r.Mor_Sys_Code == sr.Syr_Sys_Code &&
                     r.Mor_Ver_Code == sr.Syr_Ver_Code);

            List<Module> modules = new List<Module>();
            foreach(var r in module_regs)
                modules.Add(mod_repo.Modules.FirstOrDefault(m => m.Mod_Code == r.Mor_Mod_Code &&
                                                                 m.Mod_Sys_Code == r.Mor_Sys_Code));

            return View(modules);
        }
    }
}
