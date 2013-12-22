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
using System.IO;

namespace UOMachine.Data
{
    public static class Map
    {
        private struct StaticIndexRecord
        {
            public int start;
            public int length;
        }

        private struct StaticRecord
        {
            public ushort id;
            public byte x;
            public byte y;
            public sbyte z;
        }

        private static StaticIndexRecord[] myStaticsIndex0 = null;
        private static StaticIndexRecord[] myStaticsIndex1 = null;
        private static StaticIndexRecord[] myStaticsIndex2 = null;
        private static StaticIndexRecord[] myStaticsIndex3 = null;
        private static StaticIndexRecord[] myStaticsIndex4 = null;
        private static StaticIndexRecord[] myStaticsIndex5 = null;

        private static StaticRecord[][] myStaticItems0 = null;
        private static StaticRecord[][] myStaticItems1 = null;
        private static StaticRecord[][] myStaticItems2 = null;
        private static StaticRecord[][] myStaticItems3 = null;
        private static StaticRecord[][] myStaticItems4 = null;
        private static StaticRecord[][] myStaticItems5 = null;

        private static FileStream myMap0Reader = null;
        private static FileStream myMap1Reader = null;
        private static FileStream myMap2Reader = null;
        private static FileStream myMap3Reader = null;
        private static FileStream myMap4Reader = null;
        private static FileStream myMap5Reader = null;

        private static FileStream myStaticIndex0Reader = null;
        private static FileStream myStaticIndex1Reader = null;
        private static FileStream myStaticIndex2Reader = null;
        private static FileStream myStaticIndex3Reader = null;
        private static FileStream myStaticIndex4Reader = null;
        private static FileStream myStaticIndex5Reader = null;

        private static FileStream myStatics0Reader = null;
        private static FileStream myStatics1Reader = null;
        private static FileStream myStatics2Reader = null;
        private static FileStream myStatics3Reader = null;
        private static FileStream myStatics4Reader = null;
        private static FileStream myStatics5Reader = null;

        private static bool myUsingNewMaps;
        private const int myNewMap0Len = 89915392;
        private const int myOldMap0XLimit = 6136;
        private const int myOldMap0YLimit = 4088;
        private const int myNewMap0XLimit = 7160;
        private const int myNewMap0YLimit = 4088;
        private const int myOldMap1XLimit = 6136;
        private const int myOldMap1YLimit = 4088;
        private const int myNewMap1XLimit = 7160;
        private const int myNewMap1YLimit = 4088;
        private const int myMap2XLimit = 2296;
        private const int myMap2YLimit = 1592;
        private const int myMap3XLimit = 2552;
        private const int myMap3YLimit = 2040;
        private const int myMap4XLimit = 1440;
        private const int myMap4YLimit = 1440;
        private const int myMap5XLimit = 1272;
        private const int myMap5YLimit = 4088;

        private static bool myCacheStaticIndices = false;
        private static bool myCacheStatics = false;

