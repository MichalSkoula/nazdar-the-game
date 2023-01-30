using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using static Nazdar.Enums;

namespace Nazdar.Shared.Parallax
{
    internal class ParallaxManager
    {
        private List<Layer> layers = new List<Layer>();

        public void Init(int mapWidth, WeatherType type)
        {
            this.layers.Clear();

            // static -------------------------------------------------------------------------------
            this.AddLayer(new Layer(Assets.Images["Mountain"], 1, 0.1f, 0, mapWidth, 1));
            if (type == WeatherType.Grass)
            {
                this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.15f, 0, mapWidth, 1));
            }

            // moving -------------------------------------------------------------------------------
            // back clouds
            if (Tools.GetRandom(2) == 0) 
            { 
                this.AddLayer(new Layer(Assets.Images["Clouds_bg"], 0, 0.05f, 1, mapWidth, 0.7f));
            }
            // fog
            if (Tools.GetRandom(2) == 0)
            {
                this.AddLayer(new Layer(Assets.Images["Clouds_mg_1"], 5, 0.2f, 1.6f, mapWidth, 0.3f));
            }
            // another fog? - only in winter
            if (type == WeatherType.Snow && Tools.GetRandom(2) == 0)
            {
                this.AddLayer(new Layer(Assets.Images["Clouds_mg_3"], 3, 0.2f, 1.2f, mapWidth, 0.5f));
            }
            if (type == WeatherType.Snow && Tools.GetRandom(2) == 0)
            {
                this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 4, 0.2f, 1.4f, mapWidth, 0.4f));
            }
        }

        public void AddLayer(Layer layer)
        {
            layers.Add(layer);
        }

        public void Update(int playerPosition, Color parallaxColor)
        {
            foreach (Layer layer in layers)
            {
                layer.Update(playerPosition, parallaxColor);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Layer layer in layers.OrderBy(l => l.Depth))
            {
                layer.Draw(spriteBatch);
            }
        }
    }
}
