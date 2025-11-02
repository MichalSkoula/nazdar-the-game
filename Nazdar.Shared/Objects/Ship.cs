using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nazdar.Objects
{
    public class Ship : BaseObject
    {
        public const int Cost = 48;
        public string Name => Nazdar.Shared.Translation.Translation.Get("building.ship");
        public Enums.Building.Status Status { get; set; } = Enums.Building.Status.InProcess;

        public Ship(int x, int y) : base()
        {
            this.Sprite = Assets.Images["Ship"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox,
            };
        }
    }
}
