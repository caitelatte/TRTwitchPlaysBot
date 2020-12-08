﻿/* This file is part of TRBot.
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
using TwitchLib.Client.Events;

namespace TRBot
{
    /// <summary>
    /// Shows the contents of a macro, provided it exists.
    /// </summary>
    public sealed class ShowCommand : BaseCommand
    {
        public override void ExecuteCommand(EvtChatCommandArgs e)
        {
            List<string> args = e.Command.ArgumentsAsList;

            if (args.Count != 1)
            {
                BotProgram.MsgHandler.QueueMessage($"{Globals.CommandIdentifier}show usage: \"macroname\"");
                return;
            }

            string macroName = args[0].ToLowerInvariant();

            if (BotProgram.BotData.Macros.TryGetValue(macroName, out string macroVal) == false)
            {
                BotProgram.MsgHandler.QueueMessage($"{macroName} not found. For dynamic macros, use the generic form (Ex. \"#test(*)\")");
                return;
            }

            BotProgram.MsgHandler.QueueMessage($"{macroName} = {macroVal}");
        }
    }
}
