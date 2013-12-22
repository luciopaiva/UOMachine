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
    public sealed class SkillInfo
    {
        private int myID;
        public int ID
        {
            get { return myID; }
            internal set { myID = value; }
        }

        private float myValue;
        public float Value
        {
            get { return myValue; }
            internal set { myValue = value; }
        }

        private float myBaseValue;
        public float BaseValue
        {
            get { return myBaseValue; }
            internal set { myBaseValue = value; }
        }

        private LockStatus myLockStatus;
        public LockStatus LockStatus
        {
            get { return myLockStatus; }
            internal set { myLockStatus = value; }
        }

        private float mySkillCap;
        public float SkillCap
        {
            get { return mySkillCap; }
            internal set { mySkillCap = value; }
        }
    }
}