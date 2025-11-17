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
                
                // Splash Screen
                { "splash.pressEnter", "Press ENTER" },
                { "splash.pressButtonA", "Press button A" },
                { "splash.touchToContinue", "Touch to continue" },
            
                // Splash Screen 2
                { "splash2.line1", "The Czechoslovak Legion was a military force" },
                { "splash2.line2", "fighting for the Allies during WWI." },
                { "splash2.line3", "The main goal was to win the support of the" },
                { "splash2.line4", "Allies for the independence of Czechoslovakia" },
                { "splash2.line5", "from the Austria-Hungary. The Legion's efforts" },
                { "splash2.line6", "during the Russian Civil War included clearing" },
                { "splash2.line7", "the entire Trans-Siberian Railway of" },
                { "splash2.line8", "Bolshevik forces. They evacuated to" },
                { "splash2.line9", "Europe by 1920." },
                { "splash2.disclaimer", "This game is for entertainment and is not historically accurate." },
          
                // Credits
                { "credits.gameBy", "A GAME BY" },
                { "credits.art", "ART" },
                { "credits.music", "MUSIC" },
                { "credits.sounds", "SOUNDS" },
                { "credits.font", "FONT" },
                { "credits.tech", "TECH" },
                { "credits.communityHelp", "CONTRIBUTORS"},
     
                // Map Mission 1
                { "mission1.line1", "May 1918. We were attacked by" },
                { "mission1.line2", "the Hungarians loyal to the Central Powers!" },
                { "mission1.line3", "Defend the train!" },
                { "mission1.goals", "MISSION GOALS" },
                { "mission1.goal1", "Repair the locomotive and head east" },
      
                // Map Mission 2
                { "mission2.line1", "May 1918. The damned Bolsheviks attacked" },
                { "mission2.line2", "the Legion train at the station! Defend!" },
                { "mission2.goals", "MISSION GOALS" },
                { "mission2.goal1", "Repair the locomotive and head east" },
                { "mission2.tips", "TIPS" },
                { "mission2.tip1", "New wagon - Hospital" },
    
                // Map Mission 3
                { "mission3.line1", "May 1918. The damned Bolsheviks blew up" },
                { "mission3.line2", "the rails!" },
                { "mission3.goals", "MISSION GOALS" },
                { "mission3.goal1", "Repair damaged rails" },
                { "mission3.goal2", "Repair the locomotive and head east" },
                { "mission3.tips", "TIPS" },
                { "mission3.tip1", "New wagon - Defense Tower" },
                { "mission3.tip2", "Absence of an Armory will make you defend" },
                { "mission3.tip3", "your people on your own." },
             
                // Map Mission 4
                { "mission4.line1", "June 1918. A great battle is coming. You" },
                { "mission4.line2", "must capture the city of Lipjag, where" },
                { "mission4.line3", "a large number of those Bolsheviks" },
                { "mission4.line4", "are gathered." },
                { "mission4.goals", "MISSION GOALS" },
                { "mission4.goal1", "Repair the locomotive and head east" },
                { "mission4.tips", "TIPS" },
                { "mission4.tip1", "Cholera epidemic struck! Make sure to have" },
                { "mission4.tip2", "a lot of medics." },
         
                // Map Mission 5
                { "mission5.line1", "June 1918. You must capture the city of Ufa" },
                { "mission5.line2", "and take control over near villages." },
                { "mission5.goals", "MISSION GOALS" },
                { "mission5.goal1", "Repair damaged rails" },
                { "mission5.goal2", "Repair the locomotive and head east" },
                { "mission5.tips", "TIPS" },
                { "mission5.tip1", "New wagon - Market" },
                { "mission5.tip2", "A hard winter struck, you will have to find" },
                { "mission5.tip3", "other sources of money than farming." },
           
                // Map Mission 6
                { "mission6.line1", "July 1918. You arrived late. Only a week" },
                { "mission6.line2", "earlier, the Bolsheviks had murdered" },
                { "mission6.line3", "the Russian Tsar and his entire family." },
                { "mission6.goals", "MISSION GOALS" },
                { "mission6.goal1", "Repair the locomotive and head east" },
                { "mission6.tips", "TIPS" },
                { "mission6.tip1", "Cannot build Arsenal - your future depends" },
                { "mission6.tip2", "on your heroic soldiers." },
                { "mission6.tip3", "Cholera epidemic struck hard! Make sure" },
                { "mission6.tip4", "to have lot of medics." },
     
                // Map Mission 7
                { "mission7.line1", "August 1918. You must conquer the city" },
                { "mission7.line2", "of Kazan and defend the Russian Golden" },
                { "mission7.line3", "Treasure. It will serve the White Guards" },
                { "mission7.line4", "to finance the fight." },
                { "mission7.goals", "MISSION GOALS" },
                { "mission7.goal1", "Defend the Golden Treasure" },
                { "mission7.goal2", "Repair damaged rails" },
                { "mission7.goal3", "Repair the locomotive and head east" },
                { "mission7.tips", "TIPS" },
                { "mission7.tip1", "The Golden Treasure cannot be lost!!" },
    
                // Map Mission 8
                { "mission8.line1", "January 1919. Time to go home. You took" },
                { "mission8.line2", "control the entire Trans-Siberian Railway," },
                { "mission8.line3", "but the promised help from the Allied Powers" },
                { "mission8.line4", "did not come." },
                { "mission8.goals", "MISSION GOALS" },
                { "mission8.goal1", "Buy the ship and go home" },
                { "mission8.tips", "TIPS" },
                { "mission8.tip1", "Damned Bolsheviks will attact only from" },
                { "mission8.tip2", "the left side. With all they got." },
                { "mission8.tip3", "Even with Mechanized Lenins." },
                { "mission8.tip4", "Cholera epidemic struck hard! Make sure" },
                { "mission8.tip5", "to have lot of medics." },
   
                // Survival
                { "survival.line1", "Endless farming." },
                { "survival.line2", "Endless slaughter." },
                { "survival.line3", "This is the life of a legionnaire." },
                { "survival.line4", "This is the survival." },
                { "survival.goals", "MISSION GOALS" },
                { "survival.goal1", "Unlimited time. Survive as long as you can." },
                { "survival.tips", "TIPS" },
                { "survival.tip1", "Cholera epidemic struck hard! Make sure to have" },
                { "survival.tip2", "a lot of medics." },
                { "survival.tip3", "Stronger enemies will attack very soon." },

                // Tutorial messages
                { "tutorial.buildBase", "I should build the Base" },
                { "tutorial.hirePeopleForBase", "I should hire some people to build the Base" },
                { "tutorial.buildFarms", "I should build some farms to generate money" },
                { "tutorial.hirePeopleForFarm", "I should hire some people to build the farm" },
                { "tutorial.createTools", "I should create some farm tools to be able to recruit farmers" },
                { "tutorial.hireFarmers", "I should hire some people to become farmers" },
                { "tutorial.buildArmory", "I should build the Armory to be able to recruit soldiers" },
                { "tutorial.hirePeopleForArmory", "I should hire some people to build the Armory" },
                { "tutorial.createWeapons", "I should create some weapons to be able to recruit soldiers" },
                { "tutorial.hireSoldiers", "I should hire some people to become soldiers" },
                { "tutorial.buildArsenal", "I should build the Arsenal to be able to buy cartridges" },
                { "tutorial.hirePeopleForArsenal", "I should hire some people to build the Arsenal" },
                { "tutorial.defendAndEarn", "I should defend the base and make enough money to repair the Locomotive" },

                // Action messages
                { "action.firstUpgradeBase", "First you need to upgrade the base" },
                { "action.firstRepairRails", "First you need to repair all rails" },
                { "action.offWeGo", "Off we go!" },
                { "action.weDidIt", "We did it! Nazdar!" },

                // Player actions
                { "action.build", "Build" },
                { "action.hire", "Hire" },
                { "action.create", "Create" },
                { "action.upgrade", "Upgrade" },
                { "action.buy", "Buy" },
                { "action.repair", "Repair" },

                // Building names
                { "building.base", "Base" },
                { "building.armory", "Armory" },
                { "building.arsenal", "Arsenal" },
                { "building.defenseTower", "Defense Tower" },
                { "building.farm", "Farm" },
                { "building.hospital", "Hospital" },
                { "building.market", "Market" },
                { "building.locomotive", "Locomotive" },
                { "building.rails", "Rails" },
                { "building.ship", "Ship" },
                { "building.treasure", "Golden Treasure" },

                // Items
                { "item.weapon", "Weapon" },
                { "item.tool", "Tool" },
                { "item.medicalKit", "Medical Kit" },
                { "item.cartridge", "Cartridge" },

                // Enemies
                { "enemy.rogueBolshevik", "Rogue Bolshevik" },
                { "enemy.pigRider", "Pig Rider" },
                { "enemy.leninTractor", "Lenin Tractor" },

                // People
                { "people.homeless", "Homeless" },

                // Stats
                { "stats.day", "Day " },
                { "stats.kills", "Kills: " },
                { "stats.money", "Money: " },
            };
        }
    }
}
