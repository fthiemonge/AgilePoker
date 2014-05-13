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
            //Session[Constants.Session.CurrentRoomName] = model.NewRoomName;
            return View("Index", model);
        }

        private ActionResult EnterRoom(string roomName)
        {
            Session[Constants.Session.CurrentRoomName] = roomName;
            var agilePokerRoom = GetCachedRoom();
           
            var model = new Room
                {
                    SerializedAgilePokerRoom = JsonConvert.SerializeObject(GetCachedRoom()),
                    PlayingCards = AgilePokerCard.GetCards(agilePokerRoom.Deck),
                    RoomName = roomName
                };

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

        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "ReVote")]
        //public ActionResult ReVote(Room model)
        //{
        //    // TODO: Handle if session expired, etc.
        //    var roomName = (string) Session["CurrentRoomName"];

        //    var AgilePokerTables = GetCachedRooms();
        //    foreach (var user in AgilePokerTables.First(x => x.TableName == roomName).Players)
        //    {
        //        var roomIndex = AgilePokerTables.FindIndex(x => x.TableName == roomName);
        //        var userIndex = AgilePokerTables[roomIndex].Players.FindIndex(x => x.UniqueName == user.UniqueName);
        //        var newUser = AgilePokerTables[roomIndex].Players[userIndex];
        //        newUser.SelectedCard = null;
        //    }

        //    var serializedRooms = JsonConvert.SerializeObject(AgilePokerTables);
        //    HttpRuntime.Cache.Insert(Constants.Cache.AgilePokerRooms, serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        //    DisplayVotes(false);

        //    model.PokerRoom = GetCachedRoom();
        //    model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);


        //    return View("Room", model);
        //}

        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "Refresh")]
        //public ActionResult Refresh(Room model)
        //{
        //    // TODO: Handle if session expired, etc.
        //    model.PokerRoom = GetCachedRoom();
        //    model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);
        //    model.SelectedCard = model.PlayingCards.First(x => x.Value == model.SelectedCardValue);

        //    return View("Room", model);
        //}

        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "SelectCard")]
        //public ActionResult SelectCard(Room model)
        //{
        //    // TODO: Handle if session expired, etc.
        //    model.PokerRoom = GetCachedRoom();
        //    model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);
        //    model.SelectedCard = model.PlayingCards.First(x => x.Value == model.SelectedCardValue);
        //    var user = model.PokerRoom.Players.First(x => x.UniqueName == User.Identity.Name);
        //    user.SelectedCard = model.SelectedCard;
        //    UpdateVote(model.PokerRoom.TableName, user);
        //    return View("Room", model);
        //}

        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "ShowVotes")]
        //public ActionResult ShowVotes(Room model)
        //{
        //    // TODO: Handle if session expired, etc.
        //    DisplayVotes(true);
        //    model.PokerRoom = GetCachedRoom();
        //    model.PlayingCards = AgilePokerCard.GetCards(model.PokerRoom.Deck);
        //    var user = model.PokerRoom.Players.First(x => x.UniqueName == User.Identity.Name);
        //    model.SelectedCard = user.SelectedCard;

        //    return View("Room", model);
        //}

        #endregion

        #region Private Instance Methods

        private void AddUserToRoom(string roomName, string preferredUsername)
        {
            var agilePokerRooms =
                JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
            if (agilePokerRooms.First(x => x.RoomName == roomName).Votes.FirstOrDefault(x => x.User.UniqueName == User.Identity.Name) == null)
            {
                agilePokerRooms.First(x => x.RoomName == roomName).Votes.Add(
                                new AgilePokerVote
                                    {
                                        User = new AgilePokerUser {
                                            PreferredName = preferredUsername,
                                            UniqueName = User.Identity.Name
                                        },
                                        Card = null
                                    }
                            );
            }
            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert(Constants.Cache.AgilePokerRooms, serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        private void CreateRoom(string roomName, Deck deck, string preferredUsername)
        {
            var agilePokerRooms = new List<AgilePokerRoom>();
            if (HttpRuntime.Cache[Constants.Cache.AgilePokerRooms] != null)
            {
                agilePokerRooms =
                    JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
            }
            agilePokerRooms.Add(
                new AgilePokerRoom
                    {
                        Deck = deck,
                        RoomName = roomName,
                        Votes = new List<AgilePokerVote>
                            {
                                new AgilePokerVote
                                    {
                                        User = new AgilePokerUser {
                                            PreferredName = preferredUsername,
                                            UniqueName = User.Identity.Name
                                        },
                                        Card = null
                                    }
                            },
                        ShowVotes = false,
                        ScrumMaster = new AgilePokerUser
                        {
                            PreferredName = preferredUsername,
                            UniqueName = User.Identity.Name
                        }
                    });

            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert(Constants.Cache.AgilePokerRooms, serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        //private void ShowVotes(bool showVotes)
        //{
        //    var roomName = (string) Session["CurrentRoomName"];
        //    var agilePokerTables = GetCachedRooms();
        //    var roomIndex = agilePokerTables.FindIndex(x => x.RoomName == roomName);
        //    agilePokerTables[roomIndex].ShowVotes = showVotes;
        //    var serializedRooms = JsonConvert.SerializeObject(agilePokerTables);
        //    HttpRuntime.Cache.Insert(Constants.Cache.AgilePokerRooms, serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        //}

        private AgilePokerRoom GetCachedRoom()
        {
            var roomName = (string) Session[Constants.Session.CurrentRoomName];
            var rooms =
                JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
            return rooms.First(x => x.RoomName == roomName);
        }

        private List<string> GetCachedRoomNames()
        {
            return GetCachedRooms().Select(x => x.RoomName).ToList();
        }

        private List<AgilePokerRoom> GetCachedRooms()
        {
            var roomNames = new List<AgilePokerRoom>();
            if (HttpRuntime.Cache[Constants.Cache.AgilePokerRooms] != null)
            {
                return
                    JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
            }
            return roomNames;
        }

        private string GetUserPreferredNameFromCookie()
        {
            var cookie = Request.Cookies[Constants.Cookie.AgilePokerCookieName];
            return cookie == null || cookie[Constants.Cookie.PreferredUsername] == null
                ? User.Identity.Name.Split('\\')[1]
                : cookie[Constants.Cookie.PreferredUsername];
        }

        private void SetUserPreferredNameInCookie(string preferredUsername)
        {
            if (preferredUsername == null)
            {
                throw new ArgumentNullException("preferredUsername");
            }

            var cookie = new HttpCookie(Constants.Cookie.AgilePokerCookieName);
            cookie[Constants.Cookie.PreferredUsername] = preferredUsername;
            cookie.Expires = DateTime.Now.AddDays(60);
            HttpContext.Response.Cookies.Add(cookie);
        }

        //private void UpdateVote(string tableName, AgilePokerUser user)
        //{
        //    var agilePokerTables =
        //        JsonConvert.DeserializeObject<List<AgilePokerTable>>(HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
        //    if (
        //        agilePokerTables.First(x => x.TableName == tableName)
        //                       .Players.FirstOrDefault(x => x.UniqueName == User.Identity.Name) != null)
        //    {
        //        var roomIndex = agilePokerTables.FindIndex(x => x.TableName == tableName);
        //        var userIndex = agilePokerTables[roomIndex].Players.FindIndex(x => x.UniqueName == user.UniqueName);

        //        agilePokerTables[roomIndex].Players[userIndex] = user;

        //    }
        //    var serializedRooms = JsonConvert.SerializeObject(agilePokerTables);
        //    HttpRuntime.Cache.Insert(Constants.Cache.AgilePokerRooms, serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));

            
        //}

        #endregion
    }
}