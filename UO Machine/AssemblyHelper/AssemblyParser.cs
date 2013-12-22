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

/* This is loosely based on AssemblyParser by Mike Woodring */

using System;
using System.IO;

namespace UOMachine
{
    internal static class AssemblyParser
    {
        public static bool IsAssembly(string fileName)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fileName);
                // check for "MZ"
                if (!(fs.ReadByte() == 0x4D && fs.ReadByte() == 0x5A)) return false;
                fs.Position = 0x3C;
                int offset = fs.ReadByte() | fs.ReadByte() << 8 | fs.ReadByte() << 16 | fs.ReadByte() << 24;
                fs.Position = offset;
                // check for "PE\0\0"
                if (!(fs.ReadByte() == 0x50 && fs.ReadByte() == 0x45 && fs.ReadByte() == 0x00 && fs.ReadByte() == 0x00)) return false;
                // check machine code
                int machineCode = (fs.ReadByte() | fs.ReadByte() << 8);
                if (machineCode != 0x14C) return false;
                fs.Position = offset + 24 + 92;
                // check data dirs in pe optional header
                int dataDirs = fs.ReadByte() | fs.ReadByte() << 8 | fs.ReadByte() << 16 | fs.ReadByte() << 24;
                if (dataDirs != 16) return false;
                fs.Position = offset + 24 + 208;
                // check for CLI header
                int cliHeader = fs.ReadByte() | fs.ReadByte() << 8 | fs.ReadByte() << 16 | fs.ReadByte() << 24;;
                if (cliHeader == 0) return false;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
    }
}