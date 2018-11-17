using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VechicleRegistration.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }

        public int StudentID { get; set; }
        public virtual Student Student { get; set; }

        public string Plate_number { get; set; }

        public string Chasis_number { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }



        public byte[] Drivers_License { get; set; }

        public byte[] vehicle_front { get; set; }

        public byte[] vehicle_back { get; set; }

        public byte[] vehicle_side { get; set; }

        public string Make { get; set; }

        public string Color { get; set; }

        public byte[] proof_of_ownership { get; set; }

        public DateTime date_created { get; set; }

        public DateTime last_modified { get; set; }
    }
}