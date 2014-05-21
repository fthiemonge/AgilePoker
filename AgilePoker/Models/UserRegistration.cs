using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AgilePoker.Models
{
    public class UserRegistration
    {
        #region Properties

        public List<SelectListItem> Decks
        {
            get
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem
                            {
                                Text = "Standard",
                                Value = Deck.Standard.ToString()
                            },
                        new SelectListItem
                            {
                                Text = "Fibonacci",
                                Value = Deck.Fibonacci.ToString()
                            },
                        new SelectListItem
                            {
                                Text = "TeeShirt",
                                Value = Deck.TeeShirt.ToString()
                            }
                    };
            }
        }

        public List<string> ExistingRoomNames { get; set; }

        [Display(Name = "Poker Deck")]
        public Deck NewRoomDeck { get; set; }

        [Display(Name = "Room Name")]
        public string NewRoomName { get; set; }

        public string SelectedExistingRoomName { get; set; }

        [Display(Name = "Your Name")]
        [Required(ErrorMessage = "Your Name is required")]
        public string UserPreferredName { get; set; }

        [Display(Name = "Create as an observer")]
        public bool CreateAsObserver { get; set; }

        [Display(Name = "Join as an observer")]
        public bool JoinAsObserver { get; set; }

        #endregion
    }
}