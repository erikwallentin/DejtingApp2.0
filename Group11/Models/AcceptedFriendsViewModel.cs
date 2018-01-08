using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group11.Models
{

    public class AcceptedFriendsViewModel
    {
        public string Nickname { get; set; }
        public List<ApplicationUser> ListOfAcceptedFriends { get; set; }

        public AcceptedFriendsViewModel()
        {
            List<ApplicationUser> listOfAcceptedFriends = new List<ApplicationUser>();
        }

        
    }


}