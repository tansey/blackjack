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
    /// A player which plays basic strategy.
    /// 
    /// For more information, see the Wizard of Odds page:
    /// http://wizardofodds.com/blackjack
    /// </summary>
    public class BasicStrategyPlayer : WizardSimpleStrategy
    {
        public BasicStrategyPlayer(long hands) : base(hands)
        {
        }

        public override bool Surrender(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            if (!soft && value == 15 && dealer == Ranks.Ten)
                return true;

            if (!soft && value == 16 && dealer > Ranks.Eight)
                return true;

            if (!soft && dealer > Ranks.Nine &&
                hand.Cards.ElementAt(0).Rank == Ranks.Seven && 
                hand.Cards.ElementAt(1).Rank == Ranks.Seven)
                return true;
                        
            return base.Surrender(info);
        }

        public override bool DoubleDown(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            if (soft)
            {
                if (value == 18 && dealer == Ranks.Two)
                    return false;

                if (value == 16 && dealer < Ranks.Four)
                    return false;

                if (value > 12 && value < 16 && dealer > Ranks.Four && dealer < Ranks.Seven)
                    return true;

                if (value == 15 && dealer == Ranks.Four)
                    return true;

                if (value == 13 && dealer == Ranks.Five)
                    return true;

                if (value == 13 && dealer == Ranks.Four)
                    return true;

                if (value == 14 && dealer == Ranks.Four)
                    return true;

                if (value == 19 && dealer == Ranks.Six)
                    return true;
            }
            else
            {
                if (value == 8 && dealer > Ranks.Four && dealer < Ranks.Seven)
                    return true;

                if (value == 11)
                    return true;
            }
            return base.DoubleDown(info);
        }

        public override bool Split(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var rank = hand.Cards.ElementAt(0).Rank;
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            if (rank == Ranks.Seven && dealer == Ranks.Seven)
                return true;

            if (rank == Ranks.Seven && dealer == Ranks.Eight)
                return true;

            if (rank == Ranks.Nine && dealer > Ranks.Seven && dealer < Ranks.Ten)
                return true;

            if (rank < Ranks.Four && dealer < Ranks.Eight)
                return true;

            if (rank == Ranks.Four && dealer < Ranks.Seven && dealer > Ranks.Three)
                return true;

            if (rank == Ranks.Three && dealer == Ranks.Eight)
                return true;

            if (rank == Ranks.Six && dealer == Ranks.Seven)
                return true;

            if (rank == Ranks.Nine && dealer == Ranks.Ace)
                return true;

            return base.Split(info);
        }

        public override bool Hit(HandInfo info)
        {
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var value = hand.Value;
            var soft = hand.Soft;
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;

            if (!soft && value == 12 && dealer < Ranks.Four)
                return true;

            if (soft && value == 18 && dealer > Ranks.Six && dealer < Ranks.Nine)
                return false;

            if (soft && value == 18 && dealer == Ranks.Two)
                return false;

            if (soft && value == 17 && dealer == Ranks.Two)
                return true;

            if (!soft && dealer == Ranks.Ten &&
                hand.Cards.ElementAt(0).Rank == Ranks.Seven &&
                hand.Cards.ElementAt(1).Rank == Ranks.Seven)
                return false;
                        
            return base.Hit(info);
        }
    }
}
