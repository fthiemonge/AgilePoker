using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public void JoinRoom(string roomName)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            Clients.All.broadcastUpdateRoom(_roomState.GetRoom(roomName), false);
        }

        public AgilePokerCard Vote(string roomName, string uniqueUsername, decimal cardValue)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            var card = _roomState.Vote(roomName, uniqueUsername, cardValue);

            Clients.All.broadcastUpdateRoom(_roomState.GetRoom(roomName), false);

            return card;
        }

        public void ShowVotes(string roomName)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            _roomState.ShowVotes(roomName);

            Clients.All.broadcastUpdateRoom(_roomState.GetRoom(roomName), false);
        }

        public void ClearVotes(string roomName)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            _roomState.ClearVotes(roomName);

            Clients.All.broadcastUpdateRoom(_roomState.GetRoom(roomName), true);
        }

        public void LeaveRoom(string roomName, string uniqueUsername)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            _roomState.LeaveRoom(roomName, uniqueUsername);

            var room = _roomState.GetRoom(roomName);
            // Room will not exist if the scrum master leaves
            if (room != null)
            {
                Clients.All.broadcastUpdateRoom(room, false);
            }
            else
            {
                Clients.All.broadcastKillRoom();
            }
        }

        #endregion
    }
}