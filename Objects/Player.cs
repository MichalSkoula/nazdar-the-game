using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Screens;
using System.Collections.Generic;

namespace MyGame.Objects
{
    public class Player : Component
    {
        private int _speed = 1000;

        private Animation _anim;

        private List<Animation> _animations = new List<Animation>()
        {
            new Animation(Assets.playerUp, 3, 10),
            new Animation(Assets.playerRight, 3, 10),
            new Animation(Assets.playerDown, 3, 10),
            new Animation(Assets.playerLeft, 3, 10),
        };

        public Player(Texture2D sprite, int x, int y)
        {
            _anim = _animations[(int)Enums.Direction.Down];
            Hitbox = new Rectangle(x, y, _anim.FrameWidth, _anim.FrameHeight);
        }

        public void Load(dynamic data)
        {
            Hitbox = new Rectangle((int)data.Hitbox.X, (int)data.Hitbox.Y, (int)data.Hitbox.Width, (int)data.Hitbox.Height);
        }

        public override void Update(float deltaTime)
        {
            // movement
            bool isMoving = true;
            Rectangle newHitbox = Hitbox;

            if (Controls.Keyboard.IsPressed(Keys.Right) && Hitbox.X < MapScreen.mapWidth - Hitbox.Width)
            {
                newHitbox.X += (int)(deltaTime * _speed);
                _anim = _animations[(int)Enums.Direction.Right];
            }
            else if (Controls.Keyboard.IsPressed(Keys.Left) && Hitbox.X > 0)
            {
                newHitbox.X -= (int)(deltaTime * _speed);
                _anim = _animations[(int)Enums.Direction.Left];
            }
            else
            {
                isMoving = false;
            }

            if (isMoving)
            {
                Hitbox = newHitbox;
                _anim.Loop = true;
            }
            else
            {
                _anim.Loop = false;
                _anim.ResetLoop();
            }

            // shoot 
            if (Controls.Keyboard.HasBeenPressed(Keys.Space))
            {

            }

            _anim.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _anim.Draw(spriteBatch, Hitbox);
        }
    }
}
