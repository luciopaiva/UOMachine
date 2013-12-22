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
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;
using UOMachine.Utility;

namespace UOMachine.IPC
{
    public sealed class ServerInstance
    {
        public static event dPing PingEvent;
        public static event dPingResponse PingResponseEvent;
        public static event dException ExceptionEvent;
        public static event dMessage MessageEvent;
        public static event dFunctionPointer FunctionPointerEvent;
        public static event dClientVersion ClientVersionEvent;
        public static event dClientID ClientIDEvent;
        public static event dIncomingPacket IncomingPacketEvent;
        public static event dOutgoingPacket OutgoingPacketEvent;
        public static event dKeyDown KeyDownEvent;
        public static event dKeyUp KeyUpEvent;
        public static event dMouseMove MouseMoveEvent;
        public static event dMouseWheel MouseWheelEvent;
        public static event dMouseDblClick MouseDblClickEvent;
        public static event dMouseClick MouseClickEvent;
        public static event dMouseDown MouseDownEvent;
        public static event dMouseUp MouseUpEvent;

        private NamedPipeServerStream myNamedPipeServerStream;
        private const int buffSize = 131072;
        private byte[] myBuffer;
        private MemoryStream myMemoryStream;
        private Thread myThread;
        private long myReadPos;
        private int myInstance;

        private bool myThreadRunning;
        private bool ThreadRunning
        {
            get { return ThreadHelper.VolatileRead(ref myThreadRunning); }
            set { myThreadRunning = value; }
        }

        public ServerInstance(string serverName, bool writeThrough, int instance, int maxServerCount)
        {
            myMemoryStream = new MemoryStream(buffSize);
            myReadPos = 0;
            myThreadRunning = true;
            myInstance = instance;
            myBuffer = new byte[buffSize];
            myThread = new Thread(new ThreadStart(ProcessServerStream));
            myThread.Start();
            myNamedPipeServerStream = new NamedPipeServerStream(
                serverName,
                PipeDirection.InOut,
                maxServerCount,
                PipeTransmissionMode.Byte,
                writeThrough ? PipeOptions.Asynchronous | PipeOptions.WriteThrough : PipeOptions.Asynchronous);
            myNamedPipeServerStream.BeginWaitForConnection(new AsyncCallback(OnConnect), null);
        }

        /// <summary>
        /// Stop helper thread and cleanup.
        /// </summary>
        public void Dispose()
        {
            myThreadRunning = false;
            lock (myMemoryStream) { Monitor.Pulse(myMemoryStream); }

            if (myThread.IsAlive)
            {
                myThread.Abort();
                myThread.Join();
            }

            if (myNamedPipeServerStream != null)
            {
                if (myNamedPipeServerStream.IsConnected)
                {
                    myNamedPipeServerStream.Disconnect();
                }
                myNamedPipeServerStream.Dispose();
            }
        }

        /// <summary>
        /// Process incoming data from the IPC connection.
        /// </summary>
        private void ProcessServerStream()
        {
            byte[] message;

            while (ThreadRunning)
            {
                lock (myMemoryStream)
                {
                    try
                    {
                        while ((message = Data.GetMessage(myMemoryStream, ref myReadPos)) != null)
                        {
                            ProcessMessage(message, myInstance);
                        }
                    }
                    finally { Monitor.Wait(myMemoryStream); }
                }
            }
        }

