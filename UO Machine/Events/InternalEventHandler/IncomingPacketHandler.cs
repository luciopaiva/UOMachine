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
 * You should have  a copy of the GNU General Public License
 * along with UO Machine.  If not, see <http://www.gnu.org/licenses/>. */

using System;
using UOMachine.Tree;
using UOMachine.Utility;
using UOMachine.Data;

namespace UOMachine.Events
{
    internal static partial class InternalEventHandler
    {
        internal static class IncomingPacketHandler
        {
            internal static void Initialize()
            {
                IncomingPackets.InternalContainerContentsEvent += new IncomingPackets.dContainerContents(IncomingPackets_ContainerContentsEvent);
                IncomingPackets.InternalEquippedMobAddedEvent += new IncomingPackets.dEquippedMobAdded(IncomingPackets_EquippedMobAddedEvent);
                IncomingPackets.InternalHealthUpdatedEvent += new IncomingPackets.dHealthUpdated(IncomingPackets_HealthUpdatedEvent);
                IncomingPackets.InternalItemAddedToContainerEvent += new IncomingPackets.dItemAddedToContainer(IncomingPackets_ItemAddedToContainerEvent);
                IncomingPackets.InternalItemDeletedEvent += new IncomingPackets.dItemDeleted(IncomingPackets_ItemDeletedEvent);
                IncomingPackets.InternalLocalizedTextEvent += new IncomingPackets.dLocalizedText(IncomingPackets_LocalizedTextEvent);
                IncomingPackets.InternalManaUpdatedEvent += new IncomingPackets.dManaUpdated(IncomingPackets_ManaUpdatedEvent);
                IncomingPackets.InternalMapChangedEvent += new IncomingPackets.dMapChanged(IncomingPackets_MapChangedEvent);
                IncomingPackets.InternalMobileMovingEvent += new IncomingPackets.dMobileMoving(IncomingPackets_MobileMovingEvent);
                IncomingPackets.InternalMobileUpdatedEvent += new IncomingPackets.dMobileUpdated(IncomingPackets_MobileUpdatedEvent);
                IncomingPackets.InternalPlayerInitializedEvent += new IncomingPackets.dPlayerInitialized(IncomingPackets_PlayerInitializedEvent);
                IncomingPackets.InternalStaminaUpdatedEvent += new IncomingPackets.dStaminaUpdated(IncomingPackets_StaminaUpdatedEvent);
                IncomingPackets.InternalWorldItemAddedEvent += new IncomingPackets.dWorldItemAdded(IncomingPackets_WorldItemAddedEvent);
                IncomingPackets.InternalPropertiesEvent += new IncomingPackets.dProperties(IncomingPackets_PropertiesEvent);
                IncomingPackets.InternalMoveAcceptedEvent += new IncomingPackets.dMoveAccepted(IncomingPackets_MoveAcceptedEvent);
                IncomingPackets.InternalMoveRejectedEvent += new IncomingPackets.dMoveRejected(IncomingPackets_MoveRejectedEvent);
                IncomingPackets.InternalShortStatusEvent += new IncomingPackets.dShortStatus(IncomingPackets_ShortStatusEvent);
                IncomingPackets.InternalLongStatusEvent += new IncomingPackets.dLongStatus(IncomingPackets_LongStatusEvent);
                IncomingPackets.InternalSkillUpdateEvent += new IncomingPackets.dSkillUpdate(IncomingPackets_SkillUpdateEvent);
                IncomingPackets.InternalSkillListEvent += new IncomingPackets.dSkillList(IncomingPackets_SkillListEvent);
                IncomingPackets.InternalTargetEvent += new IncomingPackets.dTarget(IncomingPackets_TargetEvent);
                IncomingPackets.InternalStatLockStatusEvent += new IncomingPackets.dStatLockStatus(IncomingPackets_StatLockStatusEvent);
                IncomingPackets.InternalMobileNameEvent += new IncomingPackets.dMobileName(IncomingPackets_MobileNameEvent);
                IncomingPackets.InternalUnicodeTextEvent += new IncomingPackets.dUnicodeText(IncomingPackets_UnicodeTextEvent);
                IncomingPackets.InternalTextEvent += new IncomingPackets.dText(IncomingPackets_TextEvent);
                IncomingPackets.InternalPartyTextEvent += new IncomingPackets.dPartyText(IncomingPackets_PartyTextEvent);
                IncomingPackets.InternalItemEquippedEvent += new IncomingPackets.dItemEquipped(IncomingPackets_InternalItemEquippedEvent);
                IncomingPackets.InternalAttackTargetEvent += new IncomingPackets.dAttackTarget(IncomingPackets_InternalAttackTargetEvent);
                IncomingPackets.InternalGenericGumpEvent += new IncomingPackets.dGenericGump(IncomingPackets_InternalGenericGumpEvent);
                IncomingPackets.InternalCloseGumpEvent += new IncomingPackets.dCloseGump(IncomingPackets_InternalCloseGumpEvent);
            }

