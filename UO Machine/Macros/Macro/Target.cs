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
        public static void Target(int client, Targeting targeting)
        {
            switch (targeting)
            {
                case Targeting.Select_Next_Hostile:
                    Event(client, 50, 1);
                    break;
                case Targeting.Select_Next_Party_Member:
                    Event(client, 50, 2);
                    break;
                case Targeting.Select_Next_Follower:
                    Event(client, 50, 3);
                    break;
                case Targeting.Select_Next_Object:
                    Event(client, 50, 4);
                    break;
                case Targeting.Select_Next_Mobile:
                    Event(client, 50, 5);
                    break;
                case Targeting.Select_Previous_Hostile:
                    Event(client, 51, 1);
                    break;
                case Targeting.Select_Previous_Party_Member:
                    Event(client, 51, 2);
                    break;
                case Targeting.Select_Previous_Follower:
                    Event(client, 51, 3);
                    break;
                case Targeting.Select_Previous_Object:
                    Event(client, 51, 4);
                    break;
                case Targeting.Select_Previous_Mobile:
                    Event(client, 51, 5);
                    break;
                case Targeting.Select_Nearest_Hostile:
                    Event(client, 52, 1);
                    break;
                case Targeting.Select_Nearest_Party_Member:
                    Event(client, 52, 2);
                    break;
                case Targeting.Select_Nearest_Follower:
                    Event(client, 52, 3);
                    break;
                case Targeting.Select_Nearest_Object:
                    Event(client, 52, 4);
                    break;
                case Targeting.Select_Nearest_Mobile:
                    Event(client, 52, 5);
                    break;
                case Targeting.Attack_Selected:
                    Event(client, 53, 0);
                    break;
                case Targeting.Use_Selected:
                    Event(client, 54, 0);
                    break;
                case Targeting.Current_Target:
                    Event(client, 55, 0);
                    break;
                case Targeting.Toggle_Targeting_System:
                    Event(client, 56, 0);
                    break;
                case Targeting.Toggle_Buff_Window:
                    Event(client, 57, 0);
                    break;
                case Targeting.Bandage_Self:
                    Event(client, 58, 0);
                    break;
                case Targeting.Bandage_Target:
                    Event(client, 59, 0);
                    break;
            }
        }
    }
}