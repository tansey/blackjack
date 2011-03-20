using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    public class HandInfo
    {
        public DealerHand DealerHand { get; set; }
        public IEnumerable<PlayerHand> PlayerHands { get; set; }
        public int HandToPlay { get; set; }
    }
}
