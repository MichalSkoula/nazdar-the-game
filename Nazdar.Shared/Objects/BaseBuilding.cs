using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public abstract class BaseBuilding : BaseObject
    {
        public Peasant WorkingPeasant { get; set; } = null;
        public float TimeToBuild;
        public Building.Type Type;
        public Building.Status Status = Building.Status.InProcess;

        public BaseBuilding()
        {
            this.Color = MyColor.UniversalColors[Tools.GetRandom(MyColor.UniversalColors.Length)];
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (this.TimeToBuild > 0 && this.WorkingPeasant != null)
            {
                this.TimeToBuild -= deltaTime;

                // particles
                if (Game1.GlobalTimer % 50 == 0)
                {
                    this.WorkingPeasant.particleConstruction.Run(50);
                }
            }

            // built?
            if (this.TimeToBuild < 0)
            {
                this.Status = Building.Status.Built;
                this.TimeToBuild = 0;
                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingBuilt", this.Type.ToString()), MessageType.Success);

                // if market, peasant becomes merchant => die
                if (this.Type == Building.Type.Market)
                {
                    this.WorkingPeasant.ToDelete = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Status != Building.Status.InProcess)
            {
                return;
            }

            // cover it with construction tape
            int textureWidth = Assets.Images["Construction"].Width;
            int textureHeight = Assets.Images["Construction"].Height;
            int howManyDraws = this.Hitbox.Width / textureWidth;
            int howManyDrawsOffset = (this.Hitbox.Width % textureWidth) / 2;

            for (int i = 0; i < howManyDraws; i++)
            {
                spriteBatch.Draw(
                    Assets.Images["Construction"],
                    new Rectangle(
                        this.Hitbox.X + howManyDrawsOffset + i * textureWidth,
                        this.Hitbox.Y + this.Hitbox.Height / 2 - textureHeight / 2,
                        textureWidth,
                        textureHeight),
                    Color.White);
            }
        }
    }
}
