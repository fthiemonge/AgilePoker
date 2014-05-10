using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgilePoker.Models
{
    public class Room
    {
        public AgilePokerRoom PokerRoom { get; set; }
        public List<AgilePokerCard> PlayingCards { get; set; }
        [Display(Name = "Select a card")]
        public decimal SelectedCardValue { get; set; }
        public AgilePokerCard SelectedCard { get; set; }
    }
}