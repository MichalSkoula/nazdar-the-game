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
                case 4:
                    this.Status = true;
                    this.Power = 1;
                    break;
                case 6:
                    this.Status = true;
                    this.Power = 2;
                    break;
                case 7:
                    this.Status = true;
                    this.Power = 3;
                    break;
            }
        }
    }
}
