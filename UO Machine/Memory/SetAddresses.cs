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
using UOMachine.Utility;

namespace UOMachine
{
    internal static partial class Memory
    {
        private static bool FindSignatureOffset(byte[] signature, byte[] buffer, out int offset)
        {
            bool found = false;
            offset = 0;
            for (int x = 0; x < buffer.Length - signature.Length; x++)
            {
                for (int y = 0; y < signature.Length; y++)
                {
                    if (signature[y] == 0xCC || buffer[x + y] == signature[y])
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

        private static bool GetGumpPointer(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 0x8B, 0x44, 0x24, 0x04, 0xC7, 0x41, 0x5C, 0x00, 0x00, 0x00, 0x00 };
            byte[] sig2 = new byte[] { 0x8B, 0x44, 0x24, 0x04, 0x85, 0xC0, 0x89, 0x41, 0x64 };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                clientInfo.TopGumpPointer = (IntPtr)BitConverter.ToUInt32(buffer, offset + 0x14);
                return true;
            }

            if (FindSignatureOffset(sig2, buffer, out offset))
            {
                clientInfo.TopGumpPointer = (IntPtr)BitConverter.ToUInt32(buffer, offset + 0x14);
                return true;
            }

            return false;
        }

        private static bool GetPathFindAddress(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 0x0F, 0xBF, 0x68, 0x24, 0x0F, 0xBF, 0x50, 0x26 };
            byte[] sig2 = new byte[] { 0x0F, 0xBF, 0x50, 0x26, 0x53, 0x8B, 0x1D };

            int offset;
            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                clientInfo.PathFindFunction = baseAddress + offset;
                clientInfo.PathFindType = 0;
                return true;
            }

            if (FindSignatureOffset(sig2, buffer, out offset))
            {
                clientInfo.PathFindFunction = baseAddress + offset;
                clientInfo.PathFindType = 1;
                return true;
            }

            return false;
        }

