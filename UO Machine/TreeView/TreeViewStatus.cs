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

namespace UOMachine.Tree
{
    internal sealed class TreeStatus
    {
        public TreeStatus[] StatusList;
        public bool IsExpanded;
        public TreeStatus(TreeViewItem treeViewItem)
        {
            this.IsExpanded = treeViewItem.IsExpanded;
            this.StatusList = new TreeStatus[treeViewItem.Items.Count];
        }
    }

    internal static class TreeViewStatus
    {
        private static void SaveStatus(TreeViewItem treeViewItem, TreeStatus treeStatus)
        {
            treeStatus.IsExpanded = treeViewItem.IsExpanded;
            for (int x = 0; x < treeViewItem.Items.Count; x++)
            {
                TreeViewItem t = treeViewItem.Items[x] as TreeViewItem;
                if (t != null)
                {
                    TreeStatus ts = new TreeStatus(t);
                    treeStatus.StatusList[x] = ts;
                    SaveStatus(t, ts);
                }
            }
        }

        public static TreeStatus Save(TreeViewItem treeViewItem)
        {
            if (treeViewItem == null) return null;
            TreeStatus status = new TreeStatus(treeViewItem);
            SaveStatus(treeViewItem, status);
            return status;
        }

        public static void Restore(TreeViewItem treeViewItem, TreeStatus treeStatus)
        {
            treeViewItem.IsExpanded = treeStatus.IsExpanded;
            for (int x = 0; x < treeStatus.StatusList.Length; x++)
            {
                TreeViewItem t = treeViewItem.Items[x] as TreeViewItem;
                if (t != null)
                {
                    t.IsExpanded = treeStatus.StatusList[x].IsExpanded;
                    Restore(t, treeStatus.StatusList[x]);
                }
            }
        }
    }
}