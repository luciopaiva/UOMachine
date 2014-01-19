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
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using UOMachine.IPC;
using UOMachine.Macros;
using UOMachine.Tree;
using UOMachine.Utility;

namespace UOMachine
{
    public sealed class ClientInfo
    {
        private object myWaitForTargetLock;
        private Thread myCleanupThread;
        private const int myMaxDistance = 36; //items get deleted past this point
        private int[] mySequenceList;
        private int myWaitingForTarget;
        //private const int myCleanupDelay = 60000;

        public int DateStamp { get; internal set; }
        public int IPCServerIndex { get; private set; }
        public int ProcessID { get; private set; }
        public string Version { get; internal set; }
        public IntPtr HookAddress { get; internal set; }
        public int OriginalDest { get; internal set; }
        public int ReturnAddress { get; internal set; }
        public IntPtr TopGumpPointer { get; internal set; }

        public IntPtr TopGumpHandle
        {
            get
            {
                byte[] data = new byte[4];
                Memory.Read(this.Handle, this.TopGumpPointer, data, true);
                return (IntPtr)BitConverter.ToUInt32(data, 0);
            }
        }

        public IntPtr LoginServerAddress { get; internal set; }
        public IntPtr LoginPortAddress { get; internal set; }
        public IntPtr GumpListPointer { get; internal set; }
        public IntPtr CaveAddress { get; internal set; }
        public IntPtr GumpFunctionCaveAddress { get; internal set; }
        public IntPtr PlayerInfoPointer { get; internal set; }
        public IntPtr ZAddress { get; internal set; }
        public IntPtr RecvCaveAddress { get; internal set; }
        public IntPtr RecvHookAddress  { get; internal set; }
        public IntPtr RecvFunctionPointer { get; internal set; }  //this is address of method in clienthook
        public IntPtr SendCaveAddress { get; internal set; }
        public IntPtr SendHookAddress { get; internal set; }
        public IntPtr SendFunctionPointer { get; internal set; }  //this is address of method in clienthook
        public IntPtr ClientSendCaveAddress { get; internal set; }
        public IntPtr ServerSendCaveAddress { get; internal set; }
        public IntPtr PathFindCaveAddress { get; internal set; }
        public int PathFindFunction { get; internal set; }
        public IntPtr CursorAddress { get; internal set; }
        public int ClientPacketData { get; internal set; }
        public int ServerPacketSendFunction { get; internal set; }
        public int LoginFunction { get; internal set; }
        public byte RecvHookType { get; internal set; } // 0 = old type, 1 = new type
        public byte SendHookType { get; internal set; }
        public byte PathFindType { get; internal set; }
        public bool IsValid { get; internal set; }
        public byte[] PrimaryCave { get; internal set; }
        public IntPtr WindowHandle { get; private set; }
        public IntPtr Handle { get; private set; }
        public IntPtr BaseAddress { get; private set; }
        public IntPtr EntryPoint { get; private set; }
        public uint ThreadID { get; private set; }
        public HotKeyList HotKeyList { get; private set; }
        public ItemCollection Items { get; private set; }
        public MobileCollection Mobiles { get; private set; }
        public GenericGumpCollection GenericGumps { get; private set; }
        public CircularBuffer<JournalEntry> Journal{ get; private set; }
        public int LastTarget { get; internal set; }

        private SkillInfo[] mySkills;
        public SkillInfo[] Skills
        {
            set { Interlocked.Exchange(ref mySkills, value); }
            get { return ThreadHelper.VolatileRead<SkillInfo[]>(ref mySkills); }
        }

        //shouldn't need to be thread safe (hopefully)
        private PlayerMobile myPlayer;
        public PlayerMobile Player
        {
            get { return myPlayer; }
            set
            {
                myPlayer = value;
                value.PropertyChangedEvent += new PlayerMobile.dPropertyChanged(Player_PropertyChanged);
            }
        }

        // testing 
        private IntPtr myAllocCodeAddress;
        public IntPtr AllocCodeAddress
        {
            get { return myAllocCodeAddress; }
            set { myAllocCodeAddress = value; }
        }

