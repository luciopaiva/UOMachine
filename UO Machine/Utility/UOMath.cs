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

namespace UOMachine.Utility
{
    public static class UOMath
    {
        /// <summary>
        /// Calculate the direction from one map location to another.
        /// </summary>
        /// <returns>Direction.Invalid on failure.</returns>
        public static Direction MapDirection(float x1, float y1, float x2, float y2)
        {
            float slope = (y2 - y1) / (x2 - x1);
            if (slope == float.NaN) return Direction.Invalid;

            if ((slope >= 2f && slope <= float.PositiveInfinity) ||
                (slope <= -2f && slope >= float.NegativeInfinity))
            {
                if (y2 < y1) return Direction.North;
                return Direction.South;
            }

            if ((slope <= .5f && slope >= 0f) || (slope >= -.5f && slope <= 0f))
            {
                if (x2 < x1) return Direction.West;
                return Direction.East;
            }

            if ((slope <= -.5f && slope >= -1f) || (slope >= -2f && slope <= -1f))
            {
                if (y2 < y1) return Direction.Northeast;
                return Direction.Southwest;
            }

            if ((slope <= 2f && slope >= 1) || (slope >= .5f && slope <= 1))
            {
                if (y2 < y1) return Direction.Northwest;
                return Direction.Southeast;
            }

            return Direction.Invalid;
        }

        /// <summary>
        /// Calculate the distance in tiles between two points.
        /// </summary>
        public static double Distance(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            int d1 = Math.Abs(x1 - x2);
            int d2 = Math.Abs(y1 - y2);
            int d3 = Math.Abs(z1 - z2);
            return Math.Sqrt(d1 * d1 + d2 * d2 + d3 * d3);
        }

        /// <summary>
        /// Calculate the distance in tiles between two points.
        /// </summary>
        public static double Distance(int x1, int y1, int x2, int y2)
        {
            int d1 = Math.Abs(x1 - x2);
            int d2 = Math.Abs(y1 - y2);
            return Math.Sqrt(d1 * d1 + d2 * d2);
        }

        public static bool IsMobile(int serial)
        {
            return ((uint)serial < 0x40000000);
        }
    }
}