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
        public static void SetRange(int client, Range range)
        {
            switch (range)
            {
                case Range.Set_Update_Range:
                    System.Windows.Forms.MessageBox.Show("Range.Set_Update_Range requires an additional argument.", "Error");
                    break;
                case Range.Modify_Update_Range:
                    System.Windows.Forms.MessageBox.Show("Range.Modify_Update_Range requires an additional argument.", "Error");
                    break;
                case Range.Increase_Update_Range:
                    Event(client, 40, 0);
                    break;
                case Range.Decrease_Update_Range:
                    Event(client, 41, 0);
                    break;
                case Range.Maximum_Update_Range:
                    Event(client, 42, 0);
                    break;
                case Range.Minimum_Update_Range:
                    Event(client, 43, 0);
                    break;
                case Range.Default_Update_Range:
                    Event(client, 44, 0);
                    break;
                case Range.Update_Update_Range:
                    Event(client, 45, 0);
                    break;
                case Range.Enable_Update_Range_Color:
                    Event(client, 46, 0);
                    break;
                case Range.Disable_Update_Range_Color:
                    Event(client, 47, 0);
                    break;
                case Range.Toggle_Update_Range_Color:
                    Event(client, 48, 0);
                    break;
            }
        }

        public static void SetRange(int client, Range range, string distance)
        {
            switch (range)
            {
                case Range.Set_Update_Range:
                    Event(client, 38, 0, distance);
                    break;
                case Range.Modify_Update_Range:
                    Event(client, 39, 0, distance);
                    break;
            }
        }


    }
}