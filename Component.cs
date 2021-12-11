using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public abstract class Component
    {
        public Rectangle Hitbox { get; protected set; }
        public Texture2D Sprite { get; protected set; }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(float deltaTime);
    }
}
