using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace RegistrationSystem.Controllers
{
    [Authorize(Roles = "Admin, User, Viewer")]
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

        public IActionResult Index()
        {
            SetKey(string.Empty, string.Empty, string.Empty);
            return View();
        }

       [Authorize(Roles = "Admin, User")]
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

        private string GetCustomerName(string cst_code) => 
            cst_repo.Customers.FirstOrDefault(r => r.Cst_Code == cst_code)?.Cst_Name;

        public IActionResult SystemRegistrationForm(string cst_code, string sys_code, string ver_code, 
                                                    string mod_code, string cur_tab)
        {

            var model = sys_reg_repository.SystemRegistrations.FirstOrDefault(r => r.Syr_Cst_Code == cst_code &&
                                                                                   r.Syr_Sys_Code == sys_code &&
                                                                                   r.Syr_Ver_Code == ver_code);

            ViewBag.CstName = GetCustomerName(cst_code ?? string.Empty);

            if (model == null)
            {
                if ((!string.IsNullOrWhiteSpace(cst_code) &&
                     !string.IsNullOrWhiteSpace(sys_code) &&
                     !string.IsNullOrWhiteSpace(ver_code)) ||
                    (!string.IsNullOrWhiteSpace(cst_code) &&
                     !string.IsNullOrWhiteSpace(sys_code)) ||
                    (!string.IsNullOrWhiteSpace(cst_code)))

                {
                    return View(nameof(Index), new SystemRegistration
                    {
                        Syr_Cst_Code = cst_code,
                        Syr_Sys_Code = sys_code,
                        Syr_Ver_Code = ver_code
                    });
                }
            }
            else
            {
                ViewBag.CurrentTab = cur_tab;
                ViewBag.Mod_Code = mod_code;
                SetKey(model.Syr_Cst_Code, model.Syr_Sys_Code, model.Syr_Ver_Code);
            }

            return View(nameof(Index), model);
        }

        [HttpPost]
        public IActionResult SystemRegForm([FromBody]SystemRegistration sr)
            => RedirectToAction(nameof(SystemRegistrationForm), sr);

        public IActionResult SystemRegForm(SystemRegistration sr, string mod_code, string cur_tab)
        {
            SetKey(sr.Syr_Cst_Code, sr.Syr_Sys_Code, sr.Syr_Ver_Code);
            return RedirectToAction(nameof(SystemRegistrationForm), new { cst_code = sr.Syr_Cst_Code,
                                                                          sys_code = sr.Syr_Sys_Code,
                                                                          ver_code = sr.Syr_Ver_Code,
                                                                          mod_code,
                                                                          cur_tab });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public IActionResult Delete(SystemRegistration sr)
        {
            if (ModelState.IsValid)
            {
                mod_reg_repository.DeleteModuleRegistration(sr.Syr_Cst_Code, 
                                                            sr.Syr_Sys_Code, 
                                                            sr.Syr_Ver_Code, 
                                                            string.Empty);

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
            SetKeyField("Syr_Cst_Code", cst.Cst_Code);

            return RedirectToAction(nameof(SystemRegistrationForm), new
            {
                cst_code = GetKeyField("Syr_Cst_Code"),
                sys_code = string.Empty,
                ver_code = string.Empty
            });
        }

        public IActionResult Select(Models.System sys)
        {
            SetKeyField("Syr_Sys_Code", sys.Sys_Code);
            
            return RedirectToAction(nameof(SystemRegistrationForm), new
            {
                cst_code = GetKeyField("Syr_Cst_Code"),
                sys_code = GetKeyField("Syr_Sys_Code"),
                ver_code = string.Empty
            });
        }

        public IActionResult SelectVersion(Models.Version ver)
        {
            SetKeyField("Syr_Ver_Code", ver.Ver_Code);

            return RedirectToAction(nameof(SystemRegistrationForm), new
            {
                cst_code = GetKeyField("Syr_Cst_Code"),
                sys_code = GetKeyField("Syr_Sys_Code"),
                ver_code = GetKeyField("Syr_Ver_Code")
            });
        }

        [HttpGet]
        public IActionResult GetFirst()
        {
            ModelState.Clear();
            var model = sys_reg_repository.MoveFirst();

            if (model == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(SystemRegForm), model);
        }

        [HttpGet]
        public IActionResult GetPrev(SystemRegistration sr)
        {
            ModelState.Clear();
            var model = sys_reg_repository.MovePrev(sr.Syr_Cst_Code, sr.Syr_Sys_Code, sr.Syr_Ver_Code);

            if (model == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(SystemRegForm), model);
        }

        [HttpGet]
        public IActionResult GetNext(SystemRegistration sr)
        {
            ModelState.Clear();
            var model = sys_reg_repository.MoveNext(sr.Syr_Cst_Code, sr.Syr_Sys_Code, sr.Syr_Ver_Code);

            if (model == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(SystemRegForm), model);

        }

        [HttpGet]
        public IActionResult GetLast()
        {
            ModelState.Clear();
            var model = sys_reg_repository.MoveLast();

            if (model == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(SystemRegForm), model);
        }

        private void SetKeyField(string fieldName, string fieldValue) =>
            HttpContext.Session.Set(fieldName, Encoding.ASCII.GetBytes(fieldValue));

        private string GetKeyField(string fieldName)
        {
            HttpContext.Session.TryGetValue(fieldName, out var field);
            return Encoding.ASCII.GetString(field, 0, field.Length);
        }

        private void SetKey(string cst_code, string sys_code, string ver_code)
        {
            SetKeyField("Syr_Cst_Code", cst_code);
            SetKeyField("Syr_Sys_Code", sys_code);
            SetKeyField("Syr_Ver_Code", ver_code);
        }

        [Authorize(Roles = "Admin, User")]
        public IActionResult SelectModules(SystemRegistration sr)
        {
            var module_selection_list = new ModuleSelectionList(mod_repo, sr, mod_reg_repository);
            return View(module_selection_list);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public IActionResult ModuleSelection(ModuleSelectionList msl)
        {
            bool add_system_registration = true;

            for (int i = 0; i < msl.module_list.Count(); i++)
            {
                if (msl.selection_list[i] == true)
                {

                    if (add_system_registration)
                    {
                        if (!sys_reg_repository.SystemRegistrations.Any(r => r.Syr_Cst_Code == msl.srk.Syr_Cst_Code &&
                                                                             r.Syr_Sys_Code == msl.srk.Syr_Sys_Code &&
                                                                             r.Syr_Ver_Code == msl.srk.Syr_Ver_Code))
                        {
                            sys_reg_repository.AddSystemRegistration(msl.srk);
                            add_system_registration = false;

                        }
                    }

                    mod_reg_repository.AddModuleRegistration(
                        new ModuleRegistration
                        {
                            Mor_Cst_Code = msl.srk.Syr_Cst_Code,
                            Mor_Sys_Code = msl.srk.Syr_Sys_Code,
                            Mor_Ver_Code = msl.srk.Syr_Ver_Code,
                            Mor_Mod_Code = msl.module_list[i].Mod_Code
                        });
                } else
                {
                    mod_reg_repository.DeleteModuleRegistration(msl.srk.Syr_Cst_Code,
                                                                msl.srk.Syr_Sys_Code,
                                                                msl.srk.Syr_Ver_Code,
                                                                msl.module_list[i].Mod_Code);
                }
            }

            return RedirectToAction(nameof(SystemRegistrationForm), new
            {
                cst_code = msl.srk.Syr_Cst_Code,
                sys_code = msl.srk.Syr_Sys_Code,
                ver_code = msl.srk.Syr_Ver_Code,
                mod_code = string.Empty,
                cur_tab = "Module"
            });
        }

        public IActionResult ModuleCancel(ModuleSelectionList msl)
        {
            return RedirectToAction(nameof(SystemRegistrationForm), new
            {
                cst_code = msl.srk.Syr_Cst_Code,
                sys_code = msl.srk.Syr_Sys_Code,
                ver_code = msl.srk.Syr_Ver_Code,
                mod_code = string.Empty,
                cur_tab = "Module"
            });
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
            return View("SearchModule", modules.Where(
                m => m.Mod_Sys_Code == sr.Syr_Sys_Code && m.Mod_Code.StartsWith(Mod_Code)));
        }

        public IActionResult SelectModule(Models.Module mod)
        {
            return RedirectToAction(nameof(SystemRegistrationForm), new
            {
                cst_code = GetKeyField("Syr_Cst_Code"),
                sys_code = GetKeyField("Syr_Sys_Code"),
                ver_code = GetKeyField("Syr_Ver_Code"),
                mod_code = mod.Mod_Code,
                cur_tab = "Enhancement"
            });
        }

        [Authorize(Roles = "Admin, User")]
        public IActionResult SelectEnhancements(SystemRegistration sr, string Mod_Code)
        {
            EnhancementSelectionList enhancement_selection_list =
                new EnhancementSelectionList(enh_repo, sr, Mod_Code, enh_reg_repository);
            return View(enhancement_selection_list);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public IActionResult EnhancementSelection(EnhancementSelectionList esl)
        {

            bool add_system_registration = true;

            for (int i = 0; i < esl.enhancement_list.Count(); i++)
            {
                if (esl.selection_list[i] == true)
                {

                    if (add_system_registration)
                    {
                        if (!sys_reg_repository.SystemRegistrations.Any(r => r.Syr_Cst_Code == esl.srk.Syr_Cst_Code &&
                                                                             r.Syr_Sys_Code == esl.srk.Syr_Sys_Code &&
                                                                             r.Syr_Ver_Code == esl.srk.Syr_Ver_Code))
                        {
                            sys_reg_repository.AddSystemRegistration(esl.srk);
                            add_system_registration = false;

                        }
                    }

                    enh_reg_repository.AddEnhancementRegistration(
                        new EnhancementRegistration
                        {
                            Enr_Cst_Code = esl.srk.Syr_Cst_Code,
                            Enr_Sys_Code = esl.srk.Syr_Sys_Code,
                            Enr_Ver_Code = esl.srk.Syr_Ver_Code,
                            Enr_Mod_Code = esl.Mod_Code,
                            Enr_Enh_Code = esl.enhancement_list[i].Enh_Code
                        });
                }
                else
                {
                    enh_reg_repository.DeleteEnhancementRegistration(esl.srk.Syr_Cst_Code,
                                                                     esl.srk.Syr_Sys_Code,
                                                                     esl.srk.Syr_Ver_Code,
                                                                     esl.Mod_Code,
                                                                     esl.enhancement_list[i].Enh_Code);
                }
            }

            return RedirectToAction(nameof(SystemRegistrationForm), new
            {
                cst_code = esl.srk.Syr_Cst_Code,
                sys_code = esl.srk.Syr_Sys_Code,
                ver_code = esl.srk.Syr_Ver_Code,
                mod_code = esl.Mod_Code,
                cur_tab = "Enhancement"
            });

        }

        public IActionResult SearchLookup(string Syr_Cst_Code, string Syr_Sys_Code, string Syr_Ver_Code)
        {
            var field_names = new List<string> { "Customer Code", "System Code", "Version Code", "Name" };
            var model = new List<string[]>();

            var sys_reg = sys_reg_repository.Search(Syr_Cst_Code, Syr_Sys_Code, Syr_Ver_Code);
            foreach (var sr in sys_reg)
            {
                var cst_name =
                    cst_repo.Customers.FirstOrDefault(r => r.Cst_Code == sr.Syr_Cst_Code).Cst_Name;

                var field_values = new string[field_names.Count()];
                field_values[0] = sr.Syr_Cst_Code;
                field_values[1] = sr.Syr_Sys_Code;
                field_values[2] = sr.Syr_Ver_Code;
                field_values[3] = cst_name;

                model.Add(field_values);
            }

            ViewBag.FieldNames = field_names;
            ViewBag.ControllerName = nameof(SystemRegistration);
            return View(model);
        }

        public IActionResult SelectLookupOption(IEnumerable<string> field_values)
        {
            return RedirectToAction(nameof(SystemRegistrationForm), new
                                    {
                                        cst_code = field_values.ToList()[0],
                                        sys_code = field_values.ToList()[1],
                                        ver_code = field_values.ToList()[2],
                                        mod_code = string.Empty,
                                        cur_tab = string.Empty
                                    });
        }

    }
}