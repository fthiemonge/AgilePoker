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
        public decimal Value { get; set; }
        public string TextValue { get; set; }
        public string Representation {
            get
            {
                var representation = "Unknown";
                switch (Type)
                {
                    case CardType.Numeric:
                        if (String.IsNullOrWhiteSpace(TextValue))
                        {
                            representation = Value.ToString();
                        }
                        else
                        {
                            representation = TextValue;
                        }
                        break;
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
                                    Value = 0,
                                    CardName = "Zero",
                                    Type = CardType.Numeric,
                                    PictureUrl =  "../../Images/Standard_Zero.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    Value = .5M,
                                    TextValue = "1/2",
                                    CardName = "Half",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_Half.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    Value = 1,
                                    CardName = "One",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_One.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    Value = 2,
                                    CardName = "Two",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_Two.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    Value = 3,
                                    CardName = "Three",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_Three.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    Value = 5,
                                    CardName = "Five",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_Five.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    Value = 8,
                                    CardName = "Eight",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_Eight.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    Value = 13,
                                    CardName = "Thirteen",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_Thirteen.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    Value = 20,
                                    CardName = "Twenty",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_Twenty.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    Value = 40,
                                    CardName = "Forty",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Standard_Forty.jpg"
                                }
                            ,
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    Value = -1,
                                    TextValue = "?",
                                    CardName = "Question",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Standard_Question.jpg"
                                }
                            ,
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    Value = -2,
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
                                    Value = 0,
                                    CardName = "Zero",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_Zero.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    Value = 1,
                                    CardName = "One",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_One.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    Value = 2,
                                    CardName = "Two",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_Two.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    Value = 3,
                                    CardName = "Three",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_Three.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    Value = 5,
                                    CardName = "Five",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_Five.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    Value = 8,
                                    CardName = "Eight",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_Eight.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    Value = 13,
                                    CardName = "Thirteen",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_Thirteen.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    Value = 21,
                                    CardName = "Twenty One",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_TwentyOne.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    Value = 34,
                                    CardName = "Thirty Four",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_ThirtyFour.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    Value = 55,
                                    CardName = "Fifty Five",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_FiftyFive.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 11,
                                    Value = 89,
                                    CardName = "Eighty Nine",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_EightyNine.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 12,
                                    Value = 144,
                                    CardName = "One Hundred Forty Four",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_OneHundredFortyFour.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 13,
                                    Value = decimal.MaxValue,
                                    TextValue = "\u221e",
                                    CardName = "Infinity",
                                    Type = CardType.Numeric,
                                    PictureUrl = "../../Images/Fibonacci_Infinity.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 14,
                                    Value = -1,
                                    TextValue = "?",
                                    CardName = "Question",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/Fibonacci_Question.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 15,
                                    Value = -2,
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
                                    Value = 1,
                                    TextValue = "XS",
                                    CardName = "X-Small",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_XS.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    Value = 2,
                                    TextValue = "S",
                                    CardName = "Small",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_S.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    Value = 3,
                                    TextValue = "M",
                                    CardName = "Medium",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_M.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    Value = 4,
                                    TextValue = "L",
                                    CardName = "Large",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_L.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    Value = 5,
                                    TextValue = "XL",
                                    CardName = "X-Large",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_XL.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    Value = 6,
                                    TextValue = "XXL",
                                    CardName = "XX-Large",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_XXL.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    Value = decimal.MaxValue,
                                    TextValue = "\u221e",
                                    CardName = "Infinity",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_Infinity.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    Value = -1,
                                    TextValue = "?",
                                    CardName = "Question",
                                    Type = CardType.Text,
                                    PictureUrl = "../../Images/TeeShirt_Question.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    Value = -2,
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