        internal static void Initialize(string dataFolder, int cacheLevel)
        {
            switch (cacheLevel)
            {
                case 0:
                    myCacheStaticIndices = false;
                    myCacheStatics = false;
                    break;
                case 1:
                    myCacheStaticIndices = true;
                    myCacheStatics = false;
                    break;
                case 2:
                    myCacheStaticIndices = true;
                    myCacheStatics = true;
                    break;
            }

            string staidx0Path = Path.Combine(dataFolder, "staidx0.mul");
            string staidx1Path = Path.Combine(dataFolder, "staidx1.mul");
            string staidx2Path = Path.Combine(dataFolder, "staidx2.mul");
            string staidx3Path = Path.Combine(dataFolder, "staidx3.mul");
            string staidx4Path = Path.Combine(dataFolder, "staidx4.mul");
            string staidx5Path = Path.Combine(dataFolder, "staidx5.mul");

            string statics0Path = Path.Combine(dataFolder, "statics0.mul");
            string statics1Path = Path.Combine(dataFolder, "statics1.mul");
            string statics2Path = Path.Combine(dataFolder, "statics2.mul");
            string statics3Path = Path.Combine(dataFolder, "statics3.mul");
            string statics4Path = Path.Combine(dataFolder, "statics4.mul");
            string statics5Path = Path.Combine(dataFolder, "statics5.mul");

            string map0Path = Path.Combine(dataFolder, "map0.mul");
            string map1Path = Path.Combine(dataFolder, "map1.mul");
            string map2Path = Path.Combine(dataFolder, "map2.mul");
            string map3Path = Path.Combine(dataFolder, "map3.mul");
            string map4Path = Path.Combine(dataFolder, "map4.mul");
            string map5Path = Path.Combine(dataFolder, "map5.mul");

            if (myCacheStaticIndices)
            {
                LoadStaticIndex(staidx0Path, ref myStaticsIndex0);
                LoadStaticIndex(staidx1Path, ref myStaticsIndex1);
                LoadStaticIndex(staidx2Path, ref myStaticsIndex2);
                LoadStaticIndex(staidx3Path, ref myStaticsIndex3);
                LoadStaticIndex(staidx4Path, ref myStaticsIndex4);
                LoadStaticIndex(staidx5Path, ref myStaticsIndex5);
            }
            else
            {
                if (File.Exists(staidx0Path))
                    myStaticIndex0Reader = File.OpenRead(staidx0Path);
                if (File.Exists(staidx1Path))
                    myStaticIndex1Reader = File.OpenRead(staidx1Path);
                if (File.Exists(staidx2Path))
                    myStaticIndex2Reader = File.OpenRead(staidx2Path);
                if (File.Exists(staidx3Path))
                    myStaticIndex3Reader = File.OpenRead(staidx3Path);
                if (File.Exists(staidx4Path))
                    myStaticIndex4Reader = File.OpenRead(staidx4Path);
                if (File.Exists(staidx5Path))
                    myStaticIndex5Reader = File.OpenRead(staidx5Path);
            }

            if (myCacheStatics)
            {
                if (myStaticsIndex0 != null)
                {
                    myStaticItems0 = new StaticRecord[myStaticsIndex0.Length][];
                    LoadStatics(statics0Path, ref myStaticsIndex0, ref myStaticItems0);
                }

                if (myStaticsIndex1 != null)
                {
                    myStaticItems1 = new StaticRecord[myStaticsIndex1.Length][];
                    LoadStatics(statics1Path, ref myStaticsIndex1, ref myStaticItems1);
                }

                if (myStaticsIndex2 != null)
                {
                    myStaticItems2 = new StaticRecord[myStaticsIndex2.Length][];
                    LoadStatics(statics2Path, ref myStaticsIndex2, ref myStaticItems2);
                }

                if (myStaticsIndex3 != null)
                {
                    myStaticItems3 = new StaticRecord[myStaticsIndex3.Length][];
                    LoadStatics(statics3Path, ref myStaticsIndex3, ref myStaticItems3);
                }

                if (myStaticsIndex4 != null)
                {
                    myStaticItems4 = new StaticRecord[myStaticsIndex4.Length][];
                    LoadStatics(statics4Path, ref myStaticsIndex4, ref myStaticItems4);
                }

                if (myStaticsIndex5 != null)
                {
                    myStaticItems5 = new StaticRecord[myStaticsIndex5.Length][];
                    LoadStatics(statics5Path, ref myStaticsIndex5, ref myStaticItems5);
                }

            }
            else
            {
                if (File.Exists(statics0Path))
                {
                    myStatics0Reader = File.OpenRead(statics0Path);
                }

                if (File.Exists(statics1Path))
                {
                    myStatics1Reader = File.OpenRead(statics1Path);
                }

                if (File.Exists(statics2Path))
                {
                    myStatics2Reader = File.OpenRead(statics2Path);
                }

                if (File.Exists(statics3Path))
                {
                    myStatics3Reader = File.OpenRead(statics3Path);
                }

                if (File.Exists(statics4Path))
                {
                    myStatics4Reader = File.OpenRead(statics4Path);
                }

                if (File.Exists(statics5Path))
                {
                    myStatics5Reader = File.OpenRead(statics5Path);
                }
            }

            if (File.Exists(map0Path))
                myMap0Reader = File.OpenRead(map0Path);
            if (File.Exists(map1Path))
                myMap1Reader = File.OpenRead(map1Path);
            if (File.Exists(map2Path))
                myMap2Reader = File.OpenRead(map2Path);
            if (File.Exists(map3Path))
                myMap3Reader = File.OpenRead(map3Path);
            if (File.Exists(map4Path))
                myMap4Reader = File.OpenRead(map4Path);
            if (File.Exists(map5Path))
                myMap5Reader = File.OpenRead(map5Path);

            if (myMap0Reader != null && myMap0Reader.Length == myNewMap0Len)
                myUsingNewMaps = true;
            else myUsingNewMaps = false;

            GC.Collect(); //if full caching enabled this will free a lot of memory
        }

