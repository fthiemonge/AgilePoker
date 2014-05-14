using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgilePoker.Models;
using Newtonsoft.Json;

namespace AgilePoker.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        #region Instance Methods

        public ActionResult Index(string roomName)
        {
            Session[Constants.Session.CurrentRoomName] = roomName;
            var agilePokerRoom = GetCachedRoom();

            if (agilePokerRoom == null ||
                agilePokerRoom.Votes.FirstOrDefault(x => x.User.UniqueName == User.Identity.Name) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new Room
                {
                    PlayingCards = AgilePokerCard.GetCards(agilePokerRoom.Deck),
                    RoomName = roomName
                };

            return View(model);
        }

        #endregion

        #region Private Instance Methods

        private AgilePokerRoom GetCachedRoom()
        {
            var roomName = (string) Session[Constants.Session.CurrentRoomName];
            var rooms =
                JsonConvert.DeserializeObject<List<AgilePokerRoom>>(
                    HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
            return rooms.FirstOrDefault(x => x.RoomName == roomName);
        }

        #endregion
    }
}