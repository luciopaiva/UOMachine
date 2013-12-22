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
        /// <summary>
        /// Launch a new client instance.
        /// </summary>
        /// <param name="clientIndex">Client index of launched client.</param>
        /// <returns>Client index of launched client.</returns>
        public static bool LaunchClient(out int clientIndex)
        {
            return ClientLauncher.Launch(MainWindow.CurrentOptions, out clientIndex);
        }

        /// <summary>
        /// Launch a new client instance.
        /// </summary>
        /// <param name="options">UOMachine.OptionsData to use for launching client.</param>
        /// <param name="clientIndex">Client index of launched client.</param>
        /// <returns>Client index of launched client.</returns>
        public static bool LaunchClient(OptionsData options, out int clientIndex)
        {
            return ClientLauncher.Launch(options, out clientIndex);
        }
    }
}