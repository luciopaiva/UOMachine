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
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Reflection;
//using System.Windows.Forms;
using System.Windows;
using System.IO;
using System.Net;
using UOMachine.IPC;

namespace UOMachine
{
    internal static class RazorLauncher
    {
        public static bool Launch(OptionsData options, out int index)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = MainWindow.CurrentOptions.UOFolder;
            startInfo.FileName = MainWindow.CurrentOptions.UOClientPath;
            Win32.SafeProcessHandle hProcess;
            Win32.SafeThreadHandle hThread;
            uint pid, tid;
            UOM.SetStatusLabel("Status : Launching client");
            index = -1;
            if (Win32.CreateProcess(startInfo, true, out hProcess, out hThread, out pid, out tid))
            {
                UOM.SetStatusLabel("Status : Patching client");

                if (!ClientPatcher.MultiPatch(hProcess.DangerousGetHandle()))
                {
                    UOM.SetStatusLabel("Status : MultiUO patch failed");
                    hProcess.Dispose();
                    hThread.Dispose();
                    return false;
                }

                if (Win32.ResumeThread(hThread.DangerousGetHandle()) == -1)
                {
                    UOM.SetStatusLabel("Status : ResumeThread failed");
                    hProcess.Dispose();
                    hThread.Dispose();
                    return false;
                }

                hProcess.Close();
                hThread.Close();
                startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = options.RazorFolder;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = Path.Combine(UOM.StartupPath, "RazorLoader.exe");
                string args = "--server " + options.Server + "," + options.Port.ToString();
                args += " --path " + options.RazorFolder;
                if (!options.PatchClientEncryption)
                    args += " --clientenc";
                if (options.EncryptedServer)
                    args += " --serverenc";
                args += " --pid " + pid.ToString();
                startInfo.Arguments = args;
                Process p = new Process();
                p.StartInfo = startInfo;
                p.Start();

                if (ClientLauncher.Attach(pid, options, true, out index))
                {
                    UOM.SetStatusLabel("Status : Razor successfully launched");
                    return true;
                }
                else
                {
                    UOM.SetStatusLabel("Status : Failed to attach to Razor client");
                    MessageBox.Show("Error attaching to Razor client.", "Error");
                }
            }
            return false;
        }
    }
}