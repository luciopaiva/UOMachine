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
        /// Cast Necromancy spell with a spellbook.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="necromancy">Spell to cast.</param>
        public static void Cast(int client, Necromancy necromancy)
        {
            byte[] myBaseAOSCastPacket = new byte[] { 0xBF, 0x00, 0x09, 0x00, 0x1C, 0x00, 0x02, 0x00, 0x00 };

            switch (necromancy)
            {
                case Necromancy.Animate_Dead:
                    myBaseAOSCastPacket[8] = 0x65;
                    break;
                case Necromancy.Blood_Oath:
                    myBaseAOSCastPacket[8] = 0x66;
                    break;
                case Necromancy.Corpse_Skin:
                    myBaseAOSCastPacket[8] = 0x67;
                    break;
                case Necromancy.Curse_Weapon:
                    myBaseAOSCastPacket[8] = 0x68;
                    break;
                case Necromancy.Evil_Omen:
                    myBaseAOSCastPacket[8] = 0x69;
                    break;
                case Necromancy.Horrific_Beast:
                    myBaseAOSCastPacket[8] = 0x6A;
                    break;
                case Necromancy.Lich_Form:
                    myBaseAOSCastPacket[8] = 0x6B;
                    break;
                case Necromancy.Mind_Rot:
                    myBaseAOSCastPacket[8] = 0x6C;
                    break;
                case Necromancy.Pain_Spike:
                    myBaseAOSCastPacket[8] = 0x6D;
                    break;
                case Necromancy.Poison_Strike:
                    myBaseAOSCastPacket[8] = 0x6E;
                    break;
                case Necromancy.Strangle:
                    myBaseAOSCastPacket[8] = 0x6F;
                    break;
                case Necromancy.Summon_Familiar:
                    myBaseAOSCastPacket[8] = 0x70;
                    break;
                case Necromancy.Vampiric_Embrace:
                    myBaseAOSCastPacket[8] = 0x71;
                    break;
                case Necromancy.Vengeful_Spirit:
                    myBaseAOSCastPacket[8] = 0x72;
                    break;
                case Necromancy.Wither:
                    myBaseAOSCastPacket[8] = 0x73;
                    break;
                case Necromancy.Wraith_Form:
                    myBaseAOSCastPacket[8] = 0x74;
                    break;
                case Necromancy.Exorcism:
                    myBaseAOSCastPacket[8] = 0x75;
                    break;
                default:
                    return;
            }
            SendPacketToServer(client, myBaseAOSCastPacket);
        }
    }
}