using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AgilePoker.Hubs
{
    [HubName("AgilePokerHub")]
    public class AgilePokerHub : Hub
    {
        #region Static Member Variables

        private readonly AgilePokerRoomState _roomState;

        #endregion

        #region Constructors

        public AgilePokerHub() : this(AgilePokerRoomState.Instance)
        {
        }

        public AgilePokerHub(AgilePokerRoomState roomState)
        {
            _roomState = roomState;
        }

        #endregion

        #region Instance Methods

        public void Join(string tableName)
        {
            Groups.Add(Context.ConnectionId, tableName);
        }

        public AgilePokerRoom GetRoom(string roomName)
        {
            return _roomState.GetRoom(roomName);
        }

        public void Vote(string roomName, string uniqueUsername, decimal cardValue)
        {
            _roomState.Vote(roomName, uniqueUsername, cardValue);

            Clients.All.broadcastRoom(_roomState.GetRoom(roomName));
        }

        #endregion
    }
}