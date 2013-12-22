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
        public static void Target(int client, TargetType targetType, int serial, int x, int y, int z, int ID)
        {
            PlayerMobile pm;
            if (ClientInfoCollection.GetPlayer(client, out pm))
            {
                byte[] packet = new byte[19];
                packet[0] = 0x6C;
                switch (targetType)
                {
                    case TargetType.Object:
                        packet[1] = 0x00;
                        break;
                    case TargetType.Ground:
                        packet[1] = 0x01;
                        break;
                }
                packet[2] = (byte)(pm.Serial >> 24);
                packet[3] = (byte)(pm.Serial >> 16);
                packet[4] = (byte)(pm.Serial >> 8);
                packet[5] = (byte)(pm.Serial);
                packet[7] = (byte)(serial >> 24);
                packet[8] = (byte)(serial >> 16);
                packet[9] = (byte)(serial >> 8);
                packet[10] = (byte)(serial);
                packet[11] = (byte)(x >> 8);
                packet[12] = (byte)x;
                packet[13] = (byte)(y >> 8);
                packet[14] = (byte)y;
                //packet[15] = (byte)(z >> 8);
                packet[16] = (byte)z;
                packet[17] = (byte)(ID >> 8);
                packet[18] = (byte)ID;
                SendPacketToServer(client, packet);
                Macro.SetTargetCursor(client, false);
            }
        }

        public static void Target(int client, Item item)
        {
            PlayerMobile pm;
            if (item != null && ClientInfoCollection.GetPlayer(client, out pm))
            {
                byte[] packet = new byte[19];
                packet[0] = 0x6C;
                packet[1] = 0x00;
                packet[2] = (byte)(pm.Serial >> 24);
                packet[3] = (byte)(pm.Serial >> 16);
                packet[4] = (byte)(pm.Serial >> 8);
                packet[5] = (byte)(pm.Serial);
                packet[7] = (byte)(item.Serial >> 24);
                packet[8] = (byte)(item.Serial >> 16);
                packet[9] = (byte)(item.Serial >> 8);
                packet[10] = (byte)(item.Serial);
                packet[11] = (byte)(item.X >> 8);
                packet[12] = (byte)item.X;
                packet[13] = (byte)(item.Y >> 8);
                packet[14] = (byte)item.Y;
                //packet[15] = (byte)(item.Z >> 8);
                packet[16] = (byte)item.Z;
                packet[17] = (byte)(item.ID >> 8);
                packet[18] = (byte)item.ID;
                SendPacketToServer(client, packet);
                Macro.SetTargetCursor(client, false);
            }
        }

        public static void Target(int client, Mobile mobile)
        {
            PlayerMobile pm;
            if (mobile != null && ClientInfoCollection.GetPlayer(client, out pm))
            {
                byte[] packet = new byte[19];
                packet[0] = 0x6C;
                packet[1] = 0x00;
                packet[2] = (byte)(pm.Serial >> 24);
                packet[3] = (byte)(pm.Serial >> 16);
                packet[4] = (byte)(pm.Serial >> 8);
                packet[5] = (byte)(pm.Serial);
                packet[7] = (byte)(mobile.Serial >> 24);
                packet[8] = (byte)(mobile.Serial >> 16);
                packet[9] = (byte)(mobile.Serial >> 8);
                packet[10] = (byte)(mobile.Serial);
                packet[11] = (byte)(mobile.X >> 8);
                packet[12] = (byte)mobile.X;
                packet[13] = (byte)(mobile.Y >> 8);
                packet[14] = (byte)mobile.Y;
                //packet[15] = (byte)(mobile.Z >> 8);
                packet[16] = (byte)mobile.Z;
                packet[17] = (byte)(mobile.ID >> 8);
                packet[18] = (byte)mobile.ID;
                SendPacketToServer(client, packet);
                Macro.SetTargetCursor(client, false);
            }
        }

        public static void Target(int client, LandTile landTile)
        {
            PlayerMobile pm;
            if (landTile != null && ClientInfoCollection.GetPlayer(client, out pm))
            {
                byte[] packet = new byte[19];
                packet[0] = 0x6C;
                packet[1] = 0x01;
                packet[2] = (byte)(pm.Serial >> 24);
                packet[3] = (byte)(pm.Serial >> 16);
                packet[4] = (byte)(pm.Serial >> 8);
                packet[5] = (byte)(pm.Serial);
                packet[11] = (byte)(landTile.X >> 8);
                packet[12] = (byte)landTile.X;
                packet[13] = (byte)(landTile.Y >> 8);
                packet[14] = (byte)landTile.Y;
                //packet[15] = (byte)(landTile.Z >> 8);
                packet[16] = (byte)landTile.Z;
                packet[17] = (byte)(landTile.ID >> 8);
                packet[18] = (byte)landTile.ID;
                SendPacketToServer(client, packet);
                Macro.SetTargetCursor(client, false);
            }
        }

        public static void Target(int client, StaticTile staticTile)
        {
            PlayerMobile pm;
            if (staticTile != null && ClientInfoCollection.GetPlayer(client, out pm))
            {
                int x = staticTile.X;
                int y = staticTile.Y;
                int z = staticTile.Z;
                int ID = staticTile.ID;
                byte[] packet = new byte[19];
                packet[0] = 0x6C;
                packet[1] = 0x01;
                packet[2] = (byte)(pm.Serial >> 24);
                packet[3] = (byte)(pm.Serial >> 16);
                packet[4] = (byte)(pm.Serial >> 8);
                packet[5] = (byte)(pm.Serial);
                packet[11] = (byte)(staticTile.X >> 8);
                packet[12] = (byte)staticTile.X;
                packet[13] = (byte)(staticTile.Y >> 8);
                packet[14] = (byte)staticTile.Y;
                //packet[15] = (byte)(staticTile.Z >> 8);
                packet[16] = (byte)staticTile.Z;
                packet[17] = (byte)(staticTile.ID >> 8);
                packet[18] = (byte)staticTile.ID;
                SendPacketToServer(client, packet);
                Macro.SetTargetCursor(client, false);
            }
        }

    }
}