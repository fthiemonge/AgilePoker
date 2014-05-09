using System.Collections.Generic;

namespace AgilePoker.Models
{
    public class Room
    {
        public AgilePokerRoom PokerRoom { get; set; }
        public List<AgilePokerCard>PlayingCards { get; set; }
    }
}