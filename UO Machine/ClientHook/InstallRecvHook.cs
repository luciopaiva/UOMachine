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

namespace UOMachine
{
    internal static partial class ClientHook
    {
        public static void InstallRecvHook(ClientInfo ci)
        {
            switch (ci.RecvHookType)
            {
                case 0:

                     /* 00400801   50               PUSH EAX
                      * 00400802   51               PUSH ECX
                      * 00400803   52               PUSH EDX
                      * 00400804   57               PUSH EDI
                      * 00400805   55               PUSH EBP
                      * 00400806   E8 4708B845      CALL 45F81052
                      * 0040080B   5A               POP EDX
                      * 0040080C   59               POP ECX
                      * 0040080D   58               POP EAX
                      * 0040080E   53               PUSH EBX
                      * 0040080F   56               PUSH ESI
                      * 00400810   57               PUSH EDI
                      * 00400811   89CF             MOV EDI,ECX
                      * 00400813  -E9 6D420300      JMP client.00434A85 */

                    int caveAddress0 = ci.RecvCaveAddress.ToInt32();

                    IntPtr callStart0 = (IntPtr)(caveAddress0 + 5);
                    int jmpStart0 = caveAddress0 + 18;
                    int jmpEnd0 = ci.RecvHookAddress.ToInt32() + 5;
                    byte[] call0 = CreateCALL(callStart0, ci.RecvFunctionPointer, CallType.CALL);
                    byte[] jump0 = CreateCALL(jmpStart0, jmpEnd0, CallType.JMP);
                    byte[] newCode0 = new byte[] { 
                        0x50, 0x51, 0x52, 0x57, 0x55, 0xE8, 0x00, 0x00, 
                        0x00, 0x00, 0x5A, 0x59, 0x58, 0x53, 0x56, 0x57, 
                        0x89, 0xCF, 0xE9, 0x00, 0x00, 0x00, 0x00 };
                    Buffer.BlockCopy(call0, 0, newCode0, 5, 5);
                    Buffer.BlockCopy(jump0, 0, newCode0, 18, 5);
                    byte[] hook0 = CreateCALL(ci.RecvHookAddress, ci.RecvCaveAddress, CallType.JMP);
                    Memory.Write(ci.Handle, ci.RecvCaveAddress, newCode0, true);
                    Memory.Write(ci.Handle, ci.RecvHookAddress, hook0, true);
                    return;
                case 1:

                    /* 004007F9   50               PUSH EAX
                     * 004007FA   51               PUSH ECX
                     * 004007FB   52               PUSH EDX
                     * 004007FC   57               PUSH EDI
                     * 004007FD   55               PUSH EBP
                     * 004007FE   E8 4F08AD45      CALL 45ED1052
                     * 00400803   5A               POP EDX
                     * 00400804   59               POP ECX
                     * 00400805   58               POP EAX
                     * 00400806   A1 D4797F00      MOV EAX,DWORD PTR DS:[7F79D4]
                     * 0040080B  -E9 45890300      JMP client.00439155           */

                    int caveAddress1 = ci.RecvCaveAddress.ToInt32();
                    byte[] firstInstruction = new byte[5];
                    Memory.Read(ci.Handle, ci.RecvHookAddress, firstInstruction, true);
                    IntPtr callStart1 = (IntPtr)(caveAddress1 + 5);
                    int jmpStart1 = caveAddress1 + 18;
                    int jmpEnd1 = ci.RecvHookAddress.ToInt32() + 5;
                    byte[] call1 = CreateCALL(callStart1, ci.RecvFunctionPointer, CallType.CALL);
                    byte[] jump1 = CreateCALL(jmpStart1, jmpEnd1, CallType.JMP);
                    byte[] newCode1 = new byte[] { 
                        0x50, 0x51, 0x52, 0x57, 0x55, 0xE8, 0x00, 0x00, 
                        0x00, 0x00, 0x5A, 0x59, 0x58, 0xFF, 0xFF, 0xFF, 
                        0xFF, 0xFF, 0xE9, 0x00, 0x00, 0x00, 0x00 };
                    Buffer.BlockCopy(firstInstruction, 0, newCode1, 13, 5);
                    Buffer.BlockCopy(call1, 0, newCode1, 5, 5);
                    Buffer.BlockCopy(jump1, 0, newCode1, 18, 5);
                    byte[] hook1 = CreateCALL(ci.RecvHookAddress, ci.RecvCaveAddress, CallType.JMP);
                    Memory.Write(ci.Handle, ci.RecvCaveAddress, newCode1, true);
                    Memory.Write(ci.Handle, ci.RecvHookAddress, hook1, true);
                    return;
            }
        }
    }
}