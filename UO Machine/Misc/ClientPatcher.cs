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

/* Thanks to Daniel 'Necr0Potenc3' Cavalcanti for the encryption removal method */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using UOMachine.IPC;
using EasyHook;

namespace UOMachine
{
    public static class ClientPatcher
    {
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

        private static bool TripleCheckPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            /* Patches following checks:
             * "Another instance of UO may already be running."
             * "Another instance of UO is already running."
             * "An instance of UO Patch is already running." */

            byte[] oldClientSig = new byte[] { 0xFF, 0xD6, 0x6A, 0x01, 0xFF, 0xD7, 0x68 };
            byte[] newClientSig = new byte[] { 0x3B, 0xC3, 0x89, 0x44, 0x24, 0x08 };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                if (fileBuffer[offset - 0x2D] == 0x75 && fileBuffer[offset - 0x0E] == 0x75 && fileBuffer[offset + 0x18] == 0x74)
                {
                    byte[] EB = new byte[] { 0xEB };
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset - 0x2D)), EB, true);
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset - 0x0E)), EB, true);
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x18)), EB, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                if (fileBuffer[offset + 0x06] == 0x75 && fileBuffer[offset + 0x2D] == 0x75 && fileBuffer[offset + 0x5F] == 0x74)
                {
                    byte[] EB = new byte[] { 0xEB };
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x06)), EB, true);
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x2D)), EB, true);
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x5F)), EB, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private static bool SingleCheckPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            /* Patches the following check:
             * "Another copy of UO is already running!" */

            byte[] oldClientSig = new byte[] { 0xC7, 0x44, 0x24, 0x10, 0x11, 0x01, 0x00, 0x00 };
            byte[] newClientSig = new byte[] { 0x83, 0xC4, 0x04, 0x33, 0xDB, 0x53, 0x50 };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                if (fileBuffer[offset + 0x17] == 0x74)
                {
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x17)), new byte[] { 0xEB }, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                if (fileBuffer[offset + 0x0F] == 0x74)
                {
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x0F)), new byte[] { 0xEB }, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }


        private static bool ErrorCheckPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            /* Patches the following check:
             * GetLastError returns non-zero */

            byte[] oldClientSig = new byte[] { 0x85, 0xC0, 0x75, 0x2F, 0xBF };
            byte[] newClientSig = new byte[] { 0x85, 0xC0, 0x5F, 0x5E, 0x75, 0x2F };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                //XOR AX, AX
                byte[] patch = new byte[] { 0x66, 0x33, 0xC0, 0x90 };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset)), patch, true);
                return true;
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0x90, 0x90 };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x04)), patch, true);
                return true;
            }

            return false;
        }

        private static bool LoginCryptPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            byte[] oldClientSig = new byte[] { 0x81, 0xF9, 0x00, 0x00, 0x01, 0x00, 0x0F, 0x8F };
            byte[] newClientSig = new byte[] { 0x75, 0x12, 0x8B, 0x54, 0x24, 0x0C };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0x84 };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x15)), patch, true);
                return true;
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0xEB };
                Memory.Write(hProcess, (IntPtr)(baseAddress + offset), patch, true);
                return true;
            }
            return false;
        }

        private static bool TwoFishCryptPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            byte[] oldClientSig = new byte[] { 0x8B, 0xD9, 0x8B, 0xC8, 0x48, 0x85, 0xC9, 0x0F, 0x84 };
            byte[] newClientSig = new byte[] { 0x74, 0x0F, 0x83, 0xB9, 0xB4, 0x00, 0x00, 0x00, 0x00 };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0x85 };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x08)), patch, true);
                return true;
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0xEB };
                Memory.Write(hProcess, (IntPtr)(baseAddress + offset), patch, true);
                return true;
            }
            return false;
        }

        private static bool DecryptPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            byte[] oldClientSig = new byte[] { 0x8B, 0x86, 0x04, 0x01, 0x0A, 0x00, 0x85, 0xC0, 0x74, 0x52 };
            byte[] newClientSig = new byte[] { 0x74, 0x37, 0x83, 0xBE, 0xB4, 0x00, 0x00, 0x00, 0x00 };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0x3B };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x06)), patch, true);
                return true;
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0xEB };
                Memory.Write(hProcess, (IntPtr)(baseAddress + offset), patch, true);
                return true;
            }
            return false;
        }

        private static bool StaminaPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            byte[] oldClientSig = new byte[] { 0x8B, 0x91, 0x10, 0x02, 0x00, 0x00, 0x8B, 0x81 };
            byte[] newClientSig = new byte[] { 0x8B, 0x91, 0x1C, 0x02, 0x00, 0x00, 0x3B, 0x91 };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0xC0 };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x0D)), patch, true);
                return true;
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0xEB };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x0C)), patch, true);
                return true;
            }
            return false;
        }

        private static bool LightPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            byte[] oldClientSig = new byte[] { 0x25, 0xFF, 0x00, 0x00, 0x00, 0x83, 0xC4 };
            byte[] newClientSig = new byte[] { 0x8A, 0x4C, 0x24, 0x0F, 0x0F, 0xB6, 0xC1, 0x83, 0xC4 };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0xEB };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x0A)), patch, true);
                return true;
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                byte[] patch = new byte[] { 0xEB };
                Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x10)), patch, true);
                return true;
            }
            return false;
        }

        private static bool NewMultiCheckPatch(byte[] fileBuffer, IntPtr hProcess, long baseAddress)
        {
            //Credit: Reximus 02/2010

            /*  005DB3EE   83C4 04          ADD ESP,4
                005DB3F1   33ED             XOR EBP,EBP
                005DB3F3   55               PUSH EBP
                005DB3F4   50               PUSH EAX
                005DB3F5   FF15 18846500    CALL DWORD PTR DS:[<&USER32.FindWindowA>>; USER32.FindWindowA
                005DB3FB   85C0             TEST EAX,EAX
                005DB3FD   8B1D A0846500    MOV EBX,DWORD PTR DS:[<&USER32.MessageBo>; USER32.MessageBoxA
                005DB403   74 1C            JE SHORT client-7.005DB421
                005DB405   6A 04            PUSH 4
                005DB407   68 20CC6700      PUSH client-7.0067CC20                   ; ASCII "Multiple Instances Running"
                005DB40C   68 70CB6700      PUSH client-7.0067CB70                   ; ASCII "Running two instances of Ultima Online at once is an UNSUPPORTED feature. This will not work correctly on some systems, and is not supported by Mythic Tech Support. Proceed?"
                005DB411   55               PUSH EBP
                005DB412   FFD3             CALL EBX
                005DB414   83F8 07          CMP EAX,7
                005DB417   75 08            JNZ SHORT client-7.005DB421
            */

            byte[] clientSig = new byte[] { 0x83, 0xC4, 0x04, 0x33, 0xED, 0x55, 0x50 };
            int offset;

            if (FindSignatureOffset(clientSig, fileBuffer, out offset))
            {
                byte[] EB = new byte[] { 0xEB };
                if (fileBuffer[offset + 0x15] == 0x74)
                {
                    Memory.Write(hProcess, (IntPtr)(baseAddress + (offset + 0x15)), EB, true);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Patch suspended process to allow multiple simultaneous clients.
        /// </summary>
        public static bool MultiPatch(IntPtr hProcess)
        {
            long baseAddress = 0x0400000;
            byte[] buffer = new byte[4194304];
            if (!Memory.Read(hProcess, (IntPtr)baseAddress, buffer, false)) return false;
            if (!NewMultiCheckPatch(buffer, hProcess, baseAddress))
            {
                if (!TripleCheckPatch(buffer, hProcess, baseAddress)) return false;
                if (!SingleCheckPatch(buffer, hProcess, baseAddress)) return false;
                if (!ErrorCheckPatch(buffer, hProcess, baseAddress)) return false;
            }
            return true;
        }

        /// <summary>
        /// Patch encryption to allow connection to servers which don't support it.
        /// </summary>
        public static bool PatchEncryption(IntPtr hProcess)
        {
            long baseAddress = 0x0400000;
            byte[] buffer = new byte[4194304];
            if (!Memory.Read(hProcess, (IntPtr)baseAddress, buffer, false)) return false;
            if (!LoginCryptPatch(buffer, hProcess, baseAddress)) return false;
            if (!TwoFishCryptPatch(buffer, hProcess, baseAddress)) return false;
            if (!DecryptPatch(buffer, hProcess, baseAddress)) return false;
            return true;
        }

        /// <summary>
        /// Patch stamina check in Felucca when trying to walk over mobiles.
        /// </summary>
        public static bool PatchStaminaCheck(IntPtr hProcess)
        {
            long baseAddress = 0x0400000;
            byte[] buffer = new byte[4194304];
            if (!Memory.Read(hProcess, (IntPtr)baseAddress, buffer, false)) return false;
            if (!StaminaPatch(buffer, hProcess, baseAddress)) return false;
            return true;
        }

        /// <summary>
        /// Always light patch.
        /// </summary>
        public static bool PatchLight(IntPtr hProcess)
        {
            long baseAddress = 0x0400000;
            byte[] buffer = new byte[4194304];
            if (!Memory.Read(hProcess, (IntPtr)baseAddress, buffer, false)) return false;
            if (!LightPatch(buffer, hProcess, baseAddress)) return false;
            return true;
        }
    }
}