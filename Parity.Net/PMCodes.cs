using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net
{
    // ProcessMessageCodes
    public class PMCodes
    {

        public static readonly int READY_CLICK = 0x32;
        public static readonly int MAP_CLICK = 0x33;
        public static readonly int TYPE_CLICK = 0x34;
        public static readonly int AILEVEL_CLICK = 0x3F;
        public static readonly int ROUND_CLICK = 0x35;
        public static readonly int TIME_CLICK = 0x36;
        public static readonly int KILL_CLICK = 0x37;
        public static readonly int TEAM_CLICK = 0x38;
        public static readonly int SCORE_CLICK = 0x39;
        public static readonly int HOLD_CLICK = 0x3A;
        public static readonly int PING_CLICK = 0x3B;
        public static readonly int VOTERATE_CLICK = 0x3C;
        public static readonly int VOTE_KICK = 0x3D;
        public static readonly int AUTOSTART_CLICK = 0x3E;
        public static readonly int BRANCH_CLICK = 0x64;
        public static readonly int HEALING_PLAYER = 0x65;
        public static readonly int HEALING_UNIT = 0x66;
        public static readonly int RELOAD_PLAYER = 0x69;
        public static readonly int DAMAGED_PLAYER = 0x67;
        public static readonly int DAMAGED_UNIT = 0x68;
        public static readonly int PLAYER_REGEN = 0x96;
        public static readonly int UNIT_REGEN = 0x97;
        public static readonly int PLAYER_DIE = 0x98;
        public static readonly int UNIT_DIE = 0x99;
        public static readonly int PLAYER_DENY = 0x9A;
        public static readonly int CHANGE_WEAPONS = 0x9B;
        public static readonly int TOTALWAR = 0xA5;
        public static readonly int REWARD_CHEAT = 0xA7;
        public static readonly int SUMMON_UNIT = 0xA6;
        public static readonly int COMBAT_SUPPORT = 0xA8;
        public static readonly int SUMMON_COOLTIME = 0xA9;
        public static readonly int CONQUEST_CAMP = 0x9C;
        public static readonly int SUICIDE = 0x9D;
        public static readonly int SUICIDE_UNIT = 0x9E;
        public static readonly int FIRE_ARTILLERY = 0x9F;
        public static readonly int ITEM_DROP = 0x190;
        public static readonly int ITEM_PICKUP = 0x191;
        public static readonly int OBJECT_RIDE = 0xC8;
        public static readonly int OBJECT_CHANGE_SEAT = 0xC9;
        public static readonly int OBJECT_ALIGHT = 0xCA;
        public static readonly int OBJECT_NETSTART = 0xCB;
        public static readonly int DUMMY_PACKET = 0x12C;
        public static readonly int GO = 0x193;
        public static readonly int CRASH_OBJECT = 0x1F4;
        public static readonly int INVALID_BULLET = 0x6B;
        public static readonly int ISHERO = 0x2BC;
        public static readonly int HERO_SKILL = 0x2BD;
        public static readonly int HERO_HEAL_SKILL = 0x2BE;
        public static readonly int HACK_PREVENT = 0x320;
        public static readonly int AI_DAMAGE_SP = 0x384;
        public static readonly int AI_ITEM_DROP = 0x385;
        public static readonly int AI_ITEM_PICKUP = 0x386;
        public static readonly int AI_ITEM_DELETE = 0x387;
        public static readonly int TRACE_GAS = 0x388;
        public static readonly int RANDOM_ITEM_DROP = 0x15E;
        public static readonly int RANDOM_ITEM_PICKUP = 0x15F;
        public static readonly int INFECTION_SKILL = 0x2BF;

    }
}
