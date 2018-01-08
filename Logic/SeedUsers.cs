using Group11.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class SeedUsers : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("seedusers");
            var steve = new ApplicationUser {
                UserName = "Steve@Steve.com",
                Email = "Steve@Steve.com",
                PasswordHash = password,
                Nickname = "Steve",
                Information = "Hello! this is Steve. Contact me if you want to hang out sometime!",
                UserPhoto = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Pictures\Steve.jpg")),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var paddington = new ApplicationUser {
                UserName = "Paddington@Paddington.com",
                Email = "Paddington@Paddington.com",
                PasswordHash = password,
                Nickname = "Paddington",
                Information = "Paddington here! I live in London UK. Leave a message to me on my wall.",
                UserPhoto = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Pictures\Paddington.jpg")),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var daisy = new ApplicationUser {
                UserName = "Daisy@Daisy.com",
                Email = "Daisy@Daisy.com",
                PasswordHash = password,
                Nickname = "Daisy",
                Information = "A teddy bear that likes music, italian food and sunny days at the beach.",
                UserPhoto = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Pictures\Daisy.jpg")),
                SecurityStamp = Guid.NewGuid().ToString()

            };

            context.Users.Add(steve);
            context.Users.Add(paddington);
            context.Users.Add(daisy);
            

            
            base.Seed(context);
            context.SaveChanges();
        }
    }
}
