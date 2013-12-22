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
using UOMachine.Events;
using UOMachine.Data;

namespace UOMachine.Macros
{
    public static partial class MacroEx
    {
        private static int[] mySelectedServers = new int[32];
        private static int[] mySelectedChars = new int[32];
        private static object[] myWaitObjects = new object[32];
        private static object[] myLockObjects = new object[32];
        private static bool[] mySuccess = new bool[32];
        private static bool[] myWaiting = new bool[32];
        private const int defaultTimeout = 15000;

        private static byte[] GetStringBytes(string input, int maxLen)
        {
            int len = Math.Min(input.Length, maxLen);
            byte[] output = new byte[len + 1]; //null terminated
            for (int x = 0; x < len; x++)
            {
                output[x] = (byte)input[x];
            }
            return output;
        }

        private static void Wait(object waitObject, int timeout)
        {
            lock (waitObject)
            {
                Monitor.Wait(waitObject, timeout);
            }
        }


        /// <summary>
        /// Log in to UO server using specified account name and password.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="account">Account name to use.</param>
        /// <param name="password">Password for specified account.</param>
        /// <param name="serverIndex">Zero based index of shard to select.</param>
        /// <param name="charIndex">Zero based index of character to select.</param>
        /// <returns>True on success.</returns>
        public static bool Login(int client, string account, string password, int serverIndex, int charIndex)
        {
            return Login(client, account, password, serverIndex, charIndex, defaultTimeout);
        }

        /// <summary>
        /// Log in to UO server using specified account name and password.
        /// </summary>
        /// <param name="client">Target client.</param>
        /// <param name="account">Account name to use.</param>
        /// <param name="password">Password for specified account.</param>
        /// <param name="serverIndex">Zero based index of shard to select.</param>
        /// <param name="charIndex">Zero based index of character to select.</param>
        /// <param name="timeout">Timeout in milliseconds to wait for successful login.</param>
        /// <returns>True on success.</returns>
        public static bool Login(int client, string account, string password, int serverIndex, int charIndex, int timeout)
        {
            ClientInfo ci;
            if (ClientInfoCollection.GetClient(client, out ci))
            {
                mySuccess[client] = false;
                byte[] passwordBytes = GetStringBytes(password, 16);
                byte[] loginBytes = GetStringBytes(account, 16);
                GumpInfo[] gi = Macro.GetGumpList(client);
                foreach (GumpInfo g in gi)
                {
                    if (g.Type == "MainMenu gump")
                    {
                        foreach (GumpInfo g2 in g.SubGumps)
                        {
                            if (g2.Type == "AcctLogin gump")
                            {
                                lock (myLockObjects[client])
                                {
                                    byte[] data = new byte[4];
                                    IntPtr loginPtr = (IntPtr)(g2.GumpHandle.ToInt32() + 0xC0);
                                    IntPtr passPtr = (IntPtr)(g2.GumpHandle.ToInt32() + 0xC4);
                                    Memory.Read(ci.Handle, loginPtr, data, true);
                                    loginPtr = (IntPtr)(BitConverter.ToUInt32(data, 0) + 0x118);
                                    Memory.Read(ci.Handle, passPtr, data, true);
                                    passPtr = (IntPtr)(BitConverter.ToUInt32(data, 0) + 0x118);
                                    Memory.Write(ci.Handle, loginPtr, loginBytes, true);
                                    Memory.Write(ci.Handle, passPtr, passwordBytes, true);
                                    mySelectedServers[client] = serverIndex;
                                    mySelectedChars[client] = charIndex;
                                    IncomingPackets.InternalServerListEvent += new IncomingPackets.dServerList(IncomingPackets_InternalServerListEvent);
                                    myWaiting[client] = true;
                                    if (ci.DateStamp > 0x43A06A35) g2.CallFunction(26);
                                    else g2.CallFunction(23);
                                    ThreadStart ts = new ThreadStart(delegate { Wait(myWaitObjects[client], timeout); });
                                    Thread t = new Thread(ts);
                                    t.Start();
                                    t.Join();
                                    return mySuccess[client];
                                }
                            }
                        }
                    }
                }

            }
            return false;
        }

        private static void IncomingPackets_InternalServerListEvent(int client, ServerInfo[] serverList)
        {
            if (myWaiting[client])
            {
                IncomingPackets.InternalServerListEvent -= new IncomingPackets.dServerList(IncomingPackets_InternalServerListEvent);
                for (int x = 0; x < 100; x++)
                {
                    foreach (GumpInfo g in Macro.GetGumpList(client))
                    {
                        if (g.Type == "normal gump" && g.SubGumps.Length > 0)
                        {
                            IntPtr selectServer = (IntPtr)(g.GumpHandle.ToInt32() + 0x114);
                            ClientInfo ci;
                            if (ClientInfoCollection.GetClient(client, out ci))
                            {
                                Memory.Write(ci.Handle, selectServer, BitConverter.GetBytes(mySelectedServers[client]), true);
                                IncomingPackets.InternalCharacterListEvent += new IncomingPackets.dCharacterList(IncomingPackets_InternalCharacterListEvent);
                                if (ci.DateStamp > 0x43A06A35) g.CallFunction(26);
                                else g.CallFunction(23);
                            }
                            else return;
                        }
                    }
                    Thread.Sleep(100);
                }
            }
        }

        private static void IncomingPackets_InternalCharacterListEvent(int client, string[] characterList)
        {
            if (myWaiting[client])
            {
                IncomingPackets.InternalCharacterListEvent -= new IncomingPackets.dCharacterList(IncomingPackets_InternalCharacterListEvent);
                for (int x = 0; x < 100; x++)
                {
                    foreach (GumpInfo g in Macro.GetGumpList(client))
                    {
                        if (g.Type == "Login gump")
                        {
                            IntPtr selectChar = (IntPtr)(g.GumpHandle.ToInt32() + 0xBC);
                            ClientInfo ci;
                            if (ClientInfoCollection.GetClient(client, out ci))
                            {
                                Memory.Write(ci.Handle, selectChar, BitConverter.GetBytes(mySelectedChars[client]), true);
                                if (ci.DateStamp > 0x43A06A35) g.CallFunction(26);
                                else g.CallFunction(23);
                                lock (myWaitObjects[client])
                                {
                                    Monitor.Pulse(myWaitObjects[client]);
                                }
                                mySuccess[client] = true;
                                myWaiting[client] = false;
                                return;
                            }

                        }
                    }
                    Thread.Sleep(100);
                }
            }
        }
    }
}