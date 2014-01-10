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
using UOMachine.IPC;
using UOMachine.Utility;
using UOMachine.Data;
using System.Threading;

namespace UOMachine.Events
{
    /// <summary>
    /// Collection of thread safe events which provide notifications for specific incoming packets.
    /// </summary>
    public static class IncomingPackets
    {
        public delegate void dPlayerInitialized(int client, PlayerMobile player);
        public delegate void dDamage(int client, int serial, int damage);
        public delegate void dHealthUpdated(int client, int serial, int maxHealth, int health);
        public delegate void dManaUpdated(int client, int serial, int maxMana, int mana);
        public delegate void dStaminaUpdated(int client, int serial, int maxStamina, int stamina);
        public delegate void dLocalizedText(int client, JournalEntry journalEntry);
        public delegate void dEquippedMobAdded(int client, Mobile mobile, ItemCollection equipment);
        public delegate void dMobileMoving(int client, int serial, int ID, int x, int y, int z, int direction, int hue, int status, int notoriety);
        public delegate void dContainerContents(int client, ItemCollection container);
        public delegate void dWorldItemAdded(int client, Item item);
        public delegate void dItemDeleted(int client, int serial);
        public delegate void dMobileUpdated(int client, int serial, int ID, int hue, int status, int x, int y, int z, int direction);
        public delegate void dMapChanged(int client, int map);
        public delegate void dItemAddedToContainer(int client, Item item);
        //public delegate void dProperties(int client, int serial, string name, string properties);
        public delegate void dProperties(int client, int serial, string name, Property[] properties, string propertyText);
        public delegate void dMoveRejected(int client, int sequence, int x, int y, int z, int direction);
        public delegate void dMoveAccepted(int client, int sequence, int status);
        public delegate void dShortStatus(int client, int serial, string name, int health, int maxHealth, int sex);
        public delegate void dLongStatus(int client, int serial, PlayerStatus playerStatus);
        public delegate void dSkillUpdate(int client, int skillID, float value, float baseValue, LockStatus lockStatus, float skillCap);
        public delegate void dSkillList(int client, SkillInfo[] skills);
        public delegate void dTarget(int client, int targetType);
        public delegate void dStatLockStatus(int client, int serial, int statLockStatus);
        public delegate void dStandardGump(int client, int serial, int ID);
        public delegate void dMobileDeath(int client, int mobileSerial, int corpseSerial);
        public delegate void dPlayerDeath(int client);
        public delegate void dMobileName(int client, int serial, string name);
        public delegate void dUnicodeText(int client, JournalEntry journalEntry);
        public delegate void dText(int client, JournalEntry journalEntry);
        public delegate void dPartyText(int client, JournalEntry journalEntry);
        public delegate void dContextMenu(int client, ContextEntry[] contextEntries);
        public delegate void dItemEquipped(int client, int serial, int ID, Layer layer, int mobileSerial, int hue);
        public delegate void dSound(int client, byte flags, int effect, int volume, int x, int y, int z);
        public delegate void dAttackTarget(int client, int serial);
        public delegate void dBondedStatus(int client, int serial, bool isDead);
        public delegate void dLoggedOut(int client);
        public delegate void dAttackGranted(int client, int serial);
        public delegate void dAttackEnded(int client);
        public delegate void dAttackSwing(int client, int attackerSerial, int defenderSerial);
        public delegate void dGenericGump(int client, int serial, int ID, int x, int y, string layout, string[] text);
        public delegate void dCloseGump(int client, int gumpID, int buttonID);
        public delegate void dServerList(int client, ServerInfo[] serverList);
        public delegate void dCharacterList(int client, string[] characterList);

        private static object myCharacterListLock = new object();
        private static event dCharacterList myCharacterListEvent;
        internal static event dCharacterList InternalCharacterListEvent;
        /// <summary>
        /// Fired when server asks client to close a gump.
        /// </summary>
        public static event dCharacterList CharacterListEvent
        {
            add { lock (myCharacterListLock) { myCharacterListEvent += value; } }
            remove { lock (myCharacterListLock) { myCharacterListEvent -= value; } }
        }

        private static object myServerListLock = new object();
        private static event dServerList myServerListEvent;
        internal static event dServerList InternalServerListEvent;
        /// <summary>
        /// Fired when server asks client to close a gump.
        /// </summary>
        public static event dServerList ServerListEvent
        {
            add { lock (myServerListLock) { myServerListEvent += value; } }
            remove { lock (myServerListLock) { myServerListEvent -= value; } }
        }

        private static object myCloseGumpLock = new object();
        private static event dCloseGump myCloseGumpEvent;
        internal static event dCloseGump InternalCloseGumpEvent;
        /// <summary>
        /// Fired when server asks client to close a gump.
        /// </summary>
        public static event dCloseGump CloseGumpEvent
        {
            add { lock (myCloseGumpLock) { myCloseGumpEvent += value; } }
            remove { lock (myCloseGumpLock) { myCloseGumpEvent -= value; } }
        }

        private static object myGenericGumpLock = new object();
        private static event dGenericGump myGenericGumpEvent;
        internal static event dGenericGump InternalGenericGumpEvent;
        /// <summary>
        /// Fired on receipt of compressed gump packet.
        /// </summary>
        public static event dGenericGump CustomGumpEvent
        {
            add { lock (myGenericGumpLock) { myGenericGumpEvent += value; } }
            remove { lock (myGenericGumpLock) { myGenericGumpEvent -= value; } }
        }

