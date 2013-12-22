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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace UOMachine
{
    public delegate void KeyPressCallback();

    public sealed class HotKeyList
    {
        private sealed class HotKeyInfo
        {
            public readonly List<Keys> hotKeys;
            private KeyPressCallback callback;

            public HotKeyInfo(Keys[] hotkeys, KeyPressCallback callback)
            {
                this.hotKeys = new List<Keys>(hotkeys);
                this.callback = callback;
            }

            public void Fire()
            {
                if (this.callback != null)
                    ThreadPool.QueueUserWorkItem(delegate { this.callback(); });
            }
        }

        private List<HotKeyInfo> myHotKeyList;
        private List<Keys> myDownKeyList;

        public HotKeyList()
        {
            myHotKeyList = new List<HotKeyInfo>(32);
            myDownKeyList = new List<Keys>(32);
        }

        public HotKeyList(int capacity)
        {
            myHotKeyList = new List<HotKeyInfo>(capacity);
            myDownKeyList = new List<Keys>(32);
        }

        public void ClearKeys()
        {
            lock (myHotKeyList)
            {
                myHotKeyList.Clear();
            }
        }

        public bool RemoveKeys(Keys[] keys)
        {
            lock (myHotKeyList)
            {
                bool found = false;
                for (int x = myHotKeyList.Count - 1; x >= 0; x--)
                {
                    foreach (Keys k in keys)
                    {
                        if (myHotKeyList[x].hotKeys.Contains(k))
                        {
                            found = true;
                        }
                        else
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        myHotKeyList.RemoveAt(x);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AddKeys(Keys[] keys, KeyPressCallback callback)
        {
            if (keys == null || keys.Length == 0) return false;
            lock (myHotKeyList)
            {
                HotKeyInfo hki = new HotKeyInfo(keys, callback);
                if (!myHotKeyList.Contains(hki))
                {
                    myHotKeyList.Add(hki);
                    return true;
                }
            }
            return false;
        }

        public void KeyDown(Keys key)
        {
            lock (myDownKeyList)
            {
                if (!myDownKeyList.Contains(key))
                {
                    myDownKeyList.Add(key);
                }
                CheckKeyStates();
                CheckKeys();
            }
        }

        public void KeyUp(Keys key)
        {
            lock (myDownKeyList)
            {
                myDownKeyList.Remove(key);
            }
        }

        /// <summary>
        /// Makes sure keys we think are down are actually down.  Must be called from myDownKeyList lock.
        /// </summary>
        private void CheckKeyStates()
        {
            ushort keyState;
            for (int x = myDownKeyList.Count - 1; x >= 0; x--)
            {
                if (myDownKeyList[x] == Keys.Alt) keyState = Win32.GetAsyncKeyState(Keys.Menu);
                else keyState = Win32.GetAsyncKeyState(myDownKeyList[x]);
                if ((keyState & 0x8000) == 0) //key is up
                {
                    myDownKeyList.RemoveAt(x);
                }
            }
        }

        /// <summary>
        /// Checks pressed keys against stored hotkeys and fires as needed.  Must be called from myDownKeyList lock.
        /// </summary>
        private void CheckKeys()
        {
            lock (myHotKeyList)
            {
                bool found = false;
                foreach (HotKeyInfo hki in myHotKeyList)
                {
                    if (hki.hotKeys.Count != myDownKeyList.Count) continue;
                    foreach (Keys k in myDownKeyList)
                    {
                        if (hki.hotKeys.Contains(k))
                        {
                            found = true;
                        }
                        else
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        hki.Fire();
                    }
                }
            }
        }


    }
}