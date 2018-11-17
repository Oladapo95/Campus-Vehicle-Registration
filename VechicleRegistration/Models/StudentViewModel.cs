using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VechicleRegistration.Models
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string make { get; set; }
        public string color { get; set; }
        public int matricnumber { get; set; }
    }
}