using Nazdar.Shared;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public abstract class BaseBuilding : BaseObject
    {
        public Peasant WorkingPeasant { get; set; } = null;
        public float TimeToBuild;
        public Building.Type Type;
        public Building.Status Status = Building.Status.InProcess;

        public BaseBuilding()
        {
            this.Alpha = 0.25f;
            this.Color = MyColor.UniversalColors[Tools.GetRandom(MyColor.UniversalColors.Length)];
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            this.Alpha = this.Status == Building.Status.InProcess ? 0.25f : 1;

            if (this.TimeToBuild > 0 && this.WorkingPeasant != null)
            {
                this.TimeToBuild -= deltaTime;
            }

            // built?
            if (this.TimeToBuild < 0)
            {
                this.Status = Building.Status.Built;
                this.TimeToBuild = 0;
                Game1.MessageBuffer.AddMessage(this.Type.ToString() + " built", MessageType.Success);

                // if market, peasant becomes merchant => die
                if (this.Type == Building.Type.Market)
                {
                    this.WorkingPeasant.ToDelete = true;
                }
            }
        }
    }
}
