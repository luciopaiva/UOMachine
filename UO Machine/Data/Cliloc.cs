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

namespace UOMachine.Data
{
    public static class Cliloc
    {
        private static Dictionary<int, string> myPropertyHashList;

        /// <summary>
        /// Build hash table of localized text strings.
        /// </summary>
        /// <param name="dataFolder">Ultima Online installation folder.</param>
        public static void Initialize(string dataFolder)
        {
            string filename = Path.Combine(dataFolder, "cliloc.enu");
            if (!File.Exists(filename)) throw new ArgumentException("File not found " + filename);
            myPropertyHashList = new Dictionary<int, string>(100000);
            byte[] fileBytes = File.ReadAllBytes(filename);
            ushort len = 0;
            for (int x = 6; x < fileBytes.Length; x += 7 + len)
            {
                len = BitConverter.ToUInt16(fileBytes, x + 5);
                myPropertyHashList.Add(BitConverter.ToInt32(fileBytes, x), Encoding.UTF8.GetString(fileBytes, x + 7, len));
            }
        }

        /// <summary>
        /// Dump all clilocs to specified text file.
        /// </summary>
        /// <param name="filename">Filename to write results to.</param>
        public static void DumpClilocs(string filename)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Number of entries = " + myPropertyHashList.Count + ".\r\n\r\n");
            foreach (KeyValuePair<int, string> kvp in myPropertyHashList)
            {
                sb.Append(String.Format("{0} = {1}\r\n", kvp.Key, kvp.Value));
            }
            try
            {
                File.WriteAllText(filename, sb.ToString());
            }
            catch { }
        }

        /// <summary>
        /// Get localized string with tokens.
        /// </summary>
        public static string GetProperty(int property)
        {
            string propertyString;
            if (myPropertyHashList.TryGetValue(property, out propertyString))
                return propertyString;
            return string.Format("Localized string {0} not found!", property);
        }


        public static string GetLocalString(string tokenizedString)
        {
            while (tokenizedString.Contains("#"))
            {
                int y;
                for (int x = 0; x < tokenizedString.Length; x++)
                {
                    if (tokenizedString[x] == '#' && x < tokenizedString.Length - 1)
                    {
                        for (y = x + 1; y < tokenizedString.Length; y++)
                            if (!Char.IsNumber(tokenizedString[y])) break;
                        string token = tokenizedString.Substring(x, y - x);
                        string tokenNum = tokenizedString.Substring(x + 1, y - x - 1);
                        int propertyNum;
                        if (tokenNum.Length > 0)
                        {
                            try { propertyNum = Convert.ToInt32(tokenNum); }
                            catch { return tokenizedString; }
                            string property = GetProperty(propertyNum);
                            tokenizedString = tokenizedString.Replace(token, property);
                        }
                    }
                }
            }
            return tokenizedString;
        }

        /// <summary>
        /// Get localized string with tokens replaced by arguments.
        /// </summary>
        /// <param name="property">Text entry to retrieve.</param>
        /// <param name="arguments">Property arguments already split at tab marks.</param>
        public static string GetLocalString(int property, string[] arguments)
        {
            string propertyString = GetProperty(property);
            if (arguments == null) return propertyString;
            //foreach (string s in arguments)
            for (int x = 0; x < arguments.Length; x++)
            {
                arguments[x] = GetLocalString(arguments[x]);
                bool found = false;
                int start = 0;
                int index = 0;
                foreach (char c in propertyString)
                {
                    if (c == '~')
                    {
                        if (found)
                        {
                            string subString = propertyString.Substring(start, (index - start) + 1);
                            propertyString = propertyString.Replace(subString, arguments[x]);
                            break;
                        }
                        else
                        {
                            start = index;
                            found = true;
                        }
                    }
                    index++;
                }
                if (!found) return propertyString;
            }

            return propertyString;
        }
    }
}