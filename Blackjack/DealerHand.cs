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
    /// The hand for the dealer. This is the same as the hands
    /// of the players, except that the dealer hand has a hidden
    /// card which is flipped over at the end.
    /// </summary>
    public class DealerHand : Hand
    {
        /// <summary>
        /// The card that's hidden from the players.
        /// </summary>
        public Card HiddenCard { get; set; }

        /// <summary>
        /// Flips the hidden card so that all players can
        /// see what the dealer has.
        /// </summary>
        public void FlipHiddenCard()
        {
            AddCard(HiddenCard);
        }
    }
}
