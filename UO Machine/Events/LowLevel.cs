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
using UOMachine.IPC;
using UOMachine.Utility;
using System.Windows.Forms;

namespace UOMachine.Events
{
    /// <summary>
    /// Collection of thread safe events which provide notifications for low level events.
    /// </summary>
    public static class LowLevel
    {
        private static object myIncomingPacketLock = new object();
        private static event dIncomingPacket myIncomingPacketEvent;
        public static event dIncomingPacket IncomingPacketEvent
        {
            add { lock (myIncomingPacketLock) { myIncomingPacketEvent += value; } }
            remove { lock (myIncomingPacketLock) { myIncomingPacketEvent -= value; } }
        }

        private static object myOutgoingPacketLock = new object();
        private static event dOutgoingPacket myOutgoingPacketEvent;
        public static event dOutgoingPacket OutgoingPacketEvent
        {
            add { lock (myOutgoingPacketLock) { myOutgoingPacketEvent += value; } }
            remove { lock (myOutgoingPacketLock) { myOutgoingPacketEvent -= value; } }
        }

        private static object myKeyDownLock = new object();
        private static event dKeyDown myKeyDownEvent;
        public static event dKeyDown KeyDownEvent
        {
            add { lock (myKeyDownLock) { myKeyDownEvent += value; } }
            remove { lock (myKeyDownLock) { myKeyDownEvent -= value; } }
        }

        private static object myKeyUpLock = new object();
        private static event dKeyUp myKeyUpEvent;
        public static event dKeyUp KeyUpEvent
        {
            add { lock (myKeyUpLock) { myKeyUpEvent += value; } }
            remove { lock (myKeyUpLock) { myKeyUpEvent -= value; } }
        }

        private static object myMouseClickLock = new object();
        private static event dMouseClick myMouseClickEvent;
        public static event dMouseClick MouseClickEvent
        {
            add { lock (myMouseClickLock) { myMouseClickEvent += value; } }
            remove { lock (myMouseClickLock) { myMouseClickEvent -= value; } }
        }

        private static object myMouseDblClickLock = new object();
        private static event dMouseDblClick myMouseDblClickEvent;
        public static event dMouseDblClick MouseDblClickEvent
        {
            add { lock (myMouseDblClickLock) { myMouseDblClickEvent += value; } }
            remove { lock (myMouseDblClickLock) { myMouseDblClickEvent -= value; } }
        }

        private static object myMouseDownLock = new object();
        private static event dMouseDown myMouseDownEvent;
        public static event dMouseDown MouseDownEvent
        {
            add { lock (myMouseDownLock) { myMouseDownEvent += value; } }
            remove { lock (myMouseDownLock) { myMouseDownEvent -= value; } }
        }

        private static object myMouseUpLock = new object();
        private static event dMouseUp myMouseUpEvent;
        public static event dMouseUp MouseUpEvent
        {
            add { lock (myMouseUpLock) { myMouseUpEvent += value; } }
            remove { lock (myMouseUpLock) { myMouseUpEvent -= value; } }
        }

        private static object myMouseMoveLock = new object();
        private static event dMouseMove myMouseMoveEvent;
        public static event dMouseMove MouseMoveEvent
        {
            add { lock (myMouseMoveLock) { myMouseMoveEvent += value; } }
            remove { lock (myMouseMoveLock) { myMouseMoveEvent -= value; } }
        }

        private static object myMouseWheelLock = new object();
        private static event dMouseWheel myMouseWheelEvent;
        public static event dMouseWheel MouseWheelEvent
        {
            add { lock (myMouseWheelLock) { myMouseWheelEvent += value; } }
            remove { lock (myMouseWheelLock) { myMouseWheelEvent -= value; } }
        }

        public static void ClearEvents()
        {
            lock (myIncomingPacketLock) { myIncomingPacketEvent = null; }
            lock (myOutgoingPacketLock) { myOutgoingPacketEvent = null; }
            lock (myKeyDownLock) { myKeyDownEvent = null; }
            lock (myKeyUpLock) { myKeyUpEvent = null; }
            lock (myMouseClickLock) { myMouseClickEvent = null; }
            lock (myMouseDblClickLock) { myMouseDblClickEvent = null; }
            lock (myMouseDownLock) { myMouseDownEvent = null; }
            lock (myMouseUpLock) { myMouseUpEvent = null; }
            lock (myMouseMoveLock) { myMouseMoveEvent = null; }
            lock (myMouseWheelLock) { myMouseWheelEvent = null; }
        }

        internal static void OnIncomingPacket(int client, byte[] data)
        {
            lock (myIncomingPacketLock)
            {
                dIncomingPacket handler = myIncomingPacketEvent;
                try
                {
                if (handler != null) handler(client, data);
            }
                catch (Exception e) { }
            }
        }

        internal static void OnOutgoingPacket(int client, byte[] data)
        {
            lock (myOutgoingPacketLock)
            {
                dOutgoingPacket handler = myOutgoingPacketEvent;
                if (handler != null) handler(client, data);
            }
        }

        internal static void OnMouseMove(int client, int x, int y)
        {
            lock (myMouseMoveLock)
            {
                dMouseMove handler = myMouseMoveEvent;
                if (handler != null) handler(client, x, y);
            }
        }

        internal static void OnMouseDown(int client, int x, int y, MouseButtons button)
        {
            lock (myMouseDownLock)
            {
                dMouseDown handler = myMouseDownEvent;
                if (handler != null) handler(client, x, y, button);
            }
        }

        internal static void OnMouseUp(int client, int x, int y, MouseButtons button)
        {
            lock (myMouseUpLock)
            {
                dMouseUp handler = myMouseUpEvent;
                if (handler != null) handler(client, x, y, button);
            }
        }

        internal static void OnMouseClick(int client, int x, int y, MouseButtons button)
        {
            lock (myMouseClickLock)
            {
                dMouseClick handler = myMouseClickEvent;
                if (handler != null) handler(client, x, y, button);
            }
        }

        internal static void OnMouseDblClick(int client, int x, int y, MouseButtons button)
        {
            lock (myMouseDblClickLock)
            {
                dMouseDblClick handler = myMouseDblClickEvent;
                if (handler != null) handler(client, x, y, button);
            }
        }

        internal static void OnMouseWheel(int client, int x, int y, sbyte delta)
        {
            lock (myMouseWheelLock)
            {
                dMouseWheel handler = myMouseWheelEvent;
                if (handler != null) handler(client, x, y, delta);
            }
        }

        internal static void OnKeyDown(int client, Keys key)
        {
            lock (myKeyDownLock)
            {
                dKeyDown handler = myKeyDownEvent;
                if (handler != null) handler(client, key);
            }
        }

        internal static void OnKeyUp(int client, Keys key)
        {
            lock (myKeyUpLock)
            {
                dKeyUp handler = myKeyUpEvent;
                if (handler != null) handler(client, key);
            }
        }

    }
}