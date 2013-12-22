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

namespace UOMachine
{
    public enum Skill : byte
    {
        Alchemy,
        Anatomy,
        Animal_Lore,
        Item_Identification,
        Arms_Lore,
        Parrying,
        Begging,
        Blacksmithy,
        Bowcraft_Fletching,
        Peacemaking,
        Camping,
        Carpentry,
        Cartography,
        Cooking,
        Detecting_Hidden,
        Discordance,
        Evaluating_Intelligence,
        Healing,
        Fishing,
        Forensic_Evaluation,
        Herding,
        Hiding,
        Provocation,
        Inscription,
        Lockpicking,
        Magery,
        Resisting_Spells,
        Tactics,
        Snooping,
        Musicianship,
        Poisoning,
        Archery,
        Spirit_Speak,
        Stealing,
        Tailoring,
        Animal_Taming,
        Taste_Identification,
        Tinkering,
        Tracking,
        Veterinary,
        Swordsmanship,
        Mace_Fighting,
        Fencing,
        Wrestling,
        Lumberjacking,
        Mining,
        Meditation,
        Stealth,
        Remove_Trap,
        Necromancy,
        Focus,
        Chivalry,
        Bushido,
        Ninjitsu,
        Spellweaving
    }

    public enum Direction : byte
    {
        North,
        Northeast,
        East,
        Southeast,
        South,
        Southwest,
        West,
        Northwest,
        Invalid
    }

    [Flags]
    public enum MobileStatus : byte
    {
        None = 0x00,
        Female = 0x02,
        Poisoned = 0x04,
        Invulnerable = 0x08,
        WarMode = 0x40,
        Hidden = 0x80
    }

    [Flags]
    public enum StatLockStatus : byte
    {
        AllStatsUp = 0x00,
        IntDown = 0x01,
        IntLocked = 0x02,
        DexDown = 0x04,
        DexLocked = 0x08,
        StrDown = 0x10,
        StrLocked = 0x20
    }

    public enum PlayerSex : byte
    {
        Male,
        Female,
        Unknown
    }

    public enum PlayerRace : byte
    {
        Unknown,
        Human,
        Elf,
        Gargoyle
    }

    public enum Facet : byte
    {
        Felucca,
        Trammel,
        Ilshenar,
        Malas,
        Tokuno,
        Ter_Mur
    }

    public enum LockStatus : byte
    {
        Up,
        Down,
        Locked
    }

    public enum TargetType : byte
    {
        Object,
        Ground
    }

    public enum Notoriety : byte
    {
        /// <summary>
        /// Invalid notoriety.
        /// </summary>
        Invalid,
        /// <summary>
        /// Blue.
        /// </summary>
        Innocent,
        /// <summary>
        /// Green.
        /// </summary>
        Ally,
        /// <summary>
        /// Grey, but not criminal.
        /// </summary>
        Attackable,
        /// <summary>
        /// Grey.
        /// </summary>
        Criminal,
        /// <summary>
        /// Orange.
        /// </summary>
        Enemy,
        /// <summary>
        /// Red.
        /// </summary>
        Murderer,
        /// <summary>
        /// Iris wiki says translucent...
        /// </summary>
        Unknown
    }

    //ripped directly from RunUO RC2
    public enum BuffIcon : short
    {
        DismountPrevention = 0x3E9,
        NoRearm = 0x3EA,
        //Currently, no 0x3EB or 0x3EC
        NightSight = 0x3ED,	//*
        DeathStrike,
        EvilOmen,
        UnknownStandingSwirl,
        UnknownKneelingSword,
        DivineFury,
        EnemyOfOne,
        HidingAndOrStealth,	//*
        ActiveMeditation,	//*
        BloodOathCaster,
        BloodOathCurse,
        CorpseSkin,			//*
        Mindrot,			//*
        PainSpike,			//*
        Strangle,
        GiftOfRenewal,		//*
        AttuneWeapon,
        Thunderstorm,
        EssenceOfWind,
        EtherealVoyage,
        GiftOfLife,
        ArcaneEmpowerment,
        MortalStrike,
        ReactiveArmor,
        Protection,
        ArchProtection,
        MagicReflection,	//*
        Incognito,			//*
        Disguised,
        AnimalForm,
        Polymorph,
        Invisibility,		//*
        Paralyze,			//*
        Poison,
        Bleed,
        Clumsy,				//*
        FeebleMind,			//*
        Weaken,				//*
        Curse,
        MassCurse,
        Agility,			//*
        Cunning,			//*
        Strength,			//*
        Bless
    }

    //ripped from RunUO RC2
    public enum Layer : byte
    {
        /// <summary>
        /// Invalid layer.
        /// </summary>
        Invalid = 0x00,
        /// <summary>
        /// One handed weapon.
        /// </summary>
        OneHanded = 0x01,
        /// <summary>
        /// Two handed weapon or shield.
        /// </summary>
        TwoHanded = 0x02,
        /// <summary>
        /// Shoes.
        /// </summary>
        Shoes = 0x03,
        /// <summary>
        /// Pants.
        /// </summary>
        Pants = 0x04,
        /// <summary>
        /// Shirts.
        /// </summary>
        Shirt = 0x05,
        /// <summary>
        /// Helmets, hats, and masks.
        /// </summary>
        Helm = 0x06,
        /// <summary>
        /// Gloves.
        /// </summary>
        Gloves = 0x07,
        /// <summary>
        /// Rings.
        /// </summary>
        Ring = 0x08,
        /// <summary>
        /// Talismans.
        /// </summary>
        Talisman = 0x09,
        /// <summary>
        /// Gorgets and necklaces.
        /// </summary>
        Neck = 0x0A,
        /// <summary>
        /// Hair.
        /// </summary>
        Hair = 0x0B,
        /// <summary>
        /// Half aprons.
        /// </summary>
        Waist = 0x0C,
        /// <summary>
        /// Torso, inner layer.
        /// </summary>
        InnerTorso = 0x0D,
        /// <summary>
        /// Bracelets.
        /// </summary>
        Bracelet = 0x0E,
        /// <summary>
        /// Unused.
        /// </summary>
        Unused_xF = 0x0F,
        /// <summary>
        /// Beards and mustaches.
        /// </summary>
        FacialHair = 0x10,
        /// <summary>
        /// Torso, outer layer.
        /// </summary>
        MiddleTorso = 0x11,
        /// <summary>
        /// Earings.
        /// </summary>
        Earrings = 0x12,
        /// <summary>
        /// Arms and sleeves.
        /// </summary>
        Arms = 0x13,
        /// <summary>
        /// Cloaks.
        /// </summary>
        Cloak = 0x14,
        /// <summary>
        /// Backpacks.
        /// </summary>
        Backpack = 0x15,
        /// <summary>
        /// Torso, outer layer.
        /// </summary>
        OuterTorso = 0x16,
        /// <summary>
        /// Leggings, outer layer.
        /// </summary>
        OuterLegs = 0x17,
        /// <summary>
        /// Leggings, inner layer.
        /// </summary>
        InnerLegs = 0x18,
        /// <summary>
        /// Mount item layer.
        /// </summary>
        Mount = 0x19,
        /// <summary>
        /// Vendor 'buy pack' layer.
        /// </summary>
        ShopBuy = 0x1A,
        /// <summary>
        /// Vendor 'resale pack' layer.
        /// </summary>
        ShopResale = 0x1B,
        /// <summary>
        /// Vendor 'sell pack' layer.
        /// </summary>
        ShopSell = 0x1C,
    }


}