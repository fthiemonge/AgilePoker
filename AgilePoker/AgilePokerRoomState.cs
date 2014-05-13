using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgilePoker.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace AgilePoker
{
    public class AgilePokerRoomState
    {
        private readonly static Lazy<AgilePokerRoomState> _instance = new Lazy<AgilePokerRoomState>(() => new AgilePokerRoomState(GlobalHost.ConnectionManager.GetHubContext<AgilePokerHub>().Clients));
        private readonly ConcurrentDictionary<string, AgilePokerRoom> _pokerRooms = new ConcurrentDictionary<string, AgilePokerRoom>();

        private AgilePokerRoomState(IHubConnectionContext clients)
        {
            Clients = clients;

            _pokerRooms.Clear();
            var pokerRooms = GetPokerRoomsFromCache();
            pokerRooms.ForEach(x => _pokerRooms.TryAdd(x.RoomName, x));
        }

        public List<AgilePokerRoom> GetPokerRoomsFromCache()
        {
            
            if (HttpRuntime.Cache[Constants.Cache.AgilePokerRooms] != null)
            {
                return JsonConvert.DeserializeObject<List<AgilePokerRoom>>(
                        HttpRuntime.Cache[Constants.Cache.AgilePokerRooms].ToString());
            }
            return new List<AgilePokerRoom>();
        }

        public static AgilePokerRoomState Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public AgilePokerRoom GetRoom(string roomName)
        {
            return GetPokerRoomsFromCache().First(x => x.RoomName == roomName);
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        public void Vote(string roomName, string uniqueUsername, decimal cardValue)
        {
            var room = GetRoom(roomName);

            var cards = AgilePokerCard.GetCards(room.Deck);

            var card = cards.First(x => x.Value == cardValue);

            var voteIndex = room.Votes.FindIndex(x => x.User.UniqueName.Replace("\\", "") == uniqueUsername.Replace("\\", ""));

            room.Votes[voteIndex].Card = card;

            var rooms = GetPokerRoomsFromCache();
            var roomIndex = rooms.FindIndex(x => x.RoomName == roomName);
            rooms[roomIndex] = room;

            var serializedRooms = JsonConvert.SerializeObject(rooms);
            HttpRuntime.Cache.Insert(Constants.Cache.AgilePokerRooms, serializedRooms, null, DateTime.MaxValue, new TimeSpan(2, 0, 0));
        }


        //private void BroadcastPokerHands(string tableName)
        //{
        //    Clients.All.broadcastPokerHands(GetAllRooms(tableName));
        //}
    }
}