        private bool myNewStyleLoginPatch = false;
        public bool NewStyleLoginPatch
        {
            get { return myNewStyleLoginPatch; }
            set { myNewStyleLoginPatch = true; }
        }
        // end testing

        private int myCurrentAttackTarget;
        public int CurrentAttackTarget
        {
            get { return Thread.VolatileRead(ref myCurrentAttackTarget); }
            set { myCurrentAttackTarget = value; }
        }

        private bool myThreadRunning;
        private bool ThreadRunning
        {
            get { return ThreadHelper.VolatileRead(ref myThreadRunning); }
            set { myThreadRunning = value; }
        }

        private int myLastMoveSequence;
        public int LastMoveSequence
        {
            get { return Thread.VolatileRead(ref myLastMoveSequence); }
            set { myLastMoveSequence = value; }
        }

        public int NextMoveSequence
        {
            get
            {
                int nextSequence = LastMoveSequence++;
                if (nextSequence == 256) nextSequence = 1;
                return nextSequence;
            }
        }

        private int myInstance;
        public int Instance
        {
            get { return Thread.VolatileRead(ref myInstance); }
            set { myInstance = value; }
        }

        private byte[] myLastTargetPacket;
        public byte[] LastTargetPacket
        {
            get { return ThreadHelper.VolatileRead<byte[]>(ref myLastTargetPacket); }
            set { myLastTargetPacket = value; }
        }

        public int GetSequence(int sequence)
        {
            return Thread.VolatileRead(ref mySequenceList[sequence]);
        }

        public void SetSequence(int sequence, int direction)
        {
            mySequenceList[sequence] = direction;
        }

        public void TargetReceived()
        {
            if (Thread.VolatileRead(ref myWaitingForTarget) == 1)
            {
                lock (myWaitForTargetLock) { Monitor.Pulse(myWaitForTargetLock); }
            }
        }

        public void WaitForTarget(int timeout)
        {
            myWaitingForTarget = 1;
            lock (myWaitForTargetLock) { Monitor.Wait(myWaitForTargetLock, timeout); }
        }

        private bool SetProcessInfo(Process p)
        {
            try
            {
                this.WindowHandle = p.MainWindowHandle;
                this.Handle = p.Handle;
                this.BaseAddress = p.MainModule.BaseAddress;
                this.EntryPoint = p.MainModule.EntryPointAddress;
                return true;
            }
            catch
            {
                p = Process.GetProcessById(p.Id);
                return false;
            }
        }

        public ClientInfo(Process p)
        {
            mySequenceList = new int[256];
            this.Instance = 0;
            this.ProcessID = p.Id;
            bool success = false;
            for (int x = 0; x < 100; x++)
            {
                if ((success = SetProcessInfo(p)))
                    break;
                Thread.Sleep(100);
            }
            if (!success)
            {
                this.IsValid = false;
                return;
            }

            // testing
            IntPtr codeMemory;
            codeMemory = Memory.Allocate(p.Handle, IntPtr.Zero, 4096, true); //Win32.VirtualAllocEx(p.Handle, IntPtr.Zero, 4096, Win32.AllocationType.Commit, Win32.MemoryProtection.ExecuteReadWrite);
            if (codeMemory != IntPtr.Zero)
                this.AllocCodeAddress = codeMemory;
            // end testing
            Memory.SetAddresses(this, p.MainModule.FileName);
            uint pid;
            this.ThreadID = Win32.GetWindowThreadProcessId(this.WindowHandle, out pid);
            this.HotKeyList = new HotKeyList(32);
            this.Items = new ItemCollection(0, 512);
            this.Mobiles = new MobileCollection(this.Items);
            this.Journal = new CircularBuffer<JournalEntry>(1024);
            this.GenericGumps = new GenericGumpCollection();
            this.Player = new PlayerMobile(0, -1);
            myWaitForTargetLock = new object();
            myWaitingForTarget = 0;
            this.Items.CollectionChangedEvent += new ItemCollection.dCollectionChanged(Items_CollectionChanged);
            this.Mobiles.CollectionChangedEvent += new MobileCollection.dCollectionChanged(Mobiles_CollectionChanged);
            this.GenericGumps.CollectionChangedEvent += new GenericGumpCollection.dCollectionChanged(myCustomGumps_CollectionChangedEvent);
            myThreadRunning = true;
            myCleanupThread = new Thread(new ThreadStart(CleanupObjects));
            myCleanupThread.Start();
            this.IPCServerIndex = Network.CreateServer(UOM.ServerName, true);
        }

