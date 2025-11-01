using System.Collections.Generic;

namespace Nazdar.Shared.Translation
{
    public static class TranslationEN
    {
        public static Dictionary<string, string> GetTranslations()
        {
            return new Dictionary<string, string>
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
    }
}