        private static object myAttackSwingLock = new object();
        private static event dAttackSwing myAttackSwingEvent;
        internal static event dAttackSwing InternalAttackSwingEvent;
        /// <summary>
        /// Fired on receipt of attack granted packet.
        /// </summary>
        public static event dAttackSwing AttackSwingEvent
        {
            add { lock (myAttackSwingLock) { myAttackSwingEvent += value; } }
            remove { lock (myAttackSwingLock) { myAttackSwingEvent -= value; } }
        }

        private static object myAttackEndedLock = new object();
        private static event dAttackEnded myAttackEndedEvent;
        internal static event dAttackEnded InternalAttackEndedEvent;
        /// <summary>
        /// Fired on receipt of attack granted packet.
        /// </summary>
        public static event dAttackEnded AttackEndedEvent
        {
            add { lock (myAttackEndedLock) { myAttackEndedEvent += value; } }
            remove { lock (myAttackEndedLock) { myAttackEndedEvent -= value; } }
        }

        private static object myAttackGrantedLock = new object();
        private static event dAttackGranted myAttackGrantedEvent;
        internal static event dAttackGranted InternalAttackGrantedEvent;
        /// <summary>
        /// Fired on receipt of attack granted packet.
        /// </summary>
        public static event dAttackGranted AttackGrantedEvent
        {
            add { lock (myAttackGrantedLock) { myAttackGrantedEvent += value; } }
            remove { lock (myAttackGrantedLock) { myAttackGrantedEvent -= value; } }
        }

        private static object myLoggedOutLock = new object();
        private static event dLoggedOut myLoggedOutEvent;
        internal static event dLoggedOut InternalLoggedOutEvent;
        /// <summary>
        /// Fired on receipt of logout confirmation packet.
        /// </summary>
        public static event dLoggedOut LoggedOutEvent
        {
            add { lock (myLoggedOutLock) { myLoggedOutEvent += value; } }
            remove { lock (myLoggedOutLock) { myLoggedOutEvent -= value; } }
        }

        private static object myBondedStatusLock = new object();
        private static event dBondedStatus myBondedStatusEvent;
        internal static event dBondedStatus InternalBondedStatusEvent;
        /// <summary>
        /// Fired on receipt of bonded status packet.
        /// </summary>
        public static event dBondedStatus BondedStatusEvent
        {
            add { lock (myBondedStatusLock) { myBondedStatusEvent += value; } }
            remove { lock (myBondedStatusLock) { myBondedStatusEvent -= value; } }
        }

        private static object myAttackTargetLock = new object();
        private static event dAttackTarget myAttackTargetEvent;
        internal static event dAttackTarget InternalAttackTargetEvent;
        /// <summary>
        /// Fired on receipt of current attack target packet.
        /// </summary>
        public static event dAttackTarget AttackTargetEvent
        {
            add { lock (myAttackTargetLock) { myAttackTargetEvent += value; } }
            remove { lock (myAttackTargetLock) { myAttackTargetEvent -= value; } }
        }

        private static object mySoundLock = new object();
        private static event dSound mySoundEvent;
        internal static event dSound InternalSoundEvent;
        /// <summary>
        /// Fired on receipt of sound packet.
        /// </summary>
        public static event dSound SoundEvent
        {
            add { lock (mySoundLock) { mySoundEvent += value; } }
            remove { lock (mySoundLock) { mySoundEvent -= value; } }
        }

        private static object myItemEquippedLock = new object();
        private static event dItemEquipped myItemEquippedEvent;
        internal static event dItemEquipped InternalItemEquippedEvent;
        /// <summary>
        /// Fired when player character equips an item.
        /// </summary>
        public static event dItemEquipped ItemEquippedEvent
        {
            add { lock (myItemEquippedLock) { myItemEquippedEvent += value; } }
            remove { lock (myItemEquippedLock) { myItemEquippedEvent -= value; } }
        }

        private static object myContextMenuLock = new object();
        private static event dContextMenu myContextMenuEvent;
        internal static event dContextMenu InternalContextMenuEvent;
        /// <summary>
        /// Fired on receipt of mobile's context menu.
        /// </summary>
        public static event dContextMenu ContextMenuEvent
        {
            add { lock (myContextMenuLock) { myContextMenuEvent += value; } }
            remove { lock (myContextMenuLock) { myContextMenuEvent -= value; } }
        }

        private static object myPartyTextLock = new object();
        private static event dPartyText myPartyTextEvent;
        internal static event dPartyText InternalPartyTextEvent;
        /// <summary>
        /// Fired on receipt of party chat.
        /// </summary>
        public static event dPartyText PartyTextEvent
        {
            add { lock (myPartyTextLock) { myPartyTextEvent += value; } }
            remove { lock (myPartyTextLock) { myPartyTextEvent -= value; } }
        }

        private static object myTextLock = new object();
        private static event dText myTextEvent;
        internal static event dText InternalTextEvent;
        /// <summary>
        /// Fired on receipt of ASCII text.
        /// </summary>
        public static event dText TextEvent
        {
            add { lock (myTextLock) { myTextEvent += value; } }
            remove { lock (myTextLock) { myTextEvent -= value; } }
        }

