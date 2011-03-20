using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    public interface IBlackjackPlayer
    {
        // the total amount that the player has won or lost in the current game
        decimal Profit { get; set; }

        // returns true if the player will play the next hand
        // false means they're done and not sitting back down
        bool PlayAnotherHand();

        // returns the amount the player is wagering on this hand
        decimal GetBet(decimal min, decimal max);

        // returns true if the player wants to split the hand
        bool Split(HandInfo info);

        // returns true if the player wants to double down on the hand
        bool DoubleDown(HandInfo info);

        // returns true if the player wants to hit the hand
        bool Hit(HandInfo info);

        // returns true if the player wants to buy insurance
        bool BuyInsurance(HandInfo info);

        // returns true if the player wants to surrender this hand
        bool Surrender(HandInfo info);

        // updates the player with the full info on the hand and how much they won or lost on it.
        void HandOver(HandInfo info);
    }
}
