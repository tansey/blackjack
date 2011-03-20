using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackjack;
using System.Xml.Serialization;
using System.IO;
using Blackjack.Players;

namespace ConsoleBlackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            //BlackjackSettings settings = SaveDefaultSettings();
            BlackjackSettings settings = LoadSettingsFromFile("settings.xml");
            BlackjackGame game = new BlackjackGame(settings);

            //ConsoleBlackjackPlayer player = new ConsoleBlackjackPlayer() { Game = game };
            var handsToPlay = 100000000L;
            var player = new BasicStrategyPlayer(handsToPlay);

            game.Play(new [] { player });
            Console.WriteLine("Profit: {0}%", Math.Round((player.Profit / settings.MinimumBet / (decimal)handsToPlay) * 100m, 2));
        }

        private static BlackjackSettings SaveDefaultSettings()
        {
            BlackjackSettings settings = new BlackjackSettings()
            {
                DecksPerShoe = 8,
                MinCardsDealtBeforeReshuffle = 230,

                MinimumBet = 5,
                MaximumBet = 200,

                DealerHardStandThreshold = 17,
                DealerSoftStandThreshold = 18,

                BlackjackPayoff = 2.5m,

                InsuranceOffered = true,
                InsuranceCost = 1m,
                InsurancePayoff = 2m,

                ResplitAcesAllowed = true,
                SplitTensAllowed = true,

                HittingSplitAcesAllowed = true,

                DoubleDownOnlyTenOrEleven = false,
                SoftDoubleDownAllowed = true,
                DoubleDownNonAceSplitsAllowed = true,
                DoubleDownSplitAcesAllowed = false,

                SurrenderAllowed = true,
                SurrenderPayoff = 0.5m
            };
            XmlSerializer ser = new XmlSerializer(typeof(BlackjackSettings));
            using (TextWriter writer = new StreamWriter("settings.xml"))
                ser.Serialize(writer, settings);
            return settings;
        }

        private static BlackjackSettings LoadSettingsFromFile(string file)
        {
            BlackjackSettings settings = null;
            XmlSerializer ser = new XmlSerializer(typeof(BlackjackSettings));
            using (TextReader writer = new StreamReader(file))
                settings = (BlackjackSettings)ser.Deserialize(writer);
            return settings;
        }
    }
}
