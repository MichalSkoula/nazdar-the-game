using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Objects;
using Nazdar.Shared;
using Nazdar.Weather;
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
            // background - ground
            Assets.TilesetGroups["village" + this.Game.Village].Draw("ground", this.Game.SpriteBatch);
            // background - edges
            Assets.TilesetEdges["left" + this.Game.Village].Draw("ground", this.Game.SpriteBatch, -Offset.MapTilesOffset);
            Assets.TilesetEdges["right" + this.Game.Village].Draw("ground", this.Game.SpriteBatch, MapWidth);

            // stats --------------------------------------------------------------------------
            int leftOffset = Offset.StatusBarX - (int)this.camera.Transform.Translation.X;
            int rightOffset = Game1.CurrentPlatform == Enums.Platform.Android ? 400 : 520;

            // healthbar
            this.Game.SpriteBatch.DrawRectangle(
                new Rectangle(leftOffset, Offset.StatusBarY, 100, 10),
                MyColor.Black
            );
            this.Game.SpriteBatch.DrawRectangle(
                new Rectangle(leftOffset + 1, Offset.StatusBarY + 1, 98, 8),
                MyColor.White,
                8
            );
            this.Game.SpriteBatch.DrawRectangle(
                new Rectangle(leftOffset + 1, Offset.StatusBarY + 1, (int)((this.player.Health / 100f) * 98), 8),
                MyColor.Green,
                8
            );

            // day or night
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Attack " + this.player.Caliber.ToString(),
                new Vector2(leftOffset + 160, Offset.StatusBarY + 1),
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
                this.Game.Village + ". " + (Villages)this.Game.Village + ", day " + this.player.Days.ToString() + ".",
                new Vector2(leftOffset, 10),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
               Assets.Fonts["Small"],
               Tools.DayPhaseTimerToHours(this.dayPhase, this.dayPhaseTimer),
               new Vector2(leftOffset, 25),
               MyColor.White);

            // right stats
            this.Game.SpriteBatch.Draw(
                Assets.Images["PeasantRight"],
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY),
                new Rectangle(0, 0, Assets.Images["PeasantRight"].Width / 4, Assets.Images["PeasantRight"].Height),
                Color.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                " x " + (this.peasants.Count).ToString(),
                new Vector2(leftOffset + rightOffset + 20, Offset.StatusBarY + Assets.Images["PeasantRight"].Height / 2),
                MyColor.White);

            this.Game.SpriteBatch.Draw(
                Assets.Images["FarmerRight"],
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 25),
                new Rectangle(0, 0, Assets.Images["FarmerRight"].Width / 4, Assets.Images["FarmerRight"].Height),
                Color.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                " x " + (this.farmers.Count).ToString(),
                new Vector2(leftOffset + rightOffset + 20, Offset.StatusBarY + 25 + Assets.Images["FarmerRight"].Height / 2),
                MyColor.White);

            this.Game.SpriteBatch.Draw(
                Assets.Images["SoldierRight"],
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 50),
                new Rectangle(0, 0, Assets.Images["SoldierRight"].Width / 4, Assets.Images["SoldierRight"].Height),
                Color.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                " x " + (this.soldiers.Count).ToString(),
                new Vector2(leftOffset + rightOffset + 20, Offset.StatusBarY + 50 + Assets.Images["SoldierRight"].Height / 2),
                MyColor.White);

            this.Game.SpriteBatch.Draw(
                Assets.Images["MedicRight"],
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 75),
                new Rectangle(0, 0, Assets.Images["MedicRight"].Width / 4, Assets.Images["MedicRight"].Height),
                Color.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                " x " + (this.medics.Count).ToString(),
                new Vector2(leftOffset + rightOffset + 20, Offset.StatusBarY + 75 + Assets.Images["MedicRight"].Height / 2),
                MyColor.White);


            // messages
            Game1.MessageBuffer.Draw(this.Game.SpriteBatch, this.camera.Transform.Translation.X);

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

            foreach (Enemy enemy in this.enemies)
            {
                enemy.Draw(this.Game.SpriteBatch);
            }

            foreach (Pig pig in this.pigs)
            {
                pig.Draw(this.Game.SpriteBatch);
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
    }
}
