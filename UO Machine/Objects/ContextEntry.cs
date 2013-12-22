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
    public sealed class ContextEntry
    {
        public readonly int client;
        public readonly int entry;
        public readonly int serial;
        public readonly string text;
        public readonly int flags;
        public readonly int hue;
        public bool IsEnabled { get { return !((flags & 1) == 1); } }
        public bool IsColored { get { return (flags & 0x20) == 0x20; } }

        public ContextEntry(int client, int entry, int serial, string text, int flags, int hue)
        {
            this.client = client;
            this.entry = entry;
            this.serial = serial;
            this.text = text;
            this.flags = flags;
            this.hue = hue;
        }

        public void Click()
        {
            Macros.MacroEx.ContextMenuClick(this.client, this.serial, this.entry);
        }
    }
}