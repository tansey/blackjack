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
using Blackjack;

namespace ConsoleBlackjack
{
    /// <summary>
    /// A helper class to let a human play Blackjack
    /// via the console.
    /// </summary>
    public class ConsoleBlackjackPlayer : IBlackjackPlayer
    {
        private int[] counts = new int[10];

        public BlackjackGame Game { get; set; }
        public decimal Profit { get; set; }
        public int Splits { get; set; }
        private decimal bankroll = 200;
        private bool YesNoQuery(string query)
        {
            string line = "";
            while (line != "y" && line != "n")
            {
                Console.Write(query + " ");
                line = Console.ReadLine().ToLower();
            }
            return line == "y";
        }

        private string nextAction = "";
        private void MultiQuery(HandInfo info)
        {
            PrintHand(info);
            Console.WriteLine();
            PrintCount(info);
            Console.WriteLine();
            bool canSurrender = Game.CanSurrender(info.PlayerHands.ElementAt(info.HandToPlay));
            bool canSplit = Game.CanSplit(info.PlayerHands.ElementAt(info.HandToPlay));
            bool canDoubleDown = Game.CanDoubleDown(info.PlayerHands.ElementAt(info.HandToPlay));
            while (nextAction != "h" 
                && nextAction != "s"
                && (nextAction != "r" || !canSurrender)
                && (nextAction != "p" || !canSplit)
                && (nextAction != "d" || !canDoubleDown))
            {
                Console.Write("[h=hit, s=stand");
                if(canSurrender)
                    Console.Write(", r=surrender");
                if(canSplit)
                    Console.Write(", p=split");
                if(canDoubleDown)
                    Console.Write(", d=double down");
                Console.WriteLine("]");
                Console.Write("Action? ");
                nextAction = Console.ReadLine().ToLower();
            }
        }

        #region IBlackjackPlayer Members

        public bool PlayAnotherHand()
        {
            printedFinishedHand = false;
            //return YesNoQuery("Play another h?");

            Console.WriteLine("Press any key to play next hand.");
            Console.ReadKey(true);
            return true;
        }

        public decimal GetBet(decimal min, decimal max)
        {
            Console.Clear();
            PrintCount(counts);
            string line = "x";
            int bet;
            //Console.WriteLine("Current Profit: {0}${1}", Profit < 0 ? "-" : "", Math.Abs(Profit));
            Console.WriteLine("Current Bankroll: {0}${1}", bankroll < 0 ? "-" : "", Math.Abs(bankroll));
            while (!int.TryParse(line, out bet))
            {
                Console.Write("Bet (min={0} max={1}): ", min, max);
                line = Console.ReadLine();
            } 
            return bet;
        }

        public bool Split(HandInfo info)
        {
            if (nextAction == "")
                MultiQuery(info);
            if (nextAction == "p")
            {
                nextAction = "";
                return true;
            }
            return false;
        }

        public bool DoubleDown(HandInfo info)
        {
            //PrintHand(info);
            //return YesNoQuery(string.Format("Double Down for {0} more?", info.PlayerHands.ElementAt(info.HandToPlay).Bet));

            if (nextAction == "")
                MultiQuery(info);
            if (nextAction == "d")
            {
                nextAction = "";
                return true;
            }
            return false;
        }

        public bool Hit(HandInfo info)
        {
            //PrintHand(info);
            //return YesNoQuery("Hit?");

            if (nextAction == "")
                MultiQuery(info);
            if (nextAction == "h")
            {
                nextAction = "";
                return true;
            }
            if (nextAction == "s")
            {
                nextAction = "";
                return false;
            }
            return false;
        }

        public bool BuyInsurance(HandInfo info)
        {
            PrintCount(info);
            PrintHand(info);
            
            return YesNoQuery("Buy Insurance?");
            //if (nextAction == "")
            //    MultiQuery(info);
            //if (nextAction == "temp")
            //{
            //    nextAction = "";
            //    return true;
            //}
            //return false;
        }

