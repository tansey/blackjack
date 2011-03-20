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
    /// A class containing all the information about the hands
    /// in the current Blackjack game.
    /// </summary>
    public class HandInfo
    {
        /// <summary>
        /// The dealer's hand.
        /// </summary>
        public DealerHand DealerHand { get; set; }

        /// <summary>
        /// The players' hands.
        /// </summary>
        public IEnumerable<PlayerHand> PlayerHands { get; set; }

        /// <summary>
        /// The hand which has to act next.
        /// </summary>
        public int HandToPlay { get; set; }
    }
}
