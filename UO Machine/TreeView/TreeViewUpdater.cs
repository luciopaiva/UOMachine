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
using System.Windows.Controls;
using UOMachine.Utility;
using System.Threading;
using System.Text;

namespace UOMachine.Tree
{
    internal sealed class NodeEditCache
    {
        public int ParentTag;
        public int Child;
        public int SubChild;
        public string Header;

        public NodeEditCache(int parentTag, int child, int subChild, string header)
        {
            this.ParentTag = parentTag;
            this.Child = child;
            this.SubChild = subChild;
            this.Header = header;
        }
    }

    internal static class TreeViewUpdater
    {
        private static TreeView myTreeView;
        private delegate void dEditPlayerNode(int parentTag, PlayerMobile player);
        private delegate void dEditNodeHeader(int parentTag, int child, int subChild, string header);
        private delegate void dAddPlaceHolder(int client, int tag);
        private delegate void dRemoveParent(int tag);
        private delegate void dUpdateLastTarget(int parentTag, int targetType, bool checkCrime, int serial, int x, int y, int z, int ID);
        private delegate void dUpdateLastObject(int parentTag, WorldItem worldItem);
        private delegate void dUpdateMouse(int parentTag, int x, int y);
        private delegate void dUpdateKeypress(int parentTag, System.Windows.Forms.Keys key);
        private delegate void dUpdateLastGumpAction(int parentTag, int serial, int gumpID, int buttonID, int[] switches);

        private static dEditPlayerNode myEditPlayerNodeDelegate;
        private static dEditNodeHeader myEditNodeHeaderDelegate;
        private static dAddPlaceHolder myAddPlaceHolderDelegate;
        private static dRemoveParent myRemoveParentDelegate;
        private static dUpdateLastTarget myUpdateLastTargetDelegate;
        private static dUpdateLastObject myUpdateLastObjectDelegate;
        private static dUpdateMouse myUpdateMouseDelegate;
        private static dUpdateKeypress myUpdateKeypressDelegate;
        private static dUpdateLastGumpAction myUpdateLastGumpActionDelegate;

        public static void Initialize(TreeView targetTreeView)
        {
            myTreeView = targetTreeView;
            myEditPlayerNodeDelegate = new dEditPlayerNode(myEditPlayerNode);
            myEditNodeHeaderDelegate = new dEditNodeHeader(myEditNodeHeader);
            myAddPlaceHolderDelegate = new dAddPlaceHolder(myAddPlaceHolder);
            myRemoveParentDelegate = new dRemoveParent(myRemoveParent);
            myUpdateLastTargetDelegate = new dUpdateLastTarget(myUpdateLastTarget);
            myUpdateLastObjectDelegate = new dUpdateLastObject(myUpdateLastObject);
            myUpdateMouseDelegate = new dUpdateMouse(myUpdateMouse);
            myUpdateKeypressDelegate = new dUpdateKeypress(myUpdateKeypress);
            myUpdateLastGumpActionDelegate = new dUpdateLastGumpAction(myUpdateLastGumpAction);
        }

        private static TreeViewItem GetParentByTag(int tag)
        {
            lock (myTreeView)
            {
                foreach (object o in myTreeView.Items)
                {
                    TreeViewItem t = o as TreeViewItem;
                    if (t != null && (int)t.Tag == tag) return t;
                }
            }
            return null;
        }

