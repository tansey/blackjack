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
using System.Xml.Serialization;
using System.IO;
using Blackjack.Players;

namespace ConsoleBlackjack
{
class Program
{
    static void Main(string[] args)
    {
        BlackjackSettings settings = LoadSettingsFromFile("settings.xml");
        BlackjackGame game = new BlackjackGame(settings);

        var handsToPlay = 100000000L;

        //BasicStrategyPlayer basic = new BasicStrategyPlayer(handsToPlay);
        //var table = ActionTable.FromStrategy(basic);

        //var player = basic;
        //var player = new ConsoleBlackjackPlayer() { Game = game };
        //var player = new WizardSimpleStrategy(handsToPlay);
        //var player = new ActionTablePlayer(table, handsToPlay) { Print = true };
        var player = new SimpleFiveCountPlayer(handsToPlay);

        game.Play(new [] { player });
        Console.WriteLine("Profit: {0:N2}%", 
            player.Profit / settings.MinimumBet / (decimal)handsToPlay * 100m);
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
