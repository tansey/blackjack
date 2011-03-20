/* ***************************************************************************
 * This file is part of the NashCoding Blackjack Framework.
 * 
 * Copyright 2010, Wesley Tansey (wes@nashcoding.com)
 *
 * This tutorial is free software: you can redistribute
 * it and/or modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version. If this code is used
 * for a research or other publication, please use the following citation:
 * 
 * W. Tansey. The NashCoding Blackjack Framework, 2010.
 * http://nashcoding.com/blackjack.
 *
 * The NashCoding Blackjack Framework is distributed in the hope 
 * that it will be useful, but WITHOUT ANY WARRANTY; without even 
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR 
 * PURPOSE.  See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the NashCoding Blackjack Framework.  If not, 
 * see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack.Players
{
    /// <summary>
    /// This class implements the Simple Wizard Strategy described
    /// at the WizardOfOdds page:
    /// http://wizardofodds.com/blackjack/apx21.html
    /// 
    /// The EV of this strategy at a single-deck game with Vegas Strip
    /// rules is -0.67%.
    /// </summary>
    public class WizardSimpleStrategy : IBlackjackPlayer
    {
        private long handsToPlay;
        private long handsPlayed;

        public WizardSimpleStrategy(long handsToPlay)
        {
            this.handsToPlay = handsToPlay;
        }

        #region IBlackjackPlayer Members

        public decimal Profit { get; set; }
        public int Splits { get; set; }

        public virtual bool PlayAnotherHand()
        {
            return handsToPlay > handsPlayed;
        }

        public virtual decimal GetBet(decimal min, decimal max)
        {
            return min;
        }

        public virtual bool Split(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var card = hand.Cards.ElementAt(0).Rank;
            if (info.DealerHand.Cards.ElementAt(0).Rank < Ranks.Seven)
            {
                if (card == Ranks.Two ||
                   card == Ranks.Three ||
                    card == Ranks.Six ||
                    card == Ranks.Seven ||
                    card == Ranks.Eight ||
                    card == Ranks.Nine ||
                    card == Ranks.Ace)
                    return true;
                return false;
            }

            return card == Ranks.Eight || card == Ranks.Ace;
        }

        public virtual bool DoubleDown(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            if (soft)
            {
                if (value == 16 || value == 17 || value == 18)
                    return dealer < Ranks.Seven;
                return false;
            }

            if ((value == 10 || value == 11) && value > info.DealerHand.Value)
                return true;

            if (dealer < Ranks.Seven && value == 9)
                return true;

            return false;
        }

        public virtual bool Hit(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            if (soft)
            {
                if (value < 16)
                    return true;

                if (dealer < Ranks.Seven && value == 18)
                    return false;

                if (value < 19)
                    return true;

                return false;
            }

            if (value < 12)
                return true;

            if (value < 17 && dealer > Ranks.Six)
                return true;

            return false;
        }

        public virtual bool BuyInsurance(HandInfo info)
        {
            return false;
        }

        public virtual bool Surrender(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            if (!soft && dealer == Ranks.Ten && value == 16)
                return true;

            return false;
        }

        long nextPrint = 100000L;
        public virtual void HandOver(HandInfo info)
        {
            handsPlayed++;

            if (handsPlayed == nextPrint)
            {
                Console.WriteLine(handsPlayed);
                nextPrint += 100000L;
            }
        }

        public virtual void Reshuffle()
        {
        }

        #endregion
    }
}
