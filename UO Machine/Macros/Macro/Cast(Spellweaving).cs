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
        public static void Cast(int client, Spellweaving spellweaving)
        {
            switch (spellweaving)
            {
                case Spellweaving.Arcane_Circle:
                    Event(client, 15, 0x02, 0x59);
                    break;
                case Spellweaving.Gift_of_Renewal:
                    Event(client, 15, 0x02, 0x5A);
                    break;
                case Spellweaving.Immolating_Weapon:
                    Event(client, 15, 0x02, 0x5B);
                    break;
                case Spellweaving.Attunement:
                    Event(client, 15, 0x02, 0x5C);
                    break;
                case Spellweaving.Thunderstorm:
                    Event(client, 15, 0x02, 0x5D);
                    break;
                case Spellweaving.Natures_Fury:
                    Event(client, 15, 0x02, 0x5E);
                    break;
                case Spellweaving.Summon_Fey:
                    Event(client, 15, 0x02, 0x5F);
                    break;
                case Spellweaving.Summon_Fiend:
                    Event(client, 15, 0x02, 0x60);
                    break;
                case Spellweaving.Reaper_Form:
                    Event(client, 15, 0x02, 0x61);
                    break;
                case Spellweaving.Wildfire:
                    Event(client, 15, 0x02, 0x62);
                    break;
                case Spellweaving.Essence_of_Wind:
                    Event(client, 15, 0x02, 0x63);
                    break;
                case Spellweaving.Dryad_Allure:
                    Event(client, 15, 0x02, 0x64);
                    break;
                case Spellweaving.Ethereal_Voyage:
                    Event(client, 15, 0x02, 0x65);
                    break;
                case Spellweaving.Word_of_Death:
                    Event(client, 15, 0x02, 0x66);
                    break;
                case Spellweaving.Gift_of_Life:
                    Event(client, 15, 0x02, 0x67);
                    break;
                case Spellweaving.Arcane_Empowerment:
                    Event(client, 15, 0x02, 0x68);
                    break;
            }
        }
    }
}