        private static bool GetServerSendAddress(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 0x85, 0xC9, 0x74, 0x0A, 0x8B, 0x44, 0x24, 0x04, 0x50, 0xE8 };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                int address = BitConverter.ToInt32(buffer, offset + 10);
                clientInfo.ServerPacketSendFunction = baseAddress + offset + 14 + address;
                return true;
            }

            return false;
        }

        private static bool GetCursorAddress(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 0xD0, 0x00, 0x00, 0x00, 0x3B, 0xC7, 0x74, 0x38 };
            byte[] sig2 = new byte[] { 0x8B, 0xF1, 0x83, 0xBE, 0xFC, 0x00, 0x00, 0x00, 0x00 };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                clientInfo.CursorAddress = (IntPtr)BitConverter.ToInt32(buffer, offset + 9);
                return true;
            }

            if (FindSignatureOffset(sig2, buffer, out offset))
            {
                clientInfo.CursorAddress = (IntPtr)BitConverter.ToInt32(buffer, offset + 12);
                return true;
            }

            return false;
        }

        private static bool GetClientPacketData(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 
                0xA1, 0xCC, 0xCC, 0xCC, 0xCC, 0x8B, 0x0D, 0xCC, 
                0xCC, 0xCC, 0xCC, 0x8B, 0x16 };
            byte[] sig2 = new byte[] { 
                0xC7, 0x06, 0xCC, 0xCC, 0xCC, 0xCC, 0x89, 0x35, 
                0xCC, 0xCC, 0xCC, 0xCC, 0x8B, 0x4C, 0x24, 0x0C };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                clientInfo.ClientPacketData = BitConverter.ToInt32(buffer, offset + 7);
                return true;
            }

            if (FindSignatureOffset(sig2, buffer, out offset))
            {
                clientInfo.ClientPacketData = BitConverter.ToInt32(buffer, offset + 8);
                return true;
            }

            return false;
        }

        private static bool GetSendAddress(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 0x8D, 0x8B, 0x94, 0x00, 0x00, 0x00 };
            byte[] sig2 = new byte[] { 0x0F, 0xB7, 0xD8, 0x0F, 0xB6, 0x06, 0x83, 0xC4, 0x04 };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                clientInfo.SendHookAddress = (IntPtr)(baseAddress + offset + 11);
                clientInfo.SendHookType = 0;
                return true;
            }

            if (FindSignatureOffset(sig2, buffer, out offset))
            {
                clientInfo.SendHookAddress = (IntPtr)(baseAddress + offset + 9);
                clientInfo.SendHookType = 1;
                return true;
            }

            return false;
        }


        private static bool GetRecvAddress(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 0x53, 0x56, 0x57, 0x8B, 0xF9, 0x8B, 0x0D };
            byte[] sig2 = new byte[] { 0x8B, 0x38, 0x8B, 0xE9, 0xBE };
            byte[] sig3 = new byte[] { 0x8B, 0x38, 0x8B, 0xD9, 0xBE }; // Reximus 03/2011

            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                clientInfo.RecvHookAddress = (IntPtr)(baseAddress + offset);
                clientInfo.RecvHookType = 0;
                return true;
            }

            if (FindSignatureOffset(sig2, buffer, out offset))
            {
                clientInfo.RecvHookAddress = (IntPtr)(baseAddress + offset - 18);
                clientInfo.RecvHookType = 1;
                return true;
            }

            if (FindSignatureOffset(sig3, buffer, out offset))
            {
                clientInfo.RecvHookAddress = (IntPtr)(baseAddress + offset - 18);
                clientInfo.RecvHookType = 1;
                return true;
            }

            return false;
        }

        private static bool GetPlayerInfoPointer(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            int offset;
            byte[] sig1 = new byte[] { 0x0F, 0xBF, 0x4A, 0x28, 0x83, 0xC1, 0x05 };

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                clientInfo.PlayerInfoPointer = (IntPtr)BitConverter.ToUInt32(buffer, offset - 6);
                return true;
            }

            return false;
        }


        private static bool GetCaveAddress(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[3192]; //was 1024 (Reximus 02/2010)
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                uint caveAddress = (uint)baseAddress + (uint)offset + 1750; //was 750 (Reximus 02/2010)
                uint recvCaveAddress = caveAddress + 526;
                uint clientSendCaveAddress = recvCaveAddress + 28;
                uint sendCaveAddress = clientSendCaveAddress + 80;
                uint serverSendCaveAddress = sendCaveAddress + 26;
                uint pathFindCaveAddress = serverSendCaveAddress + 74;
                uint gumpFunctionCaveAddress = pathFindCaveAddress + 64;

                clientInfo.CaveAddress = (IntPtr)caveAddress;
                clientInfo.RecvCaveAddress = (IntPtr)recvCaveAddress;
                clientInfo.SendCaveAddress = (IntPtr)sendCaveAddress;
                clientInfo.ClientSendCaveAddress = (IntPtr)clientSendCaveAddress;
                clientInfo.ServerSendCaveAddress = (IntPtr)serverSendCaveAddress;
                clientInfo.PathFindCaveAddress = (IntPtr)pathFindCaveAddress;
                clientInfo.GumpFunctionCaveAddress = (IntPtr)gumpFunctionCaveAddress;
                return true;
            }

            return false;
        }

        private static bool GetReturnAddress1(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 
                0x6A, 0xFF, 0x68, 0xCC, 0xCC, 0xCC, 0xCC, 0x64, 
                0xA1, 0x00, 0x00, 0x00, 0x00, 0x50, 0xB8, 0x78, 
                0x30, 0x00, 0x00 };
            byte[] sig2 = new byte[] { 
                0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xCC, 0xCC, 
                0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x68, 0xCC, 
                0xCC, 0xCC, 0xCC, 0x50, 0xB8, 0xCC, 0xCC, 0xCC, 
                0xCC, 0x64, 0x89, 0x25, 0x00, 0x00, 0x00, 0x00, 
                0xE8, 0xCC, 0xCC, 0xCC, 0xCC, 0x8B, 0x84, 0x24 };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                clientInfo.ReturnAddress = baseAddress + offset;
                return true;
            }

            if (FindSignatureOffset(sig2, buffer, out offset))
            {
                clientInfo.ReturnAddress = baseAddress + offset + 6;
                return true;
            }

            return false;
        }

        private static bool GetHookAddress(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            byte[] sig1 = new byte[] { 0x53, 0x68, 0xE8, 0x03, 0x00, 0x00, 0x52 };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                //Credit: Reximus 02/2010
                //clientInfo.HookAddress = (IntPtr)(baseAddress + offset - 5);
                //clientInfo.OriginalDest = baseAddress + (offset - 4) + 4 + BitConverter.ToInt32(buffer, offset - 4);
                clientInfo.HookAddress = (IntPtr)(baseAddress + offset + 8);
                clientInfo.OriginalDest = baseAddress + (offset + 9) + 4 + BitConverter.ToInt32(buffer, offset + 9);
                return true;
            }

            return false;
        }

        private static bool GetLoginServerAddress(int baseAddress, byte[] buffer, ClientInfo clientInfo)
        {
            /* Here we're getting address for server IP as well as preventing client from overwriting our IP
             * we're also getting port address and protecting it as well */
            
            byte[] sig1 = new byte[] { 0x8B, 0x44, 0x24, 0x0C, 0xC1, 0xE1, 0x08, 0x0B, 0xC8, 0x66, 0x89, 0x15 };
            byte[] sig2 = new byte[] { 0xC1, 0xE1, 0x08, 0x0B, 0x4C, 0x24, 0x0C, 0x89, 0x0D };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                byte[] patch = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                byte[] portPatch = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                IntPtr portPatchAddress = (IntPtr)(baseAddress + offset + 0x09);
                IntPtr patchAddress = (IntPtr)(baseAddress + offset + 0x10);
                clientInfo.LoginServerAddress = (IntPtr)(BitConverter.ToInt32(buffer, offset + 0x12));
                clientInfo.LoginPortAddress = (IntPtr)(BitConverter.ToInt32(buffer, offset + 0x0C));
                if (Memory.Write(clientInfo.Handle, patchAddress, patch, true))
                    return Memory.Write(clientInfo.Handle, portPatchAddress, portPatch, true);
            }

            if (FindSignatureOffset(sig2, buffer, out offset))
            {
                byte[] patch = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                byte[] portPatch = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                IntPtr portPatchAddress = (IntPtr)(baseAddress + offset - 0x07);
                IntPtr patchAddress = (IntPtr)(baseAddress + offset + 0x07);
                clientInfo.LoginServerAddress = (IntPtr)(BitConverter.ToInt32(buffer, offset + 0x09));
                clientInfo.LoginPortAddress = (IntPtr)(BitConverter.ToInt32(buffer, offset - 0x04));
                if (Memory.Write(clientInfo.Handle, patchAddress, patch, true))
                    return Memory.Write(clientInfo.Handle, portPatchAddress, portPatch, true);
            }

            return false;
        }

        public static bool FindVersion(int baseAddress, byte[] buffer, out string output)
        {
            // 4.0.11c = 
            /* 00525970  /$ A0 FCF18700         MOV AL,BYTE PTR DS:[87F1FC]
             * 00525975  |. 84C0                TEST AL,AL
             * 00525977  |. 75 24               JNZ SHORT uoml_win.0052599D
             * 00525979  |. 68 94FC5D00         PUSH uoml_win.005DFC94                   ;  ASCII "c"
             * 0052597E  |. 6A 0B               PUSH 0B
             * 00525980  |. 6A 00               PUSH 0
             * 00525982  |. 6A 04               PUSH 4
             * 00525984  |. 68 88FC5D00         PUSH uoml_win.005DFC88                   ;  ASCII "%d.%d.%d%s"
             * 00525989  |. 68 D4F18700         PUSH uoml_win.0087F1D4
             * 0052598E  |. E8 5CAF0100         CALL uoml_win.005408EF
             * 00525993  |. 83C4 18             ADD ESP,18
             * 00525996  |. C605 FCF18700 01    MOV BYTE PTR DS:[87F1FC],1
             * 0052599D  |> B8 D4F18700         MOV EAX,uoml_win.0087F1D4
             * 005259A2  \. C3                  RETN */

            byte[] sig1 = new byte[] { 0x84, 0xC0, 0x75, 0x24, 0x68 };
            int offset;

            if (FindSignatureOffset(sig1, buffer, out offset))
            {
                int address = BitConverter.ToInt32(buffer, offset + 5);
                char letter = (char)buffer[address - baseAddress];
                int part3 = buffer[offset + 0x0A];
                int part2 = buffer[offset + 0x0C];
                int part1 = buffer[offset + 0x0E];
                output = part1 + "." + part2 + "." + part3 + letter;
                return true;
            }

            output = "";
            return false;
        }

        public static bool SetAddresses(ClientInfo clientInfo, string fileName)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                int baseAddress = clientInfo.BaseAddress.ToInt32();
                clientInfo.Version = ExeInfo.GetFileVersion(fileName);
                clientInfo.DateStamp = ExeInfo.GetStamp(fileBytes);
                if (clientInfo.Version == "0.0.0.0")
                {
                    clientInfo.Version = ExeInfo.DateToVersion(clientInfo.DateStamp);
                    if (clientInfo.Version == "")
                    {
                        string version;
                        if (!FindVersion(baseAddress, fileBytes, out version))
                        {
                            //return false;
                        }
                        clientInfo.Version = version;
                    }
                }

                if (!GetHookAddress(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetRecvAddress(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetSendAddress(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetReturnAddress1(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetCaveAddress(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetPlayerInfoPointer(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetClientPacketData(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetCursorAddress(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetServerSendAddress(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetPathFindAddress(baseAddress, fileBytes, clientInfo)) return false;
                if (!GetGumpPointer(baseAddress, fileBytes, clientInfo)) return false;
                //if (!GetLoginServerAddress(baseAddress, fileBytes, clientInfo)) return false;

                clientInfo.IsValid = true;
                return true;
            }
            catch { }
            return false;
        }
    }
}