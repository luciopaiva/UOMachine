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
using System.Windows.Controls;
using UOMachine.Tree;
using UOMachine.Utility;

namespace UOMachine
{
    public sealed class PlayerMobile : Mobile
    {
        public PlayerMobile(int serial, int instance) : base(serial, instance) { }
        public delegate void dPropertyChanged(int headerNum, string header);
        public event dPropertyChanged PropertyChangedEvent;

        private void OnPropertyChanged(int headerNum, string header)
        {
            dPropertyChanged handler = PropertyChangedEvent;
            if (handler != null) handler(headerNum, header);
        }

        internal void UpdateLocation(int direction)
        {
            switch (direction)
            {
                case 0: //north
                    if (direction == this.Direction) this.Y--;
                    else this.Direction = direction;
                    break;
                case 1: //northeast
                    if (direction == this.Direction)
                    {
                        this.X++;
                        this.Y--;
                    }
                    else this.Direction = direction;
                    break;
                case 2: //east
                    if (direction == this.Direction) this.X++;
                    else this.Direction = direction;
                    break;
                case 3: //southeast
                    if (direction == this.Direction)
                    {
                        this.X++;
                        this.Y++;
                    }
                    else this.Direction = direction;
                    break;
                case 4: //south
                    if (direction == this.Direction) this.Y++;
                    else this.Direction = direction;
                    break;
                case 5: //southwest
                    if (direction == this.Direction)
                    {
                        this.X--;
                        this.Y++;
                    }
                    else this.Direction = direction;
                    break;
                case 6: //west
                    if (direction == this.Direction) this.X--;
                    else this.Direction = direction;
                    break;
                case 7: //northwest
                    if (direction == this.Direction)
                    {
                        this.X--;
                        this.Y--;
                    }
                    else this.Direction = direction;
                    break;
            }
        }

        public override int ID
        {
            get { return base.ID; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myID, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.ID, "ID = " + value);
            }
        }

