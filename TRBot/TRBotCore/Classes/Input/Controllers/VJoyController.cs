﻿using System;
using System.Collections.Generic;
using System.Text;
using vJoyInterfaceWrap;
using System.Runtime.CompilerServices;
using static vJoyInterfaceWrap.vJoy;

namespace TRBot
{
    public class VJoyController : IVirtualController
    {
        /// <summary>
        /// Minimum acceptable vJoy device ID.
        /// </summary>
        public const uint MIN_VJOY_DEVICE_ID = 1;

        /// <summary>
        /// Maximum acceptable vJoy device ID.
        /// </summary>
        public const uint MAX_VJOY_DEVICE_ID = 16;

        /// <summary>
        /// Tells whether the vJoy instance is initialized.
        /// </summary>
        public static bool Initialized => (VJoyInstance != null);

        public static vJoy VJoyInstance { get; private set; } = null;
        public static VJoyController[] Joysticks { get; private set; } = null;

        /// <summary>
        /// The ID of the controller.
        /// </summary>
        public uint ControllerID { get; private set; } = 0;

        /// <summary>
        /// Tells whether the controller device was successfully acquired through vJoy.
        /// If this is false, don't use this controller instance to make inputs.
        /// </summary>
        public bool IsAcquired { get; private set; } = false;

        /// <summary>
        /// The JoystickState of the controller, used in the Efficient implementation.
        /// </summary>
        private JoystickState JSState = default;

        private Dictionary<int, (long AxisMin, long AxisMax)> MinMaxAxes = new Dictionary<int, (long, long)>();

        public VJoyController(in uint controllerID)
        {
            ControllerID = controllerID;
        }

        public void Dispose()
        {
            if (Initialized == false)
                return;

            Reset();
            Close();
        }

        public void Acquire()
        {
            IsAcquired = VJoyInstance.AcquireVJD(ControllerID);
        }

        public void Close()
        {
            VJoyInstance.RelinquishVJD(ControllerID);
            IsAcquired = false;
        }

        public void Init()
        {
            Reset();

            //Initialize axes
            HID_USAGES[] axes = EnumUtility.GetValues<HID_USAGES>.EnumValues;

            for (int i = 0; i < axes.Length; i++)
            {
                if (VJoyInstance.GetVJDAxisExist(ControllerID, axes[i]))
                {
                    long min = 0L;
                    long max = 0L;
                    VJoyInstance.GetVJDAxisMin(ControllerID, axes[i], ref min);
                    VJoyInstance.GetVJDAxisMax(ControllerID, axes[i], ref max);

                    MinMaxAxes.Add((int)axes[i], (min, max));
                }
            }
        }

        public void Reset()
        {
            if (Initialized == false)
                return;

            JSState.Buttons = JSState.ButtonsEx1 = JSState.ButtonsEx2 = JSState.ButtonsEx3 = 0;

            foreach (KeyValuePair<int, (long, long)> val in MinMaxAxes)
            {
                if (val.Key == (int)HID_USAGES.HID_USAGE_RZ || val.Key == (int)HID_USAGES.HID_USAGE_Z)
                {
                    ReleaseAbsoluteAxis(val.Key);
                }
                else
                {
                    ReleaseAxis(val.Key);
                }
            }

            UpdateJoystickEfficient();
        }

        public void PressInput(in Parser.Input input)
        {
            if (InputGlobals.CurrentConsole.IsAbsoluteAxis(input) == true)
            {
                PressAbsoluteAxis(InputGlobals.CurrentConsole.InputAxes[input.name], input.percent);

                //Kimimaru: In the case of L and R buttons on GCN, when the axes are pressed, the buttons should be released
                ReleaseButton(input.name);
            }
            else if (InputGlobals.CurrentConsole.GetAxis(input, out int axis) == true)
            {
                PressAxis(axis, InputGlobals.CurrentConsole.IsMinAxis(input), input.percent);
            }
            else if (InputGlobals.CurrentConsole.IsButton(input) == true)
            {
                PressButton(input.name);

                //Kimimaru: In the case of L and R buttons on GCN, when the buttons are pressed, the axes should be released
                if (InputGlobals.CurrentConsole.InputAxes.TryGetValue(input.name, out int value) == true)
                {
                    ReleaseAbsoluteAxis(value);
                }
            }
        }

        public void ReleaseInput(in Parser.Input input)
        {
            if (InputGlobals.CurrentConsole.IsAbsoluteAxis(input) == true)
            {
                ReleaseAbsoluteAxis(InputGlobals.CurrentConsole.InputAxes[input.name]);

                //Kimimaru: In the case of L and R buttons on GCN, when the axes are released, the buttons should be too
                ReleaseButton(input.name);
            }
            else if (InputGlobals.CurrentConsole.GetAxis(input, out int axis) == true)
            {
                ReleaseAxis(axis);
            }
            else if (InputGlobals.CurrentConsole.IsButton(input) == true)
            {
                ReleaseButton(input.name);

                //Kimimaru: In the case of L and R buttons on GCN, when the buttons are released, the axes should be too
                if (InputGlobals.CurrentConsole.InputAxes.TryGetValue(input.name, out int value) == true)
                {
                    ReleaseAbsoluteAxis(value);
                }
            }
        }

