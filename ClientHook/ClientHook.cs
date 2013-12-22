using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using System.IO.Pipes;
using EasyHook;
using UOMachine;
using UOMachine.IPC;
using System.Diagnostics;
using System.Security;

namespace ClientHook
{
    public sealed class Main : EasyHook.IEntryPoint
    {
        private static LocalHook mySocketHook, myCloseSocketHook;
        private static int myPID, myThreadID, myDateStamp;
        private static dSendRecv myRecvDelegate, mySendDelegate;
        private static dSocket mySocketDelegate;
        private static dCloseSocket myCloseSocketDelegate;
        private static uint mySocket = UInt32.MaxValue;
        private static bool[] myRecvFilter, mySendFilter;
        private static object myClientSendLock;
        private static bool myNewStylePackets, mySocketReady;
        private static ClientInstance myClientInstance;
        private static IntPtr myServerSendBuffer, myClientSendBuffer;
        private static byte[] myServerBufferAddress, myClientBufferAddress;

        private const int SOCKET_ERROR = -1;

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate void dSendRecv(IntPtr buf, int len);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int dCloseSocket(uint s);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate uint dSocket(int af, int type, int protocol);

        [DllImport("WSOCK32.dll", SetLastError = false, CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        static extern int send(uint s, IntPtr buf, int len, int flags);

        [DllImport("WS2_32.dll", SetLastError = false, CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        static extern uint socket(int af, int type, int protocol);

        [DllImport("WS2_32.dll", EntryPoint = "closesocket", SetLastError = false, CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        static extern int closesocket(uint s);

        private static uint SocketHook(int af, int type, int protocol)
        {
            mySocket = socket(af, type, protocol);
            mySocketReady = true;
            //myClientInstance.SendCommand(Command.Message, "Socket " + mySocket.ToString() + " created.");
            return mySocket;
        }

        public static void SendHook(IntPtr buf, int len)
        {
            byte[] buffer = new byte[len];
            Marshal.Copy(buf, buffer, 0, len);
            if (mySendFilter[buffer[0]])
                myClientInstance.SendCommand(Command.OutgoingPacket, buffer);
        }

        public static void ReceiveHook(IntPtr buf, int len)
        {
            byte[] buffer = new byte[len];
            Marshal.Copy(buf, buffer, 0, len);
            if (myRecvFilter[buffer[0]])
                myClientInstance.SendCommand(Command.IncomingPacket, buffer);
        }

        private static unsafe int GetDateStamp()
        {
            byte* address = (byte*)0x40003C;
            int offset = address[1] << 8 | address[0];
            address = (byte*)0x400000 + offset + 8;
            return address[3] << 24 | address[2] << 16 | address[1] << 8 | address[0];
        }

        private static int SocketCloseHook(uint s)
        {
            if (mySocket == s)
            {
                mySocketReady = false;
                mySocket = 0;
            }
            //myClientInstance.SendCommand(Command.Message, "Socket " + s.ToString() + " closed.");
            return closesocket(s);
        }

        public Main(RemoteHooking.IContext InContext, string serverName)
        {
            mySocketReady = false;
            myRecvFilter = new bool[256];
            mySendFilter = new bool[256];
            mySocket = 0;
            myRecvDelegate = new dSendRecv(ReceiveHook);
            mySendDelegate = new dSendRecv(SendHook);
            mySocketDelegate = new dSocket(SocketHook);
            myCloseSocketDelegate = new dCloseSocket(SocketCloseHook);
            myClientSendLock = new object();
            myPID = RemoteHooking.GetCurrentProcessId();
            myThreadID = RemoteHooking.GetCurrentThreadId();
            myDateStamp = GetDateStamp();
            if (myDateStamp >= 1183740939) myNewStylePackets = true;
            else myNewStylePackets = false;
            myServerSendBuffer = Marshal.AllocHGlobal(65536);
            myClientSendBuffer = Marshal.AllocHGlobal(65536);
            myServerBufferAddress = BitConverter.GetBytes(myServerSendBuffer.ToInt32());
            myClientBufferAddress = BitConverter.GetBytes(myClientSendBuffer.ToInt32());

            myClientInstance = new ClientInstance(serverName, true);
            myClientInstance.SendCommand(Command.ClientID, myPID);
            myClientInstance.SendPacketEvent += new dSendPacket(myClientInstance_sendPacketEvent);
            myClientInstance.PingEvent += new dPing(myClientInstance_pingEvent);
            myClientInstance.AddRecvFilterEvent += new dAddRecvFilter(myClientInstance_addRecvFilterEvent);
            myClientInstance.AddSendFilterEvent += new dAddSendFilter(myClientInstance_addSendFilterEvent);
            myClientInstance.RemoveRecvFilterEvent += new dRemoveRecvFilter(myClientInstance_removeRecvFilterEvent);
            myClientInstance.RemoveSendFilterEvent += new dRemoveSendFilter(myClientInstance_removeSendFilterEvent);
            myClientInstance.ClearRecvFilterEvent += new dClearRecvFilter(myClientInstance_clearRecvFilterEvent);
            myClientInstance.ClearSendFilterEvent += new dClearSendFilter(myClientInstance_clearSendFilterEvent);
            BuildDefaultPacketFilters();
        }

        ~Main()
        {
            if (myServerSendBuffer != IntPtr.Zero)
                Marshal.FreeHGlobal(myServerSendBuffer);
            if (myClientSendBuffer != IntPtr.Zero)
                Marshal.FreeHGlobal(myClientSendBuffer);
        }

        public void Run(RemoteHooking.IContext InContext, string serverName)
        {
            try
            {
                myClientInstance.SendCommand(Command.ClientVersion, myDateStamp);
                myCloseSocketHook = LocalHook.Create(LocalHook.GetProcAddress("WSOCK32.dll", "closesocket"), myCloseSocketDelegate, null);
                mySocketHook = LocalHook.Create(LocalHook.GetProcAddress("WSOCK32.dll", "socket"), mySocketDelegate, null);
                myCloseSocketHook.ThreadACL.SetExclusiveACL(new int[] { 0 });
                mySocketHook.ThreadACL.SetExclusiveACL(new int[] { 0 });
                IntPtr functionPtr = Marshal.GetFunctionPointerForDelegate(myRecvDelegate);
                myClientInstance.SendCommand(Command.FunctionPointer, functionPtr.ToInt32(), 0);
                functionPtr = Marshal.GetFunctionPointerForDelegate(mySendDelegate);
                myClientInstance.SendCommand(Command.FunctionPointer, functionPtr.ToInt32(), 1);
                MessageHook.Initialize(myClientInstance);
            }
            catch (Exception X)
            {
                string message = "<Exception : " + X.Message + "> <Stack trace: " + X.StackTrace + ">";
                myClientInstance.SendCommand(Command.Exception, message);
                return;
            }

            while (true)
            {
                Thread.Sleep(1000);
            }

        }

        private static void BuildDefaultPacketFilters()
        {
            mySendFilter[0x02] = true;
            mySendFilter[0x06] = true;
            mySendFilter[0x07] = true;
            mySendFilter[0x08] = true;
            mySendFilter[0x3A] = true;
            mySendFilter[0x6C] = true;
            mySendFilter[0xB1] = true;
            mySendFilter[0xBF] = true;

            myRecvFilter[0x01] = true;
            myRecvFilter[0x0B] = true;
            myRecvFilter[0x11] = true;
            myRecvFilter[0x1A] = true;
            myRecvFilter[0x1B] = true;
            myRecvFilter[0x1C] = true;
            myRecvFilter[0x1D] = true;
            myRecvFilter[0x20] = true;
            myRecvFilter[0x21] = true;
            myRecvFilter[0x22] = true;
            myRecvFilter[0x24] = true;
            myRecvFilter[0x25] = true;
            myRecvFilter[0x2C] = true;
            myRecvFilter[0x2D] = true;
            myRecvFilter[0x2E] = true;
            myRecvFilter[0x2F] = true;
            myRecvFilter[0x30] = true;
            myRecvFilter[0x31] = true;
            myRecvFilter[0x3A] = true;
            myRecvFilter[0x3C] = true;
            myRecvFilter[0x4F] = true;
            myRecvFilter[0x4E] = true;
            myRecvFilter[0x54] = true;
            myRecvFilter[0x6C] = true;
            myRecvFilter[0x77] = true;
            myRecvFilter[0x78] = true;
            myRecvFilter[0x7C] = true;
            myRecvFilter[0x89] = true;
            myRecvFilter[0x98] = true;
            myRecvFilter[0xA1] = true;
            myRecvFilter[0xA2] = true;
            myRecvFilter[0xA3] = true;
            myRecvFilter[0xA8] = true;
            myRecvFilter[0xA9] = true;//
            myRecvFilter[0xAA] = true;
            myRecvFilter[0xAD] = true;
            myRecvFilter[0xAE] = true;
            myRecvFilter[0xAF] = true;
            myRecvFilter[0xB0] = true;
            myRecvFilter[0xB2] = true;
            myRecvFilter[0xBF] = true;
            myRecvFilter[0xC1] = true;
            myRecvFilter[0xC2] = true;
            myRecvFilter[0xCC] = true;
            myRecvFilter[0xD1] = true;
            myRecvFilter[0xD2] = true;
            myRecvFilter[0xD6] = true;
            myRecvFilter[0xDD] = true;
            myRecvFilter[0xDE] = true;
            myRecvFilter[0xDF] = true;
            myRecvFilter[0xF3] = true;
            myRecvFilter[0x9E] = true;
            myRecvFilter[0x74] = true;
            myRecvFilter[0x3B] = true;
        }

        private static void myClientInstance_addSendFilterEvent(byte packetID)
        {
            mySendFilter[packetID] = true;
        }

        private static void myClientInstance_addRecvFilterEvent(byte packetID)
        {
            myRecvFilter[packetID] = true;
        }

        private static void myClientInstance_removeSendFilterEvent(byte packetID)
        {
            mySendFilter[packetID] = false;
        }

        private static void myClientInstance_removeRecvFilterEvent(byte packetID)
        {
            myRecvFilter[packetID] = false;
        }

        private static void myClientInstance_clearSendFilterEvent()
        {
            mySendFilter.Initialize();
        }

        private static void myClientInstance_clearRecvFilterEvent()
        {
            myRecvFilter.Initialize();
        }

        private static void myClientInstance_pingEvent(int clientID)
        {
            myClientInstance.SendCommand(Command.PingResponse);
        }

        private static unsafe void myClientInstance_sendPacketEvent(int caveAddress, PacketType packetType, byte[] data)
        {
            byte* cave = (byte*)caveAddress;
            lock (myClientSendLock)
            {
                switch (packetType)
                {
                    case PacketType.Client:
                        Marshal.Copy(data, 0, myClientSendBuffer, data.Length);
                        cave[0] = myClientBufferAddress[0];
                        cave[1] = myClientBufferAddress[1];
                        cave[2] = myClientBufferAddress[2];
                        cave[3] = myClientBufferAddress[3];
                        cave[4] = (byte)(data.Length & 0xFF);
                        cave[5] = (byte)((data.Length >> 8) & 0xFF);
                        cave[6] = (byte)((data.Length >> 16) & 0xFF);
                        cave[7] = (byte)((data.Length >> 24) & 0xFF);
                        cave[8] = 0x01;
                        break;
                    case PacketType.Server:
                        Marshal.Copy(data, 0, myServerSendBuffer, data.Length);
                        cave[0] = myServerBufferAddress[0];
                        cave[1] = myServerBufferAddress[1];
                        cave[2] = myServerBufferAddress[2];
                        cave[3] = myServerBufferAddress[3];
                        cave[4] = 0x01;
                        break;
                }
            }
        }

    }
}
