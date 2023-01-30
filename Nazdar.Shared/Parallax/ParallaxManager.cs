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

        public void Init(int mapWidth, int village)
        {
            this.layers.Clear();

            switch (village)
            {
                case 1:
                    this.AddLayer(new Layer(Assets.Images["MountainGrass"], 1, 0.1f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.2f, 0, mapWidth, 1));
                    break;
                case 2:
                    this.AddLayer(new Layer(Assets.Images["MountainSnow"], 2, 0.1f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.2f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 5, 0.2f, 1f, mapWidth, 0.3f));
                    break;
                case 3:
                    this.AddLayer(new Layer(Assets.Images["MountainSnow"], 2, 0.1f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.2f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_1"], 5, 0.2f, 1f, mapWidth, 0.3f));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_3"], 5, 0.25f, 1f, mapWidth, 0.3f));
                    break;
                case 4:
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.2f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 5, 0.2f, 1f, mapWidth, 0.3f));
                    break;
                case 5:
                    this.AddLayer(new Layer(Assets.Images["MountainGrass"], 1, 0.1f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.2f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_3"], 5, 0.2f, 1f, mapWidth, 0.3f));
                    break;
                case 6:
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.2f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_1"], 5, 0.2f, 1f, mapWidth, 0.3f));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 5, 0.25f, 1f, mapWidth, 0.3f));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_3"], 5, 0.3f, 1f, mapWidth, 0.3f));
                    break;
                case 7:
                    this.AddLayer(new Layer(Assets.Images["MountainSnow"], 2, 0.1f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.2f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_1"], 5, 0.2f, 1f, mapWidth, 0.3f));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 5, 0.25f, 1f, mapWidth, 0.3f));
                    break;
                case 8:
                    this.AddLayer(new Layer(Assets.Images["MountainGreen"], 2, 0.1f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.2f, 0, mapWidth, 1));
                    break;
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
