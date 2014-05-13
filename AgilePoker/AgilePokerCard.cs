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
                                    PictureUrl =  "../../Images/Standard_Zero.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    Value = .5M,
                                    CardName = "Half",
                                    PictureUrl = "../../Images/Standard_Half.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    Value = 1,
                                    CardName = "One",
                                    PictureUrl = "../../Images/Standard_One.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    Value = 2,
                                    CardName = "Two",
                                    PictureUrl = "../../Images/Standard_Two.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    Value = 3,
                                    CardName = "Three",
                                    PictureUrl = "../../Images/Standard_Three.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    Value = 5,
                                    CardName = "Five",
                                    PictureUrl = "../../Images/Standard_Five.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    Value = 8,
                                    CardName = "Eight",
                                    PictureUrl = "../../Images/Standard_Eight.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    Value = 13,
                                    CardName = "Thirteen",
                                    PictureUrl = "../../Images/Standard_Thirteen.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    Value = 20,
                                    CardName = "Twenty",
                                    PictureUrl = "../../Images/Standard_Twenty.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    Value = 40,
                                    CardName = "Forty",
                                    PictureUrl = "../../Images/Standard_Forty.jpg"
                                }
                            ,
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    Value = -1,
                                    CardName = "Question",
                                    PictureUrl = "../../Images/Standard_Question.jpg"
                                }
                            ,
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    Value = -2,
                                    CardName = "Coffee",
                                    PictureUrl = "../../Images/Standard_Coffee.jpg"
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
                                    PictureUrl = "../../Images/Fibonacci_Zero.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    Value = 1,
                                    CardName = "One",
                                    PictureUrl = "../../Images/Fibonacci_One.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    Value = 2,
                                    CardName = "Two",
                                    PictureUrl = "../../Images/Fibonacci_Two.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    Value = 3,
                                    CardName = "Three",
                                    PictureUrl = "../../Images/Fibonacci_Three.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    Value = 5,
                                    CardName = "Five",
                                    PictureUrl = "../../Images/Fibonacci_Five.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    Value = 8,
                                    CardName = "Eight",
                                    PictureUrl = "../../Images/Fibonacci_Eight.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    Value = 13,
                                    CardName = "Thirteen",
                                    PictureUrl = "../../Images/Fibonacci_Thirteen.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    Value = 21,
                                    CardName = "Twenty One",
                                    PictureUrl = "../../Images/Fibonacci_TwentyOne.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    Value = 34,
                                    CardName = "Thirty Four",
                                    PictureUrl = "../../Images/Fibonacci_ThirtyFour.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 10,
                                    Value = 55,
                                    CardName = "Fifty Five",
                                    PictureUrl = "../../Images/Fibonacci_FiftyFive.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 11,
                                    Value = 89,
                                    CardName = "Eighty Nine",
                                    PictureUrl = "../../Images/Fibonacci_EightyNine.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 12,
                                    Value = 144,
                                    CardName = "One Hundred Forty Four",
                                    PictureUrl = "../../Images/Fibonacci_OneHundredFortyFour.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 13,
                                    Value = -1,
                                    CardName = "Question",
                                    PictureUrl = "../../Images/Fibonacci_Question.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 14,
                                    Value = -2,
                                    CardName = "Coffee",
                                    PictureUrl = "../../Images/Fibonacci_Coffee.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 15,
                                    Value = decimal.MaxValue,
                                    CardName = "Infinity",
                                    PictureUrl = "../../Images/Fibonacci_Infinity.jpg"
                                }
                        };
                case Deck.TeeShirt:
                    return new List<AgilePokerCard>
                        {
                            new AgilePokerCard
                                {
                                    Sequence = 1,
                                    Value = 1,
                                    CardName = "X-Small",
                                    PictureUrl = "../../Images/TeeShirt_XS.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    Value = 2,
                                    CardName = "Small",
                                    PictureUrl = "../../Images/TeeShirt_S.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 3,
                                    Value = 3,
                                    CardName = "Medium",
                                    PictureUrl = "../../Images/TeeShirt_M.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 4,
                                    Value = 4,
                                    CardName = "Large",
                                    PictureUrl = "../../Images/TeeShirt_L.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 5,
                                    Value = 5,
                                    CardName = "X-Large",
                                    PictureUrl = "../../Images/TeeShirt_XL.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 6,
                                    Value = 6,
                                    CardName = "XX-Large",
                                    PictureUrl = "../../Images/TeeShirt_XXL.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 7,
                                    Value = -1,
                                    CardName = "Question",
                                    PictureUrl = "../../Images/TeeShirt_Question.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 8,
                                    Value = -2,
                                    CardName = "Coffee",
                                    PictureUrl = "../../Images/TeeShirt_Coffee.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 9,
                                    Value = decimal.MaxValue,
                                    CardName = "Infinity",
                                    PictureUrl = "../../Images/TeeShirt_Infinity.jpg"
                                }
                        };
                default:
                    throw new ApplicationException("No cards associated with deck [" + deck.ToString() + "]");
            }
        }

        #endregion
    }
}