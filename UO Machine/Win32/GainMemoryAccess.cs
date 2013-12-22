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

namespace UOMachine
{
    internal static partial class Win32
    {
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
    }
}