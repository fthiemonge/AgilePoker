using System;
using System.Collections.Generic;

namespace AgilePoker
{
    public class AgilePokerCard
    {
        public int Sequence { get; set; }
        public double Value { get; set; }
        public string ToolTip { get; set; }
        public string PictureUrl { get; set; }

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
                                    ToolTip = "Zero",
                                    PictureUrl = "~/Images/Standard_Zero.jpg"
                                },
                            new AgilePokerCard
                                {
                                    Sequence = 2,
                                    Value = .5,
                                    ToolTip = "Half",
                                    PictureUrl = "~/Images/Standard_Half.jpg"
                                }
                            // TODO: Add the rest...
                        };
                case Deck.Fibonacci:
                    // TODO:
                    return new List<AgilePokerCard>();
                case Deck.TeeShirt:
                    // TODO:
                    return new List<AgilePokerCard>();
                default:
                    throw new ApplicationException("No cards associated with deck [" + deck.ToString() + "]");
            }

        }
    }

    
}