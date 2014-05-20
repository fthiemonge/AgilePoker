using System;
using System.Collections.Generic;

namespace AgilePoker
{
    public class AgilePokerCard
    {
        #region Properties

        public string PictureUrl { get; set; }
        public int Sequence { get; set; }
        public string CardName { get; set; }
        public string TextValue { get; set; }
        public string Representation {
            get
            {
                var representation = "Unknown";
                switch (Type)
                {
                    case CardType.Text:
                        representation = TextValue;
                        break;
                    case CardType.Image:
                        representation = CardName;
                        break;
                    default:
                        representation = CardName;
                        break;
                }
                return representation;
            }
        }
        public CardType Type { get; set; }

        #endregion

        #region Public Static Methods

        public static List<AgilePokerCard> GetCards(Deck deck)
        {
            switch (deck)
            {
                case Deck.Standard:
                    return new List<AgilePokerCard>
                        {
                            new AgilePokerCard
                                {
                                    Sequence = 1,
                                    TextValue = "0",
                                    CardName = "Zero",
                                    Type = CardType.Text,
                                    PictureUrl =  "../../Images/Standard_Zero.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    TextValue = "1/2",
                                    CardName = "Half",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Half.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    TextValue = "1",
                                    CardName = "One",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_One.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    TextValue = "2",
                                    CardName = "Two",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Two.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    TextValue = "3",
                                    CardName = "Three",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Three.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    TextValue = "5",
                                    CardName = "Five",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Five.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    TextValue = "8",
                                    CardName = "Eight",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Eight.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    TextValue = "13",
                                    CardName = "Thirteen",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Thirteen.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    TextValue = "20",
                                    CardName = "Twenty",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Twenty.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    TextValue = "40",
                                    CardName = "Forty",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Forty.jpg"
                                }
                            ,
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    TextValue = "?",
                                    CardName = "Question",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Question.jpg"
                                }
                            ,
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    CardName = "Coffee",
                                    Type = CardType.Image,
                                    PictureUrl = "../../Images/500px-Coffee_cup_icon.svg.png"
                                }
                        };
                case Deck.Fibonacci:
                    return new List<AgilePokerCard>
                        {
                            new AgilePokerCard
                                {
                                    Sequence = 1,
                                    TextValue = "0",
                                    CardName = "Zero",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Zero.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    TextValue = "1",
                                    CardName = "One",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_One.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    TextValue = "2",
                                    CardName = "Two",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Two.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    TextValue = "3",
                                    CardName = "Three",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Three.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    TextValue = "5",
                                    CardName = "Five",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Five.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    TextValue = "8",
                                    CardName = "Eight",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Eight.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    TextValue = "13",
                                    CardName = "Thirteen",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Thirteen.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    TextValue = "21",
                                    CardName = "Twenty One",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_TwentyOne.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    TextValue = "34",
                                    CardName = "Thirty Four",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_ThirtyFour.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    TextValue = "55",
                                    CardName = "Fifty Five",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_FiftyFive.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 11,
                                    TextValue = "89",
                                    CardName = "Eighty Nine",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_EightyNine.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 12,
                                    TextValue = "144",
                                    CardName = "One Hundred Forty Four",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_OneHundredFortyFour.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 13,
                                    TextValue = "\u221e",
                                    CardName = "Infinity",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Infinity.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 14,
                                    TextValue = "?",
                                    CardName = "Question",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Question.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 15,
                                    CardName = "Coffee",
                                    Type = CardType.Image,
                                    PictureUrl = "../../Images/500px-Coffee_cup_icon.svg.png"
                                }
                        };
                case Deck.TeeShirt:
                    return new List<AgilePokerCard>
                        {
                            new AgilePokerCard
                                {
                                    Sequence = 1,
                                    TextValue = "XS",
                                    CardName = "X-Small",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_XS.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    TextValue = "S",
                                    CardName = "Small",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_S.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    TextValue = "M",
                                    CardName = "Medium",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_M.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    TextValue = "L",
                                    CardName = "Large",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_L.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    TextValue = "XL",
                                    CardName = "X-Large",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_XL.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    TextValue = "XXL",
                                    CardName = "XX-Large",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_XXL.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    TextValue = "\u221e",
                                    CardName = "Infinity",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_Infinity.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    TextValue = "?",
                                    CardName = "Question",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_Question.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    CardName = "Coffee",
                                    Type = CardType.Image,
                                    PictureUrl = "../../Images/500px-Coffee_cup_icon.svg.png"
                                }
                        };
                default:
                    throw new ApplicationException("No cards associated with deck [" + deck.ToString() + "]");
            }
        }

        #endregion
    }
}