        /// <summary>
        /// Process IPC message and fire corresponding event.
        /// </summary>
        /// <param name="message">
        /// IPC message to process.
        /// </param>
        /// <param name="instance">
        /// Instance of the IPC server that received the message.
        /// </param>
        /* Since there is one server for each client server instance = client instance */
        private static void ProcessMessage(byte[] message, int instance)
        {
            Command command = (Command)message[0];
            switch (command)
            {
                case Command.Ping:
                    dPing ping = PingEvent;
                    if (ping != null)
                    {
                        ThreadPool.QueueUserWorkItem(delegate { ping(instance); });
                    }
                    return;
                case Command.PingResponse:
                    dPingResponse pingResponse = PingResponseEvent;
                    if (pingResponse != null)
                    {
                        ThreadPool.QueueUserWorkItem(delegate { pingResponse(instance); });
                    }
                    return;
                case Command.Exception:
                    dException exception = ExceptionEvent;
                    if (exception != null)
                    {
                        string exceptionString = UnicodeEncoding.Unicode.GetString(message, 3, message.Length - 3);
                        ThreadPool.QueueUserWorkItem(delegate { exception(instance, exceptionString); });
                    }
                    return;
                case Command.Message:
                    dMessage dmessage = MessageEvent;
                    if (dmessage != null)
                    {
                        string messageString = UnicodeEncoding.Unicode.GetString(message, 3, message.Length - 3);
                        ThreadPool.QueueUserWorkItem(delegate { dmessage(instance, messageString); });
                    }
                    return;
                case Command.ClientID:
                    dClientID clientid = ClientIDEvent;
                    if (clientid != null)
                    {
                        int pid = BitConverter.ToInt32(message, 1);
                        ThreadPool.QueueUserWorkItem(delegate { clientid(instance, pid); });
                    }
                    return;
                case Command.ClientVersion:
                    dClientVersion clientVersion = ClientVersionEvent;
                    if (clientVersion != null)
                    {
                        int version = BitConverter.ToInt32(message, 1);
                        ThreadPool.QueueUserWorkItem(delegate { clientVersion(instance, version); });
                    }
                    return;
                case Command.FunctionPointer:
                    dFunctionPointer functionPointer = FunctionPointerEvent;
                    if (functionPointer != null)
                    {
                        uint address = BitConverter.ToUInt32(message, 1);
                        ThreadPool.QueueUserWorkItem(delegate { functionPointer(instance, address, (FunctionType)message[5]); });
                    }
                    return;
                case Command.KeyDown:
                    dKeyDown keyDown = KeyDownEvent;
                    if (keyDown != null)
                    {
                        int downCode = BitConverter.ToInt32(message, 1);
                        ThreadPool.QueueUserWorkItem(delegate { keyDown(instance, (Keys)downCode); });
                    }
                    return;
                case Command.KeyUp:
                    dKeyUp keyUp = KeyUpEvent;
                    if (keyUp != null)
                    {
                        int upCode = BitConverter.ToInt32(message, 1);
                        ThreadPool.QueueUserWorkItem(delegate { keyUp(instance, (Keys)upCode); });
                    }
                    return;
                case Command.MouseMove:
                    dMouseMove mouseMove = MouseMoveEvent;
                    if (mouseMove != null)
                    {
                        int x = BitConverter.ToInt32(message, 1);
                        int y = BitConverter.ToInt32(message, 5);
                        ThreadPool.QueueUserWorkItem(delegate { mouseMove(instance, x, y); });
                    }
                    return;
                case Command.MouseWheel:
                    dMouseWheel mouseWheel = MouseWheelEvent;
                    if (mouseWheel != null)
                    {
                        int x = BitConverter.ToInt32(message, 1);
                        int y = BitConverter.ToInt32(message, 5);
                        ThreadPool.QueueUserWorkItem(delegate { mouseWheel(instance, x, y, (sbyte)message[9]); });
                    }
                    return;
                case Command.MouseDblClick:
                    dMouseDblClick mouseDblClick = MouseDblClickEvent;
                    if (mouseDblClick != null)
                    {
                        int x = BitConverter.ToInt32(message, 1);
                        int y = BitConverter.ToInt32(message, 5);
                        MouseButtons button;
                        switch (message[9])
                        {
                            case 1:
                                button = MouseButtons.Left;
                                break;
                            case 2:
                                button = MouseButtons.Middle;
                                break;
                            case 3:
                                button = MouseButtons.Right;
                                break;
                            default: //should never happen
                                button = MouseButtons.None;
                                break;
                        }
                        ThreadPool.QueueUserWorkItem(delegate { mouseDblClick(instance, x, y, button); });
                    }
                    return;
                case Command.MouseClick:
                    dMouseClick mouseClick = MouseClickEvent;
                    if (mouseClick != null)
                    {
                        int x = BitConverter.ToInt32(message, 1);
                        int y = BitConverter.ToInt32(message, 5);
                        MouseButtons button;
                        switch (message[9])
                        {
                            case 1:
                                button = MouseButtons.Left;
                                break;
                            case 2:
                                button = MouseButtons.Middle;
                                break;
                            case 3:
                                button = MouseButtons.Right;
                                break;
                            default: //should never happen
                                button = MouseButtons.None;
                                break;
                        }
                        ThreadPool.QueueUserWorkItem(delegate { mouseClick(instance, x, y, button); });
                    }
                    return;
                case Command.MouseDown:
                    dMouseDown mouseDown = MouseDownEvent;
                    if (mouseDown != null)
                    {
                        int x = BitConverter.ToInt32(message, 1);
                        int y = BitConverter.ToInt32(message, 5);
                        MouseButtons button;
                        switch (message[9])
                        {
                            case 1:
                                button = MouseButtons.Left;
                                break;
                            case 2:
                                button = MouseButtons.Middle;
                                break;
                            case 3:
                                button = MouseButtons.Right;
                                break;
                            default: //should never happen
                                button = MouseButtons.None;
                                break;
                        }
                        ThreadPool.QueueUserWorkItem(delegate { mouseDown(instance, x, y, button); });
                    }
                    return;
                case Command.MouseUp:
                    dMouseUp mouseUp = MouseUpEvent;
                    if (mouseUp != null)
                    {
                        int x = BitConverter.ToInt32(message, 1);
                        int y = BitConverter.ToInt32(message, 5);
                        MouseButtons button;
                        switch (message[9])
                        {
                            case 1:
                                button = MouseButtons.Left;
                                break;
                            case 2:
                                button = MouseButtons.Middle;
                                break;
                            case 3:
                                button = MouseButtons.Right;
                                break;
                            default: //should never happen
                                button = MouseButtons.None;
                                break;
                        }
                        ThreadPool.QueueUserWorkItem(delegate { mouseUp(instance, x, y, button); });
                    }
                    return;
                case Command.IncomingPacket:
                    dIncomingPacket incomingPacket = IncomingPacketEvent;
                    if (incomingPacket != null)
                    {
                        byte[] inPacket = new byte[message.Length - 3];
                        Buffer.BlockCopy(message, 3, inPacket, 0, message.Length - 3);
                        ThreadPool.QueueUserWorkItem(delegate { incomingPacket(instance, inPacket); });
                    }
                    return;
                case Command.OutgoingPacket:
                    dOutgoingPacket outgoingPacket = OutgoingPacketEvent;
                    if (outgoingPacket != null)
                    {
                        byte[] outPacket = new byte[message.Length - 3];
                        Buffer.BlockCopy(message, 3, outPacket, 0, message.Length - 3);
                        ThreadPool.QueueUserWorkItem(delegate { outgoingPacket(instance, outPacket); });
                    }
                    return;
                default:
                    return;
            }
        }

