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

using UOMachine.Utility;
using System;
using System.Threading;

namespace UOMachine
{
    public sealed class Item : WorldItem
    {
        public readonly int ContainerSerial = 0;

        public Item(int serial) : base(serial) { }
        public Item(int serial, int containerSerial)
            : base(serial)
        {
            this.ContainerSerial = containerSerial;
        }

        public bool IsContainer
        {
            get { return this.Container != null; }
        }

        private ItemCollection myContainer = null;
        public ItemCollection Container
        {
            get { return ThreadHelper.VolatileRead<ItemCollection>(ref myContainer); }
            internal set { myContainer = value; }
        }

        private int myOwner = 0;
        public int Owner
        {
            get { return Thread.VolatileRead(ref myOwner); }
            internal set { myOwner = value; }
        }

        private int myGrid = 0;
        public int Grid
        {
            get { return Thread.VolatileRead(ref myGrid); }
            internal set { myGrid = value; }
        }

        private int myCount = 0;
        public int Count
        {
            get { return Thread.VolatileRead(ref myCount); }
            internal set { myCount = value; }
        }

        private Layer myLayer = 0;
        public Layer Layer
        {
            get { return ThreadHelper.VolatileRead<Layer>(ref myLayer); }
            internal set { myLayer = value; }
        }

    }
}