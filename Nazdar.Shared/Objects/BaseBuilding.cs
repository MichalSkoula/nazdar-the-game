using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public abstract class BaseBuilding : BaseObject
    {
        public bool IsBeingBuilt { get; set; }
        public float TimeToBuilt;
        public Building.Type Type;
        public Building.Status Status = Building.Status.InProcess;

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            this.Alpha = this.Status == Building.Status.InProcess ? 0.25f : 1;

            if (this.TimeToBuilt > 0 && this.IsBeingBuilt)
            {
                this.TimeToBuilt -= deltaTime;
            }

            if (this.TimeToBuilt < 0)
            {
                this.Status = Building.Status.Built;
                this.TimeToBuilt = 0;
                Game1.MessageBuffer.AddMessage("Building built", MessageType.Success);
            }
        }
    }
}
