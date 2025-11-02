using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Shared.Translation;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Armory : BaseBuilding
    {
        public const int Cost = 4;

        public const int WeaponCost = 2;
        private readonly int weaponLimit = 6;
        public List<Weapon> Weapons { get; private set; } = new List<Weapon>();
        public int WeaponsCount => Weapons.Count;

        public Armory(int x, int y, Building.Status status, int? weaponCount = 0, float ttb = 5) : base()
        {
            this.Sprite = Assets.Images["Armory"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Armory;

            for (int i = 0; i < weaponCount; i++)
            {
                this.Weapons.Add(new Weapon());
            }
        }

        public bool AddWeapon()
        {
            if (this.Weapons.Count < this.weaponLimit)
            {
                this.Weapons.Add(new Weapon());
                return true;
            }

            return false;
        }

        public bool DropWeapon()
        {
            if (this.Weapons.Count > 0)
            {
                this.Weapons.RemoveAt(0);
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            base.Draw(spriteBatch);

            foreach (var weapon in this.Weapons)
            {
                weapon.Draw(spriteBatch);
            }
        }

        public new void Update(float deltaTime)
        {
            int i = 0;
            foreach (var weapon in this.Weapons)
            {
                weapon.SetPosition(this.X + 6 + (i * 12), this.Y + this.Height - weapon.Height);
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
                this.WeaponsCount,
                this.TimeToBuild
            };
        }

        public override string Name => Translation.Get("building.armory");
    }
}
