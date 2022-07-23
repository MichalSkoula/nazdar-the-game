using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using SiberianAnabasis.Shared;
using MonoGame.Extended.TextureAtlases;
using Microsoft.Xna.Framework;

namespace SiberianAnabasis
{
    public static class Assets
    {
        public static Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> Songs = new Dictionary<string, Song>();
        public static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
        public static Dictionary<string, TilesetGroup> TilesetGroups = new Dictionary<string, TilesetGroup>();
        public static Dictionary<string, TextureRegion2D> ParticleTextureRegions = new Dictionary<string, TextureRegion2D>();
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
            Assets.Images["EnemyLeft"] = content.Load<Texture2D>("Enemy/enemy_left");
            Assets.Images["EnemyRight"] = content.Load<Texture2D>("Enemy/enemy_right");
            Assets.Images["HomelessLeft"] = content.Load<Texture2D>("Homeless/homeless_left");
            Assets.Images["HomelessRight"] = content.Load<Texture2D>("Homeless/homeless_right");
            Assets.Images["BulletLeft"] = content.Load<Texture2D>("Bullet/bullet_left");
            Assets.Images["BulletRight"] = content.Load<Texture2D>("Bullet/bullet_right");
            Assets.Images["Coin"] = content.Load<Texture2D>("Coin/coin");

            // load fonts
            Assets.Fonts["Small"] = content.Load<SpriteFont>("Fonts/fontPublicPixelSmall");
            Assets.Fonts["Medium"] = content.Load<SpriteFont>("Fonts/fontPublicPixelMedium");
            Assets.Fonts["Large"] = content.Load<SpriteFont>("Fonts/fontPublicPixelLarge");

            // load audio
            Assets.Sounds["Blip"] = content.Load<SoundEffect>("Sounds/blip");
            Assets.Songs["Nature"] = content.Load<Song>("Sounds/nature");
            Assets.Songs["Map"] = content.Load<Song>("Sounds/Map");

            // load effects
            Assets.Effects["AllWhite"] = content.Load<Effect>("Effects/AllWhite");
            Assets.Effects["Pixelate"] = content.Load<Effect>("Effects/Pixelate");
            Assets.Effects["Pixelate"].Parameters["pixelation"].SetValue(5);

            // load tilesets
            // Set the "Copy to Output Directory" property of these two files to `Copy if newer` by clicking them in the solution explorer.
            Assets.TilesetGroups["village1"] = new TilesetGroup(
                content.RootDirectory + "\\Envs\\first-village.tmx",
                content.RootDirectory + "\\Envs\\tileset forest.tsx", 
                content.Load<Texture2D>("Envs/Rocky Roads/tileset forest")
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
