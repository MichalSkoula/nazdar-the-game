﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Screens;
using Nazdar.Shared;
using System;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Enemy : BasePerson
    {
        public const int DefaultHealth = 100;
        public const int DefaultCaliber = 16;

        private List<Animation> animations = new List<Animation>();

        public Enemy(int x, int y, Direction direction, int health = DefaultHealth, int caliber = DefaultCaliber, int villageNumber = 1) : base()
        {
            this.Direction = direction;
            this.Health = health;
            this.Caliber = caliber;
            this.Speed = 110;
            this.Name = "Rogue Bolshevik";

            this.particleBlood = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>(this.Width / 2, this.Height / 2),
                Direction.Down,
                2,
                Assets.ParticleTextureRegions["Blood"]
            );

            // every village adds one potentional enemy type (sprite)
            int maxAnimIndex = 4; // number of sprites
            int villages = Assets.TilesetGroups.Count;
            int randomAnimIndex = Tools.GetRandom(villages > maxAnimIndex ? maxAnimIndex : villageNumber) + 2;
            // first village? only hungarians
            if (villageNumber == 1)
            {
                randomAnimIndex = 1;
            }

            // choose random enemy animation and set it for all directions
            this.animations.Add(new Animation(Assets.Images["Enemy" + randomAnimIndex + "Right"], 4, 10));
            this.animations.Add(new Animation(Assets.Images["Enemy" + randomAnimIndex + "Right"], 4, 10));
            this.animations.Add(new Animation(Assets.Images["Enemy" + randomAnimIndex + "Left"], 4, 10));
            this.animations.Add(new Animation(Assets.Images["Enemy" + randomAnimIndex + "Left"], 4, 10));

            this.Anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.Anim.FrameWidth, this.Anim.FrameHeight);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // particles
            this.particleBlood.Update(deltaTime, new Vector2(this.X, this.Y));

            if (this.Dead)
            {
                return;
            }

            // is enemy moving?
            bool isMoving = true;
            if (this.Direction == Direction.Right)
            {
                this.X += (int)(deltaTime * this.Speed);
            }
            else if (this.Direction == Direction.Left)
            {
                this.X -= (int)(deltaTime * this.Speed);
            }
            else
            {
                isMoving = false;
            }

            if (isMoving)
            {
                this.Anim.Loop = true;
                this.Anim = this.animations[(int)this.Direction];
            }
            else
            {
                this.Anim.Loop = false;
                this.Anim.ResetLoop();
            }

            this.Anim.Update(deltaTime);

            // out of game map
            if (this.X < 0 || this.X > VillageScreen.MapWidth)
            {
                this.ToDelete = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Anim.Draw(spriteBatch, this.Hitbox, this.FinalColor);
            this.DrawHealth(spriteBatch);
            // this.DrawCaliber(spriteBatch);

            // particles
            this.particleBlood.Draw(spriteBatch);
        }
    }
}
