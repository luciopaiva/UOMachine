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
        public static void Action(int client, MacroAction action)
        {
            switch (action)
            {
                case MacroAction.Last_Spell:
                    Event(client, 16, 0);
                    break;
                case MacroAction.Last_Object:
                    Event(client, 17, 0);
                    break;
                case MacroAction.Bow:
                    Event(client, 18, 0);
                    break;
                case MacroAction.Salute:
                    Event(client, 19, 0);
                    break;
                case MacroAction.Quit_Game:
                    Event(client, 20, 0);
                    break;
                case MacroAction.All_Names:
                    Event(client, 21, 0);
                    break;
                case MacroAction.Last_Target:
                    Event(client, 22, 0);
                    break;
                case MacroAction.Target_Self:
                    Event(client, 23, 0);
                    break;
                case MacroAction.Arm_Disarm_Left:
                    Event(client, 24, 1);
                    break;
                case MacroAction.Arm_Disarm_Right:
                    Event(client, 24, 2);
                    break;
                case MacroAction.Wait_For_Target:
                    Event(client, 25, 0);
                    break;
                case MacroAction.Target_Next:
                    Event(client, 26, 0);
                    break;
                case MacroAction.Attack_Last:
                    Event(client, 27, 0);
                    break;
                case MacroAction.Circletrans:
                    Event(client, 29, 0);
                    break;
                case MacroAction.Close_Gumps:
                    Event(client, 31, 0);
                    break;
                case MacroAction.Always_Run:
                    Event(client, 32, 0);
                    break;
                case MacroAction.Save_Desktop:
                    Event(client, 33, 0);
                    break;
                case MacroAction.Kill_Gump_Open:
                    Event(client, 34, 0);
                    break;
                case MacroAction.Primary_Ability:
                    Event(client, 35, 0);
                    break;
                case MacroAction.Secondary_Ability:
                    Event(client, 36, 0);
                    break;
                case MacroAction.Equip_Last_Weapon:
                    Event(client, 37, 0);
                    break;
                case MacroAction.Open_Door:
                    Event(client, 12, 0);
                    break;
                case MacroAction.Toggle_War_Peace:
                    Event(client, 6, 0);
                    break;
                case MacroAction.Paste:
                    Event(client, 7, 0);
                    break;
                case MacroAction.Last_Skill:
                    Event(client, 14, 0);
                    break;
            }
        }

        public static void Action(int client, MacroAction action, string DelayNum)
        {
            switch (action)
            {
                case MacroAction.Delay:
                    Event(client, 28, 0, DelayNum);
                    break;
                default:
                    //TODO throw exception
                    break;
            }
        }

    }
}