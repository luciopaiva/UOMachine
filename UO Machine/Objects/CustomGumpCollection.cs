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
using System.Threading;
using UOMachine.Tree;
using UOMachine.Utility;

namespace UOMachine
{
    public sealed class GenericGumpCollection
    {
        private const int myDefaultCapacity = 8;
        private object mySyncRoot;
        private Dictionary<int, Gump> myGumpList;
        public delegate void dCollectionChanged(int newCount);
        public event dCollectionChanged CollectionChangedEvent;

        public GenericGumpCollection() : this(myDefaultCapacity) { }

        public GenericGumpCollection(int capacity)
        {
            mySyncRoot = this;
            myGumpList = new Dictionary<int, Gump>(capacity);
        }

        private void OnCollectionChanged()
        {
            dCollectionChanged handler = CollectionChangedEvent;
            if (handler != null)
            {
                ThreadPool.QueueUserWorkItem(delegate { handler(myGumpList.Count); });
            }
        }

        public void Add(Gump gump)
        {
            lock (mySyncRoot)
            {
                if (!myGumpList.ContainsKey(gump.ID))
                {
                    myGumpList.Add(gump.ID, gump);
                    OnCollectionChanged();
                }
            }
        }

        public bool Remove(int ID)
        {
            lock (mySyncRoot)
            {
                if (myGumpList.ContainsKey(ID))
                {
                    if (myGumpList.Remove(ID))
                    {
                        OnCollectionChanged();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool GetGump(int ID, out Gump gump)
        {
            lock (mySyncRoot)
            {
                return myGumpList.TryGetValue(ID, out gump);
            }
        }

        public bool FindGump(int serial, out Gump gump)
        {
            lock (mySyncRoot)
            {
                foreach (KeyValuePair<int, Gump> kvp in myGumpList)
                {
                    if (kvp.Value.Serial == serial)
                    {
                        gump = kvp.Value;
                        return true;
                    }
                }
            }
            gump = null;
            return false;
        }

        public bool GetGumps(out Gump[] gumps)
        {
            lock (mySyncRoot)
            {
                if (myGumpList.Values.Count > 0)
                {
                    gumps = new Gump[myGumpList.Values.Count];
                    myGumpList.Values.CopyTo(gumps, 0);
                    return true;
                }
            }
            gumps = null;
            return false;
        }
    }
}