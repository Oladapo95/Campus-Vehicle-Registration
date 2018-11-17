using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using VechicleRegistration.Context;
using VechicleRegistration.Utils;
using System.Drawing;
using QRCoder;

namespace VechicleRegistration.Models
{
    public class VRDataContextInitializer : DropCreateDatabaseAlways<VRContext>
    {
        protected override void Seed(VechicleRegistration.Context.VRContext context)
        {
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
                name = "Iyanda Oladapo Taofeek",
                MatricNumber = 120001,
                Department = "Computer Science",
                Email = "otiyanda@student.lautech.edu.ng",
                StudentImage = Helper.imageToByteArray(profileimage),
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
                Plate_number = "LAG-00001",
                Drivers_License = Helper.imageToByteArray(driverslicense),
                vehicle_front = Helper.imageToByteArray(frontviewimage),
                vehicle_back = Helper.imageToByteArray(frontviewimage),
                vehicle_side = Helper.imageToByteArray(frontviewimage),
                Make = "Honda-CRV",
                Color = "YELLOW",
                date_created = DateTime.Now,
                last_modified = DateTime.Now,
                proof_of_ownership = Helper.imageToByteArray(proofofownership),
                Model = "2013",
                Chasis_number = "mcv33467ju",
                Year = "2018"
            };

            context.Vehicles.Add(Vehicle);
            context.Students.Add(student);
            context.QRCodes.Add(qrcode);

            context.SaveChanges();
        }
    }
}