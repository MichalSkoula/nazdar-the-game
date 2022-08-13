using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static SiberianAnabasis.Enums;
using Microsoft.Xna.Framework;

namespace SiberianAnabasis.Objects
{
    public class Armory : BaseBuilding
    {
        public const int Cost = 2;
        public const int WeaponCost = 1;
        private int weaponLimit = 6;

        private List<Weapon> weapons = new List<Weapon>();
        public int WeaponsCount => weapons.Count;

        public Armory(int x, int y, Building.Status status, int? weaponCount = 0)
        {
            this.Sprite = Assets.Images["Armory"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuilt = 5;
            this.Type = Building.Type.Armory;

            for (int i = 0; i < weaponCount; i++)
            {
                this.weapons.Add(new Weapon());
            }
        }

        public bool AddWeapon()
        {
            if (this.weapons.Count < this.weaponLimit)
            {
                this.weapons.Add(new Weapon());
                return true;
            }

            return false;
        }

        public bool DropWeapon()
        {
            if (this.weapons.Count > 0)
            {
                this.weapons.RemoveAt(0);
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            
            foreach (var weapon in this.weapons)
            {
                weapon.Draw(spriteBatch);
            }
        }

        public new void Update(float deltaTime)
        {
            int i = 0;
            foreach (var weapon in this.weapons)
            {
                weapon.SetPosition(this.X + - 5 + i * 5, this.Y + this.Height - weapon.Height);
                i++;
            }
            base.Update(deltaTime);
        }
    }
}