        public override int Direction
        {
            get { return base.Direction; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myDirection, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Direction, "Direction = " + (Direction)value);
            }
        }

        public override int X
        {
            get { return base.X; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myX, value);
                if (oldValue != value)
                {
                    OnPropertyChanged((int)PlayerNode.X, "X = " + value);
                    OnPropertyChanged((int)PlayerNode.Z, "Z = " + this.Z);
                }
            }
        }

        public override int Y
        {
            get { return base.Y; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myY, value);
                if (oldValue != value)
                {
                    OnPropertyChanged((int)PlayerNode.Y, "Y = " + value);
                    OnPropertyChanged((int)PlayerNode.Z, "Z = " + this.Z);
                }
            }
        }

        public override int Z
        {
            get { return ClientInfoCollection.GetZ(this.Client); }
            /*get { return base.Z; }
            internal set
            {
                base.Z = value;
                OnPropertyChanged((int)PlayerNode.Z, "Z = " + value.ToString());
            }*/
        }

        public override int Status
        {
            get { return base.Status; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myStatus, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Status, "Status = " + (MobileStatus)value);
            }
        }

        public override int Health
        {
            get { return base.Health; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myHealth, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Health, "Health = " + value);
            }
        }

        public override int MaxHealth
        {
            get { return base.MaxHealth; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myMaxHealth, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.MaxHealth, "MaxHealth = " + value);
            }
        }

        public override int Stamina
        {
            get { return base.Stamina; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myStamina, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Stamina, "Stamina = " + value);
            }
        }

        public override int MaxStamina
        {
            get { return base.MaxStamina; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myMaxStamina, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.MaxStamina, "MaxStamina = " + value);
            }
        }

        public override int Mana
        {
            get { return base.Mana; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myMana, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Mana, "Mana = " + value);
            }
        }

        public override int MaxMana
        {
            get { return base.MaxMana; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myMaxMana, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.MaxMana, "MaxMana = " + value);
            }
        }

        public override int Notoriety
        {
            get { return base.Notoriety; }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.myNotoriety, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Notoriety, "Notoriety = " + (Notoriety)value);
            }
        }

        public override string Label
        {
            get { return base.Label; }
            internal set
            {
                string oldValue = Interlocked.Exchange(ref base.myLabel, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Label, "Label = " + value);
            }
        }

        public override string Name
        {
            get { return base.Name; }
            internal set
            {
                string oldValue = Interlocked.Exchange(ref base.myName, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Name, "Name = " + value);
            }
        }

        public override string PropertyText
        {
            get { return base.PropertyText; }
            internal set
            {
                string oldValue = Interlocked.Exchange(ref base.myPropertyText, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.PropertyText, "PropertyText = " + value);
            }
        }

        public override int Sex
        {
            get { return Thread.VolatileRead(ref base.mySex); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref base.mySex, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Sex, "Sex = " + (PlayerSex)value);
            }
        }

        private int myStatLock;
        public int StatLock
        {
            get { return Thread.VolatileRead(ref myStatLock); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myStatLock, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.StatLock, "StatLock = " + (StatLockStatus)value);
            }
        }

        private int myRace;
        public int Race
        {
            get { return Thread.VolatileRead(ref myRace); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myRace, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Race, "Race = " + (PlayerRace)value);
            }
        }

        private int myFacet = 0;
        public int Facet
        {
            get { return Thread.VolatileRead(ref myFacet); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myFacet, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Facet, "Facet = " + (Facet)value);
            }
        }

        private int myPhysicalResist = 0;
        public int PhysicalResist
        {
            get { return Thread.VolatileRead(ref myPhysicalResist); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myPhysicalResist, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.PhysicalResist, "PhysicalResist = " + value);
            }
        }

        private int myColdResist = 0;
        public int ColdResist
        {
            get { return Thread.VolatileRead(ref myColdResist); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myColdResist, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.ColdResist, "ColdResist = " + value);
            }
        }

        private int myPoisonResist = 0;
        public int PoisonResist
        {
            get { return Thread.VolatileRead(ref myPoisonResist); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myPoisonResist, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.PoisonResist, "PoisonResist = " + value);
            }
        }

        private int myEnergyResist = 0;
        public int EnergyResist
        {
            get { return Thread.VolatileRead(ref myEnergyResist); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myEnergyResist, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.EnergyResist, "EnergyResist = " + value);
            }
        }

        private int myFireResist = 0;
        public int FireResist
        {
            get { return Thread.VolatileRead(ref myFireResist); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myFireResist, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.FireResist, "FireResist = " + value);
            }
        }

        private int myStr = 0;
        public int Str
        {
            get { return Thread.VolatileRead(ref myStr); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myStr, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Str, "Str = " + value);
            }
        }

        private int myDex = 0;
        public int Dex
        {
            get { return Thread.VolatileRead(ref myDex); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myDex, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Dex, "Dex = " + value);
            }
        }

        private int myInt = 0;
        public int Int
        {
            get { return Thread.VolatileRead(ref myInt); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myInt, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Int, "Int = " + value);
            }
        }

        private int myStatCap = 0;
        public int StatCap
        {
            get { return Thread.VolatileRead(ref myStatCap); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myStatCap, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.StatCap, "StatCap = " + value);
            }
        }

        private int myLuck = 0;
        public int Luck
        {
            get { return Thread.VolatileRead(ref myLuck); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myLuck, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Luck, "Luck = " + value);
            }
        }

        private int myWeight = 0;
        public int Weight
        {
            get { return Thread.VolatileRead(ref myWeight); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myWeight, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Weight, "Weight = " + value);
            }
        }

        private int myMaxWeight = 0;
        public int MaxWeight
        {
            get { return Thread.VolatileRead(ref myMaxWeight); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myMaxWeight, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.MaxWeight, "MaxWeight = " + value);
            }
        }

        private int myMinDamage = 0;
        public int MinDamage
        {
            get { return Thread.VolatileRead(ref myMinDamage); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myMinDamage, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.MinDamage, "MinDamage = " + value);
            }
        }

        private int myMaxDamage = 0;
        public int MaxDamage
        {
            get { return Thread.VolatileRead(ref myMaxDamage); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myMaxDamage, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.MaxDamage, "MaxDamage = " + value);
            }
        }

        private int myGold = 0;
        public int Gold
        {
            get { return Thread.VolatileRead(ref myGold); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myGold, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Gold, "Gold = " + value);
            }
        }

        private int myTithingPoints = 0;
        public int TithingPoints
        {
            get { return Thread.VolatileRead(ref myTithingPoints); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myTithingPoints, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.TithingPoints, "TithingPoints = " + value);
            }
        }

        private int myFollowers = 0;
        public int Followers
        {
            get { return Thread.VolatileRead(ref myFollowers); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myFollowers, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.Followers, "Followers = " + value);
            }
        }

        private int myMaxFollowers = 0;
        public int MaxFollowers
        {
            get { return Thread.VolatileRead(ref myMaxFollowers); }
            internal set
            {
                int oldValue = Interlocked.Exchange(ref myMaxFollowers, value);
                if (oldValue != value) OnPropertyChanged((int)PlayerNode.MaxFollowers, "MaxFollowers = " + value);
            }
        }
    }
}