using System.Collections.Generic;

namespace AgilePoker
{
    public class AgilePokerRoom
    {
        public string RoomName { get; set; }
        public List<AgilePokerVote> Votes { get; set; }
        public List<AgilePokerUser> Observers { get; set; }
        public Deck Deck { get; set; }
        public bool ShowVotes { get; set; }
        public AgilePokerUser ScrumMaster { get; set; }
    }
}