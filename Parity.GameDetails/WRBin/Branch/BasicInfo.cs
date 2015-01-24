using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Branch
{
    public class BasicInfo
    {

        public readonly string Branch;

        public readonly int Strength;
        public readonly double StrengthRate;

        public readonly int Con;
        public readonly double ConRate;

        public readonly int Defense;
        public readonly double DefenseRate;

        public readonly int Stamina;
        public readonly double StaminaRate;

        public readonly int Wiz;
        public readonly double WizRate;

        public BasicInfo(string branch,
            int strength, double strengthRate,
            int con, double conRate,
            int defense, double defenseRate,
            int stamina, double staminaRate,
            int wiz, double wizRate)
        {
            this.Branch = branch;

            this.Strength = strength;
            this.StrengthRate = strengthRate;

            this.Con = con;
            this.ConRate = conRate;

            this.Defense = defense;
            this.DefenseRate = defenseRate;

            this.Stamina = stamina;
            this.StaminaRate = staminaRate;

            this.Wiz = wiz;
            this.WizRate = wizRate;
        }

    }
}
