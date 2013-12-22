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
    internal static class Skills
    {
        /// <summary>
        /// Get string array of skills from specified UO installation.
        /// </summary>
        /// <param name="dataFolder">
        /// UO installation folder.
        /// </param>
        internal static string[] GetSkills(string dataFolder)
        {
            string skillIndexFile = Path.Combine(dataFolder, "skills.idx");
            string skillMulFile = Path.Combine(dataFolder, "skills.mul");
            if (!File.Exists(skillIndexFile))
                throw new FileNotFoundException(string.Format("File \"{0}\" not found!", skillIndexFile));
            if (!File.Exists(skillMulFile))
                throw new FileNotFoundException(string.Format("File \"{0}\" not found!", skillMulFile));
            byte[] indexBytes = File.ReadAllBytes(skillIndexFile);
            byte[] mulBytes = File.ReadAllBytes(skillMulFile);
            string[] skillArray = new string[indexBytes.Length / 12];
            int offset = 0;
            for (int x = 0; x < skillArray.Length; x++)
            {
                offset = x * 12;
                int start = BitConverter.ToInt32(indexBytes, offset);
                int length = BitConverter.ToInt32(indexBytes, offset + 4);
                if (length == 0)
                {
                    string[] newArray = new string[x];
                    Array.Copy(skillArray, 0, newArray, 0, x);
                    skillArray = newArray;
                    break;
                }
                skillArray[x] = ASCIIEncoding.ASCII.GetString(mulBytes, start + 1, length - 2);
            }
            return skillArray;
        }
    }
}