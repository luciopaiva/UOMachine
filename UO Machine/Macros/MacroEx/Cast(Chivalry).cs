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
        /// Cast Chivalry spell with a spellbook.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="chivalry">Spell to cast.</param>
        public static void Cast(int client, Chivalry chivalry)
        {
            byte[] myBaseAOSCastPacket = new byte[] { 0xBF, 0x00, 0x09, 0x00, 0x1C, 0x00, 0x02, 0x00, 0x00 };

            switch (chivalry)
            {
                case Chivalry.Cleanse_By_Fire:
                    myBaseAOSCastPacket[8] = 0xC9;
                    break;
                case Chivalry.Close_Wounds:
                    myBaseAOSCastPacket[8] = 0xCA;
                    break;
                case Chivalry.Consecrate_Weapon:
                    myBaseAOSCastPacket[8] = 0xCB;
                    break;
                case Chivalry.Dispel_Evil:
                    myBaseAOSCastPacket[8] = 0xCC;
                    break;
                case Chivalry.Divine_Fury:
                    myBaseAOSCastPacket[8] = 0xCD;
                    break;
                case Chivalry.Enemy_Of_One:
                    myBaseAOSCastPacket[8] = 0xCE;
                    break;
                case Chivalry.Holy_Light:
                    myBaseAOSCastPacket[8] = 0xCF;
                    break;
                case Chivalry.Noble_Sacrifice:
                    myBaseAOSCastPacket[8] = 0xD0;
                    break;
                case Chivalry.Remove_Curse:
                    myBaseAOSCastPacket[8] = 0xD1;
                    break;
                case Chivalry.Sacred_Journey:
                    myBaseAOSCastPacket[8] = 0xD2;
                    break;
                default:
                    return;
            }
            SendPacketToServer(client, myBaseAOSCastPacket);
        }
    }
}