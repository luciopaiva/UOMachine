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
    [Flags]
    public enum TileFlags : uint
    {
        Background =  0x00000001,
        Weapon =      0x00000002,
        Transparent = 0x00000004,
        Translucent = 0x00000008,
        Wall =        0x00000010,
        Damaging =    0x00000020,
        Impassable =  0x00000040,
        Wet =         0x00000080,
        Unknown =     0x00000100,
        Surface =     0x00000200,
        Bridge =      0x00000400,
        Stackable =   0x00000800,
        Window =      0x00001000,
        NoShoot =     0x00002000,
        PrefixA =     0x00004000,
        PrefixAn =    0x00008000,
        Internal =    0x00010000,
        Foliage =     0x00020000,
        PartialHue =  0x00040000,
        Unknown1 =    0x00080000,
        Map =         0x00100000,
        Container =   0x00200000,
        Wearable =    0x00400000,
        LightSource = 0x00800000,
        Animated =    0x01000000,
        NoDiagonal =  0x02000000,
        Unknown2 =    0x04000000,
        Armor =       0x08000000,
        Roof =        0x10000000,
        Door =        0x20000000,
        StairBack =   0x40000000,
        StairRight =  0x80000000
    }
}