using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationSystem.Models
{
    public class Module
    {
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Please enter system code")]
        public string Mod_Sys_Code { get; set; }
        [Key, Column(Order = 1)]
        [Required(ErrorMessage = "Please enter module code")]
        public string Mod_Code { get; set; }
        [Required(ErrorMessage = "Please enter module name")]
        public string Mod_Name { get; set; }
        [Required(ErrorMessage = "Please enter module description name")]
        public string Mod_Description { get; set; }
        [Required(ErrorMessage = "Please enter version")]
        public string Mod_Version { get; set; }
        public DateTime Mod_CreatedDate { get; set; }
        public DateTime Mod_CreatedTime { get; set; }
    }
}
