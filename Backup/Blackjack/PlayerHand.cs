using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    public class PlayerHand : Hand
    {
        public decimal Bet { get; set; }
        public decimal Profit { get { return Won - Bet; } }
        public IBlackjackPlayer Player { get; set; }
        public decimal Won { get; set;  }
        public bool Finished { get; set; }
        public bool Insured { get; set; }
        public bool HasBeenSplit { get; set; }

        public PlayerHand Split()
        {
            PlayerHand p1 = new PlayerHand()
            {
                Bet = this.Bet,
                Player = this.Player,
                HasBeenSplit = true
            };
            Card temp = cards[0];
            p1.AddCard(cards[1]);


            cards.Clear();
            Value = 0;
            Soft = false;
            AddCard(temp);
            HasBeenSplit = true;

            return p1;
        }
    }
}
