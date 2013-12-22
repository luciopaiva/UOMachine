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
        /// Cast Ninjitsu spell with a spellbook.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="ninjitsu">Spell to cast.</param>
        public static void Cast(int client, Ninjitsu ninjitsu)
        {
            byte[] myBaseSECastPacket = new byte[] { 0xBF, 0x00, 0x09, 0x00, 0x1C, 0x00, 0x02, 0x01, 0x00 };

            switch (ninjitsu)
            {
                case Ninjitsu.Focus_Attack:
                    myBaseSECastPacket[8] = 0xF5;
                    break;
                case Ninjitsu.Death_Strike:
                    myBaseSECastPacket[8] = 0xF6;
                    break;
                case Ninjitsu.Animal_Form:
                    myBaseSECastPacket[8] = 0xF7;
                    break;
                case Ninjitsu.Ki_Attack:
                    myBaseSECastPacket[8] = 0xF8;
                    break;
                case Ninjitsu.Surprise_Attack:
                    myBaseSECastPacket[8] = 0xF9;
                    break;
                case Ninjitsu.Backstab:
                    myBaseSECastPacket[8] = 0xFA;
                    break;
                case Ninjitsu.Shadowjump:
                    myBaseSECastPacket[8] = 0xFB;
                    break;
                case Ninjitsu.Mirror_Image:
                    myBaseSECastPacket[8] = 0xFC;
                    break;
                default:
                    return;
            }
            SendPacketToServer(client, myBaseSECastPacket);
        }
    }
}