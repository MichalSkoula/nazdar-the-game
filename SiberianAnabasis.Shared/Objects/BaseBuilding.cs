using System;
using System.Collections.Generic;
using System.Text;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public abstract class BaseBuilding : BaseObject
    {
        public bool IsBeingBuilt { get; set; }
        public float TimeToBuilt;
        public Building.Type Type;
        public Building.Status Status = Building.Status.InProcess;

        public void Update(float deltaTime)
        {
            this.Alpha = this.Status == Building.Status.InProcess ? 0.25f : 1;

            if (this.TimeToBuilt > 0 && this.IsBeingBuilt)
            {
                this.TimeToBuilt -= deltaTime;
            }

            if (this.TimeToBuilt <= 0)
            {
                this.Status = Building.Status.Built;
            }
        }
    }
}
