using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    public class DealerHand : Hand
    {
        public Card HiddenCard { get; set; }

        public void FlipHiddenCard()
        {
            AddCard(HiddenCard);
        }
    }
}
