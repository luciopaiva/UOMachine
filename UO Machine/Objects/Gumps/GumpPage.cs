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

using UOMachine.Utility;
using System;
using System.Collections.Generic;
using System.Threading;

namespace UOMachine
{
    public sealed class GumpPage
    {
        private Gump myParentGump;
        public Gump ParentGump
        {
            get { return myParentGump; }
            internal set { myParentGump = value; }
        }

        private int myPage;
        public int Page
        {
            get { return myPage; }
            internal set { myPage = value; }
        }

        private GumpElement[] myGumpElements;
        public GumpElement[] GumpElements
        {
            get { return myGumpElements; }
            internal set { myGumpElements = value; }
        }

        /// <summary>
        /// Get array of GumpElements which match the specified ElementType.
        /// </summary>
        public GumpElement[] GetElementsByType(ElementType type)
        {
            List<GumpElement> elementList = new List<GumpElement>();
            foreach (GumpElement ge in myGumpElements)
            {
                if (ge.Type == type)
                {
                    elementList.Add(ge);
                }
            }
            return elementList.ToArray();
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
            bool found;
            foreach (GumpElement ge in myGumpElements)
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

        /// <summary>
        /// Get nearest GumpElement from source.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(GumpElement source, out GumpElement element)
        {
            GumpElement nearest = null;
            double closest = 0, distance;
            foreach (GumpElement ge in myGumpElements)
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
    }
}