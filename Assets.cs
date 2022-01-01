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

        public static Texture2D Button;
        public static Texture2D ButtonPressed;
        public static Texture2D ButtonHover;

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
    }

    public class AssetsLoader
    {
        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Assets.Player = content.Load<Texture2D>("player");
            Assets.PlayerDown = content.Load<Texture2D>("Player/walkingDown");
            Assets.PlayerUp = content.Load<Texture2D>("Player/walkingUp");
            Assets.PlayerLeft = content.Load<Texture2D>("Player/walkingLeft");
            Assets.PlayerRight = content.Load<Texture2D>("Player/walkingRight");

            Assets.Button = content.Load<Texture2D>("button");
            Assets.ButtonPressed = content.Load<Texture2D>("button_pressed");
            Assets.ButtonHover = content.Load<Texture2D>("button_hover");

            Assets.Bullet = content.Load<Texture2D>("bullet");

            Assets.Background = content.Load<Texture2D>("Middle");
            Assets.Tunnel = content.Load<Texture2D>("tunnel");

            Assets.FontSmall = content.Load<SpriteFont>("fontSmall");
            Assets.FontMedium = content.Load<SpriteFont>("fontMedium");
            Assets.FontLarge = content.Load<SpriteFont>("fontLarge");

            Assets.Blip = content.Load<SoundEffect>("Sounds/blip");
            Assets.Nature = content.Load<Song>("Sounds/nature");
        }
    }
}
