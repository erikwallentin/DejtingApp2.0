using Group11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FriendRepository : Repository<Friend>
    {
        public FriendRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
