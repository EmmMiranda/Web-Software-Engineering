using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationSystem.Models
{
    public class Customer
    {
        [Key]
        [Required(ErrorMessage = "Please enter customer code")]
        public string Cst_Code { get; set; }
        public string Cst_Name { get; set; }
        public string Cst_Description { get; set; }
        public string Cst_AdressLine1 { get; set; }
        public string Cst_AdressLine2 { get; set; }
        public string Cst_AdressLine3 { get; set; }
        public string Cst_City { get; set; }
        public string Cst_State { get; set; }
        public string Cst_ZipCode { get; set; }
        public string Cst_TelephoneNo { get; set; }
        public string Cst_EmailAddress { get; set; }
        public DateTime Cst_CreatedDate { get; set; }
        public DateTime Cst_CreatedTime { get; set; }
    }
}
