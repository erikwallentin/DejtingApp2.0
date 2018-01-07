using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group11.Models
{

    public class AcceptedFriendsViewModel
    {
        public AcceptedFriendsViewModel()
        {
            List<AcceptedFriend> listOfAcceptedFriends = new List<AcceptedFriend>();
        }

        public List<AcceptedFriend> listOfAcceptedFriends { get; set; }
    }


}