﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TRBot
{
    /// <summary>
    /// Post-processes inputs from the parser.
    /// </summary>
    public static class ParserPostProcess
    {
        public static bool ValidateButtonCombos(List<List<Parser.Input>> inputs, List<string> invalidCombo)
        {
            List<string> currentCombo = new List<string>(invalidCombo.Count);
            List<string> subCombo = new List<string>(invalidCombo.Count);
            
            for (int i = 0; i < inputs.Count; i++)
            {
                List<Parser.Input> inputList = inputs[i];
                subCombo.Clear();
                
                for (int j = 0; j < inputList.Count; j++)
                {
                    Parser.Input input = inputList[j];
                    
                    if (invalidCombo.Contains(input.name) == true)
                    {
                        if (input.release == false && subCombo.Contains(input.name) == false)
                        {
                            subCombo.Add(input.name);
                            
                            if ((subCombo.Count + currentCombo.Count) == invalidCombo.Count)
                            {
                                return false;
                            }
                        }
                        
                        if (input.hold == true)
                        {
                            if (currentCombo.Contains(input.name) == false)
                            {
                                currentCombo.Add(input.name);
                                
                                if (currentCombo.Count == invalidCombo.Count)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (input.release == true)
                        {
                            currentCombo.Remove(input.name);
                        }
                    }
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Validates how long the pause input is held for. This is to prevent resetting the game for certain inputs.
        /// </summary>
        /// <param name="parsedInputs"></param>
        /// <param name="pauseInput"></param>
        /// <param name="maxPauseDuration"></param>
        /// <returns></returns>
        public static bool IsValidPauseInputDuration(List<List<Parser.Input>> parsedInputs, in string pauseInput, in int maxPauseDuration)
        {
            int curPauseDuration = 0;
            bool held = false;
            
            //Check for pause duration
            if (maxPauseDuration >= 0)
            {
                for (int i = 0; i < parsedInputs.Count; i++)
                {
                    bool pauseFound = false;
                    int longestSubInput = 0;
                    int longestPauseDur = 0;
                    
                    List<Parser.Input> inputList = parsedInputs[i];
                    for (int j = 0; j < inputList.Count; j++)
                    {
                        Parser.Input input = inputList[j];

                        //Check for longest subduration
                        if (input.duration > longestSubInput)
                        {
                            longestSubInput = input.duration;
                        }

                        //We found the pause input
                        if (input.name == pauseInput)
                        {
                            //Check for longest duration of this input (Ex. "start+start1s")
                            if (input.duration > longestPauseDur)
                            {
                                longestPauseDur = input.duration;
                            }
                            
                            pauseFound = true;
                            
                            //Release
                            if (input.release == true)
                            {
                                held = false;
                                pauseFound = false;
                            }
                            //Hold if not held
                            else if (held == false && input.hold == true)
                            {
                                held = true;
                            }
                        }
                    }
                    
                    //If held or found, add to the total duration
                    if (pauseFound == true || held == true)
                    {
                        //If not held, add only the longest pause duration
                        if (held == false)
                        {
                            curPauseDuration += longestPauseDur;
                        }
                        //If held, add the longest subduration
                        else
                        {
                            curPauseDuration += longestSubInput;
                        }
                        
                        //Invalid if over the max duration
                        if (curPauseDuration > maxPauseDuration)
                        {
                            return false;
                        }
                    }
                    //Otherwise reset
                    else
                    {
                        curPauseDuration = 0;
                    }
                }
            }
            
            //Invalid if over the max duration
            if (curPauseDuration > maxPauseDuration)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Validates permission for a user to perform a certain input.
        /// </summary>
        /// <param name="userLevel">The level of the user.</param>
        /// <param name="inputName">The input to check.</param>
        /// <param name="inputAccessLevels">The dictionary of access levels for inputs.</param>
        /// <returns>An InputValidation object specifying if the input is valid and a message, if any.</returns>
        public static InputValidation CheckInputPermissions(in int userLevel, in string inputName, Dictionary<string,int> inputAccessLevels)
        {
            if (inputAccessLevels.TryGetValue(inputName, out int accessLvl) == true)
            {
                if (userLevel < accessLvl)
                {
                    return new InputValidation(false, $"No permission to use input \"{inputName}\", which requires {(AccessLevels.Levels)accessLvl} access.");
                }
            }

            return new InputValidation(true, string.Empty);
        }

        /// <summary>
        /// Validates permission for a user to perform certain inputs.
        /// </summary>
        /// <param name="userLevel">The level of the user.</param>
        /// <param name="inputs">The inputs to check.</param>
        /// <param name="inputAccessLevels">The dictionary of access levels for inputs.</param>
        /// <returns>An InputValidation object specifying if the input is valid and a message, if any.</returns>
        public static InputValidation CheckInputPermissions(in int userLevel, List<List<Parser.Input>> inputs, Dictionary<string, int> inputAccessLevels)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                for (int j = 0; j < inputs[i].Count; j++)
                {
                    Parser.Input input = inputs[i][j];

                    if (inputAccessLevels.TryGetValue(input.name, out int accessLvl) == false)
                    {
                        continue;
                    }

                    if (userLevel < accessLvl)
                    {
                        return new InputValidation(false, $"No permission to use input \"{input.name}\", which requires at least {(AccessLevels.Levels)accessLvl} access.");
                    }
                }
            }

            return new InputValidation(true, string.Empty);
        }

        public struct InputValidation
        {
            public bool IsValid;
            public string Message;

            public InputValidation(in bool isValid, string message)
            {
                IsValid = isValid;
                Message = message;
            }
        }
    }
}
