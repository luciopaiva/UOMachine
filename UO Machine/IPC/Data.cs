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

namespace UOMachine.IPC
{
    static class Data
    {
        /* lengths of fixed-length IPC commands */
        private static byte[] myCommandLen = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 5, 5, 5, 5, 6, 9, 10, 10, 10, 10, 10, 13 };

        /// <summary>
        /// Returns message length of first IPC message in the stream.
        /// </summary>
        /// <param name="memoryStream">
        /// MemoryStream containing IPC message to examine.
        /// </param>
        /// <param name="readPos">
        /// Current read position of the MemoryStream.
        /// </param>
        /// <returns>
        /// Length of IPC message or 0 if incomplete message.
        /// </returns>
        private static int GetMessageLength(MemoryStream memoryStream, long readPos)
        {
            if (memoryStream.Length - readPos <= 0) return 0;
            memoryStream.Position = readPos;
            int command = memoryStream.ReadByte();
            if (command > 22)
            {
                if (memoryStream.Length - readPos < 3)
                    return 0;
                int messageLen = memoryStream.ReadByte() | memoryStream.ReadByte() << 8;
                memoryStream.Position = readPos;
                return messageLen;
            }
            memoryStream.Position = readPos;
            return myCommandLen[command];
        }

        /// <summary>
        /// Returns first IPC message contained in the stream.
        /// </summary>
        /// <param name="memoryStream">
        /// MemoryStream containing IPC message.
        /// </param>
        /// <param name="readPos">
        /// Current read position of the MemoryStream.
        /// </param>
        /// <returns>
        /// IPC message or null if incomplete message.
        /// </returns>
        public static byte[] GetMessage(MemoryStream memoryStream, ref long readPos)
        {
            int messageLen = GetMessageLength(memoryStream, readPos);
            if (messageLen == 0 || memoryStream.Position + messageLen > memoryStream.Length)
                return null;
            byte[] message = new byte[messageLen];
            memoryStream.Read(message, 0, message.Length);
            readPos += messageLen;
            if (readPos >= memoryStream.Length)
            {
                memoryStream.SetLength(0);
                readPos = 0;
            }
            else
                memoryStream.Position = memoryStream.Length;
            return message;
        }
    }

}