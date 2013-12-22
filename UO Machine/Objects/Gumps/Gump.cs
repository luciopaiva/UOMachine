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
using UOMachine.Utility;

namespace UOMachine
{
    public sealed class Gump
    {
        public readonly string Layout;
        public readonly string[] Strings;
        public readonly GumpPage[] Pages;
        public readonly GumpElement[] GumpElements;
        public readonly int Client;
        public readonly int ID;
        public readonly int Serial;
        public readonly int X;
        public readonly int Y;

        private bool myClosable;
        public bool Closable
        {
            get { return myClosable; }
            internal set { myClosable = value; }
        }

        private bool myResizable;
        public bool Resizable
        {
            get { return myResizable; }
            internal set { myResizable = value; }
        }

        private bool myDisposable;
        public bool Disposable
        {
            get { return myDisposable; }
            internal set { myDisposable = value; }
        }

        private bool myMovable;
        public bool Movable
        {
            get { return myMovable; }
            internal set { myMovable = value; }
        }

        public Gump(int client, int x, int y, int ID, int serial, string layout, string[] strings, GumpElement[] elements, GumpPage[] pages)
        {
            this.Client = client;
            this.X = x;
            this.Y = y;
            this.ID = ID;
            this.Serial = serial;
            this.Layout = layout;
            this.Strings = strings;
            this.GumpElements = elements;
            this.Pages = pages;
            foreach (GumpPage gp in pages)
            {
                gp.ParentGump = this;
            }
        }

        /// <summary>
        /// Get array of GumpElements which match the specified ElementType from all pages.
        /// </summary>
        public GumpElement[] GetElementsByType(ElementType type)
        {
            List<GumpElement> elementList = new List<GumpElement>();
            if (this.GumpElements != null)
            {
                foreach (GumpElement ge in this.GumpElements)
                {
                    if (ge.Type == type)
                    {
                        elementList.Add(ge);
                    }
                }
            }

            if (this.Pages != null)
            {
                foreach (GumpPage p in this.Pages)
                {
                    foreach (GumpElement ge in p.GumpElements)
                    {
                        if (ge.Type == type)
                        {
                            elementList.Add(ge);
                        }
                    }
                }
            }
            return elementList.ToArray();
        }

        /// <summary>
        /// Get the GumpElement with the specified ID.  Searches all pages/elements.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool GetElementByID(int ID, out GumpElement element)
        {
            if (this.GumpElements != null)
            {
                foreach (GumpElement ge in this.GumpElements)
                {
                    if (ge.ElementID == ID)
                    {
                        element = ge;
                        return true;
                    }
                }
            }

            if (this.Pages != null)
            {
                foreach (GumpPage p in this.Pages)
                {
                    foreach (GumpElement ge in p.GumpElements)
                    {
                        if (ge.ElementID == ID)
                        {
                            element = ge;
                            return true;
                        }
                    }
                }
            }

            element = null;
            return false;
        }

        /// <summary>
        /// Get the GumpElement nearest to the specified GumpElement.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(GumpElement source, out GumpElement element)
        {
            GumpElement nearest = null;
            double closest = 0, distance;
            if (source.ParentPage != null)
            {
                return source.ParentPage.GetNearestElement(source, out element);
            }

            foreach (GumpElement ge in this.GumpElements)
            {
                if (ge == source) continue;
                distance = UOMath.Distance(source.X, source.Y, ge.X, ge.Y);
                if (nearest == null)
                {
                    closest = distance;
                    nearest = ge;
                }
                else
                {
                    if (distance < closest)
                    {
                        closest = distance;
                        nearest = ge;
                    }
                }
            }
            element = nearest;
            return nearest != null;
        }

        /// <summary>
        /// Get nearest GumpElement to source, but only if it's ElementType is contained in the include list.
        /// </summary>
        /// <param name="includeTypes">Array of ElementTypes which specifies valid GumpElements to search.</param>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(GumpElement source, ElementType[] includeTypes, out GumpElement element)
        {
            GumpElement nearest = null;
            double closest = 0, distance;
            if (source.ParentPage != null)
            {
                return source.ParentPage.GetNearestElement(source, includeTypes, out element);
            }
            bool found;
            foreach (GumpElement ge in this.GumpElements)
            {
                if (ge == source) continue;
                found = false;
                foreach (ElementType et in includeTypes)
                {
                    if (ge.Type == et)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) continue;
                distance = UOMath.Distance(source.X, source.Y, ge.X, ge.Y);
                if (nearest == null)
                {
                    closest = distance;
                    nearest = ge;
                }
                else
                {
                    if (distance < closest)
                    {
                        closest = distance;
                        nearest = ge;
                    }
                }
            }
            element = nearest;
            return nearest != null;
        }

        public void Close()
        {
            Macros.MacroEx.CloseClientGump(this.Client, this.ID);
            Macros.MacroEx.GumpButtonClick(this.Client, this.Serial, this.ID, 0);
        }

        public override int GetHashCode()
        {
            return (int)CRC32.GetHash((uint)this.ID, this.Layout);
        }
    }
}