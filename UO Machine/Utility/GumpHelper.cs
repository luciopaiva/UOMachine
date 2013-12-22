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
using System.Text;

namespace UOMachine.Utility
{
    internal class GumpHelper
    {
        public static uint GetUINT(IntPtr pHandle, IntPtr address, int offset)
        {
            byte[] data = new byte[4];
            IntPtr newAddress = (IntPtr)((uint)address + offset);
            Memory.Read(pHandle, newAddress, data, true);
            return BitConverter.ToUInt32(data, 0);
        }

        public static uint GetUINT(IntPtr pHandle, IntPtr address)
        {
            byte[] data = new byte[4];
            Memory.Read(pHandle, address, data, true);
            return BitConverter.ToUInt32(data, 0);
        }

        public static int GetINT(IntPtr pHandle, IntPtr address, int offset)
        {
            byte[] data = new byte[4];
            IntPtr newAddress = (IntPtr)((uint)address + offset);
            Memory.Read(pHandle, newAddress, data, true);
            return BitConverter.ToInt32(data, 0);
        }

        public static int GetINT(IntPtr pHandle, IntPtr address)
        {
            byte[] data = new byte[4];
            Memory.Read(pHandle, address, data, true);
            return BitConverter.ToInt32(data, 0);
        }

        public static IntPtr[] GetSubGumps(IntPtr processHandle, IntPtr gumpHandle)
        {
            List<IntPtr> pointerList = new List<IntPtr>();
            byte[] pointer = new byte[4];
            uint gumpAddress;
            IntPtr gumpPointer;

            gumpAddress = (uint)gumpHandle;
            if (gumpAddress == 0) return pointerList.ToArray();
            gumpAddress += 0x68;


            gumpAddress = GetUINT(processHandle, (IntPtr)gumpAddress);
            if (gumpAddress != 0)
            {
                gumpPointer = (IntPtr)gumpAddress;
                pointerList.AddRange(GetGumpHandles(processHandle, gumpPointer));
                if (!pointerList.Contains(gumpPointer))
                    pointerList.Add(gumpPointer);
            }

            gumpAddress = (uint)gumpHandle;
            gumpAddress += 0x60;

            gumpAddress = GetUINT(processHandle, (IntPtr)gumpAddress);
            if (gumpAddress != 0)
            {
                gumpPointer = (IntPtr)gumpAddress;
                IntPtr[] pointerList2 = GetGumpHandles(processHandle, gumpPointer);
                foreach (IntPtr i in pointerList2)
                {
                    if (!pointerList.Contains(i))
                        pointerList.Add(i);
                }
                if (!pointerList.Contains(gumpPointer))
                    pointerList.Add(gumpPointer);
            }

            return pointerList.ToArray();
        }

        public static IntPtr[] GetGumpHandles(IntPtr processHandle, IntPtr gumpHandle)
        {
            List<IntPtr> pointerList = new List<IntPtr>();
            byte[] pointer = new byte[4];
            uint gumpAddress = (uint)gumpHandle, lastGump = gumpAddress;
            IntPtr gumpPointer;

            if (gumpAddress == 0) return pointerList.ToArray();
            bool atStart = false;
            gumpAddress += 0x5C;
            for (; ; )
            {
                gumpAddress = GetUINT(processHandle, (IntPtr)gumpAddress);
                if (!atStart) // seek backwards to first gump
                {
                    if (gumpAddress == 0)
                    {
                        atStart = true;
                        gumpAddress = lastGump + 0x58;
                        continue;
                    }
                    lastGump = gumpAddress;
                    gumpAddress += 0x5C;
                }
                else
                {
                    if (gumpAddress == 0) break;
                    gumpPointer = (IntPtr)gumpAddress;
                    if (!pointerList.Contains(gumpPointer))
                        pointerList.Add(gumpPointer);
                    gumpAddress += 0x58;
                }

            }

            return pointerList.ToArray();
        }
    }
}