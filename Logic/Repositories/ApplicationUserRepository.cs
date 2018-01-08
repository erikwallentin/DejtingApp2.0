using Group11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ApplicationUserRepository : Repository<ApplicationUser>
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
