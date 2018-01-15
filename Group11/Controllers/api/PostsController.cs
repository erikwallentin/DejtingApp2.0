using Group11.Models;
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Group11.Controllers.api
{
    public class PostsController : ApiController
    {
        [HttpGet]
        public List<PostModel> List()
        {
            using (var db = new ApplicationDbContext())
            {
                var userName = User.Identity.Name;

                var list = new List<Posts>();
                var posts = db.Posts.ToList();
                foreach (var item in posts)
                {
                    list.Add(item);
                }
                return list.Select(post => new PostModel
                {
                    Id = post.Id,
                    Text = post.Text,
                    FromUser = post.FromUser.Nickname,
                    ToUser = post.ToUser.Id
                })
                    .ToList();
            }
        }

        [HttpPost]
        public void PostMessage(PostModel post)
        {

            using (var db = new ApplicationDbContext())
            {
                var from = db.Users.Single(u => u.Id == post.FromUser);
                var to = db.Users.Single(u => u.Id == post.ToUser);
                var posts = new Posts() { Text = post.Text, FromUser = from, ToUser = to };

                db.Posts.Add(posts);
                db.SaveChanges();
            }


        }
    }
}
