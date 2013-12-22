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
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace UOMachine
{
    internal static class AssemblyHelper
    {

        private static bool GetDotNet20Folder(out string folder)
        {
            folder = Environment.ExpandEnvironmentVariables(@"%windir%\Microsoft.NET\Framework\v2.0.50727");
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                return true;
            }
            return false;
        }

        private static bool GetDotNet20RegistryFolder(out string folder)
        {
            folder = "";
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Microsoft\ASP.NET\2.0.50727.0", false);
            if (rk == null) rk = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\ASP.NET\2.0.50727.0", false);
            else
            {
                folder = (string)rk.GetValue("Path");
                if (!string.IsNullOrEmpty(folder)) return true;
            }
            if (rk == null) rk = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Microsoft\MSBuild\ToolsVersions\2.0", false);
            else
            {
                folder = (string)rk.GetValue("Path");
                if (!string.IsNullOrEmpty(folder)) return true;
            }
            if (rk == null) rk = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\MSBuild\ToolsVersions\2.0", false);
            else
            {
                folder = (string)rk.GetValue("MSBuildToolsPath");
                if (!string.IsNullOrEmpty(folder)) return true;
            }
            if (rk == null)
            {
                return GetDotNet20Folder(out folder);
            }
            else
            {
                folder = (string)rk.GetValue("MSBuildToolsPath");
                if (!string.IsNullOrEmpty(folder)) return true;
            }
            return false;
        }

        private static bool GetDotNet30Folder(out string folder)
        {
            folder = Environment.ExpandEnvironmentVariables(@"%programfiles%\Reference Assemblies\Microsoft\Framework\v3.0");
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                return true;
            }
            return false;
        }

        private static bool GetDotNet30RegistryFolder(out string folder)
        {
            folder = "";
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework\AssemblyFolders\v3.0", false);
            if (rk == null)
            {
                return GetDotNet30Folder(out folder);
            }
            folder = (string)rk.GetValue("All Assemblies In");
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                return true;
            }
            return false;
        }

        private static bool GetDotNet35Folder(out string folder)
        {
            folder = Environment.ExpandEnvironmentVariables(@"%programfiles%\Reference Assemblies\Microsoft\Framework\v3.5");
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                return true;
            }
            return false;
        }

        private static bool GetDotNet35RegistryFolder(out string folder)
        {
            folder = "";
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework\AssemblyFolders\v3.5", false);
            if (rk == null)
            {
                return GetDotNet30Folder(out folder);
            }
            folder = (string)rk.GetValue("All Assemblies In");
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                return true;
            }
            return false;
        }

        private static void LoadFiles(string[] files, ObservableCollection<AssemblyPicker.AssemblyData> assemblyDataCollection)
        {
            AssemblyPicker.AssemblyData data;
            foreach (string file in files)
            {
                if (AssemblyParser.IsAssembly(file))
                {
                    data = new AssemblyPicker.AssemblyData();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(file);
                    data.assembly = Path.GetFileName(file);
                    data.version = fvi.ProductVersion;
                    data.path = file;
                    assemblyDataCollection.Add(data);
                }
            }
        }

        public static void GetAllAssemblies(ObservableCollection<AssemblyPicker.AssemblyData> assemblyDataCollection)
        {
            string path;
            string[] files;
            if (GetDotNet20RegistryFolder(out path))
            {
                files = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);
                LoadFiles(files, assemblyDataCollection);
            }
            if (GetDotNet30RegistryFolder(out path))
            {
                files = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);
                LoadFiles(files, assemblyDataCollection);
            }
            if (GetDotNet35RegistryFolder(out path))
            {
                files = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);
                LoadFiles(files, assemblyDataCollection);
            }
        }
    }
}