using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    static class Assets
    {
        public static Texture2D player;
        public static Texture2D button;
        public static Texture2D buttonPressed;
        public static Texture2D background;

        public static SpriteFont font;
    }

    class AssetsLoader
    {
        public void Load(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Assets.player = Content.Load<Texture2D>("player");
            Assets.button = Content.Load<Texture2D>("button");
            Assets.buttonPressed = Content.Load<Texture2D>("button_pressed");
            Assets.background = Content.Load<Texture2D>("Background");

            Assets.font = Content.Load<SpriteFont>("spaceFont");
        }
    }
}
