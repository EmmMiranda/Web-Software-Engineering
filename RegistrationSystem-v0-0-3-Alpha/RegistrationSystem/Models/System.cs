using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationSystem.Models
{
    public class System
    {
        [Key]
        [Required(ErrorMessage = "Please enter system code")]
        public string Sys_Code { get; set; }
        [Required(ErrorMessage = "Please enter system name")]
        public string Sys_Name { get; set; }
        [Required(ErrorMessage = "Please enter system description name")]
        public string Sys_Description { get; set; }
        public DateTime Sys_CreatedDate { get; set; }
        public DateTime Sys_CreatedTime { get; set; }
    }
}
