using System.Collections.Generic;

namespace AgilePoker
{
    public class AgilePokerTable
    {
        #region Properties

        public AgilePokerPlayer AgileMasterPlayer { get; set; }
        public Deck Deck { get; set; }
        public List<AgilePokerPlayer> Players { get; set; }
        public bool ShowHands { get; set; }
        public string TableName { get; set; }

        #endregion

        #region Instance Methods

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return TableName == ((AgilePokerTable) obj).TableName;
        }

        #endregion
    }
}