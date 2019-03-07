using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using RegistrationSystem.Controllers;

namespace RegistrationSystem.Components
{
    public class SelectedEnhancements : ViewComponent {
        private IEnhancementRegistrationReposity repository;
        private IEnhancementRepository enh_repo;

        public SelectedEnhancements(IEnhancementRegistrationReposity repo, IEnhancementRepository erepo)
        { 
            repository = repo;
            enh_repo = erepo;
        }

        /*
        public IViewComponentResult Invoke(SystemRegistration sr)
        {
            if (sr == null)
                return View();

            var enhancement_regs = repository.EnhancementRegistrations.Where(
                r => r.Enr_Cst_Code == sr.Syr_Cst_Code &&
                     r.Enr_Sys_Code == sr.Syr_Sys_Code &&
                     r.Enr_Ver_Code == sr.Syr_Ver_Code);
                    // r.Enr_Mod_Code == module code ?????

            List<Enhancement> enhancements = new List<Enhancement>();
            foreach(var r in enhancement_regs)
                enhancements.Add(enh_repo.Enhancements.FirstOrDefault(e => e.Enh_Sys_Code == r.Enr_Sys_Code &&
                                                                           e.Enh_Mod_Code == r.Enr_Mod_Code &&
                                                                           e.Enh_Code == r.Enr_Enh_Code));

            return View(enhancements);
        }
        */

        public IViewComponentResult Invoke(EnhancementSelectionList sr)
        {
            if (sr == null)
                return View();

            var enhancement_regs = repository.EnhancementRegistrations.Where(
                r => r.Enr_Cst_Code == sr.srk.Syr_Cst_Code &&
                     r.Enr_Sys_Code == sr.srk.Syr_Sys_Code &&
                     r.Enr_Ver_Code == sr.srk.Syr_Ver_Code &&
                     r.Enr_Mod_Code == sr.Mod_Code);
            // r.Enr_Mod_Code == module code ?????

            List<Enhancement> enhancements = new List<Enhancement>();
            foreach (var r in enhancement_regs)
                enhancements.Add(enh_repo.Enhancements.FirstOrDefault(e => e.Enh_Sys_Code == r.Enr_Sys_Code &&
                                                                           e.Enh_Mod_Code == r.Enr_Mod_Code &&
                                                                           e.Enh_Code == r.Enr_Enh_Code));

            return View(enhancements);
        }

    }
}