        public void Dispose()
        {
            myThreadRunning = false;
            Network.RemoveServer(IPCServerIndex);
            TreeViewUpdater.RemoveParent(this.ProcessID);
            try
            {
                if (myCleanupThread != null)
                    myCleanupThread.Abort();
            }
            catch { }
        }

        /// <summary>
        /// Detach from client's message loop.
        /// </summary>
        /// <returns></returns>
        public bool DetachFromWindow()
        {
            uint thread = Win32.GetCurrentThreadId();
            return Win32.AttachThreadInput(thread, this.ThreadID, false);
        }

        /// <summary>
        /// Attach to client's message loop, restore window if minimized, and bring window forward.
        /// </summary>
        public bool PrepareWindowForInput()
        {
            Win32.WINDOWPLACEMENT wp = new Win32.WINDOWPLACEMENT();
            uint thread = Win32.GetCurrentThreadId();
            if (Win32.AttachThreadInput(thread, this.ThreadID, true))
            {
                if (Win32.GetWindowPlacement(this.WindowHandle, ref wp))
                {
                    if (wp.showCmd == Win32.SW_SHOWMINIMIZED)
                    {
                        wp.showCmd = Win32.SW_SHOWDEFAULT;
                        if (!Win32.SetWindowPlacement(this.WindowHandle, ref wp))
                            return false;
                    }
                    return Win32.SetForegroundWindow(this.WindowHandle);
                }
            }
            return false;
        }

        public void OnMouseMove(int x, int y)
        {
            TreeViewUpdater.UpdateMouse(this.ProcessID, x, y);
        }

        public void OnKeyDown(Keys key)
        {
            TreeViewUpdater.UpdateKeypress(this.ProcessID, key);
        }

        public void OnGumpAction(int serial, int gumpID, int buttonID, int[] switches)
        {
            TreeViewUpdater.UpdateLastGumpAction(this.ProcessID, serial, gumpID, buttonID, switches);
        }

        public void SetSkillInfo(int skillID, SkillInfo skillInfo)
        {
            Interlocked.Exchange<SkillInfo>(ref this.Skills[skillID], skillInfo);
        }

        public SkillInfo GetSkillInfo(int skillID)
        {
            if (this.Skills != null)
            {
                return ThreadHelper.VolatileRead<SkillInfo>(ref this.Skills[skillID]);
            }
            else
            {
                return null;
            }
        }

        private void Player_PropertyChanged(int headerNum, string header)
        {
            TreeViewUpdater.EditNodeHeader(this.ProcessID, 0, headerNum, header);
        }

        private void CleanupObjects()
        {
            while (ThreadRunning)
            {
                if (this.Player != null)
                {
                    this.Mobiles.RemoveByDistance(myMaxDistance, Player.X, Player.Y);
                    this.Items.RemoveByDistance(myMaxDistance, Player.X, Player.Y);
                }
                for (int x = 0; x < 120; x++)
                {
                    if (ThreadRunning)
                    {
                        Thread.Sleep(500);
                    }
                    else break;
                }
            }
        }

        private void Items_CollectionChanged(int newCount)
        {
            TreeViewUpdater.EditNodeHeader(this.ProcessID, 3, 0, "Tracked items = " + newCount);
        }

        private void Mobiles_CollectionChanged(int newCount)
        {
            TreeViewUpdater.EditNodeHeader(this.ProcessID, 3, 1, "Tracked mobiles = " + (newCount + 1));
        }

        private void myCustomGumps_CollectionChangedEvent(int newCount)
        {
            TreeViewUpdater.EditNodeHeader(this.ProcessID, 3, 2, "Custom gumps = " + (newCount));
        }

        public void InstallMacroHook()
        {
            Assembler.PrepareGumpFunctionCave(this);
            Assembler.PreparePathfindCave(this);
            Assembler.PrepareClientSendCave(this);
            Assembler.PrepareServerSendCave(this);
            Assembler.PreparePrimaryCave(this);
        }
    }
}