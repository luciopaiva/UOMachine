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

using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Diagnostics;

namespace UOMachine.Utility
{
    public static class ExeInfo
    {

        public static int GetStamp(byte[] fileBytes)
        {
            int offset = BitConverter.ToUInt16(fileBytes, 60);
            offset += 8;
            return BitConverter.ToInt32(fileBytes, offset);
        }

        public static int GetBaseAddress(byte[] fileBytes)
        {
            int offset = BitConverter.ToUInt16(fileBytes, 60);
            offset += 0x18 + 0x1C;
            return BitConverter.ToInt32(fileBytes, offset);
        }

        public static string GetFileVersion(string fileName)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
            return fvi.FileMajorPart.ToString() + "." +
                fvi.FileMinorPart.ToString() + "." +
                fvi.FileBuildPart.ToString() + "." +
                fvi.FilePrivatePart.ToString();
        }

        public static string DateToVersion(int dateStamp)
        {
            string version;

            switch (dateStamp)
            {
                case 1122359145:
                    version = "4.0.11c";
                    break;
                case 1123020364:
                    version = "4.0.11c";  //client still says c
                    break;
                case 1124847093:
                    version = "5.0.0a";
                    break;
                case 1125351474:
                    version = "5.0.0b";
                    break;
                case 1126675936:
                    version = "5.0.1a";
                    break;
                case 1127236860:
                    version = "5.0.1a"; //client still says a
                    break;
                case 1127868200:
                    version = "5.0.1c";
                    break;
                case 1129931910:
                    version = "5.0.1d";
                    break;
                case 1133214491:
                    version = "5.0.1e";
                    break;
                case 1133872114:
                    version = "5.0.1f";
                    break;
                case 1134586421:
                    version = "5.0.1h";
                    break;
                case 1138395179:
                    version = "5.0.1i";
                    break;
                case 1139436531:
                    version = "5.0.1j";
                    break;
                case 1145969399:
                    version = "5.0.2";
                    break;
                case 1147305936:
                    version = "5.0.2a";
                    break;
                case 1147903365:
                    version = "5.0.2b";
                    break;
                case 1150402069:
                    version = "5.0.2c";
                    break;
                case 1151017776:
                    version = "5.0.2d";
                    break;
                case 1152122170:
                    version = "5.0.2e";
                    break;
                case 1152158582:
                    version = "5.0.2f";
                    break;
                case 1152583317:
                    version = "5.0.2g";
                    break;
                case 1155601081:
                    version = "5.0.3";
                    break;
                case 1156284223:
                    version = "5.0.4a";
                    break;
                case 1156371995:
                    version = "5.0.4b";
                    break;
                case 1156880023:
                    version = "5.0.4c";
                    break;
                case 1158185649:
                    version = "5.0.4d";
                    break;
                default:
                    version = "";
                    break;
            }

            return version;
        }

    }
}