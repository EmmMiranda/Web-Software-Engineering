using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationSystem.Models
{
    public class Enhancement
    {
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Please enter system code")]
        public string Enh_Sys_Code { get; set; }

        [Key, Column(Order = 2)]
        [Required(ErrorMessage = "Please enter module code")]
        public string Enh_Mod_Code { get; set; }

        [Key, Column(Order = 3)]
        [Required(ErrorMessage = "Please enter enhancement code")]
        public string Enh_Code { get; set; }

        [Required(ErrorMessage = "Please enter enhancement name")]
        public string Enh_Name { get; set; }

        [Required(ErrorMessage = "Please enter description name")]
        public string Enh_Description { get; set; }

        [Required(ErrorMessage = "Please enter enhancement version name")]

        public string Enh_Version { get; set; }

        public DateTime Enh_CreatedDate { get; set; }
        public DateTime Enh_CreatedTime { get; set; }
    }
}
