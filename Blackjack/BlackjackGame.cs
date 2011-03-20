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
using System.IO;

namespace Blackjack
{
    /// <summary>
    /// A Blackjack game engine class. This class enables the user to simulate
    /// a potentially-infinite number of blackjack hands played by a collection
    /// of players.
    /// </summary>
    public class BlackjackGame
    {
        /// <summary>
        /// The settings for this game.
        /// </summary>
        public BlackjackSettings Settings { get; set; }

        private const int SHUFFLES_PER_SHOE = 10;
        private Shoe _shoe;

        /// <summary>
        /// Creates a new instance of a game of Blackjack.
        /// </summary>
        public BlackjackGame(BlackjackSettings settings)
        {
            Settings = settings;
            _shoe = new Shoe(settings.DecksPerShoe);
        }

        /// <summary>
        /// Plays a game of Blackjack with the collection of players.
        /// </summary>
        public void Play(IEnumerable<IBlackjackPlayer> players)
        {
            _shoe.Shuffle(SHUFFLES_PER_SHOE);
            foreach (var player in players)
                player.Reshuffle();
            bool[] done = new bool[players.Count()];

            while (done.Count(d => d == false) > 0)
            {
                if (_shoe.CardsDealt >= Settings.MinCardsDealtBeforeReshuffle)
                {
                    _shoe.Shuffle(SHUFFLES_PER_SHOE);
                    foreach (var player in players)
                        player.Reshuffle();
                }

                //get bets
                List<PlayerHand> hands = new List<PlayerHand>();
                for (int i = 0; i < players.Count(); i++)
                {
                    if (done[i])
                        continue;
                    var player = players.ElementAt(i);
                    bool play = player.PlayAnotherHand();

                    if (!play)
                    {
                        done[i] = true;
                        continue;
                    }

                    var bet = player.GetBet(Settings.MinimumBet, Settings.MaximumBet);
                    if (bet < Settings.MinimumBet)
                        bet = Settings.MinimumBet;
                    else if (bet > Settings.MaximumBet)
                        bet = Settings.MaximumBet;


                    hands.Add(new PlayerHand()
                    {
                        Player = player,
                        Bet = bet
                    });
                }

                //deal cards
                foreach (var hand in hands)
                {
                    hand.AddCard(_shoe.NextCard());
                    hand.AddCard(_shoe.NextCard());
                }
                DealerHand dealerHand = new DealerHand();
                dealerHand.HiddenCard = _shoe.NextCard();
                dealerHand.AddCard(_shoe.NextCard());

                HandInfo info = new HandInfo()
                {
                    DealerHand = dealerHand,
                    PlayerHands = hands
                };

                try
                {
                    #region offer insurance
                    if (Settings.InsuranceOffered && dealerHand.Cards.ElementAt(0).Rank == Ranks.Ace)
                    {
                        for (int i = 0; i < hands.Count; i++)
                        {
                            if (hands[i].Finished)
                                continue;

                            info.HandToPlay = i;
                            if (hands[i].Player.BuyInsurance(info))
                            {
                                hands[i].Insured = true;
                                hands[i].Won -= hands[i].Bet * Settings.InsuranceCost;
                            }
                        }

                        //payoff insurance --> everyone else loses
                        if (dealerHand.HiddenCard.HighValue == 10)
                        {
                            dealerHand.FlipHiddenCard();

                            foreach (var hand in hands)
                            {
                                if (hand.Insured)//payoff insurance bets
                                    hand.Won += hand.Bet * Settings.InsuranceCost * Settings.InsurancePayoff;
                                if (hand.Value == 21)//blackjack pushes
                                    hand.Won += hand.Bet;
                                hand.Finished = true;//game's over
                            }
                        }
                    }

                    #endregion

                    //payoff blackjacks --> remove from hand
                    foreach (var hand in hands)
                    {
                        if (hand.Finished)
                            continue;

                        if (hand.Value == 21)
                        {
                            hand.Won += hand.Bet * Settings.BlackjackPayoff;
                            hand.Finished = true;
                        }
                    }

                    //each player acts
                    for (int i = 0; i < hands.Count; i++)
                    {
                        if (hands[i].Finished)
                            continue;

                        info.HandToPlay = i;
                        var hand = hands[i];

                        //accept surrenders
                        if (CanSurrender(hand))
                        {
                            if (hand.Player.Surrender(info))
                            {
                                hand.Won = Settings.SurrenderPayoff * hand.Bet;
                                hand.Finished = true;
                                continue;
                            }
                        }

                        //split loop
                        if (CanSplit(hand) && hand.Player.Split(info))
                        {
                            var secondHand = hand.Split();
                            hand.AddCard(_shoe.NextCard());
                            secondHand.AddCard(_shoe.NextCard());

                            hands.Insert(i + 1, secondHand);
                            i--;
                            hand.Player.Splits++;
                            continue;
                        }

                        //double down
                        if (CanDoubleDown(hand) && hand.Player.DoubleDown(info))
                        {
                            hand.Bet *= 2;
                            hand.AddCard(_shoe.NextCard());
                            if (hand.Value > 21)
                                hand.Finished = true;
                            continue;
                        }

                        //hit/stand loop
                        while (!hand.Finished && CanHit(hand) && hand.Player.Hit(info))
                        {
                            var next = _shoe.NextCard();
                            hand.AddCard(next);

                            if (hand.Value > 21)
                                hand.Finished = true;
                        }
                    }

                    //dealer acts (only if everyone hasn't busted or hit BJ)
                    if (hands.Count(h => !h.Finished) > 0)
                    {
                        dealerHand.FlipHiddenCard();
                        while (DealerMustHit(dealerHand))
                            dealerHand.AddCard(_shoe.NextCard());

                        //if the dealer busted, everyone wins
                        if (dealerHand.Value > 21)
                        {
                            foreach (var hand in hands)
                                if (!hand.Finished)
                                    hand.Won += hand.Bet * 2;
                        }
                        else
                        {
                            //if the dealer didn't bust, then players win if they're closer to 21
                            foreach (var hand in hands)
                                if (!hand.Finished)
                                {
                                    if (hand.Value > dealerHand.Value)
                                        hand.Won += hand.Bet * 2;
                                    else if (hand.Value == dealerHand.Value)
                                        hand.Won += hand.Bet;
                                }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogError(info);
                    throw new Exception("", e);
                }

                foreach (var hand in hands)
                    hand.Player.Profit += hand.Won - hand.Bet;

                for (int i = 0; i < done.Length; i++)
                    if (!done[i])
                    {
                        players.ElementAt(i).HandOver(info);
                        players.ElementAt(i).Splits = 0;
                    }


            }

        }

        private static void LogError(HandInfo info)
        {
            using (TextWriter log = new StreamWriter("error.txt"))
            {
                log.WriteLine("Hand:");
                var dealer = info.DealerHand;

                log.WriteLine();
                log.WriteLine("     Dealer ({0})", dealer.Value);
                log.WriteLine("      {0}", dealer.ToString());
                log.WriteLine();

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

                log.WriteLine("     " + bets.ToString());
                log.WriteLine();
                log.Write("     ");
                foreach (var str in handStr)
                    log.Write(str + "  ");
                log.WriteLine();
                log.WriteLine("     " + player.ToString());
                log.WriteLine("     " + playerHand.ToString());
                log.WriteLine();
            }
        }

        public bool CanSurrender(PlayerHand hand)
        {
            if (hand.Finished)
                return false;
            if (hand.Cards.Count() != 2)
                return false;
            return Settings.SurrenderAllowed && !hand.HasBeenSplit;
        }

        public bool CanSplit(PlayerHand hand)
        {
            if (hand.Finished)
                return false;

            if (hand.Player.Splits >= Settings.MaxSplitsAllowed)
                return false;

            if (hand.Cards.Count() != 2)
                return false;

            var c = hand.Cards.ElementAt(0);
            if (c.HighValue != hand.Cards.ElementAt(1).HighValue)
                return false;

            if (!Settings.ResplitAcesAllowed && c.Rank == Ranks.Ace && hand.HasBeenSplit)
                return false;

            if (!Settings.SplitTensAllowed && c.HighValue == 10)
                return false;

            return true;
        }

        public bool CanDoubleDown(PlayerHand hand)
        {
            if (hand.Finished)
                return false;

            if (hand.Cards.Count() != 2)
                return false;

            if (hand.HasBeenSplit)
            {
                if (hand.Cards.ElementAt(0).Rank == Ranks.Ace)
                {
                    if (!Settings.DoubleDownSplitAcesAllowed)
                        return false;
                }
                else if (!Settings.DoubleDownNonAceSplitsAllowed)
                    return false;
            }

            var c1 = hand.Cards.ElementAt(0);
            var c2 = hand.Cards.ElementAt(1);

            if(!Settings.SoftDoubleDownAllowed && (c1.Rank == Ranks.Ace || c2.Rank == Ranks.Ace))
                return false;

            int low = c1.LowValue + c2.LowValue;
            if (Settings.DoubleDownOnlyTenOrEleven && (low < 10 || low > 11))
                return false;

            return true;
        }

        public bool CanHit(PlayerHand hand)
        {
            if (hand.Finished)
                return false;

            if (hand.Value > 21)
                return false;

            //check if the hand split was two aces and if the player is not
            //allowed to hit split aces
            if (hand.HasBeenSplit && !Settings.HittingSplitAcesAllowed && hand.Cards.ElementAt(0).Rank == Ranks.Ace)
                return false;

            return true;
        }

        public bool DealerMustHit(DealerHand hand)
        {
            if(hand.Soft)
                return hand.Value < Settings.DealerSoftStandThreshold;

            return hand.Value < Settings.DealerHardStandThreshold;
        }
    }
}
