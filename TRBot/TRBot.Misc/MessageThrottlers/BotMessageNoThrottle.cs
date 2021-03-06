﻿/* Copyright (C) 2019-2020 Thomas "Kimimaru" Deeb
 * 
 * This file is part of TRBot,software for playing games through text.
 *
 * TRBot is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * TRBot is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with TRBot.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Text;
using TRBot.Connection;
using TRBot.Utilities;

namespace TRBot.Misc
{
    /// <summary>
    /// Sends messages without any throttling.
    /// </summary>
    public class BotMessageNoThrottle : BotMessageThrottler
    {
        public BotMessageNoThrottle()
        {
            
        }

        public override void Update(in DateTime nowUTC, BotMessageHandler botMsgHandler)
        {
            //Send all the messages in the queue until we hit our limit
            while (botMsgHandler.ClientMessageCount > 0)
            {
                //Return if sending the message fails
                if (botMsgHandler.SendNextQueuedMessage() == false)
                {
                    return;
                }
            }
        }
    }
}
