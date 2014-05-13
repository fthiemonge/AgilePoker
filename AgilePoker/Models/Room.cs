using System.Collections.Generic;

namespace AgilePoker.Models
{
    public class Room
    {
        #region Properties

        public string RoomName { get; set; }
        public List<AgilePokerCard> PlayingCards { get; set; }
        public string SerializedAgilePokerRoom { get; set; }

        #endregion
    }
}