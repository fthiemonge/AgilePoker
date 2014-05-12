using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgilePoker.Models;
using Newtonsoft.Json;

namespace AgilePoker.Controllers
{
    public class HomeController : Controller
    {
        #region Instance Methods

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreateRoom")]
        public ActionResult CreateRoom(UserRegistration model)
        {
            var existingRooms = GetCachedRoomNames().Select(x => x.ToLower());
            if (string.IsNullOrWhiteSpace(model.NewRoomName))
            {
                ModelState.AddModelError("NewRoomName", "Room Name required");
            }
            else if (existingRooms.Contains(model.NewRoomName.ToLower().Trim()))
            {
                ModelState.AddModelError("NewRoomName", "Room by this name already in use");
            }

            if (ModelState.IsValid)
            {
                SetUserPreferredNameInCookie(model.UserPreferredName);
                CreateRoom(model.NewRoomName, model.NewRoomDeck, model.UserPreferredName);
                return EnterRoom(model.NewRoomName);
            }

            model.UserPreferredName = GetUserPreferredNameFromCookie();
            model.ExistingRoomNames = GetCachedRoomNames();
            Session["CurrentRoomName"] = model.NewRoomName;
            return View("Index", model);
        }

        public ActionResult EnterRoom(string roomName)
        {
            Session["CurrentRoomName"] = roomName;
            var model = new Room
                {
                    PokerRoom = GetCachedRoom()
                };
            model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);
            return View("Room", model);
        }

