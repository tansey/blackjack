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
    /// A player which does some simple card counting and
    /// modifies basic strategy slightly depending on the count.
    /// </summary>
    public class CountingSystemBasicStrategyPlayer : BasicStrategyPlayer
    {
        private int count;

        public CountingSystemBasicStrategyPlayer(long handsToPlay)
            : base(handsToPlay)
        {
        }



        public override decimal GetBet(decimal min, decimal max)
        {
            if (count > 0)
                return min * count;
            return min;
        }

        public override bool Hit(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            if (count > 1 && value == 16 && !soft && dealer == Ranks.Ten)
                return false;

            if (count > 3 && value == 12 && !soft && dealer < Ranks.Four)
                return false;

            return base.Hit(info);
        }

        public override bool DoubleDown(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            //if (count > 3 && dealer == Ranks.Six &&
            //    (value == 8 || (soft && value == 18)))
            //    return true;

            return base.DoubleDown(info);
        }

        public override bool BuyInsurance(HandInfo info)
        {
            //if (count > 4)
            //    return true;
            return base.BuyInsurance(info);
        }

        public override void HandOver(HandInfo info)
        {
            //Adjust the count based on what was seen this hand.
            foreach (var c in info.DealerHand.Cards)
                if (c.Rank < Ranks.Seven)
                    count++;
                else if (c.Rank > Ranks.Nine)
                    count--;

            foreach (var h in info.PlayerHands)
                foreach (var c in h.Cards)
                    if (c.Rank < Ranks.Seven)
                        count++;
                    else if (c.Rank > Ranks.Nine)
                        count--;

            base.HandOver(info);
        }

        public override void Reshuffle()
        {
            count = 0;
            base.Reshuffle();
        }
    }
}
