using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.TextureAtlases;
using SiberianAnabasis.Shared;
using System.Collections.Generic;

namespace SiberianAnabasis
{
    public static class Assets
    {
        public static Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
        public static Dictionary<string, TilesetGroup> TilesetGroups = new Dictionary<string, TilesetGroup>();
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
            Assets.Images["EnemyLeft"] = content.Load<Texture2D>("Enemy/enemy_left");
            Assets.Images["EnemyRight"] = content.Load<Texture2D>("Enemy/enemy_right");
            Assets.Images["HomelessLeft"] = content.Load<Texture2D>("Homeless/homeless_left");
            Assets.Images["HomelessRight"] = content.Load<Texture2D>("Homeless/homeless_right");
            Assets.Images["BulletLeft"] = content.Load<Texture2D>("Bullet/bullet_left");
            Assets.Images["BulletRight"] = content.Load<Texture2D>("Bullet/bullet_right");
            Assets.Images["Coin"] = content.Load<Texture2D>("Items/coin");
            Assets.Images["Crate"] = content.Load<Texture2D>("Items/crate");
            Assets.Images["Center"] = content.Load<Texture2D>("Buildings/Center");
            Assets.Images["Armory"] = content.Load<Texture2D>("Buildings/Armory");
            Assets.Images["Tower"] = content.Load<Texture2D>("Buildings/Tower");

            Assets.Images["KeyboardUp"] = content.Load<Texture2D>("Controls/keyboard/up");
            Assets.Images["KeyboardRight"] = content.Load<Texture2D>("Controls/keyboard/right");
            Assets.Images["KeyboardDown"] = content.Load<Texture2D>("Controls/keyboard/down");
            Assets.Images["KeyboardLeft"] = content.Load<Texture2D>("Controls/keyboard/left");
            Assets.Images["KeyboardSpace"] = content.Load<Texture2D>("Controls/keyboard/space");

            Assets.Images["GamepadLeftStick"] = content.Load<Texture2D>("Controls/gamepad/left_stick");
            Assets.Images["GamepadA"] = content.Load<Texture2D>("Controls/gamepad/a");
            Assets.Images["GamepadB"] = content.Load<Texture2D>("Controls/gamepad/b");
            Assets.Images["GamepadX"] = content.Load<Texture2D>("Controls/gamepad/x");
            Assets.Images["GamepadY"] = content.Load<Texture2D>("Controls/gamepad/y");

            // load fonts
            Assets.Fonts["Small"] = content.Load<SpriteFont>("Fonts/fontPublicPixelSmall");
            Assets.Fonts["Medium"] = content.Load<SpriteFont>("Fonts/fontPublicPixelMedium");
            Assets.Fonts["Large"] = content.Load<SpriteFont>("Fonts/fontPublicPixelLarge");

            // load sounds
            Assets.Sounds["Blip"] = content.Load<SoundEffect>("Sounds/blip");
            Assets.Sounds["Explosion"] = content.Load<SoundEffect>("Sounds/explosion");
            Assets.Sounds["GunFire"] = content.Load<SoundEffect>("Sounds/gun_fire");
            Assets.Sounds["Jump"] = content.Load<SoundEffect>("Sounds/jump");
            Assets.Sounds["Coin"] = content.Load<SoundEffect>("Sounds/coin");
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
            Assets.SoundsCollection["SoldierDeaths"] = new List<SoundEffect> {
                content.Load<SoundEffect>("Sounds/soldiers/die1"),
                content.Load<SoundEffect>("Sounds/soldiers/hit1"),
                content.Load<SoundEffect>("Sounds/soldiers/hit2"),
                content.Load<SoundEffect>("Sounds/soldiers/hit3"),
                content.Load<SoundEffect>("Sounds/soldiers/hit4"),
                content.Load<SoundEffect>("Sounds/soldiers/hit5"),

            };
            Assets.Sounds["SoldierSpawn"] = content.Load<SoundEffect>("Sounds/soldiers/metal_03");

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
                content.Load<Song>("Songs/chill/lofiagain")
            };
            Assets.SongsCollection["Night"] = new List<Song>
            {
                content.Load<Song>("Songs/metal/Dragged_Through_Hellfire"),
                content.Load<Song>("Songs/metal/Fight_Them_Until_We_Cant"),
                content.Load<Song>("Songs/metal/Oklahoma_Motel_Larvae"),
                content.Load<Song>("Songs/metal/The_Reach_Of_Hunger"),
                content.Load<Song>("Songs/metal/The_Recon_Mission"),
            };

            // load effects
            Assets.Effects["AllWhite"] = content.Load<Effect>("Effects/AllWhite");
            Assets.Effects["Pixelate"] = content.Load<Effect>("Effects/Pixelate");
            Assets.Effects["Pixelate"].Parameters["pixelation"].SetValue(5);

            // load tilesets
            // Set the "Copy to Output Directory" property of these two files to `Copy if newer` by clicking them in the solution explorer.
            Assets.TilesetGroups["village1"] = new TilesetGroup(
                content.RootDirectory + "/Envs/1_village.tmx",
                content.RootDirectory + "/Envs/1_village.tsx",
                content.Load<Texture2D>("Envs/tileset1")
            );

            // load particle textures
            Texture2D blood = new Texture2D(graphicsDevice, 1, 1);
            blood.SetData(new[] { Color.Red });
            Assets.ParticleTextureRegions["Blood"] = new TextureRegion2D(blood);

            Texture2D smoke = new Texture2D(graphicsDevice, 1, 1);
            smoke.SetData(new[] { Color.DarkGray });
            Assets.ParticleTextureRegions["Smoke"] = new TextureRegion2D(smoke);
        }
    }
}
