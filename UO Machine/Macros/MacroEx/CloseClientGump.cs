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
using UOMachine.Data;

namespace UOMachine.Macros
{
    public static partial class MacroEx
    {
        /// <summary>
        /// Sends close gump packet to client.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="gumpID">ID of gump to close.</param>
        public static void CloseClientGump(int client, int gumpID)
        {
            byte[] packet = new byte[13];
            packet[0] = 0xBF;
            packet[1] = 0x0D;
            packet[4] = 0x04;
            packet[5] = (byte)(gumpID >> 24);
            packet[6] = (byte)(gumpID >> 16);
            packet[7] = (byte)(gumpID >> 8);
            packet[8] = (byte)(gumpID);
            SendPacketToClient(client, packet);
        }
    }
}