        private static object myUnicodeTextLock = new object();
        private static event dUnicodeText myUnicodeTextEvent;
        internal static event dUnicodeText InternalUnicodeTextEvent;
        /// <summary>
        /// Fired on receipt of unicode text, should include language.
        /// </summary>
        public static event dUnicodeText UnicodeTextEvent
        {
            add { lock (myUnicodeTextLock) { myUnicodeTextEvent += value; } }
            remove { lock (myUnicodeTextLock) { myUnicodeTextEvent -= value; } }
        }

        private static object myMobileNameLock = new object();
        private static event dMobileName myMobileNameEvent;
        internal static event dMobileName InternalMobileNameEvent;
        /// <summary>
        /// Fired on receipt of mobile name packet.
        /// </summary>
        public static event dMobileName MobileNameEvent
        {
            add { lock (myMobileNameLock) { myMobileNameEvent += value; } }
            remove { lock (myMobileNameLock) { myMobileNameEvent -= value; } }
        }

        private static object myPlayerDeathLock = new object();
        private static event dPlayerDeath myPlayerDeathEvent;
        internal static event dPlayerDeath InternalPlayerDeathEvent;
        /// <summary>
        /// Fired on death of player character.
        /// </summary>
        public static event dPlayerDeath PlayerDeathEvent
        {
            add { lock (myPlayerDeathLock) { myPlayerDeathEvent += value; } }
            remove { lock (myPlayerDeathLock) { myPlayerDeathEvent -= value; } }
        }

        private static object myMobileDeathLock = new object();
        private static event dMobileDeath myMobileDeathEvent;
        internal static event dMobileDeath InternalMobileDeathEvent;
        /// <summary>
        /// Fired on death of mobile.
        /// </summary>
        public static event dMobileDeath MobileDeathEvent
        {
            add { lock (myMobileDeathLock) { myMobileDeathEvent += value; } }
            remove { lock (myMobileDeathLock) { myMobileDeathEvent -= value; } }
        }

        private static object myStandardGumpLock = new object();
        private static event dStandardGump myStandardGumpEvent;
        internal static event dStandardGump InternalStandardGumpEvent;
        /// <summary>
        /// Fired on receipt of standard gump packet. (chests, etc.)
        /// </summary>
        public static event dStandardGump StandardGumpEvent
        {
            add { lock (myStandardGumpLock) { myStandardGumpEvent += value; } }
            remove { lock (myStandardGumpLock) { myStandardGumpEvent -= value; } }
        }

        private static object myStatLockStatusLock = new object();
        private static event dStatLockStatus myStatLockStatusEvent;
        internal static event dStatLockStatus InternalStatLockStatusEvent;
        /// <summary>
        /// Fired on receipt of stat lock status packet.
        /// </summary>
        public static event dStatLockStatus StatLockStatusEvent
        {
            add { lock (myStatLockStatusLock) { myStatLockStatusEvent += value; } }
            remove { lock (myStatLockStatusLock) { myStatLockStatusEvent -= value; } }
        }

        private static object myTargetLock = new object();
        private static event dTarget myTargetEvent;
        internal static event dTarget InternalTargetEvent;
        /// <summary>
        /// Fired on receipt of target packet which sets target cursor.
        /// </summary>
        public static event dTarget TargetEvent
        {
            add { lock (myTargetLock) { myTargetEvent += value; } }
            remove { lock (myTargetLock) { myTargetEvent -= value; } }
        }

        private static object mySkillListLock = new object();
        private static event dSkillList mySkillListEvent;
        internal static event dSkillList InternalSkillListEvent;
        /// <summary>
        /// Fired on receipt of skill list packet.
        /// </summary>
        public static event dSkillList SkillListEvent
        {
            add { lock (mySkillListLock) { mySkillListEvent += value; } }
            remove { lock (mySkillListLock) { mySkillListEvent -= value; } }
        }

        private static object mySkillUpdateLock = new object();
        private static event dSkillUpdate mySkillUpdateEvent;
        internal static event dSkillUpdate InternalSkillUpdateEvent;
        /// <summary>
        /// Fired on receipt of skill update packet.
        /// </summary>
        public static event dSkillUpdate SkillUpdateEvent
        {
            add { lock (mySkillUpdateLock) { mySkillUpdateEvent += value; } }
            remove { lock (mySkillUpdateLock) { mySkillUpdateEvent -= value; } }
        }

        private static object myLongStatusLock = new object();
        private static event dLongStatus myLongStatusEvent;
        internal static event dLongStatus InternalLongStatusEvent;
        /// <summary>
        /// Fired on receipt of extended status packet (player character).
        /// </summary>
        public static event dLongStatus LongStatusEvent
        {
            add { lock (myLongStatusLock) { myLongStatusEvent += value; } }
            remove { lock (myLongStatusLock) { myLongStatusEvent -= value; } }
        }

        private static object myShortStatusLock = new object();
        private static event dShortStatus myShortStatusEvent;
        internal static event dShortStatus InternalShortStatusEvent;
        /// <summary>
        /// Fired on receipt of basic status packet.
        /// </summary>
        public static event dShortStatus ShortStatusEvent
        {
            add { lock (myShortStatusLock) { myShortStatusEvent += value; } }
            remove { lock (myShortStatusLock) { myShortStatusEvent -= value; } }
        }

