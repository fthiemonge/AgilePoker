using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgilePoker.Models;
using Newtonsoft.Json;

namespace AgilePoker.Controllers
{
    [Authorize] 
    public class HomeController : Controller
    {
        #region Instance Methods

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreateRoom")]
        public ActionResult CreateRoom(UserRegistration model)
        {
            var existingRooms = GetCachedRoomNames().Select(x => x.ToLower());
            if (model.NewRoomName == null || model.NewRoomName.Trim() == "")
            {
                ModelState.AddModelError("NewRoomName", "Room Name required");
            }
            else if (existingRooms.Contains(model.NewRoomName.ToLower().Trim()))
            {
                ModelState.AddModelError("NewRoomName", "Room by this name already in use");
            }
            else
            {
                model.NewRoomName = model.NewRoomName.Trim();    
            }

            if (ModelState.IsValid)
            {
                SetUserPreferredNameInCookie(model.UserPreferredName);
                CreateRoom(model.NewRoomName, model.NewRoomDeck, model.UserPreferredName, model.CreateAsObserver);
                return RedirectToAction("Index", "Room", new
                    {
                        id = model.NewRoomName
                    });
            }

            model.UserPreferredName = GetUserPreferredNameFromCookie();
            model.ExistingRoomNames = GetCachedRoomNames();
            return View("Index", model);
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
            AddUserToRoom(model.SelectedExistingRoomName, model.UserPreferredName, model.JoinAsObserver);
            return RedirectToAction("Index", "Room", new
                {
                    id = model.SelectedExistingRoomName
                });
        }

        #endregion

        #region Private Instance Methods

        private void AddUserToRoom(string roomName, string preferredUsername, bool isObserver)
        {
            var agilePokerRooms =
                JsonConvert.DeserializeObject<List<AgilePokerRoom>>(
                    HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
            var votes = agilePokerRooms.First(x => x.RoomName == roomName).Votes;

            // You can be a voter or an overserver, but not both
            if (!isObserver && votes.FirstOrDefault(x => x.User.UniqueName == User.Identity.Name) == null)
            {
                agilePokerRooms.First(x => x.RoomName == roomName).Votes.Add(
                    new AgilePokerVote
                        {
                            User = new AgilePokerUser
                                {
                                    PreferredName = preferredUsername,
                                    UniqueName = User.Identity.Name
                                },
                            Card = null
                        }
                    );
            }
            else if (isObserver && votes.FirstOrDefault(x => x.User.UniqueName == User.Identity.Name) != null)
            {
                var voterIndex = votes.FindIndex(x => x.User.UniqueName == User.Identity.Name);
                agilePokerRooms.First(x => x.RoomName == roomName).Votes.RemoveAt(voterIndex);
            }

            var observers = agilePokerRooms.First(x => x.RoomName == roomName).Observers;

            if (isObserver && observers.FirstOrDefault(x => x.UniqueName == User.Identity.Name) == null)
            {
                observers.Add(new AgilePokerUser
                {
                    PreferredName = preferredUsername,
                    UniqueName = User.Identity.Name
                });
            }
            else if (!isObserver && observers.FirstOrDefault(x => x.UniqueName == User.Identity.Name) != null)
            {
                var observerIndex = observers.FindIndex(x => x.UniqueName == User.Identity.Name);
                observers.RemoveAt(observerIndex);
            }

            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert(Constants.Cache.AgilePokerRooms, serializedRooms, null, DateTime.MaxValue,
                new TimeSpan(2, 0, 0));
        }

        private void CreateRoom(string roomName, Deck deck, string preferredUsername, bool isObserver)
        {
            var agilePokerRooms = new List<AgilePokerRoom>();
            if (HttpRuntime.Cache[Constants.Cache.AgilePokerRooms] != null)
            {
                agilePokerRooms =
                    JsonConvert.DeserializeObject<List<AgilePokerRoom>>(
                        HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
            }
            agilePokerRooms.Add(
                new AgilePokerRoom
                    {
                        Deck = deck,
                        RoomName = roomName,
                        Votes = isObserver 
                            ? new List<AgilePokerVote>()
                            : new List<AgilePokerVote>
                                {
                                    new AgilePokerVote
                                        {
                                            User = new AgilePokerUser
                                                {
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
                            },
                        Observers = isObserver 
                            ? new List<AgilePokerUser> 
                                {
                                    new AgilePokerUser
                                    {
                                        PreferredName = preferredUsername,
                                        UniqueName = User.Identity.Name
                                    }
                                } 
                            : new List<AgilePokerUser>()
                    });

            

            var serializedRooms = JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert(Constants.Cache.AgilePokerRooms, serializedRooms, null, DateTime.MaxValue,
                new TimeSpan(2, 0, 0));
        }


        private List<string> GetCachedRoomNames()
        {
            return GetCachedRooms().Select(x => x.RoomName).ToList();
        }

        private IEnumerable<AgilePokerRoom> GetCachedRooms()
        {
            var roomNames = new List<AgilePokerRoom>();
            if (HttpRuntime.Cache[Constants.Cache.AgilePokerRooms] != null)
            {
                return
                    JsonConvert.DeserializeObject<List<AgilePokerRoom>>(
                        HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
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

        #endregion
    }
}