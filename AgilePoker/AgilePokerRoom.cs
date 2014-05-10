using System.Collections.Generic;

namespace AgilePoker
{
    public class AgilePokerRoom
    {
        public string Name { get; set; }
        public Deck Deck { get; set; }
        public List<AgilePokerUser> Users { get; set; }
        public AgilePokerUser AgileMasterUser { get; set; }
    }
}