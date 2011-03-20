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

namespace Blackjack
{
    /// <summary>
    /// A collection of various settings for a game of Blackjack.
    /// </summary>
    public class BlackjackSettings
    {
        /// <summary>
        /// The number of 52-card decks contained in the dealer's shoe.
        /// </summary>
        public int DecksPerShoe { get; set; }

        /// <summary>
        /// The number of cards to deal before the shoe is reshuffled.
        /// </summary>
        public int MinCardsDealtBeforeReshuffle { get; set; }

        /// <summary>
        /// The minimum amount a player can bet.
        /// </summary>
        public decimal MinimumBet { get; set; }

        /// <summary>
        /// The maximum amount a player can bet.
        /// </summary>
        public decimal MaximumBet { get; set; }
                
        /// <summary>
        /// The threshold where the dealer stops hitting and 
        /// stands (typically 17).
        /// </summary>
        public int DealerHardStandThreshold { get; set; }

        /// <summary>
        /// The threshold where the dealer stops hitting and
        /// stands if one of their cards is a soft ace.
        /// </summary>
        public int DealerSoftStandThreshold { get; set; }
        
        /// <summary>
        /// The reward multiplier for Blackjack, default is
        /// 2.5 (i.e., pays 1.5:1).
        /// </summary>
        public decimal BlackjackPayoff { get; set; }
        
        /// <summary>
        /// Whether the player is offered insurance when the
        /// dealer shows a potential blackjack.
        /// </summary>
        public bool InsuranceOffered { get; set; }

        /// <summary>
        /// The cost to purchase insurance, as a fraction
        /// of the original bet.
        /// </summary>
        public decimal InsuranceCost { get; set; }

        /// <summary>
        /// The payoff for insurance, where 2 is 1:1 payoff.
        /// </summary>
        public decimal InsurancePayoff { get; set; }
        
        /// <summary>
        /// The maximum number of times a player can split
        /// in a hand.
        /// </summary>
        public int MaxSplitsAllowed { get; set; }

        /// <summary>
        /// Whether the player is allowed to resplit aces.
        /// Ex: Dealt AA, split to AA and Ax -- can the 
        /// first hand be resplit?
        /// </summary>
        public bool ResplitAcesAllowed { get; set; }

        /// <summary>
        /// Whether the player is allowed to split on 20.
        /// </summary>
        public bool SplitTensAllowed { get; set; }

        /// <summary>
        /// Whether the player is allowed to hit after
        /// splitting aces.
        /// </summary>
        public bool HittingSplitAcesAllowed { get; set; }
        
        /// <summary>
        /// Whether the player can double down only on ten or
        /// eleven. If false, the player can double down on
        /// any hand.
        /// </summary>
        public bool DoubleDownOnlyTenOrEleven { get; set; }

        /// <summary>
        /// Whether the player is allowed to double down on a
        /// soft hand (contains an ace).
        /// </summary>
        public bool SoftDoubleDownAllowed { get; set; }

        /// <summary>
        /// Whether the player is allowed to double down after
        /// splitting hands other than AA.
        /// </summary>
        public bool DoubleDownNonAceSplitsAllowed { get; set; }

        /// <summary>
        /// Whether the player is allowed to double down after
        /// splitting AA.
        /// </summary>
        public bool DoubleDownSplitAcesAllowed { get; set; }
        
        /// <summary>
        /// Whether the player is allowed to surrender their hand
        /// and receive a partial payoff back.
        /// </summary>
        public bool SurrenderAllowed { get; set; }

        /// <summary>
        /// The payoff for surrendering, as a fraction of the
        /// original bet. Typically this is 0.5.
        /// </summary>
        public decimal SurrenderPayoff { get; set; }
    }
}