        private void OnReceive(IAsyncResult asyncResult)
        {
            try
            {
                int received = myNamedPipeServerStream.EndRead(asyncResult);
                if (received > 0)
                {
                    lock (myMemoryStream)
                    {
                        myMemoryStream.Write(myBuffer, 0, received);
                        Monitor.Pulse(myMemoryStream);
                    }
                }
                if (myNamedPipeServerStream.IsConnected)
                    myNamedPipeServerStream.BeginRead(myBuffer, 0, myBuffer.Length, new AsyncCallback(OnReceive), null); 
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        private void OnConnect(IAsyncResult asyncResult)
        {
            try
            {
                myNamedPipeServerStream.EndWaitForConnection(asyncResult);
                myNamedPipeServerStream.BeginRead(myBuffer, 0, myBuffer.Length, new AsyncCallback(OnReceive), null);
            }
            catch (ObjectDisposedException) { }
            catch (OperationCanceledException) { }
        }

        /// <summary>
        /// Send variable length IPC message to IPC client.
        /// </summary>
        /// <param name="command">
        /// IPC.Command to send.
        /// </param>
        /// <param name="message">
        /// Command argument.
        /// </param>
        public void SendCommand(Command command, string message)
        {
            try
            {
                lock (myNamedPipeServerStream)
                {
                    if (myNamedPipeServerStream.IsConnected)
                    {
                        byte[] stringBytes = UnicodeEncoding.Unicode.GetBytes(message);
                        byte[] ipcMessage = new byte[stringBytes.Length + 3];
                        ipcMessage[0] = (byte)command;
                        ipcMessage[1] = (byte)message.Length;
                        ipcMessage[2] = (byte)(message.Length >> 8);
                        Buffer.BlockCopy(stringBytes, 0, ipcMessage, 3, stringBytes.Length);
                        myNamedPipeServerStream.Write(ipcMessage, 0, ipcMessage.Length);
                    }
                }
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Send variable length IPC message to IPC client.
        /// </summary>
        /// <param name="command">
        /// IPC.Command to send.
        /// </param>
        /// <param name="data">
        /// Command argument.
        /// </param>
        public void SendCommand(Command command, byte[] data)
        {
            try
            {
                lock (myNamedPipeServerStream)
                {
                    if (myNamedPipeServerStream.IsConnected)
                    {
                        byte[] message = new byte[data.Length + 3];
                        message[0] = (byte)command;
                        message[1] = (byte)message.Length;
                        message[2] = (byte)(message.Length >> 8);
                        Buffer.BlockCopy(data, 0, message, 3, data.Length);
                        myNamedPipeServerStream.Write(message, 0, message.Length);
                    }
                }
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Send variable length IPC message to IPC client.
        /// </summary>
        /// <param name="command">
        /// IPC.Command to send.
        /// </param>
        /// <param name="arg1"> Command argument.</param>
        /// <param name="arg2"> Command argument.</param>
        /// <param name="data"> Command argument.</param>
        public void SendCommand(Command command, int arg1, byte arg2, byte[] data)
        {
            try
            {
                lock (myNamedPipeServerStream)
                {
                    if (myNamedPipeServerStream.IsConnected)
                    {
                        byte[] message = new byte[data.Length + 8];  //command = 1 byte, len = 2, int = 4 each, byte = 1
                        message[0] = (byte)command;
                        message[1] = (byte)message.Length;
                        message[2] = (byte)(message.Length >> 8);
                        message[3] = (byte)arg1;
                        message[4] = (byte)(arg1 >> 8);
                        message[5] = (byte)(arg1 >> 16);
                        message[6] = (byte)(arg1 >> 24);
                        message[7] = (byte)arg2;
                        Buffer.BlockCopy(data, 0, message, 8, data.Length);
                        myNamedPipeServerStream.Write(message, 0, message.Length);
                    }
                }
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Send variable length IPC message to IPC client.
        /// </summary>
        /// <param name="command">
        /// IPC.Command to send.
        /// </param>
        /// <param name="arg1"> Command argument.</param>
        /// <param name="arg2"> Command argument.</param>
        /// <param name="data"> Command argument.</param>
        public void SendCommand(Command command, int arg1, int arg2, byte arg3, byte[] data)
        {
            try
            {
                lock (myNamedPipeServerStream)
                {
                    if (myNamedPipeServerStream.IsConnected)
                    {
                        byte[] message = new byte[data.Length + 12];
                        message[0] = (byte)command;
                        message[1] = (byte)message.Length;
                        message[2] = (byte)(message.Length >> 8);
                        message[3] = (byte)arg1;
                        message[4] = (byte)(arg1 >> 8);
                        message[5] = (byte)(arg1 >> 16);
                        message[6] = (byte)(arg1 >> 24);
                        message[7] = (byte)arg2;
                        message[8] = (byte)(arg2 >> 8);
                        message[9] = (byte)(arg2 >> 16);
                        message[10] = (byte)(arg2 >> 24);
                        message[11] = arg3;
                        Buffer.BlockCopy(data, 0, message, 12, data.Length);
                        myNamedPipeServerStream.Write(message, 0, message.Length);
                    }
                }
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Send 13 byte IPC message to IPC client.
        /// </summary>
        public void SendCommand(Command command, int arg1, int arg2, int arg3)
        {
            try
            {
                lock (myNamedPipeServerStream)
                {
                    if (myNamedPipeServerStream.IsConnected)
                    {
                        byte[] message = new byte[13];
                        message[0] = (byte)command;
                        message[1] = (byte)arg1;
                        message[2] = (byte)(arg1 >> 8);
                        message[3] = (byte)(arg1 >> 16);
                        message[4] = (byte)(arg1 >> 24);
                        message[5] = (byte)arg2;
                        message[6] = (byte)(arg2 >> 8);
                        message[7] = (byte)(arg2 >> 16);
                        message[8] = (byte)(arg2 >> 24);
                        message[9] = (byte)arg3;
                        message[10] = (byte)(arg3 >> 8);
                        message[11] = (byte)(arg3 >> 16);
                        message[12] = (byte)(arg3 >> 24);
                        myNamedPipeServerStream.Write(message, 0, message.Length);
                    }
                }
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Send 2 byte IPC message to IPC client.
        /// </summary>
        /// <param name="command">
        /// IPC.Command to send.
        /// </param>
        /// <param name="data">
        /// Command argument.
        /// </param>
        public void SendCommand(Command command, byte data)
        {
            try
            {
                lock (myNamedPipeServerStream)
                {
                    if (myNamedPipeServerStream.IsConnected)
                    {
                        byte[] message = new byte[2];
                        message[0] = (byte)command;
                        message[1] = data;
                        myNamedPipeServerStream.Write(message, 0, 2);
                    }
                }
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Send single-byte IPC message to IPC client.
        /// </summary>
        /// <param name="command">
        /// IPC.Command to send.
        /// </param>
        public void SendCommand(Command command)
        {
            try
            {
                lock (myNamedPipeServerStream)
                {
                    if (myNamedPipeServerStream.IsConnected)
                        myNamedPipeServerStream.WriteByte((byte)command);
                }
            }
            catch (IOException) { }
        }
    }
}