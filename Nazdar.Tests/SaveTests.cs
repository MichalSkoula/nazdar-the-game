using Nazdar.Shared;
using Nazdar.Shared.Translation;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace Nazdar.Tests
{
    // Fixtures are real saves from the current version; a failing test here means old saves would break.
    public class SaveTests : IDisposable
    {
        private readonly string folder = Path.Combine(Path.GetTempPath(), "NazdarTests", Guid.NewGuid().ToString("N"));

        public SaveTests()
        {
            Translation.CurrentLanguage = "en";
        }

        public void Dispose()
        {
            if (Directory.Exists(this.folder))
            {
                Directory.Delete(this.folder, true);
            }
        }

        private static JObject LoadFixture(string name)
        {
            var file = new FileIO(name + ".json", Path.Combine(AppContext.BaseDirectory, "Fixtures", "Saves"));
            Assert.True(file.Exists, "Fixture " + name + " not found");
            return Assert.IsType<JObject>(file.Load());
        }

        [Theory]
        [InlineData("campaign-village8", "8/Vladivostok", "Day 41", "Kills: 0", "Money: 43")]
        [InlineData("campaign-village2", "2/Omsk", "Day 5", "Kills: 0", "Money: 18")]
        [InlineData("survival-day1", "Survival", "Day 1", "Kills: 19", "Money: 36")]
        public void SaveSlotSummary(string fixture, string village, string day, string kills, string money)
        {
            string[] summary = Tools.ParseSaveData(LoadFixture(fixture));
            Assert.Equal(new[] { village, day, kills, money }, summary);
        }

        [Fact]
        public void EmptySlotSummary()
        {
            Assert.Equal(new[] { " ", " ", " ", " " }, Tools.ParseSaveData(null));
        }

        [Theory]
        [InlineData("campaign-village8")]
        [InlineData("campaign-village2")]
        [InlineData("survival-day1")]
        public void SaveHasEverythingTheGameReads(string fixture)
        {
            JObject save = LoadFixture(fixture);

            // read without ContainsKey checks in VillageScreen.Load and Player.Load
            foreach (string key in new[] { "village", "player", "dayPhase", "dayPhaseTimer" })
            {
                Assert.True(save.ContainsKey(key), "Missing " + key);
            }

            var player = (JObject)save["player"];
            foreach (string key in new[] { "Hitbox", "Direction", "Health", "Days", "Money", "Cartridges", "Kills", "Caliber" })
            {
                Assert.True(player.ContainsKey(key), "Missing player." + key);
            }

            foreach (string key in new[] { "X", "Width", "Height" })
            {
                Assert.True(((JObject)player["Hitbox"]).ContainsKey(key), "Missing player.Hitbox." + key);
            }

            foreach (string list in new[] { "enemies", "pigs", "lenins", "soldiers", "homelesses", "peasants", "farmers", "medics", "coins",
                "armories", "rails", "arsenals", "farms", "hospitals", "markets", "towers" })
            {
                if (save.ContainsKey(list))
                {
                    Assert.IsType<JArray>(save[list]);
                }
            }
        }

        [Fact]
        public void SaveAndLoadRoundTrip()
        {
            var file = new FileIO("slot.json", this.folder);
            file.Save(new { village = 3, player = new { Days = 12, Money = 7, Kills = 2 } });

            dynamic data = file.Load();
            Assert.Equal(3, (int)data.village);
            Assert.Equal(12, (int)data.player.Days);
        }

        [Fact]
        public void SaveReplacesExistingFileWithoutLeftovers()
        {
            var file = new FileIO("slot.json", this.folder);
            file.Save(new { village = 1 });
            file.Save(new { village = 2 });

            dynamic data = file.Load();
            Assert.Equal(2, (int)data.village);
            Assert.Equal(new[] { "slot.json" }, Directory.GetFiles(this.folder).Select(Path.GetFileName));
        }

        [Fact]
        public void CorruptSaveLoadsAsNull()
        {
            var file = new FileIO("slot.json", this.folder);
            File.WriteAllText(Path.Combine(this.folder, "slot.json"), "{ \"village\": ");
            Assert.Null(file.Load());
        }

        [Fact]
        public void MissingSaveLoadsAsNull()
        {
            var file = new FileIO("slot.json", this.folder);
            Assert.False(file.Exists);
            Assert.Null(file.Load());
        }

        [Fact]
        public void DeleteRemovesSave()
        {
            var file = new FileIO("slot.json", this.folder);
            file.Save(new { village = 1 });
            file.Delete();
            Assert.False(file.Exists);
        }
    }
}
