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
        public static void Cast(int client, Chivalry chivalry)
        {
            switch (chivalry)
            {
                case Chivalry.Cleanse_By_Fire:
                    Event(client, 15, 0xC9);
                    break;
                case Chivalry.Close_Wounds:
                    Event(client, 15, 0xCA);
                    break;
                case Chivalry.Consecrate_Weapon:
                    Event(client, 15, 0xCB);
                    break;
                case Chivalry.Dispel_Evil:
                    Event(client, 15, 0xCC);
                    break;
                case Chivalry.Divine_Fury:
                    Event(client, 15, 0xCD);
                    break;
                case Chivalry.Enemy_Of_One:
                    Event(client, 15, 0xCE);
                    break;
                case Chivalry.Holy_Light:
                    Event(client, 15, 0xCF);
                    break;
                case Chivalry.Noble_Sacrifice:
                    Event(client, 15, 0xD0);
                    break;
                case Chivalry.Remove_Curse:
                    Event(client, 15, 0xD1);
                    break;
                case Chivalry.Sacred_Journey:
                    Event(client, 15, 0xD2);
                    break;
            }
        }
    }
}