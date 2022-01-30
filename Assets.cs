namespace MyGame
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Media;

    public static class Assets
    {
        // images
        public static Texture2D Player;
        public static Texture2D PlayerDown;
        public static Texture2D PlayerUp;
        public static Texture2D PlayerLeft;
        public static Texture2D PlayerRight;

        public static Texture2D Bullet;

        public static Texture2D Background;
        public static Texture2D Tunnel;

        // fonts
        public static SpriteFont FontSmall;
        public static SpriteFont FontMedium;
        public static SpriteFont FontLarge;

        // audio
        public static SoundEffect Blip;
        public static Song Nature;

        // effects
        public static Effect AllWhite;
        public static Effect Pixelate;
    }

    public class AssetsLoader
    {
        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Assets.PlayerDown = content.Load<Texture2D>("Player/player_left");
            Assets.PlayerUp = content.Load<Texture2D>("Player/player_left");
            Assets.PlayerLeft = content.Load<Texture2D>("Player/player_left");
            Assets.PlayerRight = content.Load<Texture2D>("Player/player_left");

            Assets.Bullet = content.Load<Texture2D>("bullet");

            Assets.Background = content.Load<Texture2D>("Middle");
            Assets.Tunnel = content.Load<Texture2D>("tunnel");

            Assets.FontSmall = content.Load<SpriteFont>("Fonts/fontPublicPixelSmall");
            Assets.FontMedium = content.Load<SpriteFont>("Fonts/fontPublicPixelMedium");
            Assets.FontLarge = content.Load<SpriteFont>("Fonts/fontPublicPixelLarge");

            Assets.Blip = content.Load<SoundEffect>("Sounds/blip");
            Assets.Nature = content.Load<Song>("Sounds/nature");

            Assets.AllWhite = content.Load<Effect>("Effects/AllWhite");
            Assets.Pixelate = content.Load<Effect>("Effects/Pixelate");
            Assets.Pixelate.Parameters["pixelation"].SetValue(5);
        }
    }
}
