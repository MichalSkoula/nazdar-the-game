using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Nazdar.Shared.Parallax
{
    internal class ParallaxManager
    {
        private readonly List<Layer> layers = new List<Layer>();

        public void Init(int mapWidth, int village)
        {
            this.layers.Clear();

            switch (village)
            {
                case 0: // grass
                    this.AddLayer(new Layer(Assets.Images["MountainGrass"], 1, 0.01f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    break;
                case 1: // grass
                    this.AddLayer(new Layer(Assets.Images["MountainGrass"], 1, 0.01f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    break;
                case 2: // snow
                    this.AddLayer(new Layer(Assets.Images["MountainSnow"], 2, 0.01f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 5, 0.04f, 1f, mapWidth, 0.3f));
                    break;
                case 3: // grass
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_1"], 5, 0.04f, 1f, mapWidth, 0.3f));
                    break;
                case 4: // grass
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 5, 0.04f, 1f, mapWidth, 0.3f));
                    break;
                case 5: // snow
                    this.AddLayer(new Layer(Assets.Images["MountainSnow"], 2, 0.01f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_1"], 5, 0.04f, 1f, mapWidth, 0.3f));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_3"], 5, 0.05f, 1f, mapWidth, 0.3f));
                    break;
                case 6: // grass
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_1"], 5, 0.04f, 1f, mapWidth, 0.3f));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 5, 0.05f, 1f, mapWidth, 0.3f));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_3"], 5, 0.06f, 1f, mapWidth, 0.3f));
                    break;
                case 7: // snow
                    this.AddLayer(new Layer(Assets.Images["MountainSnow"], 2, 0.01f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_1"], 5, 0.04f, 1f, mapWidth, 0.3f));
                    this.AddLayer(new Layer(Assets.Images["Clouds_mg_2"], 5, 0.05f, 1f, mapWidth, 0.3f));
                    break;
                case 8: // grass
                    this.AddLayer(new Layer(Assets.Images["MountainGrass"], 2, 0.03f, 0, mapWidth, 2));
                    this.AddLayer(new Layer(Assets.Images["Trees"], 2, 0.03f, 0, mapWidth, 1));
                    break;
            }
        }

        private void AddLayer(Layer layer)
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
