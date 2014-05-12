using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgilePoker.Models
{
    public class Room
    {
        #region Properties

        public List<AgilePokerCard> PlayingCards { get; set; }
        public AgilePokerTable PokerRoom { get; set; }

        [Display(Name = "Select a Card")]
        public AgilePokerCard SelectedCard { get; set; }

        public decimal SelectedCardValue { get; set; }

        public string Hands { get; set; }

        #endregion
    }
}