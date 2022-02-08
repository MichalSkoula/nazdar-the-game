﻿namespace MyGame
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public static class Settings
    {
        public static string SettingsFile = "settings.json";

        public static void LoadSettings(Game1 game)
        {
            if (!File.Exists(SettingsFile))
            {
                return;
            }

            // parse
            string json = File.ReadAllText(SettingsFile);
            dynamic settings = JObject.Parse(json);

            // apply fullscreen?
            if (settings.ContainsKey("fullscreen") && game.Graphics.IsFullScreen != (bool)settings.fullscreen)
            {
                game.Graphics.IsFullScreen = !game.Graphics.IsFullScreen;
                game.Graphics.ApplyChanges();
            }
        }

        public static void SaveSettings(Game1 game)
        {
            var saveData = new
            {
                fullscreen = game.Graphics.IsFullScreen,
            };

            string json = JsonConvert.SerializeObject(saveData);
            File.WriteAllText(SettingsFile, json);
        }
    }
}