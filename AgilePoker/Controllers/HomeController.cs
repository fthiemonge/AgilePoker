using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgilePoker.Hubs;
using AgilePoker.Models;
using Newtonsoft.Json;

namespace AgilePoker.Controllers
{
    public class HomeController : Controller
    {
        #region Instance Methods

        AgilePokerHub hub = new AgilePokerHub();

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
            var hands = model.PokerRoom.Players.Select(x => new AgilePokerHand()
                {
                    Hand = x.SelectedCard,
                    TableName = roomName,
                    UniqueName = x.UniqueName,
                    UserPreferredName = x.PreferredName
                });
            model.Hands = JsonConvert.SerializeObject(hands);
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

            var AgilePokerTables = GetCachedRooms();
            foreach (var user in AgilePokerTables.First(x => x.TableName == roomName).Players)
            {
                var roomIndex = AgilePokerTables.FindIndex(x => x.TableName == roomName);
                var userIndex = AgilePokerTables[roomIndex].Players.FindIndex(x => x.UniqueName == user.UniqueName);
                var newUser = AgilePokerTables[roomIndex].Players[userIndex];
                newUser.SelectedCard = null;
            }

            var serializedRooms = JsonConvert.SerializeObject(AgilePokerTables);
            HttpRuntime.Cache.Insert("AgilePokerTables", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
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
            var user = model.PokerRoom.Players.First(x => x.UniqueName == User.Identity.Name);
            user.SelectedCard = model.SelectedCard;
            UpdateVote(model.PokerRoom.TableName, user);
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
            var user = model.PokerRoom.Players.First(x => x.UniqueName == User.Identity.Name);
            model.SelectedCard = user.SelectedCard;

            return View("Room", model);
        }

        #endregion

        #region Private Instance Methods

        private void AddUserToRoom(string roomName, string preferredName)
        {
            var AgilePokerTables =
                JsonConvert.DeserializeObject<List<AgilePokerTable>>(HttpRuntime.Cache["AgilePokerTables"].ToString());
            if (
                AgilePokerTables.First(x => x.TableName == roomName)
                               .Players.FirstOrDefault(x => x.UniqueName == User.Identity.Name) == null)
            {
                AgilePokerTables.First(x => x.TableName == roomName).Players.Add(new AgilePokerPlayer
                    {
                        PreferredName = preferredName,
                        UniqueName = User.Identity.Name
                    });
            }
            var serializedRooms = JsonConvert.SerializeObject(AgilePokerTables);
            HttpRuntime.Cache.Insert("AgilePokerTables", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        private void CreateRoom(string roomName, Deck deck, string userPreferredName)
        {
            var AgilePokerTables = new List<AgilePokerTable>();
            if (HttpRuntime.Cache["AgilePokerTables"] != null)
            {
                AgilePokerTables =
                    JsonConvert.DeserializeObject<List<AgilePokerTable>>(HttpRuntime.Cache["AgilePokerTables"].ToString());
            }
            AgilePokerTables.Add(
                new AgilePokerTable
                    {
                        Deck = deck,
                        TableName = roomName,
                        Players = new List<AgilePokerPlayer>
                            {
                                new AgilePokerPlayer
                                    {
                                        PreferredName = userPreferredName,
                                        UniqueName = User.Identity.Name
                                    }
                            },
                        AgileMasterPlayer = new AgilePokerPlayer
                            {
                                PreferredName = userPreferredName,
                                UniqueName = User.Identity.Name
                            }
                    });

            var serializedRooms = JsonConvert.SerializeObject(AgilePokerTables);
            HttpRuntime.Cache.Insert("AgilePokerTables", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        private void DisplayVotes(bool displayVotes)
        {
            var roomName = (string) Session["CurrentRoomName"];
            var AgilePokerTables = GetCachedRooms();
            var roomIndex = AgilePokerTables.FindIndex(x => x.TableName == roomName);
            AgilePokerTables[roomIndex].ShowHands = displayVotes;
            var serializedRooms = JsonConvert.SerializeObject(AgilePokerTables);
            HttpRuntime.Cache.Insert("AgilePokerTables", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        private AgilePokerTable GetCachedRoom()
        {
            var roomName = (string) Session["CurrentRoomName"];
            var rooms =
                JsonConvert.DeserializeObject<List<AgilePokerTable>>(HttpRuntime.Cache["AgilePokerTables"].ToString());
            return rooms.First(x => x.TableName == roomName);
        }

        private List<string> GetCachedRoomNames()
        {
            return GetCachedRooms().Select(x => x.TableName).ToList();
        }

        private List<AgilePokerTable> GetCachedRooms()
        {
            var roomNames = new List<AgilePokerTable>();
            if (HttpRuntime.Cache["AgilePokerTables"] != null)
            {
                return
                    JsonConvert.DeserializeObject<List<AgilePokerTable>>(HttpRuntime.Cache["AgilePokerTables"].ToString());
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

        private void UpdateVote(string tableName, AgilePokerPlayer user)
        {
            var agilePokerTables =
                JsonConvert.DeserializeObject<List<AgilePokerTable>>(HttpRuntime.Cache["AgilePokerTables"].ToString());
            if (
                agilePokerTables.First(x => x.TableName == tableName)
                               .Players.FirstOrDefault(x => x.UniqueName == User.Identity.Name) != null)
            {
                var roomIndex = agilePokerTables.FindIndex(x => x.TableName == tableName);
                var userIndex = agilePokerTables[roomIndex].Players.FindIndex(x => x.UniqueName == user.UniqueName);

                agilePokerTables[roomIndex].Players[userIndex] = user;

            }
            var serializedRooms = JsonConvert.SerializeObject(agilePokerTables);
            HttpRuntime.Cache.Insert("AgilePokerTables", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));

            
        }

        #endregion
    }
}