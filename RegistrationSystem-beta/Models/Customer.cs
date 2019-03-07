using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationSystem.Models
{
    public class Customer
    {
        [Key]
        [Required(ErrorMessage = "Please enter customer code")]
        public string Cst_Code { get; set; }
        [Required(ErrorMessage = "Please enter customer name")]
        public string Cst_Name { get; set; }
        [Required(ErrorMessage = "Please enter customer description")]
        public string Cst_Description { get; set; }
        [Required(ErrorMessage = "Please enter customer address 1")]
        public string Cst_AdressLine1 { get; set; }
        public string Cst_AdressLine2 { get; set; }
        public string Cst_AdressLine3 { get; set; }
        [Required(ErrorMessage = "Please enter customer city")]
        public string Cst_City { get; set; }
        [Required(ErrorMessage = "Please enter customer state")]
        public string Cst_State { get; set; }
        [Required(ErrorMessage = "Please enter customer zip code")]
        public string Cst_ZipCode { get; set; }
        [Required(ErrorMessage = "Please enter customer telephone")]
        public string Cst_TelephoneNo { get; set; }
        [Required(ErrorMessage = "Please enter customer email address")]
        public string Cst_EmailAddress { get; set; }
        public DateTime Cst_CreatedDate { get; set; }
        public DateTime Cst_CreatedTime { get; set; }
    }
}
