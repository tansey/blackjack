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
    /// A look-up table for a Blackjack strategy. This is useful in simulating
    /// strategies that do not rely on the count of the deck. Given a hand
    /// value and dealer card, it will simply look up the appropriate action 
    /// to take.
    /// </summary>
    public class ActionTable
    {
        public enum ActionTypes
        {
            Stand,
            Hit,
            SurrenderOrHit,
            SurrenderOrStand,
            SplitOrHit,
            SplitOrStand,
            DoubleDownOrHit,
            DoubleDownOrStand
        }

        public ActionTypes[,] Table;

        public ActionTypes this[int x, int y] { get { return Table[x, y]; } set { Table[x, y] = value; } }

        public ActionTable()
        {
            Table = new ActionTypes[35, 10];
        }

        public ActionTable(ActionTypes[,] table)
        {
            Table = table;
        }

        public ActionTypes GetAction(HandInfo info)
        {
            var dealer = info.DealerHand.Cards.ElementAt(0).Rank;
            var hand = info.PlayerHands.ElementAt(info.HandToPlay);
            var rank1 = hand.Cards.ElementAt(0).Rank;
            if (hand.Cards.Count() == 2 && rank1 == hand.Cards.ElementAt(1).Rank)
                return Table[(int)rank1,(int)dealer];

            var value = hand.Value;
            var soft = hand.Soft;

            if (value >= 21)
                return ActionTypes.Stand;

            if (soft)
                return Table[value - 3, (int)dealer];

            //if (value > 17)
            //    return ActionTypes.Stand;

            return Table[value + 14, (int)dealer];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
                sb.Append("\t" + " " + SHORT_RANK_STRINGS[i]);
            sb.AppendLine();

            for (int i = 0; i < 10; i++)
            {
                sb.Append(string.Format("{0}{0}", SHORT_RANK_STRINGS[i]));
                for (int j = 0; j < 10; j++)
                    sb.Append("\t" + ACTION_STRINGS[(int)Table[i, j]]);

                sb.AppendLine();
            }

            for (int i = 0; i < 9; i++)
            {
                sb.Append(string.Format("{0}{1}", SHORT_RANK_STRINGS.Last(), SHORT_RANK_STRINGS[i]));
                for (int j = 0; j < 10; j++)
                    sb.Append("\t" + ACTION_STRINGS[(int)Table[i+10, j]]);

                sb.AppendLine();
            }

            for (int i = 5; i < 21; i++)
            {
                sb.Append(string.Format("{0}", i));
                for (int j = 0; j < 10; j++)
                    sb.Append("\t" + ACTION_STRINGS[(int)Table[i + 14, j]]);

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string[] ACTION_STRINGS =
            new string[] { "S", "H", "R/H", "R/S", "P/H", "P/S", "D/H", "D/S" };

        public static string[] SHORT_RANK_STRINGS = 
            new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "T", "A" };

        public static ActionTable FromStrategy(IBlackjackPlayer strategy)
        {
            var table = new ActionTypes[35, 10];
            List<PlayerHand> hands = new List<PlayerHand>();
            hands.Add(null);
            for (int dealer = 0; dealer < 10; dealer++)
            {
                DealerHand dealerHand = new DealerHand();
                dealerHand.AddCard(new Card((Ranks)dealer));

                for (int p = 0; p < 35; p++)
                {
                    PlayerHand playerHand = new PlayerHand()
                    {
                        Player = strategy,
                        Bet = 1
                    };

                    if (p < 10)
                    {
                        playerHand.AddCard(new Card((Ranks)p));
                        playerHand.AddCard(new Card((Ranks)p));
                    }
                    else if (p < 19)
                    {
                        playerHand.AddCard(new Card(Ranks.Ace));
                        playerHand.AddCard(new Card((Ranks)(p - 10)));
                    }
                    else if (p < 26)
                    {
                        playerHand.AddCard(new Card(Ranks.Two));
                        playerHand.AddCard(new Card((Ranks)(p - 18)));
                    }
                    else
                    {
                        playerHand.AddCard(new Card(Ranks.Ten));
                        playerHand.AddCard(new Card((Ranks)(p - 26)));
                    }
                    hands[0] = playerHand;

                    HandInfo info = new HandInfo()
                    {
                        DealerHand = dealerHand,
                        HandToPlay = 0,
                        PlayerHands = hands
                    };

                    var hs = strategy.Hit(info) ? ActionTypes.Hit : ActionTypes.Stand;
                    var type = hs;
                    if (p < 10 && strategy.Split(info))
                        type = hs == ActionTypes.Hit ? ActionTypes.SplitOrHit : ActionTypes.SplitOrStand;
                    else if (strategy.DoubleDown(info))
                        type = hs == ActionTypes.Hit ? ActionTypes.DoubleDownOrHit : ActionTypes.DoubleDownOrStand;

                    table[p, dealer] = type;
                }
                
            }

            return new ActionTable(table);
        }
    }
}
