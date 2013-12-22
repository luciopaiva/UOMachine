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
using System.Threading;
using UOMachine.Utility;
using UOMachine.Events;

namespace UOMachine.Data
{
    internal static class OutgoingPacketParser
    {
        public static void ProcessPacket(int client, byte[] packet)
        {
            switch (packet[0])
            {
                case 0x00:
                    break;
                case 0x01:
                    break;
                case 0x02: //request walk
                    int direction2 = packet[1] & 0x07;
                    int sequence2 = packet[2];
                    OutgoingPackets.OnMoveRequested(client, direction2, sequence2);
                    return;
                case 0x03:
                    break;
                case 0x04:
                    break;
                case 0x05:
                    break;
                case 0x06: //double click object
                    int serial6 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    OutgoingPackets.OnUseItemRequested(client, serial6);
                    return;
                case 0x07: //drag item
                    int serial7 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int amount7 = packet[5] << 8 | packet[6];
                    OutgoingPackets.OnDragItemRequested(client, serial7, amount7);
                    return;
                case 0x08: //drop item
                    const int oldLen08 = 0x0E;
                    const int newLen08 = 0x0F;
                    if (packet.Length != oldLen08 && packet.Length != newLen08)
                        break;
                    int serial8 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int x8 = packet[5] << 8 | packet[6];
                    int y8 = packet[7] << 8 | packet[8];
                    int z8 = (sbyte)packet[9];
                    int container8;
                    if (packet.Length == oldLen08)
                    {
                        container8 = packet[10] << 24 | packet[11] << 16 | packet[12] << 8 | packet[13];
                    }
                    else
                    {
                        container8 = packet[11] << 24 | packet[12] << 16 | packet[13] << 8 | packet[14];
                    }
                    OutgoingPackets.OnDropItemRequested(client, serial8, x8, y8, z8, container8);
                    return;
                case 0x09:
                    break;
                case 0x0A:
                    break;
                case 0x0B:
                    break;
                case 0x0C:
                    break;
                case 0x0D:
                    break;
                case 0x0E:
                    break;
                case 0x0F:
                    break;
                case 0x10:
                    break;
                case 0x11:
                    break;
                case 0x12: //use skill
                    return;
                case 0x13: //equip item request
                    return;
                case 0x14:
                    break;
                case 0x15:
                    break;
                case 0x16:
                    break;
                case 0x17:
                    break;
                case 0x18:
                    break;
                case 0x19:
                    break;
                case 0x1A:
                    break;
                case 0x1B:
                    break;
                case 0x1C:
                    break;
                case 0x1D:
                    break;
                case 0x1E:
                    break;
                case 0x1F:
                    break;
                case 0x20:
                    break;
                case 0x21:
                    break;
                case 0x22:
                    break;
                case 0x23:
                    break;
                case 0x24:
                    break;
                case 0x25:
                    break;
                case 0x26:
                    break;
                case 0x27:
                    break;
                case 0x28:
                    break;
                case 0x29:
                    break;
                case 0x2A:
                    break;
                case 0x2B:
                    break;
                case 0x2C:
                    break;
                case 0x2D:
                    break;
                case 0x2E:
                    break;
                case 0x2F:
                    break;
                case 0x30:
                    break;
                case 0x31:
                    break;
                case 0x32:
                    break;
                case 0x33:
                    break;
                case 0x34:
                    break;
                case 0x35:
                    break;
                case 0x36:
                    break;
                case 0x37:
                    break;
                case 0x38:
                    break;
                case 0x39:
                    break;
                case 0x3A: //change skill lock
                    int skillID3a = packet[3] << 8 | packet[4];
                    LockStatus lockStatus3a = (LockStatus)packet[5];
                    OutgoingPackets.OnSkillLockChanged(client, skillID3a, lockStatus3a);
                    return;
                case 0x3B:
                    break;
                case 0x3C:
                    break;
                case 0x3D:
                    break;
                case 0x3E:
                    break;
                case 0x3F:
                    break;
                case 0x40:
                    break;
                case 0x41:
                    break;
                case 0x42:
                    break;
                case 0x43:
                    break;
                case 0x44:
                    break;
                case 0x45:
                    break;
                case 0x46:
                    break;
                case 0x47:
                    break;
                case 0x48:
                    break;
                case 0x49:
                    break;
                case 0x4A:
                    break;
                case 0x4B:
                    break;
                case 0x4C:
                    break;
                case 0x4D:
                    break;
                case 0x4E:
                    break;
                case 0x4F:
                    break;
                case 0x50:
                    break;
                case 0x51:
                    break;
                case 0x52:
                    break;
                case 0x53:
                    break;
                case 0x54:
                    break;
                case 0x55:
                    break;
                case 0x56:
                    break;
                case 0x57:
                    break;
                case 0x58:
                    break;
                case 0x59:
                    break;
                case 0x5A:
                    break;
                case 0x5B:
                    break;
                case 0x5C:
                    break;
                case 0x5D:
                    break;
                case 0x5E:
                    break;
                case 0x5F:
                    break;
                case 0x60:
                    break;
                case 0x61:
                    break;
                case 0x62:
                    break;
                case 0x63:
                    break;
                case 0x64:
                    break;
                case 0x65:
                    break;
                case 0x66:
                    break;
                case 0x67:
                    break;
                case 0x68:
                    break;
                case 0x69:
                    break;
                case 0x6A:
                    break;
                case 0x6B:
                    break;
                case 0x6C:
                    int type6c = packet[1];
                    //int charSerial6c = packet[2] << 24 | packet[3] << 16 | packet[4] << 8 | packet[5];
                    bool checkCrime6c = packet[6] == 1;
                    int serial6c = packet[7] << 24 | packet[8] << 16 | packet[9] << 8 | packet[10];
                    int x6c = (short)(packet[11] << 8 | packet[12]);
                    int y6c = (short)(packet[13] << 8 | packet[14]);
                    int z6c = (short)(packet[15] << 8 | packet[16]);
                    int id6c = packet[17] << 8 | packet[18];
                    OutgoingPackets.OnTargetSent(client, type6c, checkCrime6c, serial6c, x6c, y6c, z6c, id6c, packet);
                    return;
                case 0x6D:
                    break;
                case 0x6E:
                    break;
                case 0x6F:
                    break;
                case 0x70:
                    break;
                case 0x71:
                    break;
                case 0x72:
                    break;
                case 0x73:
                    break;
                case 0x74:
                    break;
                case 0x75:
                    break;
                case 0x76:
                    break;
                case 0x77:
                    break;
                case 0x78:
                    break;
                case 0x79:
                    break;
                case 0x7A:
                    break;
                case 0x7B:
                    break;
                case 0x7C:
                    break;
                case 0x7D:
                    break;
                case 0x7E:
                    break;
                case 0x7F:
                    break;
                case 0x80:
                    break;
                case 0x81:
                    break;
                case 0x82:
                    break;
                case 0x83:
                    break;
                case 0x84:
                    break;
                case 0x85:
                    break;
                case 0x86:
                    break;
                case 0x87:
                    break;
                case 0x88:
                    break;
                case 0x89:
                    break;
                case 0x8A:
                    break;
                case 0x8B:
                    break;
                case 0x8C:
                    break;
                case 0x8D:
                    break;
                case 0x8E:
                    break;
                case 0x8F:
                    break;
                case 0x90:
                    break;
                case 0x91:
                    break;
                case 0x92:
                    break;
                case 0x93:
                    break;
                case 0x94:
                    break;
                case 0x95:
                    break;
                case 0x96:
                    break;
                case 0x97:
                    break;
                case 0x98:
                    break;
                case 0x99:
                    break;
                case 0x9A:
                    break;
                case 0x9B:
                    break;
                case 0x9C:
                    break;
                case 0x9D:
                    break;
                case 0x9E:
                    break;
                case 0x9F:
                    break;
                case 0xA0:
                    break;
                case 0xA1:
                    break;
                case 0xA2:
                    break;
                case 0xA3:
                    break;
                case 0xA4:
                    break;
                case 0xA5:
                    break;
                case 0xA6:
                    break;
                case 0xA7:
                    break;
                case 0xA8:
                    break;
                case 0xA9:
                    break;
                case 0xAA:
                    break;
                case 0xAB:
                    break;
                case 0xAC:
                    break;
                case 0xAD:
                    break;
                case 0xAE:
                    break;
                case 0xAF:
                    break;
                case 0xB0:
                    break;
                case 0xB1: //gump choice, only button & switches are processed
                    int serialb1 = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    int gumpb1 = packet[7] << 24 | packet[8] << 16 | packet[9] << 8 | packet[10];
                    int buttonb1 = packet[11] << 24 | packet[12] << 16 | packet[13] << 8 | packet[14];
                    int[] switchvaluesb1 = new int[0];
                    if (gumpb1 != 461)
                    {
                        int switchesb1 = packet[15] << 24 | packet[16] << 16 | packet[17] << 8 | packet[18];
                        switchvaluesb1 = new int[switchesb1];
                        int offsetb1 = 19;
                        for (int xb1 = 0; xb1 < switchesb1; xb1++)
                        {
                            switchvaluesb1[xb1] = packet[offsetb1] << 24 | packet[offsetb1 + 1] << 16 | packet[offsetb1 + 2] << 8 | packet[offsetb1 + 3];
                            offsetb1 += 4;
                        }
                    }
                    OutgoingPackets.OnGumpButtonPressed(client, serialb1, gumpb1, buttonb1, switchvaluesb1);
                    return;
                case 0xB2:
                    break;
                case 0xB3:
                    break;
                case 0xB4:
                    break;
                case 0xB5:
                    break;
                case 0xB6:
                    break;
                case 0xB7:
                    break;
                case 0xB8:
                    break;
                case 0xB9:
                    break;
                case 0xBA:
                    break;
                case 0xBB:
                    break;
                case 0xBC:
                    break;
                case 0xBD:
                    break;
                case 0xBE:
                    break;
                case 0xBF: //misc commands
                    int commandbf = packet[3] << 8 | packet[4];
                    /*if (commandbf == 0x13) //context menu request
                    {
                        return;
                    }
                    else if (commandbf == 0x15) //context menu click
                    {
                        return;
                    }*/
                    /*if (commandbf == 0x0C)
                        break;*/
                    if (commandbf == 0x1A) //change stat lock
                    {
                        OutgoingPackets.OnStatLockChanged(client, packet[5], packet[6]);
                        return;
                    }
                    return;
                case 0xC0:
                    break;
                case 0xC1:
                    break;
                case 0xC2:
                    break;
                case 0xC3:
                    break;
                case 0xC4:
                    break;
                case 0xC5:
                    break;
                case 0xC6:
                    break;
                case 0xC7:
                    break;
                case 0xC8:
                    break;
                case 0xC9:
                    break;
                case 0xCA:
                    break;
                case 0xCB:
                    break;
                case 0xCC:
                    break;
                case 0xCD:
                    break;
                case 0xCE:
                    break;
                case 0xCF:
                    break;
                case 0xD0:
                    break;
                case 0xD1:
                    break;
                case 0xD2:
                    break;
                case 0xD3:
                    break;
                case 0xD4:
                    break;
                case 0xD5:
                    break;
                case 0xD6:
                    break;
                case 0xD7:
                    break;
                case 0xD8:
                    break;
                case 0xD9:
                    break;
                case 0xDA:
                    break;
                case 0xDB:
                    break;
                case 0xDC:
                    break;
                case 0xDD:
                    break;
                case 0xDE:
                    break;
                case 0xDF:
                    break;
                case 0xE0:
                    break;
                case 0xE1:
                    break;
                case 0xE2:
                    break;
                case 0xE3:
                    break;
                case 0xE4:
                    break;
                case 0xE5:
                    break;
                case 0xE6:
                    break;
                case 0xE7:
                    break;
                case 0xE8:
                    break;
                case 0xE9:
                    break;
                case 0xEA:
                    break;
                case 0xEB:
                    break;
                case 0xEC:
                    break;
                case 0xED:
                    break;
                case 0xEE:
                    break;
                case 0xEF:
                    break;
                case 0xF0:
                    break;
                case 0xF1:
                    break;
                case 0xF2:
                    break;
                default:
                    break;
            }
            Log.LogDataMessage(client, packet, "+++ Outgoing packet:\r\n");
        }
    }
}