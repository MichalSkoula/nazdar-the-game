using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TiledCS;

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

        public static Texture2D Bullet;

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

        // envs
        public static Texture2D TilesetTexture;
        public static TiledMap TilesetMap;
        public static TiledTileset Tileset;
        public static int TileWidth;
        public static int TileHeight;
        public static int TilesetTilesWide;
        public static int TilesetTilesHeight;
    }

    public class AssetsLoader
    {
        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Assets.PlayerLeft = content.Load<Texture2D>("Player/player_left");
            Assets.PlayerRight = content.Load<Texture2D>("Player/player_right");

            Assets.SoldierLeft = content.Load<Texture2D>("Soldier/soldier_left");
            Assets.SoldierRight = content.Load<Texture2D>("Soldier/soldier_right");

            Assets.EnemyLeft = content.Load<Texture2D>("Enemy/enemy_left");
            Assets.EnemyRight = content.Load<Texture2D>("Enemy/enemy_right");

            Assets.Bullet = content.Load<Texture2D>("bullet");

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
            Assets.TilesetMap = new TiledMap(content.RootDirectory + "\\Envs\\first-village.tmx");
            Assets.Tileset = new TiledTileset(content.RootDirectory + "\\Envs\\tileset forest.tsx");
            Assets.TilesetTexture = content.Load<Texture2D>("Envs/Rocky Roads/tileset forest");
            Assets.TileWidth = Assets.Tileset.TileWidth;
            Assets.TileHeight = Assets.Tileset.TileHeight;
            Assets.TilesetTilesWide = Assets.Tileset.Columns; // Amount of tiles on each row (left right)
            Assets.TilesetTilesHeight = Assets.Tileset.TileCount / Assets.Tileset.Columns; // Amount of tiels on each column (up down)
        }
    }
}
