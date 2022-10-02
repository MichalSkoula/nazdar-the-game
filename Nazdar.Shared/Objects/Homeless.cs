using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Screens;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Homeless : BasePerson
    {
        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.Images["HomelessRight"], 4, 10),
            new Animation(Assets.Images["HomelessRight"], 4, 10),
            new Animation(Assets.Images["HomelessLeft"], 4, 10),
            new Animation(Assets.Images["HomelessLeft"], 4, 10),
        };

        public const int Cost = 1;
        public const string Name = "Homeless";

        public Homeless(int x, int y, Direction direction)
        {
            this.Anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.Anim.FrameWidth, this.Anim.FrameHeight);
            this.Direction = direction;
            this.Health = 100;
            this.Alpha = 0.5f;
            this.Speed = 61;
            this.Color = UniversalColors[Tools.GetRandom(UniversalColors.Length)];
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // is he moving?
            bool isMoving = false;
            if (Tools.GetRandom(16) < 2)
            {
                if (this.Direction == Direction.Right)
                {
                    this.X += (int)(deltaTime * this.Speed);
                    isMoving = true;
                }
                else if (this.Direction == Direction.Left)
                {
                    this.X -= (int)(deltaTime * this.Speed);
                    isMoving = true;
                }
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

            // out of game map - change direction
            if (this.X < 0)
            {
                this.X = 0;
                this.ChangeDirection();
            }
            else if (this.X > VillageScreen.MapWidth)
            {
                this.X = VillageScreen.MapWidth;
                this.ChangeDirection();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Anim.Draw(spriteBatch, this.Hitbox, this.FinalColor);
            //this.DrawHealth(spriteBatch);
        }
    }
}
