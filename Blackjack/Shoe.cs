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
    /// A class representing a Blackjack dealer's shoe, containing
    /// a multiple number of 52-card decks.
    /// </summary>
    public class Shoe
    {
        private const int CARDS_PER_DECK = 52;
        private Card[] _shoe;
        private Random _random = new Random();
        private int _shoeIdx = 0;

        /// <summary>
        /// The number of cards left until the dealer shoe runs out.
        /// </summary>
        public int CardsLeft { get { return _shoe.Length - _shoeIdx; } }

        /// <summary>
        /// The number of cards dealt since the last shuffle of the shoe.
        /// </summary>
        public int CardsDealt { get { return _shoeIdx; } }

        /// <summary>
        /// Creates a new shoe with the specified number of 52-card decks.
        /// </summary>
        /// <param name="decks"></param>
        public Shoe(int decks)
        {
            _shoe = new Card[decks * CARDS_PER_DECK];

            int idx = 0;

            // For each deck in the shoe.
            for (int i = 0; i < decks; i++)
                // For each rank in a deck.
                for (int j = 0; j < 10; j++)
                    // For each suit in a deck.
                    for (int k = 0; k < 4; k++, idx++)
                    {
                        // Since T, J, Q, and K are all lumped
                        // together, we need to add 4x as many
                        // tens in the deck as every other card.
                        if (j == 8)
                        {
                            _shoe[idx++] = new Card((Ranks)j);
                            _shoe[idx++] = new Card((Ranks)j);
                            _shoe[idx++] = new Card((Ranks)j);
                        }

                        // Add a card to the shoe.
                        _shoe[idx] = new Card((Ranks)j);
                    }
        }

        /// <summary>
        /// Shuffles the shoe the specified number of times.
        /// </summary>
        public void Shuffle(int shuffleCount)
        {
            for (int i = 0; i < shuffleCount; i++)
                Shuffle();
        }

        /// <summary>
        /// Shuffles the shoe back to a random order.
        /// </summary>
        public void Shuffle()
        {
            for (int i = 0; i < _shoe.Length; i++)
            {
                var temp = _shoe[i];
                var swapIdx = _random.Next(_shoe.Length);
                _shoe[i] = _shoe[swapIdx];
                _shoe[swapIdx] = temp;
            }
            _shoeIdx = 0;
        }
        
        /// <summary>
        /// Deals the next card from the top of the shoe.
        /// </summary>
        public Card NextCard()
        {
            var card = _shoe[_shoeIdx];
            _shoeIdx++;
            return card;
        }

        
    }
}
