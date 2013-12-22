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
using System.Text;

namespace UOMachine.Data
{
    public sealed class MapInfo
    {
        public LandTile landTile;
        public StaticTile[] staticTiles;

        /// <summary>
        /// Find StaticTile with matching ID.
        /// </summary>
        /// <returns>True on success</returns>
        public bool FindTile(int ID, out StaticTile staticTile)
        {
            if (staticTiles != null)
            {
                foreach (StaticTile s in staticTiles)
                {
                    if (s.ID == ID)
                    {
                        staticTile = s;
                        return true;
                    }
                }
            }
            staticTile = null;
            return false;
        }

        /// <summary>
        /// Find StaticTile which matches any of the given IDs.
        /// </summary>
        /// <param name="ID">List of IDs to search for.</param>
        /// <returns>True on success.</returns>
        public bool FindTile(int[] ID, out StaticTile staticTile)
        {
            if (staticTiles != null && ID != null)
            {
                foreach (StaticTile s in staticTiles)
                {
                    foreach (int i in ID)
                    {
                        if (i == s.ID)
                        {
                            staticTile = s;
                            return true;
                        }
                    }
                }
            }
            staticTile = null;
            return false;
        }

        /// <summary>
        /// Returns true if any tiles in this MapInfo are passable.
        /// </summary>
        public bool IsPassable()
        {
            if (this.landTile != null && (this.landTile.Flags & TileFlags.Impassable) == 0)
                return true;
            if (this.staticTiles != null)
                foreach (StaticTile s in this.staticTiles)
                    if ((s.Flags & TileFlags.Impassable) == 0) return true;
            return false;
        }

        /// <summary>
        /// Returns true if all tiles at the specified Z height are passable.
        /// </summary>
        public bool IsPassable(int z)
        {
            if (this.landTile != null && this.landTile.Z == z && (this.landTile.Flags & TileFlags.Impassable) != 0)
                return false;
            if (this.staticTiles != null)
                foreach (StaticTile s in this.staticTiles)
                    if (s.Z == z && (s.Flags & TileFlags.Impassable) != 0) return false;
            return true;
        }

        public override int GetHashCode()
        {
            if (this.landTile != null)
                return (this.landTile.X * 8192) + (this.landTile.Y * 4096) + this.landTile.Z;
            if (this.staticTiles != null)
                return (this.staticTiles[0].X * 8192) + (this.staticTiles[0].Y * 4096) + this.staticTiles[0].Z;
            return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IsPassable = " + this.IsPassable());
            if (this.landTile != null)
            {
                sb.AppendLine("\r\nLand tile:");
                sb.AppendFormat("Location = {0}, {1}, {2}\r\n", this.landTile.X, this.landTile.Y, this.landTile.Z);
                sb.AppendLine("ID = " + this.landTile.ID);
                sb.AppendLine("Name = " + this.landTile.Name);
                sb.AppendLine("Flags = " + this.landTile.Flags);
            }
            if (this.staticTiles != null)
            {
                foreach (StaticTile s in this.staticTiles)
                {
                    sb.AppendLine("\r\nStatic tile:");
                    sb.AppendFormat("Location = {0}, {1} , {2}\r\n", s.X, s.Y, s.Z);
                    sb.AppendLine("ID = " + s.ID);
                    sb.AppendLine("Name = " + s.Name);
                    sb.AppendLine("Flags = " + s.Flags);
                    sb.AppendLine("Quantity = " + s.Quantity);
                    sb.AppendLine("Weight = " + s.Weight);
                }
            }
            return sb.ToString();
        }
    }
}