        public void PressAxis(in int axis, in bool min, in int percent)
        {
            if (MinMaxAxes.TryGetValue(axis, out (long, long) axisVals) == false)
            {
                return;
            }

            long mid = (axisVals.Item2 - axisVals.Item1) / 2;
            int val = 0;

            if (min)
            {
                val = (int)(mid - ((percent / 100f) * mid));
            }
            else
            {
                val = (int)(mid + ((percent / 100f) * mid));
            }

            SetAxisEfficient(axis, val);
        }

        public void ReleaseAxis(in int axis)
        {
            if (MinMaxAxes.TryGetValue(axis, out (long, long) axisVals) == false)
            {
                return;
            }

            int val = (int)((axisVals.Item2 - axisVals.Item1) / 2);

            SetAxisEfficient(axis, val);
        }

        public void PressAbsoluteAxis(in int axis, in int percent)
        {
            if (MinMaxAxes.TryGetValue(axis, out (long, long) axisVals) == false)
            {
                return;
            }

            int val = (int)(axisVals.Item2 * (percent / 100f));

            SetAxisEfficient(axis, val);
        }

        public void ReleaseAbsoluteAxis(in int axis)
        {
            if (MinMaxAxes.ContainsKey(axis) == false)
            {
                return;
            }

            SetAxisEfficient(axis, 0);
        }

        public void PressButton(in string buttonName)
        {
            uint buttonVal = InputGlobals.CurrentConsole.ButtonInputMap[buttonName];

            //Kimimaru: Handle button counts greater than 32
            //Each buttons value contains 32 bits, so choose the appropriate one based on the value of the button pressed
            //Note that not all emulators (such as Dolphin) support more than 32 buttons
            int buttonDiv = ((int)buttonVal - 1);
            int divVal = buttonDiv / 32;
            int realVal = buttonDiv - (32 * divVal);
            uint addition = (uint)(1 << realVal);

            switch (divVal)
            {
                case 0: JSState.Buttons |= addition; break;
                case 1: JSState.ButtonsEx1 |= addition; break;
                case 2: JSState.ButtonsEx2 |= addition; break;
                case 3: JSState.ButtonsEx3 |= addition; break;
            }
        }