        private static object myMoveAcceptedLock = new object();
        private static event dMoveAccepted myMoveAcceptedEvent;
        internal static event dMoveAccepted InternalMoveAcceptedEvent;
        /// <summary>
        /// Fired on receipt of movement accepted packet.
        /// </summary>
        public static event dMoveAccepted MoveAcceptedEvent
        {
            add { lock (myMoveAcceptedLock) { myMoveAcceptedEvent += value; } }
            remove { lock (myMoveAcceptedLock) { myMoveAcceptedEvent -= value; } }
        }

        private static object myMoveRejectedLock = new object();
        private static event dMoveRejected myMoveRejectedEvent;
        internal static event dMoveRejected InternalMoveRejectedEvent;
        /// <summary>
        /// Fired on receipt of movement rejected packet.
        /// </summary>
        public static event dMoveRejected MoveRejectedEvent
        {
            add { lock (myMoveRejectedLock) { myMoveRejectedEvent += value; } }
            remove { lock (myMoveRejectedLock) { myMoveRejectedEvent -= value; } }
        }

        private static object myPropertiesLock = new object();
        private static event dProperties myPropertiesEvent;
        internal static event dProperties InternalPropertiesEvent;
        /// <summary>
        /// Fired on receipt of properties packet.
        /// </summary>
        public static event dProperties PropertiesEvent
        {
            add { lock (myPropertiesLock) { myPropertiesEvent += value; } }
            remove { lock (myPropertiesLock) { myPropertiesEvent -= value; } }
        }

        private static object myItemAddedToContainerLock = new object();
        private static event dItemAddedToContainer myItemAddedToContainerEvent;
        internal static event dItemAddedToContainer InternalItemAddedToContainerEvent;
        /// <summary>
        /// Fired when item is added to a container.
        /// </summary>
        public static event dItemAddedToContainer ItemAddedToContainerEvent
        {
            add { lock (myItemAddedToContainerLock) { myItemAddedToContainerEvent += value; } }
            remove { lock (myItemAddedToContainerLock) { myItemAddedToContainerEvent -= value; } }
        }

        private static object myMapChangedLock = new object();
        private static event dMapChanged myMapChangedEvent;
        internal static event dMapChanged InternalMapChangedEvent;
        /// <summary>
        /// Fired when player character is transported to a different map.
        /// </summary>
        public static event dMapChanged MapChangedEvent
        {
            add { lock (myMapChangedLock) { myMapChangedEvent += value; } }
            remove { lock (myMapChangedLock) { myMapChangedEvent -= value; } }
        }

        private static object myMobileUpdatedLock = new object();
        private static event dMobileUpdated myMobileUpdatedEvent;
        internal static event dMobileUpdated InternalMobileUpdatedEvent;
        /// <summary>
        /// Fired on receipt of mobile updated packet.
        /// </summary>
        public static event dMobileUpdated MobileUpdatedEvent
        {
            add { lock (myMobileUpdatedLock) { myMobileUpdatedEvent += value; } }
            remove { lock (myMobileUpdatedLock) { myMobileUpdatedEvent -= value; } }
        }

        private static object myItemDeletedLock = new object();
        private static event dItemDeleted myItemDeletedEvent;
        internal static event dItemDeleted InternalItemDeletedEvent;
        /// <summary>
        /// Fired on receipt of item deleted packet.
        /// </summary>
        public static event dItemDeleted ItemDeletedEvent
        {
            add { lock (myItemDeletedLock) { myItemDeletedEvent += value; } }
            remove { lock (myItemDeletedLock) { myItemDeletedEvent -= value; } }
        }

        private static object myWorldItemAddedLock = new object();
        private static event dWorldItemAdded myWorldItemAddedEvent;
        internal static event dWorldItemAdded InternalWorldItemAddedEvent;
        /// <summary>
        /// Fired on receipt of world item packet. (items on ground)
        /// </summary>
        public static event dWorldItemAdded WorldItemAddedEvent
        {
            add { lock (myWorldItemAddedLock) { myWorldItemAddedEvent += value; } }
            remove { lock (myWorldItemAddedLock) { myWorldItemAddedEvent -= value; } }
        }

        private static object myContainerContentsLock = new object();
        private static event dContainerContents myContainerContentsEvent;
        internal static event dContainerContents InternalContainerContentsEvent;
        /// <summary>
        /// Fired on receipt of container contents packet.
        /// </summary>
        public static event dContainerContents ContainerContentsEvent
        {
            add { lock (myContainerContentsLock) { myContainerContentsEvent += value; } }
            remove { lock (myContainerContentsLock) { myContainerContentsEvent -= value; } }
        }

        private static object myMobileMovingLock = new object();
        private static event dMobileMoving myMobileMovingEvent;
        internal static event dMobileMoving InternalMobileMovingEvent;
        /// <summary>
        /// Fired on receipt of mobile moving packet.
        /// </summary>
        public static event dMobileMoving MobileMovingEvent
        {
            add { lock (myMobileMovingLock) { myMobileMovingEvent += value; } }
            remove { lock (myMobileMovingLock) { myMobileMovingEvent -= value; } }
        }

        private static object myEquippedMobAddedLock = new object();
        private static event dEquippedMobAdded myEquippedMobAddedEvent;
        internal static event dEquippedMobAdded InternalEquippedMobAddedEvent;
        /// <summary>
        /// Fired on receipt of equipped mob packet.
        /// </summary>
        public static event dEquippedMobAdded EquippedMobAddedEvent
        {
            add { lock (myEquippedMobAddedLock) { myEquippedMobAddedEvent += value; } }
            remove { lock (myEquippedMobAddedLock) { myEquippedMobAddedEvent -= value; } }
        }

