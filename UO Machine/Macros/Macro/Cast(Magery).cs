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
    public static partial class Macro
    {
        public static void Cast(int client, Magery magery)
        {
            switch (magery)
            {
                case Magery.Clumsy:
                    Event(client, 15, 0x01);
                    break;
                case Magery.Create_Food:
                    Event(client, 15, 0x02);
                    break;
                case Magery.Feeblemind:
                    Event(client, 15, 0x03);
                    break;
                case Magery.Heal:
                    Event(client, 15, 0x04);
                    break;
                case Magery.Magic_Arrow:
                    Event(client, 15, 0x05);
                    break;
                case Magery.Night_Sight:
                    Event(client, 15, 0x06);
                    break;
                case Magery.Reactive_Armor:
                    Event(client, 15, 0x07);
                    break;
                case Magery.Weaken:
                    Event(client, 15, 0x08);
                    break;
                case Magery.Agility:
                    Event(client, 15, 0x09);
                    break;
                case Magery.Cunning:
                    Event(client, 15, 0x0A);
                    break;
                case Magery.Cure:
                    Event(client, 15, 0x0B);
                    break;
                case Magery.Harm:
                    Event(client, 15, 0x0C);
                    break;
                case Magery.Magic_Trap:
                    Event(client, 15, 0x0D);
                    break;
                case Magery.Magic_Untrap:
                    Event(client, 15, 0x0E);
                    break;
                case Magery.Protection:
                    Event(client, 15, 0x0F);
                    break;
                case Magery.Strength:
                    Event(client, 15, 0x10);
                    break;
                case Magery.Bless:
                    Event(client, 15, 0x11);
                    break;
                case Magery.Fireball:
                    Event(client, 15, 0x12);
                    break;
                case Magery.Magic_Lock:
                    Event(client, 15, 0x13);
                    break;
                case Magery.Poison:
                    Event(client, 15, 0x14);
                    break;
                case Magery.Telekinesis:
                    Event(client, 15, 0x15);
                    break;
                case Magery.Teleport:
                    Event(client, 15, 0x16);
                    break;
                case Magery.Unlock:
                    Event(client, 15, 0x17);
                    break;
                case Magery.Wall_Of_Stone:
                    Event(client, 15, 0x18);
                    break;
                case Magery.Arch_Cure:
                    Event(client, 15, 0x19);
                    break;
                case Magery.Arch_Protection:
                    Event(client, 15, 0x1A);
                    break;
                case Magery.Curse:
                    Event(client, 15, 0x1B);
                    break;
                case Magery.Fire_Field:
                    Event(client, 15, 0x1C);
                    break;
                case Magery.Greater_Heal:
                    Event(client, 15, 0x1D);
                    break;
                case Magery.Lightning:
                    Event(client, 15, 0x1E);
                    break;
                case Magery.Mana_Drain:
                    Event(client, 15, 0x1F);
                    break;
                case Magery.Recall:
                    Event(client, 15, 0x20);
                    break;
                case Magery.Blade_Spirits:
                    Event(client, 15, 0x21);
                    break;
                case Magery.Dispel_Field:
                    Event(client, 15, 0x22);
                    break;
                case Magery.Incognito:
                    Event(client, 15, 0x23);
                    break;
                case Magery.Magic_Reflection:
                    Event(client, 15, 0x24);
                    break;
                case Magery.Mind_Blast:
                    Event(client, 15, 0x25);
                    break;
                case Magery.Paralyze:
                    Event(client, 15, 0x26);
                    break;
                case Magery.Poison_Field:
                    Event(client, 15, 0x27);
                    break;
                case Magery.Summon_Creature:
                    Event(client, 15, 0x28);
                    break;
                case Magery.Dispel:
                    Event(client, 15, 0x29);
                    break;
                case Magery.Energy_Bolt:
                    Event(client, 15, 0x2A);
                    break;
                case Magery.Explosion:
                    Event(client, 15, 0x2B);
                    break;
                case Magery.Invisibility:
                    Event(client, 15, 0x2C);
                    break;
                case Magery.Mark:
                    Event(client, 15, 0x2D);
                    break;
                case Magery.Mass_Curse:
                    Event(client, 15, 0x2E);
                    break;
                case Magery.Paralyze_Field:
                    Event(client, 15, 0x2F);
                    break;
                case Magery.Reveal:
                    Event(client, 15, 0x30);
                    break;
                case Magery.Chain_Lightning:
                    Event(client, 15, 0x31);
                    break;
                case Magery.Energy_Field:
                    Event(client, 15, 0x32);
                    break;
                case Magery.Flame_Strike:
                    Event(client, 15, 0x33);
                    break;
                case Magery.Gate_Travel:
                    Event(client, 15, 0x34);
                    break;
                case Magery.Mana_Vampire:
                    Event(client, 15, 0x35);
                    break;
                case Magery.Mass_Dispel:
                    Event(client, 15, 0x36);
                    break;
                case Magery.Meteor_Swarm:
                    Event(client, 15, 0x37);
                    break;
                case Magery.Polymorph:
                    Event(client, 15, 0x38);
                    break;
                case Magery.Earthquake:
                    Event(client, 15, 0x39);
                    break;
                case Magery.Energy_Vortex:
                    Event(client, 15, 0x3A);
                    break;
                case Magery.Resurrection:
                    Event(client, 15, 0x3B);
                    break;
                case Magery.Air_Elemental:
                    Event(client, 15, 0x3C);
                    break;
                case Magery.Summon_Daemon:
                    Event(client, 15, 0x3D);
                    break;
                case Magery.Earth_Elemental:
                    Event(client, 15, 0x3E);
                    break;
                case Magery.Fire_Elemental:
                    Event(client, 15, 0x3F);
                    break;
                case Magery.Water_Elemental:
                    Event(client, 15, 0x40);
                    break;
            }
        }
    }
}