        internal static void Dispose()
        {
            if (myMap0Reader != null) myMap0Reader.Close();
            if (myMap1Reader != null) myMap1Reader.Close();
            if (myMap2Reader != null) myMap2Reader.Close();
            if (myMap3Reader != null) myMap3Reader.Close();
            if (myMap4Reader != null) myMap4Reader.Close();
            if (myStaticIndex0Reader != null) myStaticIndex0Reader.Close();
            if (myStaticIndex1Reader != null) myStaticIndex1Reader.Close();
            if (myStaticIndex2Reader != null) myStaticIndex2Reader.Close();
            if (myStaticIndex3Reader != null) myStaticIndex3Reader.Close();
            if (myStaticIndex4Reader != null) myStaticIndex4Reader.Close();
            if (myStatics0Reader != null) myStatics0Reader.Close();
            if (myStatics1Reader != null) myStatics1Reader.Close();
            if (myStatics2Reader != null) myStatics2Reader.Close();
            if (myStatics3Reader != null) myStatics3Reader.Close();
            if (myStatics4Reader != null) myStatics4Reader.Close();
        }

        private static void LoadStaticIndex(string fileName, ref StaticIndexRecord[] staticsIndex)
        {
            if (File.Exists(fileName))
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                staticsIndex = new StaticIndexRecord[fileBytes.Length / 12];
                for (int x = 0; x < staticsIndex.Length; x++)
                {
                    staticsIndex[x].start = BitConverter.ToInt32(fileBytes, x * 12);
                    staticsIndex[x].length = BitConverter.ToInt32(fileBytes, x * 12 + 4);
                }
            }
        }

