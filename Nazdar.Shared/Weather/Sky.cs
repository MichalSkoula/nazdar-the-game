using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using Nazdar.Shared;
using Nazdar.Screens;
using static Nazdar.Enums;

namespace Nazdar.Weather
{
    public class Sky
    {
        private List<IDrop> drops = new List<IDrop>();
        public double Ttl = 0;
        public DropType Type = DropType.Raining;
        public int DropCount
        {
            get
            {
                return drops.Count;
            }
        }
        public bool Active
        {
            get
            {
                return Ttl > 0;
            }
        }

        public void Start(int ttl, DropType? type = null, int startCount = 0)
        {
            this.Ttl = ttl;
            this.Type = (DropType)(type == null ? (DropType)Tools.GetRandom(2) : type);

            if (startCount > 0)
            {
                for (int i = 0; i < startCount; i++)
                {
                    this.AddDrop(true);
                }
            }
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
            if (this.Active)
            {
                this.AddDrop();
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
        }

        private void AddDrop(bool randomY = false)
        {
            int x = Tools.GetRandom(VillageScreen.MapWidth + Enums.Screen.Width) - Enums.Screen.Width / 2;
            int y = -10;
            if (randomY)
            {
                y = Tools.GetRandom(Enums.Screen.Height);
            }

            if (this.Type == DropType.Raining)
            {
                this.drops.Add(new Raindrop(x, y));
            }
            else
            {
                this.drops.Add(new Snowflake(x, y));
            }
        }
    }
}
