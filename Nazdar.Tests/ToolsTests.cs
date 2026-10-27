using Nazdar.Objects;
using Nazdar.Shared;
using static Nazdar.Enums;

namespace Nazdar.Tests
{
    public class ToolsTests
    {
        [Theory]
        [InlineData(64, "06:00")] // day starts
        [InlineData(62, "06:30")]
        [InlineData(32, "14:00")]
        [InlineData(0, "22:00")] // day ends
        public void DayTimerToHours(double timer, string expected)
        {
            Assert.Equal(expected, Tools.DayPhaseTimerToHours(DayPhase.Day, timer));
        }

        [Theory]
        [InlineData(32, "22:00")] // night starts
        [InlineData(28, "23:00")]
        [InlineData(16, "02:00")]
        [InlineData(0, "06:00")] // night ends
        public void NightTimerToHours(double timer, string expected)
        {
            Assert.Equal(expected, Tools.DayPhaseTimerToHours(DayPhase.Night, timer));
        }

        [Fact]
        public void WholeDayAndNightGiveValidQuarterHours()
        {
            foreach (DayPhase phase in new[] { DayPhase.Day, DayPhase.Night })
            {
                double length = phase == DayPhase.Day ? (double)DayNightLength.Day : (double)DayNightLength.Night;
                for (double timer = length; timer >= 0; timer -= 0.1)
                {
                    string[] parts = Tools.DayPhaseTimerToHours(phase, timer).Split(':');
                    Assert.InRange(int.Parse(parts[0]), 0, 23);
                    Assert.Contains(parts[1], new[] { "00", "15", "30", "45" });
                }
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(19, 0)]
        [InlineData(20, 20)]
        [InlineData(39, 20)]
        [InlineData(105, 100)]
        public void FloorOffRoundsDownToTwenties(int value, int expected)
        {
            Assert.Equal(expected, value.FloorOff());
        }

        [Theory]
        [InlineData(1, 24)]
        [InlineData(2, 28)]
        [InlineData(7, 48)] // same as the ship
        [InlineData(8, 52)]
        public void LocomotiveCostGrowsPerMission(int village, int expected)
        {
            Assert.Equal(expected, Locomotive.GetCost(village));
        }
    }
}
