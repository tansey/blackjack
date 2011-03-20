using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackjack;

namespace ConsoleBlackjack
{
    public class ConsoleBlackjackPlayer : IBlackjackPlayer
    {
        private int[] counts = new int[10];

        public BlackjackGame Game { get; set; }
        public decimal Profit { get; set; }
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
            //return YesNoQuery("Play another hand?");

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
            //if (nextAction == "i")
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
            foreach (var hand in info.PlayerHands)
                handStr.Add(hand.ToString());

            StringBuilder bets = new StringBuilder(), player = new StringBuilder(), playerHand = new StringBuilder();
            int i = 0;
            foreach (var hand in info.PlayerHands)
            {
                if (i == info.HandToPlay)
                    player.Append("Player".PadRight(handStr[i].Length + 2, ' '));
                else
                    player.Append("".PadRight(handStr[i].Length + 2, ' '));
                playerHand.Append(string.Format("({0})", hand.Value).PadLeft(4).PadRight(handStr[i].Length + 2, ' '));
                bets.Append(string.Format("${0} BET", hand.Bet).PadRight(handStr[i].Length + 2, ' '));
                //Console.WriteLine("${0}: {1}", hand.Bet, hand.ToString());
                i++;
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
