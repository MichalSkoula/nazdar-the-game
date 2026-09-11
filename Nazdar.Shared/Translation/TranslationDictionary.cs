using System;
using System.Collections.Generic;
using System.Globalization;

namespace Nazdar.Shared.Translation
{
    internal static class TranslationDictionary
    {
        internal static readonly Dictionary<string, (string cs, string en)> Values = new Dictionary<string, (string cs, string en)>
        {
            // Menu
            { "menu.newGame", (cs: "Nova hra", en: "New game") },
            { "menu.survival", (cs: "Rezim preziti", en: "Survival") },
            { "menu.controls", (cs: "Ovladani", en: "Controls") },
            { "menu.credits", (cs: "Autori", en: "Credits") },
            { "menu.music", (cs: "Hudba:", en: "Music:") },
            { "menu.sounds", (cs: "Zvuky:", en: "Sounds:") },
            { "menu.vibrations", (cs: "Vibrace:", en: "Vibrations:") },
            { "menu.fullscreen", (cs: "Cela obrazovka:", en: "Fullscreen:") },
            { "menu.language", (cs: "Jazyk:", en: "Language:") },
            { "menu.exit", (cs: "Konec", en: "Exit") },
            { "menu.on", (cs: "Zap", en: "On") },
            { "menu.off", (cs: "Vyp", en: "Off") },
            { "menu.backToMenu", (cs: "Zpet do menu", en: "Back to Menu") },

            // Controls
            { "controls.title", (cs: "Ovladani", en: "Controls") },
            { "controls.movement", (cs: "pohyb", en: "movement") },
            { "controls.shoot", (cs: "strelba", en: "shoot") },
            { "controls.jump", (cs: "skok", en: "jump") },
            { "controls.action", (cs: "akce", en: "action") },

            // Credits
            { "credits.title", (cs: "Autori", en: "Credits") },
            { "credits.officialPage", (cs: "Oficialni stranka", en: "Official page") },
            { "credits.coffee", (cs: "Kup mi kafe!", en: "Buy me a coffee") },

            // Map
            { "map.start", (cs: "Start", en: "Start") },
            { "map.deleteSave", (cs: "Smazat pozici", en: "Delete save") },

            // Game Over
            { "gameOver.title", (cs: "KONEC HRY", en: "GAME OVER") },
            { "gameOver.loadLastSave", (cs: "Nacist posledni pozici", en: "Load last save") },
            { "gameOver.newGame", (cs: "Nova hra", en: "New game") },
            { "gameOver.confirmTitle", (cs: "Zacit novou hru?", en: "Start new game?") },
            { "gameOver.yes", (cs: "Ano", en: "Yes") },
            { "gameOver.no", (cs: "Ne", en: "No") },

            // Game Finished
            { "gameFinished.title", (cs: "DOBRA PRACE!", en: "CONGRATULATIONS!") },

            // Map Delete Save
            { "mapDeleteSave.title", (cs: "Smazat pozici?", en: "Delete save?") },

            // In-game messages
            { "message.gameLoaded", (cs: "Hra nactena", en: "Game loaded") },
            { "message.gameSaved", (cs: "Hra ulozena", en: "Game saved") },
            { "message.goToAnotherVillage", (cs: "Pojdme do dalsi vesnice!", en: "Lets go to another village!") },
            { "message.youWon", (cs: "VYHRAL JSI. Stesti zacatecnika?", en: "YOU WON. Beginner's luck.") },
            { "message.treasureStolen", (cs: "Zlaty poklad byl ukraden!", en: "The Golden Treasure was stolen") },
            { "message.soldierKilled", (cs: "Hrdinny vojak zabit ({0})", en: "Heroic soldier killed by {0}") },
            { "message.peasantKilled", (cs: "Nevinny sedlak zabit ({0})", en: "Innocent peasant killed by {0}") },
            { "message.farmerKilled", (cs: "Nevinny farmar zabit ({0})", en: "Innocent farmer killed by {0}") },
            { "message.medicKilled", (cs: "Nevinny medik zabit ({0})", en: "Innocent medic killed by {0}") },
            { "message.peasantToSoldier", (cs: "Sedlak => vojak", en: "Peasant => soldier") },
            { "message.peasantToFarmer", (cs: "Sedlak => farmar", en: "Peasant => farmer") },
            { "message.peasantToMedic", (cs: "Sedlak => medik", en: "Peasant => medic") },
            { "message.buildingStarted", (cs: "Stavba zahajena", en: "Building started") },
            { "message.notEnoughMoney", (cs: "Nedostatek penez", en: "Not enough money") },
            { "message.cantHoldMoney", (cs: "Neuneses vsechny ty penize", en: "Can't hold all this money") },
            { "message.cantHoldCartridges", (cs: "Neuneses vsechny ty naboje", en: "Can't hold all these cartridges") },
            { "message.noCartridges", (cs: "Zadne naboje", en: "No cartridges") },
            { "message.buildingBuilt", (cs: "Postavili jsme {0}", en: "{0} built") },
            { "message.buildingUpgraded", (cs: "Budova vylepsena", en: "Building upgraded") },
            { "message.towerUpgraded", (cs: "Vez vylepsena", en: "Tower upgraded") },
            { "message.weaponKitPurchased", (cs: "Naboje zakoupeny", en: "Weapon kit purchased") },
            { "message.armoryFull", (cs: "Zbrojnice je plna", en: "Armory is full") },
            { "message.toolPurchased", (cs: "Naradi zakoupeno", en: "Tool purchased") },
            { "message.farmFull", (cs: "Farma je plna", en: "Farm is full") },
            { "message.medicalKitPurchased", (cs: "Lekarnicka zakoupena", en: "Medical kit purchased") },
            { "message.cantBuyMoreMedicalKits", (cs: "Nelze koupit dalsi lekarnicku", en: "Can't buy any more medical kits") },
            { "message.cartridgePurchased", (cs: "Naboje zakoupeny", en: "Cartridge purchased") },
            { "message.shipBought", (cs: "Lod zakoupena", en: "Ship bought") },
            { "message.homelessHired", (cs: "Bezdomovec => sedlak", en: "Homeless hired => peasant") },
            { "message.enemiesComing", (cs: "Pripravte se, prichazi nepratele", en: "Brace yourselves, enemies are coming") },
            { "message.newDawn", (cs: "Novy den", en: "New dawn") },
            { "message.newHomelessAvailable", (cs: "Novy bezdomovec k dispozici", en: "New homeless available to hire!") },

            // Splash Screen
            { "splash.pressEnter", (cs: "Stiskni ENTER", en: "Press ENTER") },
            { "splash.pressButtonA", (cs: "Stiskni tlacitko A", en: "Press button A") },
            { "splash.touchToContinue", (cs: "Pokracuj dotykem", en: "Touch to continue") },

            // Splash Screen 2
            { "splash2.line1", (cs: "Ceskoslovenske legie byly vojenske", en: "The Czechoslovak Legion was a military force") },
            { "splash2.line2", (cs: "jednotky bojujici za spojence v 1. sv. valce.", en: "fighting for the Allies during WWI.") },
            { "splash2.line3", (cs: "Hlavnim cilem bylo ziskat podporu", en: "The main goal was to win the support of the") },
            { "splash2.line4", (cs: "pro nezavislost Ceskoslovenska", en: "Allies for the independence of Czechoslovakia") },
            { "splash2.line5", (cs: "na Rakousko-Uhersku. Usili legie", en: "from the Austria-Hungary. The Legion's efforts") },
            { "splash2.line6", (cs: "behem ruske obcanske valky zahrnovalo", en: "during the Russian Civil War included clearing") },
            { "splash2.line7", (cs: "obsazeni cele Transsibirske magistraly", en: "the entire Trans-Siberian Railway of") },
            { "splash2.line8", (cs: "z ruk bolseviku. Do Evropy se legionari", en: "Bolshevik forces. They evacuated to") },
            { "splash2.line9", (cs: "dostali az v roce 1920.", en: "Europe by 1920.") },
            { "splash2.disclaimer", (cs: "Tato hra je urcena pro zabavu a neni historicky presna.", en: "This game is for entertainment and is not historically accurate.") },

            // Credits
            { "credits.gameBy", (cs: "HRU VYTVORIL", en: "A GAME BY") },
            { "credits.art", (cs: "GRAFIKA", en: "ART") },
            { "credits.music", (cs: "HUDBA", en: "MUSIC") },
            { "credits.sounds", (cs: "ZVUKY", en: "SOUNDS") },
            { "credits.font", (cs: "FONTY", en: "FONT") },
            { "credits.tech", (cs: "TECHNOLOGIE", en: "TECH") },
            { "credits.communityHelp", (cs: "KONTRIBUTORI", en: "CONTRIBUTORS") },

            // Map Mission 1
            { "mission1.line1", (cs: "Kveten 1918. Byli jsme napadeni", en: "May 1918. We were attacked by") },
            { "mission1.line2", (cs: "Madary vernymi Centralnim mocnostem!", en: "the Hungarians loyal to the Central Powers!") },
            { "mission1.line3", (cs: "Brante vlak!", en: "Defend the train!") },
            { "mission1.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "mission1.goal1", (cs: "Oprav lokomotivu a jed na vychod", en: "Repair the locomotive and head east") },

            // Map Mission 2
            { "mission2.line1", (cs: "Kveten 1918. Prokleti bolsevici zautocili", en: "May 1918. The damned Bolsheviks attacked") },
            { "mission2.line2", (cs: "na vlak legie ve stanici! Branit!", en: "the Legion train at the station! Defend!") },
            { "mission2.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "mission2.goal1", (cs: "Oprav lokomotivu a jed na vychod", en: "Repair the locomotive and head east") },
            { "mission2.tips", (cs: "TIPY", en: "TIPS") },
            { "mission2.tip1", (cs: "Novy vagon - Nemocnice", en: "New wagon - Hospital") },

            // Map Mission 3
            { "mission3.line1", (cs: "Kveten 1918. Prokleti bolsevici vyhodili", en: "May 1918. The damned Bolsheviks blew up") },
            { "mission3.line2", (cs: "do povetri koleje!", en: "the rails!") },
            { "mission3.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "mission3.goal1", (cs: "Oprav poskozene koleje", en: "Repair damaged rails") },
            { "mission3.goal2", (cs: "Oprav lokomotivu a jed na vychod", en: "Repair the locomotive and head east") },
            { "mission3.tips", (cs: "TIPY", en: "TIPS") },
            { "mission3.tip1", (cs: "Novy vagon - Obrana vez", en: "New wagon - Defense Tower") },
            { "mission3.tip2", (cs: "Bez zbrojnice budes muset branit", en: "Absence of an Armory will make you defend") },
            { "mission3.tip3", (cs: "sve lidi sam.", en: "your people on your own.") },

            // Map Mission 4
            { "mission4.line1", (cs: "Cerven 1918. Prichazi velka bitva.", en: "June 1918. A great battle is coming. You") },
            { "mission4.line2", (cs: "Musis dobyt mesto Lipjag, kde se", en: "must capture the city of Lipjag, where") },
            { "mission4.line3", (cs: "shromazduje velke mnozstvi", en: "a large number of those Bolsheviks") },
            { "mission4.line4", (cs: "tech proradnych bolseviku.", en: "are gathered.") },
            { "mission4.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "mission4.goal1", (cs: "Oprav lokomotivu a jed na vychod", en: "Repair the locomotive and head east") },
            { "mission4.tips", (cs: "TIPY", en: "TIPS") },
            { "mission4.tip1", (cs: "Uderila cholera! Mel bych mit", en: "Cholera epidemic struck! Make sure to have") },
            { "mission4.tip2", (cs: "hodne mediku.", en: "a lot of medics.") },

            // Map Mission 5
            { "mission5.line1", (cs: "Cerven 1918. Musis dobyt mesto Ufa", en: "June 1918. You must capture the city of Ufa") },
            { "mission5.line2", (cs: "a prevzit kontrolu nad okolnimi vesnicemi.", en: "and take control over near villages.") },
            { "mission5.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "mission5.goal1", (cs: "Oprav poskozene koleje", en: "Repair damaged rails") },
            { "mission5.goal2", (cs: "Oprav lokomotivu a jed na vychod", en: "Repair the locomotive and head east") },
            { "mission5.tips", (cs: "TIPY", en: "TIPS") },
            { "mission5.tip1", (cs: "Novy vagon - Prodejna", en: "New wagon - Market") },
            { "mission5.tip2", (cs: "Uderila tvrda zima, budes muset najit", en: "A hard winter struck, you will have to find") },
            { "mission5.tip3", (cs: "jine zdroje penez nez farmareni.", en: "other sources of money than farming.") },

            // Map Mission 6
            { "mission6.line1", (cs: "Cervenec 1918. Prisel jsi pozde.", en: "July 1918. You arrived late. Only a week") },
            { "mission6.line2", (cs: "Jen tyden predtim bolsevici zavrazdili", en: "earlier, the Bolsheviks had murdered") },
            { "mission6.line3", (cs: "ruskeho cara a celou jeho rodinu.", en: "the Russian Tsar and his entire family.") },
            { "mission6.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "mission6.goal1", (cs: "Oprav lokomotivu a jed na vychod", en: "Repair the locomotive and head east") },
            { "mission6.tips", (cs: "TIPY", en: "TIPS") },
            { "mission6.tip1", (cs: "Nelze postavit Municni sklad - tva budoucnost", en: "Cannot build Arsenal - your future depends") },
            { "mission6.tip2", (cs: "zavisi na tvych hrdinskych vojacich.", en: "on your heroic soldiers.") },
            { "mission6.tip3", (cs: "Cholera uderila tvrde! Mel bych mit", en: "Cholera epidemic struck hard! Make sure") },
            { "mission6.tip4", (cs: "hodne mediku.", en: "to have lot of medics.") },

            // Map Mission 7
            { "mission7.line1", (cs: "Srpen 1918. Musis dobyt mesto Kazan", en: "August 1918. You must conquer the city") },
            { "mission7.line2", (cs: "a branit rusky zlaty poklad.", en: "of Kazan and defend the Russian Golden") },
            { "mission7.line3", (cs: "Poslouzi Bilym gardistum", en: "Treasure. It will serve the White Guards") },
            { "mission7.line4", (cs: "k financovani boje.", en: "to finance the fight.") },
            { "mission7.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "mission7.goal1", (cs: "Branit zlaty poklad", en: "Defend the Golden Treasure") },
            { "mission7.goal2", (cs: "Oprav poskozene koleje", en: "Repair damaged rails") },
            { "mission7.goal3", (cs: "Oprav lokomotivu a jed na vychod", en: "Repair the locomotive and head east") },
            { "mission7.tips", (cs: "TIPY", en: "TIPS") },
            { "mission7.tip1", (cs: "Zlaty poklad nesmi byt ztracen!!", en: "The Golden Treasure cannot be lost!!") },

            // Map Mission 8
            { "mission8.line1", (cs: "Leden 1919. Cas jit domu. Ovladl jsi", en: "January 1919. Time to go home. You took") },
            { "mission8.line2", (cs: "celou Transsibirskou magistralu,", en: "control the entire Trans-Siberian Railway,") },
            { "mission8.line3", (cs: "ale slibena pomoc od Spojencu", en: "but the promised help from the Allied Powers") },
            { "mission8.line4", (cs: "neprisla.", en: "did not come.") },
            { "mission8.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "mission8.goal1", (cs: "Kup lod a jed domu", en: "Buy the ship and go home") },
            { "mission8.tips", (cs: "TIPY", en: "TIPS") },
            { "mission8.tip1", (cs: "Prokleti bolsevici utoci pouze", en: "Damned Bolsheviks will attact only from") },
            { "mission8.tip2", (cs: "zleva. Se vsim, co maji.", en: "the left side. With all they got.") },
            { "mission8.tip3", (cs: "I s mechanizovanymi Leniny!", en: "Even with Mechanized Lenins.") },
            { "mission8.tip4", (cs: "Cholera uderila tvrde! Mel bych mit", en: "Cholera epidemic struck hard! Make sure") },
            { "mission8.tip5", (cs: "hodne mediku.", en: "to have lot of medics.") },

            // Survival
            { "survival.line1", (cs: "Nekonecne farmareni.", en: "Endless farming.") },
            { "survival.line2", (cs: "Nekonecna jatka.", en: "Endless slaughter.") },
            { "survival.line3", (cs: "Toto je zivot legionare.", en: "This is the life of a legionnaire.") },
            { "survival.line4", (cs: "Toto je boj o preziti.", en: "This is the survival.") },
            { "survival.goals", (cs: "CILE MISE", en: "MISSION GOALS") },
            { "survival.goal1", (cs: "Neomezeny cas. Prezij co nejdele.", en: "Unlimited time. Survive as long as you can.") },
            { "survival.tips", (cs: "TIPY", en: "TIPS") },
            { "survival.tip1", (cs: "Cholera uderila tvrde! Mel bych mit", en: "Cholera epidemic struck hard! Make sure to have") },
            { "survival.tip2", (cs: "hodne mediku.", en: "a lot of medics.") },
            { "survival.tip3", (cs: "Silnejsi nepratele utoci velmi brzy.", en: "Stronger enemies will attack very soon.") },

            // Tutorial messages
            { "tutorial.buildBase", (cs: "Mel bych postavit zakladnu", en: "I should build the Base") },
            { "tutorial.hirePeopleForBase", (cs: "Mel bych najat bezdomovce na vystavbu zakladny", en: "I should hire some people to build the Base") },
            { "tutorial.buildFarms", (cs: "Mel bych postavit farmy na vytvareni penez", en: "I should build some farms to generate money") },
            { "tutorial.hirePeopleForFarm", (cs: "Mel bych najat lidi na vystavbu farmy", en: "I should hire some people to build the farm") },
            { "tutorial.createTools", (cs: "Mel bych vyrobit naradi, abych mohl nabrat farmare", en: "I should create some farm tools to be able to recruit farmers") },
            { "tutorial.hireFarmers", (cs: "Mel bych najat lidi, kteri se stanu farmari", en: "I should hire some people to become farmers") },
            { "tutorial.buildArmory", (cs: "Mel bych postavit zbrojnici, abych mohl nabrat vojaky", en: "I should build the Armory to be able to recruit soldiers") },
            { "tutorial.hirePeopleForArmory", (cs: "Mel bych najat lidi na vystavbu zbrojnice", en: "I should hire some people to build the Armory") },
            { "tutorial.createWeapons", (cs: "Mel bych vyrobit zbrane, abych mohl nabrat vojaky", en: "I should create some weapons to be able to recruit soldiers") },
            { "tutorial.hireSoldiers", (cs: "Mel bych najat lidi, kteri se stanou vojaky", en: "I should hire some people to become soldiers") },
            { "tutorial.buildArsenal", (cs: "Mel bych postavit municni sklad, abych mohl kupovat naboje", en: "I should build the Arsenal to be able to buy cartridges") },
            { "tutorial.hirePeopleForArsenal", (cs: "Mel bych najat lidi na vystavbu municniho skladu", en: "I should hire some people to build the Arsenal") },
            { "tutorial.defendAndEarn", (cs: "Mel bych branit zakladnu a vydelat dost penez na opravu lokomotivy", en: "I should defend the base and make enough money to repair the Locomotive") },

            // Action messages
            { "action.firstUpgradeBase", (cs: "Nejdriv musim vylepsit zakladnu", en: "First you need to upgrade the base") },
            { "action.firstRepairRails", (cs: "Nejdriv musim opravit vsechny koleje", en: "First you need to repair all rails") },
            { "action.offWeGo", (cs: "Jedeme!", en: "Off we go!") },
            { "action.weDidIt", (cs: "Povedlo se! Nazdar!", en: "We did it! Nazdar!") },

            // Player actions
            { "action.build", (cs: "Postavit", en: "Build") },
            { "action.hire", (cs: "Najmout", en: "Hire") },
            { "action.create", (cs: "Vyrobit", en: "Create") },
            { "action.upgrade", (cs: "Vylepsit", en: "Upgrade") },
            { "action.buy", (cs: "Koupit", en: "Buy") },
            { "action.repair", (cs: "Opravit", en: "Repair") },
            { "tower.upgrade", (cs: "tuto vez - sila {0} => {1}", en: "this tower - strength: {0} => {1}") },

            // Building names
            { "building.base", (cs: "Zakladnu", en: "Base") },
            { "building.armory", (cs: "Zbrojnici", en: "Armory") },
            { "building.arsenal", (cs: "Municni sklad", en: "Arsenal") },
            { "building.defenseTower", (cs: "Obrannou vez", en: "Defense Tower") },
            { "building.farm", (cs: "Farmu", en: "Farm") },
            { "building.hospital", (cs: "Nemocnici", en: "Hospital") },
            { "building.market", (cs: "Prodejnu", en: "Market") },
            { "building.locomotive", (cs: "Lokomotivu", en: "Locomotive") },
            { "building.rails", (cs: "Koleje", en: "Rails") },
            { "building.ship", (cs: "Lod", en: "Ship") },
            { "building.treasure", (cs: "Zlaty poklad", en: "Golden Treasure") },

            // Items
            { "item.weapon", (cs: "Zbrane", en: "Weapon") },
            { "item.tool", (cs: "Naradi", en: "Tool") },
            { "item.medicalKit", (cs: "Lekarnicku", en: "Medical Kit") },
            { "item.cartridge", (cs: "Naboje", en: "Cartridge") },

            // Enemies
            { "enemy.rogueBolshevik", (cs: "Proradny bolsevik", en: "Rogue Bolshevik") },
            { "enemy.pigRider", (cs: "Praseci jezdec", en: "Pig Rider") },
            { "enemy.leninTractor", (cs: "Motorizovany Lenin", en: "Lenin Tractor") },

            // People
            { "people.homeless", (cs: "Bezdomovce", en: "Homeless") },

            // Stats
            { "stats.day", (cs: "Den ", en: "Day ") },
            { "stats.kills", (cs: "Zabiti: ", en: "Kills: ") },
            { "stats.money", (cs: "Penize: ", en: "Money: ") },
        };

        internal static void Validate()
        {
            foreach (KeyValuePair<string, (string cs, string en)> item in Values)
            {
                if (item.Value.cs == null || item.Value.en == null)
                {
                    throw new InvalidOperationException("Translation '" + item.Key + "' contains a null value.");
                }

                HashSet<int> csArguments = GetFormatArgumentIndexes(item.Key, "cs", item.Value.cs);
                HashSet<int> enArguments = GetFormatArgumentIndexes(item.Key, "en", item.Value.en);
                if (!csArguments.SetEquals(enArguments))
                {
                    throw new InvalidOperationException(
                        "Translation '" + item.Key + "' uses different format arguments in Czech and English.");
                }
            }
        }

        private static HashSet<int> GetFormatArgumentIndexes(string key, string language, string text)
        {
            var result = new HashSet<int>();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '{')
                {
                    if (i + 1 < text.Length && text[i + 1] == '{')
                    {
                        i++;
                        continue;
                    }

                    int cursor = i + 1;
                    int numberStart = cursor;
                    while (cursor < text.Length && char.IsDigit(text[cursor]))
                    {
                        cursor++;
                    }

                    if (cursor == numberStart)
                    {
                        ThrowInvalidFormat(key, language);
                    }

                    int argumentIndex = int.Parse(
                        text.Substring(numberStart, cursor - numberStart),
                        CultureInfo.InvariantCulture);

                    while (cursor < text.Length && text[cursor] != '}')
                    {
                        if (text[cursor] == '{')
                        {
                            ThrowInvalidFormat(key, language);
                        }
                        cursor++;
                    }

                    if (cursor >= text.Length)
                    {
                        ThrowInvalidFormat(key, language);
                    }

                    result.Add(argumentIndex);
                    i = cursor;
                }
                else if (text[i] == '}')
                {
                    if (i + 1 < text.Length && text[i + 1] == '}')
                    {
                        i++;
                        continue;
                    }

                    ThrowInvalidFormat(key, language);
                }
            }

            return result;
        }

        private static void ThrowInvalidFormat(string key, string language)
        {
            throw new FormatException(
                "Translation '" + key + "' contains an invalid " + language + " format string.");
        }
    }
}
