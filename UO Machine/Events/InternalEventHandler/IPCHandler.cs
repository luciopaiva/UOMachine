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
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using UOMachine.IPC;
using UOMachine.Utility;
using UOMachine.Events;
using UOMachine.Data;

namespace UOMachine.Events
{
    internal static partial class InternalEventHandler
    {
        public static class IPCHandler
        {
            private static int[] myIPCClientList, myClientList;

            internal static void Initialize()
            {

                myIPCClientList = new int[32];
                myClientList = new int[32];
                ServerInstance.ClientIDEvent += new dClientID(ServerInstance_ClientIDEvent);
                ServerInstance.ClientVersionEvent += new dClientVersion(ServerInstance_ClientVersionEvent);
                ServerInstance.ExceptionEvent += new dException(ServerInstance_ExceptionEvent);
                ServerInstance.IncomingPacketEvent += new dIncomingPacket(ServerInstance_IncomingPacketEvent);
                ServerInstance.MessageEvent += new dMessage(ServerInstance_MessageEvent);
                ServerInstance.OutgoingPacketEvent += new dOutgoingPacket(ServerInstance_OutgoingPacketEvent);
                ServerInstance.PingEvent += new dPing(ServerInstance_PingEvent);
                ServerInstance.FunctionPointerEvent += new dFunctionPointer(ServerInstance_FunctionPointerEvent);
                ServerInstance.KeyDownEvent += new dKeyDown(ServerInstance_KeyDownEvent);
                ServerInstance.KeyUpEvent += new dKeyUp(ServerInstance_KeyUpEvent);
                ServerInstance.MouseClickEvent += new dMouseClick(ServerInstance_MouseClickEvent);
                ServerInstance.MouseDblClickEvent += new dMouseDblClick(ServerInstance_MouseDblClickEvent);
                ServerInstance.MouseDownEvent += new dMouseDown(ServerInstance_MouseDownEvent);
                ServerInstance.MouseMoveEvent += new dMouseMove(ServerInstance_MouseMoveEvent);
                ServerInstance.MouseUpEvent += new dMouseUp(ServerInstance_MouseUpEvent);
                ServerInstance.MouseWheelEvent += new dMouseWheel(ServerInstance_MouseWheelEvent);
            }

            internal static void Dispose()
            {
                ServerInstance.ClientIDEvent -= new dClientID(ServerInstance_ClientIDEvent);
                ServerInstance.ClientVersionEvent -= new dClientVersion(ServerInstance_ClientVersionEvent);
                ServerInstance.ExceptionEvent -= new dException(ServerInstance_ExceptionEvent);
                ServerInstance.IncomingPacketEvent -= new dIncomingPacket(ServerInstance_IncomingPacketEvent);
                ServerInstance.MessageEvent -= new dMessage(ServerInstance_MessageEvent);
                ServerInstance.OutgoingPacketEvent -= new dOutgoingPacket(ServerInstance_OutgoingPacketEvent);
                ServerInstance.PingEvent -= new dPing(ServerInstance_PingEvent);
                ServerInstance.FunctionPointerEvent -= new dFunctionPointer(ServerInstance_FunctionPointerEvent);
                ServerInstance.KeyDownEvent -= new dKeyDown(ServerInstance_KeyDownEvent);
                ServerInstance.KeyUpEvent -= new dKeyUp(ServerInstance_KeyUpEvent);
                ServerInstance.MouseClickEvent -= new dMouseClick(ServerInstance_MouseClickEvent);
                ServerInstance.MouseDblClickEvent -= new dMouseDblClick(ServerInstance_MouseDblClickEvent);
                ServerInstance.MouseDownEvent -= new dMouseDown(ServerInstance_MouseDownEvent);
                ServerInstance.MouseMoveEvent -= new dMouseMove(ServerInstance_MouseMoveEvent);
                ServerInstance.MouseUpEvent -= new dMouseUp(ServerInstance_MouseUpEvent);
                ServerInstance.MouseWheelEvent -= new dMouseWheel(ServerInstance_MouseWheelEvent);
                IPC.Network.Dispose();
            }

            /// <summary>
            /// Get the IPC client associated with the specified UO client.
            /// </summary>
            public static int GetIPCClient(int clientIndex)
            {
                return Thread.VolatileRead(ref myClientList[clientIndex]);
            }

            /// <summary>
            /// Get the UO client associated with the specified IPC client.
            /// </summary>
            public static int GetClient(int IPCClient)
            {
                return Thread.VolatileRead(ref myIPCClientList[IPCClient]);
            }

