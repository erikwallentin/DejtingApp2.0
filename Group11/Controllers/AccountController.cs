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
using Logic;
using System.Collections.Generic;

namespace Group11.Controllers
{


    [Authorize]
    public class AccountController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: /Account/UserProfile
        public ActionResult UserProfile(string id = null)
        {

            if (id == null)
            {
                var userId = User.Identity.GetUserId();
                ApplicationUser user = context.Users.Single(e => e.Id == userId);
                return View(user);
            }

            else
            {
                var model = context.Users.Where(x => x.Id == id);
                return View(model);
            }
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

            var searchuser = context.Users.Single(e => e.Nickname == nick);

            return View(searchuser);
        }


        [Authorize]
        public ActionResult Friends(string id)
        {

            var user = User.Identity.GetUserId();
            List<Friend> usersFromUser1Column = context.Friends.Include(x => x.User2).Where(x => x.User1.Id == user).ToList();
            List<Friend> usersFromUser2Column = context.Friends.Include(x => x.User1).Where(x => x.User2.Id == user).ToList();

            List<ApplicationUser> listOfFriends = new List<ApplicationUser>();
            AcceptedFriendsViewModel acceptedFriendsViewModel = new AcceptedFriendsViewModel();

            foreach (Friend friend in usersFromUser1Column)
            {
                if (id != user)
                {

                    listOfFriends.Add(friend.User1);
                }
            }

            foreach (Friend friend in usersFromUser2Column)
            {

                if (!listOfFriends.Contains(friend.User2))
                    listOfFriends.Add(friend.User2);
            }

            ViewBag.List = listOfFriends;
            return View(listOfFriends);
        }

        [Authorize]
        public ActionResult SendFriendRequest(string id)
        {

            var userID = User.Identity.GetUserId();
            var receiverID = id;

            FriendRequestRepository friendRequestRepository = new FriendRequestRepository(context);

            ApplicationUserRepository applicationUserRepository = new ApplicationUserRepository(context);

            ApplicationUser sendingUser = applicationUserRepository.GetAll().Find(x => x.Id == userID);
            ApplicationUser recievingUser = applicationUserRepository.GetAll().Find(x => x.Id == receiverID);

            FriendRequest friendRequest = new FriendRequest
            {
                FriendSender = sendingUser,
                FriendReceiver = recievingUser
            };
            friendRequestRepository.Add(friendRequest);
            friendRequestRepository.Save();


            return RedirectToAction("SearchPage", "Home", new { SearchString = "" });
        }

        public ActionResult FriendRequest()
        {

            var user = User.Identity.GetUserId();
            List<FriendRequest> friendRequests = context.FriendRequests.Include(x => x.FriendSender).Where(x => x.FriendReceiver.Id == user).ToList();


            return View(friendRequests);

        }

        public bool FriendRequestCheck(string id, ApplicationDbContext context)
        {

            List<FriendRequest> friendRequests = context.FriendRequests.Where(x => x.FriendReceiver.Id == id).ToList();

            bool friendIsNew = false;

            if (friendRequests.Count > 0)
            {
                friendIsNew = true;
            }

            return friendIsNew;
        }

        //
        // GET: /Account/ChangeUserData

        public ActionResult Accept(string id)
        {

            String userID = User.Identity.GetUserId();
            String senderID = id;

            //Create new repositories
            FriendRequestRepository friendRequestRepository = new FriendRequestRepository(context);
            FriendRepository friendRepository = new FriendRepository(context);
            ApplicationUserRepository applicationUserRepository = new ApplicationUserRepository(context);

            ApplicationUser sendingUser = applicationUserRepository.GetAll().Find(x => x.Id == senderID);
            ApplicationUser recievingUser = applicationUserRepository.GetAll().Find(x => x.Id == userID);

            var friend = new Friend
            {
                User1 = recievingUser,
                User2 = sendingUser
            };

            friendRepository.Add(friend);
            friendRepository.Save();


            var removeList = context.FriendRequests.Where(x => (x.FriendReceiver.Id == userID && x.FriendSender.Id == senderID) ||
                (x.FriendReceiver.Id == senderID && x.FriendSender.Id == userID));

            foreach (var item in removeList)
            {
                friendRequestRepository.Items.Remove(item);
            }

            friendRequestRepository.Save();

            return RedirectToAction("FriendRequest");

        }

        public ActionResult DeclineFriendRequest(int id)
        {

            var friendrequestRepository = new FriendRequestRepository(context);
            var user = friendrequestRepository.GetAll().Find(x => x.Id == id);

            friendrequestRepository.Items.Remove(user);
            friendrequestRepository.Save();

            return RedirectToAction("FriendRequest");


        }
        public ActionResult ChangeUserData()
        {
            string userId = User.Identity.GetUserId();

            ApplicationUser user = context.Users.FirstOrDefault(u => u.Id.Equals(userId));

            ChangeUserDataViewModel model = new ChangeUserDataViewModel();
            model.Email = user.UserName;
            model.Nickname = user.Nickname;
            model.Information = user.Information;
            model.Searchable = user.Searchable;


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
                string userId = User.Identity.GetUserId();

                ApplicationUser user = context.Users.FirstOrDefault(u => u.Id.Equals(userId));

                user.UserName = userprofile.Email;
                user.Email = user.UserName;
                user.Nickname = userprofile.Nickname;
                user.Information = userprofile.Information;
                user.Searchable = userprofile.Searchable;

                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();

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

                string userId = User.Identity.GetUserId();

                ApplicationUser user = context.Users.FirstOrDefault(u => u.Id.Equals(userId));
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
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
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

        //A method that makes the users nickname display on the navbar when logged in

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
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