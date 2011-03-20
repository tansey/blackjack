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
    /// An interface that all Blackjack players and strategies must implement.
    /// </summary>
    public interface IBlackjackPlayer
    {
        /// <summary>
        /// The total amount that the player has won or lost in the current game.
        /// </summary>
        decimal Profit { get; set; }

        /// <summary>
        /// The number of splits this player has done in the current game.
        /// </summary>
        int Splits { get; set; }

        /// <summary>
        /// Returns true if the player will play the next hand.
        /// False means they're done and not sitting back down.
        /// </summary>
        bool PlayAnotherHand();

        /// <summary>
        /// Returns the amount the player is wagering on this hand.
        /// </summary>
        decimal GetBet(decimal min, decimal max);

        /// <summary>
        /// Returns true if the player wants to split the hand.
        /// </summary>
        bool Split(HandInfo info);

        /// <summary>
        /// Returns true if the player wants to double down on the hand.
        /// </summary>
        bool DoubleDown(HandInfo info);

        /// <summary>
        /// Returns true if the player wants to hit the hand.
        /// </summary>
        bool Hit(HandInfo info);

        /// <summary>
        /// Returns true if the player wants to buy insurance.
        /// </summary>
        bool BuyInsurance(HandInfo info);

        /// <summary>
        /// Returns true if the player wants to surrender this hand.
        /// </summary>
        bool Surrender(HandInfo info);

        /// <summary>
        /// Updates the player with the full info on the hand and how 
        /// much they won or lost on it. Counting strategies can update
        /// here.
        /// </summary>
        void HandOver(HandInfo info);

        /// <summary>
        /// Called when the dealer shuffles the deck
        /// </summary>
        void Reshuffle();
    }
}
