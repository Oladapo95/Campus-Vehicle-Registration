using QRCoder;
namespace VechicleRegistration.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Drawing;
    using System.Linq;
    using Utils;

    internal sealed class Configuration : DbMigrationsConfiguration<VechicleRegistration.Context.VRContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VechicleRegistration.Context.VRContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            Image profileimage = Image.FromFile(@"D:\profile.jpg");
            Image frontviewimage = Image.FromFile(@"D:\frontview.jpg");
            Image driverslicense = Image.FromFile(@"D:\driverslicense.jpg");
            Image proofofownership = Image.FromFile(@"D:\proofofownership.jpg");



            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            Student student = new Student()
            {
                name = "Iyanda Oladapo Olalekan",
                MatricNumber = 120003,
                Department = "Mechanical Engineering",
                Email = "dappyfresh@gmail.com",
                StudentImage = Helper.imageToByteArray(profileimage),
                Faculty = "Faculty of Engineering",
                
                date_created = DateTime.Now,
                last_modified = DateTime.Now,

            };


            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(student.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);//Set higher to get larger image

            QRCode qrcode = new QRCode()
            {
                QRImage = Helper.imageToByteArray(qrCodeImage),
                date_created = DateTime.Now,
                last_modified = DateTime.Now,
                Student = student
            };



            Vehicle Vehicle = new Vehicle()
            {
                Student = student,
                Plate_number = "LAG-0003",
                Drivers_License = Helper.imageToByteArray(driverslicense),
                vehicle_front = Helper.imageToByteArray(frontviewimage),
                vehicle_back = Helper.imageToByteArray(frontviewimage),
                vehicle_side = Helper.imageToByteArray(frontviewimage),
                Make = "Toyota",
                Color = "Green",
                
                date_created = DateTime.Now,
                last_modified = DateTime.Now,
                proof_of_ownership = Helper.imageToByteArray(proofofownership),
                Model = "2017",
                Chasis_number = "MKV 3000 33A 8UJ",
                Year = "2017"
            };

            Models.Admin admin = new Models.Admin()
            {
                Username = "dappyfresh@gmail.com",
                Password = "@Nigeria95"
            };

            context.Vehicles.AddOrUpdate(Vehicle);
            context.Students.AddOrUpdate(student);
            context.QRCodes.AddOrUpdate(qrcode);
            context.Admins.AddOrUpdate(admin);
            context.SaveChanges();
        }
    }
}
