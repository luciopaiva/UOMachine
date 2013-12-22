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
using System.Text;

namespace UOMachine.Macros
{
    internal static class Assembler
    {

        private static byte[] myFunc1Bytes = new byte[] { 
            0xE8, 0xFF, 0xFF, 0xFF, 0xFF, 0x68, 0xFF, 0xFF, 
            0xFF, 0xFF, 0x66, 0xA1, 0xFF, 0xFF, 0xFF, 0xFF, 
            0x66, 0x83, 0xF8, 0x00, 0x75, 0x01, 0xC3, 0x66, 
            0x31, 0xC0, 0x66, 0xA3, 0xFF, 0xFF, 0xFF, 0xFF, 
            0x90, 0x90, 0x90, 0x90, 0x90 };

        private static byte[] myFunc2Bytes = new byte[] { 
            0x68, 0x00, 0x00, 0x00, 0x00, 0x68, 0xFF, 0xFF,
            0xFF, 0xFF, 0x68, 0xFF, 0xFF, 0xFF, 0xFF, 0x68, 
            0xFF, 0xFF, 0xFF, 0xFF, 0xC3, 0x83, 0xC4, 0x08, 
            0xC3 };

        private static byte[] myData1Bytes = new byte[] { 
            0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x19, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0xFF, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 
            0xFF, 0xFF, 0xFF, 0xFF };

        private const int func1offset = 2;
        private const int func2offset = 34;
        private const int data1offset = 114;
        private const int data2offset = 258;

        private static byte[] emptyArray = new byte[256];

