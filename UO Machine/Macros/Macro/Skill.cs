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
        public static void Skill (int client, Skill skill)
        {
            switch (skill)
            {
                case UOMachine.Skill.Anatomy:
                    Event(client, 13, 1);
                    break;
                case UOMachine.Skill.Animal_Lore:
                    Event(client, 13, 2);
                    break;
                case UOMachine.Skill.Animal_Taming:
                    Event(client, 13, 35);
                    break;
                case UOMachine.Skill.Arms_Lore:
                    Event(client, 13, 4);
                    break;
                case UOMachine.Skill.Begging:
                    Event(client, 13, 6);
                    break;
                case UOMachine.Skill.Cartography:
                    Event(client, 13, 12);
                    break;
                case UOMachine.Skill.Detecting_Hidden:
                    Event(client, 13, 14);
                    break;
                case UOMachine.Skill.Discordance:
                    Event(client, 13, 15);
                    break;
                case UOMachine.Skill.Evaluating_Intelligence:
                    Event(client, 13, 16);
                    break;
                case UOMachine.Skill.Forensic_Evaluation:
                    Event(client, 13, 19);
                    break;
                case UOMachine.Skill.Hiding:
                    Event(client, 13, 21);
                    break;
                case UOMachine.Skill.Inscription:
                    Event(client, 13, 23);
                    break;
                case UOMachine.Skill.Item_Identification:
                    Event(client, 13, 3);
                    break;
                case UOMachine.Skill.Meditation:
                    Event(client, 13, 46);
                    break;
                case UOMachine.Skill.Peacemaking:
                    Event(client, 13, 9);
                    break;
                case UOMachine.Skill.Poisoning:
                    Event(client, 13, 30);
                    break;
                case UOMachine.Skill.Provocation:
                    Event(client, 13, 22);
                    break;
                case UOMachine.Skill.Remove_Trap:
                    Event(client, 13, 48);
                    break;
                case UOMachine.Skill.Spirit_Speak:
                    Event(client, 13, 32);
                    break;
                case UOMachine.Skill.Stealing:
                    Event(client, 13, 33);
                    break;
                case UOMachine.Skill.Stealth:
                    Event(client, 13, 47);
                    break;
                case UOMachine.Skill.Taste_Identification:
                    Event(client, 13, 36);
                    break;
                case UOMachine.Skill.Tracking:
                    Event(client, 13, 38);
                    break;
            }
        }
    }
}