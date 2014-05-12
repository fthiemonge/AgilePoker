namespace AgilePoker
{
    public class AgilePokerPlayer
    {
        #region Properties

        public string PreferredName { get; set; }
        public AgilePokerCard SelectedCard { get; set; }
        public string UniqueName { get; set; }

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

            return UniqueName == ((AgilePokerPlayer) obj).UniqueName;
        }

        #endregion
    }
}