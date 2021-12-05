using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ScreenTest
{
    static class Assets
    {
        public static Texture2D player;

        public static SpriteFont font;
    }

    class AssetsLoader
    {
        public void Load(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Assets.player = Content.Load<Texture2D>("player");

            Assets.font = Content.Load<SpriteFont>("spaceFont");
        }
    }
}
