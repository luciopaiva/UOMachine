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
        /// Cast Bushido spell with a spellbook.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="bushido">Spell to cast.</param>
        public static void Cast(int client, Bushido bushido)
        {
            byte[] myBaseSECastPacket = new byte[] { 0xBF, 0x00, 0x09, 0x00, 0x1C, 0x00, 0x02, 0x01, 0x00 };
            switch (bushido)
            {
                case Bushido.Honorable_Execution:
                    myBaseSECastPacket[8] = 0x91;
                    break;
                case Bushido.Confidence:
                    myBaseSECastPacket[8] = 0x92;
                    break;
                case Bushido.Evasion:
                    myBaseSECastPacket[8] = 0x93;
                    break;
                case Bushido.Counter_Attack:
                    myBaseSECastPacket[8] = 0x94;
                    break;
                case Bushido.Lightning_Strike:
                    myBaseSECastPacket[8] = 0x95;
                    break;
                case Bushido.Momentum_Strike:
                    myBaseSECastPacket[8] = 0x96;
                    break;
                default:
                    return;
            }
            SendPacketToServer(client, myBaseSECastPacket);
        }

    }
}