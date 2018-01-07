using Group11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FriendRequestRepository : Repository<FriendRequest>
    {
        public FriendRequestRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
