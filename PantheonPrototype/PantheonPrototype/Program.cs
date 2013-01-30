using System;

namespace PantheonPrototype
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Pantheon game = new Pantheon())
            {
                game.Run();
            }
        }
    }
#endif
}

