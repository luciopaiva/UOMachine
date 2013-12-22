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
        public static void InstallSendHook(ClientInfo ci)
        {
            switch (ci.SendHookType)
            {
                case 0:
                    byte[] newCode0 = new byte[] { 
                        0x50, 0x51, 0x52, 0x55, 0x56, 0xE8, 0xFF, 0xFF, 
                        0xFF, 0xFF, 0x5A, 0x59, 0x58, 0x55, 0x8D, 0x8B, 
                        0xBC, 0x00, 0x00, 0x00, 0xE9, 0xFF, 0xFF, 0xFF, 
                        0xFF };
                    byte[] hook0 = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                    byte[] hookTemp0 = CreateCALL(ci.SendHookAddress, ci.SendCaveAddress, CallType.JMP);
                    byte[] functionCall0 = CreateCALL(ci.SendCaveAddress.ToInt32() + 5, ci.SendFunctionPointer.ToInt32(), CallType.CALL);
                    byte[] jmpBack0 = CreateCALL(ci.SendCaveAddress.ToInt32() + 20, ci.SendHookAddress.ToInt32() + 7, CallType.JMP);

                    Buffer.BlockCopy(hookTemp0, 0, hook0, 0, 5);
                    Buffer.BlockCopy(functionCall0, 0, newCode0, 5, 5);
                    Buffer.BlockCopy(jmpBack0, 0, newCode0, 20, 5);

                    Memory.Write(ci.Handle, ci.SendCaveAddress, newCode0, true);
                    Memory.Write(ci.Handle, ci.SendHookAddress, hook0, true);
                    break;
                case 1:
                    byte[] newCode1 = new byte[] { 
                        0x50, 0x51, 0x52, 0x53, 0x56, 0xE8, 0xFF, 0xFF,
                        0xFF, 0xFF, 0x5A, 0x59, 0x58, 0x53, 0x50, 0x8D, 
                        0x4F, 0x6C, 0xE9, 0xFF, 0xFF, 0xFF, 0xFF };
                    byte[] hook1 = CreateCALL(ci.SendHookAddress, ci.SendCaveAddress, CallType.JMP);
                    byte[] functionCall1 = CreateCALL(ci.SendCaveAddress.ToInt32() + 5, ci.SendFunctionPointer.ToInt32(), CallType.CALL);
                    byte[] jmpBack1 = CreateCALL(ci.SendCaveAddress.ToInt32() + 18, ci.SendHookAddress.ToInt32() + 5, CallType.JMP);

                    Buffer.BlockCopy(functionCall1, 0, newCode1, 5, 5);
                    Buffer.BlockCopy(jmpBack1, 0, newCode1, 18, 5);

                    Memory.Write(ci.Handle, ci.SendCaveAddress, newCode1, true);
                    Memory.Write(ci.Handle, ci.SendHookAddress, hook1, true);
                    break;
            }
        }
    }
}