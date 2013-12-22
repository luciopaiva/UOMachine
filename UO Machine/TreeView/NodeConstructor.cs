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
using System.Threading;

namespace UOMachine.Tree
{
    public enum PlayerNode : byte
    {
        Name,
        Label,
        PropertyText,
        Status,
        Race,
        Sex,
        ID,
        X,
        Y,
        Z,
        Direction,
        Health,
        MaxHealth,
        Stamina,
        MaxStamina,
        Mana,
        MaxMana,
        Notoriety,
        Facet,
        Str,
        Dex,
        Int,
        Luck,
        PhysicalResist,
        ColdResist,
        PoisonResist,
        EnergyResist,
        FireResist,
        Weight,
        MaxWeight,
        MinDamage,
        MaxDamage,
        Followers,
        MaxFollowers,
        StatCap,
        StatLock,
        Gold,
        TithingPoints
    }

    internal static class NodeConstructor
    {
        private static TreeViewItem CreateItem(string header)
        {
            TreeViewItem t = new TreeViewItem();
            t.Header = header;
            return t;
        }

        public static TreeViewItem Create(PlayerMobile player)
        {
            TreeViewItem t = new TreeViewItem();
            t.Header = "Player (" + player.Serial + ") (" + player.Name + ")";
            t.Tag = player.Serial;
            t.Items.Add(CreateItem("Name = " + player.Name));
            t.Items.Add(CreateItem("Label = " + player.Label));
            t.Items.Add(CreateItem("PropertyText = " + player.PropertyText));
            t.Items.Add(CreateItem("Status = " + (MobileStatus)player.Status));
            t.Items.Add(CreateItem("Race = " + (PlayerRace)player.Race));
            t.Items.Add(CreateItem("Sex = " + (PlayerSex)player.Sex));
            t.Items.Add(CreateItem("ID = " + player.ID));
            t.Items.Add(CreateItem("X = " + player.X));
            t.Items.Add(CreateItem("Y = " + player.Y));
            t.Items.Add(CreateItem("Z = " + player.Z));
            t.Items.Add(CreateItem("Direction = " + (Direction)player.Direction));
            t.Items.Add(CreateItem("Health = " + player.Health));
            t.Items.Add(CreateItem("MaxHealth = " + player.MaxHealth));
            t.Items.Add(CreateItem("Stamina = " + player.Stamina));
            t.Items.Add(CreateItem("MaxStamina = " + player.MaxStamina));
            t.Items.Add(CreateItem("Mana = " + player.Mana));
            t.Items.Add(CreateItem("MaxMana = " + player.MaxMana));
            t.Items.Add(CreateItem("Notoriety = " + (Notoriety)player.Notoriety));
            t.Items.Add(CreateItem("Facet = " + (Facet)player.Facet));
            t.Items.Add(CreateItem("Str = " + player.Str));
            t.Items.Add(CreateItem("Dex = " + player.Dex));
            t.Items.Add(CreateItem("Int = " + player.Int));
            t.Items.Add(CreateItem("Luck = " + player.Luck));
            t.Items.Add(CreateItem("PhysicalResist = " + player.PhysicalResist));
            t.Items.Add(CreateItem("ColdResist = " + player.ColdResist));
            t.Items.Add(CreateItem("PoisonResist = " + player.PoisonResist));
            t.Items.Add(CreateItem("EnergyResist = " + player.EnergyResist));
            t.Items.Add(CreateItem("FireResist = " + player.FireResist));
            t.Items.Add(CreateItem("Weight = " + player.Weight));
            t.Items.Add(CreateItem("MaxWeight = " + player.MaxWeight));
            t.Items.Add(CreateItem("MinDamage = " + player.MinDamage));
            t.Items.Add(CreateItem("MaxDamage = " + player.MaxDamage));
            t.Items.Add(CreateItem("Followers = " + player.Followers));
            t.Items.Add(CreateItem("MaxFollowers = " + player.MaxFollowers));
            t.Items.Add(CreateItem("StatCap = " + player.StatCap));
            t.Items.Add(CreateItem("StatLock = " + (StatLockStatus)player.StatLock));
            t.Items.Add(CreateItem("Gold = " + player.Gold));
            t.Items.Add(CreateItem("TithingPoints = " + player.TithingPoints));
            return t;
        }
    }
}