        private static void myUpdateLastGumpAction(int parentTag, int serial, int gumpID, int buttonID, int[] switches)
        {
            TreeViewItem t = GetParentByTag(parentTag);
            lock (myTreeView)
            {
                if (t != null)
                {
                    TreeViewItem lastActions = t.Items[1] as TreeViewItem;
                    if (lastActions != null)
                    {
                        TreeViewItem lastGumpAction = lastActions.Items[2] as TreeViewItem;
                        if (lastGumpAction != null)
                        {
                            TreeViewItem newLastGumpAction = new TreeViewItem();
                            newLastGumpAction.IsExpanded = lastGumpAction.IsExpanded;
                            newLastGumpAction.Header = "Last gump action";
                            newLastGumpAction.Items.Add("Gump serial = " + serial);
                            newLastGumpAction.Items.Add("Gump ID = " + gumpID);
                            newLastGumpAction.Items.Add("Button ID = " + buttonID);
                            if (switches.Length == 0)
                                newLastGumpAction.Items.Add("Switch IDs = 0");
                            else
                            {
                                StringBuilder sb = new StringBuilder();
                                for (int x = 0; x < switches.Length; x++)
                                {
                                    sb.Append(' ');
                                    sb.Append(switches[x]);
                                }
                                newLastGumpAction.Items.Add("Switch IDs =" + sb.ToString());
                            }
                            lastActions.Items[2] = newLastGumpAction;
                            lastActions.Items.Refresh();
                        }
                    }
                }
            }
        }

        public static void UpdateLastGumpAction(int parentTag, int serial, int gumpID, int buttonID, int[] switches)
        {
            if (myTreeView.CheckAccess()) myUpdateLastGumpAction(parentTag, serial, gumpID, buttonID, switches);
            else myTreeView.Dispatcher.BeginInvoke(myUpdateLastGumpActionDelegate, new object[] { parentTag, serial, gumpID, buttonID, switches });
        }

        private static void myUpdateLastObject(int parentTag, WorldItem worldItem)
        {
            TreeViewItem t = GetParentByTag(parentTag);
            lock (myTreeView)
            {
                if (t != null)
                {
                    TreeViewItem lastActions = t.Items[1] as TreeViewItem;
                    if (lastActions != null)
                    {
                        TreeViewItem lastObject = lastActions.Items[0] as TreeViewItem;
                        if (lastObject != null)
                        {
                            TreeViewItem newLastObject = new TreeViewItem();
                            TreeViewItem oldLastObject = lastActions.Items[0] as TreeViewItem;
                            newLastObject.Header = "Last object";
                            newLastObject.IsExpanded = lastObject.IsExpanded;
                            newLastObject.Items.Add("Serial = " + worldItem.Serial);
                            newLastObject.Items.Add("ID = " + worldItem.ID);
                            if (!string.IsNullOrEmpty(worldItem.Name)) newLastObject.Items.Add("Name = " + worldItem.Name);
                            newLastObject.Items.Add("X = " + worldItem.X);
                            newLastObject.Items.Add("Y = " + worldItem.Y);
                            newLastObject.Items.Add("Z = " + worldItem.Z);
                            lastActions.Items[0] = newLastObject;
                            lastActions.Items.Refresh();
                        }
                    }
                }
            }
        }

        public static void UpdateLastObject(int parentTag, WorldItem worldItem)
        {
            if (myTreeView.CheckAccess()) myUpdateLastObject(parentTag, worldItem);
            else myTreeView.Dispatcher.BeginInvoke(myUpdateLastObjectDelegate, new object[] { parentTag, worldItem });
        }

        private static void myUpdateLastTarget(int parentTag, int targetType, bool checkCrime, int serial, int x, int y, int z, int ID)
        {
            TreeViewItem t = GetParentByTag(parentTag);
            lock (myTreeView)
            {
                if (t != null)
                {
                    TreeViewItem lastActions = t.Items[1] as TreeViewItem;
                    if (lastActions != null)
                    {
                        TreeViewItem lastTarget = lastActions.Items[1] as TreeViewItem;
                        if (lastTarget != null)
                        {
                            string cliloc = UOMachine.Data.Cliloc.GetProperty(ID + 1020000);
                            TreeViewItem newLastTarget = new TreeViewItem();
                            newLastTarget.Header = "Last target";
                            newLastTarget.IsExpanded = lastTarget.IsExpanded;
                            newLastTarget.Items.Add("Target type = " + targetType);
                            newLastTarget.Items.Add("Check crime = " + checkCrime);
                            newLastTarget.Items.Add("Serial = " + serial);
                            newLastTarget.Items.Add("Cliloc = " + cliloc);
                            newLastTarget.Items.Add("ID = " + ID);
                            newLastTarget.Items.Add("X = " + x);
                            newLastTarget.Items.Add("Y = " + y);
                            newLastTarget.Items.Add("Z = " + z);
                            lastActions.Items[1] = newLastTarget;
                            lastActions.Items.Refresh();
                        }

                    }
                }
            }
        }

