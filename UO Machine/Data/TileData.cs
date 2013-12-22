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
using System.Text;
using System.IO;
using System.Threading;

namespace UOMachine.Data
{
    internal static class TileData
    {
        private static InternalLandTile[] myLandTiles;
        private static InternalStaticTile[] myStaticTiles;

        private static void LoadLandTiles(byte[] data, InternalLandTile[] landTiles)
        {
            int offset = 4;
            int index = 0;
            for (int x = 0; x < landTiles.Length; x++)
            {
                if (index == 32)
                {
                    offset += 4;
                    index = 0;
                }
                landTiles[x].Flags = (TileFlags)BitConverter.ToUInt32(data, (x * 26) + offset);
                landTiles[x].ID = BitConverter.ToUInt16(data, (x * 26) + 4 + offset);
                landTiles[x].Name = ASCIIEncoding.ASCII.GetString(data, (x * 26) + 6 + offset, 20).TrimEnd('\0');
                index++;
            }
        }

        private static void LoadStaticTiles(byte[] data, InternalStaticTile[] staticTiles)
        {
            int offset = 428032 + 4; //beginning of static section + 4
            int index = 0;
            for (int x = 0; x < staticTiles.Length; x++)
            {
                if (index == 32)
                {
                    offset += 4;
                    index = 0;
                }
                staticTiles[x].ID = (ushort)x;
                staticTiles[x].Flags = (TileFlags)BitConverter.ToUInt32(data, (x * 37) + offset);
                staticTiles[x].Weight = data[(x * 37) + 4 + offset];
                staticTiles[x].Quantity = data[(x * 37) + 9 + offset];
                //staticTiles[x].AnimID = BitConverter.ToUInt16(data, (x * 37) + 10 + offset);
                staticTiles[x].Name = ASCIIEncoding.ASCII.GetString(data, (x * 37) + 17 + offset, 20).TrimEnd('\0');
                index++;
            }
        }

        /// <summary>
        /// Load tiledata.mul from specified UO installation into memory.
        /// </summary>
        public static void Initialize(string dataFolder)
        {
            string fileName = Path.Combine(dataFolder, "tiledata.mul");
            if (!File.Exists(fileName)) throw new FileNotFoundException(string.Format("File {0} doesn't exist!", fileName));
            byte[] fileBytes = File.ReadAllBytes(fileName);
            myLandTiles = new InternalLandTile[16384];
            myStaticTiles = new InternalStaticTile[(fileBytes.Length - 428032) / 1188 * 32];
            LoadLandTiles(fileBytes, myLandTiles);
            LoadStaticTiles(fileBytes, myStaticTiles);
        }

        /// <summary>
        /// Get specified land tile.
        /// </summary>
        public static InternalLandTile GetLandTile(int index)
        {
            return myLandTiles[index];
        }

        /// <summary>
        /// Get specified static tile.
        /// </summary>
        public static InternalStaticTile GetStaticTile(int index)
        {
            return myStaticTiles[index];
        }

        public static void DumpAllTiles(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Land tiles:");
            for (int x = 0; x < myLandTiles.Length; x++)
            {
                sb.AppendLine("index " + x + " = ID " + myLandTiles[x].ID + " = " + myLandTiles[x].Name);
            }
            sb.AppendLine("\r\n***********************************************************");
            sb.AppendLine("Static tiles:");
            for (int x = 0; x < myStaticTiles.Length; x++)
            {
                sb.AppendLine( x + " = " + myStaticTiles[x].Name);
            }
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}