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
using System.Windows;
using System.IO;
using System.Net;
using UOMachine.IPC;
using EasyHook;

namespace UOMachine
{
    internal static class ClientLauncher
    {
        private static object myLock = new object();

        public static bool Attach(uint pid, OptionsData options, bool isRazor, out int index)
        {
            lock (myLock)
            {
                Process p = null;
                index = -1;
                try
                {
                    Thread.Sleep(2000);
                    p = Process.GetProcessById((int)pid);
                    p.EnableRaisingEvents = true;
                    p.Exited += new EventHandler(UOM.OnClientExit);
                    ClientInfo ci = new ClientInfo(p);
                    Memory.MemoryInit(ci);
                    if (ci.IsValid) ci.InstallMacroHook();
                    else return false;

                    if (!isRazor && options.PatchClientEncryptionUOM)
                    {
                        if (!ClientPatcher.PatchEncryption(p.Handle))
                        {
                            MessageBox.Show("Error patching client encryption!", "Error");
                        }
                    }
                    if (ci.DateStamp < 0x4AA52CC4 && options.PatchStaminaCheck)
                    {
                        if (!ClientPatcher.PatchStaminaCheck(p.Handle))
                        {
                            MessageBox.Show("Error patching stamina check!", "Error");
                        }
                    }
                    if (options.PatchAlwaysLight)
                    {
                        if (!ClientPatcher.PatchLight(p.Handle))
                        {
                            MessageBox.Show("Error with always light patch!", "Error");
                        }
                    }
                    int instance;
                    if (!ClientInfoCollection.AddClient(ci, out instance))
                        throw new ApplicationException("Unknown error at ClientInfoCollection.Add.");
                    ci.Instance = instance;
                    index = instance;
                    Macros.Macro.ChangeServer(instance, options.Server, options.Port);
                    Thread.Sleep(500);
                    RemoteHooking.Inject(p.Id, Path.Combine(UOM.StartupPath, "clienthook.dll"), Path.Combine(UOM.StartupPath, "clienthook.dll"), UOM.ServerName);
                }
                catch (Exception e)
                {
                    Utility.Log.LogMessage(e);
                    MessageBox.Show("Error injecting ClientHook.dll into target process.  Please try again.", "Error");
                    try { if (p != null) p.Kill(); }
                    catch (Win32Exception) { }
                    catch (InvalidOperationException) { }
                    return false;
                }
                UOM.SetStatusLabel("Status: Attached to client");
                return true;
            }
        }

        public static bool Launch(OptionsData options, out int index)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = MainWindow.CurrentOptions.UOFolder;
            startInfo.FileName = MainWindow.CurrentOptions.UOClientPath;
            index = -1;
            Win32.SafeProcessHandle hProcess;
            Win32.SafeThreadHandle hThread;
            uint pid, tid;
            UOM.SetStatusLabel("Status : Launching client");
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
                return Attach(pid, options, false, out index);
            }
            UOM.SetStatusLabel("Status : Process creation failed");
            return false;
        }
    }
}