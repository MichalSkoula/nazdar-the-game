using Nazdar.Shared;

namespace Nazdar.Tests
{
    public class EnemyDifficultyTests
    {
        [Fact]
        public void InfantryBaseShrinksWithVillageAndDays()
        {
            Assert.Equal(260 - 10 - 0, EnemyDifficulty.GetInfantryRandomBase(1, 0));
            Assert.Equal(260 - 30 - 20, EnemyDifficulty.GetInfantryRandomBase(3, 5));
        }

        [Fact]
        public void RandomBaseNeverDropsBelowMinimum()
        {
            Assert.Equal(16, EnemyDifficulty.GetInfantryRandomBase(8, 1000));
            Assert.Equal(16, EnemyDifficulty.GetTractorRandomBase(8, 1000));
        }

        [Fact]
        public void SurvivalHasFourTimesMoreInfantryAndTwiceRarerTractors()
        {
            Assert.Equal(260 / 4, EnemyDifficulty.GetInfantryRandomBase(0, 0));
            Assert.Equal((260 - 40) / 4, EnemyDifficulty.GetInfantryRandomBase(0, 10));
            Assert.Equal((260 - 40) * 2, EnemyDifficulty.GetTractorRandomBase(0, 10));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void NoPigsBeforeVillageThree(int village)
        {
            Assert.Equal(0.0, EnemyDifficulty.GetPigSpawnChance(village, 50));
        }

        [Fact]
        public void PigChanceStartsAtCalibratedValue()
        {
            Assert.Equal(0.00230, EnemyDifficulty.GetPigSpawnChance(3, 0), 10);
            Assert.Equal(0.01095 + 41 * 0.00005, EnemyDifficulty.GetPigSpawnChance(8, 41), 10);
        }

        [Fact]
        public void PigChanceIsCapped()
        {
            Assert.Equal(0.03125, EnemyDifficulty.GetPigSpawnChance(8, 100000), 10);
        }

        [Fact]
        public void SurvivalPigChanceUsesInfantryBase()
        {
            Assert.Equal(1.0 / 220, EnemyDifficulty.GetPigSpawnChance(0, 10), 10);
        }

        [Fact]
        public void DifficultyNeverDecreasesOverTime()
        {
            for (int village = 0; village <= 8; village++)
            {
                for (int days = 1; days < 200; days++)
                {
                    Assert.True(EnemyDifficulty.GetInfantryRandomBase(village, days) <= EnemyDifficulty.GetInfantryRandomBase(village, days - 1));
                    Assert.True(EnemyDifficulty.GetTractorRandomBase(village, days) <= EnemyDifficulty.GetTractorRandomBase(village, days - 1));
                    Assert.True(EnemyDifficulty.GetPigSpawnChance(village, days) >= EnemyDifficulty.GetPigSpawnChance(village, days - 1));
                }
            }
        }
    }
}
