using Group11.Models;
using Logic;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Group11.Controllers
{
    public class FriendsController : Controller
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        public ActionResult Index(string id)
        {
            var user = User.Identity.GetUserId();
            var userOneColumn = applicationDbContext.Friends.Include(x => x.User2).Where(x => x.User1.Id == user).ToList();
            var userTwoColumn = applicationDbContext.Friends.Include(x => x.User1).Where(x => x.User2.Id == user).ToList();
            var model = new AcceptedFriendsViewModel();


            foreach (var friend in userOneColumn)
            {
                AcceptedFriend friendItem = new AcceptedFriend();


                friendItem.id = friend.User2.Id;
                friendItem.name = friend.User2.Nickname;

                model.listOfAcceptedFriends.Add(friendItem);
            }

            foreach (var item in userTwoColumn)
            {
                AcceptedFriend friendItem = new AcceptedFriend();

                friendItem.id = item.User1.Id;
                friendItem.name = item.User1.Nickname;

                model.listOfAcceptedFriends.Add(friendItem);

            }

            return View(model);

            
        }
    }
}

        // GET: Friends
        //public ActionResult Index(string id)
        //{
        //    var user = User.Identity.GetUserId();
        //    List<Friend> usersFromFirstColumn = applicationDbContext.Friends.Include(x => x.User2).Where(x => x.User1.Id == user).ToList();
        //    List<Friend> usersFromSecondColumn = applicationDbContext.Friends.Include(x => x.User1).Where(x => x.User2.Id == user).ToList();
        //    AcceptedFriendsViewModel acceptedFriendsViewModel = new AcceptedFriendsViewModel();
        //    acceptedFriendsViewModel.listOfAcceptedFriends.Clear();
        //    foreach (Friend friend in usersFromFirstColumn)
        //    {
        //        if (id != user)
        //        {
        //            AcceptedFriend acceptedFriend = new AcceptedFriend();
        //            acceptedFriend.id = friend.User2.Id;
        //            acceptedFriend.name = friend.User2.Nickname;
        //            acceptedFriendsViewModel.listOfAcceptedFriends.Add(friend);
        //        }
        //    }

//    foreach (Friend friend in usersFromSecondColumn)
//    {
//        AcceptedFriend acceptedFriend = new AcceptedFriend();
//        acceptedFriend.id = friend.User1.Id;
//        acceptedFriend.name = friend.User1.Nickname;


//        if (!acceptedFriendsViewModel.listOfAcceptedFriends.Contains(friend))
//            acceptedFriendsViewModel.listOfAcceptedFriends.Add(friend);
//    }
//    return View(acceptedFriendsViewModel.listOfAcceptedFriends);
//}
//}

//}
