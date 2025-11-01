using System.Collections.Generic;

namespace Nazdar.Shared.Translation
{
    public static class TranslationCS
    {
        public static Dictionary<string, string> GetTranslations()
        {
            return new Dictionary<string, string>
            {
                // Menu
                { "menu.newGame", "Nova hra" },
                { "menu.survival", "Preziti" },
                { "menu.controls", "Ovladani" },
                { "menu.credits", "Tvurci" },
                { "menu.music", "Hudba:" },
                { "menu.sounds", "Zvuky:" },
                { "menu.vibrations", "Vibrace:" },
                { "menu.fullscreen", "Cela obrazovka:" },
                { "menu.language", "Jazyk:" },
                { "menu.exit", "Konec" },
                { "menu.on", "Zap" },
                { "menu.off", "Vyp" },
                { "menu.backToMenu", "Zpet do menu" },
                
                // Controls
                { "controls.title", "Ovladani" },
                { "controls.movement", "pohyb" },
                { "controls.shoot", "strelba" },
                { "controls.jump", "skok" },
                { "controls.action", "akce" },
                
                // Credits
                { "credits.title", "Tvurci" },
                { "credits.officialPage", "Oficialni stranka" },
                { "credits.coffee", "Kup mi kafe" },
                
                // Map
                { "map.start", "Start" },
                { "map.deleteSave", "Smazat pozici" },
                
                // Game Over
                { "gameOver.title", "KONEC HRY" },
                { "gameOver.loadLastSave", "Nacist posledni pozici" },
                { "gameOver.newGame", "Nova hra" },
                { "gameOver.confirmTitle", "Zacit novou hru?" },
                { "gameOver.yes", "Ano" },
                { "gameOver.no", "Ne" },
                
                // Game Finished
                { "gameFinished.title", "GRATULUJEME!" },
                
                // Map Delete Save
                { "mapDeleteSave.title", "Smazat pozici?" },
                
                // In-game messages
                { "message.gameLoaded", "Hra nactena" },
                { "message.gameSaved", "Hra ulozena" },
                { "message.goToAnotherVillage", "Pojdme do dalsi vesnice!" },
                { "message.youWon", "VYHRAL JSI. Zacatecnicke stesti." },
                { "message.treasureStolen", "Zlaty poklad byl ukraden" },
                { "message.soldierKilled", "Hrdina vojak zabit {0}" },
                { "message.peasantKilled", "Nevinny sedlak zabit {0}" },
                { "message.farmerKilled", "Nevinny farmar zabit {0}" },
                { "message.medicKilled", "Nevinny medik zabit {0}" },
                { "message.peasantToSoldier", "Sedlak => vojak" },
                { "message.peasantToFarmer", "Sedlak => farmar" },
                { "message.peasantToMedic", "Sedlak => medik" },
                { "message.buildingStarted", "Stavba zahajene" },
                { "message.notEnoughMoney", "Nedostatek penez" },
                { "message.cantHoldMoney", "Neuneses vsechny penize" },
                { "message.cantHoldCartridges", "Neuneses vsechny naboje" },
                { "message.noCartridges", "Zadne naboje" },
                { "message.buildingBuilt", "{0} postaveno" },
                { "message.buildingUpgraded", "Budova vylepsen" },
                { "message.towerUpgraded", "Vez vylepsen" },
                { "message.weaponKitPurchased", "Zbranova sada zakoupena" },
                { "message.armoryFull", "Zbrojnice je plna" },
                { "message.toolPurchased", "Naradi zakoupeno" },
                { "message.farmFull", "Farma je plna" },
                { "message.medicalKitPurchased", "Lekarnick zakoupena" },
                { "message.cantBuyMoreMedicalKits", "Nelze koupit dalsi lekarnic" },
                { "message.cartridgePurchased", "Naboj zakoupen" },
                { "message.shipBought", "Lod zakoupena" },
                { "message.homelessHired", "Bezdomovec najat => sedlak" },
                { "message.enemiesComing", "Pripravte se, prichazi nepratele" },
                { "message.newDawn", "Novy usvit" },
                { "message.newHomelessAvailable", "Novy bezdomovec k dispozici!" },
            };
        }
    }
}
