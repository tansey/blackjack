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
    /// modifies the Wizard of Odds simple strategy slightly 
    /// depending on the number of fives that have been seen.
    /// </summary>
    public class SimpleFiveCountPlayer : WizardSimpleStrategy
    {
        int fives = 0;
        public SimpleFiveCountPlayer(long handsToPlay) : base(handsToPlay)
        {

        }

        public override decimal GetBet(decimal min, decimal max)
        {
            //return min * (1+fives);
            if (fives == 1)
                return min * 2;
            if (fives == 2)
                return min * 4;
            if (fives == 3)
                return min * 6;
            if (fives == 4)
                return min * 9;
            return min;
        }

        public override bool Surrender(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            //if (!soft && value == 16 && fives > 1 && dealer == Ranks.Ten)
            //    return false;

            return base.Surrender(info);
        }

        public override bool Hit(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            //if (!soft && value == 16 && fives > 1 && dealer == Ranks.Ten)
            //    return false;

            return base.Hit(info);
        }

        public override void HandOver(HandInfo info)
        {
            foreach (var c in info.DealerHand.Cards)
                if (c.Rank == Ranks.Five)
                    fives++;

            foreach (var h in info.PlayerHands)
                foreach (var c in h.Cards)
                    if (c.Rank == Ranks.Five)
                        fives++;

            base.HandOver(info);
        }

        public override void Reshuffle()
        {
            fives = 0;
        }
    }
}
