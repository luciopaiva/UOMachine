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
using System.ComponentModel;
using System.Security;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Text;

namespace RazorLoader
{
    [SuppressUnmanagedCodeSecurity]
    internal static partial class Win32
    {
        public const uint PAGE_EXECUTE_READWRITE = 0x40;
        public const uint MEM_COMMIT = 0x1000;

        public const uint PROCESS_ALL_ACCESS = 0x1F0FFF;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, uint bInheritHandle, int dwProcessId);

        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
             byte[] lpBuffer, UIntPtr nSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, UIntPtr nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern uint VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress,
        out MEMORY_BASIC_INFORMATION lpBuffer, IntPtr dwLength);

        [DllImport("kernel32.dll")]
        public static extern uint VirtualQuery(IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, IntPtr dwLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,
            IntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(IntPtr lpAddress, IntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool CloseHandle(IntPtr handle);

        private static ulong RoundUpToPageBoundary(ulong address, uint pageSize)
        {
            ulong mod = address % pageSize;
            if (mod > 0) address += (pageSize - mod);
            return address;
        }

        public static void GainMemoryAccess(IntPtr address, ulong len)
        {
            SYSTEM_INFO si = new SYSTEM_INFO();
            GetSystemInfo(out si);
            MEMORY_BASIC_INFORMATION mbi;
            ulong currentAddress = RoundUpToPageBoundary((ulong)address, si.pageSize);
            ulong endAddress = currentAddress + len;
            uint ret;
            uint oldProtect = 0;

            while (currentAddress < endAddress)
            {
                mbi = new MEMORY_BASIC_INFORMATION();
                ret = VirtualQuery((IntPtr)currentAddress, out mbi, (IntPtr)Marshal.SizeOf(mbi));
                if (ret != 0)
                {
                    if (mbi.state == MEM_COMMIT)
                    {
                        VirtualProtect(mbi.baseAddress, mbi.regionSize, PAGE_EXECUTE_READWRITE, out oldProtect);
                    }
                    if ((ulong)mbi.regionSize > 0) currentAddress += (ulong)mbi.regionSize;
                    else currentAddress += si.pageSize;
                }
                else currentAddress += si.pageSize;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            public ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr baseAddress;
            public IntPtr allocationBase;
            public uint allocationProtect;
            public IntPtr regionSize;
            public uint state;
            public uint protect;
            public uint lType;
        }

    }
}