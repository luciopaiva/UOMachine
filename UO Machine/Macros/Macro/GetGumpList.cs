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
using UOMachine;
using UOMachine.Utility;

namespace UOMachine.Macros
{
    public static partial class Macro
    {
        public static GumpInfo[] GetGumpList(int client)
        {
            ClientInfo ci;
            List<GumpInfo> gumpList = new List<GumpInfo>();
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                gumpList.Add(new GumpInfo(ci.Handle, ci.GumpFunctionCaveAddress, ci.TopGumpHandle, ci.DateStamp));
                IntPtr[] gumpHandles = GumpHelper.GetGumpHandles(ci.Handle, ci.TopGumpHandle);
                foreach (IntPtr i in gumpHandles)
                {
                    gumpList.Add(new GumpInfo(ci.Handle, ci.GumpFunctionCaveAddress, i, ci.DateStamp));
                }
            }
            return gumpList.ToArray();
        }
    }
}