using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Objects;
using Nazdar.Shared;
using System;
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
            Color currentColor = this.GetSkyColor();
            this.GraphicsDevice.Clear(currentColor);

            // background - tileset
            Assets.TilesetEdges["left" + this.Game.Village].Draw("ground", this.Game.SpriteBatch, -Offset.MapTilesOffset);
            Assets.TilesetGroups["village" + this.Game.Village].Draw("ground", this.Game.SpriteBatch);
            Assets.TilesetEdges["right" + this.Game.Village].Draw("ground", this.Game.SpriteBatch, MapWidth);

            // stats --------------------------------------------------------------------------
            int leftOffset = Offset.StatusBarX - (int)this.camera.Transform.Translation.X;
            int rightOffset = Game1.CurrentPlatform == Enums.Platform.Android ? 380 : 460;

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

            // money
            Coin.DrawStatic(this.Game.SpriteBatch, this.player.Money, leftOffset, Offset.StatusBarY + 40, 1);

            // cartridges
            Arsenal.DrawCartridgesStatic(this.Game.SpriteBatch, this.player.Cartridges, leftOffset, Offset.StatusBarY + 75, 1);

            // right stats
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Peasants: " + (this.peasants.Count).ToString(),
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 0),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Farmers: " + (this.farmers.Count).ToString(),
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 10),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Soldiers: " + (this.soldiers.Count).ToString(),
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 20),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Medics: " + (this.medics.Count).ToString(),
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 30),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Kills: " + this.player.Kills.ToString(),
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 40),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
               Assets.Fonts["Small"],
               this.dayPhase.ToString() + " (" + Math.Ceiling(this.dayPhaseTimer).ToString() + ")",
               new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 50),
               MyColor.White);

            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Score: " + (this.player.BaseScore + Tools.GetScore(this.player.Days, this.player.Money, this.peasants.Count, this.soldiers.Count, this.player.Kills, this.center != null ? this.center.Level : 0)).ToString(),
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 70),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                this.Game.Village + ". " + (Villages)this.Game.Village,
                new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 80),
                MyColor.White);
            this.Game.SpriteBatch.DrawString(
               Assets.Fonts["Small"],
               "Day " + this.player.Days.ToString() + ".",
               new Vector2(leftOffset + rightOffset, Offset.StatusBarY + 90),
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

        private Color GetSkyColor()
        {
            float currentPhaseLength = this.dayPhase == DayPhase.Day ? (float)DayNightLength.Day : (float)DayNightLength.Night;
            float progress = (float)((currentPhaseLength - this.dayPhaseTimer) / currentPhaseLength);

            if (this.dayPhase == DayPhase.Day)
            {
                if (progress < 0.2f)
                {
                    // dawn
                    return MyColor.Violet;
                }
                else if (progress > 0.8f)
                {
                    // dusk
                    return MyColor.DarkViolet;
                }
                else
                {
                    // day
                    return MyColor.Blue;
                }
            }
            else
            {
                // night
                return MyColor.DarkerViolet;
            }
        }
    }
}