        public bool Surrender(HandInfo info)
        {
            //PrintHand(info);
            //return YesNoQuery("Surrender?");

            if (nextAction == "")
                MultiQuery(info);
            if (nextAction == "r")
            {
                nextAction = "";
                return true;
            }
            return false;
        }

        bool printedFinishedHand = false;
        public void HandOver(HandInfo info)
        {
            if (printedFinishedHand)
                return;
            decimal won = 0, bet = 0;
            for (int i = 0; i < info.PlayerHands.Count(); i++)
            {
                var hand = info.PlayerHands.ElementAt(i);
                if (hand.Player != this)
                    continue;

                won += hand.Won;
                bet += hand.Bet;
            }
            var profit = won - bet;
            bankroll += profit;

            PrintHand(info);
            //Console.WriteLine("You won ${0} ({1}${2} profit)", won, profit < 0 ? "-" : "", Math.Abs(profit));
            if (profit > 0)
                Console.WriteLine("You Won!");
            else if (profit < 0)
                Console.WriteLine("You Lost.");
            else
                Console.WriteLine("You Pushed.");
            printedFinishedHand = true;
            UpdateCount(info);
        }

        public void Reshuffle()
        {
        }
        #endregion

        private void PrintHand(HandInfo info)
        {
            Console.Clear();
            var dealer = info.DealerHand;

            Console.WriteLine("Current Bankroll: {0}${1}", bankroll < 0 ? "-" : "", Math.Abs(bankroll));
            Console.WriteLine();
            Console.WriteLine("     Dealer ({0})", dealer.Value);
            Console.WriteLine("      {0}", dealer.ToString());
            Console.WriteLine();

            List<string> handStr = new List<string>();
            foreach (var h in info.PlayerHands)
                handStr.Add(h.ToString());

            StringBuilder bets = new StringBuilder(), player = new StringBuilder(), playerHand = new StringBuilder();
            int temp = 0;
            foreach (var h in info.PlayerHands)
            {
                if (temp == info.HandToPlay)
                    player.Append("Player".PadRight(handStr[temp].Length + 2, ' '));
                else
                    player.Append("".PadRight(handStr[temp].Length + 2, ' '));
                playerHand.Append(string.Format("({0})", h.Value).PadLeft(4).PadRight(handStr[temp].Length + 2, ' '));
                bets.Append(string.Format("${0} BET", h.Bet).PadRight(handStr[temp].Length + 2, ' '));
                temp++;
            }

            Console.WriteLine("     " + bets.ToString());
            Console.WriteLine();
            Console.Write("     ");
            foreach (var str in handStr)
                Console.Write(str + "  ");
            Console.WriteLine();
            Console.WriteLine("     " + player.ToString());
            Console.WriteLine("     " + playerHand.ToString());
            Console.WriteLine();
        }

        private void PrintCount(HandInfo info)
        {
            int[] temp = new int[10];
            counts.CopyTo(temp, 0);
            foreach (var hand in info.PlayerHands)
                foreach (var card in hand.Cards)
                    temp[(int)card.Rank]++;

            foreach (var card in info.DealerHand.Cards)
                temp[(int)card.Rank]++;

            PrintCount(temp);
        }

        private static void PrintCount(int[] temp)
        {
            int count = 0;
            Console.WriteLine("Cards Dealt");
            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                    count += temp[i];
                else if (i > 7)
                    count -= temp[i];

                Console.WriteLine("{0}: {1}", Card.RANK_STRINGS[i], temp[i]);
            }
            Console.WriteLine("Count: {0}", count);
        }


        private void UpdateCount(HandInfo info)
        {
            foreach (var hand in info.PlayerHands)
                foreach (var card in hand.Cards)
                    counts[(int)card.Rank]++;

            foreach (var card in info.DealerHand.Cards)
                counts[(int)card.Rank]++;
        }
    }
}
