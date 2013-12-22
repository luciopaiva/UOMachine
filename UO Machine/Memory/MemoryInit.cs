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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace UOMachine
{
    internal static partial class Memory
    {
        private static bool SetInfoAddresses(ClientInfo ci)
        {
            byte[] memBytes = new byte[4];
            uint playerInfoAddress = 0;
            for (int x = 0; x < 100; x++)
            {
                Memory.Read(ci.Handle, ci.PlayerInfoPointer, memBytes, true);
                playerInfoAddress = BitConverter.ToUInt32(memBytes, 0);
                if (playerInfoAddress > 0) break;
                Thread.Sleep(100);
            }
            if (playerInfoAddress == 0) return false;
            ci.ZAddress = (IntPtr)(playerInfoAddress + 0x28);
            return true;
        }

        public static void MemoryInit(ClientInfo ci)
        {
            ulong len = (ulong)ci.EntryPoint - (ulong)ci.BaseAddress;
            Win32.GainMemoryAccessEx(ci.Handle, ci.BaseAddress, len);
            if (!SetInfoAddresses(ci))
            {
                ci.IsValid = false;
            }
        }
    }
}
