using System.Collections.Generic;
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

        public AgilePokerRoom GetRoom(string roomName)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            return _roomState.GetRoom(roomName);
        }

        public AgilePokerCard Vote(string roomName, string uniqueUsername, decimal cardValue)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            var card = _roomState.Vote(roomName, uniqueUsername, cardValue);

            Clients.All.broadcastRoom(_roomState.GetRoom(roomName));

            return card;
        }

        public void ShowVotes(string roomName)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            _roomState.ShowVotes(roomName);

            Clients.All.broadcastRoom(_roomState.GetRoom(roomName));
        }

        public void ClearVotes(string roomName)
        {
            roomName = HttpUtility.HtmlDecode(roomName);
            _roomState.ClearVotes(roomName);

            Clients.All.broadcastRoom(_roomState.GetRoom(roomName));
        }

        #endregion
    }
}