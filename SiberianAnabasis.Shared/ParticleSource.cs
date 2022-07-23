using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

namespace SiberianAnabasis.Shared
{
    public class ParticleSource
    {
        private ParticleEffect particleEffect;

        private Tuple<int, int> offset;

        public ParticleSource(Vector2 position, Tuple<int, int> offset, Enums.Direction direction, Color startColor, Color endColor)
        {
            this.offset = offset;
            this.particleEffect = new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(position.X, position.Y),
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
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = HslColor.FromRgb(startColor),
                                        EndValue = HslColor.FromRgb(endColor),
                                    }
                                }
                            },
                            new RotationModifier {RotationRate = -2.1f},
                            //new RectangleContainerModifier {Width = 800, Height = 480},
                            new LinearGravityModifier {Direction = this.convertDirection(direction), Strength = 30f},
                        }
                    }
                }
            };

            // default - stopped
            this.Stop();
        }

        public void Start()
        {
            foreach (var emi in this.particleEffect.Emitters)
            {
                emi.AutoTrigger = true;
            }
        }

        public void Stop()
        {
            foreach (var emi in this.particleEffect.Emitters)
            {
                emi.AutoTrigger = false;
            }
        }

        public void Update(float deltaTime, Vector2? position = null)
        {
            if (position.HasValue)
            {
                this.particleEffect.Position = new Vector2(position.Value.X + this.offset.Item1, position.Value.Y + this.offset.Item2);
            }

            this.particleEffect.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.particleEffect);
        }

        private Vector2 convertDirection(Enums.Direction direction)
        {
            switch (direction)
            {
                case Enums.Direction.Up:
                    return -Vector2.UnitY;
                case Enums.Direction.Down:
                    return Vector2.UnitY;
                case Enums.Direction.Left: 
                    return -Vector2.UnitX;
                default:
                case Enums.Direction.Right: 
                    return Vector2.UnitX;
            }
        }
    }
}
