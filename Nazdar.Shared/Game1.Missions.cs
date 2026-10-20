using Nazdar.Screens;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar
{
    // Keeps paused missions in memory and coordinates saving, loading, and resuming each save slot.
    public partial class Game1
    {
        // Captures a paused screen and the global values needed to resume it after visiting another menu.
        private sealed class MissionState
        {
            public VillageScreen Screen;
            public JObject SaveData;
            public int Village;
            public int CenterLevel;
            public int MapWidth;
            public bool FirstRun;
            public bool NextLevelAnimation;
            public bool WonAnimation;

            public MissionState(Game1 game, VillageScreen screen = null, JObject saveData = null)
            {
                this.Screen = screen;
                this.SaveData = saveData;
                this.Village = game.Village;
                this.CenterLevel = Game1.CenterLevel;
                this.MapWidth = VillageScreen.MapWidth;
                this.FirstRun = game.FirstRun;
                this.NextLevelAnimation = Game1.NextLevelAnimation;
                this.WonAnimation = Game1.WonAnimation;
            }

            public void RestoreGlobals(Game1 game)
            {
                game.Village = this.Village;
                Game1.CenterLevel = this.CenterLevel;
                VillageScreen.MapWidth = this.MapWidth;
                game.FirstRun = this.FirstRun;
                Game1.NextLevelAnimation = this.NextLevelAnimation;
                Game1.WonAnimation = this.WonAnimation;
            }
        }

        private readonly Dictionary<string, MissionState> pausedMissions = new Dictionary<string, MissionState>();

        internal bool CanResumeMission => this.pausedMissions.ContainsKey(this.SaveSlot);
        internal bool HasMissionSave
        {
            get
            {
                try { return new FileIO(this.SaveSlot).Exists; }
                catch (Exception error)
                {
                    Tools.Dump(error.ToString());
                    return false;
                }
            }
        }

        internal JObject GetMissionMenuData()
        {
            if (this.pausedMissions.TryGetValue(this.SaveSlot, out var state))
            {
                return state.SaveData;
            }

            return this.ReadMissionSave();
        }

        private JObject ReadMissionSave()
        {
            try { return new FileIO(this.SaveSlot).Load() as JObject; }
            catch (Exception error)
            {
                Tools.Dump(error.ToString());
                return null;
            }
        }

        internal void PauseMission(VillageScreen screen)
        {
            if (this.IsChangingScreen)
            {
                return;
            }

            screen.PauseForMenu();
            this.pausedMissions[this.SaveSlot] = new MissionState(this, screen, screen.CaptureSaveData());
            this.LoadScreen(this.Village == 0 ? typeof(SurvivalScreen) : typeof(MapScreen));
        }

        internal bool ContinueMission()
        {
            if (this.IsChangingScreen)
            {
                return false;
            }

            if (this.pausedMissions.TryGetValue(this.SaveSlot, out var state))
            {
                if (state.Screen == null)
                {
                    return this.TryActivateMission(state.SaveData);
                }

                state.RestoreGlobals(this);
                state.Screen.ResumeFromMenu();
                this.ShowScreen(state.Screen);
                return true;
            }

            if (this.HasMissionSave)
            {
                return this.LoadLastMissionSave();
            }

            return this.TryActivateMission(null);
        }

        internal bool SavePausedMission()
        {
            return this.pausedMissions.TryGetValue(this.SaveSlot, out var state) && this.TrySaveMissionData(state.SaveData);
        }

        internal bool TrySaveMissionData(JObject saveData)
        {
            try
            {
                new FileIO(this.SaveSlot).Save(saveData);
                MessageBuffer.AddMessage(Translation.Get("message.gameSaved"), MessageType.Info);
                return true;
            }
            catch (Exception error)
            {
                Tools.Dump(error.ToString());
                MessageBuffer.AddMessage(Translation.Get("message.saveFailed"), MessageType.Fail);
                return false;
            }
        }

        internal bool LoadLastMissionSave()
        {
            var data = this.ReadMissionSave();
            if (data == null)
            {
                MessageBuffer.AddMessage(Translation.Get("message.loadFailed"), MessageType.Fail);
                return false;
            }

            return this.TryActivateMission(data);
        }

        private bool TryActivateMission(JObject saveData)
        {
            if (this.IsChangingScreen)
            {
                return false;
            }

            var previousGlobals = new MissionState(this);
            try
            {
                if (saveData != null)
                {
                    int village = (int)saveData["village"];
                    if (village < 0 || village > MaxVillage ||
                        (village == 0) != (this.SaveSlot == Save.Survival) || !(saveData["player"] is JObject))
                    {
                        throw new InvalidOperationException("The save does not match this mission slot.");
                    }
                    this.Village = village;
                }
                else
                {
                    this.Village = this.SaveSlot == Save.Survival ? 0 : 1;
                    this.FirstRun = false;
                }

                NextLevelAnimation = false;
                WonAnimation = false;
                var screen = new VillageScreen(this, saveData);

                // Build the replacement before dropping the paused game; failed loads restore its globals.
                screen.Initialize();
                this.ShowScreen(screen);
                if (saveData != null)
                {
                    MessageBuffer.AddMessage(Translation.Get("message.gameLoaded"), MessageType.Info);
                }
                if (this.Village > 0)
                {
                    Audio.PlaySound(this.Village == MaxVillage ? "FinalRound" : "Ready");
                }
                this.ForgetPausedMission();
                return true;
            }
            catch (Exception error)
            {
                previousGlobals.RestoreGlobals(this);
                Audio.SongVolume = 0.25f;
                Tools.Dump(error.ToString());
                MessageBuffer.AddMessage(Translation.Get("message.loadFailed"), MessageType.Fail);
                return false;
            }
        }

        internal void ForgetPausedMission()
        {
            this.pausedMissions.Remove(this.SaveSlot);
        }

        // Keep the next village in memory if its automatic checkpoint could not be written.
        internal void KeepMissionCheckpoint(JObject saveData)
        {
            this.pausedMissions[this.SaveSlot] = new MissionState(this, saveData: saveData);
        }
    }
}
