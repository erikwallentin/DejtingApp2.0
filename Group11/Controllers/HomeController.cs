using Group11.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group11.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult StartPage()
        {
            return View();
        }
        [Authorize]
        public ActionResult SearchPage(string searchString)
        {
           
            var db = new ApplicationDbContext();

            var users = db.Users.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Nickname.Contains(searchString)).ToList();
            }

            return View(users);
        }


    }
}