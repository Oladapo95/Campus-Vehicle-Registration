using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VechicleRegistration.Models
{
    public class QRCode
    {
        [Key()]
        [ForeignKey("Student")]
        public int StudentID { get; set; }

        public byte[] QRImage { get; set; } 

        public DateTime date_created { get; set; }

        public DateTime last_modified { get; set; }

        public virtual Student Student { get; set; }
    }
}