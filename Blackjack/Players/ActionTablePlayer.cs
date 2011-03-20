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

namespace Blackjack.Players
{
    /// <summary>
    /// A helper strategy that just plays according to a
    /// look-up table.
    /// </summary>
    public class ActionTablePlayer : IBlackjackPlayer
    {
        public ActionTable Table { get; set; }
        public bool Print { get; set; }
        private long handsToPlay;
        private long handsPlayed;

        public ActionTablePlayer(ActionTable table, long handsToPlay)
        {
            this.handsToPlay = handsToPlay;
            this.Table = table;
        }

        #region IBlackjackPlayer Members

        public decimal Profit { get; set; }
        public int Splits { get; set; }
        public bool PlayAnotherHand()
        {
            return handsToPlay > handsPlayed;
        }

        public decimal GetBet(decimal min, decimal max)
        {
            return min;
        }

        public bool Split(HandInfo info)
        {
            var action = Table.GetAction(info);
            return action == ActionTable.ActionTypes.SplitOrHit ||
                action == ActionTable.ActionTypes.SplitOrStand;
        }

        public bool DoubleDown(HandInfo info)
        {
            var action = Table.GetAction(info);
            return action == ActionTable.ActionTypes.DoubleDownOrHit ||
                action == ActionTable.ActionTypes.DoubleDownOrStand;            
        }

        public bool Hit(HandInfo info)
        {
            var action = Table.GetAction(info);
            return action == ActionTable.ActionTypes.SplitOrHit ||
                action == ActionTable.ActionTypes.DoubleDownOrHit ||
                action == ActionTable.ActionTypes.Hit ||
                action == ActionTable.ActionTypes.SurrenderOrHit;
        }

        public bool BuyInsurance(HandInfo info)
        {
            return false;
        }

        public bool Surrender(HandInfo info)
        {
            var action = Table.GetAction(info);
            return action == ActionTable.ActionTypes.SurrenderOrHit ||
                action == ActionTable.ActionTypes.SurrenderOrStand;
        }

        public void HandOver(HandInfo info)
        {
            handsPlayed++;
            if (Print && handsPlayed % 100000 == 0)
                Console.WriteLine(handsPlayed);
        }

        public void Reshuffle()
        {
        }

        #endregion
    }
}
