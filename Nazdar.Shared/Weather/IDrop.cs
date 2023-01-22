using Nazdar.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
