﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgilePoker.Models;

namespace AgilePoker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new UserRegistration();
            model.UserPreferredName = GetUserPreferredNameFromCookie();
            model.ExistingRoomNames = GetExistingRoomNames();
            return View(model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "JoinRoom")]
        public ActionResult JoinRoom(UserRegistration model)
        {
            AddUserToRoom(model.SelectedExistingRoomName, model.UserPreferredName);
            return EnterRoom(model.SelectedExistingRoomName);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreateRoom")]
        public ActionResult CreateRoom(UserRegistration model)
        {
            var existingRooms = GetExistingRoomNames().Select(x => x.ToLower());
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
                CreateRoom(model.NewRoomName, model.NewRoomDeck, model.UserPreferredName);
                return EnterRoom(model.NewRoomName);
            }

            model.UserPreferredName = GetUserPreferredNameFromCookie();
            model.ExistingRoomNames = GetExistingRoomNames();
            return View("Index", model);
        }

        public ActionResult EnterRoom(string roomName)
        {
            var model = new Room
                {
                    PokerRoom = GetExistingRooms().First(x => x.Name == roomName)
                };
            return View("Room", model);
        }

        private List<string> GetExistingRoomNames()
        {
            return GetExistingRooms().Select(x => x.Name).ToList();
        }

        private List<AgilePokerRoom> GetExistingRooms()
        {
            var roomNames = new List<AgilePokerRoom>();
            if (HttpRuntime.Cache["AgilePokerRooms"] != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache["AgilePokerRooms"].ToString());
            }
            return roomNames;
        }

        private void CreateRoom(string roomName, Deck deck, string userPreferredName)
        {
            var agilePokerRooms = new List<AgilePokerRoom>();
            if (HttpRuntime.Cache["AgilePokerRooms"] != null)
            {
                agilePokerRooms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache["AgilePokerRooms"].ToString());
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
                            }
                    });

            var serializedRooms = Newtonsoft.Json.JsonConvert.SerializeObject(agilePokerRooms);
            HttpRuntime.Cache.Insert("AgilePokerRooms", serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }

        private void AddUserToRoom(string roomName, string preferredName)
        {
            var agilePokerRooms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AgilePokerRoom>>(HttpRuntime.Cache["AgilePokerRooms"].ToString());
            if (agilePokerRooms.First(x => x.Name == roomName).Users.FirstOrDefault(x => x.UniqueName == User.Identity.Name) == null)
            {
                agilePokerRooms.First(x => x.Name == roomName).Users.Add(new AgilePokerUser { PreferredName = preferredName, UniqueName = User.Identity.Name});
            }
        }

        private string GetUserPreferredNameFromCookie()
        {
            var username = Request.Cookies["UserPreferredName"];
            return username == null || username.Value == null ? User.Identity.Name.Split('\\')[1] : username.Value;
        }
    }
}