using Microsoft.Xna.Framework.Graphics;

namespace Nazdar.Weather
{
    interface IDrop
    {
        int Y { get; }

        bool ToDelete { get; set; }

        void Fall();
        void Draw(SpriteBatch spriteBatch);
    }
}
