﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Management
{
    public class Level
    {

        static Level()
        {
            levelTable = new int[] { 0, 2250, 6750, 11250, 
                                    16650, 24750, 32850, 41625, 
                                    50400, 59175, 67950, 76725, 
                                    94725, 112725, 130725, 148725, 
                                    166725, 189225, 211725, 234225, 
                                    256725, 279225, 306225, 333225, 
                                    360225, 387225, 414225, 441225, 
                                    497475, 553725, 609975, 666225, 
                                    722475, 778725, 857475, 936225, 
                                    1014975, 1093725, 1172475, 1251225, 
                                    1363725, 1476225, 1588725, 1701225, 
                                    1813725, 1926225, 2038725, 2207475, 
                                    2376225, 2544975, 2713725, 2882475, 
                                    3051225, 3219975, 3444975, 3669975, 
                                    3894975, 4119975, 4344975, 4569975, 
                                    4794975, 5132475, 5469975, 5807475, 
                                    6144975, 6482475, 6819975, 7157475, 
                                    7494975, 7944975, 8394975, 8844975, 
                                    9294975, 9744975, 10194975, 10644975, 
                                    11094975, 11657475, 12219975, 12782475, 
                                    13344975, 13907475, 14469975, 15032475, 
                                    15932475, 17282475, 18632475, 19982475, 
                                    21332475, 22682475, 24032475, 25382475, 
                                    26732475, 28307475, 29882475, 31457475, 
                                    33032475, 34607475, 36182475, 37757475, 
                                    2147483647};
        }

        private static int[] levelTable;
        public static byte GetLevel(int experience)
        {
            byte level = 0;
            for (int i = 0; i < levelTable.Length; i++)
                if (experience >= levelTable[i])
                    level++;
                else
                    break;
            return level;
        }

        public static int GetExperience(byte level)
        {
            System.Diagnostics.Debug.Assert(level >= 1 && level <= 101, "Level must be between 1 and 101.");
            return levelTable[level];
        }

    }
}
