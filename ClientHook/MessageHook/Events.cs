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

namespace ClientHook
{
    static unsafe partial class MessageHook
    {
        /*public delegate void dMouseMove(int x, int y);
        public delegate void dMouseDown(int x, int y, UOMachine.IPC.MouseButton button);
        public delegate void dMouseUp(int x, int y, UOMachine.IPC.MouseButton button);
        public delegate void dMouseClick(int x, int y, UOMachine.IPC.MouseButton button);
        public delegate void dMouseDoubleClick(int x, int y, UOMachine.IPC.MouseButton button);
        public delegate void dMouseWheel(int x, int y, sbyte delta);
        public delegate void dKeyDown(Keys key);
        public delegate void dKeyUp(Keys key);

        public static event dMouseDown MouseDownEvent;
        public static event dMouseUp MouseUpEvent;
        public static event dMouseMove MouseMoveEvent;
        public static event dMouseClick MouseClickEvent;
        public static event dMouseDoubleClick MouseDoubleClickEvent;
        public static event dMouseWheel MouseWheelEvent;
        public static event dKeyDown KeyDownEvent;
        public static event dKeyUp KeyUpEvent;*/


        public static void OnMouseDown(int x, int y, UOMachine.IPC.MouseButton button)
        {
            byte value;
            switch (button)
            {
                case UOMachine.IPC.MouseButton.Left:
                    value = 1;
                    break;
                case UOMachine.IPC.MouseButton.Middle:
                    value = 2;
                    break;
                case UOMachine.IPC.MouseButton.Right:
                    value = 3;
                    break;
                default:
                    value = 0;
                    break;
            }
            myClientInstance.SendCommand(UOMachine.IPC.Command.MouseDown, x, y, value);

            /*dMouseDown handler = MouseDownEvent;
            if (handler != null)
            {
                //ThreadPool.QueueUserWorkItem(delegate { handler(x, y, button); });
                handler(x, y, button);
            }*/
        }

        public static void OnMouseUp(int x, int y, UOMachine.IPC.MouseButton button)
        {
            byte value;
            switch (button)
            {
                case UOMachine.IPC.MouseButton.Left:
                    value = 1;
                    break;
                case UOMachine.IPC.MouseButton.Middle:
                    value = 2;
                    break;
                case UOMachine.IPC.MouseButton.Right:
                    value = 3;
                    break;
                default:
                    value = 0;
                    break;
            }
            myClientInstance.SendCommand(UOMachine.IPC.Command.MouseUp, x, y, value);

            /*dMouseUp handler = MouseUpEvent;
            if (handler != null)
            {
                //ThreadPool.QueueUserWorkItem(delegate { handler(x, y, button); });
                handler(x, y, button);
            }*/
        }

        public static void OnMouseMove(int x, int y)
        {
            myClientInstance.SendCommand(UOMachine.IPC.Command.MouseMove, x, y);

            /*dMouseMove handler = MouseMoveEvent;
            if (handler != null)
            {
                //ThreadPool.QueueUserWorkItem(delegate { handler(x, y); });
                handler(x, y);
            }*/
        }

        public static void OnMouseClick(int x, int y, UOMachine.IPC.MouseButton button)
        {
            byte value;
            switch (button)
            {
                case UOMachine.IPC.MouseButton.Left:
                    value = 1;
                    break;
                case UOMachine.IPC.MouseButton.Middle:
                    value = 2;
                    break;
                case UOMachine.IPC.MouseButton.Right:
                    value = 3;
                    break;
                default:
                    value = 0;
                    break;
            }
            myClientInstance.SendCommand(UOMachine.IPC.Command.MouseClick, x, y, value);

            /*dMouseClick handler = MouseClickEvent;
            if (handler != null)
            {
                //ThreadPool.QueueUserWorkItem(delegate { handler(x, y, button); });
                handler(x, y, button);
            }*/
        }

        public static void OnMouseDoubleClick(int x, int y, UOMachine.IPC.MouseButton button)
        {
            byte value;
            switch (button)
            {
                case UOMachine.IPC.MouseButton.Left:
                    value = 1;
                    break;
                case UOMachine.IPC.MouseButton.Middle:
                    value = 2;
                    break;
                case UOMachine.IPC.MouseButton.Right:
                    value = 3;
                    break;
                default:
                    value = 0;
                    break;
            }
            myClientInstance.SendCommand(UOMachine.IPC.Command.MouseDblClick, x, y, value);

            /*dMouseDoubleClick handler = MouseDoubleClickEvent;
            if (handler != null)
            {
                //ThreadPool.QueueUserWorkItem(delegate { handler(x, y, button); });
                handler(x, y, button);
            }*/
        }

        public static void OnMouseWheel(int x, int y, byte delta)
        {
            myClientInstance.SendCommand(UOMachine.IPC.Command.MouseWheel, x, y, delta);

            /*dMouseWheel handler = MouseWheelEvent;
            if (handler != null)
            {
                //ThreadPool.QueueUserWorkItem(delegate { handler(x, y, delta); });
                handler(x, y, delta);
            }*/
        }

        public static void OnKeyDown(Keys key)
        {
            myClientInstance.SendCommand(UOMachine.IPC.Command.KeyDown, (int)key);

            /*dKeyDown handler = KeyDownEvent;
            if (handler != null)
            {
                //ThreadPool.QueueUserWorkItem(delegate { handler(key); });
                handler(key);
            }*/
        }

        public static void OnKeyUp(Keys key)
        {
            myClientInstance.SendCommand(UOMachine.IPC.Command.KeyUp, (int)key);

            /*dKeyUp handler = KeyUpEvent;
            if (handler != null)
            {
                //ThreadPool.QueueUserWorkItem(delegate { handler(key); });
                handler(key);
            }*/
        }

    }
}