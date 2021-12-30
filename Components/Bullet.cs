using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Screens;
using System.Collections.Generic;

namespace MyGame.Components
{
    public class Bullet : Component
    {
        private Enums.Direction _direction;
        private int _speed = 1200;

        public bool ToDelete { get; set; }

        public Bullet(int x, int y, Enums.Direction direction)
        {
            Sprite = Assets.bullet;
            Hitbox = new Rectangle(x, y - Sprite.Height / 2, Sprite.Width, Sprite.Height);
            _direction = direction;

            Assets.blip.Play();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Hitbox, Color.White);
        }

        public override void Update(float deltaTime)
        {
            // move it
            Rectangle newHitbox = Hitbox;
            if (_direction == Enums.Direction.Left)
            {
                newHitbox.X -= (int)(deltaTime * _speed);
            } 
            else if (_direction == Enums.Direction.Right)
            {
                newHitbox.X += (int)(deltaTime * _speed);
            }

            Hitbox = newHitbox;

            // out of game map 
            if (Hitbox.X < 0 || Hitbox.X > MapScreen.mapWidth)
            {
                ToDelete = true;
            }
        }
    }
}
