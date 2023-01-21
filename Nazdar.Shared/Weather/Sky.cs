using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using Nazdar.Shared;
using Nazdar.Screens;

namespace Nazdar.Weather
{
    public class Sky
    {
        private List<Raindrop> drops = new List<Raindrop>();
        public double Ttl;
        public bool Raining { get; set; } = false;

        public void Rain(int ttl)
        {
            this.Ttl = ttl;
            this.Raining = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var drop in this.drops)
            {
                drop.Draw(spriteBatch);
            }
        }

        public void Update(float deltaTime)
        {
            if (this.Raining && Tools.GetRandom(10) == 0)
            {
                this.drops.Add(new Raindrop(Tools.GetRandom(VillageScreen.MapWidth), -10));
            }

            foreach (var drop in this.drops)
            {
                drop.Fall();

                if (drop.Y > Enums.Screen.Height)
                {
                    drop.ToDelete = true;
                }
            }

            this.drops.RemoveAll(d => d.ToDelete);

            this.Ttl -= deltaTime;
            if (this.Ttl < 0)
            {
                this.Raining = false;
            }
        }
    }
}
