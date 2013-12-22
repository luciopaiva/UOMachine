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
using System.Net;
using System.Windows;

namespace UOMachine.Macros
{
    public static partial class Macro
    {
        public static void ChangeServer(int client, string hostname, int port)
        {
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                IPAddress address = Utility.Networking.Resolve(hostname);
                if (address != IPAddress.None)
                {
                    byte[] portBytes = new byte[2];
                    portBytes[0] = (byte)port;
                    portBytes[1] = (byte)(port >> 8);

                    byte[] oldAddress = address.GetAddressBytes();
                    byte[] addressBytes = new byte[4];
                    addressBytes[0] = oldAddress[3];
                    addressBytes[1] = oldAddress[2];
                    addressBytes[2] = oldAddress[1];
                    addressBytes[3] = oldAddress[0];
                    Memory.Write(ci.Handle, ci.LoginServerAddress, addressBytes, true);
                    Memory.Write(ci.Handle, ci.LoginPortAddress, portBytes, true);
                }
                else
                {
                    MessageBox.Show("Error, unable to resolve hostname '" + hostname + "'!", "Error");
                }
            }
        }
    }
}