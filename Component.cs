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

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(Sprite, Hitbox, Color.White);
        }

        public abstract void Update(float deltaTime);
    }
}
