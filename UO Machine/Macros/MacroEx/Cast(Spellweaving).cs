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
        /// Cast Spellweaving spell with a spellbook.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="spellweaving">Spell to cast.</param>
        public static void Cast(int client, Spellweaving spellweaving)
        {
            byte[] myBaseMLCastPacket = new byte[] { 0xBF, 0x00, 0x09, 0x00, 0x1C, 0x00, 0x02, 0x02, 0x00 };

            switch (spellweaving)
            {
                case Spellweaving.Arcane_Circle:
                    myBaseMLCastPacket[8] = 0x59;
                    break;
                case Spellweaving.Gift_of_Renewal:
                    myBaseMLCastPacket[8] = 0x5A;
                    break;
                case Spellweaving.Immolating_Weapon:
                    myBaseMLCastPacket[8] = 0x5B;
                    break;
                case Spellweaving.Attunement:
                    myBaseMLCastPacket[8] = 0x5C;
                    break;
                case Spellweaving.Thunderstorm:
                    myBaseMLCastPacket[8] = 0x5D;
                    break;
                case Spellweaving.Natures_Fury:
                    myBaseMLCastPacket[8] = 0x5E;
                    break;
                case Spellweaving.Summon_Fey:
                    myBaseMLCastPacket[8] = 0x5F;
                    break;
                case Spellweaving.Summon_Fiend:
                    myBaseMLCastPacket[8] = 0x60;
                    break;
                case Spellweaving.Reaper_Form:
                    myBaseMLCastPacket[8] = 0x61;
                    break;
                case Spellweaving.Wildfire:
                    myBaseMLCastPacket[8] = 0x62;
                    break;
                case Spellweaving.Essence_of_Wind:
                    myBaseMLCastPacket[8] = 0x63;
                    break;
                case Spellweaving.Dryad_Allure:
                    myBaseMLCastPacket[8] = 0x64;
                    break;
                case Spellweaving.Ethereal_Voyage:
                    myBaseMLCastPacket[8] = 0x65;
                    break;
                case Spellweaving.Word_of_Death:
                    myBaseMLCastPacket[8] = 0x66;
                    break;
                case Spellweaving.Gift_of_Life:
                    myBaseMLCastPacket[8] = 0x67;
                    break;
                case Spellweaving.Arcane_Empowerment:
                    myBaseMLCastPacket[8] = 0x68;
                    break;
                default:
                    return;
            }
            SendPacketToServer(client, myBaseMLCastPacket);
        }
    }
}