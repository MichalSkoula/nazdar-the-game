using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Nazdar.Shared
{
    public static class Translation
    {
        private static Dictionary<string, string> translations = new Dictionary<string, string>();
        private static string currentLanguage = "en";
        private static FileIO languageFile;

        public static string CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                if (currentLanguage != value)
                {
                    currentLanguage = value;
                    LoadLanguage(value);
                }
            }
        }

        public static void Initialize()
        {
            // Detect system language
            string systemLanguage = DetectSystemLanguage();
            LoadLanguage(systemLanguage);
        }

        private static string DetectSystemLanguage()
        {
            try
            {
                string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
                // Support Czech and English, default to English
                if (culture == "cs")
                {
                    return "cs";
                }
            }
            catch
            {
                // If detection fails, default to English
            }
            return "en";
        }

        private static void LoadLanguage(string language)
        {
            currentLanguage = language;
            translations.Clear();

            languageFile = new FileIO($"lang_{language}.json");
            dynamic data = languageFile.Load();

            if (data != null)
            {
                // Load translations from file
                JObject jObject = (JObject)data;
                foreach (var property in jObject.Properties())
                {
                    translations[property.Name] = property.Value.ToString();
                }
            }
            else
            {
                // If file doesn't exist, load default English translations
                LoadDefaultTranslations(language);
            }
        }

        private static void LoadDefaultTranslations(string language)
        {
            // Default English translations
            if (language == "en")
            {
                translations = new Dictionary<string, string>
                {
                    // Menu
                    { "menu.newGame", "New game" },
                    { "menu.survival", "Survival" },
                    { "menu.controls", "Controls" },
                    { "menu.credits", "Credits" },
                    { "menu.music", "Music:" },
                    { "menu.sounds", "Sounds:" },
                    { "menu.vibrations", "Vibrations:" },
                    { "menu.fullscreen", "Fullscreen:" },
                    { "menu.language", "Language:" },
                    { "menu.exit", "Exit" },
                    { "menu.on", "On" },
                    { "menu.off", "Off" },
                    { "menu.backToMenu", "Back to Menu" },
                    
                    // Controls
                    { "controls.title", "Controls" },
                    { "controls.movement", "movement" },
                    { "controls.shoot", "shoot" },
                    { "controls.jump", "jump" },
                    { "controls.action", "action" },
                    
                    // Credits
                    { "credits.title", "Credits" },
                    { "credits.officialPage", "Official page" },
                    { "credits.coffee", "Buy me a coffee" },
                    
                    // Map
                    { "map.start", "Start" },
                    { "map.deleteSave", "Delete save" },
                    
                    // Game Over
                    { "gameOver.title", "GAME OVER" },
                    { "gameOver.loadLastSave", "Load last save" },
                    { "gameOver.newGame", "New game" },
                    { "gameOver.confirmTitle", "Start new game?" },
                    { "gameOver.yes", "Yes" },
                    { "gameOver.no", "No" },
                    
                    // Game Finished
                    { "gameFinished.title", "CONGRATULATIONS!" },
                    
                    // Map Delete Save
                    { "mapDeleteSave.title", "Delete save?" },
                    
                    // In-game messages
                    { "message.gameLoaded", "Game loaded" },
                    { "message.gameSaved", "Game saved" },
                    { "message.goToAnotherVillage", "Lets go to another village!" },
                    { "message.youWon", "YOU WON. Beginner's luck." },
                    { "message.treasureStolen", "The Golden Treasure was stolen" },
                    { "message.soldierKilled", "Heroic soldier killed by {0}" },
                    { "message.peasantKilled", "Innocent peasant killed by {0}" },
                    { "message.farmerKilled", "Innocent farmer killed by {0}" },
                    { "message.medicKilled", "Innocent medic killed by {0}" },
                    { "message.peasantToSoldier", "Peasant => soldier" },
                    { "message.peasantToFarmer", "Peasant => farmer" },
                    { "message.peasantToMedic", "Peasant => medic" },
                    { "message.buildingStarted", "Building started" },
                    { "message.notEnoughMoney", "Not enough money" },
                    { "message.cantHoldMoney", "Can't hold all this money" },
                    { "message.cantHoldCartridges", "Can't hold all these cartridges" },
                    { "message.noCartridges", "No cartridges" },
                    { "message.buildingBuilt", "{0} built" },
                    { "message.buildingUpgraded", "Building upgraded" },
                    { "message.towerUpgraded", "Tower upgraded" },
                    { "message.weaponKitPurchased", "Weapon kit purchased" },
                    { "message.armoryFull", "Armory is full" },
                    { "message.toolPurchased", "Tool purchased" },
                    { "message.farmFull", "Farm is full" },
                    { "message.medicalKitPurchased", "Medical kit purchased" },
                    { "message.cantBuyMoreMedicalKits", "Can't buy any more medical kits" },
                    { "message.cartridgePurchased", "Cartridge purchased" },
                    { "message.shipBought", "Ship bought" },
                    { "message.homelessHired", "Homeless hired => peasant" },
                    { "message.enemiesComing", "Brace yourselves, enemies are coming" },
                    { "message.newDawn", "New dawn" },
                    { "message.newHomelessAvailable", "New homeless available to hire!" },
                };
            }
            else if (language == "cs")
            {
                // Czech translations
                translations = new Dictionary<string, string>
                {
                    // Menu
                    { "menu.newGame", "Nová hra" },
                    { "menu.survival", "Přežití" },
                    { "menu.controls", "Ovládání" },
                    { "menu.credits", "Tvůrci" },
                    { "menu.music", "Hudba:" },
                    { "menu.sounds", "Zvuky:" },
                    { "menu.vibrations", "Vibrace:" },
                    { "menu.fullscreen", "Celá obrazovka:" },
                    { "menu.language", "Jazyk:" },
                    { "menu.exit", "Konec" },
                    { "menu.on", "Zap" },
                    { "menu.off", "Vyp" },
                    { "menu.backToMenu", "Zpět do menu" },
                    
                    // Controls
                    { "controls.title", "Ovládání" },
                    { "controls.movement", "pohyb" },
                    { "controls.shoot", "střelba" },
                    { "controls.jump", "skok" },
                    { "controls.action", "akce" },
                    
                    // Credits
                    { "credits.title", "Tvůrci" },
                    { "credits.officialPage", "Oficiální stránka" },
                    { "credits.coffee", "Kup mi kafe" },
                    
                    // Map
                    { "map.start", "Start" },
                    { "map.deleteSave", "Smazat pozici" },
                    
                    // Game Over
                    { "gameOver.title", "KONEC HRY" },
                    { "gameOver.loadLastSave", "Načíst poslední pozici" },
                    { "gameOver.newGame", "Nová hra" },
                    { "gameOver.confirmTitle", "Začít novou hru?" },
                    { "gameOver.yes", "Ano" },
                    { "gameOver.no", "Ne" },
                    
                    // Game Finished
                    { "gameFinished.title", "GRATULUJEME!" },
                    
                    // Map Delete Save
                    { "mapDeleteSave.title", "Smazat pozici?" },
                    
                    // In-game messages
                    { "message.gameLoaded", "Hra načtena" },
                    { "message.gameSaved", "Hra uložena" },
                    { "message.goToAnotherVillage", "Pojďme do další vesnice!" },
                    { "message.youWon", "VYHRÁL JSI. Začátečnické štěstí." },
                    { "message.treasureStolen", "Zlatý poklad byl ukraden" },
                    { "message.soldierKilled", "Hrdina voják zabit {0}" },
                    { "message.peasantKilled", "Nevinný sedlák zabit {0}" },
                    { "message.farmerKilled", "Nevinný farmář zabit {0}" },
                    { "message.medicKilled", "Nevinný medik zabit {0}" },
                    { "message.peasantToSoldier", "Sedlák => voják" },
                    { "message.peasantToFarmer", "Sedlák => farmář" },
                    { "message.peasantToMedic", "Sedlák => medik" },
                    { "message.buildingStarted", "Stavba zahájena" },
                    { "message.notEnoughMoney", "Nedostatek peněz" },
                    { "message.cantHoldMoney", "Neuneseš všechny peníze" },
                    { "message.cantHoldCartridges", "Neuneseš všechny náboje" },
                    { "message.noCartridges", "Žádné náboje" },
                    { "message.buildingBuilt", "{0} postaveno" },
                    { "message.buildingUpgraded", "Budova vylepšena" },
                    { "message.towerUpgraded", "Věž vylepšena" },
                    { "message.weaponKitPurchased", "Zbraňová sada zakoupena" },
                    { "message.armoryFull", "Zbrojnice je plná" },
                    { "message.toolPurchased", "Nářadí zakoupeno" },
                    { "message.farmFull", "Farma je plná" },
                    { "message.medicalKitPurchased", "Lékárnička zakoupena" },
                    { "message.cantBuyMoreMedicalKits", "Nelze koupit další lékárničky" },
                    { "message.cartridgePurchased", "Náboj zakoupen" },
                    { "message.shipBought", "Loď zakoupena" },
                    { "message.homelessHired", "Bezdomovec najat => sedlák" },
                    { "message.enemiesComing", "Připravte se, přichází nepřátelé" },
                    { "message.newDawn", "Nový úsvit" },
                    { "message.newHomelessAvailable", "Nový bezdomovec k dispozici!" },
                };
            }

            // Save default translations to file
            SaveTranslations();
        }

        private static void SaveTranslations()
        {
            if (languageFile != null)
            {
                languageFile.Save(translations);
            }
        }

        public static string Get(string key, params object[] args)
        {
            if (translations.TryGetValue(key, out string value))
            {
                if (args.Length > 0)
                {
                    return string.Format(value, args);
                }
                return value;
            }
            return key; // Return the key if translation not found
        }

        public static string GetLanguageName(string langCode)
        {
            switch (langCode)
            {
                case "en":
                    return "English";
                case "cs":
                    return "Čeština";
                default:
                    return langCode;
            }
        }
    }
}
