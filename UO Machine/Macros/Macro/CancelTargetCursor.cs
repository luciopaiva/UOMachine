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
    public static partial class Macro
    {
        private static byte[] myCancelTargetPacket = new byte[] { 
            0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 
            0x00, 0x00, 0x00 };

        /// <summary>
        /// Sets cursor to normal and sends target cancel packet to server.
        /// Has the same effect as pressing Esc.
        /// </summary>
        /// <param name="client">Target client.</param>
        public static void CancelTargetCursor(int client)
        {
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                Memory.Write(ci.Handle, ci.CursorAddress, dwordZero, true);
                MacroEx.SendPacketToServer(client, myCancelTargetPacket);
            }
        }
    }
}