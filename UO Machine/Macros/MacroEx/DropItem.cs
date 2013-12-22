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

namespace UOMachine.Macros
{
    public static partial class MacroEx
    {
        /// <summary>
        /// Drop item into specified container, stacking it with like items.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="serial">Serial of item to drop.</param>
        /// <param name="containerSerial">Serial of destination container.</param>
        public static void DropItem(int client, int serial, int containerSerial)
        {
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                byte[] packet;
                if (ci.DateStamp >= 1183740939)
                {
                    packet = new byte[15];
                    packet[11] = (byte)(containerSerial >> 24);
                    packet[12] = (byte)(containerSerial >> 16);
                    packet[13] = (byte)(containerSerial >> 8);
                    packet[14] = (byte)(containerSerial);
                }
                else
                {
                    packet = new byte[14];
                    packet[10] = (byte)(containerSerial >> 24);
                    packet[11] = (byte)(containerSerial >> 16);
                    packet[12] = (byte)(containerSerial >> 8);
                    packet[13] = (byte)(containerSerial);
                }

                packet[0] = 0x08;
                packet[1] = (byte)(serial >> 24);
                packet[2] = (byte)(serial >> 16);
                packet[3] = (byte)(serial >> 8);
                packet[4] = (byte)(serial);
                packet[5] = 0xFF;
                packet[6] = 0xFF;
                packet[7] = 0xFF;
                packet[8] = 0xFF;
                SendPacketToServer(client, packet);
            }
        }

        /// <summary>
        /// Drop item into specified container at the specified coordinates.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="serial">Serial of item to drop.</param>
        /// <param name="containerSerial">Serial of destination container.</param>
        /// <param name="x">X coordinate to drop the item at.</param>
        /// <param name="y">Y coordinate to drop the item at.</param>
        /// <param name="z">Z coordinate to drop the item at.</param>
        public static void DropItem(int client, int serial, int containerSerial, int x, int y, int z)
        {
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                byte[] packet;
                if (ci.DateStamp >= 1183740939)
                {
                    packet = new byte[15];
                    packet[11] = (byte)(containerSerial >> 24);
                    packet[12] = (byte)(containerSerial >> 16);
                    packet[13] = (byte)(containerSerial >> 8);
                    packet[14] = (byte)(containerSerial);
                }
                else
                {
                    packet = new byte[14];
                    packet[10] = (byte)(containerSerial >> 24);
                    packet[11] = (byte)(containerSerial >> 16);
                    packet[12] = (byte)(containerSerial >> 8);
                    packet[13] = (byte)(containerSerial);
                }

                packet[0] = 0x08;
                packet[1] = (byte)(serial >> 24);
                packet[2] = (byte)(serial >> 16);
                packet[3] = (byte)(serial >> 8);
                packet[4] = (byte)(serial);
                packet[5] = (byte)(x >> 8);
                packet[6] = (byte)x;
                packet[7] = (byte)(y >> 8);
                packet[8] = (byte)y;
                packet[9] = (byte)z;
                SendPacketToServer(client, packet);
            }
        }

        /// <summary>
        /// Drop item onto the ground at the specified coordinates.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="serial">Serial of item to drop.</param>
        /// <param name="x">X coordinate to drop the item at.</param>
        /// <param name="y">Y coordinate to drop the item at.</param>
        /// <param name="z">Z coordinate to drop the item at.</param>
        public static void DropItem(int client, int serial, int x, int y, int z)
        {
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                byte[] packet;
                if (ci.DateStamp >= 1183740939)
                {
                    packet = new byte[15];
                    packet[11] = 0xFF;
                    packet[12] = 0xFF;
                    packet[13] = 0xFF;
                    packet[14] = 0xFF;
                }
                else
                {
                    packet = new byte[14];
                    packet[10] = 0xFF;
                    packet[11] = 0xFF;
                    packet[12] = 0xFF;
                    packet[13] = 0xFF;
                }

                packet[0] = 0x08;
                packet[1] = (byte)(serial >> 24);
                packet[2] = (byte)(serial >> 16);
                packet[3] = (byte)(serial >> 8);
                packet[4] = (byte)(serial);
                packet[5] = (byte)(x >> 8);
                packet[6] = (byte)x;
                packet[7] = (byte)(y >> 8);
                packet[8] = (byte)y;
                packet[9] = (byte)z;
                SendPacketToServer(client, packet);
            }
        }
    }
}