        public static void UpdateLastTarget(int parentTag, int targetType, bool checkCrime, int serial, int x, int y, int z, int ID)
        {
            if (myTreeView.CheckAccess()) myUpdateLastTarget(parentTag, targetType, checkCrime, serial, x, y, z, ID);
            else myTreeView.Dispatcher.BeginInvoke(myUpdateLastTargetDelegate, new object[] { parentTag, targetType, checkCrime, serial, x, y, z, ID });
        }

        private static void myUpdateMouse(int parentTag, int x, int y)
        {
            TreeViewItem t = GetParentByTag(parentTag);
            lock (myTreeView)
            {
                if (t != null)
                {
                    TreeViewItem misc = t.Items[2] as TreeViewItem;
                    if (misc != null)
                    {
                        TreeViewItem mouseLoc = misc.Items[1] as TreeViewItem;
                        if (mouseLoc != null) mouseLoc.Header = "Mouse location = " + x + " ," + y;
                    }
                }
            }
        }

        public static void UpdateMouse(int parentTag, int x, int y)
        {
            if (myTreeView.CheckAccess()) myUpdateMouse(parentTag, x, y);
            else myTreeView.Dispatcher.BeginInvoke(myUpdateMouseDelegate, new object[] { parentTag, x, y });
        }

        private static void myUpdateKeypress(int parentTag, System.Windows.Forms.Keys key)
        {
            TreeViewItem t = GetParentByTag(parentTag);
            lock (myTreeView)
            {
                if (t != null)
                {
                    TreeViewItem misc = t.Items[2] as TreeViewItem;
                    if (misc != null)
                    {
                        TreeViewItem keyItem = misc.Items[0] as TreeViewItem;
                        if (keyItem != null) keyItem.Header = "Last keypress = " + key.ToString();
                    }
                }
            }
        }

        public static void UpdateKeypress(int parentTag, System.Windows.Forms.Keys key)
        {
            if (myTreeView.CheckAccess()) myUpdateKeypress(parentTag, key);
            else myTreeView.Dispatcher.BeginInvoke(myUpdateKeypressDelegate, new object[] { parentTag, key });
        }

        private static void myRemoveParent(int tag)
        {
            lock (myTreeView)
            {
                for (int x = 0; x < myTreeView.Items.Count; x++)
                {
                    TreeViewItem parent = myTreeView.Items[x] as TreeViewItem;
                    if (parent != null && (int)parent.Tag == tag)
                    {
                        myTreeView.Items.RemoveAt(x);
                        myTreeView.Items.Refresh();
                        return;
                    }
                }
            }
        }

        public static void RemoveParent(int tag)
        {
            if (myTreeView.CheckAccess()) myRemoveParent(tag);
            else myTreeView.Dispatcher.BeginInvoke(myRemoveParentDelegate, new object[] { tag });
        }

        private static void myEditPlayerNode(int parentTag, PlayerMobile player)
        {
            lock (myTreeView)
            {
                TreeViewItem parentNode = (TreeViewItem)myTreeView.Items[parentTag];
                TreeViewItem playerNode = NodeConstructor.Create(player);
                TreeViewItem currentPlayerNode = (TreeViewItem)parentNode.Items[0];
                playerNode.IsExpanded = currentPlayerNode.IsExpanded;
                //TreeViewStatus.Save((TreeViewItem)parentNode.Items[0]);
                //TreeViewStatus.Restore(playerNode);
                parentNode.Items[0] = playerNode;
                parentNode.Items.Refresh();
            }
        }

        internal static void EditPlayerNode(int parentTag, PlayerMobile player)
        {
            if (myTreeView.CheckAccess()) myEditPlayerNode(parentTag, player);
            else myTreeView.Dispatcher.BeginInvoke(myEditPlayerNodeDelegate, new object[] { parentTag, player });
        }

