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

namespace UOMachine.Macros
{
    public static partial class MacroEx
    {
        /// <summary>
        /// Cast Magery spell with a spellbook.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="magery">Spell to cast.</param>
        public static void Cast(int client, Magery magery)
        {
            byte[] myBaseAOSCastPacket = new byte[] { 0xBF, 0x00, 0x09, 0x00, 0x1C, 0x00, 0x02, 0x00, 0x00 };

            switch (magery)
            {
                case Magery.Clumsy:
                    myBaseAOSCastPacket[8] = 0x01;
                    break;
                case Magery.Create_Food:
                    myBaseAOSCastPacket[8] = 0x02;
                    break;
                case Magery.Feeblemind:
                    myBaseAOSCastPacket[8] = 0x03;
                    break;
                case Magery.Heal:
                    myBaseAOSCastPacket[8] = 0x04;
                    break;
                case Magery.Magic_Arrow:
                    myBaseAOSCastPacket[8] = 0x05;
                    break;
                case Magery.Night_Sight:
                    myBaseAOSCastPacket[8] = 0x06;
                    break;
                case Magery.Reactive_Armor:
                    myBaseAOSCastPacket[8] = 0x07;
                    break;
                case Magery.Weaken:
                    myBaseAOSCastPacket[8] = 0x08;
                    break;
                case Magery.Agility:
                    myBaseAOSCastPacket[8] = 0x09;
                    break;
                case Magery.Cunning:
                    myBaseAOSCastPacket[8] = 0x0A;
                    break;
                case Magery.Cure:
                    myBaseAOSCastPacket[8] = 0x0B;
                    break;
                case Magery.Harm:
                    myBaseAOSCastPacket[8] = 0x0C;
                    break;
                case Magery.Magic_Trap:
                    myBaseAOSCastPacket[8] = 0x0D;
                    break;
                case Magery.Magic_Untrap:
                    myBaseAOSCastPacket[8] = 0x0E;
                    break;
                case Magery.Protection:
                    myBaseAOSCastPacket[8] = 0x0F;
                    break;
                case Magery.Strength:
                    myBaseAOSCastPacket[8] = 0x10;
                    break;
                case Magery.Bless:
                    myBaseAOSCastPacket[8] = 0x11;
                    break;
                case Magery.Fireball:
                    myBaseAOSCastPacket[8] = 0x12;
                    break;
                case Magery.Magic_Lock:
                    myBaseAOSCastPacket[8] = 0x13;
                    break;
                case Magery.Poison:
                    myBaseAOSCastPacket[8] = 0x14;
                    break;
                case Magery.Telekinesis:
                    myBaseAOSCastPacket[8] = 0x15;
                    break;
                case Magery.Teleport:
                    myBaseAOSCastPacket[8] = 0x16;
                    break;
                case Magery.Unlock:
                    myBaseAOSCastPacket[8] = 0x17;
                    break;
                case Magery.Wall_Of_Stone:
                    myBaseAOSCastPacket[8] = 0x18;
                    break;
                case Magery.Arch_Cure:
                    myBaseAOSCastPacket[8] = 0x19;
                    break;
                case Magery.Arch_Protection:
                    myBaseAOSCastPacket[8] = 0x1A;
                    break;
                case Magery.Curse:
                    myBaseAOSCastPacket[8] = 0x1B;
                    break;
                case Magery.Fire_Field:
                    myBaseAOSCastPacket[8] = 0x1C;
                    break;
                case Magery.Greater_Heal:
                    myBaseAOSCastPacket[8] = 0x1D;
                    break;
                case Magery.Lightning:
                    myBaseAOSCastPacket[8] = 0x1E;
                    break;
                case Magery.Mana_Drain:
                    myBaseAOSCastPacket[8] = 0x1F;
                    break;
                case Magery.Recall:
                    myBaseAOSCastPacket[8] = 0x20;
                    break;
                case Magery.Blade_Spirits:
                    myBaseAOSCastPacket[8] = 0x21;
                    break;
                case Magery.Dispel_Field:
                    myBaseAOSCastPacket[8] = 0x22;
                    break;
                case Magery.Incognito:
                    myBaseAOSCastPacket[8] = 0x23;
                    break;
                case Magery.Magic_Reflection:
                    myBaseAOSCastPacket[8] = 0x24;
                    break;
                case Magery.Mind_Blast:
                    myBaseAOSCastPacket[8] = 0x25;
                    break;
                case Magery.Paralyze:
                    myBaseAOSCastPacket[8] = 0x26;
                    break;
                case Magery.Poison_Field:
                    myBaseAOSCastPacket[8] = 0x27;
                    break;
                case Magery.Summon_Creature:
                    myBaseAOSCastPacket[8] = 0x28;
                    break;
                case Magery.Dispel:
                    myBaseAOSCastPacket[8] = 0x29;
                    break;
                case Magery.Energy_Bolt:
                    myBaseAOSCastPacket[8] = 0x2A;
                    break;
                case Magery.Explosion:
                    myBaseAOSCastPacket[8] = 0x2B;
                    break;
                case Magery.Invisibility:
                    myBaseAOSCastPacket[8] = 0x2C;
                    break;
                case Magery.Mark:
                    myBaseAOSCastPacket[8] = 0x2D;
                    break;
                case Magery.Mass_Curse:
                    myBaseAOSCastPacket[8] = 0x2E;
                    break;
                case Magery.Paralyze_Field:
                    myBaseAOSCastPacket[8] = 0x2F;
                    break;
                case Magery.Reveal:
                    myBaseAOSCastPacket[8] = 0x30;
                    break;
                case Magery.Chain_Lightning:
                    myBaseAOSCastPacket[8] = 0x31;
                    break;
                case Magery.Energy_Field:
                    myBaseAOSCastPacket[8] = 0x32;
                    break;
                case Magery.Flame_Strike:
                    myBaseAOSCastPacket[8] = 0x33;
                    break;
                case Magery.Gate_Travel:
                    myBaseAOSCastPacket[8] = 0x34;
                    break;
                case Magery.Mana_Vampire:
                    myBaseAOSCastPacket[8] = 0x35;
                    break;
                case Magery.Mass_Dispel:
                    myBaseAOSCastPacket[8] = 0x36;
                    break;
                case Magery.Meteor_Swarm:
                    myBaseAOSCastPacket[8] = 0x37;
                    break;
                case Magery.Polymorph:
                    myBaseAOSCastPacket[8] = 0x38;
                    break;
                case Magery.Earthquake:
                    myBaseAOSCastPacket[8] = 0x39;
                    break;
                case Magery.Energy_Vortex:
                    myBaseAOSCastPacket[8] = 0x3A;
                    break;
                case Magery.Resurrection:
                    myBaseAOSCastPacket[8] = 0x3B;
                    break;
                case Magery.Air_Elemental:
                    myBaseAOSCastPacket[8] = 0x3C;
                    break;
                case Magery.Summon_Daemon:
                    myBaseAOSCastPacket[8] = 0x3D;
                    break;
                case Magery.Earth_Elemental:
                    myBaseAOSCastPacket[8] = 0x3E;
                    break;
                case Magery.Fire_Elemental:
                    myBaseAOSCastPacket[8] = 0x3F;
                    break;
                case Magery.Water_Elemental:
                    myBaseAOSCastPacket[8] = 0x40;
                    break;
                default:
                    return;
            }
            SendPacketToServer(client, myBaseAOSCastPacket);
        }

    }
}