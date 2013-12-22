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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UOMachine.Utility;

namespace UOMachine.IPC
{
    internal static class Network
    {
        private const int myMaxServerCount = 32;
        private static ServerInstance[] myServerList;
        private static object mySyncRoot = new object();

        private static int myCount;
        /// <summary>
        /// Number of active IPC servers.
        /// </summary>
        public static int Count
        {
            get { return Thread.VolatileRead(ref myCount); }
            private set { Interlocked.Exchange(ref myCount, value); }
        }

        /// <summary>
        /// Prepare IPC network for use.
        /// </summary>
        internal static void Initialize()
        {
            myServerList = new ServerInstance[myMaxServerCount];
            Count = 0;
        }

        /// <summary>
        /// Disconnect IPC servers and cleanup.
        /// </summary>
        internal static void Dispose()
        {
            if (myServerList != null)
                for (int x = 0; x < myServerList.Length; x++)
                    if (myServerList[x] != null)
                        myServerList[x].Dispose();
            Count = 0;
        }

        internal static bool GetServer(int index, out ServerInstance serverInstance)
        {
            serverInstance = ThreadHelper.VolatileRead<ServerInstance>(ref myServerList[index]);
            return serverInstance == null ? false : true;
        }

        /// <summary>
        /// Create an IPC server and start listening for connection.
        /// </summary>
        /// <param name="serverName">
        /// Name to assign to IPC server.
        /// </param>
        /// <param name="writeThrough">
        /// If true writes will bypass system cache and go straight to the pipe.
        /// </param>
        /// <returns>Index of server created.</returns>
        internal static int CreateServer(string serverName, bool writeThrough)
        {
            if (Count >= myMaxServerCount)
                throw new ApplicationException("Maximum number of IPC servers exceeded.");
            if (myServerList != null)
            {
                for (int x = 0; x < myServerList.Length; x++)
                {
                    if (myServerList[x] == null)
                    {
                        Count++;
                        ServerInstance si = new ServerInstance(serverName, writeThrough, x, myMaxServerCount);
                        Interlocked.Exchange<ServerInstance>(ref myServerList[x], si);
                        return x;
                    }
                }
            }
            throw new ApplicationException("Error creating new IPC server.");
        }

        /// <summary>
        /// Remove specified element from ServerList.
        /// </summary>
        /// <param name="index">
        /// Zero-based index of ServerInstance to remove.
        /// </param>
        internal static void RemoveServer(int index)
        {
            ServerInstance si;
            if (GetServer(index, out si))
            {
                si.Dispose();
                Interlocked.Exchange<ServerInstance>(ref myServerList[index], null);
                Count--;
            }
        }

        /// <summary>
        /// Send single-byte IPC message to IPC client.
        /// </summary>
        /// <param name="command">IPC.Command to send.</param>
        internal static void SendCommand(int index, Command command)
        {
            ServerInstance si;
            if (GetServer(index, out si))
                si.SendCommand(command);
        }

        /// <summary>
        /// Send 2 byte IPC message to IPC client.
        /// </summary>
        /// <param name="command">IPC.Command to send.</param>
        /// <param name="data">Command argument.</param>
        internal static void SendCommand(int index, Command command, byte data)
        {
            ServerInstance si;
            if (GetServer(index, out si))
                si.SendCommand(command, data);
        }

        /// <summary>
        /// Send 13 byte IPC message to IPC client.
        /// </summary>
        internal static void SendCommand(int index, Command command, int arg1, int arg2, int arg3)
        {
            ServerInstance si;
            if (GetServer(index, out si))
                si.SendCommand(command, arg1, arg2, arg3);
        }

        /// <summary>
        /// Send variable length IPC message to IPC client.
        /// </summary>
        /// <param name="command">IPC.Command to send.</param>
        /// <param name="data">Command argument.</param>
        internal static void SendCommand(int index, Command command, byte[] data)
        {
            ServerInstance si;
            if (GetServer(index, out si))
                si.SendCommand(command, data);
        }

        /// <summary>
        /// Send variable length IPC message to IPC client.
        /// </summary>
        /// <param name="command">IPC.Command to send.</param>
        /// <param name="arg1">Command argument.</param>
        /// <param name="arg2">Command argument.</param>
        /// <param name="data">Command argument.</param>
        internal static void SendCommand(int index, Command command, int arg1, byte arg2, byte[] data)
        {
            ServerInstance si;
            if (GetServer(index, out si))
                si.SendCommand(command, arg1, arg2, data);
        }

        /// <summary>
        /// Send variable length IPC message to IPC client.
        /// </summary>
        /// <param name="command">IPC.Command to send.</param>
        /// <param name="message"> Command argument.</param>
        internal static void SendCommand(int index, Command command, string message)
        {
            ServerInstance si;
            if (GetServer(index, out si))
                si.SendCommand(command, message);
        }

    }
}