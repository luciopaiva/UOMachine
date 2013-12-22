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
using UOMachine;

namespace UOMachine.Macros
{
    public static partial class Macro
    {
        public static void Move(int client, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    Event(client, 5, 0x01);
                    break;
                case Direction.Northeast:
                    Event(client, 5, 0x02);
                    break;
                case Direction.East:
                    Event(client, 5, 0x03);
                    break;
                case Direction.Southeast:
                    Event(client, 5, 0x04);
                    break;
                case Direction.South:
                    Event(client, 5, 0x05);
                    break;
                case Direction.Southwest:
                    Event(client, 5, 0x06);
                    break;
                case Direction.West:
                    Event(client, 5, 0x07);
                    break;
                case Direction.Northwest:
                    Event(client, 5, 0x00);
                    break;
            }
        }
    }
}