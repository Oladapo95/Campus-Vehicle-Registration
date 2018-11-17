using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace VechicleRegistration.Utils
{
    public static class Helper
    {
        static Random rand = new Random();
        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static string GenerateGUID(bool noHyphens)
        {
            Guid newguid = Guid.NewGuid();

            if (noHyphens)
                return newguid.ToString().Replace("-", "");

            return newguid.ToString();
        }

        public static string ObfuscateEmail(string email)
        {
            var displayCase = email;

            var partToBeObfuscated = Regex.Match(displayCase, @"[^@]*").Value;
            if (partToBeObfuscated.Length - 3 > 0)
            {
                var obfuscation = "";
                for (var i = 0; i < partToBeObfuscated.Length - 3; i++) obfuscation += "*";
                displayCase = String.Format("{0}{1}{2}{3}", displayCase[0], displayCase[1], obfuscation, displayCase.Substring(partToBeObfuscated.Length - 1));
            }
            else if (partToBeObfuscated.Length - 3 == 0)
            {
                displayCase = String.Format("{0}*{1}", displayCase[0], displayCase.Substring(2));
            }

            return displayCase;
        }

        public static string CreateEmailFromTemplate(string userName, string title, string code, string templateSrc)
        {
            string body = string.Empty;

            //using streamreader for reading html template
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateSrc)))//"~/HtmlTemplate.html"
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{USERNAME}", userName); //replacing the required things  
            body = body.Replace("{HEADER}", title);
            body = body.Replace("{CODE}", code);

            return body;
        }

        public static string CreateSlipEmailFromTemplate(string name, string matricnumber,string regID, string department, string platenumber, string make, string model, string color )
        {
            string body = string.Empty;

            //using streamreader for reading html template
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/TemporarySlipTemplate.html")))//"~/HtmlTemplate.html"
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{NAME}", name); //replacing the required things  
            body = body.Replace("{MATRICNO}", matricnumber);
            body = body.Replace("{RegistrationID}", regID);
            body = body.Replace("{Department}", department);
            body = body.Replace("{VehiclePlateNo}", platenumber);
            body = body.Replace("{VehicleMake}", make);
            body = body.Replace("{VehicleModel}", model);
            body = body.Replace("{VehicleColor}", color);

            return body;
        }

        public static string GenerateCode()
        {
            StringBuilder six = new StringBuilder(6);
            int number;
            for (int i = 1; i <= 6; i++)
            {
                number = rand.Next(0, 9);
                six.Append(number.ToString());
            }
            return six.ToString();
        }


    }

} 