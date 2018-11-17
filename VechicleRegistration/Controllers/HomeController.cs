using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VechicleRegistration.Context;
using VechicleRegistration.Models;
using VechicleRegistration.Utils;
using VechicleRegistration.Models.Repositories;

namespace VechicleRegistration.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        VRContext db = new VRContext();
        private static string email = "";
        private ApplicationUserManager _userManager;
        private static string profileimage ;
        private static int regnumber;

        //StudentRepository repository = new StudentRepository();

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(VerifyStudentshipViewModel VerifyStudentshipVM)
        //{
        //    TryValidateModel(VerifyStudentshipVM);
        //    if (ModelState.IsValid)
        //    {

        //    }
        //        return View(VerifyStudentshipVM);
        //}

        public async Task<JsonResult> GetStudentByMatricno(string matricno, string studentmail)
        {
            
            //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Students]");

            if (matricno == null)
            {
                return Json("Error 404");
            }
            string value = string.Empty;

            Vehicle vehicle = db.Vehicles.Include("Student").Where(x => x.Student.MatricNumber.ToString() == matricno).FirstOrDefault();

            //List<Student>  student = repository.GetAll();
            //Student student = repository.GetByMatricNo(int.Parse(matricno));

            if (vehicle != null)
            {
                email = vehicle.Student.Email;
                if (vehicle.Student.StudentImage != null)
                {
                    vehicle.Student.ImageText = "data:image/png;base64," + Convert.ToBase64String(vehicle.Student.StudentImage, 0, vehicle.Student.StudentImage.Length, Base64FormattingOptions.None);
                    profileimage = vehicle.Student.ImageText;
                    regnumber = vehicle.Student.StudentID;
                }

                value = JsonConvert.SerializeObject(vehicle, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


                //Generate new Token
                string token = Helper.GenerateCode();

                //Generate new token details
                Token code = new Token();
                code.EmailAddress = studentmail;
                code.VerificationCode = token;

                //Add token to database
                db.Tokens.Add(code);
                db.SaveChanges();

                //Send email now
                //await SendEmailTokenAsync(studentmail, token);
            }
             

            else
            {
                value = JsonConvert.SerializeObject(null, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetToken(string token, string studentmail, string matricno)
        {
            if (token == null)
            {
                return Json("Error 404");
            }
            string value = string.Empty;
            Token tokendetail = db.Tokens.Where(x => x.VerificationCode == token).FirstOrDefault();
            if (tokendetail != null)
            {
                if (tokendetail.EmailAddress == studentmail)
                {
                    Vehicle vehicle = db.Vehicles.Include("Student").Where(x => x.Student.MatricNumber.ToString() == matricno).FirstOrDefault();
                    if (vehicle.Student.StudentImage != null)
                    {
                        vehicle.Student.ImageText = "data:image/png;base64," + Convert.ToBase64String(vehicle.Student.StudentImage, 0, vehicle.Student.StudentImage.Length, Base64FormattingOptions.None);
                    }

                    value = JsonConvert.SerializeObject(vehicle, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                }
            }

            else
            {
                value = JsonConvert.SerializeObject(null, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetPlateById(string num)
        {
            if (num == null)
            {
                return Json("Error 404");
            }
            string value = string.Empty;
            Vehicle vehicle = db.Vehicles.Where(x => x.Plate_number == num).FirstOrDefault();
            if (vehicle != null)
            {
                value = JsonConvert.SerializeObject(vehicle, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            else {
                value = null;
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Success()
        {
            ViewBag.Message = email;
            return View();
        }

        public async Task<JsonResult> CreateTempSlip(string name, string matricnumber, string department, string platenumber, string make, string model, string color)
        {

            string value = string.Empty;
            var emailService = new EmailService();
            var identityMessage = new IdentityMessage();
            string uniqueRegID = string.Format("{0:D6}", regnumber);
            var bod = Helper.CreateSlipEmailFromTemplate(name, matricnumber, uniqueRegID, department, platenumber, make, model, color);
            identityMessage.Body = bod;

            identityMessage.Destination = email;
            identityMessage.Subject = "Lautech Student Vehicle Temporary Slip";

            //await emailService.SendAsync(identityMessage);
            RequestResponse response = new RequestResponse();
            response.StatusCode = "00";
            response.StatusMsg = "Successful";
            value = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        private async Task SendEmailTokenAsync(string email, string verificationcode)
        {
            var emailService = new EmailService();
            var identityMessage = new IdentityMessage();
            var bod = Helper.CreateEmailFromTemplate(email, "Verification Code", verificationcode, "~/Templates/ActivationConfirmationTemplate.html");
            identityMessage.Body = bod;

            identityMessage.Destination = email;
            identityMessage.Subject = "Lautech Student mail Confirmation Code";

            await emailService.SendAsync(identityMessage);
            return;
        }

        private async Task<string> SendTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);

            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);

            var body = Helper.CreateEmailFromTemplate(userID, "", callbackUrl, string.Format("~/Templates/ActivationConfirmationTemplate-{0}.html", Session["AppLanguage"]));

            //await UserManager.SendEmailAsync(userID, subject,
            //   "Please confirm your Ecobank account by clicking <a href=\"" + callbackUrl + "\">this link</a>");

            await UserManager.SendEmailAsync(userID, subject, body);

            return callbackUrl;
        }
    }
}