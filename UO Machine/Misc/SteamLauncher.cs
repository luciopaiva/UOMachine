/* Copyright (C) 2014 John Scott
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

namespace UOMachine.Misc
{
    internal static class SteamLauncher
    {
        private static object myLock = new object();

        public static bool Launch(OptionsData options, out int index)
        {
            /* Could all seem a bit excessive just to get the pid of the client it creates? */
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = @"C:\Program Files (x86)\UOSteam\"; // to be fixed
            startInfo.FileName = @"C:\Program Files (x86)\UOSteam\UOSteam.exe"; // to be fixed
            index = -1;
            Win32.SafeProcessHandle hProcess;
            Win32.SafeThreadHandle hThread;
            uint pid, tid;
            int uopid = 0;
            UOM.SetStatusLabel("Status : Launching UOSteam");
            if (Win32.CreateProcess(startInfo, true, out hProcess, out hThread, out pid, out tid))
            {
                /* Basically rewrites a small piece of code before the client resumes to write the pid of the new client it creates, probably not portable between versions if the registers change, what can i say, I'm a noob
                 * Never had code of old code before RazorLauncher to see how xenoglyph did it before.
                 * Code will need to be cleaned up. */

                Process p = Process.GetProcessById((int)pid);
                IntPtr codeAddress;

                /* Entry point varies, so need to use GetThreadContext to find entrypoint */
                Win32.CONTEXT cntx = new Win32.CONTEXT();
                cntx.ContextFlags = (int)Win32.CONTEXT_FLAGS.CONTEXT_FULL;
                Win32.GetThreadContext(hThread.DangerousGetHandle(), ref cntx);

                /* Ebx+8 = pointer to entrypoint to be read with ReadProcessMemory. */
                byte[] tmp = new byte[4];
                Memory.Read(hProcess.DangerousGetHandle(), (IntPtr)(cntx.Ebx + 8), tmp, true);

                int baseAddress = tmp[3] << 24 | tmp[2] << 16 | tmp[1] << 8 | tmp[0];
                byte[] buffer = new byte[0x17000];
                Memory.Read(hProcess.DangerousGetHandle(), (IntPtr)(baseAddress + 0x1000), buffer, true);

                UOM.SetStatusLabel("Status : Patching UOSteam");

                /* Originally pushes hThread and hProcess on the stack before a call, we'll overwrite here. */
                byte[] findBytes = new byte[] { 0x8B, 0x44, 0x24, 0x44, // MOV EAX, [ESP+44]
                                                0x8B, 0x4C, 0x24, 0x40  // MOV ECX, [ESP+40]
                };

                int offset = 0;
                if (FindSignatureOffset(findBytes, buffer, out offset))
                {
                    if ((codeAddress = Win32.VirtualAllocEx(hProcess.DangerousGetHandle(), IntPtr.Zero, 1024, Win32.AllocationType.Commit, Win32.MemoryProtection.ExecuteReadWrite)) == IntPtr.Zero)
                    {
                        UOM.SetStatusLabel("Status : Memory Allocation failed");
                        hProcess.Dispose();
                        hThread.Dispose();
                        return false;
                    }

                    byte[] patchCode1 = new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00,  // MOV EAX, <codeAddress> 
                                                 0x8B, 0x4C, 0x24, 0x4C,        // MOV ECX, [ESP+4C]
                                                 0x89, 0x08,                    // MOV [EAX], ECX
                                                 0xB8, 0x00, 0x00, 0x00, 0x00,  // MOV EAX, <codeAddress+4>
                                                 0x8B, 0x4C, 0x24, 0x44,        // MOV ECX, [ESP+4C]
                                                 0x89, 0x08,                    // MOV [EAX], ECX
                                                 0x8B, 0x44, 0x24, 0x48,        // MOV EAX, [ESP+48] -.
                                                 0x8B, 0x4C, 0x24, 0x44,        // MOV ECX, [ESP+44] -| Original code we overwrote, but esp incremented 4 due to call return address on stack
                                                 0xC3                           // RETN
                    };

                    patchCode1[1] = (byte)codeAddress.ToInt32();
                    patchCode1[2] = (byte)(codeAddress.ToInt32() >> 8);
                    patchCode1[3] = (byte)(codeAddress.ToInt32() >> 16);
                    patchCode1[4] = (byte)(codeAddress.ToInt32() >> 24);

                    int codeAddress2 = (codeAddress.ToInt32() + 4);

                    patchCode1[12] = (byte)codeAddress2;
                    patchCode1[13] = (byte)(codeAddress2 >> 8);
                    patchCode1[14] = (byte)(codeAddress2 >> 16);
                    patchCode1[15] = (byte)(codeAddress2 >> 24);


                    int patchAddress = codeAddress.ToInt32() + 8;

                    Memory.Write(hProcess.DangerousGetHandle(), (IntPtr)patchAddress, patchCode1, true);

                    byte[] patchCode2 = new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, // MOV EAX, <patchAddress>
                                                   0xFF, 0xD0,                   // CALL EAX
                                                   0x90                          // NOP
                    };

                    patchCode2[1] = (byte)patchAddress;
                    patchCode2[2] = (byte)(patchAddress >> 8);
                    patchCode2[3] = (byte)(patchAddress >> 16);
                    patchCode2[4] = (byte)(patchAddress >> 24);

                    Memory.Write(hProcess.DangerousGetHandle(), (IntPtr)((baseAddress + 0x1000) + offset), patchCode2, true);

                    if (Win32.ResumeThread(hThread.DangerousGetHandle()) == -1)
                    {
                        UOM.SetStatusLabel("Status : ResumeThread failed");
                        hProcess.Dispose();
                        hThread.Dispose();
                        return false;
                    }


                    // This is dodgy, to be changed to something else.
                    byte[] uopidbytes = new byte[4];
                    do
                    {
                        Memory.Read(hProcess.DangerousGetHandle(), (IntPtr)codeAddress, uopidbytes, true);
                        uopid = uopidbytes[3] << 24 | uopidbytes[2] << 16 | uopidbytes[1] << 8 | uopidbytes[0];
                    } while (uopid == 0);

                }

                hProcess.Close();
                hThread.Close();
                return ClientLauncher.Attach((uint)uopid, options, true, out index);
            }
            UOM.SetStatusLabel("Status : Process creation failed");
            return false;


        }

        private static bool FindSignatureOffset(byte[] signature, byte[] buffer, out int offset)
        {
            bool found = false;
            offset = 0;
            for (int x = 0; x < buffer.Length - signature.Length; x++)
            {
                for (int y = 0; y < signature.Length; y++)
                {
                    if (buffer[x + y] == signature[y])
                        found = true;
                    else
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    offset = x;
                    break;
                }
            }
            return found;
        }

    }
}