            private static void IncomingPackets_InternalCloseGumpEvent(int client, int gumpID, int buttonID)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    ci.GenericGumps.Remove(gumpID);
                }
            }

            private static void IncomingPackets_InternalGenericGumpEvent(int client, int serial, int ID, int x, int y, string layout, string[] text)
            {
                Gump g = GumpParser.Parse(client, serial, ID, x, y, layout, text);
                if (g != null)
                {
                    ClientInfo ci;
                    if (ClientInfoCollection.GetClient(client, out ci))
                    {
                        ci.GenericGumps.Add(g);
                        General.OnCustomGump(client, g);
                    }
                }
            }

            private static void IncomingPackets_InternalAttackTargetEvent(int client, int serial)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    ci.CurrentAttackTarget = serial;
                }
            }

            private static void IncomingPackets_InternalItemEquippedEvent(int client, int serial, int ID, Layer layer, int mobileSerial, int hue)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    if (ci.Player.Serial == mobileSerial)
                        ci.Player.SetLayer((int)layer, serial);
                    else
                    {
                        Mobile m;
                        if (ci.Mobiles.GetMobile(mobileSerial, out m))
                        {
                            try
                            {
                            m.SetLayer((int)layer, serial);
                        }
                            catch (Exception e) { }
                        }
                    }
                    Item i;
                    if (ci.Items.GetItem(serial, out i))
                    {
                        i.Layer = layer;
                        i.Owner = mobileSerial;
                    }
                    else
                    {
                        i = new Item(serial);
                        i.Owner = mobileSerial;
                        i.Layer = layer;
                        i.Hue = hue;
                        i.ID = ID;
                        ci.Items.Add(i);
                    }
                }
            }

            private static void IncomingPackets_PartyTextEvent(int client, JournalEntry journalEntry)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    ci.Journal.Write(journalEntry);
                }
            }

            private static void IncomingPackets_TextEvent(int client, JournalEntry journalEntry)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    ci.Journal.Write(journalEntry);
                }
            }

            private static void IncomingPackets_UnicodeTextEvent(int client, JournalEntry journalEntry)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    ci.Journal.Write(journalEntry);
                }
            }

            private static void IncomingPackets_MobileNameEvent(int client, int serial, string name)
            {
                Mobile m;
                if (ClientInfoCollection.GetMobile(client, serial, out m))
                    m.Name = name;
            }

            private static void IncomingPackets_StatLockStatusEvent(int client, int serial, int statLockStatus)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    if (ci.Player != null && ci.Player.Serial == serial) ci.Player.StatLock = statLockStatus;
                }
            }

            private static void IncomingPackets_TargetEvent(int client, int targetType)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci)) ci.TargetReceived();
            }

            private static void IncomingPackets_SkillUpdateEvent(int client, int skillID, float value, float baseValue, LockStatus lockStatus, float skillCap)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    SkillInfo si = ci.GetSkillInfo(skillID);
                    if (si != null)
                    {
                        si.BaseValue = baseValue;
                        si.LockStatus = lockStatus;
                        si.SkillCap = skillCap;
                        si.Value = value;
                    }
                }
            }

            private static void IncomingPackets_SkillListEvent(int client, SkillInfo[] skills)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                    ci.Skills = skills;
            }


            private static void IncomingPackets_LongStatusEvent(int client, int serial, PlayerStatus playerStatus)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    PlayerMobile p = ci.Player;
                    if (p == null || p.Serial != serial) return;
                    switch (playerStatus.Features)
                    {
                        case 3:
                            p.Health = playerStatus.Health;
                            p.MaxHealth = playerStatus.MaxHealth;
                            p.Sex = playerStatus.Sex;
                            p.Str = playerStatus.Str;
                            p.Dex = playerStatus.Dex;
                            p.Int = playerStatus.Int;
                            p.Stamina = playerStatus.Stamina;
                            p.MaxStamina = playerStatus.MaxStamina;
                            p.Mana = playerStatus.Mana;
                            p.MaxMana = playerStatus.MaxMana;
                            p.Gold = playerStatus.Gold;
                            p.PhysicalResist = playerStatus.PhysicalResist;
                            p.Weight = playerStatus.Weight;

                            p.StatCap = playerStatus.StatCap;
                            p.Followers = playerStatus.Followers;
                            p.MaxFollowers = playerStatus.MaxFollowers;
                            break;
                        case 4:
                            p.Health = playerStatus.Health;
                            p.MaxHealth = playerStatus.MaxHealth;
                            p.Sex = playerStatus.Sex;
                            p.Str = playerStatus.Str;
                            p.Dex = playerStatus.Dex;
                            p.Int = playerStatus.Int;
                            p.Stamina = playerStatus.Stamina;
                            p.MaxStamina = playerStatus.MaxStamina;
                            p.Mana = playerStatus.Mana;
                            p.MaxMana = playerStatus.MaxMana;
                            p.Gold = playerStatus.Gold;
                            p.PhysicalResist = playerStatus.PhysicalResist;
                            p.Weight = playerStatus.Weight;

                            p.StatCap = playerStatus.StatCap;
                            p.Followers = playerStatus.Followers;
                            p.MaxFollowers = playerStatus.MaxFollowers;
                            p.FireResist = playerStatus.FireResist;
                            p.ColdResist = playerStatus.ColdResist;
                            p.PoisonResist = playerStatus.PoisonResist;
                            p.EnergyResist = playerStatus.EnergyResist;
                            p.Luck = playerStatus.Luck;
                            p.MinDamage = playerStatus.MinDamage;
                            p.MaxDamage = playerStatus.MaxDamage;
                            p.TithingPoints = playerStatus.TithingPoints;
                            break;
                        case 5:
                            p.Health = playerStatus.Health;
                            p.MaxHealth = playerStatus.MaxHealth;
                            p.Sex = playerStatus.Sex;
                            p.Str = playerStatus.Str;
                            p.Dex = playerStatus.Dex;
                            p.Int = playerStatus.Int;
                            p.Stamina = playerStatus.Stamina;
                            p.MaxStamina = playerStatus.MaxStamina;
                            p.Mana = playerStatus.Mana;
                            p.MaxMana = playerStatus.MaxMana;
                            p.Gold = playerStatus.Gold;
                            p.PhysicalResist = playerStatus.PhysicalResist;
                            p.Weight = playerStatus.Weight;

                            p.MaxWeight = playerStatus.MaxWeight;
                            p.Race = playerStatus.Race;
                            p.StatCap = playerStatus.StatCap;
                            p.Followers = playerStatus.Followers;
                            p.MaxFollowers = playerStatus.MaxFollowers;
                            p.FireResist = playerStatus.FireResist;
                            p.ColdResist = playerStatus.ColdResist;
                            p.PoisonResist = playerStatus.PoisonResist;
                            p.EnergyResist = playerStatus.EnergyResist;
                            p.Luck = playerStatus.Luck;
                            p.MinDamage = playerStatus.MinDamage;
                            p.MaxDamage = playerStatus.MaxDamage;
                            p.TithingPoints = playerStatus.TithingPoints;
                            break;
                        case 6:
                            goto case 5;
                    }
                }
            }

            private static void IncomingPackets_ShortStatusEvent(int client, int serial, string name, int health, int maxHealth, int sex)
            {
                Mobile m;
                if (ClientInfoCollection.GetMobile(client, serial, out m))
                {
                    m.Name = name;
                    m.Health = health;
                    m.MaxHealth = maxHealth;
                    m.Sex = sex;
                }
            }

            private static void IncomingPackets_MoveRejectedEvent(int client, int sequence, int x, int y, int z, int direction)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    PlayerMobile p = ci.Player;
                    p.X = x;
                    p.Y = y;
                    p.Z = z;
                    p.Direction = direction;
                }
            }

            private static void IncomingPackets_MoveAcceptedEvent(int client, int sequence, int status)
            {
                //status tells us if player is running or not?
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    int direction = ci.GetSequence(sequence);
                    ci.Player.UpdateLocation(direction);
                }
            }

            private static void IncomingPackets_PropertiesEvent(int client, int serial, string name, Property[] properties, string propertyText)
            {
                ClientInfoCollection.UpdateProperties(client, serial, name, properties, propertyText);
            }

            private static void IncomingPackets_WorldItemAddedEvent(int client, Item item)
            {
                ClientInfoCollection.AddItem(client, item);
            }

            private static void IncomingPackets_PlayerInitializedEvent(int client, PlayerMobile player)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    if (ci.Player == null)
                    {
                        ci.Player = player;
                    }
                    else
                    {
                        ci.Player.ID = player.ID;
                        ci.Player.Serial = player.Serial;
                        ci.Player.X = player.X;
                        ci.Player.Y = player.Y;
                        ci.Player.Z = player.Z;
                        ci.Player.Direction = player.Direction;
                        ci.Player.Client = client;
                    }
                    TreeViewUpdater.EditPlayerNode(client, ci.Player);
                }
            }

            private static void IncomingPackets_MobileUpdatedEvent(int client, int serial, int ID, int hue, int status, int x, int y, int z, int direction)
            {
                Mobile m;
                if (ClientInfoCollection.GetMobile(client, serial, out m))
                {
                    m.ID = ID;
                    m.X = x;
                    m.Y = y;
                    m.Z = z;
                    m.Direction = direction;
                    m.Hue = hue;
                    m.Status = status;
                }
                else
                {
                    ClientInfo ci;
                    if (ClientInfoCollection.GetClient(client, out ci))
                    {
                        PlayerMobile p = ci.Player;
                        if (p != null && p.Serial == serial)
                        {
                            p.ID = ID;
                            p.X = x;
                            p.Y = y;
                            p.Z = z;
                            p.Direction = direction;
                            p.Hue = hue;
                            p.Status = status;
                        }
                    }
                }
            }

            private static void IncomingPackets_MobileMovingEvent(int client, int serial, int ID, int x, int y, int z, int direction, int hue, int status, int notoriety)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci) && ci.Player.Serial == serial)
                {
                    PlayerMobile p = ci.Player;
                    p.ID = ID;
                    p.X = x;
                    p.Y = y;
                    p.Z = z;
                    p.Direction = direction;
                    p.Hue = hue;
                    p.Status = status;
                    p.Notoriety = notoriety;
                }
                else
                {
                    Mobile m;
                    if (ClientInfoCollection.GetMobile(client, serial, out m))
                    {
                        m.ID = ID;
                        m.X = x;
                        m.Y = y;
                        m.Z = z;
                        m.Direction = direction;
                        m.Hue = hue;
                        m.Status = status;
                        m.Notoriety = notoriety;
                    }
                    else
                    {
                        Mobile newMobile = new Mobile(serial, client);
                        newMobile.myID = ID;
                        newMobile.myX = x;
                        newMobile.myY = y;
                        newMobile.myZ = z;
                        newMobile.myDirection = direction;
                        newMobile.myHue = hue;
                        newMobile.myStatus = status;
                        newMobile.myNotoriety = notoriety;
                        ClientInfoCollection.ClientList[client].Mobiles.Add(newMobile);
                    }
                }
            }

            private static void IncomingPackets_MapChangedEvent(int client, int map)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                    if (ci.Player != null) ci.Player.Facet = map;
            }

            private static void IncomingPackets_StaminaUpdatedEvent(int client, int serial, int maxStamina, int stamina)
            {
                ClientInfoCollection.UpdateStamina(client, serial, maxStamina, stamina);
            }

            private static void IncomingPackets_ManaUpdatedEvent(int client, int serial, int maxMana, int mana)
            {
                ClientInfoCollection.UpdateMana(client, serial, maxMana, mana);
            }

            private static void IncomingPackets_HealthUpdatedEvent(int client, int serial, int maxHealth, int health)
            {
                ClientInfoCollection.UpdateHealth(client, serial, maxHealth, health);
            }

            private static void IncomingPackets_LocalizedTextEvent(int client, JournalEntry journalEntry)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                    ci.Journal.Write(journalEntry);

                switch (journalEntry.speechType)
                {
                    case JournalSpeech.Say:
                        break;
                    case JournalSpeech.System:
                        break;
                    case JournalSpeech.Emote:
                        break;
                    case JournalSpeech.Unknown1:
                        break;
                    case JournalSpeech.Unknown2:
                        break;
                    case JournalSpeech.Unknown3:
                        break;
                    case JournalSpeech.Label:
                        ClientInfoCollection.UpdateLabel(client, journalEntry.serial, journalEntry.name, journalEntry.text);
                        break;
                    case JournalSpeech.Focus:
                        break;
                    case JournalSpeech.Whisper:
                        break;
                    case JournalSpeech.Yell:
                        break;
                    case JournalSpeech.Spell:
                        break;
                    case JournalSpeech.Unknown4:
                        break;
                    case JournalSpeech.Unknown5:
                        break;
                    case JournalSpeech.Guild:
                        break;
                    case JournalSpeech.Alliance:
                        break;
                    case JournalSpeech.GM:
                        break;
                }
            }

            private static void IncomingPackets_ItemDeletedEvent(int client, int serial)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    if (UOMath.IsMobile(serial))
                    {
                        ci.Mobiles.Remove(serial);
                        ci.Items.RemoveByOwner(serial);
                    }
                    else
                    {
                        ci.Items.Remove(serial);
                    }
                }
            }

            private static void IncomingPackets_ItemAddedToContainerEvent(int client, Item item)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    Item i;
                    if (ci.Items.GetItem(item.ContainerSerial, out i))
                    {
                        item.Owner = i.Owner;
                        if (i.Container == null) i.Container = new ItemCollection(client, i.Serial, 125);
                        i.Container.Add(item);
                    }
                }
            }

            private static void IncomingPackets_EquippedMobAddedEvent(int client, Mobile mobile, ItemCollection equipment)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    foreach (Item i in equipment.GetItems())
                    {
                        mobile.SetLayer((int)i.Layer, i.Serial);
                        ci.Items.Add(i);
                    }
                    PlayerMobile p = ci.Player;
                    if (p != null && mobile.Serial == p.Serial)
                    {
                        p.Status = mobile.myStatus;
                        p.ID = mobile.myID;
                        p.X = mobile.myX;
                        p.Y = mobile.myY;
                        p.Z = mobile.myZ;
                        p.Direction = mobile.myDirection;
                        p.Hue = mobile.myHue;
                        p.Notoriety = mobile.myNotoriety;
                        p.myLayerArray = mobile.myLayerArray;
                    }
                    else
                    {
                        //mobile.Equipment = equipment;
                        ci.Mobiles.Add(mobile);
                    }
                }
            }

            private static void IncomingPackets_ContainerContentsEvent(int client, ItemCollection container)
            {
                Item i;
                if (ClientInfoCollection.GetItem(client, container.Serial, out i))
                {
                    i.Container = container;
                }
                else
                {
                    Item newItem = new Item(container.Serial);
                    newItem.Container = container;
                    ClientInfoCollection.AddItem(client, newItem);
                }
            }
        }
    }
}