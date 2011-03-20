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
    /// The ranks of cards in a game of Blackjack.
    /// Note that Ten, Jack, Queen, and King are all
    /// of equal value and thus lumped together as
    /// Ten.
    /// </summary>
    public enum Ranks
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Ace
    }

    /// <summary>
    /// The suits of cards in a deck.
    /// Note that these are not meaningful
    /// in Blackjack and here only for the heck of it.
    /// </summary>
    public enum Suits
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    /// <summary>
    /// A card in a game of Blackjack.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// The rank of this card.
        /// </summary>
        public Ranks Rank { get; set; }

        /// <summary>
        /// The high value of this card-- same as the low
        /// value for all cards except Ace, which has 11 for high
        /// value and 1 for low value.
        /// </summary>
        public int HighValue { get { return HIGH_RANK_VALUES[(int)Rank]; } }

        /// <summary>
        /// The low value of this card-- same as the high
        /// value for all cards except Ace, which has 11 for high
        /// value and 1 for low value.
        /// </summary>
        public int LowValue { get { return LOW_RANK_VALUES[(int)Rank]; } }

        public Card(Ranks rank)
        {
            Rank = rank;
        }


        public static string[] RANK_STRINGS = 
            //new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };
    new string[] { "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ACE" };
        public static string[] SUIT_STRINGS = 
            new string[] { "c", "d", "h", "s" };

        public static int[] HIGH_RANK_VALUES =
            new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        public static int[] LOW_RANK_VALUES =
            new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 1 };

        public override string ToString()
        {
            return RANK_STRINGS[(int)Rank];// +SUIT_STRINGS[(int)Suit];
        }
    }
}
