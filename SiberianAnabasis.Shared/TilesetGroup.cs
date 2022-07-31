using System;
using System.Collections.Generic;
using System.Text;
using TiledCS;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace SiberianAnabasis.Shared
{
    public class TilesetGroup
    {
        public Texture2D TilesetTexture;
        public TiledMap TilesetMap;
        public TiledTileset Tileset;

        public TilesetGroup(string tmx, string tsx, Texture2D texture)
        {
            this.TilesetMap = new TiledMap(tmx);
            this.Tileset = new TiledTileset(tsx);
            this.TilesetTexture = texture;
        }

        public IEnumerable<TiledObject> GetObjects(string layerName, string objectName)
        {
            var layer = this.TilesetMap.Layers.First(l => l.name == layerName);
            return layer.objects.Where(o => o.name == objectName);
        }

        public TiledObject GetObject(string layerName, string objectName)
        {
            var layer = this.TilesetMap.Layers.First(l => l.name == layerName);
            return layer.objects.First(o => o.name == objectName);
        }

        public void Draw(string layerName, SpriteBatch spriteBatch)
        {
            var layer = this.TilesetMap.Layers.First(l => l.name == layerName);
            for (var i = 0; i < layer.data.Length; i++)
            {
                int gid = layer.data[i];
                if (gid != 0)
                {
                    int tileFrame = gid - 1;

                    int column = tileFrame % this.Tileset.Columns;
                    int row = (int)Math.Floor((double)tileFrame / (double)this.Tileset.Columns);

                    float x = (i % this.TilesetMap.Width) * this.TilesetMap.TileWidth;
                    float y = (float)Math.Floor(i / (double)this.TilesetMap.Width) * this.TilesetMap.TileHeight;

                    Rectangle tilesetRec = new Rectangle(this.Tileset.TileWidth * column, this.Tileset.TileHeight * row, this.Tileset.TileWidth, this.Tileset.TileHeight);

                    spriteBatch.Draw(this.TilesetTexture, new Rectangle((int)x, (int)y, this.Tileset.TileWidth, this.Tileset.TileHeight), tilesetRec, Color.White);
                }
            }
        }
    }
}
