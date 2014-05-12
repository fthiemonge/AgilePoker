using System.Collections.Generic;

namespace AgilePoker
{
    public class AgilePokerRoom
    {
        #region Properties

        public AgilePokerUser AgileMasterUser { get; set; }
        public Deck Deck { get; set; }
        public bool DisplayVotes { get; set; }
        public string Name { get; set; }
        public List<AgilePokerUser> Users { get; set; }

        #endregion
    }
}