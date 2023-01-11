using System.Threading.Tasks;

namespace Nazdar.Shared
{
    public static class MyShake
    {
        public static bool Active { get; set; }

        public static async void Shake(int ms = 250)
        {
            if (Active)
            {
                return;
            }

            Active = true;
            await Task.Delay(ms);
            Active = false;
        }
    }
}
