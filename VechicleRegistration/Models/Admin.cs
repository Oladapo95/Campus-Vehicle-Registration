using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VechicleRegistration.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }


    }
}