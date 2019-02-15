using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationSystem.Models
{
    public class ModuleRegistration
    {
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Please enter customer code")]
        public string Mor_Cst_Code { get; set; }

        [Key, Column(Order = 1)]
        [Required(ErrorMessage = "Please enter system code")]
        public string Mor_Sys_Code { get; set; }

        [Key, Column(Order = 2)]
        [Required(ErrorMessage = "Please enter version code")]
        public string Mor_Ver_Code { get; set; }
        
        [Key, Column(Order = 3)]
        [Required(ErrorMessage = "Please enter module code")]
        public string Mor_Mod_Code { get; set; }

        public DateTime Mor_CreatedDate { get; set; }

        public DateTime Mor_CreatedTime { get; set; }
    }
}
