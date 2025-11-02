using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Hospital : BaseBuilding
    {
        public const int Cost = 4;

        public const int MedicalKitCost = 4;
        private readonly int medicalKitLimit = 6;
        public List<MedicalKit> MedicalKits { get; private set; } = new List<MedicalKit>();
        public int MedicalKitsCount => MedicalKits.Count;

        public Hospital(int x, int y, Building.Status status, int? medicalKitsCount = 0, float ttb = 10) : base()
        {
            this.Sprite = Assets.Images["Hospital"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Hospital;

            for (int i = 0; i < medicalKitsCount; i++)
            {
                MedicalKits.Add(new MedicalKit());
            }
        }

        public bool AddMedicalKit()
        {
            if (MedicalKits.Count < this.medicalKitLimit)
            {
                MedicalKits.Add(new MedicalKit());
                return true;
            }

            return false;
        }

        public bool DropMedicalKit()
        {
            if (MedicalKits.Count > 0)
            {
                MedicalKits.RemoveAt(0);
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            base.Draw(spriteBatch);

            foreach (var weapon in this.MedicalKits)
            {
                weapon.Draw(spriteBatch);
            }
        }

        public new void Update(float deltaTime)
        {
            int i = 0;
            foreach (var medicalKit in this.MedicalKits)
            {
                medicalKit.SetPosition(this.X + 6 + (i * 12), this.Y + this.Height - medicalKit.Height);
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

        public override string Name => Nazdar.Shared.Translation.Translation.Get("building.hospital");
    }
}
