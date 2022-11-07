using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Farm : BaseBuilding
    {
        public const int Cost = 4;
        public const string Name = "Farm";

        private List<Tool> tools = new List<Tool>();
        private int toolLimit = 6;
        public const int ToolCost = 1;
        public int ToolsCount => tools.Count;

        public Farm(int x, int y, Building.Status status, int? toolsCount = 0)
        {
            this.Sprite = Assets.Images["Farm"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuilt = 5;
            this.Type = Building.Type.Farm;

            for (int i = 0; i < toolsCount; i++)
            {
                this.tools.Add(new Tool());
            }
        }

        public bool AddTool()
        {
            if (this.tools.Count < this.toolLimit)
            {
                this.tools.Add(new Tool());
                return true;
            }

            return false;
        }

        public bool DropTool()
        {
            if (this.tools.Count > 0)
            {
                this.tools.RemoveAt(0);
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);

            foreach (var tool in this.tools)
            {
                tool.Draw(spriteBatch);
            }
        }

        public new void Update(float deltaTime)
        {
            int i = 0;
            foreach (var tool in this.tools)
            {
                tool.SetPosition(this.X + -5 + i * 5, this.Y + this.Height - tool.Height);
                i++;
            }
            base.Update(deltaTime);
        }
    }
}
