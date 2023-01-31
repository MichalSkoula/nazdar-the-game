using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Hospital : BaseBuilding
    {
        public const int Cost = 4;
        public const string Name = "Hospital";

        public const int MedicalKitCost = 4;
        private int medicalKitLimit = 6;
        private List<MedicalKit> medicalKits = new List<MedicalKit>();
        public int MedicalKitsCount => medicalKits.Count;

        public Hospital(int x, int y, Building.Status status, int? medicalKitsCount = 0, float ttb = 10) : base()
        {
            this.Sprite = Assets.Images["Hospital"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Hospital;

            for (int i = 0; i < medicalKitsCount; i++)
            {
                medicalKits.Add(new MedicalKit());
            }
        }

        public bool AddMedicalKit()
        {
            if (medicalKits.Count < this.medicalKitLimit)
            {
                medicalKits.Add(new MedicalKit());
                return true;
            }

            return false;
        }

        public bool DropMedicalKit()
        {
            if (medicalKits.Count > 0)
            {
                medicalKits.RemoveAt(0);
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            base.Draw(spriteBatch);

            foreach (var weapon in this.medicalKits)
            {
                weapon.Draw(spriteBatch);
            }
        }

        public new void Update(float deltaTime)
        {
            int i = 0;
            foreach (var medicalKit in this.medicalKits)
            {
                medicalKit.SetPosition(this.X + 6 + i * 12, this.Y + this.Height - medicalKit.Height);
                i++;
            }
            base.Update(deltaTime);
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox,
                this.Status,
                this.MedicalKitsCount,
                this.TimeToBuild
            };
        }
    }
}
