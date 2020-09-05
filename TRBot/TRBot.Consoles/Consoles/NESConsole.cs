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
using TRBot.Parsing;
using TRBot.VirtualControllers;

namespace TRBot.Consoles
{
    /// <summary>
    /// The NES, or Famicom.
    /// </summary>
    public sealed class NESConsole : GameConsole
    {
        public NESConsole()
        {
            Identifier = "NES";

            Initialize();

            UpdateInputRegex();
        }

        private void Initialize()
        {
            InputAxes = new Dictionary<string, InputAxis>();

            ButtonInputMap = new Dictionary<string, InputButton>(20)
            {
                { "left", new InputButton((int)GlobalButtonVals.BTN1) },
                { "right", new InputButton((int)GlobalButtonVals.BTN2) },
                { "up", new InputButton((int)GlobalButtonVals.BTN3) },
                { "down", new InputButton((int)GlobalButtonVals.BTN4) },
                { "a", new InputButton((int)GlobalButtonVals.BTN5) },
                { "b", new InputButton((int)GlobalButtonVals.BTN6) },
                { "select", new InputButton((int)GlobalButtonVals.BTN7) },
                { "start", new InputButton((int)GlobalButtonVals.BTN8) },
                { "ss1", new InputButton((int)GlobalButtonVals.BTN19) },
                { "ss2", new InputButton((int)GlobalButtonVals.BTN20) },
                { "ss3", new InputButton((int)GlobalButtonVals.BTN21) },
                { "ss4", new InputButton((int)GlobalButtonVals.BTN22) },
                { "ss5", new InputButton((int)GlobalButtonVals.BTN23) },
                { "ss6", new InputButton((int)GlobalButtonVals.BTN24) },
                { "ls1", new InputButton((int)GlobalButtonVals.BTN25) },
                { "ls2", new InputButton((int)GlobalButtonVals.BTN26) },
                { "ls3", new InputButton((int)GlobalButtonVals.BTN27) },
                { "ls4", new InputButton((int)GlobalButtonVals.BTN28) },
                { "ls5", new InputButton((int)GlobalButtonVals.BTN29) },
                { "ls6", new InputButton((int)GlobalButtonVals.BTN30) },
            };

            ValidInputs = new List<string>(21)
            {
                "up", "down", "left", "right", "a", "b", "select", "start",
                "ss1", "ss2", "ss3", "ss4", "ss5", "ss6",
                "ls1", "ls2", "ls3", "ls4", "ls5", "ls6",
                "#"
            };
        }
    }
}