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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace UOMachine
{
    internal static partial class Memory
    {
        public static bool Read(IntPtr processHandle, IntPtr address, byte[] buffer, bool throwException)
        {
            int bytesRead = 0;
            bool success = Win32.ReadProcessMemory(processHandle, address, buffer, (UIntPtr)buffer.Length, out bytesRead);
            if (!success && throwException) throw new Win32Exception(Marshal.GetLastWin32Error());
            return success;
        }

        public static bool Write(IntPtr processHandle, IntPtr address, byte[] buffer, bool throwException)
        {
            int bytesWritten = 0;
            bool success = Win32.WriteProcessMemory(processHandle, address, buffer, (UIntPtr)buffer.Length, out bytesWritten);
            if (!success && throwException) throw new Win32Exception(Marshal.GetLastWin32Error());
            return success;
        }

        public static IntPtr Allocate(IntPtr processHandle, IntPtr address, uint size, bool throwException)
        {
            IntPtr allocAddress = IntPtr.Zero;

            if ((allocAddress = Win32.VirtualAllocEx(processHandle, address, size, Win32.AllocationType.Commit, Win32.MemoryProtection.ExecuteReadWrite)) == IntPtr.Zero)
            {
                if (throwException) throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return allocAddress;
        }
    }
}