        private static object myLocalizedTextLock = new object();
        private static event dLocalizedText myLocalizedTextEvent;
        internal static event dLocalizedText InternalLocalizedTextEvent;
        /// <summary>
        /// Fired on receipt of localized text packet. Text is decoded according to cliloc.
        /// </summary>
        public static event dLocalizedText LocalizedTextEvent
        {
            add { lock (myLocalizedTextLock) { myLocalizedTextEvent += value; } }
            remove { lock (myLocalizedTextLock) { myLocalizedTextEvent -= value; } }
        }

        private static object myHealthUpdatedLock = new object();
        private static event dHealthUpdated myHealthUpdatedEvent;
        internal static event dHealthUpdated InternalHealthUpdatedEvent;
        /// <summary>
        /// Fired on receipt of health updated packet.
        /// </summary>
        public static event dHealthUpdated HealthUpdatedEvent
        {
            add { lock (myHealthUpdatedLock) { myHealthUpdatedEvent += value; } }
            remove { lock (myHealthUpdatedLock) { myHealthUpdatedEvent -= value; } }
        }

        private static object myManaUpdatedLock = new object();
        private static event dManaUpdated myManaUpdatedEvent;
        internal static event dManaUpdated InternalManaUpdatedEvent;
        /// <summary>
        /// Fired on receipt of mana updated packet.
        /// </summary>
        public static event dManaUpdated ManaUpdatedEvent
        {
            add { lock (myManaUpdatedLock) { myManaUpdatedEvent += value; } }
            remove { lock (myManaUpdatedLock) { myManaUpdatedEvent -= value; } }
        }

        private static object myStaminaUpdatedLock = new object();
        private static event dStaminaUpdated myStaminaUpdatedEvent;
        internal static event dStaminaUpdated InternalStaminaUpdatedEvent;
        /// <summary>
        /// Fired on receipt of stamina updated packet.
        /// </summary>
        public static event dStaminaUpdated StaminaUpdatedEvent
        {
            add { lock (myStaminaUpdatedLock) { myStaminaUpdatedEvent += value; } }
            remove { lock (myStaminaUpdatedLock) { myStaminaUpdatedEvent -= value; } }
        }

        private static object myDamageLock = new object();
        private static event dDamage myDamageEvent;
        internal static event dDamage InternalDamageEvent;
        /// <summary>
        /// Fired on receipt of damage packet.
        /// </summary>
        public static event dDamage DamageEvent
        {
            add { lock (myDamageLock) { myDamageEvent += value; } }
            remove { lock (myDamageLock) { myDamageEvent -= value; } }
        }

        private static object myPlayerInitializedLock = new object();
        private static event dPlayerInitialized myPlayerInitializedEvent;
        internal static event dPlayerInitialized InternalPlayerInitializedEvent;
        /// <summary>
        /// Fired on receipt of player initialized packet.
        /// </summary>
        public static event dPlayerInitialized PlayerInitializedEvent
        {
            add { lock (myPlayerInitializedLock) { myPlayerInitializedEvent += value; } }
            remove { lock (myPlayerInitializedLock) { myPlayerInitializedEvent -= value; } }
        }

        /// <summary>
        /// Reset all public events.
        /// </summary>
        public static void ClearEvents()
        {
            lock (myPlayerInitializedLock) { myPlayerInitializedEvent = null; }
            lock (myDamageLock) { myDamageEvent = null; }
            lock (myHealthUpdatedLock) { myHealthUpdatedEvent = null; }
            lock (myManaUpdatedLock) { myManaUpdatedEvent = null; }
            lock (myStaminaUpdatedLock) { myStaminaUpdatedEvent = null; }
            lock (myLocalizedTextLock) { myLocalizedTextEvent = null; }
            lock (myEquippedMobAddedLock) { myEquippedMobAddedEvent = null; }
            lock (myMobileMovingLock) { myMobileMovingEvent = null; }
            lock (myContainerContentsLock) { myContainerContentsEvent = null; }
            lock (myWorldItemAddedLock) { myWorldItemAddedEvent = null; }
            lock (myItemDeletedLock) { myItemDeletedEvent = null; }
            lock (myMobileUpdatedLock) { myMobileUpdatedEvent = null; }
            lock (myMapChangedLock) { myMapChangedEvent = null; }
            lock (myItemAddedToContainerLock) { myItemAddedToContainerEvent = null; }
            lock (myPropertiesLock) { myPropertiesEvent = null; }
            lock (myMoveRejectedLock) { myMoveRejectedEvent = null; }
            lock (myMoveAcceptedLock) { myMoveAcceptedEvent = null; }
            lock (myShortStatusLock) { myShortStatusEvent = null; }
            lock (myLongStatusLock) { myLongStatusEvent = null; }
            lock (mySkillUpdateLock) { mySkillUpdateEvent = null; }
            lock (mySkillListLock) { mySkillListEvent = null; }
            lock (myTargetLock) { myTargetEvent = null; }
            lock (myStatLockStatusLock) { myStatLockStatusEvent = null; }
            lock (myStandardGumpLock) { myStandardGumpEvent = null; }
            lock (myMobileDeathLock) { myMobileDeathEvent = null; }
            lock (myPlayerDeathLock) { myPlayerDeathEvent = null; }
            lock (myMobileNameLock) { myMobileNameEvent = null; }
            lock (myUnicodeTextLock) { myUnicodeTextEvent = null; }
            lock (myTextLock) { myTextEvent = null; }
            lock (myPartyTextLock) { myPartyTextEvent = null; }
            lock (myContextMenuLock) { myContextMenuEvent = null; }
            lock (myItemEquippedLock) { myItemEquippedEvent = null; }
            lock (mySoundLock) { mySoundEvent = null; }
            lock (myAttackTargetLock) { myAttackTargetEvent = null; }
            lock (myBondedStatusLock) { myBondedStatusEvent = null; }
            lock (myLoggedOutLock) { myLoggedOutEvent = null; }
            lock (myAttackGrantedLock) { myAttackGrantedEvent = null; }
            lock (myAttackEndedLock) { myAttackEndedEvent = null; }
            lock (myAttackSwingLock) { myAttackSwingEvent = null; }
            lock (myGenericGumpLock) { myGenericGumpEvent = null; }
            lock (myCloseGumpLock) { myCloseGumpEvent = null; }
            lock (myServerListLock) { myServerListEvent = null; }
            lock (myCharacterListLock) { myCharacterListEvent = null; }

        }

