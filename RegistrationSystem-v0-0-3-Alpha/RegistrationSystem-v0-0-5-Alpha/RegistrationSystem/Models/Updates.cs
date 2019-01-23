using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationSystem.Models
{
    public class Updates
    {
        [Key]
        [Required(ErrorMessage = "Please enter system code")]
        public string Upd_Sys_Code { get; set; }
        [Key]
        [Required(ErrorMessage = "Please enter version code")]
        public string Upd_Ver_Code { get; set; }
        [Key]
        [Required(ErrorMessage = "Please enter update code")]
        public string Upd_Code { get; set; }
        [Required(ErrorMessage = "Please enter udpate description")]
        public string Upd_Description { get; set; }
        public DateTime Upd_CreatedDate { get; set; }
        public DateTime Upd_CreatedTime { get; set; }
    }
}
