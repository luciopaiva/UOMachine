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
using System.Text;
using System.Collections.Generic;
using UOMachine.Events;

namespace UOMachine.Utility
{
    public static class Log
    {
        private static StreamWriter myFileWriter;
        private static string myLogFileName;

        internal static void Initialize(string logFileName)
        {
            myLogFileName = logFileName;
            myFileWriter = File.AppendText(logFileName);
            myFileWriter.AutoFlush = true;
        }

        internal static void Dispose()
        {
            try
            {
                if (myFileWriter != null)
                {
                    myFileWriter.Flush();
                    myFileWriter.Close();
                    FileInfo fi = new FileInfo(myLogFileName);
                    if (fi.Exists && fi.Length == 0)
                    {
                        File.Delete(myLogFileName);
                    }
                }
            }
            catch (ObjectDisposedException) { }
        }

        private static string GetStringFromBytes(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2 + 1);
            int index = 0;
            for (int x = 0; x < data.Length; x++)
            {
                sb.AppendFormat("{0:X2} ", data[x]);
                index++;
                if (index == 16)
                {
                    sb.Append("\r\n");
                    index = 0;
                }
            }
            return sb.ToString();
        }

        private static string GetDateString()
        {
            return "<+> ================ < " + DateTime.Now + " > ================ <+>\r\n";
        }

        private static string GetClientString(int clientIndex)
        {
            int IPCIndex = InternalEventHandler.IPCHandler.GetIPCClient(clientIndex);
            return "Log entry for IPC client " + IPCIndex + " which corresponds to UOM client " + clientIndex + ":\r\n";
        }

        public static void LogDataMessage(int clientIndex, byte[] data, string message)
        {
            string dataLen = "Length of data: " + data.Length + " bytes (0x" + data.Length.ToString("X") + ")\r\n";
            string logMessage = GetDateString() + GetClientString(clientIndex) + message + dataLen + GetStringFromBytes(data) + "\r\n\r\n";
            lock (myFileWriter) { myFileWriter.Write(logMessage); }
        }

        public static void LogMessage(int clientIndex, string message)
        {
            string logMessage = GetDateString() + GetClientString(clientIndex) + message + "\r\n\r\n";
            lock (myFileWriter) { myFileWriter.Write(logMessage); }
        }

        public static void LogMessage(string message)
        {
            string logMessage = GetDateString() + message + "\r\n\r\n";
            lock (myFileWriter) { myFileWriter.Write(logMessage); }
        }

        public static void LogMessage(Exception exception)
        {
            string message = exception.Message + "\r\n" + exception.Source + "\r\n" + exception.StackTrace;
            string logMessage = GetDateString() + "Exception:\r\n" + message + "\r\n\r\n";
            lock (myFileWriter) { myFileWriter.Write(logMessage); }
        }
    }
}