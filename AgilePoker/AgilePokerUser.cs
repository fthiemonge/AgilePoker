namespace AgilePoker
{
    public class AgilePokerUser
    {
        #region Properties

        public string PreferredName { get; set; }
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

            return UniqueName == ((AgilePokerUser) obj).UniqueName;
        }

        #endregion
    }
}