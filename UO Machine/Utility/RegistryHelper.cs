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
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace UOMachine.Utility
{
    public static class RegistryHelper
    {
        public static string GetRazorPath()
        {
            string path = "";
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"Software\Razor", false);
            if (rk == null)
                rk = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Razor", false);
            if (rk != null)
            {
                path = (string)rk.GetValue("InstallDir");
                rk.Close();
            }
            return path;
        }

        public static string GetUOPath()
        {
            string path = "";
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"Software\Origin Worlds Online\Ultima Online\1.0", false);
            if (rk==null)
                rk = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Origin Worlds Online\Ultima Online\1.0", false);
            if (rk != null)
            {
                path = (string)rk.GetValue("InstCDPath");
                rk.Close();
            }
            return path;
        }
    }
}