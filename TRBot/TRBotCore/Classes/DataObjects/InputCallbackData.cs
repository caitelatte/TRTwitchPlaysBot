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
using System.Collections.Concurrent;
using System.Text;

namespace TRBot
{
    /// <summary>
    /// Data for input callbacks.
    /// </summary>
    public class InputCallbackData
    {
        public readonly Dictionary<string, InputCBWrapper> CallbackData = null;

        [Newtonsoft.Json.JsonIgnore]
        public Dictionary<string, InputCallback> Callbacks = new Dictionary<string, InputCallback>(16);

        public InputCallbackData()
        {
            CallbackData = new Dictionary<string, InputCBWrapper>(16);

            CallbackData["ss1"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.SavestateLog, 1);
            CallbackData["ss2"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.SavestateLog, 2);
            CallbackData["ss3"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.SavestateLog, 3);
            CallbackData["ss4"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.SavestateLog, 4);
            CallbackData["ss5"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.SavestateLog, 5);
            CallbackData["ss6"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.SavestateLog, 6);
            
            CallbackData["ss"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.SavestateSlotLog, null);
            CallbackData["incs"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.ChangeStateSlot, 1);
            CallbackData["decs"] = new InputCBWrapper(InputCBInvocation.Press | InputCBInvocation.Hold, InputCBTypes.ChangeStateSlot, -1);
        }

        public void PopulateCBWithData()
        {
            if (Callbacks == null)
            {
                int capacity = 16;
                if (CallbackData != null)
                {
                    capacity = CallbackData.Count;
                }

                Callbacks = new Dictionary<string, InputCallback>(capacity);
            }

            Callbacks.Clear();

            //Return if the callback data is null
            if (CallbackData == null)
            {
                return;
            }

            foreach (KeyValuePair<string, InputCBWrapper> kvPair in CallbackData)
            {
                string inputName = kvPair.Key;
                InputCBWrapper cbWrapper = kvPair.Value;

                Callbacks[inputName] = new InputCallback(inputName, cbWrapper.Invocation, cbWrapper.CBValue,
                    InputCallback.GetCallbackForCBType(cbWrapper.CBType));
            }
        }

        public struct InputCBWrapper
        {
            public InputCBInvocation Invocation;
            public InputCBTypes CBType;
            
            /// <summary>
            /// The value associated with the callback.
            /// </summary>
            public object CBValue;

            public InputCBWrapper(in InputCBInvocation invocation, in InputCBTypes cbType, object cbValue)
            {
                Invocation = invocation;
                CBType = cbType;
                CBValue = cbValue;
            }

            public override bool Equals(object obj)
            {
                if (obj is InputCBWrapper inputCBWrap)
                {
                    return (this == inputCBWrap);
                }

                return false;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 31;
                    hash = (hash * 37) + Invocation.GetHashCode();
                    hash = (hash * 37) + CBType.GetHashCode();
                    hash = (hash * 37) + ((CBValue == null) ? 0 : CBValue.GetHashCode());
                    return hash;
                } 
            }

            public static bool operator==(InputCBWrapper a, InputCBWrapper b)
            {
                return (a.Invocation == b.Invocation && a.CBType == b.CBType && a.CBValue == b.CBValue);
            }

            public static bool operator!=(InputCBWrapper a, InputCBWrapper b)
            {
                return !(a == b);
            }
        }
    }
}
