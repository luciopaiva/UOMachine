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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using UOMachine.IPC;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;

namespace ClientHook
{
    static unsafe partial class MessageHook
    {
        private static ClientInstance myClientInstance;
        private static IntPtr myMsgHookHandle, myWindowMsgHookHandle, myWindowHandle;
        private static HookProc myMsgHook, myWindowMsgHook;


        public static void Initialize(ClientInstance clientInstance)
        {
            myWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            myClientInstance = clientInstance;
            myMsgHook = new HookProc(MsgHook);
            myWindowMsgHook = new HookProc(WindowMsgHook);
            Process p = Process.GetProcessById(EasyHook.RemoteHooking.GetCurrentProcessId());
            uint x;
            uint threadID = GetWindowThreadProcessId(p.MainWindowHandle, out x);
            myMsgHookHandle = SetWindowsHookEx(WH_GETMESSAGE,
                Marshal.GetFunctionPointerForDelegate(myMsgHook),
                Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                    threadID);
            myWindowMsgHookHandle = SetWindowsHookEx(WH_CALLWNDPROC,
                Marshal.GetFunctionPointerForDelegate(myWindowMsgHook),
                Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                    threadID);
            WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
            if (GetWindowPlacement(myWindowHandle, ref wp))
            {
                MessageHook.myWindowX = wp.rcNormalPosition.X;
                MessageHook.myWindowY = wp.rcNormalPosition.Y;
            }
            if (myMsgHookHandle == IntPtr.Zero || myWindowMsgHookHandle == IntPtr.Zero)
            {
                clientInstance.SendCommand(Command.Message, "Error installing WH_GETMESSAGE\\WH_CALLWNDPROC hooks!");
            }

        }
    }
}