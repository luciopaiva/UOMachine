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

namespace UOMachine.Macros
{
    public enum Stat : byte
    {
        Strength,
        Dexterity,
        Intelligence
    }

    public enum StatLockStatus : byte
    {
        Up,
        Down,
        Locked
    }

    public enum Magery : byte
    {
        Clumsy,
        Create_Food,
        Feeblemind,
        Heal,
        Magic_Arrow,
        Night_Sight,
        Reactive_Armor,
        Weaken,
        Agility,
        Cunning,
        Cure,
        Harm,
        Magic_Trap,
        Magic_Untrap,
        Protection,
        Strength,
        Bless,
        Fireball,
        Magic_Lock,
        Poison,
        Telekinesis,
        Teleport,
        Unlock,
        Wall_Of_Stone,
        Arch_Cure,
        Arch_Protection,
        Curse,
        Fire_Field,
        Greater_Heal,
        Lightning,
        Mana_Drain,
        Recall,
        Blade_Spirits,
        Dispel_Field,
        Incognito,
        Magic_Reflection,
        Mind_Blast,
        Paralyze,
        Poison_Field,
        Summon_Creature,
        Dispel,
        Energy_Bolt,
        Explosion,
        Invisibility,
        Mark,
        Mass_Curse,
        Paralyze_Field,
        Reveal,
        Chain_Lightning,
        Energy_Field,
        Flame_Strike,
        Gate_Travel,
        Mana_Vampire,
        Mass_Dispel,
        Meteor_Swarm,
        Polymorph,
        Earthquake,
        Energy_Vortex,
        Resurrection,
        Air_Elemental,
        Summon_Daemon,
        Earth_Elemental,
        Fire_Elemental,
        Water_Elemental
    }

    public enum Necromancy : byte
    {
        Animate_Dead,
        Blood_Oath,
        Corpse_Skin,
        Curse_Weapon,
        Evil_Omen,
        Horrific_Beast,
        Lich_Form,
        Mind_Rot,
        Pain_Spike,
        Poison_Strike,
        Strangle,
        Summon_Familiar,
        Vampiric_Embrace,
        Vengeful_Spirit,
        Wither,
        Wraith_Form,
        Exorcism
    }

    public enum Chivalry : byte
    {
        Cleanse_By_Fire,
        Close_Wounds,
        Consecrate_Weapon,
        Dispel_Evil,
        Divine_Fury,
        Enemy_Of_One,
        Holy_Light,
        Noble_Sacrifice,
        Remove_Curse,
        Sacred_Journey
    }

    public enum Bushido : byte
    {
        Honorable_Execution,
        Confidence,
        Evasion,
        Counter_Attack,
        Lightning_Strike,
        Momentum_Strike
    }

    public enum Ninjitsu : byte
    {
        Focus_Attack,
        Death_Strike,
        Animal_Form,
        Ki_Attack,
        Surprise_Attack,
        Backstab,
        Shadowjump,
        Mirror_Image
    }

    public enum Spellweaving : byte
    {
        Arcane_Circle,
        Gift_of_Renewal,
        Immolating_Weapon,
        Attunement,
        Thunderstorm,
        Natures_Fury,
        Summon_Fey,
        Summon_Fiend,
        Reaper_Form,
        Wildfire,
        Essence_of_Wind,
        Dryad_Allure,
        Ethereal_Voyage,
        Word_of_Death,
        Gift_of_Life,
        Arcane_Empowerment
    }

    public enum Virtues : byte
    {
        Honor,
        Sacrifice,
        Valor,
        Compassion,
        Honesty,
        Humility,
        Justice,
        Spirituality
    }

