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

using UOMachine.Utility;
using System;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace UOMachine
{
    public class GumpInfo
    {
        private readonly IntPtr myGumpHandle;
        private readonly IntPtr myProcessHandle;
        private readonly IntPtr myGumpFunctionCaveAddress;
        private readonly IntPtr myXAddress;
        private readonly IntPtr myYAddress;
        private readonly byte[] myGumpHandleBytes;
        private readonly int myDateStamp;

        public IntPtr GumpHandle
        {
            get { return myGumpHandle; }
        }

        private GumpInfo[] mySubGumps;
        public GumpInfo[] SubGumps
        {
            get { return mySubGumps; }
        }

        private string myType;
        public string Type
        {
            get { return myType; }
        }

        private int mySerial;
        public int Serial
        {
            get { return mySerial; }
        }

        private int myID;
        public int ID
        {
            get { return myID; }
        }

        private int myWidth;
        public int Width
        {
            get { return myWidth; }
        }

        private int myHeight;
        public int Height
        {
            get { return myHeight; }
        }

        public int X
        {
            get { return GumpHelper.GetINT(myProcessHandle, myXAddress); }
            set
            {
                byte[] data = BitConverter.GetBytes(value);
                Memory.Write(myProcessHandle, myXAddress, data, true);
            }
        }

        public int Y
        {
            get { return GumpHelper.GetINT(myProcessHandle, myYAddress); }
            set
            {
                byte[] data = BitConverter.GetBytes(value);
                Memory.Write(myProcessHandle, myYAddress, data, true);
            }
        }

        public GumpInfo(IntPtr processHandle, IntPtr gumpFunctionCaveAddress, IntPtr gumpHandle, int datestamp)
        {
            myProcessHandle = processHandle;
            myGumpFunctionCaveAddress = gumpFunctionCaveAddress;
            myGumpHandle = gumpHandle;
            myType = GetGumpType(processHandle, gumpHandle);
            myXAddress = (IntPtr)((ulong)gumpHandle + 0x34);
            myYAddress = (IntPtr)((ulong)gumpHandle + 0x38);
            myWidth = GumpHelper.GetINT(processHandle, gumpHandle, 0x24);
            myHeight = GumpHelper.GetINT(processHandle, gumpHandle, 0x28);
            myID = GetID(processHandle, gumpHandle, myType);
            mySerial = GetSerial(processHandle, gumpHandle, myType, datestamp);
            myGumpHandleBytes = BitConverter.GetBytes((uint)gumpHandle);
            myDateStamp = datestamp;
            List<GumpInfo> gumpInfoList = new List<GumpInfo>();
            IntPtr[] gumpHandles = GumpHelper.GetSubGumps(processHandle, gumpHandle);
            foreach (IntPtr i in gumpHandles)
            {
                gumpInfoList.Add(new GumpInfo(processHandle, gumpFunctionCaveAddress, i, datestamp));
            }
            mySubGumps = gumpInfoList.ToArray();
        }

        public void CallFunction(int functionIndex)
        {
            CallVTableFunction(myProcessHandle, myGumpFunctionCaveAddress, functionIndex, myGumpHandleBytes);
        }

        private static void CallVTableFunction(IntPtr processHandle, IntPtr gumpFunctionCaveAddress, int index, byte[] gumpHandleBytes)
        {
            byte[] data = new byte[9];
            byte[] functionIndex = BitConverter.GetBytes(index);
            data[0] = gumpHandleBytes[0];
            data[1] = gumpHandleBytes[1];
            data[2] = gumpHandleBytes[2];
            data[3] = gumpHandleBytes[3];
            data[4] = functionIndex[0];
            data[5] = functionIndex[1];
            data[6] = functionIndex[2];
            data[7] = functionIndex[3];
            data[8] = 0x01;
            Memory.Write(processHandle, gumpFunctionCaveAddress, data, true);
        }

        private static int GetSerial(IntPtr pHandle, IntPtr gumpHandle, string gumpType, int datestamp)
        {
            byte[] data = new byte[4];
            switch (gumpType)
            {
                case "container gump":
                    IntPtr objPtr = (IntPtr)(gumpHandle.ToInt32() + 0x4C);
                    Memory.Read(pHandle, objPtr, data, true);
                    objPtr = (IntPtr)(BitConverter.ToUInt32(data, 0));
                    if (objPtr != IntPtr.Zero)
                    {
                        IntPtr serialPtr1;
                        if (datestamp < 0x4AA52CC4) serialPtr1 = (IntPtr)(objPtr.ToInt32() + 0x80);
                        else serialPtr1 = (IntPtr)(objPtr.ToInt32() + 0x88);
                        Memory.Read(pHandle, serialPtr1, data, true);
                        return BitConverter.ToInt32(data, 0);
                    }
                    break;
                case "generic gump":
                    IntPtr serialPtr2 = (IntPtr)(gumpHandle.ToInt32() + 0xD8);
                    Memory.Read(pHandle, serialPtr2, data, true);
                    return BitConverter.ToInt32(data, 0);
                default:
                    break;
            }
            return 0;
        }

        private static int GetID(IntPtr pHandle, IntPtr gumpHandle, string gumpType)
        {
            byte[] data = new byte[4];
            byte[] shortData = new byte[2];

            switch (gumpType)
            {
                case "container gump":
                    IntPtr objPtr = (IntPtr)(gumpHandle.ToInt32() + 0x4C);
                    Memory.Read(pHandle, objPtr, data, true);
                    objPtr = (IntPtr)(BitConverter.ToUInt32(data, 0));
                    if (objPtr != IntPtr.Zero)
                    {
                        IntPtr idPtr1 = (IntPtr)(objPtr.ToInt32() + 0x38);
                        Memory.Read(pHandle, idPtr1, shortData, true);
                        return BitConverter.ToUInt16(shortData, 0);
                    }
                    break;
                case "generic gump":
                    IntPtr idPtr2 = (IntPtr)(gumpHandle.ToInt32() + 0xDC);
                    Memory.Read(pHandle, idPtr2, data, true);
                    return BitConverter.ToInt32(data, 0);
                default:
                    break;
            }
            return 0;
        }

        

        private static byte[] GetVTableBytes(IntPtr pHandle, IntPtr gHandle)
        {
            return BitConverter.GetBytes((GumpHelper.GetUINT(pHandle, gHandle)));
        }

        private static string GetGumpType(IntPtr pHandle, IntPtr gHandle)
        {
            byte[] data = new byte[128];
            IntPtr stringAddress = (IntPtr)GumpHelper.GetUINT(pHandle, gHandle, 8);
            if (stringAddress == IntPtr.Zero) return "";
            Memory.Read(pHandle, stringAddress, data, true);
            for (int x = 1; x < data.Length; x++)
            {
                if (data[x] == 0)
                {
                    return ASCIIEncoding.ASCII.GetString(data, 0, x);
                }
            }
            return "";
        }

    }
}