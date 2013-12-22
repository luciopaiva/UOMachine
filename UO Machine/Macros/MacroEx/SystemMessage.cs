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
using System.Text;

namespace UOMachine.Macros
{
    public static partial class MacroEx
    {
        /// <summary>
        /// Send system message packet to the specified client.
        /// </summary>
        public static void SystemMessage(int client, string text)
        {
            byte[] baseSystemMessagePacket = new byte[] {   
            0xAE, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 
            0xFF, 0x00, 0x03, 0xB2, 0x00, 0x03, 0x45, 0x4E, 
            0x55, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] textBytes = UnicodeEncoding.BigEndianUnicode.GetBytes(text + '\0');
            byte[] packet = new byte[baseSystemMessagePacket.Length + textBytes.Length];
            Buffer.BlockCopy(baseSystemMessagePacket, 0, packet, 0, baseSystemMessagePacket.Length);
            Buffer.BlockCopy(textBytes, 0, packet, baseSystemMessagePacket.Length, textBytes.Length);
            packet[1] = (byte)(packet.Length);
            packet[2] = (byte)(packet.Length >> 8);
            SendPacketToClient(client, packet);
        }
    }
}