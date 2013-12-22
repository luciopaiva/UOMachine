/* Copyright (C) 2009 Matthew Geyer
 * 
 * This file is part of UO Machine.
 * 
 * UO Machine is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * UO Machine is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with UO Machine.  If not, see <http://www.gnu.org/licenses/>. */

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UOMachine;

namespace UOMachine.Macros
{
    public static partial class Macro
    {
        /// <summary>
        /// Send array of System.Window.Forms.Keys to client as keypresses.  Restores and activates window.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="keys">System.Window.Forms.Keys to send.</param>
        /// <returns>True if all keys were successfully sent.</returns>
        public static bool SendKeys(int client, Keys[] keys)
        {
            if (keys.Length == 0) return false;
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                Win32.INPUT[] inputs = new Win32.INPUT[keys.Length * 2];
                Win32.KEYBDINPUT kbi = new Win32.KEYBDINPUT();

                if (!ci.PrepareWindowForInput())
                {
                    ci.DetachFromWindow();
                    return false;
                }

                for (int x = 0; x < keys.Length; x++)
                {
                    kbi.dwFlags = 0;
                    kbi.wVk = (ushort)keys[x];
                    inputs[x * 2].mkhi.ki = kbi;
                    inputs[x * 2].type = Win32.INPUT_KEYBOARD;
                    kbi.dwFlags = Win32.KEYEVENTF_KEYUP;
                    inputs[x * 2 + 1].mkhi.ki = kbi;
                    inputs[x * 2 + 1].type = Win32.INPUT_KEYBOARD;
                }
                uint success = Win32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(inputs[0]));
                ci.DetachFromWindow();
                return success == inputs.Length;
            }
            return false;
        }
    }
}