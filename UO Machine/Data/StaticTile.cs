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

namespace UOMachine.Data
{
    public sealed class StaticTile
    {
        public TileFlags Flags { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Quantity { get; set; }
        public int Weight { get; set; }

        //public int AnimID { get; set; }

        internal StaticTile(InternalStaticTile staticTile)
        {
            this.Flags = staticTile.Flags;
            this.Weight = staticTile.Weight;
            this.Quantity = staticTile.Quantity;
            this.ID = staticTile.ID;
            //this.AnimID = staticTile.AnimID;
            this.Name = staticTile.Name;
        }
    }
}