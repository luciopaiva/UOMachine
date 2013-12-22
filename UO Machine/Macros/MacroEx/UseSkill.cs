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

namespace UOMachine.Macros
{
    public static partial class MacroEx
    {
        public static void UseSkill(int client, Skill skill)
        {
            byte[] shortBaseSkillPacket = new byte[] { 0x12, 0x00, 0x08, 0x24, 0x00, 0x20, 0x30, 0x00 };
            byte[] longBaseSkillPacket = new byte[] { 0x12, 0x00, 0x09, 0x24, 0x00, 0x00, 0x20, 0x30, 0x00 };

            switch (skill)
            {
                case Skill.Anatomy:
                    shortBaseSkillPacket[4] = 0x31;
                    SendPacketToServer(client, shortBaseSkillPacket);
                    break;
                case Skill.Animal_Lore:
                    shortBaseSkillPacket[4] = 0x32;
                    SendPacketToServer(client, shortBaseSkillPacket);
                    break;
                case Skill.Animal_Taming:
                    longBaseSkillPacket[4] = 0x33;
                    longBaseSkillPacket[5] = 0x35;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Arms_Lore:
                    shortBaseSkillPacket[4] = 0x34;
                    SendPacketToServer(client, shortBaseSkillPacket);
                    break;
                case Skill.Begging:
                    shortBaseSkillPacket[4] = 0x36;
                    SendPacketToServer(client, shortBaseSkillPacket);
                    break;
                case Skill.Cartography:
                    longBaseSkillPacket[4] = 0x31;
                    longBaseSkillPacket[5] = 0x32;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Detecting_Hidden:
                    longBaseSkillPacket[4] = 0x31;
                    longBaseSkillPacket[5] = 0x34;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Discordance:
                    longBaseSkillPacket[4] = 0x31;
                    longBaseSkillPacket[5] = 0x35;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Evaluating_Intelligence:
                    longBaseSkillPacket[4] = 0x31;
                    longBaseSkillPacket[5] = 0x36;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Forensic_Evaluation:
                    longBaseSkillPacket[4] = 0x31;
                    longBaseSkillPacket[5] = 0x39;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Hiding:
                    longBaseSkillPacket[4] = 0x32;
                    longBaseSkillPacket[5] = 0x31;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Inscription:
                    longBaseSkillPacket[4] = 0x32;
                    longBaseSkillPacket[5] = 0x33;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Item_Identification:
                    shortBaseSkillPacket[4] = 0x33;
                    SendPacketToServer(client, shortBaseSkillPacket);
                    break;
                case Skill.Meditation:
                    longBaseSkillPacket[4] = 0x34;
                    longBaseSkillPacket[5] = 0x36;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Peacemaking:
                    shortBaseSkillPacket[4] = 0x39;
                    SendPacketToServer(client, shortBaseSkillPacket);
                    break;
                case Skill.Poisoning:
                    longBaseSkillPacket[4] = 0x33;
                    longBaseSkillPacket[5] = 0x30;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Provocation:
                    longBaseSkillPacket[4] = 0x32;
                    longBaseSkillPacket[5] = 0x32;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Remove_Trap:
                    longBaseSkillPacket[4] = 0x34;
                    longBaseSkillPacket[5] = 0x38;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Spirit_Speak:
                    longBaseSkillPacket[4] = 0x33;
                    longBaseSkillPacket[5] = 0x32;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Stealing:
                    longBaseSkillPacket[4] = 0x33;
                    longBaseSkillPacket[5] = 0x33;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Stealth:
                    longBaseSkillPacket[4] = 0x34;
                    longBaseSkillPacket[5] = 0x37;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Taste_Identification:
                    longBaseSkillPacket[4] = 0x33;
                    longBaseSkillPacket[5] = 0x36;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                case Skill.Tracking:
                    longBaseSkillPacket[4] = 0x33;
                    longBaseSkillPacket[5] = 0x38;
                    SendPacketToServer(client, longBaseSkillPacket);
                    break;
                default:
                    break;
            }
        }
    }
}