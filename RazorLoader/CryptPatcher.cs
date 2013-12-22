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

namespace RazorLoader
{
    internal static unsafe class CryptPatcher
    {
        private static bool FindSignatureOffset(byte[] signature, byte* buffer, int bufLen, out int offset)
        {
            bool found = false;
            offset = 0;
            for (int x = 0; x < bufLen - signature.Length; x++)
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

        private static bool Patch1(byte* hModule, int len)
        {

            byte[] sig = new byte[] { 0x81, 0xEC, 0x00, 0x01, 0x00, 0x00, 0x53, 0x56, 0x8B, 0xB4, 0x24 };
            int offset;

            if (FindSignatureOffset(sig, hModule, len, out offset))
            {
                int oldAddress = hModule[0x23] << 24 | hModule[0x22] << 16 | hModule[0x21] << 8 | hModule[0x20];
                oldAddress += 1;
                hModule[offset + 0x1E] = 0xE9;
                hModule[offset + 0x1E + 1] = (byte)(oldAddress & 0xFF);
                hModule[offset + 0x1E + 2] = (byte)((oldAddress >> 8) & 0xFF);
                hModule[offset + 0x1E + 3] = (byte)((oldAddress >> 16) & 0xFF);
                hModule[offset + 0x1E + 4] = (byte)((oldAddress >> 24) & 0xFF);
                hModule[offset + 0x1E + 5] = 0x90;
                return true;
            }
            return false;
        }

        private static bool Patch2(byte* hModule, int len)
        {
            byte[] sig = new byte[] { 0x83, 0xC2, 0x09, 0x6A, 0x28, 0x52 };
            int offset;

            if (FindSignatureOffset(sig, hModule, len, out offset))
            {
                hModule[offset + 0x18] = 0x83;
                hModule[offset + 0x19] = 0xF0;
                hModule[offset + 0x1A] = 0x01;
                return true;
            }
            return false;
        }

        private static bool Patch3(byte* hModule, int len)
        {
            byte[] sig1 = new byte[] { 0x83, 0xEC, 0x28, 0x53, 0x57, 0x33, 0xDB };
            byte[] sig2 = new byte[] { 0x83, 0xEC, 0x28, 0x53, 0x57, 0x68 };
            int offset;

            if (FindSignatureOffset(sig1, hModule, len, out offset))
            {
                hModule[offset + 0x10] = 0xEB;
                return true;
            }

            if (FindSignatureOffset(sig2, hModule, len, out offset))
            {
                hModule[offset + 0x1C] = 0xEB; // change JZ to JMP
                return true;
            }
            return false;
        }

        public static bool Patch(IntPtr hModule)
        {
            byte* address = (byte*)hModule;
//            if (!Patch1(address, 0x10000)) return false;
//            if (!Patch2(address, 0x10000)) return false;
//            if (!Patch3(address, 0x10000)) return false;

            // Reximis 2012-02-11, Razor 1.0.13
/*
.text:100046D6                 cmp     byte_1002002F, bl
.text:100046DC                 jz      short loc_100046E7 <--------- CHANGE TO JMP
.text:100046DE                 pop     ebp
.text:100046DF                 lea     eax, [ebx+5]
.text:100046E2                 pop     ebx
.text:100046E3                 add     esp, 28h
.text:100046E6                 retn
 */
            byte[] sig = new byte[] { 0x38, 0x1D, 0x2F, 0x00 };
            int offset = 0;

            if (FindSignatureOffset(sig, address, 0x10000, out offset))
            {
                address[offset + 0x6] = 0xEB;
            }
            return true;
        }
    }
}