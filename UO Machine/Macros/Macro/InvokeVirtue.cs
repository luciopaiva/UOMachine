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
    public static partial class Macro
    {
        public static void InvokeVirtue(int client, Virtues virtues)
        {
            switch (virtues)
            {
                case Virtues.Honor:
                    Event(client, 49, 0x01);
                    break;
                case Virtues.Sacrifice:
                    Event(client, 49, 0x02);
                    break;
                case Virtues.Valor:
                    Event(client, 49, 0x03);
                    break;
                case Virtues.Compassion:
                    Event(client, 49, 0x04);
                    break;
                case Virtues.Honesty:
                    Event(client, 49, 0x05);
                    break;
                case Virtues.Humility:
                    Event(client, 49, 0x06);
                    break;
                case Virtues.Justice:
                    Event(client, 49, 0x07);
                    break;
                case Virtues.Spirituality:
                    Event(client, 49, 0x08);
                    break;
            }
        }
    }
}