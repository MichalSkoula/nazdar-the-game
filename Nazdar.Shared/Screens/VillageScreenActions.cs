﻿using MonoGame.Extended.Screens;
using Nazdar.Objects;
using Nazdar.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public partial class VillageScreen : GameScreen
    {
        private void Build(BaseBuilding building)
        {
            // is someone building it?
            building.IsBeingBuilt = false;
            foreach (var peasant in this.peasants.Where(p => p.Hitbox.Intersects(building.Hitbox)))
            {
                building.IsBeingBuilt = true;
                peasant.IsBuildingHere = building.Hitbox;
                break;
            }

            // no one is building it
            if (building.IsBeingBuilt == false && this.peasants.Count > 0)
            {
                var nearestPeasant = this.peasants.OrderBy(p => Math.Abs(p.X - building.X)).FirstOrDefault();
                nearestPeasant.IsBuildingHere = building.Hitbox;
            }
        }

        private void Pick(BaseBuilding building)
        {
            if (this.peasants.Count > 0)
            {
                var nearestPeasant = this.peasants.OrderBy(p => Math.Abs(p.X - building.X)).FirstOrDefault();
                nearestPeasant.IsRunningForItem = building.Hitbox;
            }
        }

        private void CreateHomeless()
        {
            this.homelesses.Add(new Homeless(
                this.slumXs[Tools.GetRandom(this.slumXs.Count)],
                Offset.Floor,
                Direction.Right
            ));
        }

        private void EnemyDie(Enemy enemy)
        {
            // maybe drop something?
            if (Tools.GetRandom(this.enemyDropProbability) == 1)
            {
                Game1.MessageBuffer.AddMessage("New coin! Enemy drop.", MessageType.Opportunity);
                this.coins.Add(new Coin(enemy.X, Offset.Floor2));
            }

            Audio.PlayRandomSound("EnemyDeaths");
            this.player.Kills++;
            enemy.Dead = true;
        }

        private void Upgrade()
        {
            foreach (Soldier soldier in this.soldiers)
            {
                soldier.Caliber = Soldier.DefaultCaliber * this.center.Level;
            }
            foreach (Peasant peasant in this.peasants)
            {
                peasant.Caliber = Peasant.DefaultCaliber * this.center.Level;
            }
            foreach (Farmer farmer in this.farmers)
            {
                farmer.Caliber = Farmer.DefaultCaliber * this.center.Level;
            }
        }
    }
}