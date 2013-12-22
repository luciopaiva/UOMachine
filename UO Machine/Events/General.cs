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
using UOMachine.Utility;

namespace UOMachine.Events
{
    /// <summary>
    /// Collection of thread safe events which provide notifications for general events.
    /// </summary>
    public static class General
    {
        public delegate void dJournalEntry(int client, JournalEntry journalEntry);
        public delegate void dGenericGump(int client, Gump gump);

        private static object myCustomGumpLock = new object();
        private static event dGenericGump myGenericGumpEvent;
        public static event dGenericGump GenericGumpEvent
        {
            add { lock (myCustomGumpLock) { myGenericGumpEvent += value; } }
            remove { lock (myCustomGumpLock) { myGenericGumpEvent -= value; } }
        }

        private static object myJournalEntryLock = new object();
        private static event dJournalEntry myJournalEntryEvent;
        public static event dJournalEntry JournalEntryEvent
        {
            add { lock (myJournalEntryLock) { myJournalEntryEvent += value; } }
            remove { lock (myJournalEntryLock) { myJournalEntryEvent -= value; } }
        }

        /// <summary>
        /// Reset all public events.
        /// </summary>
        public static void ClearEvents()
        {
            lock (myJournalEntryLock) { myJournalEntryEvent = null; }
            lock (myCustomGumpLock) { myGenericGumpEvent = null; }
        }

        internal static void OnJournalEntry(int client, JournalEntry journalEntry)
        {
            lock (myJournalEntryLock)
            {
                dJournalEntry handler = myJournalEntryEvent;
                if (handler != null) handler(client, journalEntry);
            }
        }

        internal static void OnCustomGump(int client, Gump gump)
        {
            lock (myCustomGumpLock)
            {
                dGenericGump handler = myGenericGumpEvent;
                try
                {
                if (handler != null) handler(client, gump);
            }
                catch (Exception e) { }
            }
        }
    }
}