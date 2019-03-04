using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationSystem.Models
{
    public class Version
    {
        [Key]
        [Required(ErrorMessage = "Please enter system code")]
        public string Ver_Sys_Code { get; set; }
        [Key]
        [Required(ErrorMessage = "Please enter version code")]
        public string Ver_Code { get; set; }
        [Required(ErrorMessage = "Please enter version description")]
        public string Ver_Description { get; set; }
        public DateTime Ver_CreatedDate { get; set; }
        public DateTime Ver_CreatedTime { get; set; }
    }
}
