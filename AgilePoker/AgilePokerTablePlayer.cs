namespace AgilePoker
{
    public class AgilePokerTablePlayer
    {
        #region Properties

        public string PlayerUniqueName { get; set; }
        public string TableName { get; set; }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var other = obj as AgilePokerTablePlayer;
            if (other == null)
                return false;

            return PlayerUniqueName == other.PlayerUniqueName && TableName == other.TableName;
        }
    }
}