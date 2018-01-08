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

            var context = new ApplicationDbContext();

            var users = context.Users.ToList();

            var results = context.Users.Where(x => x.Nickname.Contains(searchString) && x.Searchable || x.UserName.Contains(searchString) && x.Searchable).ToList();
            return View(results);

        }



    }
}