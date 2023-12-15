namespace Nazdar.Shared
{
    internal class Disease
    {
        public bool Status { get; private set; } = false;
        public int Power { get; private set; }

        public Disease(int village)
        {
            switch (village)
            {
                case 0:
                    this.Status = true;
                    this.Power = 2;
                    break;
                case 4:
                    this.Status = true;
                    this.Power = 1;
                    break;
                case 6:
                case 8:
                    this.Status = true;
                    this.Power = 2;
                    break;
            }
        }
    }
}
