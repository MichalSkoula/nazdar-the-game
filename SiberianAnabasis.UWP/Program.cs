namespace SiberianAnabasis
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var factory = new MonoGame.Framework.GameFrameworkViewSource<Game1>();
            Game1.CurrentPlatform = Enums.Platform.UWP;
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
        }
    }
}
