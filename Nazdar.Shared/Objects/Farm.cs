using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Farm : BaseBuilding
    {
        public const int Cost = 4;
        public const int UpgradeCost = 8;
        public const int DefaultLevel = 1;
        public const int MaxLevel = 2;
        public const int DefaultFarmerCapacity = 4;
        public const int UpgradedFarmerCapacity = 6;
        public override string Name => Nazdar.Shared.Translation.Translation.Get("building.farm");

        public List<Tool> Tools { get; private set; } = new List<Tool>();
        private readonly int toolLimit = 6;
        public const int ToolCost = 1;
        public int ToolsCount => Tools.Count;
        public int Level { get; private set; }
        public int FarmerCapacity => this.Level == MaxLevel ? UpgradedFarmerCapacity : DefaultFarmerCapacity;

        // The left two thirds contain the field and tool crates. The right third is reserved for the upgrade house.
        private int FieldWidth => (this.Width * 2) / 3;
        public Rectangle ToolZone => new Rectangle(this.X, this.Y, this.FieldWidth, this.Height);
        public Rectangle UpgradeZone => new Rectangle(this.X + this.FieldWidth, this.Y, this.Width - this.FieldWidth, this.Height);

        public Farm(int x, int y, Building.Status status, int? toolsCount = 0, float ttb = 5, int level = DefaultLevel) : base()
        {
            this.SetLevel(level);
            this.Hitbox = new Rectangle(x, y, this.Anim.FrameWidth, this.Anim.FrameHeight);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Farm;

            for (int i = 0; i < toolsCount; i++)
            {
                this.Tools.Add(new Tool());
            }
        }

        public bool Upgrade()
        {
            if (this.Level >= MaxLevel)
            {
                return false;
            }

            this.SetLevel(this.Level + 1);
            return true;
        }

        private void SetLevel(int level)
        {
            this.Level = level >= MaxLevel ? MaxLevel : DefaultLevel;
            this.Sprite = Assets.Images[this.Level == MaxLevel ? "FarmUpgraded" : "Farm"];
            this.Anim = new Animation(this.Sprite, 3, 3);
        }

        public bool AddTool()
        {
            if (this.Tools.Count < this.toolLimit)
            {
                this.Tools.Add(new Tool());
                return true;
            }

            return false;
        }

        public bool DropTool()
        {
            if (this.Tools.Count > 0)
            {
                this.Tools.RemoveAt(0);
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Anim.Draw(spriteBatch, this.Hitbox, this.FinalColor);
            base.Draw(spriteBatch);

            foreach (var tool in this.Tools)
            {
                tool.Draw(spriteBatch);
            }
        }

        public new void Update(float deltaTime)
        {
            int i = 0;
            foreach (var tool in this.Tools)
            {
                tool.SetPosition(this.X + 6 + (i * 12), this.Y + this.Height - tool.Height);
                i++;
            }

            // anim change frame
            this.Anim.Loop = true;
            this.Anim.Update(deltaTime);

            base.Update(deltaTime);
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox,
                this.Status,
                this.ToolsCount,
                this.TimeToBuild,
                this.Level
            };
        }
    }
}
