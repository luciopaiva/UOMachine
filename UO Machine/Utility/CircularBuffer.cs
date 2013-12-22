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

namespace UOMachine.Utility
{
    /* NOTE: This class forces thread safety on every access to make it more user-friendly
     * in scripts at the expense of a bit of performance.  Obviously if you use this code
     * elsewhere you would want to implement the thread-safety yourself. */

    /// <summary>
    /// Thread-safe generic circular buffer (not a queue).  Read position is remembered.
    /// </summary>
    public sealed class CircularBuffer<T>
    {
        private object mySyncRoot;
        private int myCapacity, mySize, myHead, myTail, myReadOffset;
        private T[] myBuffer;

        public CircularBuffer(int capacity)
        {
            myCapacity = capacity;
            mySize = 0;
            myHead = 0;
            myTail = 0;
            myReadOffset = 0;
            myBuffer = new T[capacity + 1];
            mySyncRoot = new object();
        }

        /// <summary>
        /// Get or set buffer capacity. Buffer can only grow.
        /// </summary>
        public int Capacity
        {
            get { return myCapacity; }
            set
            {
                lock (mySyncRoot)
                {
                    if (value > myCapacity)
                    {
                        T[] newBuffer = new T[value + 1];
                        T[] oldBuffer = this.GetBuffer();
                        Array.Copy(oldBuffer, 0, newBuffer, 0, oldBuffer.Length);
                        myBuffer = newBuffer;
                        myCapacity = value;
                        myHead = 0;
                        myTail = mySize;
                    }
                }
            }
        }

        /// <summary>
        /// Get number of elements actually contained in buffer.
        /// </summary>
        public int Count
        {
            get { return mySize; }
        }

        /// <summary>
        /// Reset buffer.
        /// </summary>
        public void Clear()
        {
            lock (mySyncRoot)
            {
                mySize = 0;
                myTail = 0;
                myHead = 0;
                myReadOffset = 0;
            }
        }

        /// <summary>
        /// Change read position to specified number of indices from the beginning.
        /// </summary>
        /// <param name="count"></param>
        public void Seek(int count)
        {
            lock (mySyncRoot)
            {
                myReadOffset = Math.Min(count, mySize);
            }
        }

        /// <summary>
        /// Write item to buffer.
        /// </summary>
        public void Write(T item)
        {
            lock (mySyncRoot)
            {
                myBuffer[myTail] = item;
                myTail = ++myTail % myBuffer.Length;
                if (myTail == myHead)
                {
                    myHead = ++myHead % myBuffer.Length;
                    myReadOffset = Math.Max(0, myReadOffset - 1);
                }
                mySize++;
                mySize = Math.Min(mySize, myCapacity);
            }
        }

        /// <summary>
        /// Read item from buffer and increment read position.  Item remains in buffer until overwritten.
        /// </summary>
        /// <returns>True on success, false if nothing new to read.</returns>
        public bool Read(out T item)
        {
            lock (mySyncRoot)
            {
                int readPos = (myHead + myReadOffset) % myBuffer.Length;
                if (readPos == myTail)
                {
                    item = default(T);
                    return false;
                }
                myReadOffset = Math.Min(myReadOffset + 1, mySize);
                item = myBuffer[readPos];
                return true;
            }
        }

        /// <summary>
        /// Get array of elements contained in buffer.
        /// </summary>
        public T[] GetBuffer()
        {
            lock (mySyncRoot)
            {
                T[] buffer = new T[mySize];
                for (int x = 0; x < mySize; x++)
                {
                    buffer[x] = myBuffer[(myHead + x) % myBuffer.Length];
                }
                return buffer;
            }
        }

    }
}