using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace RegistrationSystem.Models
{
    public class SystemRegistration
    {
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Please enter client code")]
        public string Syr_Cst_Code { get; set; }

        [Key, Column(Order = 1)]
        [Required(ErrorMessage = "Please enter system code")]
        public string Syr_Sys_Code { get; set; }

        [Key, Column(Order = 2)]
        [Required(ErrorMessage = "Please enter version code")]
        public string Syr_Ver_Code { get; set; }

        public string Syr_SerialNum { get; set; }

        public string Syr_CustomerKey { get; set; }

        public string Syr_UnlockKey { get; set; }

        public string Syr_ProdKey { get; set; }

        public DateTime Syr_CreatedDate { get; set; }

        public DateTime Syr_CreatedTime { get; set; }
    }

    public class SystemRegistrationKey
    {
        public string Cst_Code { get; set; }
        public string Sys_Code { get; set; }
        public string Ver_Code { get; set; }
    }

    public class ModuleSelectionList
    {

        public SystemRegistrationKey srk { get; set; }
        public IList<Module> module_list { get; set; }
        public IList<Boolean> selection_list { get; set; }

        public ModuleSelectionList()
        {
            srk = new SystemRegistrationKey();
            module_list = new List<Module>();
            selection_list = new List<Boolean>();
        }

        public ModuleSelectionList(IModuleRepository mrepo, SystemRegistration sreg,
                                  IModuleRegistrationRepository mrrepo) : this()
        {
            srk.Cst_Code = sreg.Syr_Cst_Code;
            srk.Sys_Code = sreg.Syr_Sys_Code;
            srk.Ver_Code = sreg.Syr_Ver_Code;

            foreach (var m in mrepo.Modules.Where(r => r.Mod_Sys_Code == sreg.Syr_Sys_Code))
            {
                module_list.Add(m);
                if (mrrepo.ModuleRegistrations.Any(r => r.Mor_Cst_Code == sreg.Syr_Cst_Code &&
                                                        r.Mor_Sys_Code == sreg.Syr_Sys_Code &&
                                                        r.Mor_Ver_Code == sreg.Syr_Ver_Code &&
                                                        r.Mor_Mod_Code == m.Mod_Code))
                    selection_list.Add(true);
                else
                    selection_list.Add(false);
            }
        }
    }

    public class EnhancementSelectionList
    {
        public EnhancementSelectionList()
        {
            srk = new SystemRegistrationKey();
            Mod_Code = string.Empty;
            enhancement_list = new List<Enhancement>();
            selection_list = new List<Boolean>();
        }

        public EnhancementSelectionList(IEnhancementRepository erepo, SystemRegistration sreg, string mod,
                                        IEnhancementRegistrationReposity errepo) : this()
        {
            srk.Cst_Code = sreg.Syr_Cst_Code;
            srk.Sys_Code = sreg.Syr_Sys_Code;
            srk.Ver_Code = sreg.Syr_Ver_Code;
            Mod_Code = mod;

            foreach (var m in erepo.Enhancements.Where(r => r.Enh_Sys_Code == sreg.Syr_Sys_Code &&
                                                            r.Enh_Mod_Code == mod))
            {
                enhancement_list.Add(m);
                if (errepo.EnhancementRegistrations.Any(r => r.Enr_Cst_Code == sreg.Syr_Cst_Code &&
                                                             r.Enr_Sys_Code == sreg.Syr_Sys_Code &&
                                                             r.Enr_Ver_Code == sreg.Syr_Ver_Code &&
                                                             r.Enr_Mod_Code == mod))
                    selection_list.Add(true);
                else
                    selection_list.Add(false);
            }
        }

        public SystemRegistrationKey srk { get; set; }
        public string Mod_Code { get; set; }
        public IList<Enhancement> enhancement_list { get; set; }
        public IList<Boolean> selection_list { get; set; }
    }

}