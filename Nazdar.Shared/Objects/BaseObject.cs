﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nazdar.Shared;
using System.Collections.Generic;

namespace Nazdar.Objects
{
    public abstract class BaseObject
    {
        protected Animation Anim { get; set; }

        public int Caliber { get; set; }

        protected ParticleSource particleSmoke;

        public List<Bullet> Bullets { get; protected set; } = new List<Bullet>();

        public bool ToDelete { get; set; }

        // dying ... ttd - time to die
        private float ttd = 0f;
        private bool dead = false;
        public bool Dead
        {
            get
            {
                return this.dead;
            }
            set
            {
                this.dead = value;
                if (this.dead == true && this.ttd == 0)
                {
                    this.Color = this.ColorDead;
                    this.Alpha = 0.25f;
                    this.ttd = 5;
                }
            }
        }

        // base update method - should be called as base.Update() 
        public void Update(float deltaTime)
        {
            if (this.dead)
            {
                this.ttd -= deltaTime;
                if (this.ttd <= 0)
                {
                    this.ToDelete = true;
                }
            }
        }

        // draw colors and alpha
        public Color Color { private get; set; } = Color.White;
        protected Color ColorDead { private get; set; } = Color.Red;
        public float Alpha { protected get; set; } = 1;
        public Color FinalColor
        {
            get
            {
                return this.Color * this.Alpha;
            }
        }

        // direction, position, hitbox, dimensions
        public Enums.Direction Direction { get; set; }
        public Rectangle Hitbox { get; protected set; }
        public int X
        {
            get
            {
                return Hitbox.X;
            }
            set
            {
                var temp = this.Hitbox;
                temp.X = value;
                this.Hitbox = temp;
            }
        }

        public int Y
        {
            get
            {
                return Hitbox.Y;
            }
            protected set
            {
                var temp = this.Hitbox;
                temp.Y = value;
                this.Hitbox = temp;
            }
        }
        public int Width
        {
            get
            {
                return Hitbox.Width;
            }
        }
        public int Height
        {
            get
            {
                return Hitbox.Height;
            }
        }
        public Vector2 Position
        {
            get
            {
                return new Vector2(Hitbox.X, Hitbox.Y);
            }

            set
            {
                this.X = (int)value.X;
                this.Y = (int)value.Y;
            }
        }

        protected Texture2D Sprite { get; set; }

        public abstract void Draw(SpriteBatch spriteBatch);

        public void ChangeDirection()
        {
            if (this.Direction == Enums.Direction.Left)
            {
                this.Direction = Enums.Direction.Right;
            }
            else if (this.Direction == Enums.Direction.Right)
            {
                this.Direction = Enums.Direction.Left;
            }
        }

        private int health;
        public int Health
        {
            get
            {
                return health;
            }

            set
            {
                this.health = value;
                if (this.health > 100)
                {
                    this.health = 100;
                }
            }
        }

        public void DrawHealth(SpriteBatch spriteBatch, bool gold = false)
        {
            if (this.Dead)
            {
                return;
            }

            // border
            spriteBatch.DrawRectangle(new Rectangle(this.X, this.Y - 6, this.Width, 4), MyColor.Black * this.Alpha);

            // background 
            spriteBatch.DrawRectangle(new Rectangle(this.X + 1, this.Y - 5, this.Width - 2, 2), MyColor.White * this.Alpha);

            // inside
            int inside = (int)((this.Health / 100f) * (this.Width - 2));
            spriteBatch.DrawRectangle(
                new Rectangle(this.X + 1, this.Y - 5, inside, 2),
                (gold ? MyColor.Yellow : MyColor.Green) * this.Alpha
            );
        }
    }
}
