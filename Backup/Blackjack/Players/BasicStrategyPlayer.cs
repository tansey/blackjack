using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack.Players
{
    public class BasicStrategyPlayer : IBlackjackPlayer
    {
        private bool[][] splits = new bool[][] {
            new bool[] { true, true, true, true, true, true, false, false, false, false },
            new bool[] { true, true, true, true, true, true, false, false, false, false },
            new bool[] { false, false, false, true, false, false, false, false, false, false },
            new bool[] { false, false, false, false, false, false, false, false, false, false },
            new bool[] { true, true, true, true, true, true, false, false, false, false },
            new bool[] { true, true, true, true, true, true, true, false, false, false },
            new bool[] { true, true, true, true, true, true, true, true, true, true },
            new bool[] { true, true, true, true, true, false, true, true, false, false },
            new bool[] { false, false, false, false, false, false, false, false, false, false },
            new bool[] { true, true, true, true, true, true, true, true, true, true }
        };
        private long _handsPlayed = 0;
        private long _handsToPlay;
        private int handsTilPrint = 10000;
        public BasicStrategyPlayer(long handsToPlay)
        {
            _handsToPlay = handsToPlay;
        }

        #region IBlackjackPlayer Members

        public decimal Profit { get; set; }

        public bool PlayAnotherHand()
        {
            return _handsPlayed <= _handsToPlay;
        }

        public decimal GetBet(decimal min, decimal max)
        {
            return min;
        }

        public bool Split(HandInfo info)
        {
            var pRank = info.PlayerHands.ElementAt(info.HandToPlay).Cards.ElementAt(0).Rank;
            var dRank = info.DealerHand.Cards.ElementAt(0).Rank;
            return splits[(int)pRank][(int)dRank];
        }

        public bool DoubleDown(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var c1 = hand.Cards.Max(c => c.Rank);
            var c2 = hand.Cards.Min(c=>c.Rank);
            var d1 = info.DealerHand.Cards.ElementAt(0).Rank;

            if (c1 == Ranks.Ace)
            {
                switch (c2)
                {
                    case Ranks.Two: 
                    case Ranks.Three:
                    case Ranks.Four:
                    case Ranks.Five:
                        return d1 > Ranks.Three && d1 < Ranks.Seven;

                    case Ranks.Six:
                        return d1 < Ranks.Seven;
                    case Ranks.Seven:
                        return d1 > Ranks.Two && d1 < Ranks.Seven;
                    case Ranks.Eight:
                    case Ranks.Nine:
                    case Ranks.Ten:
                    case Ranks.Ace:
                        return false;
                }
            }
            else if (c1 == c2)
            {
                switch (c1)
                {
                    case Ranks.Two:
                    case Ranks.Three:
                        return false;
                    case Ranks.Four:
                        return d1 == Ranks.Six;
                    case Ranks.Five:
                        return d1 < Ranks.Ten;
                    case Ranks.Six:
                    case Ranks.Seven:
                    case Ranks.Eight:
                    case Ranks.Nine:
                    case Ranks.Ten:
                    case Ranks.Ace:
                        return false;
                }
            }
            else if (hand.Value == 8)
            {
                if (c1 != Ranks.Six)
                    return d1 == Ranks.Five || d1 == Ranks.Six;
            }
            else if (hand.Value == 9)
            {
                return d1 < Ranks.Seven;
            }
            else if (hand.Value == 10)
            {
                return d1 < Ranks.Ten;
            }
            else if (hand.Value == 11)
            {
                return true;
            }

            return false;
        }

        public bool Hit(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var c1 = hand.Cards.Max(c => c.Rank);
            var c2 = hand.Cards.Min(c => c.Rank);
            var d1 = info.DealerHand.Cards.ElementAt(0).Rank;

            if (hand.Value <= 10)
                return true;

            if (hand.Cards.Count() == 2)
            {
                if (c1 == Ranks.Ace)
                {
                    switch (c2)
                    {
                        case Ranks.Two:
                        case Ranks.Three:
                        case Ranks.Four:
                        case Ranks.Five:
                        case Ranks.Six:
                            return true;
                        case Ranks.Seven:
                            return d1 == Ranks.Nine || d1 == Ranks.Ten;
                        case Ranks.Eight:
                        case Ranks.Nine:
                        case Ranks.Ten:
                            return false;
                        case Ranks.Ace:
                            return true;
                    }
                }
                if (c1 == c2)
                {
                    switch (c1)
                    {
                        case Ranks.Six:
                            if (d1 > Ranks.Seven)
                                return true;
                            break;
                        case Ranks.Seven:
                            if (d1 == Ranks.Nine || d1 == Ranks.Ace)
                                return true;
                            else if (d1 == Ranks.Ten)
                                return false;
                            break;
                        case Ranks.Nine:
                        case Ranks.Ten:
                            return false;
                        case Ranks.Ace:
                            return true;
                    }
                }
            }

            if (hand.Value >= 17)
                return false;

            if (hand.Value == 12)
                return d1 <= Ranks.Three || d1 >= Ranks.Seven;

            if (hand.Value >= 13 && hand.Value <= 15)
                return d1 >= Ranks.Seven;

            if (hand.Value == 16)
            {
                if (d1 < Ranks.Seven)
                    return false;
                if (d1 == Ranks.Ten && hand.Cards.Count() > 2)
                    return false;
                return true;
            }
            return false;
        }

        public bool BuyInsurance(HandInfo info)
        {
            return false;
        }

        public bool Surrender(HandInfo info)
        {
            return false;
        }

        public void HandOver(HandInfo info)
        {
            _handsPlayed++;
            handsTilPrint--;
            if (handsTilPrint == 0)
            {
                handsTilPrint = 10000;
                Console.WriteLine(_handsPlayed);
            }
        }

        #endregion
    }
}
