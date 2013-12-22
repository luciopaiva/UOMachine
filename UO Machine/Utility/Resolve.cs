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

namespace UOMachine.Utility
{
    public static class Networking
    {
        /// <summary>
        /// Resolve hostname to IP address.
        /// </summary>
        public static IPAddress Resolve(string hostname)
        {
            IPAddress ip = IPAddress.None;
            if (!string.IsNullOrEmpty(hostname))
            {
                if (!IPAddress.TryParse(hostname, out ip))
                {
                    try
                    {
                        IPHostEntry entry = Dns.GetHostEntry(hostname);
                        if (entry.AddressList.Length > 0)
                        {
                            ip = entry.AddressList[entry.AddressList.Length - 1];
                        }
                    }
                    catch { }
                }
            }
            return ip;
        }
    }
}