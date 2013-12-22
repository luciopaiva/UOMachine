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
using UOMachine.Events;
using UOMachine.Utility;

namespace UOMachine.Events
{
    internal static partial class InternalEventHandler
    {
        internal static class OutgoingPacketHandler
        {
            internal static void Initialize()
            {
                OutgoingPackets.InternalSkillLockChangedEvent += new OutgoingPackets.dSkillLockChanged(OutgoingPackets_SkillLockChangedEvent);
                OutgoingPackets.InternalMoveRequestedEvent += new OutgoingPackets.dMoveRequested(OutgoingPackets_MoveRequestedEvent);
                OutgoingPackets.InternalTargetSentEvent += new OutgoingPackets.dTargetSent(OutgoingPackets_TargetSentEvent);
                OutgoingPackets.InternalUseItemRequestedEvent += new OutgoingPackets.dUseItemRequested(OutgoingPackets_UseItemRequestedEvent);
                OutgoingPackets.InternalSkillLockChangedEvent += new OutgoingPackets.dSkillLockChanged(OutgoingPackets_SkillLockChangedEvent);
                OutgoingPackets.InternalStatLockChangedEvent += new OutgoingPackets.dStatLockChanged(OutgoingPackets_StatLockChangedEvent);
                OutgoingPackets.InternalDragItemRequestedEvent += new OutgoingPackets.dDragItemRequested(OutgoingPackets_InternalDragItemRequestedEvent);
                OutgoingPackets.InternalGumpButtonPressedEvent += new OutgoingPackets.dGumpButtonPressed(OutgoingPackets_InternalGumpButtonPressedEvent);
            }

            private static void OutgoingPackets_InternalGumpButtonPressedEvent(int client, int serial, int gumpID, int buttonID, int[] switches)
            {
                 ClientInfo ci;
                 if (ClientInfoCollection.GetClient(client, out ci))
                 {
                     ci.OnGumpAction(serial, gumpID, buttonID, switches);
                     ci.GenericGumps.Remove(gumpID);
                 }
            }

            private static void OutgoingPackets_InternalDragItemRequestedEvent(int client, int serial, int amount)
            {
                ClientInfo ci;
                PlayerMobile p;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    Item i;
                    if (ci.Items.GetItem(serial, out i))
                    {
                        if (i.Layer != Layer.Invalid)
                        {
                            if (ClientInfoCollection.GetPlayer(client, out p))
                            {
                                p.SetLayer((int)i.Layer, 0);
                            }
                        }
                        i.Layer = Layer.Invalid;
                    }
                }
            }

            private static void OutgoingPackets_StatLockChangedEvent(int client, int stat, int status)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    int statLock = ci.Player.StatLock;
                    switch (stat)
                    {
                        case 0: //str
                            switch (status)
                            {
                                case 0: //up
                                    statLock ^= 0x20;
                                    statLock ^= 0x10;
                                    break;
                                case 1: //down
                                    statLock ^= 0x20;
                                    statLock |= 0x10;
                                    break;
                                case 2: //locked
                                    statLock ^= 0x10;
                                    statLock |= 0x20;
                                    break;
                            }
                            ci.Player.StatLock = statLock;
                            return;
                        case 1: //dex
                            switch (status)
                            {
                                case 0:
                                    statLock ^= 0x08;
                                    statLock ^= 0x04;
                                    break;
                                case 1:
                                    statLock ^= 0x08;
                                    statLock |= 0x04;
                                    break;
                                case 2:
                                    statLock ^= 0x04;
                                    statLock |= 0x08;
                                    break;
                            }
                            ci.Player.StatLock = statLock;
                            return;
                        case 2: //int
                            switch (status)
                            {
                                case 0:
                                    statLock ^= 0x02;
                                    statLock ^= 0x01;
                                    break;
                                case 1:
                                    statLock ^= 0x02;
                                    statLock |= 0x01;
                                    break;
                                case 2:
                                    statLock ^= 0x01;
                                    statLock |= 0x02;
                                    break;
                            }
                            ci.Player.StatLock = statLock;
                            return;
                    }
                }
            }

            private static void OutgoingPackets_SkillLockChangedEvent(int client, int skillID, LockStatus lockStatus)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    SkillInfo si = ci.GetSkillInfo(skillID);
                    si.LockStatus = lockStatus;
                }
            }

            private static void OutgoingPackets_UseItemRequestedEvent(int client, int serial)
            {
                ClientInfo ci;
                if (!ClientInfoCollection.GetClient(client, out ci)) return;

                if ((serial & ~0x7FFFFFFF) != 0)
                {
                    serial ^= ~0x7FFFFFFF;
                    if (ci.Player.Serial == serial)
                    {
                        TreeViewUpdater.UpdateLastObject(ci.ProcessID, (WorldItem)ci.Player);
                        return;
                    }
                }
                if (UOMath.IsMobile(serial))
                {
                    Mobile m;
                    if (ClientInfoCollection.GetMobile(client, serial, out m))
                    {
                        TreeViewUpdater.UpdateLastObject(ci.ProcessID, (WorldItem)m);
                    }
                    else
                    {
                        Mobile newMobile = new Mobile(serial, client);
                        newMobile.myName = "This mobile not found!";
                        TreeViewUpdater.UpdateLastObject(ci.ProcessID, (WorldItem)newMobile);
                    }
                }
                else
                {
                    Item i;
                    if (ClientInfoCollection.GetItem(client, serial, out i))
                    {
                        TreeViewUpdater.UpdateLastObject(ci.ProcessID, (WorldItem)i);
                    }
                    else
                    {
                        Item newItem = new Item(serial);
                        newItem.Name = "This item not found!";
                        TreeViewUpdater.UpdateLastObject(ci.ProcessID, (WorldItem)newItem);
                    }
                }
            }

            private static void OutgoingPackets_TargetSentEvent(int client, int targetType, bool checkCrime, int serial, int x, int y, int z, int ID, byte[] packet)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                {
                    ci.LastTarget = serial;
                    ci.LastTargetPacket = packet;
                    TreeViewUpdater.UpdateLastTarget(ci.ProcessID, targetType, checkCrime, serial, x, y, z, ID);
                }
            }

            private static void OutgoingPackets_MoveRequestedEvent(int client, int direction, int sequence)
            {
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(client, out ci))
                    ci.SetSequence(sequence, direction);
            }
        }
    }
}