        internal static void OnPlayerInitialized(int client, PlayerMobile player)
        {
            dPlayerInitialized handler = InternalPlayerInitializedEvent;
            if (handler != null) handler(client, player);
            lock (myPlayerInitializedLock)
            {
                handler = myPlayerInitializedEvent;
                try { if (handler != null) handler(client, player); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnDamage(int client, int serial, int damage)
        {
            dDamage handler = InternalDamageEvent;
            if (handler != null) handler(client, serial, damage);
            lock (myDamageLock)
            {
                handler = myDamageEvent;
                try { if (handler != null) handler(client, serial, damage); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnHealthUpdated(int client, int serial, int maxHealth, int health)
        {
            dHealthUpdated handler = InternalHealthUpdatedEvent;
            if (handler != null) handler(client, serial, maxHealth, health);
            lock (myHealthUpdatedLock)
            {
                handler = myHealthUpdatedEvent;
                try { if (handler != null) handler(client, serial, maxHealth, health); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnManaUpdated(int client, int serial, int maxMana, int mana)
        {
            dManaUpdated handler = InternalManaUpdatedEvent;
            if (handler != null) handler(client, serial, maxMana, mana);
            lock (myManaUpdatedLock)
            {
                handler = myManaUpdatedEvent;
                try { if (handler != null) handler(client, serial, maxMana, mana); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnStaminaUpdated(int client, int serial, int maxStamina, int stamina)
        {
            dStaminaUpdated handler = InternalStaminaUpdatedEvent;
            if (handler != null) handler(client, serial, maxStamina, stamina);
            lock (myStaminaUpdatedLock)
            {
                handler = myStaminaUpdatedEvent;
                try { if (handler != null) handler(client, serial, maxStamina, stamina); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnLocalizedText(int client, JournalEntry journalEntry)
        {
            dLocalizedText handler = InternalLocalizedTextEvent;
            if (handler != null) handler(client, journalEntry);
            lock (myLocalizedTextLock)
            {
                handler = myLocalizedTextEvent;
                try { if (handler != null) handler(client, journalEntry); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnEquippedMobAdded(int client, Mobile mobile, ItemCollection equipment)
        {
            dEquippedMobAdded handler = InternalEquippedMobAddedEvent;
            if (handler != null) handler(client, mobile, equipment);
            lock (myEquippedMobAddedLock)
            {
                handler = myEquippedMobAddedEvent;
                try { if (handler != null) handler(client, mobile, equipment); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnMobileMoving(int client, int serial, int ID, int x, int y, int z, int direction, int hue, int status, int notoriety)
        {
            dMobileMoving handler = InternalMobileMovingEvent;
            if (handler != null) handler(client, serial, ID, x, y, z, direction, hue, status, notoriety);
            lock (myMobileMovingLock)
            {
                handler = myMobileMovingEvent;
                try { if (handler != null) handler(client, serial, ID, x, y, z, direction, hue, status, notoriety); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnContainerContents(int client, ItemCollection container)
        {
            dContainerContents handler = InternalContainerContentsEvent;
            if (handler != null) handler(client, container);
            lock (myContainerContentsLock)
            {
                handler = myContainerContentsEvent;
                try { if (handler != null) handler(client, container); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnWorldItemAdded(int client, Item item)
        {
            dWorldItemAdded handler = InternalWorldItemAddedEvent;
            if (handler != null) handler(client, item);
            lock (myWorldItemAddedLock)
            {
                handler = myWorldItemAddedEvent;
                try { if (handler != null) handler(client, item); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnItemDeleted(int client, int serial)
        {
            dItemDeleted handler = InternalItemDeletedEvent;
            if (handler != null) handler(client, serial);
            lock (myItemDeletedLock)
            {
                handler = myItemDeletedEvent;
                try { if (handler != null) handler(client, serial); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnMobileUpdated(int client, int serial, int ID, int hue, int status, int x, int y, int z, int direction)
        {
            dMobileUpdated handler = InternalMobileUpdatedEvent;
            if (handler != null) handler(client, serial, ID, hue, status, x, y, z, direction);
            lock (myMobileUpdatedLock)
            {
                handler = myMobileUpdatedEvent;
                try { if (handler != null) handler(client, serial, ID, hue, status, x, y, z, direction); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnMapChanged(int client, int map)
        {
            dMapChanged handler = InternalMapChangedEvent;
            if (handler != null) handler(client, map);
            lock (myMapChangedLock)
            {
                handler = myMapChangedEvent;
                try { if (handler != null) handler(client, map); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnItemAddedToContainer(int client, Item item)
        {
            dItemAddedToContainer handler = InternalItemAddedToContainerEvent;
            if (handler != null) handler(client, item);
            lock (myItemAddedToContainerLock)
            {
                handler = myItemAddedToContainerEvent;
                try { if (handler != null) handler(client, item); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnProperties(int client, int serial, string name, Property[] properties, string propertyText)
        {
            dProperties handler = InternalPropertiesEvent;
            if (handler != null) handler(client, serial, name, properties, propertyText);
            lock (myPropertiesLock)
            {
                handler = myPropertiesEvent;
                try { if (handler != null) handler(client, serial, name, properties, propertyText); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnMoveRejected(int client, int sequence, int x, int y, int z, int direction)
        {
            dMoveRejected handler = InternalMoveRejectedEvent;
            if (handler != null) handler(client, sequence, x, y, z, direction);
            lock (myMoveRejectedLock)
            {
                handler = myMoveRejectedEvent;
                try { if (handler != null) handler(client, sequence, x, y, z, direction); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnMoveAccepted(int client, int sequence, int status)
        {
            dMoveAccepted handler = InternalMoveAcceptedEvent;
            if (handler != null) handler(client, sequence, status);
            lock (myMoveAcceptedLock)
            {
                handler = myMoveAcceptedEvent;
                try { if (handler != null) handler(client, sequence, status); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnShortStatus(int client, int serial, string name, int health, int maxHealth, int sex)
        {
            dShortStatus handler = InternalShortStatusEvent;
            if (handler != null) handler(client, serial, name, health, maxHealth, sex);
            lock (myShortStatusLock)
            {
                handler = myShortStatusEvent;
                try { if (handler != null) handler(client, serial, name, health, maxHealth, sex); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnLongStatus(int client, int serial, PlayerStatus playerStatus)
        {
            dLongStatus handler = InternalLongStatusEvent;
            if (handler != null) handler(client, serial, playerStatus);
            lock (myLongStatusLock)
            {
                handler = myLongStatusEvent;
                try { if (handler != null) handler(client, serial, playerStatus); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnSkillUpdate(int client, int skillID, float value, float baseValue, LockStatus lockStatus, float skillCap)
        {
            dSkillUpdate handler = InternalSkillUpdateEvent;
            if (handler != null) handler(client, skillID, value, baseValue, lockStatus, skillCap);
            lock (mySkillUpdateLock)
            {
                handler = mySkillUpdateEvent;
                try { if (handler != null) handler(client, skillID, value, baseValue, lockStatus, skillCap); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnSkillList(int client, SkillInfo[] skills)
        {
            dSkillList handler = InternalSkillListEvent;
            if (handler != null) handler(client, skills);
            lock (mySkillListLock)
            {
                handler = mySkillListEvent;
                try { if (handler != null) handler(client, skills); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnTarget(int client, int targetType)
        {
            dTarget handler = InternalTargetEvent;
            if (handler != null) handler(client, targetType);
            lock (myTargetLock)
            {
                handler = myTargetEvent;
                try { if (handler != null) handler(client, targetType); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnStatLockStatus(int client, int serial, int statLockStatus)
        {
            dStatLockStatus handler = InternalStatLockStatusEvent;
            if (handler != null) handler(client, serial, statLockStatus);
            lock (myStatLockStatusLock)
            {
                handler = myStatLockStatusEvent;
                try { if (handler != null) handler(client, serial, statLockStatus); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnStandardGump(int client, int serial, int ID)
        {
            dStandardGump handler = null;
            handler = InternalStandardGumpEvent;
            if (handler != null) handler(client, serial, ID);
            lock (myStandardGumpLock)
            {
                handler = myStandardGumpEvent;
                try { if (handler != null) handler(client, serial, ID); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnMobileDeath(int client, int mobileSerial, int corpseSerial)
        {
            dMobileDeath handler = InternalMobileDeathEvent;
            if (handler != null) handler(client, mobileSerial, corpseSerial);
            lock (myMobileDeathLock)
            {
                handler = myMobileDeathEvent;
                try { if (handler != null) handler(client, mobileSerial, corpseSerial); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnPlayerDeath(int client)
        {
            dPlayerDeath handler = InternalPlayerDeathEvent;
            if (handler != null) handler(client);
            lock (myPlayerDeathLock)
            {
                handler = myPlayerDeathEvent;
                try { if (handler != null) handler(client); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnMobileName(int client, int serial, string name)
        {
            dMobileName handler = InternalMobileNameEvent;
            if (handler != null) handler(client, serial, name);
            lock (myMobileNameLock)
            {
                handler = myMobileNameEvent;
                try { if (handler != null) handler(client, serial, name); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnUnicodeText(int client, JournalEntry journalEntry)
        {
            dUnicodeText handler = InternalUnicodeTextEvent;
            if (handler != null) handler(client, journalEntry);
            lock (myUnicodeTextLock)
            {
                handler = myUnicodeTextEvent;
                try { if (handler != null) handler(client, journalEntry); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnText(int client, JournalEntry journalEntry)
        {
            dText handler = InternalTextEvent;
            if (handler != null) handler(client, journalEntry);
            lock (myTextLock)
            {
                handler = myTextEvent;
                try { if (handler != null) handler(client, journalEntry); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnPartyText(int client, JournalEntry journalEntry)
        {
            dPartyText handler = InternalPartyTextEvent;
            if (handler != null) handler(client, journalEntry);
            lock (myPartyTextLock)
            {
                handler = myPartyTextEvent;
                try { if (handler != null) handler(client, journalEntry); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnContextMenu(int client, ContextEntry[] contextEntries)
        {
            dContextMenu handler = InternalContextMenuEvent;
            if (handler != null) handler(client, contextEntries);
            lock (myContextMenuLock)
            {
                handler = myContextMenuEvent;
                try { if (handler != null) handler(client, contextEntries); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnItemEquipped(int client, int serial, int ID, Layer layer, int mobileSerial, int hue)
        {
            dItemEquipped handler = InternalItemEquippedEvent;
            if (handler != null) handler(client, serial, ID, layer, mobileSerial, hue);
            lock (myItemEquippedLock)
            {
                handler = myItemEquippedEvent;
                try { if (handler != null) handler(client, serial, ID, layer, mobileSerial, hue); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnSound(int client, byte flags, int effect, int volume, int x, int y, int z)
        {
            dSound handler = InternalSoundEvent;
            if (handler != null) handler(client, flags, effect, volume, x, y, z);
            lock (mySoundLock)
            {
                handler = mySoundEvent;
                try { if (handler != null) handler(client, flags, effect, volume, x, y, z); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnAttackTarget(int client, int serial)
        {
            dAttackTarget handler = InternalAttackTargetEvent;
            if (handler != null) handler(client, serial);
            lock (myAttackTargetLock)
            {
                handler = myAttackTargetEvent;
                try { if (handler != null) handler(client, serial); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnBondedStatus(int client, int serial, bool isDead)
        {
            dBondedStatus handler = InternalBondedStatusEvent;
            if (handler != null) handler(client, serial, isDead);
            lock (myBondedStatusLock)
            {
                handler = myBondedStatusEvent;
                try { if (handler != null) handler(client, serial, isDead); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnLoggedOut(int client)
        {
            dLoggedOut handler = InternalLoggedOutEvent;
            if (handler != null) handler(client);
            lock (myLoggedOutLock)
            {
                handler = myLoggedOutEvent;
                try { if (handler != null) handler(client); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnAttackEnded(int client)
        {
            dAttackEnded handler = InternalAttackEndedEvent;
            if (handler != null) handler(client);
            lock (myAttackEndedLock)
            {
                handler = myAttackEndedEvent;
                try { if (handler != null) handler(client); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnAttackGranted(int client, int serial)
        {
            dAttackGranted handler = InternalAttackGrantedEvent;
            if (handler != null) handler(client, serial);
            lock (myAttackGrantedLock)
            {
                handler = myAttackGrantedEvent;
                try { if (handler != null) handler(client, serial); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnAttackSwing(int client, int attackerSerial, int defenderSerial)
        {
            dAttackSwing handler = InternalAttackSwingEvent;
            if (handler != null) handler(client, attackerSerial, defenderSerial);
            lock (myAttackSwingLock)
            {
                handler = myAttackSwingEvent;
                try { if (handler != null) handler(client, attackerSerial, defenderSerial); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnGenericGump(int client, int serial, int ID, int x, int y, string layout, string[] text)
        {
            dGenericGump handler = InternalGenericGumpEvent;
            if (handler != null)
            {
                try { ThreadPool.QueueUserWorkItem(delegate { handler(client, serial, ID, x, y, layout, text); }); }
                catch (Exception ex)
                {
                    if (ex.InnerException != null) throw ex.InnerException;
                    throw ex;
                }
                handler(client, serial, ID, x, y, layout, text);
            }
            lock (myGenericGumpLock)
            {
                dGenericGump publicHandler = myGenericGumpEvent;
                try { if (publicHandler != null) publicHandler(client, serial, ID, x, y, layout, text); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnCloseGump(int client, int gumpID, int buttonID)
        {
            dCloseGump handler = InternalCloseGumpEvent;
            if (handler != null) handler(client, gumpID, buttonID);
            lock (myCloseGumpLock)
            {
                handler = myCloseGumpEvent;
                try { if (handler != null) handler(client, gumpID, buttonID); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnServerList(int client, ServerInfo[] serverList)
        {
            dServerList handler = InternalServerListEvent;
            if (handler != null) handler(client, serverList);
            lock (myServerListLock)
            {
                handler = myServerListEvent;
                try { if (handler != null) handler(client, serverList); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }

        internal static void OnCharacterList(int client, string[] characterList)
        {
            dCharacterList handler = InternalCharacterListEvent;
            if (handler != null) handler(client, characterList);
            lock (myCharacterListLock)
            {
                handler = myCharacterListEvent;
                try { if (handler != null) handler(client, characterList); }
                catch (Exception ex) { Log.LogMessage(ex); }
            }
        }
    }
}