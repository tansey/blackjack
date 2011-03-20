using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    public class Hand
    {
        private int aces = 0;
        protected List<Card> cards = new List<Card>();
        public IEnumerable<Card> Cards { get { return cards; } }
        public int Value { get; set; }
        public bool Soft { get; set; }

        public void AddCard(Card c)
        {
            cards.Add(c);

            if (c.Rank == Ranks.Ace)
                aces++;

            if (aces > 0)
            {
                int temp  = 0;
                foreach (var card in cards)
                    if (card.Rank != Ranks.Ace)
                        temp += card.HighValue;

                int soft = temp + 10 + aces;
                if (soft <= 21)
                {
                    Value = soft;
                    Soft = true;
                }
                else
                {
                    Value = temp + aces;
                    Soft = false;
                }
            }
            else
                Value += c.HighValue;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(string.Format("({0}):", Value));
            for(int i = 0; i < cards.Count; i++)
                sb.Append((i > 0 ? " " : "") + cards[i].ToString());
            return sb.ToString();
        }
    }
}
