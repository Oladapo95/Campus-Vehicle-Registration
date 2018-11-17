using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VechicleRegistration.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        public string name { get; set; }

        public int MatricNumber { get; set; }

        public byte[] StudentImage { get; set; }
        public string ImageText { get; set; }
        public string QrText { get; set; }


        public string Department { get; set; }

        public string Faculty { get; set; }

        public string Email { get; set; } 

        public DateTime date_created { get; set; }

        public DateTime last_modified { get; set; }

        public virtual List<Vehicle> Vehicles { get; set; }

        public virtual QRCode QRCodeDetails { get; set; }

    }
}