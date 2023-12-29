using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Objects;
using Nazdar.Shared;
using Nazdar.Weather;
using System.Linq;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public partial class VillageScreen : GameScreen
    {
        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = this.camera.Transform;
            this.Game.DrawStart();

            // day or night sky - color transition
            Color currentColor = Sky.GetSkyColor(this.dayPhase, this.dayPhaseTimer);
            this.GraphicsDevice.Clear(currentColor);

            // parallax
            this.parallaxManager.Draw(this.Game.SpriteBatch);

            // background - railways
            Assets.TilesetGroups["village" + this.Game.Village].Draw("background", this.Game.SpriteBatch);
            Assets.TilesetGroups["village" + this.Game.Village].Draw("ground", this.Game.SpriteBatch);
            // background - edges
            Assets.TilesetEdges["left" + this.Game.Village].Draw("background", this.Game.SpriteBatch, -Offset.MapTilesOffset);
            Assets.TilesetEdges["left" + this.Game.Village].Draw("ground", this.Game.SpriteBatch, -Offset.MapTilesOffset);
            Assets.TilesetEdges["right" + this.Game.Village].Draw("background", this.Game.SpriteBatch, MapWidth);
            Assets.TilesetEdges["right" + this.Game.Village].Draw("ground", this.Game.SpriteBatch, MapWidth);

            // stats --------------------------------------------------------------------------
            int leftOffset = Offset.StatusBarX - (int)this.camera.Transform.Translation.X;
            int rightOffset = Game1.CurrentPlatform == Enums.Platform.Android ? 370 : 490;

            // healthbar
            this.Game.SpriteBatch.DrawRectangle(
                new Rectangle(leftOffset, Offset.StatusBarY + 1, 102, 10),
                MyColor.Black
            );
            this.Game.SpriteBatch.DrawRectangle(
                new Rectangle(leftOffset + 1, Offset.StatusBarY + 2, 100, 8),
                MyColor.White,
                4
            );
            this.Game.SpriteBatch.FillRectangle(
                new Rectangle(leftOffset + 1, Offset.StatusBarY + 2, this.player.Health, 8),
                MyColor.Green
            );


            // attack power
            this.Game.SpriteBatch.Draw(
                Assets.Images["Sword"],
                new Vector2(leftOffset + 120, Offset.StatusBarY),
                new Rectangle(0, 0, Assets.Images["Sword"].Width, Assets.Images["Sword"].Height),
                Color.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                this.player.Caliber.ToString(),
                new Vector2(leftOffset + 140, Offset.StatusBarY + 3),
                MyColor.White);

            // left stats
            this.Game.SpriteBatch.Draw(
                Assets.Images["CoinStatic"],
                new Vector2(leftOffset, Offset.StatusBarY + 20),
                Color.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                " x " + (this.player.Money).ToString(),
                new Vector2(leftOffset + 15, Offset.StatusBarY + 22),
                MyColor.White);

            this.Game.SpriteBatch.Draw(
                Assets.Images["BulletStatic"],
                new Vector2(leftOffset, Offset.StatusBarY + 45),
                Color.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                " x " + (this.player.Cartridges).ToString(),
                new Vector2(leftOffset + 15, Offset.StatusBarY + 46),
                MyColor.White);

            // top left stats
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                (this.Game.Village > 0 ? (this.Game.Village + "/") : "") + (Villages)this.Game.Village + ", day " + this.player.Days.ToString() + ".",
                new Vector2(leftOffset, 10),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
               Assets.Fonts["Small"],
               Tools.DayPhaseTimerToHours(this.dayPhase, this.dayPhaseTimer),
               new Vector2(leftOffset, 25),
               MyColor.White);

            // right stats - players 
            int count = this.peasants.Where(p => p.Dead == false).Count();
            this.DrawLeftStats(
                Assets.Images["PeasantRight"],
                Assets.Images["PeasantRight"],
                count,
                count > 0 ? this.peasants.First().Caliber : 0,
                leftOffset + rightOffset,
                -5,
                0);

            count = this.soldiers.Where(p => p.Dead == false).Count();
            this.DrawLeftStats(
                Assets.Images["SoldierRight"],
                Assets.Images["PeasantRight"],
                count,
                count > 0 ? this.soldiers.First().Caliber : 0,
                leftOffset + rightOffset,
                20,
                25);

            count = this.farmers.Where(p => p.Dead == false).Count();
            this.DrawLeftStats(
                Assets.Images["FarmerRight"],
                Assets.Images["PeasantRight"],
                count,
                count > 0 ? this.farmers.First().Caliber : 0,
                leftOffset + rightOffset,
                45,
                50);

            count = this.medics.Where(p => p.Dead == false).Count();
            this.DrawLeftStats(
                Assets.Images["MedicRight"],
                Assets.Images["PeasantRight"],
                count,
                count > 0 ? this.medics.First().Caliber : 0,
                leftOffset + rightOffset,
                70,
                75);

            count = this.towers.Where(p => p.Status == Building.Status.Built).Count();
            this.DrawLeftStats(
                Assets.Images["Tower"],
                Assets.Images["PeasantRight"],
                count,
                count > 0 ? this.towers.First().Caliber : 0,
                leftOffset + rightOffset,
                95,
                100,
                true);

            // messages
            Game1.MessageBuffer.Draw(this.Game.SpriteBatch, this.camera.Transform.Translation.X, true);

            // touch controls
            TouchControls.Draw(this.Game.SpriteBatch, this.camera.Transform.Translation.X);

            // game objects
            foreach (BuildingSpot buildingSpot in this.buildingSpots)
            {
                buildingSpot.Draw(this.Game.SpriteBatch);
            }
            foreach (BuildingSpot inactiveBuildingSpot in this.slums)
            {
                inactiveBuildingSpot.Draw(this.Game.SpriteBatch);
            }

            this.center?.Draw(this.Game.SpriteBatch);

            this.locomotive?.Draw(this.Game.SpriteBatch);

            this.ship?.Draw(this.Game.SpriteBatch);

            this.treasure?.Draw(this.Game.SpriteBatch);

            foreach (Armory armory in this.armories)
            {
                armory.Draw(this.Game.SpriteBatch);
            }

            foreach (Hospital hospital in this.hospitals)
            {
                hospital.Draw(this.Game.SpriteBatch);
            }

            foreach (Market market in this.markets)
            {
                market.Draw(this.Game.SpriteBatch);
            }

            foreach (Arsenal arsenal in this.arsenals)
            {
                arsenal.Draw(this.Game.SpriteBatch);
            }

            foreach (Farm farm in this.farms)
            {
                farm.Draw(this.Game.SpriteBatch);
            }

            foreach (Rails rails in this.rails)
            {
                rails.Draw(this.Game.SpriteBatch);
            }

            foreach (Tower tower in this.towers)
            {
                tower.Draw(this.Game.SpriteBatch);
            }

            foreach (Lenin lenin in this.lenins)
            {
                lenin.Draw(this.Game.SpriteBatch);
            }

            foreach (Pig pig in this.pigs)
            {
                pig.Draw(this.Game.SpriteBatch);
            }

            foreach (Enemy enemy in this.enemies)
            {
                enemy.Draw(this.Game.SpriteBatch);
            }

            foreach (Soldier soldier in this.soldiers)
            {
                soldier.Draw(this.Game.SpriteBatch);
            }

            foreach (Farmer farmer in this.farmers)
            {
                farmer.Draw(this.Game.SpriteBatch);
            }

            foreach (Medic medic in this.medics)
            {
                medic.Draw(this.Game.SpriteBatch);
            }

            foreach (Homeless homeless in this.homelesses)
            {
                homeless.Draw(this.Game.SpriteBatch);
            }

            foreach (Peasant peasant in this.peasants)
            {
                peasant.Draw(this.Game.SpriteBatch);
            }

            foreach (Coin coin in this.coins)
            {
                coin.Draw(this.Game.SpriteBatch);
            }

            // player - camera follows him
            this.player.Draw(this.Game.SpriteBatch);

            // raining - most front
            this.sky.Draw(Game.SpriteBatch);

            this.Game.DrawEnd();
        }

        private void DrawLeftStats(Texture2D sprite, Texture2D frame, int count, int caliber, int offset, int offset1, int offset2, bool building = false)
        {
            if (count == 0)
            {
                return;
            }

            // frame - always the same
            this.Game.SpriteBatch.DrawRectangle(
                new Rectangle(offset - 2, Offset.StatusBarY + offset1 - 2, frame.Width / 4 + 4, frame.Height + 4),
                MyColor.Gray3,
                10);
            this.Game.SpriteBatch.DrawRectangle(
                new Rectangle(offset - 2, Offset.StatusBarY + offset1 - 2, frame.Width / 4 + 4, frame.Height + 4),
                MyColor.DarkerViolet);

            // sprite and count
            if (building)
            {
                this.Game.SpriteBatch.Draw(
                    sprite,
                    new Rectangle(offset, Offset.StatusBarY + offset1, (int)(frame.Width / 3.6f), frame.Height),
                    new Rectangle(0, 0, (int)(sprite.Width / 3.6f), sprite.Height),
                    Color.White);
            }
            else
            {
                this.Game.SpriteBatch.Draw(
                    sprite,
                    new Vector2(offset, Offset.StatusBarY + offset1),
                    new Rectangle(0, 0, sprite.Width / 4, sprite.Height),
                    Color.White);
            }
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                " x " + count.ToString(),
                new Vector2(offset + 20, Offset.StatusBarY + offset1 + frame.Height / 2),
                MyColor.White);

            // sword and attack power
            this.Game.SpriteBatch.Draw(
                Assets.Images["Sword"],
                new Vector2(offset + 70, Offset.StatusBarY + offset2),
                new Rectangle(0, 0, Assets.Images["Sword"].Width, Assets.Images["Sword"].Height),
                Color.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                caliber.ToString(),
                new Vector2(offset + 90, Offset.StatusBarY + offset1 + frame.Height / 2),
                MyColor.White);
        }
    }
}
