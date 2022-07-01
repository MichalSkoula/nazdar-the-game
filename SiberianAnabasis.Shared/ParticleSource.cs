using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

namespace SiberianAnabasis.Shared
{
    public class ParticleSource
    {
        private ParticleEffect particleEffect;

        public ParticleSource(Vector2 position)
        {
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
                                        StartValue = new HslColor(0.33f, 0.5f, 0.5f),
                                        EndValue = new HslColor(0.5f, 0.9f, 1.0f)
                                    }
                                }
                            },
                            new RotationModifier {RotationRate = -2.1f},
                            //new RectangleContainerModifier {Width = 800, Height = 480},
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
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
            // maybe change position?
            if (position.HasValue)
            {
                this.particleEffect.Position = (Vector2)position;
            }

            this.particleEffect.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.particleEffect);
        }
    }
}