        private static void myEditNodeHeader(int parentTag, int child, int subChild, string header)
        {
            lock (myTreeView)
            {
                TreeViewItem parentNode = GetParentByTag(parentTag);
                if (parentNode != null)
                {
                    if (parentNode.Items.Count - 1 >= child)
                    {
                        TreeViewItem childNode = parentNode.Items[child] as TreeViewItem;
                        if (childNode != null)
                        {
                            if (childNode.Items.Count - 1 >= subChild)
                            {
                                TreeViewItem subChildNode = childNode.Items[subChild] as TreeViewItem;
                                if (subChildNode != null)
                                {
                                    subChildNode.Header = header;
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static void EditNodeHeader(int parentTag, int child, int subChild, string header)
        {
            if (myTreeView.CheckAccess()) myEditNodeHeader(parentTag, child, subChild, header);
            else myTreeView.Dispatcher.BeginInvoke(myEditNodeHeaderDelegate, new object[] { parentTag, child, subChild, header });
        }

        private static void myAddPlaceHolder(int client, int tag)
        {
            TreeViewItem t = new TreeViewItem();
            t.Header = "Client " + client;
            TreeViewItem player = new TreeViewItem();
            player.Header = "Player";
            TreeViewItem lastActions = new TreeViewItem();
            lastActions.Header = "Last actions";
            TreeViewItem lastObject = new TreeViewItem();
            lastObject.Header = "Last object";
            lastObject.Items.Add("Serial = 0");
            lastObject.Items.Add("ID = 0");
            lastObject.Items.Add("Name =");
            lastObject.Items.Add("X = 0");
            lastObject.Items.Add("Y = 0");
            lastObject.Items.Add("Z = 0");
            TreeViewItem lastTarget = new TreeViewItem();
            lastTarget.Header = "Last target";
            lastTarget.Items.Add("Target type = 0");
            lastTarget.Items.Add("Check crime = false");
            lastTarget.Items.Add("Serial = 0");
            lastTarget.Items.Add("Cliloc = Not found!");
            lastTarget.Items.Add("ID = 0");
            lastTarget.Items.Add("X = 0");
            lastTarget.Items.Add("Y = 0");
            lastTarget.Items.Add("Z = 0");

            TreeViewItem lastGump = new TreeViewItem();
            lastGump.Header = "Last gump action";
            lastGump.Items.Add("Gump serial = 0");
            lastGump.Items.Add("Gump ID = 0");
            lastGump.Items.Add("Button ID = 0");
            lastGump.Items.Add("Switch IDs = 0");

            lastActions.Items.Add(lastObject);
            lastActions.Items.Add(lastTarget);
            lastActions.Items.Add(lastGump);

            TreeViewItem status = new TreeViewItem();
            status.Header = "Status";
            TreeViewItem trackedItems = new TreeViewItem();
            TreeViewItem trackedMobs = new TreeViewItem();
            TreeViewItem customGumps = new TreeViewItem();
            customGumps.Header = "Custom gumps = 0";
            trackedItems.Header = "Tracked items = 0";
            trackedMobs.Header = "Tracked mobiles = 0";
            status.Items.Add(trackedItems);
            status.Items.Add(trackedMobs);
            status.Items.Add(customGumps);

            TreeViewItem misc = new TreeViewItem();
            misc.Header = "Misc";
            TreeViewItem mouse = new TreeViewItem();
            TreeViewItem key = new TreeViewItem();
            key.Header = "Last keypress = N/A";
            mouse.Header = "Mouse location = 0, 0";
            misc.Items.Add(key);
            misc.Items.Add(mouse);

            t.Tag = tag;
            t.Items.Add(player);
            t.Items.Add(lastActions);
            t.Items.Add(misc);
            t.Items.Add(status);

            lock (myTreeView)
            {
                myTreeView.Items.Add(t);
                myTreeView.Items.Refresh();
            }
        }

        internal static void AddPlaceHolder(int parent, int tag)
        {
            if (myTreeView.CheckAccess()) myAddPlaceHolder(parent, tag);
            else myTreeView.Dispatcher.BeginInvoke(myAddPlaceHolderDelegate, new object[] { parent, tag });
        }

    }
}