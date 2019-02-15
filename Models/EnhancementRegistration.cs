using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RegistrationSystem.Models
{
    public class EnhancementRegistration
    {
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Please enter customer code")]
        public string Enr_Cst_Code { get; set; }

        [Key, Column(Order = 1)]
        [Required(ErrorMessage = "Please enter system code")]
        public string Enr_Sys_Code { get; set; }

        [Key, Column(Order = 2)]
        [Required(ErrorMessage = "Please enter version code")]
        public string Enr_Ver_Code { get; set; }

        [Key, Column(Order = 3)]
        [Required(ErrorMessage = "Please enter module code")]
        public string Enr_Mod_Code { get; set; }

        [Key, Column(Order = 4)]
        [Required(ErrorMessage = "Please enter enhancement code")]
        public string Enr_Enh_Code { get; set; }

        public string Enr_Eng_Password { get; set; }
        public DateTime Enr_CreatedDate { get; set; }
        public DateTime Enr_CreatedTime { get; set; }
    }
}
