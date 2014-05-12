using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AgilePoker.Hubs
{
    [HubName("AgilePokerHub")]
    public class AgilePokerHub : Hub
    {
        #region Static Member Variables

        private readonly AgilePokerHands _hands;

        #endregion

        #region Constructors

        public AgilePokerHub() : this(AgilePokerHands.Instance)
        {
        }

        public AgilePokerHub(AgilePokerHands hands)
        {
            _hands = hands;
        }

        #endregion

        #region Instance Methods

        public void Join(string tableName)
        {
            Groups.Add(Context.ConnectionId, tableName);
        }

        public IEnumerable<AgilePokerHand> GetAllHands(string tableName)
        {
            return _hands.GetAllHands(tableName);
        }

        public void PlayHand(string tableName, string playerUniqueName, string handValue)
        {
            Clients.All.broadcastPokerHands(_hands.GetAllHands(tableName));
        }

        #endregion
    }
}