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
    public static partial class MacroEx
    {
        /// <summary>
        /// Get packet based generic gump with specified serial from target client.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="serial">Serial of gump to get.</param>
        /// <returns>True on success.</returns>
        public static bool FindGump(int client, int serial, out Gump gump)
        {
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                return ci.GenericGumps.FindGump(serial, out gump);
            }
            gump = null;
            return false;
        }
    }
}