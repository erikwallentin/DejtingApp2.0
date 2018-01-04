namespace Logic.Migrations
{
    using Group11.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Group11.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("seedusers");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "Steve@Steve.com",
                    Email = "Steve@Steve.com",
                    PasswordHash = password,
                    Nickname = "Steve",
                    Information = "Hello! this is Steve. Contact me if you want to hang out sometime!",
                    UserPhoto = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Pictures\Steve.jpg")),
                    SecurityStamp = Guid.NewGuid().ToString()


                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "Paddington@Paddington.com",
                    Email = "Paddington@Paddington.com",
                    PasswordHash = password,
                    Nickname = "Paddington",
                    Information = "Paddington here! I live in London UK. Leave a message to me on my wall.",
                    UserPhoto = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Pictures\Paddington.jpg")),
                    SecurityStamp = Guid.NewGuid().ToString()


                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "Daisy@Daisy.com",
                    Email = "Daisy@Daisy.com",
                    PasswordHash = password,
                    Nickname = "Daisy",
                    Information = "A teddy bear that likes music, italian food and sunny days at the beach.",
                    UserPhoto = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Pictures\Daisy.jpg")),
                    SecurityStamp = Guid.NewGuid().ToString()


                });

        }
    }
}
