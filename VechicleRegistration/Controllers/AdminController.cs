using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VechicleRegistration.Context;
using VechicleRegistration.Models;

namespace VechicleRegistration.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        VRContext db = new VRContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            Admin admin = db.Admins.Where(m => m.Username == model.Username && m.Password == model.Password).FirstOrDefault();
            if (admin != null)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        //return RedirectToLocal(returnUrl);
                        return RedirectToAction("AdminPage");
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                } 
            }
            ModelState.AddModelError("", "The user doesnt exist please verify your crendentials"); 

            return View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("AdminPage", "Admin");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AdminPage()
        {
            //Pass All Department List Using ViewBag
            List<Vehicle> DeptList = db.Vehicles.Include("Student").ToList();
            ViewBag.ListOfDepartment = new SelectList(DeptList, "make", "color", "matricnumber");
            return View();
        }

        public JsonResult GetStudentList()
        {
            List<StudentViewModel> StuList = db.Vehicles.Select(x => new StudentViewModel
            {
                StudentId = x.StudentID,
                color = x.Color,
                make = x.Make,
                matricnumber = x.Student.MatricNumber
            }).ToList();

            return Json(StuList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentById(int? id)
        {
            if (id == null)
            {
                return Json("Error 404");
            }
            Vehicle vehicle = db.Vehicles.Include("Student").Where(x => x.StudentID == id).FirstOrDefault();
            if (vehicle.Student.QRCodeDetails.QRImage != null)
            {
                vehicle.Student.QrText = "data:image/png;base64," + Convert.ToBase64String(vehicle.Student.QRCodeDetails.QRImage, 0, vehicle.Student.QRCodeDetails.QRImage.Length, Base64FormattingOptions.None);
            }
            string value = string.Empty;
            value = JsonConvert.SerializeObject(vehicle, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int Id)
        {
            Vehicle frnds = new Vehicle();
            frnds = db.Vehicles.Include("Student").Where(x => x.VehicleID == Id).FirstOrDefault();
            return PartialView("_Details", frnds);
        }



        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}