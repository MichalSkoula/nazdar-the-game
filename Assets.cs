using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MyGame
{
    static class Assets
    {
        // images
        public static Texture2D player;
        public static Texture2D playerDown;
        public static Texture2D playerUp;
        public static Texture2D playerLeft;
        public static Texture2D playerRight;

        public static Texture2D button;
        public static Texture2D buttonPressed;
        public static Texture2D buttonHover;

        public static Texture2D bullet;

        public static Texture2D background;
        public static Texture2D tunnel;

        // fonts
        public static SpriteFont fontSmall;
        public static SpriteFont fontMedium;
        public static SpriteFont fontLarge;

        // audio
        public static SoundEffect blip;
        public static Song nature;
    }

    class AssetsLoader
    {
        public void Load(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Assets.player = Content.Load<Texture2D>("player");
            Assets.playerDown = Content.Load<Texture2D>("Player/walkingDown");
            Assets.playerUp = Content.Load<Texture2D>("Player/walkingUp");
            Assets.playerLeft = Content.Load<Texture2D>("Player/walkingLeft");
            Assets.playerRight = Content.Load<Texture2D>("Player/walkingRight");

            Assets.button = Content.Load<Texture2D>("button");
            Assets.buttonPressed = Content.Load<Texture2D>("button_pressed");
            Assets.buttonHover = Content.Load<Texture2D>("button_hover");

            Assets.bullet = Content.Load<Texture2D>("bullet");

            Assets.background = Content.Load<Texture2D>("Middle");
            Assets.tunnel = Content.Load<Texture2D>("tunnel");

            Assets.fontSmall = Content.Load<SpriteFont>("fontSmall");
            Assets.fontMedium = Content.Load<SpriteFont>("fontMedium");
            Assets.fontLarge = Content.Load<SpriteFont>("fontLarge");

            Assets.blip = Content.Load<SoundEffect>("Sounds/blip");
            Assets.nature = Content.Load<Song>("Sounds/nature");
        }
    }
}