        public static void PreparePrimaryCave(ClientInfo ci)
        {

            ci.PrimaryCave = new byte[] { 
            0x00, 0x00, 0xE8, 0xFF, 0xFF, 0xFF, 0xFF, 0x68, 
            0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0xA1, 0xFF, 0xFF,
            0xFF, 0xFF, 0x66, 0x83, 0xF8, 0x00, 0x75, 0x01,
            0xC3, 0x66, 0x31, 0xC0, 0x66, 0xA3, 0xFF, 0xFF,
            0xFF, 0xFF, 0x68, 0x00, 0x00, 0x00, 0x00, 0x68,
            0xFF, 0xFF, 0xFF, 0xFF, 0x68, 0xFF, 0xFF, 0xFF,
            0xFF, 0x68, 0xFF, 0xFF, 0xFF, 0xFF, 0xC3, 0x83,
            0xC4, 0x08, 0xC3, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x19, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0x00,
            0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            ci.PrimaryCave[0] = 0;
            int startWordAddress = ci.CaveAddress.ToInt32();
            byte[] hookBytes = ClientHook.CreateCALL(ci.HookAddress, (IntPtr)(startWordAddress + func1offset), CallType.CALL);
            byte[] origDest = BitConverter.GetBytes(ci.OriginalDest);
            byte[] serverSendCaveCall = ClientHook.CreateCALL(startWordAddress + func1offset, ci.ServerSendCaveAddress.ToInt32() + 6, CallType.CALL);
            byte[] startAddress = BitConverter.GetBytes(startWordAddress);
            byte[] data1Address = BitConverter.GetBytes(startWordAddress + data1offset);
            byte[] returnAddress = BitConverter.GetBytes(ci.ReturnAddress);
            byte[] addressForFunc2 = BitConverter.GetBytes(startWordAddress + func2offset + 21);

            Buffer.BlockCopy(serverSendCaveCall, 0, ci.PrimaryCave, 2, 5);
            Buffer.BlockCopy(origDest, 0, ci.PrimaryCave, 8, 4);
            Buffer.BlockCopy(startAddress, 0, ci.PrimaryCave, 14, 4);
            Buffer.BlockCopy(startAddress, 0, ci.PrimaryCave, 30, 4);
            Buffer.BlockCopy(data1Address, 0, ci.PrimaryCave, 40, 4);
            Buffer.BlockCopy(addressForFunc2, 0, ci.PrimaryCave, 45, 4);
            Buffer.BlockCopy(returnAddress, 0, ci.PrimaryCave, 50, 4);

            Memory.Write(ci.Handle, ci.CaveAddress, ci.PrimaryCave, true);
            Memory.Write(ci.Handle, ci.HookAddress, hookBytes, true);
            ci.PrimaryCave[0] = 1;

        }

        public static void Execute(ClientInfo ci, byte param1, byte param2)
        {
            ci.PrimaryCave[data1offset + 24] = param1;
            ci.PrimaryCave[data1offset + 28] = param2;
            Memory.Write(ci.Handle, ci.CaveAddress, ci.PrimaryCave, true);
        }

        public static void Execute(ClientInfo ci, byte param1, byte param2a, byte param2b)
        {
            ci.PrimaryCave[data1offset + 24] = param1;
            ci.PrimaryCave[data1offset + 28] = param2b;
            ci.PrimaryCave[data1offset + 29] = param2a;
            Memory.Write(ci.Handle, ci.CaveAddress, ci.PrimaryCave, true);
        }

        public static void Execute(ClientInfo ci, byte param1, byte param2, string param3)
        {
            ci.PrimaryCave[data1offset + 24] = param1;
            ci.PrimaryCave[data1offset + 28] = param2;
            byte[] data2Address = BitConverter.GetBytes(ci.CaveAddress.ToInt32() + data2offset);
            Buffer.BlockCopy(data2Address, 0, ci.PrimaryCave, data1offset + 32, 4);
            byte[] unicodeBytes = UnicodeEncoding.Unicode.GetBytes(param3);

            if (unicodeBytes.Length > 256) Buffer.BlockCopy(unicodeBytes, 0, ci.PrimaryCave, data2offset, 256);
            else Buffer.BlockCopy(unicodeBytes, 0, ci.PrimaryCave, data2offset, unicodeBytes.Length);
            Memory.Write(ci.Handle, ci.CaveAddress, ci.PrimaryCave, true);
            Buffer.BlockCopy(emptyArray, 0, ci.PrimaryCave, data2offset, 256);  // clear previous data
        }

        public static void PrepareGumpFunctionCave(ClientInfo ci)
        {
            /* 00400911   FFFF             ???                             ; \
             * 00400913   FFFF             ???                             ; / parent gump address
             * 00400915   FFFF             ???                             ; \
             * 00400917   FFFF             ???                             ; / function index to call
             * 00400919   0000             ADD BYTE PTR DS:[EAX],AL        ; nonzero = execute
             * 0040091B   66:A1 19094000   MOV AX,WORD PTR DS:[400919]
             * 00400921   66:83F8 00       CMP AX,0
             * 00400925   75 06            JNZ SHORT uoml_win.0040092D
             * 00400927   90               NOP                             ; reserved for future
             * 00400928   90               NOP                             ; reserved for future
             * 00400929   90               NOP                             ; reserved for future
             * 0040092A   90               NOP                             ; reserved for future
             * 0040092B   90               NOP                             ; reserved for future
             * 0040092C   C3               RETN
             * 0040092D   66:33C0          XOR AX,AX
             * 00400930   66:A3 19094000   MOV WORD PTR DS:[400919],AX
             * 00400936   8B0D 11094000    MOV ECX,DWORD PTR DS:[400911]
             * 0040093C   A1 15094000      MOV EAX,DWORD PTR DS:[400915]
             * 00400941   BA 04000000      MOV EDX,4
             * 00400946   F7E2             MUL EDX
             * 00400948   8B11             MOV EDX,DWORD PTR DS:[ECX]
             * 0040094A   03C2             ADD EAX,EDX
             * 0040094C   8B10             MOV EDX,DWORD PTR DS:[EAX]
             * 0040094E   FFD2             CALL EDX
             * 00400950   C3               RETN                           */

            byte[] cave = new byte[] { 
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 
                0x00, 0x00, 0x66, 0xA1, 0xFF, 0xFF, 0xFF, 0xFF, 
                0x66, 0x83, 0xF8, 0x00, 0x75, 0x06, 0x90, 0x90, 
                0x90, 0x90, 0x90, 0xC3, 0x66, 0x33, 0xC0, 0x66, 
                0xA3, 0xFF, 0xFF, 0xFF, 0xFF, 0x8B, 0x0D, 0xFF, 
                0xFF, 0xFF, 0xFF, 0xA1, 0xFF, 0xFF, 0xFF, 0xFF, 
                0xBA, 0x04, 0x00, 0x00, 0x00, 0xF7, 0xE2, 0x8B, 
                0x11, 0x03, 0xC2, 0x8B, 0x10, 0xFF, 0xD2, 0xC3};

            int caveAddress = ci.GumpFunctionCaveAddress.ToInt32();
            byte[] startWord = BitConverter.GetBytes(caveAddress + 8);
            byte[] gumpOffset = BitConverter.GetBytes(caveAddress);
            byte[] functionOffset = BitConverter.GetBytes(caveAddress + 4);

            Buffer.BlockCopy(startWord, 0, cave, 12, 4);
            Buffer.BlockCopy(startWord, 0, cave, 33, 4);
            Buffer.BlockCopy(gumpOffset, 0, cave, 39, 4);
            Buffer.BlockCopy(functionOffset, 0, cave, 44, 4);

            Memory.Write(ci.Handle, ci.GumpFunctionCaveAddress, cave, true);
        }

        public static void PreparePathfindCave(ClientInfo ci)
        {
            byte[] cave;
            int caveAddress = ci.PathFindCaveAddress.ToInt32();
            byte[] startWord = BitConverter.GetBytes(caveAddress + 6);
            byte[] EAXDword = BitConverter.GetBytes(caveAddress - 0x24);
            byte[] gumpFunction;
            byte[] RETNDword;
            byte[] pathFindJMP;
            switch (ci.PathFindType)
            {
                case 0:
                    cave = new byte[] { 
                        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 
                        0x66, 0xA1, 0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x83, 
                        0xF8, 0x00, 0x75, 0x06, 0x90, 0x90, 0x90, 0x90, 
                        0x90, 0xC3, 0x66, 0x33, 0xC0, 0x66, 0xA3, 0xFF, 
                        0xFF, 0xFF, 0xFF, 0x6A, 0x00, 0x6A, 0x00, 0x68, 
                        0xFF, 0xFF, 0xFF, 0xFF, 0x51, 0x53, 0x55, 0x56, 
                        0x57, 0xB8, 0xFF, 0xFF, 0xFF, 0xFF, 0xE9, 0xFF, 
                        0xFF, 0xFF, 0xFF, 0xC3 };
                    gumpFunction = ClientHook.CreateCALL(caveAddress + 20, ci.GumpFunctionCaveAddress.ToInt32() + 10, CallType.CALL);
                    pathFindJMP = ClientHook.CreateCALL(caveAddress + 54, ci.PathFindFunction, CallType.JMP);
                    RETNDword = BitConverter.GetBytes(caveAddress + cave.Length - 1);
                    Buffer.BlockCopy(startWord, 0, cave, 10, 4);
                    Buffer.BlockCopy(startWord, 0, cave, 31, 4);
                    Buffer.BlockCopy(gumpFunction, 0, cave, 20, 5);
                    Buffer.BlockCopy(RETNDword, 0, cave, 40, 4);
                    Buffer.BlockCopy(EAXDword, 0, cave, 50, 4);
                    Buffer.BlockCopy(pathFindJMP, 0, cave, 54, 5);
                    Memory.Write(ci.Handle, ci.PathFindCaveAddress, cave, true);
                    break;

                case 1:

                    /* 004008D1   FFFF             ???                         ; X
                     * 004008D3   FFFF             ???                         ; Y
                     * 004008D5   FFFF             ???                         ; Z
                     * 004008D7   0000             ADD BYTE PTR DS:[EAX],AL    ; nonzero = execute
                     * 004008D9   66:A1 D7084000   MOV AX,WORD PTR DS:[4008D7]
                     * 004008DF   66:83F8 00       CMP AX,0
                     * 004008E3   75 06            JNZ SHORT client.004008EB
                     * 004008E5   90               NOP                         ; reserved for future
                     * 004008E6   90               NOP                         ; reserved for future
                     * 004008E7   90               NOP                         ; reserved for future
                     * 004008E8   90               NOP                         ; reserved for future
                     * 004008E9   90               NOP                         ; reserved for future
                     * 004008EA   C3               RETN
                     * 004008EB   66:33C0          XOR AX,AX
                     * 004008EE   66:A3 D7084000   MOV WORD PTR DS:[4008D7],AX
                     * 004008F4   6A 00            PUSH 0
                     * 004008F6   6A 00            PUSH 0
                     * 004008F8   68 08094000      PUSH client.00400908        ; RETN from pathfind
                     * 004008FD   51               PUSH ECX
                     * 004008FE   B8 AD084000      MOV EAX,client.004008AD     ; EAX + 0x24 = X word
                     * 00400903  -E9 51980900      JMP client.0049A159
                     * 00400908   C3               RETN                        */

                    cave = new byte[] { 
                        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 
                        0x66, 0xA1, 0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x83, 
                        0xF8, 0x00, 0x75, 0x06, 0x90, 0x90, 0x90, 0x90, 
                        0x90, 0xC3, 0x66, 0x33, 0xC0, 0x66, 0xA3, 0xFF, 
                        0xFF, 0xFF, 0xFF, 0x6A, 0x00, 0x6A, 0x00, 0x68, 
                        0xFF, 0xFF, 0xFF, 0xFF, 0x51, 0xB8, 0xFF, 0xFF, 
                        0xFF, 0xFF, 0xE9, 0xFF, 0xFF, 0xFF, 0xFF, 0xC3 };
                    gumpFunction = ClientHook.CreateCALL(caveAddress + 20, ci.GumpFunctionCaveAddress.ToInt32() + 10, CallType.CALL);
                    pathFindJMP = ClientHook.CreateCALL(caveAddress + 50, ci.PathFindFunction, CallType.JMP);
                    RETNDword = BitConverter.GetBytes(caveAddress + cave.Length - 1);
                    Buffer.BlockCopy(startWord, 0, cave, 10, 4);
                    Buffer.BlockCopy(startWord, 0, cave, 31, 4);
                    Buffer.BlockCopy(RETNDword, 0, cave, 40, 4);
                    Buffer.BlockCopy(EAXDword, 0, cave, 46, 4);
                    Buffer.BlockCopy(pathFindJMP, 0, cave, 50, 5);
                    Buffer.BlockCopy(gumpFunction, 0, cave, 20, 5);
                    Memory.Write(ci.Handle, ci.PathFindCaveAddress, cave, true);
                    break;
            }
        }

        public static void PrepareServerSendCave(ClientInfo ci)
        {
            byte[] cave = new byte[] { 
                0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x66, 0xA1, 
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x83, 0xF8, 0x00, 
                0x75, 0x06, 0xE8, 0xFF, 0xFF, 0xFF, 0xFF, 0xC3, 
                0x66, 0x33, 0xC0, 0x66, 0xA3, 0xFF, 0xFF, 0xFF, 
                0xFF, 0x53, 0x54, 0x55, 0x56, 0x57, 0xA1, 0xFF, 
                0xFF, 0xFF, 0xFF, 0x50, 0x8B, 0x0D, 0xFF, 0xFF, 
                0xFF, 0xFF, 0x8B, 0xD9, 0xE8, 0xFF, 0xFF, 0xFF, 
                0xFF, 0x5F, 0x5E, 0x5D, 0x5C, 0x5B, 0xC3 };

            int caveAddress = ci.ServerSendCaveAddress.ToInt32();

            byte[] bufferDword = BitConverter.GetBytes(caveAddress);
            byte[] startWord = BitConverter.GetBytes(caveAddress + 4);
            byte[] packetSendCALL = ClientHook.CreateCALL((IntPtr)(caveAddress + 52), (IntPtr)ci.ServerPacketSendFunction, CallType.CALL);
            byte[] clientSendCaveCall = ClientHook.CreateCALL((IntPtr)(caveAddress + 18), (IntPtr)(ci.ClientSendCaveAddress.ToInt32() + 10), CallType.CALL);
            byte[] data = BitConverter.GetBytes(ci.ClientPacketData);

            Buffer.BlockCopy(startWord, 0, cave, 8, 4);
            Buffer.BlockCopy(startWord, 0, cave, 29, 4);
            Buffer.BlockCopy(bufferDword, 0, cave, 39, 4);
            Buffer.BlockCopy(data, 0, cave, 46, 4);
            Buffer.BlockCopy(packetSendCALL, 0, cave, 52, 5);
            Buffer.BlockCopy(clientSendCaveCall, 0, cave, 18, 5);

            Memory.Write(ci.Handle, ci.ServerSendCaveAddress, cave, true);
        }

        public static void PrepareClientSendCave(ClientInfo ci)
        {

            byte[] cave = new byte[] { 
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0x00, 0x00, 0x66, 0xA1, 0xFF, 0xFF, 0xFF, 0xFF, 
                0x66, 0x83, 0xF8, 0x00, 0x75, 0x06, 0x90, 0x90,
                0x90, 0x90, 0x90, 0xC3, 0x66, 0x33, 0xC0, 0x66, 
                0xA3, 0xFF, 0xFF, 0xFF, 0xFF, 0x51, 0x52, 0x53, 
                0x54, 0x55, 0x56, 0x57, 0x8B, 0x2D, 0xFF, 0xFF, 
                0xFF, 0xFF, 0x8B, 0x3D, 0xFF, 0xFF, 0xFF, 0xFF, 
                0x55, 0x8B, 0x0D, 0xFF, 0xFF, 0xFF, 0xFF, 0x8B, 
                0xF1, 0xE8, 0xFF, 0xFF, 0xFF, 0xFF, 0x5F, 0x5E, 
                0x5D, 0x5C, 0x5B, 0x5A, 0x59, 0xC3 };

            int caveAddress = ci.ClientSendCaveAddress.ToInt32();

            byte[] bufferDword = BitConverter.GetBytes(caveAddress);
            byte[] lenDword = BitConverter.GetBytes(caveAddress + 4);
            byte[] startWord = BitConverter.GetBytes(caveAddress + 8);
            byte[] packetRecvCALL = ClientHook.CreateCALL((IntPtr)(caveAddress + 65), ci.RecvHookAddress, CallType.CALL);
            byte[] pathFindCaveCALL = ClientHook.CreateCALL(caveAddress + 22, ci.PathFindCaveAddress.ToInt32() + 8, CallType.CALL);
            byte[] data = BitConverter.GetBytes(ci.ClientPacketData);

            Buffer.BlockCopy(startWord, 0, cave, 12, 4);
            Buffer.BlockCopy(pathFindCaveCALL, 0, cave, 22, 5);
            Buffer.BlockCopy(startWord, 0, cave, 33, 4);
            Buffer.BlockCopy(bufferDword, 0, cave, 46, 4);
            Buffer.BlockCopy(lenDword, 0, cave, 52, 4);
            Buffer.BlockCopy(data, 0, cave, 59, 4);
            Buffer.BlockCopy(packetRecvCALL, 0, cave, 65, 5);

            Memory.Write(ci.Handle, ci.ClientSendCaveAddress, cave, true);
        }

    }
}
