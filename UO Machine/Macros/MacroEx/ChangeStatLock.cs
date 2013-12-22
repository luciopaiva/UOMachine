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
        public static void ChangeStatLock(int client, Stat stat, StatLockStatus statLockStatus)
        {
            byte[] packet = new byte[] { 0xBF, 0x00, 0x07, 0x00, 0x1A, 0x00, 0x00 };

            switch (stat)
            {
                case Stat.Strength:
                    packet[5] = 0x00;
                    break;
                case Stat.Dexterity:
                    packet[5] = 0x01;
                    break;
                case Stat.Intelligence:
                    packet[5] = 0x02;
                    break;
            }

            switch (statLockStatus)
            {
                case StatLockStatus.Up:
                    packet[6] = 0x00;
                    break;
                case StatLockStatus.Down:
                    packet[6] = 0x01;
                    break;
                case StatLockStatus.Locked:
                    packet[6] = 0x02;
                    break;
            }

            SendPacketToServer(client, packet);
        }

    }
}