using System;

namespace AITW
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                Console.WriteLine("Version: {0}", Environment.Version);
                Console.WriteLine("OS: {0}", Environment.OSVersion);
                Console.WriteLine("Command: {0}", Environment.CommandLine);

                game.Run();
            }
        }
    }
#endif
}

