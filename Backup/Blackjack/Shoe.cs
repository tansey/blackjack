using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    public class Shoe
    {
        private const int CARDS_PER_DECK = 52;
        private Card[] _shoe;
        private Random _random = new Random();
        private int _shoeIdx = 0;
        public int CardsLeft { get { return _shoe.Length - _shoeIdx; } }
        public int CardsDealt { get { return _shoeIdx; } }


        public Shoe(int decks)
        {
            _shoe = new Card[decks * CARDS_PER_DECK];

            int idx = 0;
            for (int i = 0; i < decks; i++)
                for (int j = 0; j < 10; j++)
                    for (int k = 0; k < 4; k++, idx++)
                    {
                        if (j == 8)
                        {
                            _shoe[idx++] = new Card((Ranks)j, (Suits)k);
                            _shoe[idx++] = new Card((Ranks)j, (Suits)k);
                            _shoe[idx++] = new Card((Ranks)j, (Suits)k);
                        }
                        _shoe[idx] = new Card((Ranks)j, (Suits)k);
                    }
        }

        public void Shuffle(int shuffleCount)
        {
            for (int i = 0; i < shuffleCount; i++)
                Shuffle();
        }

        public void Shuffle()
        {
            for (int i = 0; i < _shoe.Length; i++)
            {
                var temp = _shoe[i];
                var swapIdx = _random.Next(_shoe.Length);
                _shoe[i] = _shoe[swapIdx];
                _shoe[swapIdx] = temp;
            }
            _shoeIdx = 0;
        }

        public Card NextCard()
        {
            var card = _shoe[_shoeIdx];
            _shoeIdx++;
            return card;
        }

        
    }
}
