using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.TextureAtlases;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar
{
    public static class Assets
    {
        public static Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
        public static Dictionary<string, TilesetGroup> TilesetGroups = new Dictionary<string, TilesetGroup>();
        public static Dictionary<int, WeatherType> TilesetType;
        public static Dictionary<string, TilesetGroup> TilesetEdges = new Dictionary<string, TilesetGroup>();
        public static Dictionary<string, TextureRegion2D> ParticleTextureRegions = new Dictionary<string, TextureRegion2D>();
        public static Dictionary<string, List<Song>> SongsCollection = new Dictionary<string, List<Song>>();
        public static Dictionary<string, List<SoundEffect>> SoundsCollection = new Dictionary<string, List<SoundEffect>>();
        public static Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();
    }

    public class AssetsLoader
    {
        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // load images
            Assets.Images["PlayerLeft"] = content.Load<Texture2D>("Player/player_left");
            Assets.Images["PlayerRight"] = content.Load<Texture2D>("Player/player_right");
            Assets.Images["SoldierLeft"] = content.Load<Texture2D>("Soldier/soldier_left");
            Assets.Images["SoldierRight"] = content.Load<Texture2D>("Soldier/soldier_right");
            Assets.Images["PeasantLeft"] = content.Load<Texture2D>("Peasant/peasant_left");
            Assets.Images["PeasantRight"] = content.Load<Texture2D>("Peasant/peasant_right");
            Assets.Images["HomelessLeft"] = content.Load<Texture2D>("Homeless/homeless_left");
            Assets.Images["HomelessRight"] = content.Load<Texture2D>("Homeless/homeless_right");
            Assets.Images["FarmerLeft"] = content.Load<Texture2D>("Farmer/farmer_left");
            Assets.Images["FarmerRight"] = content.Load<Texture2D>("Farmer/farmer_right");
            Assets.Images["MedicLeft"] = content.Load<Texture2D>("Medic/medic_left");
            Assets.Images["MedicRight"] = content.Load<Texture2D>("Medic/medic_right");

            Assets.Images["Enemy1Left"] = content.Load<Texture2D>("Enemy/enemy1_left");
            Assets.Images["Enemy1Right"] = content.Load<Texture2D>("Enemy/enemy1_right");
            Assets.Images["Enemy2Left"] = content.Load<Texture2D>("Enemy/enemy2_left");
            Assets.Images["Enemy2Right"] = content.Load<Texture2D>("Enemy/enemy2_right");
            Assets.Images["Enemy3Left"] = content.Load<Texture2D>("Enemy/enemy3_left");
            Assets.Images["Enemy3Right"] = content.Load<Texture2D>("Enemy/enemy3_right");
            Assets.Images["Enemy4Left"] = content.Load<Texture2D>("Enemy/enemy4_left");
            Assets.Images["Enemy4Right"] = content.Load<Texture2D>("Enemy/enemy4_right");
            Assets.Images["Enemy5Left"] = content.Load<Texture2D>("Enemy/enemy5_left");
            Assets.Images["Enemy5Right"] = content.Load<Texture2D>("Enemy/enemy5_right");

            Assets.Images["PigLeft"] = content.Load<Texture2D>("Pig/pig_left");
            Assets.Images["PigRight"] = content.Load<Texture2D>("Pig/pig_right");

            Assets.Images["Lenin"] = content.Load<Texture2D>("Lenin/lenin");

            Assets.Images["BulletLeft"] = content.Load<Texture2D>("Bullet/bullet_left");
            Assets.Images["BulletRight"] = content.Load<Texture2D>("Bullet/bullet_right");
            Assets.Images["BulletStatic"] = content.Load<Texture2D>("Bullet/bullet");
            Assets.Images["Coin"] = content.Load<Texture2D>("Coin/coin");
            Assets.Images["CoinStatic"] = content.Load<Texture2D>("Coin/coin_static");
            Assets.Images["Crate"] = content.Load<Texture2D>("Tools/crate");
            Assets.Images["Crate2"] = content.Load<Texture2D>("Tools/crate2");
            Assets.Images["MedicalKit"] = content.Load<Texture2D>("Tools/medicalkit");
            Assets.Images["Sword"] = content.Load<Texture2D>("Sword/Sword");

            Assets.Images["Construction"] = content.Load<Texture2D>("Buildings/Construction");
            Assets.Images["Center"] = content.Load<Texture2D>("Buildings/Center");
            Assets.Images["CenterRuins"] = content.Load<Texture2D>("Buildings/CenterRuins");
            Assets.Images["Armory"] = content.Load<Texture2D>("Buildings/Armory");
            Assets.Images["ArmoryRuins"] = content.Load<Texture2D>("Buildings/ArmoryRuins");
            Assets.Images["Arsenal"] = content.Load<Texture2D>("Buildings/Arsenal");
            Assets.Images["ArsenalRuins"] = content.Load<Texture2D>("Buildings/ArsenalRuins");
            Assets.Images["Tower"] = content.Load<Texture2D>("Buildings/Tower");
            Assets.Images["TowerRuins"] = content.Load<Texture2D>("Buildings/TowerRuins");
            Assets.Images["TowerFiring"] = content.Load<Texture2D>("Buildings/Tower_firing");
            Assets.Images["Farm"] = content.Load<Texture2D>("Buildings/Farm");
            Assets.Images["FarmRuins"] = content.Load<Texture2D>("Buildings/FarmRuins");
            Assets.Images["Hospital"] = content.Load<Texture2D>("Buildings/Hospital");
            Assets.Images["HospitalRuins"] = content.Load<Texture2D>("Buildings/HospitalRuins");
            Assets.Images["Locomotive"] = content.Load<Texture2D>("Buildings/Locomotive");
            Assets.Images["LocomotiveRuins"] = content.Load<Texture2D>("Buildings/LocomotiveRuins");
            Assets.Images["Ship"] = content.Load<Texture2D>("Ship/Ship");
            Assets.Images["Slum1"] = content.Load<Texture2D>("Buildings/Slum");
            Assets.Images["Slum2"] = content.Load<Texture2D>("Buildings/Slum2");
            Assets.Images["Market"] = content.Load<Texture2D>("Buildings/Market");
            Assets.Images["MarketRuins"] = content.Load<Texture2D>("Buildings/MarketRuins");
            Assets.Images["MarketActive"] = content.Load<Texture2D>("Buildings/Market_active");
            Assets.Images["RailsRuins"] = content.Load<Texture2D>("Buildings/RailsRuins");
            Assets.Images["Treasure1"] = content.Load<Texture2D>("Treasure/Treasure_1");
            Assets.Images["Treasure2"] = content.Load<Texture2D>("Treasure/Treasure_2");
            Assets.Images["Treasure3"] = content.Load<Texture2D>("Treasure/Treasure_3");
            Assets.Images["Treasure4"] = content.Load<Texture2D>("Treasure/Treasure_4");

            Assets.Images["KeyboardUp"] = content.Load<Texture2D>("Controls/keyboard/up");
            Assets.Images["KeyboardRight"] = content.Load<Texture2D>("Controls/keyboard/right");
            Assets.Images["KeyboardDown"] = content.Load<Texture2D>("Controls/keyboard/down");
            Assets.Images["KeyboardLeft"] = content.Load<Texture2D>("Controls/keyboard/left");
            Assets.Images["KeyboardSpace"] = content.Load<Texture2D>("Controls/keyboard/space");

            Assets.Images["GamepadLeftStick"] = content.Load<Texture2D>("Controls/gamepad/left_stick");
            Assets.Images["GamepadLeft"] = content.Load<Texture2D>("Controls/gamepad/left");
            Assets.Images["GamepadRight"] = content.Load<Texture2D>("Controls/gamepad/right");
            Assets.Images["GamepadSelect"] = content.Load<Texture2D>("Controls/gamepad/select");
            Assets.Images["GamepadA"] = content.Load<Texture2D>("Controls/gamepad/a");
            Assets.Images["GamepadB"] = content.Load<Texture2D>("Controls/gamepad/b");
            Assets.Images["GamepadX"] = content.Load<Texture2D>("Controls/gamepad/x");
            Assets.Images["GamepadY"] = content.Load<Texture2D>("Controls/gamepad/y");

            Assets.Images["MountainGrass"] = content.Load<Texture2D>("Parallax/mountain_grass");
            Assets.Images["MountainSnow"] = content.Load<Texture2D>("Parallax/mountain_snow");
            Assets.Images["Trees"] = content.Load<Texture2D>("Parallax/trees");
            Assets.Images["Clouds_bg"] = content.Load<Texture2D>("Parallax/clouds_bg");
            Assets.Images["Clouds_mg_1"] = content.Load<Texture2D>("Parallax/clouds_mg_1");
            Assets.Images["Clouds_mg_2"] = content.Load<Texture2D>("Parallax/clouds_mg_2");
            Assets.Images["Clouds_mg_3"] = content.Load<Texture2D>("Parallax/clouds_mg_3");

            // load fonts
            Assets.Fonts["Small"] = content.Load<SpriteFont>("Fonts/fontSmall");
            Assets.Fonts["Medium"] = content.Load<SpriteFont>("Fonts/fontMedium");
            Assets.Fonts["Large"] = content.Load<SpriteFont>("Fonts/fontLarge");

            // load sounds
            Assets.Sounds["Coin"] = content.Load<SoundEffect>("Sounds/coinsplash");
            Assets.Sounds["Rock"] = content.Load<SoundEffect>("Sounds/rock");
            Assets.Sounds["CannonShoot"] = content.Load<SoundEffect>("Sounds/shoot/cannon_fire_double");
            Assets.SoundsCollection["PlayerShoots"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/shoot/soldier_bang_01"),
                content.Load<SoundEffect>("Sounds/shoot/soldier_bang_07"),
            };
            Assets.SoundsCollection["SoldierShoots"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/shoot/soldier_bang_04"),
                content.Load<SoundEffect>("Sounds/shoot/soldier_cannon_02"),
            };
            Assets.SoundsCollection["Jumps"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/jumps/slightscream-01"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-02"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-03"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-04"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-05"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-06"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-07"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-08"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-09"),
                content.Load<SoundEffect>("Sounds/jumps/slightscream-10"),
            };
            Assets.SoundsCollection["EnemyDeaths"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/zombies/Death"),
                content.Load<SoundEffect>("Sounds/zombies/Death2"),
                content.Load<SoundEffect>("Sounds/zombies/Death3"),
                content.Load<SoundEffect>("Sounds/zombies/monster-6"),
                content.Load<SoundEffect>("Sounds/zombies/monster-15"),
                content.Load<SoundEffect>("Sounds/zombies/monster-16"),
                content.Load<SoundEffect>("Sounds/zombies/monster-17"),
            };
            Assets.SoundsCollection["EnemySpawns"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/zombies/Roar1"),
                content.Load<SoundEffect>("Sounds/zombies/Roar2"),
                content.Load<SoundEffect>("Sounds/zombies/Roar3"),
                content.Load<SoundEffect>("Sounds/zombies/monster-1"),
                content.Load<SoundEffect>("Sounds/zombies/monster-4"),
                content.Load<SoundEffect>("Sounds/zombies/monster-9"),
            };
            Assets.SoundsCollection["PigDeaths"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/pig/pig_hit1"),
                content.Load<SoundEffect>("Sounds/pig/pig_hit2"),
            };
            Assets.SoundsCollection["PigSpawns"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/pig/pig_idle"),
                content.Load<SoundEffect>("Sounds/pig/pig_idle2"),
                content.Load<SoundEffect>("Sounds/pig/pig_idle4"),
            };
            Assets.Sounds["LeninDeath"] = content.Load<SoundEffect>("Sounds/lenin/cykablyat");
            Assets.Sounds["LeninSpawn"] = content.Load<SoundEffect>("Sounds/lenin/tractor");
            Assets.SoundsCollection["SoldierDeaths"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/soldiers/die1"),
                content.Load<SoundEffect>("Sounds/soldiers/hit1"),
                content.Load<SoundEffect>("Sounds/soldiers/hit2"),
                content.Load<SoundEffect>("Sounds/soldiers/hit3"),
                content.Load<SoundEffect>("Sounds/soldiers/hit4"),
                content.Load<SoundEffect>("Sounds/soldiers/hit5"),
            };
            Assets.Sounds["SoldierSpawn"] = content.Load<SoundEffect>("Sounds/soldiers/metal_03");
            Assets.SoundsCollection["Clicks"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/clicks/j1987 blinkers click1"),
                content.Load<SoundEffect>("Sounds/clicks/j1987 blinkers click2"),
                content.Load<SoundEffect>("Sounds/clicks/j1987 blinkers click3"),
                content.Load<SoundEffect>("Sounds/clicks/j1987 blinkers click4"),
                content.Load<SoundEffect>("Sounds/clicks/j1987 blinkers click5"),
                content.Load<SoundEffect>("Sounds/clicks/j1987 blinkers click6"),
            };

            // load songs
            Assets.SongsCollection["Menu"] = new List<Song> {
                content.Load<Song>("Songs/menu/sleep_musicv3")
            };
            Assets.SongsCollection["Day"] = new List<Song>
            {
                content.Load<Song>("Songs/chill/a_cup_of_tea"),
                content.Load<Song>("Songs/chill/bartender"),
                content.Load<Song>("Songs/chill/cue"),
                content.Load<Song>("Songs/chill/chill_lofi"),
                content.Load<Song>("Songs/chill/lofiagain"),
                content.Load<Song>("Songs/chill/IcedVillage"),
                content.Load<Song>("Songs/chill/thawing_village_8_bit_lofi"),
                content.Load<Song>("Songs/chill/TheSettlementoftheFrontier.loop"),
            };
            Assets.SongsCollection["Night"] = new List<Song>
            {
                content.Load<Song>("Songs/metal/Dragged_Through_Hellfire"),
                content.Load<Song>("Songs/metal/Fight_Them_Until_We_Cant"),
                content.Load<Song>("Songs/metal/Oklahoma_Motel_Larvae"),
                content.Load<Song>("Songs/metal/The_Reach_Of_Hunger1"),
                content.Load<Song>("Songs/metal/The_Reach_Of_Hunger2"),
                content.Load<Song>("Songs/metal/The_Reach_Of_Hunger3"),
                content.Load<Song>("Songs/metal/The_Recon_Mission"),
                content.Load<Song>("Songs/metal/ente_evil"),
                content.Load<Song>("Songs/metal/Tri-Tachyon-4HR.DM8_"),
                content.Load<Song>("Songs/metal/good_as_nu_heavy_loop"),
            };

            // load effects
            Assets.Effects["AllWhite"] = content.Load<Effect>("Effects/AllWhite");
            Assets.Effects["Pixelate"] = content.Load<Effect>("Effects/Pixelate");
            Assets.Effects["Pixelate"].Parameters["pixelation"].SetValue(5);

            // load particle textures
            Texture2D blood = new Texture2D(graphicsDevice, 1, 1);
            blood.SetData(new[] { MyColor.Red });
            Assets.ParticleTextureRegions["Blood"] = new TextureRegion2D(blood);

            Texture2D smoke = new Texture2D(graphicsDevice, 1, 1);
            smoke.SetData(new[] { MyColor.Gray3 });
            Assets.ParticleTextureRegions["Smoke"] = new TextureRegion2D(smoke);

            Texture2D smokeDark = new Texture2D(graphicsDevice, 1, 1);
            smokeDark.SetData(new[] { MyColor.Gray4 });
            Assets.ParticleTextureRegions["SmokeDark"] = new TextureRegion2D(smokeDark);

            Texture2D shit = new Texture2D(graphicsDevice, 1, 1);
            shit.SetData(new[] { MyColor.LightBrown });
            Assets.ParticleTextureRegions["Shit"] = new TextureRegion2D(shit);

            Texture2D heal = new Texture2D(graphicsDevice, 1, 1);
            heal.SetData(new[] { MyColor.Aqua });
            Assets.ParticleTextureRegions["Heal"] = new TextureRegion2D(heal);

            Texture2D construction = new Texture2D(graphicsDevice, 1, 1);
            construction.SetData(new[] { MyColor.Gray2 });
            Assets.ParticleTextureRegions["Construction"] = new TextureRegion2D(construction);

            // load tilesets -----------------------------------------------------------------------------------
            Assets.TilesetType = new Dictionary<int, WeatherType>()
            {
                { 1, WeatherType.Grass },
                { 2, WeatherType.Snow },
                { 3, WeatherType.Grass },
                { 4, WeatherType.Grass },
                { 5, WeatherType.Snow },
                { 6, WeatherType.Grass },
                { 7, WeatherType.Snow },
                { 8, WeatherType.Grass },
            };
            // Set build action to Copy in MGCB
            // streams are here because Android, but TitleContainer is cross platform so cool
            foreach (var tilesetType in Assets.TilesetType)
            {
                Assets.TilesetGroups["village" + tilesetType.Key] = new TilesetGroup(
                    TitleContainer.OpenStream(@"Content/Envs/" + tilesetType.Key + "_village.tmx"),
                    TitleContainer.OpenStream(@"Content/Envs/tileset.tsx"),
                    content.Load<Texture2D>("Envs/tileset")
                );

                // load corresponding edges
                Assets.TilesetEdges["left" + tilesetType.Key] = new TilesetGroup(
                    TitleContainer.OpenStream(@"Content/Envs/left_" + tilesetType.Value.ToString().ToLower() + ".tmx"),
                    TitleContainer.OpenStream(@"Content/Envs/tileset.tsx"),
                    content.Load<Texture2D>("Envs/tileset")
                );
                Assets.TilesetEdges["right" + tilesetType.Key] = new TilesetGroup(
                    TitleContainer.OpenStream(@"Content/Envs/right_" + tilesetType.Value.ToString().ToLower() + ".tmx"),
                    TitleContainer.OpenStream(@"Content/Envs/tileset.tsx"),
                    content.Load<Texture2D>("Envs/tileset")
                );
            }

            // load sea edge at village 8
            Assets.TilesetEdges["right8"] = new TilesetGroup(
                TitleContainer.OpenStream(@"Content/Envs/right_sea.tmx"),
                TitleContainer.OpenStream(@"Content/Envs/tileset.tsx"),
                content.Load<Texture2D>("Envs/tileset")
            );
        }
    }
}
