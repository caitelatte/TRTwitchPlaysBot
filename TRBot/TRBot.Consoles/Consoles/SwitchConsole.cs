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
    /// The Nintendo Switch.
    /// </summary>
    public sealed class SwitchConsole : GameConsole
    {
        public SwitchConsole()
        {
            Name = "switch";

            Initialize();

            UpdateInputRegex();
        }

        private void Initialize()
        {
            SetConsoleInputs(new Dictionary<string, InputData>(21)
            {
                { "left", InputData.CreateAxis("left", (int)GlobalAxisVals.AXIS_X, 0, -1) },
                { "right", InputData.CreateAxis("right", (int)GlobalAxisVals.AXIS_X, 0, 1) },
                { "up", InputData.CreateAxis("up", (int)GlobalAxisVals.AXIS_Y, 0, -1) },
                { "down", InputData.CreateAxis("down", (int)GlobalAxisVals.AXIS_Y, 0, 1) },
                { "rleft", InputData.CreateAxis("rleft", (int)GlobalAxisVals.AXIS_RX, 0, -1) },
                { "rright", InputData.CreateAxis("rright", (int)GlobalAxisVals.AXIS_RX, 0, 1) },
                { "rup", InputData.CreateAxis("rup", (int)GlobalAxisVals.AXIS_RY, 0, -1) },
                { "rdown", InputData.CreateAxis("rdown", (int)GlobalAxisVals.AXIS_RY, 0, 1) },

                { "a", InputData.CreateButton("a", (int)GlobalButtonVals.BTN1) },
                { "b", InputData.CreateButton("b", (int)GlobalButtonVals.BTN2) },
                { "x", InputData.CreateButton("x", (int)GlobalButtonVals.BTN3) },
                { "y", InputData.CreateButton("y", (int)GlobalButtonVals.BTN4) },
                { "l", InputData.CreateButton("l", (int)GlobalButtonVals.BTN5) },
                { "r", InputData.CreateButton("r", (int)GlobalButtonVals.BTN6) },
                { "zl", InputData.CreateButton("zl", (int)GlobalButtonVals.BTN7) },
                { "zr", InputData.CreateButton("zr", (int)GlobalButtonVals.BTN8) },
                { "minus", InputData.CreateButton("minus", (int)GlobalButtonVals.BTN9) },
                { "plus", InputData.CreateButton("plus", (int)GlobalButtonVals.BTN10) },
                { "dleft", InputData.CreateButton("dleft", (int)GlobalButtonVals.BTN11) },
                { "dright", InputData.CreateButton("dright", (int)GlobalButtonVals.BTN12) },
                { "dup", InputData.CreateButton("dup", (int)GlobalButtonVals.BTN13) },
                { "ddown", InputData.CreateButton("ddown", ((int)GlobalButtonVals.BTN14)) },
                { "ls", InputData.CreateButton("ls", (int)GlobalButtonVals.BTN15) },
                { "rs", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN16) },

                { "#", InputData.CreateBlank("#") },

                //Spare buttons
                { "sb1", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN17) },
                { "sb2", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN18) },
                { "sb3", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN19) },
                { "sb4", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN20) },
                { "sb5", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN21) },
                { "sb6", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN22) },
                { "sb7", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN23) },
                { "sb8", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN24) },
                { "sb9", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN25) },
                { "sb10", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN26) },
                { "sb11", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN27) },
                { "sb12", InputData.CreateButton("rs", (int)GlobalButtonVals.BTN28) },
            });
        }
    }
}
