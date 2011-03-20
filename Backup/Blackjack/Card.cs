using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
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

    public enum Suits
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public class Card
    {
        public Ranks Rank { get; set; }
        public Suits Suit { get; set; }

        public int HighValue { get { return HIGH_RANK_VALUES[(int)Rank]; } }
        public int LowValue { get { return LOW_RANK_VALUES[(int)Rank]; } }

        public Card(Ranks rank, Suits suit)
        {
            Rank = rank;
            Suit = suit;
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
