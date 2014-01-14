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
using System.Collections.Generic;
using System.Threading;
using UOMachine.Utility;
using System.Reflection;
using UOMachine.Events;
using System.IO;
using System.IO.Compression;

namespace UOMachine.Data
{
    internal static class IncomingPacketParser
    {
        public static void ProcessPacket(int client, byte[] packet)
        {
            switch (packet[0])
            {
                case 0x00:
                    break;
                case 0x01:
                    IncomingPackets.OnLoggedOut(client);
                    return;
                case 0x02:
                    break;
                case 0x03:
                    break;
                case 0x04:
                    break;
                case 0x05:
                    break;
                case 0x06:
                    break;
                case 0x07:
                    break;
                case 0x08:
                    break;
                case 0x09:
                    break;
                case 0x0A:
                    break;
                case 0x0B: //damage
                    const int expectedLen0B = 0x07;
                    if (packet.Length != expectedLen0B)
                        break;
                    int serial0b = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int damage = packet[5] << 8 | packet[6];
                    IncomingPackets.OnDamage(client, serial0b, damage);
                    return;
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
                case 0x11: //mobile status
                    int length11 = packet[1] | packet[2] << 8;
                    int serial11 = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    string name11 = ASCIIEncoding.ASCII.GetString(packet, 7, 30).TrimEnd('\0');
                    int health11 = packet[37] << 8 | packet[38];
                    int maxHealth11 = packet[39] << 8 | packet[40];
                    int sex11 = 2;
                    if (length11 > 43) sex11 = packet[43];
                    if (length11 <= 44)
                    {
                        IncomingPackets.OnShortStatus(client, serial11, name11, health11, maxHealth11, sex11);
                    }
                    else
                    {
                        PlayerStatus ps = new PlayerStatus();
                        ps.Name = name11;
                        ps.Health = health11;
                        ps.MaxHealth = maxHealth11;
                        ps.Sex = sex11;
                        ps.Features = packet[42];
                        ps.Str = packet[44] << 8 | packet[45];
                        ps.Dex = packet[46] << 8 | packet[47];
                        ps.Int = packet[48] << 8 | packet[49];
                        ps.Stamina = packet[50] << 8 | packet[51];
                        ps.MaxStamina = packet[52] << 8 | packet[53];
                        ps.Mana = packet[54] << 8 | packet[55];
                        ps.MaxMana = packet[56] << 8 | packet[57];
                        ps.Gold = packet[58] << 24 | packet[59] << 16 | packet[60] << 8 | packet[61];
                        ps.PhysicalResist = packet[62] << 8 | packet[63];
                        ps.Weight = packet[64] << 8 | packet[65];
                        switch (packet[42])
                        {
                            case 3:
                                ps.StatCap = packet[66] << 8 | packet[67];
                                ps.Followers = packet[68];
                                ps.MaxFollowers = packet[69];
                                break;
                            case 4:
                                ps.StatCap = packet[66] << 8 | packet[67];
                                ps.Followers = packet[68];
                                ps.MaxFollowers = packet[69];
                                ps.FireResist = packet[70] << 8 | packet[71];
                                ps.ColdResist = packet[72] << 8 | packet[73];
                                ps.PoisonResist = packet[74] << 8 | packet[75];
                                ps.EnergyResist = packet[76] << 8 | packet[77];
                                ps.Luck = packet[78] << 8 | packet[79];
                                ps.MinDamage = packet[80] << 8 | packet[81];
                                ps.MaxDamage = packet[82] << 8 | packet[83];
                                ps.TithingPoints = packet[84] << 24 | packet[85] << 16 | packet[86] << 8 | packet[87];
                                break;
                            case 5:
                                ps.MaxWeight = packet[66] << 8 | packet[67];
                                ps.Race = packet[68];
                                ps.StatCap = packet[69] << 8 | packet[70];
                                ps.Followers = packet[71];
                                ps.MaxFollowers = packet[72];
                                ps.FireResist = packet[73] << 8 | packet[74];
                                ps.ColdResist = packet[75] << 8 | packet[76];
                                ps.PoisonResist = packet[77] << 8 | packet[78];
                                ps.EnergyResist = packet[79] << 8 | packet[80];
                                ps.Luck = packet[81] << 8 | packet[82];
                                ps.MinDamage = packet[83] << 8 | packet[84];
                                ps.MaxDamage = packet[85] << 8 | packet[86];
                                ps.TithingPoints = packet[87] << 24 | packet[88] << 16 | packet[89] << 8 | packet[90];
                                break;
                            case 6:
                                goto case 5;
                        }
                        IncomingPackets.OnLongStatus(client, serial11, ps);
                    }
                    return;
                case 0x12:
                    break;
                case 0x13:
                    break;
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
                case 0x1A: //add world item
                    Item item1a;
                    uint serial1a = (uint)(packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6]);
                    int offset1a = 9;
                    if ((serial1a & 0x80000000) != 0)
                    {
                        serial1a ^= 0x80000000;
                        item1a = new Item((int)serial1a);
                        item1a.Count = packet[offset1a] << 8 | packet[offset1a + 1];
                        offset1a += 2;
                    }
                    else item1a = new Item((int)serial1a);
                    int id1a = packet[7] << 8 | packet[8];
                    if ((id1a & 0x8000) != 0)
                    {
                        id1a ^= 0x8000;
                        id1a += packet[offset1a]; // stack id
                        offset1a++;
                    }
                    item1a.myID = id1a;
                    int x1a = packet[offset1a] << 8 | packet[offset1a + 1];
                    int y1a = packet[offset1a + 2] << 8 | packet[offset1a + 3];
                    offset1a += 4;
                    if ((x1a & 0x8000) != 0)
                    {
                        x1a ^= 0x8000;
                        item1a.myDirection = packet[offset1a];
                        offset1a++;
                    }
                    item1a.myX = x1a;
                    item1a.Z = (sbyte)packet[offset1a];
                    offset1a++;
                    if ((y1a & 0x8000) != 0)
                    {
                        y1a ^= 0x8000;
                        item1a.myHue = packet[offset1a] << 8 | packet[offset1a + 1];
                        offset1a += 2;
                    }
                    if ((y1a & 0x4000) != 0)
                    {
                        y1a ^= 0x4000;
                        item1a.Flags = packet[offset1a];  // ???
                    }
                    item1a.myY = y1a;
                    IncomingPackets.OnWorldItemAdded(client, item1a);
                    return;
                case 0x1B: //initialize player
                    int serial1b = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    PlayerMobile mobile1b = new PlayerMobile(serial1b, client);
                    mobile1b.myID = packet[9] << 8 | packet[10];
                    mobile1b.myX = packet[11] << 8 | packet[12];
                    mobile1b.myY = packet[13] << 8 | packet[14];
                    mobile1b.myZ = (sbyte)packet[16];
                    mobile1b.myDirection = packet[17] & 0x07;
                    IncomingPackets.OnPlayerInitialized(client, mobile1b);
                    return;
                case 0x1C: //text
                    JournalEntry je1c = new JournalEntry();
                    je1c.serial = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    je1c.id = packet[7] << 8 | packet[8];
                    je1c.speechType = (JournalSpeech)packet[9];
                    je1c.speechHue = packet[10] << 8 | packet[11];
                    je1c.speechFont = packet[12] << 8 | packet[13];
                    je1c.name = ASCIIEncoding.ASCII.GetString(packet, 14, 30).TrimEnd('\0');
                    je1c.text = ASCIIEncoding.ASCII.GetString(packet, 44, packet.Length - 44).TrimEnd('\0');
                    IncomingPackets.OnText(client, je1c);
                    General.OnJournalEntry(client, je1c);
                    return;
                case 0x1D: //delete item
                    int serial1d = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    IncomingPackets.OnItemDeleted(client, serial1d);
                    return;
                case 0x1E:
                    break;
                case 0x1F:
                    break;
                case 0x20: //update mobile
                    int serial20 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int id20 = packet[5] << 8 | packet[6];
                    int hue20 = packet[8] << 8 | packet[9];
                    int status20 = packet[10];
                    int x20 = packet[11] << 8 | packet[12];
                    int y20 = packet[13] << 8 | packet[14];
                    int direction20 = packet[16] & 0x07;
                    int z20 = (sbyte)packet[17];
                    IncomingPackets.OnMobileUpdated(client, serial20, id20, hue20, status20, x20, y20, z20, direction20);
                    return;
                case 0x21: //move rejected
                    int sequence21 = packet[1];
                    int x21 = packet[2] << 8 | packet[3];
                    int y21 = packet[4] << 8 | packet[5];
                    int direction21 = packet[6];
                    int z21 = (sbyte)packet[7];
                    IncomingPackets.OnMoveRejected(client, sequence21, x21, y21, z21, direction21);
                    return;
                case 0x22: //move accepted
                    int sequence22 = packet[1];
                    int status22 = packet[2];
                    IncomingPackets.OnMoveAccepted(client, sequence22, status22);
                    return;
                case 0x23:
                    break;
                case 0x24: //open gump
                    int serial24 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int id24 = packet[5] << 8 | packet[6];
                    IncomingPackets.OnStandardGump(client, serial24, id24);
                    return;
                case 0x25: //add item to container
                    const int expectedLen25a = 0x14;
                    const int expectedLen25b = 0x15;
                    if (packet.Length != expectedLen25a && packet.Length != expectedLen25b)
                        break;
                    int serial25 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int containerSerial25 = 0, hue25 = 0;
                    int id25 = packet[5] << 8 | packet[6];
                    int count25 = packet[8] << 8 | packet[9];
                    if (packet.Length == expectedLen25a)
                    {
                        containerSerial25 = packet[14] << 24 | packet[15] << 16 | packet[16] << 8 | packet[17];
                        hue25 = packet[18] << 8 | packet[19];
                    }
                    else if (packet.Length == expectedLen25b)
                    {
                        containerSerial25 = packet[15] << 24 | packet[16] << 16 | packet[17] << 8 | packet[18];
                        hue25 = packet[19] << 8 | packet[20];
                    }
                    Item item25 = new Item(serial25, containerSerial25);
                    item25.ID = id25;
                    item25.Count = count25;
                    item25.Hue = hue25;
                    item25.X = packet[10] << 8 | packet[11];
                    item25.Y = packet[12] << 8 | packet[13];
                    IncomingPackets.OnItemAddedToContainer(client, item25);
                    return;
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
                case 0x2C: //player death
                    IncomingPackets.OnPlayerDeath(client);
                    return;
                case 0x2D: //update health
                    return;
                case 0x2E: //equip item
                    int serial2e = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int id2e = packet[5] << 8 | packet[6];
                    int mobileSerial2e = packet[9] << 24 | packet[10] << 16 | packet[11] << 8 | packet[12];
                    int hue2e = packet[13] << 8 | packet[14];
                    IncomingPackets.OnItemEquipped(client, serial2e, id2e, (Layer)packet[8], mobileSerial2e, hue2e);
                    return;
                case 0x2F: //combat swing
                    int attacker2f = packet[2] << 24 | packet[3] << 16 | packet[4] << 8 | packet[5];
                    int defender2f = packet[6] << 24 | packet[7] << 16 | packet[8] << 8 | packet[9];
                    IncomingPackets.OnAttackSwing(client, attacker2f, defender2f);
                    return;
                case 0x30: //attack granted
                    int serial30 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    IncomingPackets.OnAttackGranted(client, serial30);
                    return;
                case 0x31: //combat ended
                    IncomingPackets.OnAttackEnded(client);
                    return;
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
                case 0x3A: //skills
                    int size3a = packet[1] | packet[2] << 8;
                    int id3a = packet[4] << 8 | packet[5];
                    int value3a = packet[6] << 8 | packet[7];
                    int baseValue3a = packet[8] << 8 | packet[9];
                    int skillCap3a = 1000;                          //
                    if (size3a != 11)                               // UOSteam doesn't send skillcap on changing locks in their UI
                        skillCap3a = packet[11] << 8 | packet[12];  // So we kludge it and default to 100 skillcap
                    LockStatus lockStatus3a = (LockStatus)packet[10];
                    if ((size3a == 11) || (size3a == 13))           // 11 from UOSteam on lock change
                    {
                        IncomingPackets.OnSkillUpdate(client, id3a, (float)value3a / 10, (float)baseValue3a / 10, lockStatus3a, (float)skillCap3a / 10);
                        return;
                    }
                    else
                    {
                        SkillInfo si = new SkillInfo();
                        si.Value = (float)value3a / 10;
                        si.BaseValue = (float)baseValue3a / 10;
                        si.LockStatus = lockStatus3a;
                        si.SkillCap = (float)skillCap3a / 10;
                        si.ID = id3a - 1;
                        int index3a = 13;
                        List<SkillInfo> skillInfoList = new List<SkillInfo>(128);

                        skillInfoList.Add(si);
                        for (; ; )
                        {
                            id3a = packet[index3a] << 8 | packet[index3a + 1];
                            if (id3a == 0) break;
                            value3a = packet[index3a + 2] << 8 | packet[index3a + 3];
                            baseValue3a = packet[index3a + 4] << 8 | packet[index3a + 5];
                            lockStatus3a = (LockStatus)packet[index3a + 6];
                            skillCap3a = packet[index3a + 7] << 8 | packet[index3a + 8];
                            si = new SkillInfo();
                            si.Value = (float)value3a / 10;
                            si.BaseValue = (float)baseValue3a / 10;
                            si.LockStatus = lockStatus3a;
                            si.SkillCap = (float)skillCap3a / 10;
                            si.ID = id3a - 1;
                            skillInfoList.Add(si);
                            index3a += 9;
                        }
                        IncomingPackets.OnSkillList(client, skillInfoList.ToArray());
                    }
                    return;
                case 0x3B:
                    break;
                case 0x3C: //container contents
                    int size3c = packet[1] | packet[2] << 8;
                    if (size3c == 5) return;
                    bool oldStyle3c = false;
                    int count3c = packet[3] << 8 | packet[4];
                    if (((size3c - 5) / 20) != count3c)
                        oldStyle3c = true;
                    ItemCollection container3c = null;
                    int containerSerial3c, serial3c, offset3c, id3c, itemCount3c, hue3c, grid3c = 0, x3c = 0, y3c = 0;
                    for (int x = 0; x < count3c; x++)
                    {
                        if (oldStyle3c)
                        {
                            offset3c = (x * 19) + 5;
                        }
                        else
                        {
                            offset3c = (x * 20) + 5;
                        }
                        serial3c = packet[offset3c] << 24 | packet[offset3c + 1] << 16 | packet[offset3c + 2] << 8 | packet[offset3c + 3];
                        id3c = packet[offset3c + 4] << 8 | packet[offset3c + 5];
                        int test = packet[offset3c + 6];
                        itemCount3c = packet[offset3c + 7] << 8 | packet[offset3c + 8];
                        if (oldStyle3c)
                        {
                            containerSerial3c = packet[offset3c + 13] << 24 | packet[offset3c + 14] << 16 | packet[offset3c + 15] << 8 | packet[offset3c + 16];
                            hue3c = packet[offset3c + 17] << 8 | packet[offset3c + 18];
                        }
                        else
                        {
                            x3c = packet[offset3c + 9] << 8 | packet[offset3c + 10];
                            y3c = packet[offset3c + 11] << 8 | packet[offset3c + 12];
                            grid3c = packet[offset3c + 13];
                            containerSerial3c = packet[offset3c + 14] << 24 | packet[offset3c + 15] << 16 | packet[offset3c + 16] << 8 | packet[offset3c + 17];
                            hue3c = packet[offset3c + 18] << 8 | packet[offset3c + 19];
                        }
                        if (container3c == null) container3c = new ItemCollection(client, containerSerial3c, count3c);
                        Item item3c = new Item(serial3c, containerSerial3c);
                        item3c.ID = id3c;
                        item3c.Count = itemCount3c;
                        item3c.Hue = hue3c;
                        item3c.Grid = grid3c;
                        item3c.X = x3c;
                        item3c.Y = y3c;
                        container3c.Add(item3c);
                    }
                    if (container3c != null)
                        IncomingPackets.OnContainerContents(client, container3c);
                    return;
/*
                    int size3c = packet[1] | packet[2] << 8;
                    if (size3c == 5) return;
                    bool oldStyle3c = false;
                    if ((size3c - 5) % 20 != 0)
                        oldStyle3c = true; //sometimes we still get old packets?
                    int count3c = packet[3] << 8 | packet[4];
                    ItemCollection container3c = null;
                    int containerSerial3c, serial3c, offset3c, id3c, itemCount3c, hue3c, grid3c = 0, x3c = 0, y3c = 0;
                    for (int x = 0; x < count3c; x++)
                    {
                        if (oldStyle3c)
                        {
                            offset3c = (x * 19) + 5;
                        }
                        else
                        {
                            offset3c = (x * 20) + 5;
                        }
                        serial3c = packet[offset3c] << 24 | packet[offset3c + 1] << 16 | packet[offset3c + 2] << 8 | packet[offset3c + 3];
                        id3c = packet[offset3c + 4] << 8 | packet[offset3c + 5];
                        int test = packet[offset3c + 6];
                        itemCount3c = packet[offset3c + 7] << 8 | packet[offset3c + 8];
                        if (oldStyle3c)
                        {
                            containerSerial3c = packet[offset3c + 13] << 24 | packet[offset3c + 14] << 16 | packet[offset3c + 15] << 8 | packet[offset3c + 16];
                            hue3c = packet[offset3c + 17] << 8 | packet[offset3c + 18];
                        }
                        else
                        {
                            x3c = packet[offset3c + 9] << 8 | packet[offset3c + 10];
                            y3c = packet[offset3c + 11] << 8 | packet[offset3c + 12];
                            grid3c = packet[offset3c + 13];
                            containerSerial3c = packet[offset3c + 14] << 24 | packet[offset3c + 15] << 16 | packet[offset3c + 16] << 8 | packet[offset3c + 17];
                            hue3c = packet[offset3c + 18] << 8 | packet[offset3c + 19];
                        }
                        if (container3c == null) container3c = new ItemCollection(client, containerSerial3c, count3c);
                        Item item3c = new Item(serial3c, containerSerial3c);
                        item3c.ID = id3c;
                        item3c.Count = itemCount3c;
                        item3c.Hue = hue3c;
                        item3c.Grid = grid3c;
                        item3c.X = x3c;
                        item3c.Y = y3c;
                        container3c.Add(item3c);
                    }
                    if (container3c != null)
                        IncomingPackets.OnContainerContents(client, container3c);
                    return;
 */ 
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
                case 0x4E: //personal light level
                    int i4e = packet[5];
                    return;
                case 0x4F: //global light level
                    int i4f = packet[1];
                    return;
                case 0x50:
                    break;
                case 0x51:
                    break;
                case 0x52:
                    break;
                case 0x53:
                    break;
                case 0x54: //sound
                    int effect54 = packet[2] << 8 | packet[3];
                    int vol54 = packet[4] << 8 | packet[5];
                    int x54 = packet[6] << 8 | packet[7];
                    int y54 = packet[8] << 8 | packet[9];
                    int z54 = packet[10] << 8 | packet[11];
                    IncomingPackets.OnSound(client, packet[1], effect54, vol54, x54, y54, z54);
                    return;
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
                case 0x6C: //target cursor
                    IncomingPackets.OnTarget(client, packet[1]);
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
                case 0x77: //mobile moving
                    int serial77 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int id77 = packet[5] << 8 | packet[6];
                    int x77 = packet[7] << 8 | packet[8];
                    int y77 = packet[9] << 8 | packet[10];
                    int z77 = (sbyte)packet[11];
                    int direction77 = packet[12] & 0x07;
                    int hue77 = packet[13] << 8 | packet[14];
                    int status77 = packet[15];
                    int noto77 = packet[16];
                    IncomingPackets.OnMobileMoving(client, serial77, id77, x77, y77, z77, direction77, hue77, status77, noto77);
                    return;
                case 0x78: //add equipped mob
                    int serial78 = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    ItemCollection container78 = new ItemCollection(serial78, 125);
                    Mobile mob78 = new Mobile(serial78, client);
                    mob78.myID = packet[7] << 8 | packet[8];
                    mob78.myX = packet[9] << 8 | packet[10];
                    mob78.myY = packet[11] << 8 | packet[12];
                    mob78.myZ = (sbyte)packet[13];
                    mob78.myDirection = packet[14] & 0x07;
                    mob78.myHue = packet[15] << 8 | packet[16];
                    mob78.myStatus = packet[17];
                    mob78.myNotoriety = packet[18];
                    int itemSerial78;
                    int i78 = 19; //index
                    Item item78;
                    for (; ; )
                    {
                        itemSerial78 = packet[i78] << 24 | packet[i78 + 1] << 16 | packet[i78 + 2] << 8 | packet[i78 + 3];
                        if (itemSerial78 == 0) break;
                        item78 = new Item(itemSerial78);
                        item78.Owner = serial78;
                        //i78 += 4;
                        item78.ID = packet[i78 + 4] << 8 | packet[i78 + 5];
                        //i78 += 2;
                        item78.Layer = (Layer)packet[i78 + 6];
                        //i78 += 1;
                        if ((item78.myID & 0x8000) != 0)
                        {
                            item78.myID ^= 0x8000;
                            item78.myHue = packet[i78 + 7] << 8 | packet[i78 + 8];
                            i78 += 2;
                        }
                        i78 += 7;
                        container78.Add(item78);
                    }
                    IncomingPackets.OnEquippedMobAdded(client, mob78, container78);
                    return;
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
                case 0x89: // corpse equipment
                    return;
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
                    int serial98 = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    string name98 = ASCIIEncoding.ASCII.GetString(packet, 7, 30).TrimEnd('\0');
                    IncomingPackets.OnMobileName(client, serial98, name98);
                    return;
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
                case 0xA1: //update health
                    int seriala1 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int maxHealth = packet[5] << 8 | packet[6];
                    int health = packet[7] << 8 | packet[8];
                    IncomingPackets.OnHealthUpdated(client, seriala1, maxHealth, health);
                    return;
                case 0xA2: //update mana
                    int seriala2 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int maxMana = packet[5] << 8 | packet[6];
                    int mana = packet[7] << 8 | packet[8];
                    IncomingPackets.OnManaUpdated(client, seriala2, maxMana, mana);
                    return;
                case 0xA3: //update stamina
                    int seriala3 = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int maxStamina = packet[5] << 8 | packet[6];
                    int stamina = packet[7] << 8 | packet[8];
                    IncomingPackets.OnStaminaUpdated(client, seriala3, maxStamina, stamina);
                    return;
                case 0xA4:
                    break;
                case 0xA5:
                    break;
                case 0xA6:
                    break;
                case 0xA7:
                    break;
                case 0xA8: //server list
                    int lena8 = packet[4] << 8 | packet[5];
                    ServerInfo[] sia8 = new ServerInfo[lena8];
                    int offseta8 = 6;
                    for (int x = 0; x < lena8; x++)
                    {
                        sia8[x] = new ServerInfo();
                        sia8[x].Name = ASCIIEncoding.ASCII.GetString(packet, offseta8 + 2, 32).TrimEnd('\0');
                        sia8[x].PercentFull = packet[offseta8 + 34];
                        sia8[x].Timezone = packet[offseta8 + 35];
                        sia8[x].IP = BitConverter.ToUInt32(packet, offseta8 + 36);
                        offseta8 += 40;
                    }
                    IncomingPackets.OnServerList(client, sia8);
                    return;
                case 0xA9: //character list
                    int offseta9 = 4;
                    string[] charsa9 = new string[packet[3]];
                    for (int x = 0; x < packet[3]; x++)
                    {
                        charsa9[x] = ASCIIEncoding.ASCII.GetString(packet, offseta9, 30).TrimEnd('\0');
                        offseta9 += 60;
                    }
                    IncomingPackets.OnCharacterList(client, charsa9);
                    return;
                case 0xAA: //current attack target
                    int serialaa = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    IncomingPackets.OnAttackTarget(client, serialaa);
                    return;
                case 0xAB:
                    break;
                case 0xAC:
                    break;
                case 0xAD: //speech
                    return;
                case 0xAE: //unicode text
                    JournalEntry jeae = new JournalEntry();
                    jeae.serial = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    jeae.id = packet[7] << 8 | packet[8];
                    jeae.speechType = (JournalSpeech)packet[9];
                    jeae.speechHue = packet[10] << 8 | packet[11];
                    jeae.speechFont = packet[12] << 8 | packet[13];
                    jeae.speechLanguage = ASCIIEncoding.ASCII.GetString(packet, 14, 3);
                    jeae.name = ASCIIEncoding.ASCII.GetString(packet, 18, 30).TrimEnd('\0');
                    jeae.text = UnicodeEncoding.BigEndianUnicode.GetString(packet, 48, packet.Length - 48).TrimEnd('\0');
                    IncomingPackets.OnUnicodeText(client, jeae);
                    General.OnJournalEntry(client, jeae);
                    return;
                case 0xAF: //mob death
                    int serialaf = packet[1] << 24 | packet[2] << 16 | packet[3] << 8 | packet[4];
                    int corpseaf = packet[5] << 24 | packet[6] << 16 | packet[7] << 8 | packet[8];
                    IncomingPackets.OnMobileDeath(client, serialaf, corpseaf);
                    return;
                case 0xB0: //old style generic gump
                    int serialb0 = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    int idb0 = packet[7] << 24 | packet[8] << 16 | packet[9] << 8 | packet[10];
                    int xb0 = packet[11] << 24 | packet[12] << 16 | packet[13] << 8 | packet[14];
                    int yb0 = packet[15] << 24 | packet[16] << 16 | packet[17] << 8 | packet[18];
                    int layoutLenb0 = packet[19] << 8 | packet[20];
                    string layoutb0 = ASCIIEncoding.ASCII.GetString(packet, 21, layoutLenb0).TrimEnd('\0');
                    int offsetb0 = 21 + layoutLenb0;
                    int linesb0 = packet[offsetb0] << 8 | packet[offsetb0 + 1];
                    string[] textb0 = new string[linesb0];
                    offsetb0 += 2;
                    int textLenb0;
                    for (int x = 0; x < linesb0; x++)
                    {
                        textLenb0 = (packet[offsetb0] << 8 | packet[offsetb0 + 1]) * 2;
                        offsetb0 += 2;
                        textb0[x] = UnicodeEncoding.BigEndianUnicode.GetString(packet, offsetb0, textLenb0);
                        offsetb0 += textLenb0;
                    }
                    IncomingPackets.OnGenericGump(client, serialb0, idb0, xb0, yb0, layoutb0, textb0);
                    return;
                case 0xB1:
                    break;
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

                    if (commandbf == 0x04) //close gump
                    {
                        int gumpID = packet[5] << 24 | packet[6] << 16 | packet[7] << 8 | packet[8];
                        int buttonID = packet[9] << 24 | packet[10] << 16 | packet[11] << 8 | packet[12];
                        IncomingPackets.OnCloseGump(client, gumpID, buttonID);
                        return;
                    }

                    if (commandbf == 0x06) //party command
                    {
                        if (packet[5] == 0x04) //subcommand 4, party chat
                        {
                            JournalEntry jebf = new JournalEntry();
                            jebf.serial = packet[6] << 24 | packet[7] << 16 | packet[8] << 8 | packet[9];
                            jebf.text = UnicodeEncoding.BigEndianUnicode.GetString(packet, 10, packet.Length - 10).TrimEnd('\0');
                            IncomingPackets.OnPartyText(client, jebf);
                            General.OnJournalEntry(client, jebf);
                        }
                        return;
                    }

                    if (commandbf == 0x08) //map select
                    {
                        IncomingPackets.OnMapChanged(client, packet[5]);
                        return;
                    }

                    if (commandbf == 0x14) //context menu
                    {
                        int type = packet[5] << 8 | packet[6];
                        if (type != 1 && type != 2) break;
                        int serial = packet[7] << 24 | packet[8] << 16 | packet[9] << 8 | packet[10];
                        int len = packet[11];
                        int offsetbf14 = 12;
                        int entry, cliloc, flags, hue, count = 0;
                        ContextEntry[] ce = new ContextEntry[len];
                        if (type == 1)
                        {
                            for (; ; )
                            {
                                count++;
                                entry = packet[offsetbf14] << 8 | packet[offsetbf14 + 1];
                                offsetbf14 += 2;
                                cliloc = (packet[offsetbf14] << 8 | packet[offsetbf14 + 1]) + 3000000;
                                offsetbf14 += 2;
                                flags = packet[offsetbf14] << 8 | packet[offsetbf14 + 1];
                                hue = 0;
                                if ((flags & 0x20) == 0x20)
                                {
                                    offsetbf14 += 2;
                                    hue = packet[offsetbf14] << 8 | packet[offsetbf14 + 1];
                                }
                                offsetbf14 += 2;
                                string text = Cliloc.GetProperty(cliloc);
                                ce[entry] = new ContextEntry(client, entry, serial, text, flags, hue);
                                if (count == len) break;
                            }
                            IncomingPackets.OnContextMenu(client, ce);
                            return;
                        }
                        if (type == 2) // KR / SA
                        {
                            for (int x = 0; x < len; x++)
                            {
                                cliloc = (packet[offsetbf14] << 24 | packet[offsetbf14 + 1] << 16 | packet[offsetbf14 + 2] << 8 | packet[offsetbf14 + 3]);
                                offsetbf14 += 4;
                                entry = packet[offsetbf14] << 8 | packet[offsetbf14 + 1];
                                offsetbf14 += 2;
                                flags = packet[offsetbf14] << 8 | packet[offsetbf14 + 1];
                                hue = 0;
                                if ((flags & 0x20) == 0x20)
                                {
                                    offsetbf14 += 2;
                                    hue = packet[offsetbf14] << 8 | packet[offsetbf14 + 1];
                                }
                                offsetbf14 += 2;
                                string text = Cliloc.GetProperty(cliloc);
                                ce[x] = new ContextEntry(client, entry, serial, text, flags, hue);
                            }
                            IncomingPackets.OnContextMenu(client, ce);
                            return;
                        }
                    }

                    if (commandbf == 0x19) //misc. status
                    {
                        if (packet[5] == 0x00) //subcommand 0, bonded status
                        {
                            int serialbf = packet[6] << 24 | packet[7] << 16 | packet[8] << 8 | packet[9];
                            IncomingPackets.OnBondedStatus(client, serialbf, packet[10] == 1);
                            return;
                        }

                        if (packet[5] == 0x02) //subcommand 2, stat lock status
                        {
                            int serialbf = packet[6] << 24 | packet[7] << 16 | packet[8] << 8 | packet[9];
                            IncomingPackets.OnStatLockStatus(client, serialbf, packet[11]);
                            return;
                        }

                        if (packet[5] == 0x05) //subcommand 5, KR / SA stat lock status, bonded status, mobile status
                        {
                            int serialbf;
                            if (packet[11] == 0xFF)
                            {
                                serialbf = packet[6] << 24 | packet[7] << 16 | packet[8] << 8 | packet[9];
                                IncomingPackets.OnBondedStatus(client, serialbf, packet[10] == 1);
                                return;
                            }
                            serialbf = packet[6] << 24 | packet[7] << 16 | packet[8] << 8 | packet[9];
                            IncomingPackets.OnStatLockStatus(client, serialbf, packet[11]);
                            return;
                        }
                    }
                    return;
                case 0xC0:
                    break;
                case 0xC1: //localized text
                    JournalEntry jec1 = new JournalEntry();
                    jec1.serial = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    jec1.id = packet[7] << 8 | packet[8];
                    jec1.speechType = (JournalSpeech)packet[9];
                    if (packet[9] == 0xC0)
                    {
                        Log.LogDataMessage(client, packet, "Incoming encoded C1 packet:\r\n");
                        return;
                    }
                    jec1.speechHue = packet[10] << 8 | packet[11];
                    jec1.speechFont = packet[12] << 8 | packet[13];
                    int messagec1 = packet[14] << 24 | packet[15] << 16 | packet[16] << 8 | packet[17];
                    jec1.name = ASCIIEncoding.ASCII.GetString(packet, 18, 30).TrimEnd('\0');
                    string[] argumentsc1 = UnicodeEncoding.Unicode.GetString(packet, 48, packet.Length - 50).Split('\t');
                    jec1.text = Cliloc.GetLocalString(messagec1, argumentsc1);
                    IncomingPackets.OnLocalizedText(client, jec1);
                    General.OnJournalEntry(client, jec1);
                    return;
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
                case 0xCC: //localized text + string
                    return;
                case 0xCD:
                    break;
                case 0xCE:
                    break;
                case 0xCF:
                    break;
                case 0xD0:
                    break;
                case 0xD1: //logout confirmation
                    IncomingPackets.OnLoggedOut(client);
                    return;
                case 0xD2:
                    break;
                case 0xD3:
                    break;
                case 0xD4:
                    break;
                case 0xD5:
                    break;
                case 0xD6: //item properties
                    int seriald6 = packet[5] << 24 | packet[6] << 16 | packet[7] << 8 | packet[8];
                    StringBuilder propertyText = new StringBuilder();
                    string named6 = "";
                    bool nameSet = false, first = true;
                    int offsetd6 = 15;
                    List<Property> propertyList = new List<Property>();
                    Property p;
                    int lastCliloc = -1;
                    for (; ; )
                    {
                        p = new Property();
                        p.Cliloc = packet[offsetd6] << 24 | packet[offsetd6 + 1] << 16 | packet[offsetd6 + 2] << 8 | packet[offsetd6 + 3];
                        if (p.Cliloc == 0) break;
                        if (!first) propertyText.Append("\r\n");
                        offsetd6 += 4;
                        int lend6 = packet[offsetd6] << 8 | packet[offsetd6 + 1];
                        offsetd6 += 2;
                        if (lend6 > 0)
                        {
                            p.Arguments = UnicodeEncoding.Unicode.GetString(packet, offsetd6, lend6).Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            p.Text = Cliloc.GetLocalString(p.Cliloc, p.Arguments);
                            if (!nameSet)
                            {
                                named6 = p.Text;
                                nameSet = true;
                            }
                            offsetd6 += lend6;
                        }
                        else
                        {
                            p.Text = Cliloc.GetProperty(p.Cliloc);
                            if (!nameSet)
                            {
                                named6 = p.Text;
                                nameSet = true;
                            }
                        }
                        if (lastCliloc != -1)
                        {
                            if (lastCliloc != p.Cliloc)
                            {
                                propertyList.Add(p);
                                propertyText.Append(p.Text);
                            }
                        }
                        else
                        {
                            propertyList.Add(p);
                            propertyText.Append(p.Text);
                        }
                        lastCliloc = p.Cliloc;
                        first = false;
                    }
                    IncomingPackets.OnProperties(client, seriald6, named6, propertyList.ToArray(), propertyText.ToString());
                    return;
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
                case 0xDD: //generic gump
                    int serialdd = packet[3] << 24 | packet[4] << 16 | packet[5] << 8 | packet[6];
                    int iddd = packet[7] << 24 | packet[8] << 16 | packet[9] << 8 | packet[10];
                    int xdd = packet[11] << 24 | packet[12] << 16 | packet[13] << 8 | packet[14];
                    int ydd = packet[15] << 24 | packet[16] << 16 | packet[17] << 8 | packet[18];
                    int compressLendd = packet[19] << 24 | packet[20] << 16 | packet[21] << 8 | packet[22];
                    if (compressLendd <= 4) return;
                    else compressLendd -= 4;
                    int decompressLendd = packet[23] << 24 | packet[24] << 16 | packet[25] << 8 | packet[26] + 1;
                    byte[] decompresseddd = new byte[decompressLendd];
                    byte[] compresseddd = new byte[compressLendd];
                    Buffer.BlockCopy(packet, 27, compresseddd, 0, compressLendd);
                    int success;
                    //if (IntPtr.Size == 8) success = Win32.uncompress64(decompresseddd, ref decompressLendd, compresseddd, compressLendd);
                    success = Win32.uncompress32(decompresseddd, ref decompressLendd, compresseddd, compressLendd);
                    if (success != 0)
                    {
                        Log.LogDataMessage(client, packet, "*** Error decompressing gump layout:");
                        return;
                    }
                    
                    string layoutdd = ASCIIEncoding.ASCII.GetString(decompresseddd).TrimEnd('\0');
                    int offsetdd = 27 + compressLendd;
                    int linesdd = packet[offsetdd] << 24 | packet[offsetdd + 1] << 16 | packet[offsetdd + 2] << 8 | packet[offsetdd + 3];
                    compressLendd = packet[offsetdd + 4] << 24 | packet[offsetdd + 5] << 16 | packet[offsetdd + 6] << 8 | packet[offsetdd + 7];
                    string[] textdd = new string[linesdd];
                    if (compressLendd > 4)
                    {
                        compressLendd -= 4;
                        compresseddd = new byte[compressLendd];
                        Buffer.BlockCopy(packet, offsetdd + 12, compresseddd, 0, compressLendd);
                        decompressLendd = packet[offsetdd + 8] << 24 | packet[offsetdd + 9] << 16 | packet[offsetdd + 10] << 8 | packet[offsetdd + 11] + 1;
                        decompresseddd = new byte[decompressLendd];
                        //if (IntPtr.Size == 8) success = Win32.uncompress64(decompresseddd, ref decompressLendd, compresseddd, compressLendd);
                        success = Win32.uncompress32(decompresseddd, ref decompressLendd, compresseddd, compressLendd);
                        if (success != 0)
                        {
                            Log.LogDataMessage(client, packet, "*** Error decompressing gump strings:");
                            return;
                        }
                        offsetdd = 0;
                        int lendd = 0;
                        for (int x = 0; x < linesdd; x++)
                        {
                            lendd = (decompresseddd[offsetdd] << 8 | decompresseddd[offsetdd + 1]) * 2;
                            offsetdd += 2;
                            textdd[x] = UnicodeEncoding.BigEndianUnicode.GetString(decompresseddd, offsetdd, lendd);
                            offsetdd += lendd;
                        }
                    }
                    IncomingPackets.OnGenericGump(client, serialdd, iddd, xdd, ydd, layoutdd, textdd);
                    return;
                case 0xDE:
                    break;
                case 0xDF: //buff \ debuff
                    return;
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
                case 0xF3: //stygian abyss world item
                    int serialf3 = packet[4] << 24 | packet[5] << 16 | packet[6] << 8 | packet[7];
                    Item itemf3 = new Item(serialf3);
                    itemf3.ArtDataID = packet[3];
                    itemf3.ID = packet[8] << 8 | packet[9];
                    itemf3.Direction = packet[10];
                    itemf3.Count = packet[11] << 8 | packet[12];
                    itemf3.X = packet[15] << 8 | packet[16];
                    itemf3.Y = packet[17] << 8 | packet[18];
                    itemf3.Z = (sbyte)packet[19];
                    itemf3.Light = packet[20];
                    itemf3.Hue = packet[21] << 8 | packet[22];
                    itemf3.Flags = packet[23];
                    IncomingPackets.OnWorldItemAdded(client, itemf3);
                    return;
                default:
                    break;
            }
            Log.LogDataMessage(client, packet, "--- Incoming packet:\r\n");
        }
    }
}