        private static void LoadStatics(string fileName, ref StaticIndexRecord[] staticIndexRecord, ref StaticRecord[][] staticItems)
        {
            if (File.Exists(fileName))
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                for (int x = 0; x < staticIndexRecord.Length; x++)
                {
                    if (staticIndexRecord[x].start == -1) continue;
                    int recordsToFetch = staticIndexRecord[x].length / 7;
                    staticItems[x] = new StaticRecord[recordsToFetch];
                    for (int y = 0; y < recordsToFetch; y++)
                    {
                        staticItems[x][y].id = BitConverter.ToUInt16(fileBytes, staticIndexRecord[x].start + y * 7);
                        staticItems[x][y].x = fileBytes[staticIndexRecord[x].start + y * 7 + 2];
                        staticItems[x][y].y = fileBytes[staticIndexRecord[x].start + y * 7 + 3];
                        staticItems[x][y].z = (sbyte)fileBytes[staticIndexRecord[x].start + y * 7 + 4];
                    }
                }
            }
        }

        private static StaticIndexRecord GetStaticIndexRecord(int blockIndex, FileStream fileStream)
        {
            StaticIndexRecord indexRecord = new StaticIndexRecord();
            byte[] startBytes = new byte[4], lengthBytes = new byte[4];
            lock (fileStream)
            {
                fileStream.Seek(blockIndex * 12, SeekOrigin.Begin);
                fileStream.Read(startBytes, 0, 4);
                fileStream.Read(lengthBytes, 0, 4);
            }
            indexRecord.start = BitConverter.ToInt32(startBytes, 0);
            indexRecord.length = BitConverter.ToInt32(lengthBytes, 0);
            return indexRecord;
        }

        private static StaticRecord[] GetStaticRecords(StaticIndexRecord indexRecord, FileStream fileStream)
        {
            if (indexRecord.length == -1) return null;

            StaticRecord[] staticRecords = new StaticRecord[indexRecord.length / 7];
            byte[] fileBytes = new byte[indexRecord.length];
            lock (fileStream)
            {
                fileStream.Seek(indexRecord.start, SeekOrigin.Begin);
                fileStream.Read(fileBytes, 0, indexRecord.length);
            }
            for (int x = 0; x < staticRecords.Length; x++)
            {
                staticRecords[x].id = BitConverter.ToUInt16(fileBytes, x * 7);
                staticRecords[x].x = fileBytes[x * 7 + 2];
                staticRecords[x].y = fileBytes[x * 7 + 3];
                staticRecords[x].z = (sbyte)fileBytes[x * 7 + 4];
            }
            return staticRecords;
        }

        private static StaticTile[] GetMatchingTiles(StaticRecord[] staticRecords, int x, int y)
        {
            if (staticRecords == null) return null;
            int cellX = x % 8;
            int cellY = y % 8;
            List<StaticTile> tileList = new List<StaticTile>(32);
            foreach (StaticRecord s in staticRecords)
            {
                if (s.x == cellX && s.y == cellY)
                {
                    StaticTile staticTile = new StaticTile(TileData.GetStaticTile(s.id));
                    staticTile.X = x;
                    staticTile.Y = y;
                    staticTile.Z = s.z;
                    tileList.Add(staticTile);
                }
            }
            if (tileList.Count > 0) return tileList.ToArray();
            return null;
        }

        private static StaticTile[] GetStaticTiles(int map, int x, int y)
        {
            int blockIndex;
            StaticIndexRecord indexRecord;
            StaticRecord[] staticRecords = null;
            switch (map)
            {
                case 0:
                    blockIndex = x / 8 * 512 + y / 8;
                    if (myCacheStatics && myStaticsIndex0 != null && myStaticItems0 != null)
                    {
                        indexRecord = myStaticsIndex0[blockIndex];
                        staticRecords = myStaticItems0[blockIndex];
                    }
                    else if (myCacheStaticIndices && myStaticsIndex0 != null && myStatics0Reader != null)
                    {
                        indexRecord = myStaticsIndex0[blockIndex];
                        staticRecords = GetStaticRecords(indexRecord, myStatics0Reader);
                    }
                    else if (myStaticIndex0Reader != null && myStatics0Reader != null)
                    {
                        indexRecord = GetStaticIndexRecord(blockIndex, myStaticIndex0Reader);
                        staticRecords = GetStaticRecords(indexRecord, myStatics0Reader);
                    }
                    return GetMatchingTiles(staticRecords, x, y);
                case 1:
                    blockIndex = x / 8 * 512 + y / 8;
                    if (myCacheStatics && myStaticsIndex1 != null && myStaticItems1 != null)
                    {
                        indexRecord = myStaticsIndex1[blockIndex];
                        staticRecords = myStaticItems1[blockIndex];
                    }
                    else if (myCacheStaticIndices && myStaticsIndex1 != null && myStatics1Reader != null)
                    {
                        indexRecord = myStaticsIndex1[blockIndex];
                        staticRecords = GetStaticRecords(indexRecord, myStatics1Reader);
                    }
                    else if (myStaticIndex1Reader != null && myStatics1Reader != null)
                    {
                        indexRecord = GetStaticIndexRecord(blockIndex, myStaticIndex1Reader);
                        staticRecords = GetStaticRecords(indexRecord, myStatics1Reader);
                    }
                    return GetMatchingTiles(staticRecords, x, y);
                case 2:
                    blockIndex = x / 8 * 200 + y / 8;
                    if (myCacheStatics && myStaticsIndex2 != null && myStaticItems2 != null)
                    {
                        indexRecord = myStaticsIndex2[blockIndex];
                        staticRecords = myStaticItems2[blockIndex];
                    }
                    else if (myCacheStaticIndices && myStaticsIndex2 != null && myStatics2Reader != null)
                    {
                        indexRecord = myStaticsIndex2[blockIndex];
                        staticRecords = GetStaticRecords(indexRecord, myStatics2Reader);
                    }
                    else if (myStaticIndex2Reader != null && myStatics2Reader != null)
                    {
                        indexRecord = GetStaticIndexRecord(blockIndex, myStaticIndex2Reader);
                        staticRecords = GetStaticRecords(indexRecord, myStatics2Reader);
                    }
                    return GetMatchingTiles(staticRecords, x, y);
                case 3:
                    blockIndex = x / 8 * 256 + y / 8;
                    if (myCacheStatics && myStaticsIndex3 != null && myStaticItems3 != null)
                    {
                        indexRecord = myStaticsIndex3[blockIndex];
                        staticRecords = myStaticItems3[blockIndex];
                    }
                    else if (myCacheStaticIndices && myStaticsIndex3 != null && myStatics3Reader != null)
                    {
                        indexRecord = myStaticsIndex3[blockIndex];
                        staticRecords = GetStaticRecords(indexRecord, myStatics3Reader);
                    }
                    else if (myStaticIndex3Reader != null && myStatics3Reader != null)
                    {
                        indexRecord = GetStaticIndexRecord(blockIndex, myStaticIndex3Reader);
                        staticRecords = GetStaticRecords(indexRecord, myStatics3Reader);
                    }
                    return GetMatchingTiles(staticRecords, x, y);
                case 4:
                    blockIndex = x / 8 * 181 + y / 8;
                    if (myCacheStatics && myStaticsIndex4 != null && myStaticItems4 != null)
                    {
                        indexRecord = myStaticsIndex4[blockIndex];
                        staticRecords = myStaticItems4[blockIndex];
                    }
                    else if (myCacheStaticIndices && myStaticsIndex4 != null && myStatics4Reader != null)
                    {
                        indexRecord = myStaticsIndex4[blockIndex];
                        staticRecords = GetStaticRecords(indexRecord, myStatics4Reader);
                    }
                    else if (myStaticIndex4Reader != null && myStatics4Reader != null)
                    {
                        indexRecord = GetStaticIndexRecord(blockIndex, myStaticIndex4Reader);
                        staticRecords = GetStaticRecords(indexRecord, myStatics4Reader);
                    }
                    return GetMatchingTiles(staticRecords, x, y);
                case 5:
                    blockIndex = x / 8 * 512 + y / 8;
                    if (myCacheStatics && myStaticsIndex5 != null && myStaticItems5 != null)
                    {
                        indexRecord = myStaticsIndex5[blockIndex];
                        staticRecords = myStaticItems5[blockIndex];
                    }
                    else if (myCacheStaticIndices && myStaticsIndex5 != null && myStatics5Reader != null)
                    {
                        indexRecord = myStaticsIndex5[blockIndex];
                        staticRecords = GetStaticRecords(indexRecord, myStatics5Reader);
                    }
                    else if (myStaticIndex5Reader != null && myStatics5Reader != null)
                    {
                        indexRecord = GetStaticIndexRecord(blockIndex, myStaticIndex5Reader);
                        staticRecords = GetStaticRecords(indexRecord, myStatics5Reader);
                    }
                    return GetMatchingTiles(staticRecords, x, y);
            }
            return null;
        }

        internal static bool GetInfo(int map, int x, int y, out MapInfo mapInfo)
        {
            MapInfo m = new MapInfo();
            if (x < 0 || y < 0)
            {
                mapInfo = null;
                return false;
            }

            int blockIndex;
            int cellX = x % 8;
            int cellY = y % 8;

            switch (map)
            {
                case 0:
                    if (myMap0Reader == null || (myUsingNewMaps && (x > myNewMap0XLimit || y > myNewMap0YLimit)) ||
                        (!myUsingNewMaps && (x > myOldMap0XLimit || y > myOldMap0YLimit)))
                    {
                        mapInfo = null;
                        return false;
                    }
                    blockIndex = x / 8 * 512 + y / 8;
                    lock (myMap0Reader)
                    {
                        myMap0Reader.Seek(blockIndex * 196 + 4 + (cellY * 24 + cellX * 3), SeekOrigin.Begin);
                        m.landTile = new LandTile(TileData.GetLandTile(myMap0Reader.ReadByte() | myMap0Reader.ReadByte() << 8));
                        m.landTile.X = x;
                        m.landTile.Y = y;
                        m.landTile.Z = (sbyte)myMap0Reader.ReadByte();
                    }
                    m.staticTiles = GetStaticTiles(0, x, y);
                    mapInfo = m;
                    return true;
                case 1:
                    if (myMap1Reader == null || (myUsingNewMaps && (x > myNewMap1XLimit || y > myNewMap1YLimit)) ||
                        (!myUsingNewMaps && (x > myOldMap1XLimit || y > myOldMap1YLimit)))
                    {
                        mapInfo = null;
                        return false;
                    }

                    blockIndex = x / 8 * 512 + y / 8;
                    mapInfo = new MapInfo();
                    lock (myMap1Reader)
                    {
                        myMap1Reader.Seek(blockIndex * 196 + 4 + (cellY * 24 + cellX * 3), SeekOrigin.Begin);
                        mapInfo.landTile = new LandTile(TileData.GetLandTile(myMap1Reader.ReadByte() | myMap1Reader.ReadByte() << 8));
                        mapInfo.landTile.X = x;
                        mapInfo.landTile.Y = y;
                        mapInfo.landTile.Z = (sbyte)myMap1Reader.ReadByte();
                    }
                    mapInfo.staticTiles = GetStaticTiles(1, x, y);
                    return true;
                case 2:
                    if (myMap2Reader == null || x > myMap2XLimit || y > myMap2YLimit)
                    {
                        mapInfo = null;
                        return false;
                    }
                    blockIndex = x / 8 * 200 + y / 8;
                    lock (myMap2Reader)
                    {
                        myMap2Reader.Seek(blockIndex * 196 + 4 + (cellY * 24 + cellX * 3), SeekOrigin.Begin);
                        m.landTile = new LandTile(TileData.GetLandTile(myMap2Reader.ReadByte() | myMap2Reader.ReadByte() << 8));
                        m.landTile.X = x;
                        m.landTile.Y = y;
                        m.landTile.Z = (sbyte)myMap2Reader.ReadByte();
                    }
                    m.staticTiles = GetStaticTiles(2, x, y);
                    mapInfo = m;
                    return true;
                case 3:
                    if (myMap3Reader == null || x > myMap3XLimit || y > myMap3YLimit)
                    {
                        mapInfo = null;
                        return false;
                    }
                    blockIndex = x / 8 * 256 + y / 8;
                    lock (myMap3Reader)
                    {
                        myMap3Reader.Seek(blockIndex * 196 + 4 + (cellY * 24 + cellX * 3), SeekOrigin.Begin);
                        m.landTile = new LandTile(TileData.GetLandTile(myMap3Reader.ReadByte() | myMap3Reader.ReadByte() << 8));
                        m.landTile.X = x;
                        m.landTile.Y = y;
                        m.landTile.Z = (sbyte)myMap3Reader.ReadByte();
                    }
                    m.staticTiles = GetStaticTiles(3, x, y);
                    mapInfo = m;
                    return true;
                case 4:
                    if (myMap4Reader == null || x > myMap4XLimit || y > myMap4YLimit)
                    {
                        mapInfo = null;
                        return false;
                    }
                    blockIndex = x / 8 * 181 + y / 8;
                    lock (myMap4Reader)
                    {
                        myMap4Reader.Seek(blockIndex * 196 + 4 + (cellY * 24 + cellX * 3), SeekOrigin.Begin);
                        m.landTile = new LandTile(TileData.GetLandTile(myMap4Reader.ReadByte() | myMap4Reader.ReadByte() << 8));
                        m.landTile.X = x;
                        m.landTile.Y = y;
                        m.landTile.Z = (sbyte)myMap4Reader.ReadByte();
                    }
                    m.staticTiles = GetStaticTiles(4, x, y);
                    mapInfo = m;
                    return true;
                case 5:
                    if (myMap5Reader == null || x > myMap5XLimit || y > myMap5YLimit)
                    {
                        mapInfo = null;
                        return false;
                    }
                    blockIndex = x / 8 * 512 + y / 8;
                    lock (myMap5Reader)
                    {
                        myMap5Reader.Seek(blockIndex * 196 + 4 + (cellY * 24 + cellX * 3), SeekOrigin.Begin);
                        m.landTile = new LandTile(TileData.GetLandTile(myMap5Reader.ReadByte() | myMap5Reader.ReadByte() << 8));
                        m.landTile.X = x;
                        m.landTile.Y = y;
                        m.landTile.Z = (sbyte)myMap5Reader.ReadByte();
                    }
                    m.staticTiles = GetStaticTiles(5, x, y);
                    mapInfo = m;
                    return true;
            }
            mapInfo = null;
            return false;
        }

    }
}