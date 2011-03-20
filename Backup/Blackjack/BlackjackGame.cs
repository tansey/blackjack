using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack
{
    public class BlackjackGame
    {
        public BlackjackSettings Settings { get; set; }

        private const int SHUFFLES_PER_SHOE = 10;
        private Shoe _shoe;

        public BlackjackGame(BlackjackSettings settings)
        {
            Settings = settings;
            _shoe = new Shoe(settings.DecksPerShoe);
        }

        public void Play(IEnumerable<IBlackjackPlayer> players)
        {
            _shoe.Shuffle(SHUFFLES_PER_SHOE);
            bool[] done = new bool[players.Count()];

            while (done.Count(d => d == false) > 0)
            {
                if (_shoe.CardsDealt >= Settings.MinCardsDealtBeforeReshuffle)
                    _shoe.Shuffle(SHUFFLES_PER_SHOE);

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
                            hands[i].Won = Settings.SurrenderPayoff * hands[i].Bet;
                            hands[i].Finished = true;
                            continue;
                        }
                    }

                    //split loop
                    if (CanSplit(hand) && hand.Player.Split(info))
                    {
                        var secondHand = hand.Split();
                        hand.AddCard(_shoe.NextCard());
                        secondHand.AddCard(_shoe.NextCard());

                        hands.Insert(i+1, secondHand);
                        i--;
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
                    while (CanHit(hand) && hand.Player.Hit(info))
                    {
                        hand.AddCard(_shoe.NextCard());
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

                foreach (var hand in hands)
                    hand.Player.Profit += hand.Won - hand.Bet;

                for (int i = 0; i < done.Length; i++)
                    if (!done[i])
                        players.ElementAt(i).HandOver(info);

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
