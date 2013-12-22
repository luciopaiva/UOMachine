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
        public static void Gump(int client, GumpAction gumpAction)
        {
            switch (gumpAction)
            {         
                case GumpAction.Open_Configuration:
                    Event(client, 8, 0);
                    break;
                case GumpAction.Open_Paperdoll:
                    Event(client, 8, 1);
                    break;
                case GumpAction.Open_Status:
                    Event(client, 8, 2);
                    break;
                case GumpAction.Open_Journal:
                    Event(client, 8, 3);
                    break;
                case GumpAction.Open_Skills:
                    Event(client, 8, 4);
                    break;
                case GumpAction.Open_Spellbook:
                    Event(client, 8, 5);
                    break;
                case GumpAction.Open_Chat:
                    Event(client, 8, 6);
                    break;
                case GumpAction.Open_Backpack:
                    Event(client, 8, 7);
                    break;
                case GumpAction.Open_Overview:
                    Event(client, 8, 8);
                    break;
                case GumpAction.Open_Mail:
                    Event(client, 8, 9);
                    break;
                case GumpAction.Open_Party_Manifest:
                    Event(client, 8, 10);
                    break;
                case GumpAction.Open_Party_Chat:
                    Event(client, 8, 11);
                    break;
                case GumpAction.Open_Necro_Spellbook:
                    Event(client, 8, 12);
                    break;
                case GumpAction.Open_Paladin_Spellbook:
                    Event(client, 8, 13);
                    break;
                case GumpAction.Open_Combat_Book:
                    Event(client, 8, 14);
                    break;
                case GumpAction.Open_Bushido_Spellbook:
                    Event(client, 8, 15);
                    break;
                case GumpAction.Open_Ninjitsu_Spellbook:
                    Event(client, 8, 16);
                    break;
                case GumpAction.Open_Guild:
                    Event(client, 8, 17);
                    break;
                case GumpAction.Open_Spellweaving_SpellBook:
                    Event(client, 8, 18);
                    break;
                case GumpAction.Open_Questlog:
                    Event(client, 8, 19);
                    break;
                case GumpAction.Close_Configuration:
                    Event(client, 9, 0);
                    break;
                case GumpAction.Close_Paperdoll:
                    Event(client, 9, 1);
                    break;
                case GumpAction.Close_Status:
                    Event(client, 9, 2);
                    break;
                case GumpAction.Close_Journal:
                    Event(client, 9, 3);
                    break;
                case GumpAction.Close_Skills:
                    Event(client, 9, 4);
                    break;
                case GumpAction.Close_Spellbook:
                    Event(client, 9, 5);
                    break;
                case GumpAction.Close_Chat:
                    Event(client, 9, 6);
                    break;
                case GumpAction.Close_Backpack:
                    Event(client, 9, 7);
                    break;
                case GumpAction.Close_Overview:
                    Event(client, 9, 8);
                    break;
                case GumpAction.Close_Mail:
                    Event(client, 9, 9);
                    break;
                case GumpAction.Close_Party_Manifest:
                    Event(client, 9, 10);
                    break;
                case GumpAction.Close_Party_Chat:
                    Event(client, 9, 11);
                    break;
                case GumpAction.Close_Necro_Spellbook:
                    Event(client, 9, 12);
                    break;
                case GumpAction.Close_Paladin_Spellbook:
                    Event(client, 9, 13);
                    break;
                case GumpAction.Close_Combat_Book:
                    Event(client, 9, 14);
                    break;
                case GumpAction.Close_Bushido_Spellbook:
                    Event(client, 9, 15);
                    break;
                case GumpAction.Close_Ninjitsu_Spellbook:
                    Event(client, 9, 16);
                    break;
                case GumpAction.Close_Guild:
                    Event(client, 9, 17);
                    break;
                case GumpAction.Minimize_Paperdoll:
                    Event(client, 10, 1);
                    break;
                case GumpAction.Minimize_Status:
                    Event(client, 10, 2);
                    break;
                case GumpAction.Minimize_Journal:
                    Event(client, 10, 3);
                    break;
                case GumpAction.Minimize_Skills:
                    Event(client, 10, 4);
                    break;
                case GumpAction.Minimize_Spellbook:
                    Event(client, 10, 5);
                    break;
                case GumpAction.Minimize_Chat:
                    Event(client, 10, 6);
                    break;
                case GumpAction.Minimize_Backpack:
                    Event(client, 10, 7);
                    break;
                case GumpAction.Minimize_Overview:
                    Event(client, 10, 8);
                    break;
                case GumpAction.Minimize_Mail:
                    Event(client, 10, 9);
                    break;
                case GumpAction.Minimize_Party_Manifest:
                    Event(client, 10, 10);
                    break;
                case GumpAction.Minimize_Party_Chat:
                    Event(client, 10, 11);
                    break;
                case GumpAction.Minimize_Necro_Spellbook:
                    Event(client, 10, 12);
                    break;
                case GumpAction.Minimize_Paladin_Spellbook:
                    Event(client, 10, 13);
                    break;
                case GumpAction.Minimize_Combat_Book:
                    Event(client, 10, 14);
                    break;
                case GumpAction.Minimize_Bushido_Spellbook:
                    Event(client, 10, 15);
                    break;
                case GumpAction.Minimize_Ninjitsu_Spellbook:
                    Event(client, 10, 16);
                    break;
                case GumpAction.Minimize_Guild:
                    Event(client, 10, 17);
                    break;
                case GumpAction.Maximize_Paperdoll:
                    Event(client, 11, 1);
                    break;
                case GumpAction.Maximize_Status:
                    Event(client, 11, 2);
                    break;
                case GumpAction.Maximize_Journal:
                    Event(client, 11, 3);
                    break;
                case GumpAction.Maximize_Skills:
                    Event(client, 11, 4);
                    break;
                case GumpAction.Maximize_Spellbook:
                    Event(client, 11, 5);
                    break;
                case GumpAction.Maximize_Chat:
                    Event(client, 11, 6);
                    break;
                case GumpAction.Maximize_Backpack:
                    Event(client, 11, 7);
                    break;
                case GumpAction.Maximize_Overview:
                    Event(client, 11, 8);
                    break;
                case GumpAction.Maximize_Mail:
                    Event(client, 11, 9);
                    break;
                case GumpAction.Maximize_Party_Manifest:
                    Event(client, 11, 10);
                    break;
                case GumpAction.Maximize_Party_Chat:
                    Event(client, 11, 11);
                    break;
                case GumpAction.Maximize_Necro_Spellbook:
                    Event(client, 11, 12);
                    break;
                case GumpAction.Maximize_Paladin_Spellbook:
                    Event(client, 11, 13);
                    break;
                case GumpAction.Maximize_Combat_Book:
                    Event(client, 11, 14);
                    break;
                case GumpAction.Maximize_Bushido_Spellbook:
                    Event(client, 11, 15);
                    break;
                case GumpAction.Maximize_Ninjitsu_Spellbook:
                    Event(client, 11, 16);
                    break;
                case GumpAction.Maximize_Guild:
                    Event(client, 11, 17);
                    break;
            }
        }
    }
}