            private static void ServerInstance_MouseWheelEvent(int clientID, int x, int y, sbyte delta)
            {
                int clientIndex = GetClient(clientID);
                LowLevel.OnMouseWheel(clientIndex, x, y, delta);
            }

            private static void ServerInstance_MouseUpEvent(int clientID, int x, int y, System.Windows.Forms.MouseButtons button)
            {
                int clientIndex = GetClient(clientID);
                LowLevel.OnMouseUp(clientIndex, x, y, button);
            }

            private static void ServerInstance_MouseMoveEvent(int clientID, int x, int y)
            {
                int clientIndex = GetClient(clientID);
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(clientIndex, out ci))
                {
                    ci.OnMouseMove(x, y);
                }
                LowLevel.OnMouseMove(clientIndex, x, y);
            }

            private static void ServerInstance_MouseDownEvent(int clientID, int x, int y, System.Windows.Forms.MouseButtons button)
            {
                int clientIndex = GetClient(clientID);
                LowLevel.OnMouseDown(clientIndex, x, y, button);
            }

            private static void ServerInstance_MouseDblClickEvent(int clientID, int x, int y, System.Windows.Forms.MouseButtons button)
            {
                int clientIndex = GetClient(clientID);
                LowLevel.OnMouseDblClick(clientIndex, x, y, button);
            }

            private static void ServerInstance_MouseClickEvent(int clientID, int x, int y, System.Windows.Forms.MouseButtons button)
            {
                int clientIndex = GetClient(clientID);
                LowLevel.OnMouseClick(clientIndex, x, y, button);
            }

            private static void ServerInstance_KeyUpEvent(int clientID, System.Windows.Forms.Keys key)
            {
                int clientIndex = GetClient(clientID);
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(clientIndex, out ci))
                {
                    ci.HotKeyList.KeyUp(key);
                }
                LowLevel.OnKeyUp(clientIndex, key);
            }

            private static void ServerInstance_KeyDownEvent(int clientID, System.Windows.Forms.Keys key)
            {
                int x = (int)key;
                string s = key.ToString();
                int clientIndex = GetClient(clientID);
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(clientIndex, out ci))
                {
                    ci.HotKeyList.KeyDown(key);
                    ci.OnKeyDown(key);
                }
                LowLevel.OnKeyDown(clientIndex, key);
            }

            private static void ServerInstance_FunctionPointerEvent(int clientID, uint address, FunctionType functionType)
            {
                int clientIndex = GetClient(clientID);
                ClientInfo ci;
                if (ClientInfoCollection.GetClient(clientIndex, out ci))
                {
                    switch (functionType)
                    {
                        case FunctionType.Receive:
                            ci.RecvFunctionPointer = (IntPtr)address;
                            ClientHook.InstallRecvHook(ci);
                            //Log.LogMessage(clientID, "Recv function pointer = 0x" + address.ToString("X"));
                            break;
                        case FunctionType.Send:
                            ci.SendFunctionPointer = (IntPtr)address;
                            ClientHook.InstallSendHook(ci);
                            //Log.LogMessage(clientID, "Send function pointer = 0x" + address.ToString("X"));
                            break;
                    }
                }
            }

            private static void ServerInstance_PingEvent(int clientID)
            {
                Network.SendCommand(clientID, Command.PingResponse);
            }

            private static void ServerInstance_OutgoingPacketEvent(int clientID, byte[] data)
            {
                int clientIndex = GetClient(clientID);
                OutgoingPacketParser.ProcessPacket(clientIndex, data);
                LowLevel.OnOutgoingPacket(clientIndex, data);
            }

            private static void ServerInstance_MessageEvent(int clientID, string message)
            {
                Log.LogMessage(clientID, message);
            }

            private static void ServerInstance_IncomingPacketEvent(int clientID, byte[] data)
            {
                int clientIndex = GetClient(clientID);
                IncomingPacketParser.ProcessPacket(clientIndex, data);
                LowLevel.OnIncomingPacket(clientIndex, data);
            }

            private static void ServerInstance_ExceptionEvent(int clientID, string message)
            {
                Log.LogMessage(clientID, message);
            }

            private static void ServerInstance_ClientIDEvent(int clientID, int PID)
            {
                int clientIndex;
                if (ClientInfoCollection.GetIPCOwner(clientID, out clientIndex))
                {
                    Interlocked.Exchange(ref myIPCClientList[clientID], clientIndex);
                    Interlocked.Exchange(ref myClientList[clientIndex], clientID);
                    Log.LogMessage(clientID, "Process ID = " + PID);
                }
            }

            private static void ServerInstance_ClientVersionEvent(int clientID, int version)
            {
                Log.LogMessage(clientID, "Datestamp = 0x" + version.ToString("X"));
            }
        }
    }
}