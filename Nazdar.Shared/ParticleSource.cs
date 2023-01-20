using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nazdar.Shared
{
    public class ParticleSource
    {
        private ParticleEffect particleEffect;

        private Tuple<int, int> offset;

        // ttl - time to live of one particle
        public ParticleSource(Vector2 position, Tuple<int, int> offset, Enums.Direction direction, float ttl, TextureRegion2D textureRegion, Color? startColor = null, Color? endColor = null, float gravity = 30)
        {
            this.offset = offset;
            this.particleEffect = new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(position.X, position.Y),
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(ttl), Profile.Point())
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 50f),
                            Quantity = 3,
                            Rotation = new Range<float>(-1f, 1f),
                            Scale = new Range<float>(3.0f, 4.0f) // size of a particle
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = HslColor.FromRgb(startColor.HasValue ? (Color)startColor : Color.White),
                                        EndValue = HslColor.FromRgb(endColor.HasValue ? (Color)endColor : Color.White),
                                    }
                                }
                            },
                            new RotationModifier {RotationRate = -2.1f},
                            //new RectangleContainerModifier {Width = 800, Height = 480},
                            new LinearGravityModifier {Direction = this.convertDirection(direction), Strength = gravity},
                        }
                    }
                }
            };

            // default - stopped
            this.Stop();
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

        // duration - how many ms will it be emitting
        public async void Run(int duration)
        {
            this.Start();
            await Task.Delay(duration);
            this.Stop();
        }

        private void Start()
        {
            foreach (var emi in this.particleEffect.Emitters)
            {
                emi.AutoTrigger = true;
            }
        }

        private void Stop()
        {
            foreach (var emi in this.particleEffect.Emitters)
            {
                emi.AutoTrigger = false;
            }
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
