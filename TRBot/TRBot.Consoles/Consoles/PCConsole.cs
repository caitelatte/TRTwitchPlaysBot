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
    /// A desktop computer.
    /// </summary>
    public sealed class PCConsole : GameConsole
    {
        public PCConsole()
        {
            Identifier = "PC";

            Initialize();

            UpdateInputRegex();
        }

        private void Initialize()
        {
            ValidInputs = new List<string>(18)
            {
                "left", "right", "up", "down",
                "lclick", "mclick", "rclick",
                "return", "space",
                "q", "w", "e", "r", "a", "s", "d", "p",
                "#"
            };

            InputAxes = new Dictionary<string, InputAxis>(4)
            {
                { "left", new InputAxis((int)GlobalAxisVals.AXIS_X, 0, -1) },
                { "right", new InputAxis((int)GlobalAxisVals.AXIS_X, 0, 1) },
                { "up", new InputAxis((int)GlobalAxisVals.AXIS_Y, 0, -1) },
                { "down", new InputAxis((int)GlobalAxisVals.AXIS_Y, 0, 1) }
            };

            ButtonInputMap = new Dictionary<string, InputButton>(17)
            {
                { "left", new InputButton((int)GlobalButtonVals.BTN1) },
                { "right", new InputButton((int)GlobalButtonVals.BTN2) },
                { "up", new InputButton((int)GlobalButtonVals.BTN3) },
                { "down", new InputButton((int)GlobalButtonVals.BTN4) },
                { "lclick", new InputButton((int)GlobalButtonVals.BTN5) },
                { "mclick", new InputButton((int)GlobalButtonVals.BTN6) },
                { "rclick", new InputButton((int)GlobalButtonVals.BTN7) },
                { "return", new InputButton((int)GlobalButtonVals.BTN8) },
                { "space", new InputButton((int)GlobalButtonVals.BTN9) },
                { "q", new InputButton((int)GlobalButtonVals.BTN10) },
                { "w", new InputButton((int)GlobalButtonVals.BTN11) },
                { "e", new InputButton((int)GlobalButtonVals.BTN12) },
                { "r", new InputButton((int)GlobalButtonVals.BTN13) },
                { "a", new InputButton((int)GlobalButtonVals.BTN14) },
                { "s", new InputButton((int)GlobalButtonVals.BTN15) },
                { "d", new InputButton((int)GlobalButtonVals.BTN16) },
                { "p", new InputButton((int)GlobalButtonVals.BTN17) }
            };
        }
    }
}
