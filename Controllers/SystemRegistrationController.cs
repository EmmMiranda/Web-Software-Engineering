using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;

namespace RegistrationSystem.Controllers
{
    public class SystemRegistrationController : Controller
    {
 
        private ISystemRegistrationRepository sys_reg_repository;
        private IModuleRegistrationRepository mod_reg_repository;
        private IEnhancementRegistrationReposity enh_reg_repository;
        private ISystemRepository sys_repo;
        private IModuleRepository mod_repo;
        private IVersionRepository ver_repo;
        private IEnhancementRepository enh_repo;
        private ICustomerRepository cst_repo;
        private ModuleSelectionList module_selection_list;

        public SystemRegistrationController(ISystemRegistrationRepository srrepo,
                                            IModuleRegistrationRepository mrrepo,
                                            IEnhancementRegistrationReposity errepo,
                                            ISystemRepository srepo,
                                            IModuleRepository mrepo,
                                            IVersionRepository vrepo,
                                            ICustomerRepository crepo,
                                            IEnhancementRepository erepo)
        {
            sys_reg_repository = srrepo;
            mod_reg_repository = mrrepo;
            enh_reg_repository = errepo;
            sys_repo = srepo;
            mod_repo = mrepo;
            ver_repo = vrepo;
            cst_repo = crepo;
            enh_repo = erepo;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult SystemRegistrationForm(SystemRegistration sr)
        {
            if (ModelState.IsValid)
            {
                sys_reg_repository.AddSystemRegistration(sr);
                ModelState.Clear();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult SystemRegForm([FromBody]SystemRegistration sr)
            => RedirectToAction(nameof(SystemRegistrationForm), sr);

        [HttpPost]
        public RedirectToActionResult Delete(SystemRegistration sr)
        {
            if (ModelState.IsValid)
            {
                sys_reg_repository.DeleteSystemRegistration(sr.Syr_Cst_Code,
                                                            sr.Syr_Sys_Code,
                                                            sr.Syr_Ver_Code);
            }

            return RedirectToAction(nameof(ModuleController.Index));
        }

        public ViewResult SearchCustomer(SystemRegistration sr)
        {
            ViewBag.ControllerName = "SystemRegistration";
            if (string.IsNullOrWhiteSpace(sr.Syr_Cst_Code))
                return View(cst_repo.Customers);
            return View(cst_repo.Customers.Where(r => r.Cst_Code.Contains(sr.Syr_Cst_Code)));
        }

        public ViewResult SearchSystem(SystemRegistration sr)
        {
            ViewBag.ControllerName = "SystemRegistration";
            ViewBag.Cst_Code = sr.Syr_Cst_Code;
            return View("Search", sys_repo.Search(sr.Syr_Sys_Code));
        }

        public ViewResult SearchVersion(SystemRegistration sr)
        {
            ViewBag.ControllerName = "SystemRegistration";
            return View("SearchVersion", ver_repo.Search(sr.Syr_Sys_Code, sr.Syr_Ver_Code));
        }

        public IActionResult SelectCustomer(Customer cst)
        {
            HttpContext.Session.Set("Cst_Code", Encoding.ASCII.GetBytes(cst.Cst_Code));
            return View(nameof(Index), new SystemRegistration { Syr_Cst_Code = cst.Cst_Code });
        }

        public IActionResult Select(Models.System sys)
        {
            HttpContext.Session.Set("Sys_Code", Encoding.ASCII.GetBytes(sys.Sys_Code));
            HttpContext.Session.TryGetValue("Cst_Code", out var Cst_Code);
            return View(nameof(Index),
                        new SystemRegistration
                        {
                            Syr_Cst_Code = Encoding.ASCII.GetString(Cst_Code, 0, Cst_Code.Length),
                            Syr_Sys_Code = sys.Sys_Code
                        });
        }

        public IActionResult SelectVersion(Models.Version ver)
        {
            HttpContext.Session.Set("Ver_Code", Encoding.ASCII.GetBytes(ver.Ver_Code));
            HttpContext.Session.TryGetValue("Cst_Code", out var Cst_Code);
            HttpContext.Session.TryGetValue("Sys_Code", out var Sys_Code);
            HttpContext.Session.TryGetValue("Ver_Code", out var Ver_Code);
            return View(nameof(Index), 
                        new SystemRegistration
                        {
                            Syr_Cst_Code = Encoding.ASCII.GetString(Cst_Code, 0, Cst_Code.Length),
                            Syr_Sys_Code = Encoding.ASCII.GetString(Sys_Code, 0, Sys_Code.Length),
                            Syr_Ver_Code = Encoding.ASCII.GetString(Ver_Code, 0, Ver_Code.Length)
                        });
        }

        [HttpGet]
        public IActionResult GetFirst()
        {
            ModelState.Clear();
            return View(nameof(Index), sys_reg_repository.MoveFirst());
        }

        [HttpGet]
        public ViewResult GetPrev(SystemRegistration sr)
        {
            ModelState.Clear();
            return View(nameof(Index), sys_reg_repository.MovePrev(sr.Syr_Cst_Code,
                                                                   sr.Syr_Sys_Code,
                                                                   sr.Syr_Ver_Code));
        }

        [HttpGet]
        public ActionResult GetNext(SystemRegistration sr)
        {
            ModelState.Clear();
            return View(nameof(Index), sys_reg_repository.MoveNext(sr.Syr_Cst_Code,
                                                                   sr.Syr_Sys_Code,
                                                                   sr.Syr_Ver_Code));
        }

        [HttpGet]
        public ViewResult GetLast()
        {
            ModelState.Clear();
            return View(nameof(Index), sys_reg_repository.MoveLast());
        }

        public IActionResult SelectModules(SystemRegistration sr)
        {
            module_selection_list = new ModuleSelectionList(mod_repo, sr, mod_reg_repository);
            return View(module_selection_list);
        }

        [HttpPost]
        public IActionResult ModuleSelection(ModuleSelectionList msl)
        {
            for (int i = 0; i < msl.module_list.Count(); i++)
            {
                if (msl.selection_list[i] == true)
                {
                    mod_reg_repository.AddModuleRegistration(
                        new ModuleRegistration
                        {
                            Mor_Cst_Code = msl.srk.Cst_Code,
                            Mor_Sys_Code = msl.srk.Sys_Code,
                            Mor_Ver_Code = msl.srk.Ver_Code,
                            Mor_Mod_Code = msl.module_list[i].Mod_Code
                        });
                } else
                {
                    mod_reg_repository.DeleteModuleRegistration(msl.srk.Cst_Code,
                                                                msl.srk.Sys_Code,
                                                                msl.srk.Ver_Code,
                                                                msl.module_list[i].Mod_Code);
                }
            }
            return View(nameof(Index),
                        sys_reg_repository.SystemRegistrations.FirstOrDefault(r => r.Syr_Cst_Code == msl.srk.Cst_Code &&
                                                                                   r.Syr_Sys_Code == msl.srk.Sys_Code &&
                                                                                   r.Syr_Ver_Code == msl.srk.Ver_Code));
        }

        public ViewResult SearchModule(SystemRegistration sr, string Mod_Code)
        {
            List<Module> modules = new List<Module>();
            foreach (var mr in mod_reg_repository.ModuleRegistrations.Where(r => r.Mor_Cst_Code == sr.Syr_Cst_Code &&
                                                                                 r.Mor_Sys_Code == sr.Syr_Sys_Code &&
                                                                                 r.Mor_Ver_Code == sr.Syr_Ver_Code))
            {
                modules.Add(mod_repo.Modules.FirstOrDefault(r => r.Mod_Sys_Code == mr.Mor_Sys_Code &&
                                                                 r.Mod_Code == mr.Mor_Mod_Code));
            }

            ViewBag.ControllerName = "SystemRegistration";
            if (string.IsNullOrWhiteSpace(Mod_Code))
                return View("SearchModule", modules.Where(m => m.Mod_Sys_Code == sr.Syr_Sys_Code));
            return View("SearchModule", modules.Where(m => m.Mod_Sys_Code == sr.Syr_Sys_Code &&
                                                           m.Mod_Code == Mod_Code));
        }

        public IActionResult SelectModule(Models.Module mod)
        {
            HttpContext.Session.TryGetValue("Cst_Code", out var Cst_Code);
            HttpContext.Session.TryGetValue("Sys_Code", out var Sys_Code);
            HttpContext.Session.TryGetValue("Ver_Code", out var Ver_Code);
            ModelState.Clear();
            ViewBag.Mod_Code = mod.Mod_Code;
            return View(nameof(Index), 
                        sys_reg_repository.SystemRegistrations.FirstOrDefault(
                            r => r.Syr_Cst_Code == Encoding.ASCII.GetString(Cst_Code, 0, Cst_Code.Length) &&
                                 r.Syr_Sys_Code == Encoding.ASCII.GetString(Sys_Code, 0, Sys_Code.Length) &&
                                 r.Syr_Ver_Code == Encoding.ASCII.GetString(Ver_Code, 0, Ver_Code.Length)));
        }

        public IActionResult SelectEnhancements(SystemRegistration sr, string Mod_Code)
        {
            EnhancementSelectionList enhancement_selection_list =
                new EnhancementSelectionList(enh_repo, sr, Mod_Code, enh_reg_repository);
            return View(enhancement_selection_list);
        }

        [HttpPost]
        public IActionResult EnhancementSelection(EnhancementSelectionList esl)
        {
            for (int i = 0; i < esl.enhancement_list.Count(); i++)
            {
                if (esl.selection_list[i] == true)
                {
                    enh_reg_repository.AddEnhancementRegistration(
                        new EnhancementRegistration
                        {
                            Enr_Cst_Code = esl.srk.Cst_Code,
                            Enr_Sys_Code = esl.srk.Sys_Code,
                            Enr_Ver_Code = esl.srk.Ver_Code,
                            Enr_Mod_Code = esl.Mod_Code,
                            Enr_Enh_Code = esl.enhancement_list[i].Enh_Code
                        });
                }
                else
                {
                    enh_reg_repository.DeleteEnhancementRegistration(esl.srk.Cst_Code,
                                                                     esl.srk.Sys_Code,
                                                                     esl.srk.Ver_Code,
                                                                     esl.Mod_Code,
                                                                     esl.enhancement_list[i].Enh_Code);
                }
            }
            return View(nameof(Index),
                        sys_reg_repository.SystemRegistrations.FirstOrDefault(r => r.Syr_Cst_Code == esl.srk.Cst_Code &&
                                                                                   r.Syr_Sys_Code == esl.srk.Sys_Code &&
                                                                                   r.Syr_Ver_Code == esl.srk.Ver_Code));
        }

    }
}