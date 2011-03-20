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
    /// A class representing a Blackjack player's hand.
    /// </summary>
    public class PlayerHand : Hand
    {
        /// <summary>
        /// The amount the player bet on this hand.
        /// </summary>
        public decimal Bet { get; set; }
        
        /// <summary>
        /// The amount of profit (Won - Bet) the player made on this hand.
        /// </summary>
        public decimal Profit { get { return Won - Bet; } }
        
        /// <summary>
        /// The player to which this hand belongs.
        /// </summary>
        public IBlackjackPlayer Player { get; set; }

        /// <summary>
        /// The amount the player won (Bet + Profit) on this hand.
        /// </summary>
        public decimal Won { get; set;  }

        /// <summary>
        /// Whether the hand is finished or still in progress.
        /// </summary>
        public bool Finished { get; set; }

        /// <summary>
        /// Whether the hand is insured against dealer Blackjack.
        /// </summary>
        public bool Insured { get; set; }

        /// <summary>
        /// Whether this hand was split from another hand.
        /// </summary>
        public bool HasBeenSplit { get; set; }

        /// <summary>
        /// Splits this hand into two separate hands with one card each.
        /// This hand becomes the first hand and the second hand is returned.
        /// </summary>
        /// <returns></returns>
        public PlayerHand Split()
        {
            // Create the new hand.
            PlayerHand p1 = new PlayerHand()
            {
                Bet = this.Bet,
                Player = this.Player,
                HasBeenSplit = true
            };

            // Add the top card to the new hand.
            p1.AddCard(cards[1]);

            // Save the current bottom card for this hand.
            Card temp = cards[0];
            
            // Clear the cards from this hand.
            cards.Clear();
            
            // Reset this hand.
            Value = 0;
            Soft = false;

            // Add the bottom card to this hand.
            AddCard(temp);

            // Flag that this hand has been split.
            HasBeenSplit = true;

            // Return the new hand.
            return p1;
        }
    }
}
