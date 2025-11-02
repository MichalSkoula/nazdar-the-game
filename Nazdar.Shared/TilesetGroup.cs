using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledCS;

namespace Nazdar.Shared
{
    public class TilesetGroup
    {
        public Texture2D TilesetTexture;
        public TiledMap TilesetMap;
        public TiledTileset Tileset;

        public TilesetGroup(Stream tmx, Stream tsx, Texture2D texture)
        {
            this.TilesetMap = new TiledMap(tmx);
            this.Tileset = new TiledTileset(tsx);
            this.TilesetTexture = texture;
        }

        public TilesetGroup(string tmx, string tsx, Texture2D texture)
        {
            this.TilesetMap = new TiledMap(tmx);
            this.Tileset = new TiledTileset(tsx);
            this.TilesetTexture = texture;
        }


        public IEnumerable<TiledObject> GetObjects(string layerName, string className)
        {
            var layer = this.TilesetMap.Layers.First(l => l.name == layerName);
            return layer.objects.Where(o => o.@class == className || o.type == className); // sometimes it parses @class, sometimes type (from xml)
        }

        public TiledObject? GetObject(string layerName, string className, string name)
        {
            var layer = this.TilesetMap.Layers.First(l => l.name == layerName);
            var objects = layer.objects.Where(o => (o.@class == className || o.type == className) && o.name == name); // sometimes it parses @class, sometimes type (from xml)
            return objects.Any() ? objects.First() : null;
        }

        public int GetTilesetMapWidth()
        {
            return this.TilesetMap.Width * this.TilesetMap.TileWidth;
        }

        public void Draw(string layerName, SpriteBatch spriteBatch, int offset = 0)
        {
            var layer = this.TilesetMap.Layers.First(l => l.name == layerName);
            for (var i = 0; i < layer.data.Length; i++)
            {
                int gid = layer.data[i];
                if (gid != 0)
                {
                    int tileFrame = gid - 1;

                    int column = tileFrame % this.Tileset.Columns;
                    int row = (int)Math.Floor(tileFrame / (double)this.Tileset.Columns);

                    float x = (i % this.TilesetMap.Width * this.TilesetMap.TileWidth) + offset;
                    float y = (float)Math.Floor(i / (double)this.TilesetMap.Width) * this.TilesetMap.TileHeight;

                    Rectangle tilesetRec = new Rectangle(this.Tileset.TileWidth * column, this.Tileset.TileHeight * row, this.Tileset.TileWidth, this.Tileset.TileHeight);

                    spriteBatch.Draw(this.TilesetTexture, new Rectangle((int)x, (int)y, this.Tileset.TileWidth, this.Tileset.TileHeight), tilesetRec, Color.White);
                }
            }
        }
    }
}
