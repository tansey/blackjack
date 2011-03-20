using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    public class BlackjackSettings
    {
        public int DecksPerShoe { get; set; }
        public int MinCardsDealtBeforeReshuffle { get; set; }

        public decimal MinimumBet { get; set; }
        public decimal MaximumBet { get; set; }
        
        //public bool PlayersCardsFaceDown { get; set; }
        
        public int DealerHardStandThreshold { get; set; }
        public int DealerSoftStandThreshold { get; set; }
        
        public decimal BlackjackPayoff { get; set; }
        
        public bool InsuranceOffered { get; set; }
        public decimal InsuranceCost { get; set; }
        public decimal InsurancePayoff { get; set; }
        
        //public int MaxSplitsAllowed { get; set; }
        public bool ResplitAcesAllowed { get; set; }
        public bool SplitTensAllowed { get; set; }

        public bool HittingSplitAcesAllowed { get; set; }
        
        public bool DoubleDownOnlyTenOrEleven { get; set; }
        public bool SoftDoubleDownAllowed { get; set; }
        public bool DoubleDownNonAceSplitsAllowed { get; set; }
        public bool DoubleDownSplitAcesAllowed { get; set; }
        
        public bool SurrenderAllowed { get; set; }
        public decimal SurrenderPayoff { get; set; }
    }
}
