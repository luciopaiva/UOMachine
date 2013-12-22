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
        /// Click context menu of a mobile.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="serial">Serial of target mobile.</param>
        /// <param name="contextEntry">Index of context menu entry to click.</param>
        public static void ContextMenuClick(int client, int serial, int contextEntry)
        {
            byte[] packet;
            if (serial == 0)
            {
                packet = new byte[] { 0xBF, 0x09, 0x00, 0x00, 0x15, 0x00, 0x00, 0x00, 0x00 };
            }
            else
            {
                packet = new byte[] { 0xBF, 0x0B, 0x00, 0x00, 0x15, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                packet[5] = (byte)(serial >> 24);
                packet[6] = (byte)(serial >> 16);
                packet[7] = (byte)(serial >> 8);
                packet[8] = (byte)(serial);
                packet[9] = (byte)(contextEntry >> 8);
                packet[10] = (byte)contextEntry;
            }
            SendPacketToServer(client, packet);
        }
    }
}