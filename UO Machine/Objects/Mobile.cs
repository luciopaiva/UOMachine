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
using System.Threading;
using System.Collections.Generic;
using UOMachine.Utility;

namespace UOMachine
{
    public class Mobile : WorldItem
    {
        public delegate void dStatusChanged();
        public event dStatusChanged StatusChangedEvent;

        private void OnStatusChanged()
        {
            dStatusChanged handler = StatusChangedEvent;
            if (handler != null) handler();

        }

        public Mobile(int serial, int parentClient) : base(serial)
        {
            myClient = parentClient;
        }

        // matches layer to serial
        internal int[] myLayerArray = new int[30];

        internal void SetLayer(int layer, int serial)
        {
            Interlocked.Exchange(ref myLayerArray[layer], serial);
        }

        internal int GetLayer(int layer)
        {
            return Thread.VolatileRead(ref myLayerArray[layer]);
        }

        internal int[] GetAllLayers()
        {
            return ThreadHelper.VolatileRead<int[]>(ref myLayerArray);
        }

        public bool GetEquippedItem(int layer, out Item item)
        {
            return ClientInfoCollection.GetItem(myClient, GetLayer(layer), out item);
        }

        public bool GetEquippedItem(Layer layer, out Item item)
        {
            return ClientInfoCollection.GetItem(myClient, GetLayer((int)layer), out item);
        }

        public Item[] GetEquippedItems()
        {
            List<Item> itemList = new List<Item>(29);
            int[] layerArray = this.GetAllLayers();
            Item i;
            for (int x = 0; x < layerArray.Length; x++)
            {
                if (layerArray[x] != 0)
                {
                    if (ClientInfoCollection.GetItem(myClient, layerArray[x], out i) && i.Layer != Layer.Invalid)
                        itemList.Add(i);
                }
            }
            return itemList.ToArray();
        }

        private int myClient;
        public virtual int Client
        {
            get { return Thread.VolatileRead(ref myClient); }
            internal set { myClient = value; }
        }

        internal int mySex;
        public virtual int Sex
        {
            get { return Thread.VolatileRead(ref mySex); }
            internal set { mySex = value; }
        }

        internal int myHealth = 0;
        public virtual int Health
        {
            get { return Thread.VolatileRead(ref myHealth); }
            internal set { myHealth = value; }
        }

        internal int myStamina = 0;
        public virtual int Stamina
        {
            get { return Thread.VolatileRead(ref myStamina); }
            internal set { myStamina = value; }
        }

        internal int myMana = 0;
        public virtual int Mana
        {
            get { return Thread.VolatileRead(ref myMana); }
            internal set { myMana = value; }
        }

        internal int myMaxHealth = 0;
        public virtual int MaxHealth
        {
            get { return Thread.VolatileRead(ref myMaxHealth); }
            internal set { myMaxHealth = value; }
        }

        internal int myMaxStamina = 0;
        public virtual int MaxStamina
        {
            get { return Thread.VolatileRead(ref myMaxStamina); }
            internal set { myMaxStamina = value; }
        }

        internal int myMaxMana = 0;
        public virtual int MaxMana
        {
            get { return Thread.VolatileRead(ref myMaxMana); }
            internal set { myMaxMana = value; }
        }

        internal int myStatus = 0;
        public virtual int Status
        {
            get { return Thread.VolatileRead(ref myStatus); }
            internal set { myStatus = value; }
        }

        internal int myNotoriety = 0;
        public virtual int Notoriety
        {
            get { return Thread.VolatileRead(ref myNotoriety); }
            internal set { myNotoriety = value; }
        }
    }
}