    public enum MacroAction : byte
    {
        Last_Spell,
        Last_Object,
        Bow,
        Salute,
        Quit_Game,
        All_Names,
        Last_Target,
        Target_Self,
        Arm_Disarm_Left,
        Arm_Disarm_Right,
        Wait_For_Target,
        Target_Next,
        Attack_Last,
        Delay,
        Circletrans,
        Close_Gumps,
        Always_Run,
        Save_Desktop,
        Kill_Gump_Open,
        Primary_Ability,
        Secondary_Ability,
        Equip_Last_Weapon,
        Open_Door,
        Toggle_War_Peace,
        Paste,
        Last_Skill
    }

    public enum GumpAction : byte
    {
        Open_Configuration,
        Open_Paperdoll,
        Open_Status,
        Open_Journal,
        Open_Skills,
        Open_Spellbook,
        Open_Chat,
        Open_Backpack,
        Open_Overview,
        Open_Mail,
        Open_Party_Manifest,
        Open_Party_Chat,
        Open_Necro_Spellbook,
        Open_Paladin_Spellbook,
        Open_Combat_Book,
        Open_Bushido_Spellbook,
        Open_Ninjitsu_Spellbook,
        Open_Guild,
        Open_Spellweaving_SpellBook,
        Open_Questlog,
        Close_Configuration,
        Close_Paperdoll,
        Close_Status,
        Close_Journal,
        Close_Skills,
        Close_Spellbook,
        Close_Chat,
        Close_Backpack,
        Close_Overview,
        Close_Mail,
        Close_Party_Manifest,
        Close_Party_Chat,
        Close_Necro_Spellbook,
        Close_Paladin_Spellbook,
        Close_Combat_Book,
        Close_Bushido_Spellbook,
        Close_Ninjitsu_Spellbook,
        Close_Guild,
        Minimize_Paperdoll,
        Minimize_Status,
        Minimize_Journal,
        Minimize_Skills,
        Minimize_Spellbook,
        Minimize_Chat,
        Minimize_Backpack,
        Minimize_Overview,
        Minimize_Mail,
        Minimize_Party_Manifest,
        Minimize_Party_Chat,
        Minimize_Necro_Spellbook,
        Minimize_Paladin_Spellbook,
        Minimize_Combat_Book,
        Minimize_Bushido_Spellbook,
        Minimize_Ninjitsu_Spellbook,
        Minimize_Guild,
        Maximize_Paperdoll,
        Maximize_Status,
        Maximize_Journal,
        Maximize_Skills,
        Maximize_Spellbook,
        Maximize_Chat,
        Maximize_Backpack,
        Maximize_Overview,
        Maximize_Mail,
        Maximize_Party_Manifest,
        Maximize_Party_Chat,
        Maximize_Necro_Spellbook,
        Maximize_Paladin_Spellbook,
        Maximize_Combat_Book,
        Maximize_Bushido_Spellbook,
        Maximize_Ninjitsu_Spellbook,
        Maximize_Guild
    }

    public enum Range : byte
    {
        Set_Update_Range,
        Modify_Update_Range,
        Increase_Update_Range,
        Decrease_Update_Range,
        Maximum_Update_Range,
        Minimum_Update_Range,
        Default_Update_Range,
        Update_Update_Range,
        Enable_Update_Range_Color,
        Disable_Update_Range_Color,
        Toggle_Update_Range_Color
    }

    public enum Speech : byte
    {
        Say,
        Emote,
        Whisper,
        Yell
    }

    public enum Targeting : byte
    {
        Attack_Selected,
        Use_Selected,
        Current_Target,
        Toggle_Targeting_System,
        Toggle_Buff_Window,
        Bandage_Self,
        Bandage_Target,
        Select_Next_Hostile,
        Select_Next_Party_Member,
        Select_Next_Follower,
        Select_Next_Object,
        Select_Next_Mobile,
        Select_Previous_Hostile,
        Select_Previous_Party_Member,
        Select_Previous_Follower,
        Select_Previous_Object,
        Select_Previous_Mobile,
        Select_Nearest_Hostile,
        Select_Nearest_Party_Member,
        Select_Nearest_Follower,
        Select_Nearest_Object,
        Select_Nearest_Mobile
    }
}