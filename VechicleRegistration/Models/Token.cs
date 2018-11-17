using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VechicleRegistration.Models
{
    public class Token
    {
        [Key]
        public int TokenID { get; set; }
 
        [Required]
        public string VerificationCode { get; set; }

        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }
    }
}