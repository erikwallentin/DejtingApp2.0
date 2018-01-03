using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Group11.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

       
        public DbSet<Messages> Messages { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}