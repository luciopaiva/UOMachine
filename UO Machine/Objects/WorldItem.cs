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
    public abstract class WorldItem
    {
        public override int GetHashCode() { return this.Serial; }
        public WorldItem(int serial) { this.Serial = serial; }

        //public readonly int Serial;
        internal int mySerial = 0;
        public virtual int Serial
        {
            get { return Thread.VolatileRead(ref mySerial); }
            internal set { mySerial = value; }
        }

        internal int myArtDataID = -1;
        public virtual int ArtDataID
        {
            get { return Thread.VolatileRead(ref myArtDataID); }
            internal set { myArtDataID = value; }
        }

        internal int myLight = -1;
        public virtual int Light
        {
            get { return Thread.VolatileRead(ref myLight); }
            internal set { myLight = value; }
        }

        internal int myID = 0;
        public virtual int ID
        {
            get { return Thread.VolatileRead(ref myID); }
            internal set { myID = value; }
        }

        internal int myDirection = 0;
        public virtual int Direction
        {
            get { return Thread.VolatileRead(ref myDirection); }
            internal set { myDirection = value; }
        }

        internal int myX = 0;
        public virtual int X
        {
            get { return Thread.VolatileRead(ref myX); }
            internal set { myX = value; }
        }

        internal int myY = 0;
        public virtual int Y
        {
            get { return Thread.VolatileRead(ref myY); }
            internal set { myY = value; }
        }

        internal int myZ = 0;
        public virtual int Z
        {
            get { return Thread.VolatileRead(ref myZ); }
            internal set { myZ = value; }
        }

        internal int myFlags = 0;
        public int Flags
        {
            get { return Thread.VolatileRead(ref myFlags); }
            internal set { myFlags = value; }
        }

        internal int myHue = 0;
        public int Hue
        {
            get { return Thread.VolatileRead(ref myHue); }
            internal set { myHue = value; }
        }

        internal string myName = "";
        public virtual string Name
        {
            get { return ThreadHelper.VolatileRead(ref myName); }
            internal set { myName = value; }
        }

        internal string myLabel = "";
        public virtual string Label
        {
            get { return ThreadHelper.VolatileRead(ref myLabel); }
            internal set { myLabel = value; }
        }

        internal string myPropertyText = "";
        public virtual string PropertyText
        {
            get { return ThreadHelper.VolatileRead(ref myPropertyText); }
            internal set { myPropertyText = value; }
        }

        internal Property[] myProperties = null;
        public virtual Property[] Properties
        {
            get { return ThreadHelper.VolatileRead(ref myProperties); }
            internal set { myProperties = value; }
        }

    }
}