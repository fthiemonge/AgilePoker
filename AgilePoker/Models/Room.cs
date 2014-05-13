using System.Collections.Generic;

namespace AgilePoker.Models
{
    public class Room
    {
        #region Properties

        public List<AgilePokerCard> PlayingCards { get; set; }
        public string RoomName { get; set; }

        #endregion
    }
}