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
                    { "menu.survival", "Rezim preziti" },
                    { "menu.controls", "Ovladani" },
                    { "menu.credits", "Autori" },
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
                    { "credits.title", "Autori" },
                    { "credits.officialPage", "Oficialni stranka" },
                    { "credits.coffee", "Kup mi kafe!" },

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
                    { "gameFinished.title", "DOBRA PRACE!" },

                    // Map Delete Save
                    { "mapDeleteSave.title", "Smazat pozici?" },

                    // In-game messages
                    { "message.gameLoaded", "Hra nactena" },
                    { "message.gameSaved", "Hra ulozena" },
                    { "message.goToAnotherVillage", "Pojdme do dalsi vesnice!" },
                    { "message.youWon", "VYHRAL JSI. Stesti zacatecnika?" },
                    { "message.treasureStolen", "Zlaty poklad byl ukraden!" },
                    { "message.soldierKilled", "Hrdinny vojak zabit ({0})" },
                    { "message.peasantKilled", "Nevinny sedlak zabit ({0})" },
                    { "message.farmerKilled", "Nevinny farmar zabit ({0})" },
                    { "message.medicKilled", "Nevinny medik zabit ({0})" },
                    { "message.peasantToSoldier", "Sedlak => vojak" },
                    { "message.peasantToFarmer", "Sedlak => farmar" },
                    { "message.peasantToMedic", "Sedlak => medik" },
                    { "message.buildingStarted", "Stavba zahajena" },
                    { "message.notEnoughMoney", "Nedostatek penez" },
                    { "message.cantHoldMoney", "Neuneses vsechny ty penize" },
                    { "message.cantHoldCartridges", "Neuneses vsechny ty naboje" },
                    { "message.noCartridges", "Zadne naboje" },
                    { "message.buildingBuilt", "Postavili jsme {0}" },
                    { "message.buildingUpgraded", "Budova vylepsena" },
                    { "message.towerUpgraded", "Vez vylepsena" },
                    { "message.weaponKitPurchased", "Naboje zakoupeny" },
                    { "message.armoryFull", "Zbrojnice je plna" },
                    { "message.toolPurchased", "Naradi zakoupeno" },
                    { "message.farmFull", "Farma je plna" },
                    { "message.medicalKitPurchased", "Lekarnicka zakoupena" },
                    { "message.cantBuyMoreMedicalKits", "Nelze koupit dalsi lekarnicku" },
                    { "message.cartridgePurchased", "Naboje zakoupeny" },
                    { "message.shipBought", "Lod zakoupena" },
                    { "message.homelessHired", "Bezdomovec => sedlak" },
                    { "message.enemiesComing", "Pripravte se, prichazi nepratele" },
                    { "message.newDawn", "Novy den" },
                    { "message.newHomelessAvailable", "Novy bezdomovec k dispozici" },

                    // Splash Screen
                    { "splash.pressEnter", "Stiskni ENTER" },
                    { "splash.pressButtonA", "Stiskni tlacitko A" },
                    { "splash.touchToContinue", "Pokracuj dotykem" },

                    // Splash Screen 2
                    { "splash2.line1", "Ceskoslovenske legie byly vojenske" },
                    { "splash2.line2", "jednotky bojujici za spojence v 1. sv. valce." },
                    { "splash2.line3", "Hlavnim cilem bylo ziskat podporu" },
                    { "splash2.line4", "pro nezavislost Ceskoslovenska" },
                    { "splash2.line5", "na Rakousko-Uhersku. Usili legie" },
                    { "splash2.line6", "behem ruske obcanske valky zahrnovalo" },
                    { "splash2.line7", "obsazeni cele Transsibirske magistraly" },
                    { "splash2.line8", "z ruk bolseviku. Do Evropy se legionari" },
                    { "splash2.line9", "dostali az v roce 1920." },
                    { "splash2.disclaimer", "Tato hra je urcena pro zabavu a neni historicky presna." },

                    // Credits
                    { "credits.gameBy", "HRU VYTVORIL" },
                    { "credits.art", "GRAFIKA" },
                    { "credits.music", "HUDBA" },
                    { "credits.sounds", "ZVUKY" },
                    { "credits.font", "FONTY" },
                    { "credits.tech", "TECHNOLOGIE" },

                    // Map Mission 1
                    { "mission1.line1", "Kveten 1918. Byli jsme napadeni" },
                    { "mission1.line2", "Madary vernymi Centralnim mocnostem!" },
                    { "mission1.line3", "Brante vlak!" },
                    { "mission1.goals", "CILE MISE" },
                    { "mission1.goal1", "Oprav lokomotivu a jed na vychod" },

                    // Map Mission 2
                    { "mission2.line1", "Kveten 1918. Prokleti bolsevici zautocili" },
                    { "mission2.line2", "na vlak legie ve stanici! Branit!" },
                    { "mission2.goals", "CILE MISE" },
                    { "mission2.goal1", "Oprav lokomotivu a jed na vychod" },
                    { "mission2.tips", "TIPY" },
                    { "mission2.tip1", "Novy vagon - Nemocnice" },

                    // Map Mission 3
                    { "mission3.line1", "Kveten 1918. Prokleti bolsevici vyhodili" },
                    { "mission3.line2", "do povetri koleje!" },
                    { "mission3.goals", "CILE MISE" },
                    { "mission3.goal1", "Oprav poskozene koleje" },
                    { "mission3.goal2", "Oprav lokomotivu a jed na vychod" },
                    { "mission3.tips", "TIPY" },
                    { "mission3.tip1", "Novy vagon - Obrana vez" },
                    { "mission3.tip2", "Bez zbrojnice budes muset branit" },
                    { "mission3.tip3", "sve lidi sam." },

                    // Map Mission 4
                    { "mission4.line1", "Cerven 1918. Prichazi velka bitva." },
                    { "mission4.line2", "Musis dobyt mesto Lipjag, kde se" },
                    { "mission4.line3", "shromazduje velke mnozstvi" },
                    { "mission4.line4", "tech proradnych bolseviku." },
                    { "mission4.goals", "CILE MISE" },
                    { "mission4.goal1", "Oprav lokomotivu a jed na vychod" },
                    { "mission4.tips", "TIPY" },
                    { "mission4.tip1", "Uderila cholera! Mel bych mit" },
                    { "mission4.tip2", "hodne mediku." },

                    // Map Mission 5
                    { "mission5.line1", "Cerven 1918. Musis dobyt mesto Ufa" },
                    { "mission5.line2", "a prevzit kontrolu nad okolnimi vesnicemi." },
                    { "mission5.goals", "CILE MISE" },
                    { "mission5.goal1", "Oprav poskozene koleje" },
                    { "mission5.goal2", "Oprav lokomotivu a jed na vychod" },
                    { "mission5.tips", "TIPY" },
                    { "mission5.tip1", "Novy vagon - Prodejna" },
                    { "mission5.tip2", "Uderila tvrda zima, budes muset najit" },
                    { "mission5.tip3", "jine zdroje penez nez farmareni." },

                    // Map Mission 6
                    { "mission6.line1", "Cervenec 1918. Prisel jsi pozde." },
                    { "mission6.line2", "Jen tyden predtim bolsevici zavrazdili" },
                    { "mission6.line3", "ruskeho cara a celou jeho rodinu." },
                    { "mission6.goals", "CILE MISE" },
                    { "mission6.goal1", "Oprav lokomotivu a jed na vychod" },
                    { "mission6.tips", "TIPY" },
                    { "mission6.tip1", "Nelze postavit Munièní sklad - tva budoucnost" },
                    { "mission6.tip2", "zavisi na tvych hrdinskych vojacich." },
                    { "mission6.tip3", "Cholera uderila tvrde! Mel bych mit" },
                    { "mission6.tip4", "hodne mediku." },

                    // Map Mission 7
                    { "mission7.line1", "Srpen 1918. Musis dobyt mesto Kazan" },
                    { "mission7.line2", "a branit rusky zlaty poklad." },
                    { "mission7.line3", "Poslouzi Bilym gardistum" },
                    { "mission7.line4", "k financovani boje." },
                    { "mission7.goals", "CILE MISE" },
                    { "mission7.goal1", "Branit zlaty poklad" },
                    { "mission7.goal2", "Oprav poskozene koleje" },
                    { "mission7.goal3", "Oprav lokomotivu a jed na vychod" },
                    { "mission7.tips", "TIPY" },
                    { "mission7.tip1", "Zlaty poklad nesmi byt ztracen!!" },

                    // Map Mission 8
                    { "mission8.line1", "Leden 1919. Cas jit domu. Ovladl jsi" },
                    { "mission8.line2", "celou Transsibirskou magistralu," },
                    { "mission8.line3", "ale slibena pomoc od Spojencu" },
                    { "mission8.line4", "neprisla." },
                    { "mission8.goals", "CILE MISE" },
                    { "mission8.goal1", "Kup lod a jed domu" },
                    { "mission8.tips", "TIPY" },
                    { "mission8.tip1", "Prokleti bolsevici utoci pouze" },
                    { "mission8.tip2", "zleva. Se vsim, co maji." },
                    { "mission8.tip3", "I s mechanizovanymi Leniny!" },
                    { "mission8.tip4", "Cholera uderila tvrde! Mel bych mit" },
                    { "mission8.tip5", "hodne mediku." },

                    // Survival
                    { "survival.line1", "Nekonecne farmareni." },
                    { "survival.line2", "Nekonecna jatka." },
                    { "survival.line3", "Toto je zivot legionare." },
                    { "survival.line4", "Toto je boj o preziti." },
                    { "survival.goals", "CILE MISE" },
                    { "survival.goal1", "Neomezeny cas. Prezij co nejdele." },
                    { "survival.tips", "TIPY" },
                    { "survival.tip1", "Cholera uderila tvrde! Mel bych mit" },
                    { "survival.tip2", "hodne mediku." },
                    { "survival.tip3", "Silnejsi nepratele utoci velmi brzy." },

                    // Tutorial messages
                    { "tutorial.buildBase", "Mel bych postavit zakladnu" },
                    { "tutorial.hirePeopleForBase", "Mel bych najat bezdomovce na vystavbu zakladny" },
                    { "tutorial.buildFarms", "Mel bych postavit farmy na vytvareni penez" },
                    { "tutorial.hirePeopleForFarm", "Mel bych najat lidi na vystavbu farmy" },
                    { "tutorial.createTools", "Mel bych vyrobit naradi, abych mohl nabrat farmare" },
                    { "tutorial.hireFarmers", "Mel bych najat lidi, kteri se stanu farmari" },
                    { "tutorial.buildArmory", "Mel bych postavit zbrojnici, abych mohl nabrat vojaky" },
                    { "tutorial.hirePeopleForArmory", "Mel bych najat lidi na vystavbu zbrojnice" },
                    { "tutorial.createWeapons", "Mel bych vyrobit zbrane, abych mohl nabrat vojaky" },
                    { "tutorial.hireSoldiers", "Mel bych najat lidi, kteri se stanou vojaky" },
                    { "tutorial.buildArsenal", "Mel bych postavit municni sklad, abych mohl kupovat naboje" },
                    { "tutorial.hirePeopleForArsenal", "Mel bych najat lidi na vystavbu municniho skladu" },
                    { "tutorial.defendAndEarn", "Mel bych branit zakladnu a vydelat dost penez na opravu lokomotivy" },

                    // Action messages
                    { "action.firstUpgradeBase", "Nejdriv musim vylepsit zakladnu" },
                    { "action.firstRepairRails", "Nejdriv musim opravit vsechny koleje" },
                    { "action.offWeGo", "Jedeme!" },
                    { "action.weDidIt", "Povedlo se! Nazdar!" },

                    // Player actions
                    { "action.build", "Postavit" },
                    { "action.hire", "Najmout" },
                    { "action.create", "Vyrobit" },
                    { "action.upgrade", "Vylepsit" },
                    { "action.buy", "Koupit" },
                    { "action.repair", "Opravit" },

                    // Building names
                    { "building.base", "Zakladnu" },
                    { "building.armory", "Zbrojnici" },
                    { "building.arsenal", "Municni sklad" },
                    { "building.defenseTower", "Obrannou vez" },
                    { "building.farm", "Farmu" },
                    { "building.hospital", "Nemocnici" },
                    { "building.market", "Prodejnu" },
                    { "building.locomotive", "Lokomotivu" },
                    { "building.rails", "Koleje" },
                    { "building.ship", "Lod" },
                    { "building.treasure", "Zlaty poklad" },

                    // Items
                    { "item.weapon", "Zbrane" },
                    { "item.tool", "Naradi" },
                    { "item.medicalKit", "Lekarnicku" },
                    { "item.cartridge", "Naboje" },

                    // Enemies
                    { "enemy.rogueBolshevik", "Proradny bolsevik" },
                    { "enemy.pigRider", "Praseci jezdec" },
                    { "enemy.leninTractor", "Motorizovany Lenin" },

                    // People
                    { "people.homeless", "Bezdomovce" },

                    // Stats
                    { "stats.day", "Den " },
                    { "stats.kills", "Zabiti: " },
                    { "stats.money", "Penize: " },
                };
        }
    }
}
