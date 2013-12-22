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
using System.Threading;

namespace UOMachine
{
    public sealed class GumpElement
    {
        public Gump ParentGump
        {
            get
            {
                if (myParentPage != null)
                    return myParentPage.ParentGump;
                return null;
            }
        }

        private GumpPage myParentPage;
        public GumpPage ParentPage
        {
            get { return myParentPage; }
            internal set { myParentPage = value; }
        }

        private ElementType myType;
        public ElementType Type
        {
            get { return myType; }
            internal set { myType = value; }
        }

        private int myX;
        public int X
        {
            get { return myX; }
            internal set { myX = value; }
        }

        private int myY;
        public int Y
        {
            get { return myY; }
            internal set { myY = value; }
        }

        private int myElementID;
        public int ElementID
        {
            get { return myElementID; }
            internal set { myElementID = value; }
        }

        private int myInactiveID;
        public int InactiveID
        {
            get { return myInactiveID; }
            internal set { myInactiveID = value; }
        }

        private int myActiveID;
        public int ActiveID
        {
            get { return myActiveID; }
            internal set { myActiveID = value; }
        }

        private bool myInitialState;
        public bool InitialState
        {
            get { return myInitialState; }
            internal set { myInitialState = value; }
        }

        private int myButtonType;
        public int ButtonType
        {
            get { return myButtonType; }
            internal set { myButtonType = value; }
        }

        private int myParam;
        public int Param
        {
            get { return myParam; }
            internal set { myParam = value; }
        }

        private int myItemID;
        public int ItemID
        {
            get { return myItemID; }
            internal set { myItemID = value; }
        }

        private int myTooltip;
        public int Tooltip
        {
            get { return myTooltip; }
            internal set { myTooltip = value; }
        }

        private int myHue;
        public int Hue
        {
            get { return myHue; }
            internal set { myHue = value; }
        }

        private int myWidth;
        public int Width
        {
            get { return myWidth; }
            internal set { myWidth = value; }
        }

        private int myHeight;
        public int Height
        {
            get { return myHeight; }
            internal set { myHeight = value; }
        }

        private int mySize;
        public int Size
        {
            get { return mySize; }
            internal set { mySize = value; }
        }

        private int myCliloc;
        public int Cliloc
        {
            get { return myCliloc; }
            internal set { myCliloc = value; }
        }

        private bool myBackground;
        public bool Background
        {
            get { return myBackground; }
            internal set { myBackground = value; }
        }

        private bool myScrollBar;
        public bool ScrollBar
        {
            get { return myScrollBar; }
            internal set { myScrollBar = value; }
        }

        private string myText;
        public string Text
        {
            get { return myText; }
            internal set { myText = value; }
        }

        private string myArgs;
        public string Args
        {
            get { return myArgs; }
            internal set { myArgs = value; }
        }

        /// <summary>
        /// Get nearest GumpElement.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(out GumpElement element)
        {
            if (myParentPage != null)
            {
                return myParentPage.GetNearestElement(this, out element);
            }
            element = null;
            return false;
        }

        /// <summary>
        /// Get nearest GumpElement, but only if it's ElementType is contained in the include list.
        /// </summary>
        /// <param name="includeTypes">Array of ElementTypes which specifies valid GumpElements to search.</param>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(ElementType[] includeTypes, out GumpElement element)
        {
            if (myParentPage != null)
            {
                return myParentPage.GetNearestElement(this, includeTypes, out element);
            }
            element = null;
            return false;
        }

        public void Click()
        {
            Gump g = this.ParentGump;
            if (g != null && g.ID != 461 && myType == ElementType.button)
            {
                Macros.MacroEx.CloseClientGump(g.Client, g.ID);
                Macros.MacroEx.GumpButtonClick(g.Client, g.Serial, g.ID, this.ElementID);
            }
        }
    }
}