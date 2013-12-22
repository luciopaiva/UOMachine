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
    internal enum CallType : byte
    {
        CALL,
        JMP
    }

    internal static partial class ClientHook
    {
        /// <summary>
        /// Create far JMP or CALL instruction from source address to target.
        /// </summary>
        /// <param name="sourceAddress">Address which CALL originates from rather than EIP of next instruction.</param>
        /// <param name="targetAddress">Target address to call.</param>
        /// <param name="callType">Type of instruction to assemble.</param>
        /// <returns>Byte array of assembled instruction.</returns>
        public static byte[] CreateCALL(IntPtr sourceAddress, IntPtr targetAddress, CallType callType)
        {
            int source = sourceAddress.ToInt32();
            int target = targetAddress.ToInt32();
            int offset = target - source - 5;
            byte[] CALL = new byte[5];
            switch (callType)
            {
                case CallType.CALL:
                    CALL[0] = 0xE8;
                    break;
                case CallType.JMP:
                    CALL[0] = 0xE9;
                    break;
            }
            CALL[1] = (byte)(offset);
            CALL[2] = (byte)(offset >> 8);
            CALL[3] = (byte)(offset >> 16);
            CALL[4] = (byte)(offset >> 24);
            return CALL;
        }

        /// <summary>
        /// Create far JMP or CALL instruction from source address to target.
        /// </summary>
        /// <param name="sourceAddress">Address which CALL originates from rather than EIP of next instruction.</param>
        /// <param name="targetAddress">Target address to call.</param>
        /// <param name="callType">Type of instruction to assemble.</param>
        /// <returns>Byte array of assembled instruction.</returns>
        public static byte[] CreateCALL(int sourceAddress, int targetAddress, CallType callType)
        {
            int offset = targetAddress - sourceAddress - 5;
            byte[] CALL = new byte[5];
            switch (callType)
            {
                case CallType.CALL:
                    CALL[0] = 0xE8;
                    break;
                case CallType.JMP:
                    CALL[0] = 0xE9;
                    break;
            }
            CALL[1] = (byte)(offset);
            CALL[2] = (byte)(offset >> 8);
            CALL[3] = (byte)(offset >> 16);
            CALL[4] = (byte)(offset >> 24);
            return CALL;
        }
    }
}