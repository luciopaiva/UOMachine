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
using System.Text;

namespace UOMachine.Macros
{
    public static partial class Macro
    {
        public static void ChangeServer(int client, string hostname, int port)
        {
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                if (ci.NewStyleLoginPatch)
                {
                    StringBuilder loginpart = new StringBuilder();
                    loginpart.AppendFormat("{0},{1}", hostname, port);
                    IntPtr address = (IntPtr)((int)ci.AllocCodeAddress + 1024);
                    Memory.Write(ci.Handle, address, ASCIIEncoding.ASCII.GetBytes(loginpart.ToString()), true);
                    byte[] address2 = new byte[4];
                    address2[0] = (byte)((int)address);
                    address2[1] = (byte)((int)address >> 8);
                    address2[2] = (byte)((int)address >>16);
                    address2[3] = (byte)((int)address >>24);
                    
                    Memory.Write(ci.Handle, ci.LoginServerAddress, address2, true);
                    Memory.Write(ci.Handle, (IntPtr)((int)ci.LoginServerAddress+0x04), address2, true);
                }
                else
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
}