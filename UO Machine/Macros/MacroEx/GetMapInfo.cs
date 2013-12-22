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
using UOMachine;
using UOMachine.Data;

namespace UOMachine.Macros
{
    public static partial class MacroEx
    {
        /// <summary>
        /// Get land tile and static tiles from specified map.
        /// </summary>
        /// <param name="facet">Target map.</param>
        /// <returns>True on success.</returns>
        public static bool GetMapInfo(Facet facet, int x, int y, out MapInfo mapInfo)
        {
            switch (facet)
            {
                case Facet.Felucca:
                    return Map.GetInfo(0, x, y, out mapInfo);
                case Facet.Trammel:
                    return Map.GetInfo(1, x, y, out mapInfo);
                case Facet.Ilshenar:
                    return Map.GetInfo(2, x, y, out mapInfo);
                case Facet.Malas:
                    return Map.GetInfo(3, x, y, out mapInfo);
                case Facet.Tokuno:
                    return Map.GetInfo(4, x, y, out mapInfo);
                case Facet.Ter_Mur:
                    return Map.GetInfo(5, x, y, out mapInfo);
            }
            mapInfo = null;
            return false;
        }
    }
}