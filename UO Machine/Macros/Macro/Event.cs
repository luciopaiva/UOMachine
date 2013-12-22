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
using System.Threading;

namespace UOMachine.Macros
{
    public static partial class Macro
    {
        internal static void Event(int client, byte param1, byte param2)
        {
            ClientInfo ci;
            if (!ClientInfoCollection.GetClient(client, out ci)) throw new ApplicationException("Unable to retrieve client info for client " + client + ".");
            Assembler.Execute(ci, param1, param2);
        }

        internal static void Event(int client, byte param1, byte param2a, byte param2b)
        {
            ClientInfo ci;
            if (!ClientInfoCollection.GetClient(client, out ci)) throw new ApplicationException("Unable to retrieve client info for client " + client + ".");
            Assembler.Execute(ci, param1, param2a, param2b);
        }

        internal static void Event(int client, byte param1, byte param2, string param3)
        {
            ClientInfo ci;
            if (!ClientInfoCollection.GetClient(client, out ci)) throw new ApplicationException("Unable to retrieve client info for client " + client + ".");
            Assembler.Execute(ci, param1, param2, param3);
        }
    }
}
