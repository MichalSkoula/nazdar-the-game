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
        // images
        public static Texture2D PlayerLeft;
        public static Texture2D PlayerRight;

        public static Texture2D SoldierLeft;
        public static Texture2D SoldierRight;

        public static Texture2D EnemyLeft;
        public static Texture2D EnemyRight;

        public static Texture2D BulletLeft;
        public static Texture2D BulletRight;

        // fonts
        public static SpriteFont FontSmall;
        public static SpriteFont FontMedium;
        public static SpriteFont FontLarge;

        // audio
        public static SoundEffect Blip;
        public static Song Nature;
        public static Song Map;

        // effects
        public static Effect AllWhite;
        public static Effect Pixelate;

        // tileset groups
        public static Dictionary<string, TilesetGroup> TilesetGroups;

        // particles
        public static Texture2D ParticleTexture;
        public static TextureRegion2D ParticleTextureRegion;
    }

    public class AssetsLoader
    {
        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Assets.PlayerLeft = content.Load<Texture2D>("Player/player_left");
            Assets.PlayerRight = content.Load<Texture2D>("Player/player_right");

            Assets.SoldierLeft = content.Load<Texture2D>("Soldier/soldier_left");
            Assets.SoldierRight = content.Load<Texture2D>("Soldier/soldier_right");

            Assets.EnemyLeft = content.Load<Texture2D>("Enemy/enemy_left");
            Assets.EnemyRight = content.Load<Texture2D>("Enemy/enemy_right");

            Assets.BulletLeft = content.Load<Texture2D>("Bullet/bullet_left");
            Assets.BulletRight = content.Load<Texture2D>("Bullet/bullet_right");

            Assets.FontSmall = content.Load<SpriteFont>("Fonts/fontPublicPixelSmall");
            Assets.FontMedium = content.Load<SpriteFont>("Fonts/fontPublicPixelMedium");
            Assets.FontLarge = content.Load<SpriteFont>("Fonts/fontPublicPixelLarge");

            Assets.Blip = content.Load<SoundEffect>("Sounds/blip");
            Assets.Nature = content.Load<Song>("Sounds/nature");
            Assets.Map = content.Load<Song>("Sounds/Map");

            Assets.AllWhite = content.Load<Effect>("Effects/AllWhite");
            Assets.Pixelate = content.Load<Effect>("Effects/Pixelate");
            Assets.Pixelate.Parameters["pixelation"].SetValue(5);

            // Set the "Copy to Output Directory" property of these two files to `Copy if newer` by clicking them in the solution explorer.
            Assets.TilesetGroups = new Dictionary<string, TilesetGroup>();
            Assets.TilesetGroups.Add(
                "village1",
                new TilesetGroup(
                    content.RootDirectory + "\\Envs\\first-village.tmx",
                    content.RootDirectory + "\\Envs\\tileset forest.tsx", 
                    content.Load<Texture2D>("Envs/Rocky Roads/tileset forest")
                )
            );

            // load particle texture
            Assets.ParticleTexture = new Texture2D(graphicsDevice, 1, 1);
            Assets.ParticleTexture.SetData(new[] { Color.Red });
            Assets.ParticleTextureRegion = new TextureRegion2D(Assets.ParticleTexture);
        }
    }
}
