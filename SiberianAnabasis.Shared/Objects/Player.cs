using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SiberianAnabasis.Screens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SiberianAnabasis.Enums;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

namespace SiberianAnabasis.Components
{
    public class Player : Component
    {
        private int speed = 120;

        private Animation anim;

        public List<Bullet> Bullets
        {
            get;
            private set;
        }

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.PlayerRight, 4, 10),
            new Animation(Assets.PlayerRight, 4, 10),
            new Animation(Assets.PlayerLeft, 4, 10),
            new Animation(Assets.PlayerLeft, 4, 10),
        };

        private ParticleEffect particleEffect;
        public int Money { get; set; }
        public int Days { get; set; }

        public Player(int x, int y)
        {
            this.anim = this.animations[(int)Direction.Right];
            this.Direction = Direction.Right;
            this.Bullets = new List<Bullet>();
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Health = 100;
            this.Caliber = 30;
            this.Days = 0;

            this.particleEffect = new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(this.Hitbox.X, this.Hitbox.Y),
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(Assets.ParticleTextureRegion, 500, TimeSpan.FromSeconds(2.5), Profile.Point())
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 50f),
                            Quantity = 3,
                            Rotation = new Range<float>(-1f, 1f),
                            Scale = new Range<float>(3.0f, 4.0f)
                        },
                        Modifiers =
                        {
                            /*
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = new HslColor(0.33f, 0.5f, 0.5f),
                                        EndValue = new HslColor(0.5f, 0.9f, 1.0f)
                                    }
                                }
                            },
                            */
                            new RotationModifier {RotationRate = -2.1f},
                            //new RectangleContainerModifier {Width = 800, Height = 480},
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
                        }
                    }
                }
            };
        }

        public void Load(dynamic saveData)
        {
            // load player
            this.Hitbox = new Rectangle((int)saveData.Hitbox.X, this.Hitbox.Y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Direction = (Direction)saveData.Direction;
            this.Health = (int)saveData.Health;
            this.Days = (int)saveData.Days;
            this.Money = (int)saveData.Money;

            // load bullets
            if (saveData.ContainsKey("Bullets"))
            {
                foreach (var bullet in saveData.GetValue("Bullets"))
                {
                    this.Bullets.Add(new Bullet((int)bullet.Hitbox.X, (int)bullet.Hitbox.Y, (Direction)bullet.Direction, (int)bullet.Caliber));
                }
            }
        }

        public override void Update(float deltaTime)
        {
            // moving?
            bool isMoving = false;
            Rectangle newHitbox = this.Hitbox;
            if (Controls.Keyboard.IsPressed(Keys.Right) && this.Hitbox.X < VillageScreen.MapWidth - this.Hitbox.Width)
            {
                newHitbox.X += (int)(deltaTime * this.speed);
                this.Direction = Direction.Right;
                isMoving = true;
            }
            else if (Controls.Keyboard.IsPressed(Keys.Left) && this.Hitbox.X > 0)
            {
                newHitbox.X -= (int)(deltaTime * this.speed);
                this.Direction = Direction.Left;
                isMoving = true;
            }

            if (isMoving)
            {
                this.Hitbox = newHitbox;
                this.anim.Loop = true;
                this.anim = this.animations[(int)this.Direction];
            }
            else
            {
                this.anim.Loop = false;
                this.anim.ResetLoop();
            }

            this.anim.Update(deltaTime);

            // jump?
            if (Controls.Keyboard.IsPressed(Keys.Up) && this.Hitbox.Y == Enums.Offset.Floor)
            {
                this.Jump();
                isMoving = true;
            }

            // bullets
            if (Controls.Keyboard.HasBeenPressed(Keys.Space))
            {
                this.Bullets.Add(new Bullet(this.Hitbox.X, this.Hitbox.Y + (this.Hitbox.Height / 2), this.Direction, this.Caliber));
            }

            foreach (var bullet in this.Bullets)
            {
                bullet.Update(deltaTime);
            }

            this.Bullets.RemoveAll(p => p.ToDelete);

            // particles
            this.particleEffect.Position = new Vector2(this.Hitbox.X, this.Hitbox.Y);
            this.particleEffect.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox);

            // bullets
            foreach (var bullet in this.Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // particles
            spriteBatch.Draw(particleEffect);
        }

        private async void Jump()
        {
            int h0 = Enums.Offset.Floor;
            int v0 = 25;
            int g = 10;
            float timeDivider = 5;

            for (int i = 0; i < 200; i++) 
            {
                Rectangle newHitbox = this.Hitbox;

                float t = i / timeDivider;
                int newY = h0 - (int)(v0 * t - 0.5 * g * Math.Pow(t, 2)); // https://cs.wikipedia.org/wiki/Vrh_svisl%C3%BD
                // System.Diagnostics.Debug.WriteLine(newY);
                if (newY > h0)
                {
                    newHitbox.Y = h0;
                    this.Hitbox = newHitbox;

                    break;
                }

                newHitbox.Y = newY;
                this.Hitbox = newHitbox;

                await Task.Delay(20);
            }
        }
    }
}