        public ActionResult Index()
        {
            var model = new UserRegistration();
            model.UserPreferredName = GetUserPreferredNameFromCookie();
            model.ExistingRoomNames = GetCachedRoomNames();
            return View(model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "JoinRoom")]
        public ActionResult JoinRoom(UserRegistration model)
        {
            SetUserPreferredNameInCookie(model.UserPreferredName);
            AddUserToRoom(model.SelectedExistingRoomName, model.UserPreferredName);
            return EnterRoom(model.SelectedExistingRoomName);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ReVote")]
        public ActionResult ReVote(Room model)
        {
            // TODO: Handle if session expired, etc.
            var roomName = (string) Session["CurrentRoomName"];

            var agilePokerRooms = GetCachedRooms();
            foreach (var user in agilePokerRooms.First(x => x.Name == roomName).Users)
            {
                var roomIndex = agilePokerRooms.FindIndex(x => x.Name == roomName);
                var userIndex = agilePokerRooms[roomIndex].Users.FindIndex(x => x.UniqueName == user.UniqueName);
                var newUser = agilePokerRooms[roomIndex].Users[userIndex];
                newUser.SelectedCard = null;
            }

            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert("AgilePokerRooms", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
            DisplayVotes(false);

            model.PokerRoom = GetCachedRoom();
            model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);


            return View("Room", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Refresh")]
        public ActionResult Refresh(Room model)
        {
            // TODO: Handle if session expired, etc.
            model.PokerRoom = GetCachedRoom();
            model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);
            model.SelectedCard = model.PlayingCards.First(x => x.Value == model.SelectedCardValue);

            return View("Room", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SelectCard")]
        public ActionResult SelectCard(Room model)
        {
            // TODO: Handle if session expired, etc.
            model.PokerRoom = GetCachedRoom();
            model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);
            model.SelectedCard = model.PlayingCards.First(x => x.Value == model.SelectedCardValue);
            var user = model.PokerRoom.Users.First(x => x.UniqueName == User.Identity.Name);
            user.SelectedCard = model.SelectedCard;
            UpdateVote(model.PokerRoom.Name, user);
            return View("Room", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ShowVotes")]
        public ActionResult ShowVotes(Room model)
        {
            // TODO: Handle if session expired, etc.
            DisplayVotes(true);
            model.PokerRoom = GetCachedRoom();
            model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);
            var user = model.PokerRoom.Users.First(x => x.UniqueName == User.Identity.Name);
            model.SelectedCard = user.SelectedCard;

            return View("Room", model);
        }

        #endregion

        #region Private Instance Methods

        private void AddUserToRoom(string roomName, string preferredName)
        {
            var agilePokerRooms =
                JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache["AgilePokerRooms"].ToString());
            if (
                agilePokerRooms.First(x => x.Name == roomName)
                               .Users.FirstOrDefault(x => x.UniqueName == User.Identity.Name) == null)
            {
                agilePokerRooms.First(x => x.Name == roomName).Users.Add(new AgilePokerUser
                    {
                        PreferredName = preferredName,
                        UniqueName = User.Identity.Name
                    });
            }
            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert("AgilePokerRooms", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        private void CreateRoom(string roomName, Deck deck, string userPreferredName)
        {
            var agilePokerRooms = new List<AgilePokerRoom>();
            if (HttpRuntime.Cache["AgilePokerRooms"] != null)
            {
                agilePokerRooms =
                    JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache["AgilePokerRooms"].ToString());
            }
            agilePokerRooms.Add(
                new AgilePokerRoom
                    {
                        Deck = deck,
                        Name = roomName,
                        Users = new List<AgilePokerUser>
                            {
                                new AgilePokerUser
                                    {
                                        PreferredName = userPreferredName,
                                        UniqueName = User.Identity.Name
                                    }
                            },
                        AgileMasterUser = new AgilePokerUser
                            {
                                PreferredName = userPreferredName,
                                UniqueName = User.Identity.Name
                            }
                    });

            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert("AgilePokerRooms", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        private void DisplayVotes(bool displayVotes)
        {
            var roomName = (string) Session["CurrentRoomName"];
            var agilePokerRooms = GetCachedRooms();
            var roomIndex = agilePokerRooms.FindIndex(x => x.Name == roomName);
            agilePokerRooms[roomIndex].DisplayVotes = displayVotes;
            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert("AgilePokerRooms", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        private AgilePokerRoom GetCachedRoom()
        {
            var roomName = (string) Session["CurrentRoomName"];
            var rooms =
                JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache["AgilePokerRooms"].ToString());
            return rooms.First(x => x.Name == roomName);
        }

        private List<string> GetCachedRoomNames()
        {
            return GetCachedRooms().Select(x => x.Name).ToList();
        }

        private List<AgilePokerRoom> GetCachedRooms()
        {
            var roomNames = new List<AgilePokerRoom>();
            if (HttpRuntime.Cache["AgilePokerRooms"] != null)
            {
                return
                    JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache["AgilePokerRooms"].ToString());
            }
            return roomNames;
        }

        private string GetUserPreferredNameFromCookie()
        {
            var cookie = Request.Cookies["AgilePoker"];
            return cookie == null || cookie["PreferredName"] == null
                ? User.Identity.Name.Split('\\')[1]
                : cookie["PreferredName"];
        }

        private void SetUserPreferredNameInCookie(string preferredName)
        {
            if (preferredName == null)
            {
                throw new ArgumentNullException("preferredName");
            }

            var cookie = new HttpCookie("AgilePoker");
            cookie["PreferredName"] = preferredName;
            cookie.Expires = DateTime.Now.AddDays(60);
            HttpContext.Response.Cookies.Add(cookie);
        }

        private void UpdateVote(string roomName, AgilePokerUser user)
        {
            var agilePokerRooms =
                JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache["AgilePokerRooms"].ToString());
            if (
                agilePokerRooms.First(x => x.Name == roomName)
                               .Users.FirstOrDefault(x => x.UniqueName == User.Identity.Name) != null)
            {
                var roomIndex = agilePokerRooms.FindIndex(x => x.Name == roomName);
                var userIndex = agilePokerRooms[roomIndex].Users.FindIndex(x => x.UniqueName == user.UniqueName);

                agilePokerRooms[roomIndex].Users[userIndex] = user;
            }
            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert("AgilePokerRooms", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        #endregion
    }
}