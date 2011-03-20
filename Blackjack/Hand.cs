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
    /// Represents a generic Blackjack hand that contains
    /// a common subset of functionality for both player
    /// and dealer hands.
    /// </summary>
    public abstract class Hand
    {
        private int aces = 0;
        protected List<Card> cards = new List<Card>();

        /// <summary>
        /// The cards in this hand.
        /// </summary>
        public IEnumerable<Card> Cards { get { return cards; } }

        /// <summary>
        /// The best value (closest to 21) of this hand.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Whether this hand contains a soft ace.
        /// </summary>
        public bool Soft { get; set; }

        /// <summary>
        /// Adds a card to the hand and recalculates its
        /// value.
        /// </summary>
        public void AddCard(Card c)
        {
            // Add the card to the collection.
            cards.Add(c);

            // If the new card is an ace, increment the counter.
            if (c.Rank == Ranks.Ace)
                aces++;

            // If we have at least one ace in our hand, we need
            // to consider soft values.
            if (aces > 0)
            {
                int temp = 0;

                // First calculate the base value not including aces.
                foreach (var card in cards)
                    if (card.Rank != Ranks.Ace)
                        temp += card.HighValue;

                // The soft value would be using one ace as 11.
                int soft = temp + 10 + aces;

                // If the soft value isn't a bust (> 21),
                // then it must be the closest to 21.
                if (soft <= 21)
                {
                    Value = soft;
                    Soft = true;
                }
                else
                {
                    Value = temp + aces;
                    Soft = false;
                }
            }
            // If we don't have any aces, we can just take the high value.
            else
                Value += c.HighValue;
        }

        /// <summary>
        /// Returns a pretty-print-formatted string containing
        /// all the cards in the hand.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < cards.Count; i++)
                sb.Append((i > 0 ? " " : "") + cards[i].ToString());
            return sb.ToString();
        }
    }
}
