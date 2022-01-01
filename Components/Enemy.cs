using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MyGame.Screens;

namespace MyGame.Components
{
    class Enemy : Component
    {
        private int _speed = 600;

        private Animation _anim;

        private Enums.Direction _direction;

        public bool ToDelete { get; set; }

        private List<Bullet> _bullets = new List<Bullet>();

        private List<Animation> _animations = new List<Animation>()
        {
            new Animation(Assets.playerUp, 3, 10),
            new Animation(Assets.playerRight, 3, 10),
            new Animation(Assets.playerDown, 3, 10),
            new Animation(Assets.playerLeft, 3, 10),
        };

        public Enemy(int x, int y, Enums.Direction direction)
        {
            _anim = _animations[(int)Enums.Direction.Down];
            Hitbox = new Rectangle(x, y, _anim.FrameWidth, _anim.FrameHeight);
            _direction = direction;
        }

        /*
        public void Load(dynamic data)
        {
            Hitbox = new Rectangle((int)data.Hitbox.X, (int)data.Hitbox.Y, (int)data.Hitbox.Width, (int)data.Hitbox.Height);
        }
        */

        public override void Update(float deltaTime)
        {
            // is enemy moving?
            bool isMoving = true;
            Rectangle newHitbox = Hitbox;
            if (_direction == Enums.Direction.Right)
            {
                newHitbox.X += (int)(deltaTime * _speed);
            }
            else if (_direction == Enums.Direction.Left)
            {
                newHitbox.X -= (int)(deltaTime * _speed);
            }
            else
            {
                isMoving = false;
            }

            if (isMoving)
            {
                Hitbox = newHitbox;
                _anim.Loop = true;
                _anim = _animations[(int)_direction];
            }
            else
            {
                _anim.Loop = false;
                _anim.ResetLoop();
            }

            _anim.Update(deltaTime);

            // out of game map 
            if (Hitbox.X < 0 || Hitbox.X > MapScreen.mapWidth)
            {
                ToDelete = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _anim.Draw(spriteBatch, Hitbox, Color.Red);

            // bullets
            foreach (var bullet in _bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
