using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VechicleRegistration.Models;

namespace VechicleRegistration.Context
{
    public class VRContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<QRCode> QRCodes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}