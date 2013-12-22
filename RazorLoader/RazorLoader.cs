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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Reflection;
using System.IO;

namespace RazorLoader
{
    class RazorLoader
    {
        private static int myPID = -1, myPort = -1;
        private static string myServer = "", myRazorFolder = "";
        private static bool myPatchClient = true, myServerEncrypted = false;

        private enum Switches
        {
            None, Path, Server, ServerEnc, ClientEnc, PID
        }

        private static void ParseArgs(string[] args)
        {
            Switches lastArg = Switches.None;
            StringBuilder pathBuilder = new StringBuilder();
            foreach (string s in args)
            {
                switch (lastArg)
                {
                    case Switches.None:
                        break;
                    case Switches.Path:
                        break;
                    case Switches.Server:
                        string[] serverSplit = s.Split(',');
                        if (serverSplit.Length > 1)
                        {
                            myServer = serverSplit[0].Trim();
                            Int32.TryParse(serverSplit[1].Trim(), out myPort);
                        }
                        lastArg = Switches.None;
                        continue;
                    case Switches.ServerEnc:
                        myServerEncrypted = true;
                        lastArg = Switches.None;
                        break;
                    case Switches.ClientEnc:
                        myPatchClient = false;
                        lastArg = Switches.None;
                        break;
                    case Switches.PID:
                        Int32.TryParse(s.Trim(), out myPID);
                        lastArg = Switches.None;
                        continue;
                }
                switch (s)
                {
                    case "--path":
                        lastArg = Switches.Path;
                        break;
                    case "--server":
                        lastArg = Switches.Server;
                        break;
                    case "--serverenc":
                        lastArg = Switches.ServerEnc;
                        break;
                    case "--clientenc":
                        lastArg = Switches.ClientEnc;
                        break;
                    case "--pid":
                        lastArg = Switches.PID;
                        break;
                    default:
                        if (lastArg == Switches.Path)
                        {
                            pathBuilder.Append(" " + s);
                        }
                        break;
                }
            }
            myRazorFolder = pathBuilder.ToString().Trim().Trim(Path.GetInvalidPathChars());
        }

        private static Exception GetException(Exception x)
        {
            if (x.InnerException != null)
                return (GetException(x.InnerException));
            return x;
        }

        static void Main(string[] args)
        {
            string fileVersion = "8.8.8";
            try
            {
                fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).FileVersion;
            }
            catch { }
            Console.WriteLine("RazorLoader " + fileVersion);
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: RazorLoader.exe --path <folder> --server <hostname>,<port> --serverenc --clientenc --pid <pid>");
                Console.WriteLine("Example: RazorLoader.exe --path \"C:\\Program Files (x86)\\Razor\" --server localhost,2593 --serverenc --clientenc --pid 888");
                Console.WriteLine("--serverenc means server is encrypted. Default is to patch encryption.");
                Console.WriteLine("--clientenc means client is encrypted. Default is to patch encryption.");
                Console.WriteLine();
                return;
            }
            ParseArgs(args);
            if (myPID == -1)
            {
                Console.WriteLine("--pid is required for this version of RazorLoader and is invalid or missing, now exiting.");
                Console.WriteLine();
                return;
            }
            else if (string.IsNullOrEmpty(myRazorFolder))
            {
                Console.WriteLine("--path is required, now exiting.");
                Console.WriteLine();
                return;
            }
            else if (string.IsNullOrEmpty(myServer) || myPort == -1)
            {
                Console.WriteLine("--server is missing or invalid, now exiting.");
                Console.WriteLine();
                return;
            }

            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
            {
                AssemblyName requestedName = new AssemblyName(e.Name);
                if (requestedName.Name == "Ultima")
                {
                    try
                    {
                        return Assembly.LoadFrom(Path.Combine(myRazorFolder, "Ultima.dll"));
                    }
                    catch (Exception ex)
                    {
                        Exception x = GetException(ex);
                        Console.WriteLine(x.Message);
                        Console.WriteLine(x.StackTrace);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            };

            try
            {
                Razor.Launch(myRazorFolder, myPatchClient, myServerEncrypted, myServer, myPort, myPID);
            }
            catch (Exception ex)
            {
                Exception x = GetException(ex);
                Console.WriteLine(x.Message);
                Console.WriteLine(x.StackTrace);
            }
        }
    }
}
