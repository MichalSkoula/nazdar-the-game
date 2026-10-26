using System;

namespace Nazdar.Shared
{
    // Enemy spawn tuning for campaign and survival, independent of the current screen.
    // Campaign pig values are percentages; the other spawn chances use 1 / randomBase per update.
    internal static class EnemyDifficulty
    {
        private const int InfantryStartingBase = 260;
        private const int CampaignTractorStartingBase = 800;
        private const int BaseReductionPerVillage = 10;
        private const int BaseReductionPerDay = 4;
        private const int MinimumRandomBase = 16;

        // Starting chance in percent per update, indexed by village; pigs unlock in village 3.
        // Calibrated for campaign days 12, 18, 23, 29, 35 and 41 in villages 3 through 8.
        private static readonly double[] CampaignPigStartingChancePercent =
        {
            0.0,   // Survival uses its own rule below.
            0.0,   // Village 1: no pigs.
            0.0,   // Village 2: no pigs.
            0.230, // Village 3.
            0.265, // Village 4.
            0.330, // Village 5.
            0.480, // Village 6.
            0.875, // Village 7.
            1.095, // Village 8.
        };

        // Each total campaign day adds 0.005 percentage points, up to the previous maximum chance.
        private const double CampaignPigChanceIncreasePerDayPercent = 0.005;
        private const double CampaignPigMaximumChancePercent = 3.125;

        // Survival has four times the infantry frequency and uses the unscaled base for pigs.
        private const int SurvivalInfantryFrequencyMultiplier = 4;
        private const int SurvivalTractorBaseMultiplier = 2;

        internal const int CampaignPigsFirstVillage = 3;
        internal const int SurvivalPigsFirstDay = 3;
        internal const int SurvivalTractorsFirstDay = 6;

        internal static int GetInfantryRandomBase(int village, int days)
        {
            int randomBase = CalculateRandomBase(InfantryStartingBase, village, days);
            return village == 0 ? randomBase / SurvivalInfantryFrequencyMultiplier : randomBase;
        }

        internal static double GetPigSpawnChance(int village, int days)
        {
            if (village == 0)
            {
                return 1.0 / CalculateRandomBase(InfantryStartingBase, village, days);
            }

            if (village < CampaignPigsFirstVillage)
            {
                return 0.0;
            }

            double chancePercent = CampaignPigStartingChancePercent[village]
                + days * CampaignPigChanceIncreasePerDayPercent;
            return Math.Min(CampaignPigMaximumChancePercent, chancePercent) / 100.0;
        }

        internal static int GetTractorRandomBase(int village, int days)
        {
            if (village == 0)
            {
                return CalculateRandomBase(InfantryStartingBase, village, days) * SurvivalTractorBaseMultiplier;
            }

            // Campaign tractors use their own starting base with the same village/day reductions.
            return CalculateRandomBase(CampaignTractorStartingBase, village, days);
        }

        private static int CalculateRandomBase(int startingBase, int village, int days)
        {
            // Days counts the entire run, including time spent in earlier villages; village 0 is survival.
            return Math.Max(MinimumRandomBase,
                startingBase - village * BaseReductionPerVillage - days * BaseReductionPerDay);
        }
    }
}
