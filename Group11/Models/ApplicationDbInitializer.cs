using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group11.Models
{
    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.Users.Add(new ApplicationUser { Nickname = "Flex", UserName = "flex@hotmail.com", Information = "I'm a sheep" });
            context.Users.Add(new ApplicationUser { Nickname = "mrDog", UserName = "dog@hotmail.com", Information = "I'm a dog" });
            context.Users.Add(new ApplicationUser { Nickname = "msCat", UserName = "cat@hotmail.com", Information = "I'm a cat" });
            context.Messages.Add(new Messages { Text = "Hello there, handsome", User = ""})

            base.Seed(context);

        }
    }
}