        public void ReleaseButton(in string buttonName)
        {
            uint buttonVal = InputGlobals.CurrentConsole.ButtonInputMap[buttonName];

            //Kimimaru: Handle button counts greater than 32
            //Each buttons value contains 32 bits, so choose the appropriate one based on the value of the button pressed
            //Note that not all emulators (such as Dolphin) support more than 32 buttons
            int buttonDiv = ((int)buttonVal - 1);
            int divVal = buttonDiv / 32;
            int realVal = buttonDiv - (32 * divVal);
            uint inverse = ~(uint)(1 << realVal);

            switch (divVal)
            {
                case 0: JSState.Buttons &= inverse; break;
                case 1: JSState.ButtonsEx1 &= inverse; break;
                case 2: JSState.ButtonsEx2 &= inverse; break;
                case 3: JSState.ButtonsEx3 &= inverse; break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetAxisEfficient(in int axis, in int value)
        {
            switch (axis)
            {
                case (int)HID_USAGES.HID_USAGE_X: JSState.AxisX = value; break;
                case (int)HID_USAGES.HID_USAGE_Y: JSState.AxisY = value; break;
                case (int)HID_USAGES.HID_USAGE_Z: JSState.AxisZ = value; break;
                case (int)HID_USAGES.HID_USAGE_RX: JSState.AxisXRot = value; break;
                case (int)HID_USAGES.HID_USAGE_RY: JSState.AxisYRot = value; break;
                case (int)HID_USAGES.HID_USAGE_RZ: JSState.AxisZRot = value; break;
            }
        }

        /// <summary>
        /// Updates the joystick.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateJoystickEfficient()
        {
            VJoyInstance.UpdateVJD(ControllerID, ref JSState);
        }

        /// <summary>
        /// Retrieves a joystick at a zero-based index.
        /// </summary>
        /// <param name="playerIndex">The zero-based index to retrieve the joystick at.</param>
        /// <returns>The VJoyController at the desired index. If this index is out of range, it'll throw an <see cref="IndexOutOfRangeException"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VJoyController GetController(in int playerIndex)
        {
            return Joysticks[playerIndex];
        }

        public static void OnDeviceRemoved(bool removed, bool first, object userData)
        {
            string startMessage = "A vJoy device has been removed!";
            if (removed == false)
            {
                startMessage = "A vJoy device has been added!";
            }

            BotProgram.QueueMessage($"{startMessage} {nameof(first)}: {first} | {nameof(userData)}: {userData}");
        }

        public static void Initialize()
        {
            if (Initialized == true) return;

            VJoyInstance = new vJoy();

            if (VJoyInstance.vJoyEnabled() == false)
            {
                Console.WriteLine("vJoy driver not enabled!");
                return;
            }

            //Kimimaru: The data object passed in here will be returned in the callback
            //This can be used to update the object's data when a vJoy device is connected or disconnected from the computer
            //This does not get fired when the vJoy device is acquired or relinquished
            VJoyInstance.RegisterRemovalCB(OnDeviceRemoved, null);

            string vendor = VJoyInstance.GetvJoyManufacturerString();
            string product = VJoyInstance.GetvJoyProductString();
            string serialNum = VJoyInstance.GetvJoySerialNumberString();
            Console.WriteLine($"vJoy driver found - Vendor: {vendor} | Product: {product} | Version: {serialNum}");

            uint dllver = 0;
            uint drvver = 0;
            bool match = VJoyInstance.DriverMatch(ref dllver, ref drvver);

            Console.WriteLine($"Using vJoy DLL version {dllver} and vJoy driver version {drvver} | Version Match: {match}");

            int acquiredCount = InitControllers(BotProgram.BotData.JoystickCount);
            Console.WriteLine($"Acquired {acquiredCount} controllers!");
        }

        public static int InitControllers(in int joystickCount)
        {
            if (Initialized == false) return 0;

            int count = joystickCount;

            //Ensure count of 1
            if (count < 1)
            {
                count = 1;
                Console.WriteLine($"Joystick count of {count} is less than 1. Clamping value to this limit.");
            }

            //Check for max vJoy device ID to ensure we don't try to register more devices than it can support
            if (count > MAX_VJOY_DEVICE_ID)
            {
                count = (int)MAX_VJOY_DEVICE_ID;

                Console.WriteLine($"Joystick count of {count} is greater than max {nameof(MAX_VJOY_DEVICE_ID)} of {MAX_VJOY_DEVICE_ID}. Clamping value to this limit.");
            }

            Joysticks = new VJoyController[count];
            for (int i = 0; i < Joysticks.Length; i++)
            {
                Joysticks[i] = new VJoyController((uint)i + MIN_VJOY_DEVICE_ID);
            }

            int acquiredCount = 0;

            //Acquire the device IDs
            for (int i = 0; i < Joysticks.Length; i++)
            {
                VJoyController joystick = Joysticks[i];
                VjdStat controlStatus = VJoyInstance.GetVJDStatus(joystick.ControllerID);

                switch (controlStatus)
                {
                    case VjdStat.VJD_STAT_FREE:
                        joystick.Acquire();
                        if (joystick.IsAcquired == false) goto default;

                        acquiredCount++;
                        Console.WriteLine($"Acquired vJoy device ID {joystick.ControllerID}!");

                        //Initialize the joystick
                        joystick.Init();

                        //Reset the joystick
                        joystick.Reset();
                        break;
                    default:
                        Console.WriteLine($"Unable to acquire vJoy device ID {joystick.ControllerID}");
                        break;
                }
            }

            return acquiredCount;
        }

        public static void CleanUp()
        {
            if (Initialized == false)
            {
                Console.WriteLine("Not initialized; cannot clean up");
                return;
            }

            if (Joysticks != null)
            {
                for (int i = 0; i < Joysticks.Length; i++)
                {
                    Joysticks[i]?.Dispose();
                }
            }
        }

        public static void CheckDeviceIDState(in uint deviceID)
        {
            VjdStat status = VJoyInstance.GetVJDStatus(deviceID);

            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    Console.WriteLine($"vJoy Device {deviceID} is already owned by this feeder");
                    break;
                case VjdStat.VJD_STAT_FREE:
                    Console.WriteLine($"vJoy Device {deviceID} is free!");
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    Console.WriteLine($"vJoy Device {deviceID} is already owned by another feeder - cannot continue");
                    break;
                case VjdStat.VJD_STAT_MISS:
                    Console.WriteLine($"vJoy Device {deviceID} is not installed or disabled - cannot continue");
                    break;
                default:
                    Console.WriteLine($"vJoy Device {deviceID} general error - cannot continue");
                    break;
            }
        }

        //public static void CheckButtonCount(in uint deviceID)
        //{
        //    int nBtn = VJoyInstance.GetVJDButtonNumber(deviceID);
        //    int nDPov = VJoyInstance.GetVJDDiscPovNumber(deviceID);
        //    int nCPov = VJoyInstance.GetVJDContPovNumber(deviceID);
        //    bool hasX = VJoyInstance.GetVJDAxisExist(deviceID, HID_USAGES.HID_USAGE_X);
        //    bool hasY = VJoyInstance.GetVJDAxisExist(deviceID, HID_USAGES.HID_USAGE_Y);
        //    bool hasZ = VJoyInstance.GetVJDAxisExist(deviceID, HID_USAGES.HID_USAGE_Z);
        //    bool hasRX = VJoyInstance.GetVJDAxisExist(deviceID, HID_USAGES.HID_USAGE_RX);
        //
        //    Console.WriteLine($"Device[{deviceID}]: Buttons: {nBtn} | DiscPOVs: {nDPov} | ContPOVs: {nCPov} | Axes - X:{hasX} Y:{hasY} Z: {hasZ} RX: {hasRX}");
        //}
    }
}