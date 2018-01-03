using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Group11.Models;
using System.Data.Entity;
using System.IO;

namespace Group11.Controllers
{
    

    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
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

        //
        // GET: /Account/UserProfile
        public ActionResult UserProfile(string id = null)
        {

            if (id == null)
            {
                var userId = User.Identity.GetUserId();
                var db = new ApplicationDbContext();
                var model = db.Users.Where(x => x.Id == userId);
                return View(model);
            }
            
            else
            {
                var db = new ApplicationDbContext();
                var model = db.Users.Where(x => x.Id == id);
                return View(model);
            }
        }




        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Exclude = "UserPhoto")]RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                // To convert the user uploaded Photo as Byte Array before save to DB
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Nickname = model.Nickname, Information = model.Information };
                //Here we pass the byte array to user context to store in db
                user.UserPhoto = imageData;

                bool hasAllZeroes = imageData.All(singleByte => singleByte == 0);



                if (hasAllZeroes == true)
                {
                    TempData["ImageFailMSG"] = "<script>alert('Please choose a profile picture');</script>";
                    return View(model);
                }


                var result = await UserManager.CreateAsync(user, model.Password);




                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("UserProfile", "Account");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Startpage", "Home");
        }

        public ActionResult OtherUser(string nick)
        {
            var context = new ApplicationDbContext();

            var searchuser = context.Users.Single(e => e.Nickname == nick);

            return View(searchuser);
        }

        //
        // GET: /Account/ChangeUserData

        public ActionResult ChangeUserData()
        {
            var db = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();

            ApplicationUser user = db.Users.FirstOrDefault(u => u.Id.Equals(userId));

            ChangeUserDataViewModel model = new ChangeUserDataViewModel();
            model.Email = user.UserName;
            model.Nickname = user.Nickname;
            model.Information = user.Information;

            return View(model);

        }

        //
        // POST: /Account/ChangeUserData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserData(ChangeUserDataViewModel userprofile)
        {
            if (ModelState.IsValid)
            {
                var db = new ApplicationDbContext();
                string userId = User.Identity.GetUserId();

                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id.Equals(userId));

                user.UserName = userprofile.Email;
                user.Email = user.UserName;
                user.Nickname = userprofile.Nickname;
                user.Information = userprofile.Information;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("UserProfile", "Account");

            }
            return View(userprofile);

        }

        //
        // GET: /Account/EditProfilePicture
        public ActionResult EditProfilePicture()
        {
            return View();
        }

        //
        // POST: /Account/EditProfilePicture
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfilePicture([Bind(Exclude = "UserPhoto")]EditProfilePictureViewModel model)
        {
            if (ModelState.IsValid)
            {
                // To convert the user uploaded Photo as Byte Array before save to DB
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }

                var db = new ApplicationDbContext();
                string userId = User.Identity.GetUserId();

                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id.Equals(userId));
                //Here we pass the byte array to user context to store in db
                user.UserPhoto = imageData;

                bool hasAllZeroes = imageData.All(singleByte => singleByte == 0);



                if (hasAllZeroes == true)
                {
                    TempData["ImageFailMSG"] = "<script>alert('Please choose a profile picture');</script>";
                    return View(model);
                }
                else

                user.UserPhoto = imageData;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserProfile", "Account");


            }

            // If we got this far, something failed, redisplay form

            return View(model);
        }

        //
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("UserProfile");
            }
            TempData["PasswordChangeFailMSG"] = "<script>alert('The password you entered does not match your old one. Please try again');</script>";
            //AddErrors(result);
            return View(model);
        }

        public FileContentResult UserPhotos(string id = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }
                // to get the user details to load user Image

                if (id == null)
                {
                    var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                    var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                    return new FileContentResult(userImage.UserPhoto, "image/jpeg");
                }
                else
                {
                    var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                    var userImage = bdUsers.Users.Where(x => x.Id == id).FirstOrDefault();

                    return new FileContentResult(userImage.UserPhoto, "image/jpeg");
                }
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }

        public FileContentResult OtherUserPhotos(string nick)
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }
                // to get the user details to load user Image
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Nickname == nick).FirstOrDefault();

                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }

        // A method that makes the users nickname display on the navbar when logged in

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                var context = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();

                if (!string.IsNullOrEmpty(userId))
                {
                    var user = context.Users.SingleOrDefault(u => u.Id == userId);
                    string Nick = user.Nickname;
                    ViewData.Add("Nick", Nick);
                }
            }
            base.OnActionExecuted(filterContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
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
            return RedirectToAction("UserProfile", "Account");
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