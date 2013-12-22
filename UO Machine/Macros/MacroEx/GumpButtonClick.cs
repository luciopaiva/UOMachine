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
using System.Text;

namespace UOMachine.Macros
{
    public static partial class MacroEx
    {
        private static readonly int[] emptySwitch = new int[] { };

        public static void GumpButtonClick(int client, int serial, int gumpID, int buttonID)
        {
            GumpButtonClick(client, serial, gumpID, buttonID, emptySwitch, "");
        }

        //purposely limiting this to one text string at a time for now
        public static void GumpButtonClick(int client, int serial, int gumpID, int buttonID, int[] switches, string text)
        {
            if (text.Length > 0xFFFF) text = text.Substring(0, 0xFFFF);
            int len = 23;
            int offset = 19;
            if (switches != null) len += switches.Length * 4;
            if (!string.IsNullOrEmpty(text)) len += (text.Length * 2) + 4;

            byte[] packet = new byte[len];
            packet[0] = 0xB1;
            packet[1] = (byte)(len);
            packet[2] = (byte)(len >> 8);
            packet[3] = (byte)(serial >> 24);
            packet[4] = (byte)(serial >> 16);
            packet[5] = (byte)(serial >> 8);
            packet[6] = (byte)(serial);
            packet[7] = (byte)(gumpID >> 24);
            packet[8] = (byte)(gumpID >> 16);
            packet[9] = (byte)(gumpID >> 8);
            packet[10] = (byte)(gumpID);
            packet[11] = (byte)(buttonID >> 24);
            packet[12] = (byte)(buttonID >> 16);
            packet[13] = (byte)(buttonID >> 8);
            packet[14] = (byte)(buttonID);
            if (switches != null)
            {
                packet[15] = (byte)(switches.Length >> 24);
                packet[16] = (byte)(switches.Length >> 16);
                packet[17] = (byte)(switches.Length >> 8);
                packet[18] = (byte)(switches.Length);
                for (int x = 0; x < switches.Length; x++)
                {
                    packet[offset] = (byte)(switches[x] >> 24);
                    packet[offset + 1] = (byte)(switches[x] >> 16);
                    packet[offset + 2] = (byte)(switches[x] >> 8);
                    packet[offset + 3] = (byte)(switches[x]);
                    offset += 4;
                }
            }

            if (!string.IsNullOrEmpty(text))
            {
                packet[offset + 3] = 0x01;
                offset += 4;
                packet[offset] = (byte)(buttonID >> 8);
                packet[offset + 1] = (byte)(buttonID);
                offset += 2;
                packet[offset] = (byte)(text.Length >> 8);
                packet[offset + 1] = (byte)(text.Length);
                offset += 2;
                byte[] textBytes = UnicodeEncoding.BigEndianUnicode.GetBytes(text);
                Buffer.BlockCopy(textBytes, 0, packet, offset, textBytes.Length);
            }

            SendPacketToServer(client, packet);
        }

    }
}