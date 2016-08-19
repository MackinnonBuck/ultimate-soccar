using System;
using System.Runtime.InteropServices;
using UltimateSocCar.Engine;

namespace UltimateSocCar
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Allocates a new console window in debug configuration.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            AllocConsole();
#endif
            App.Instance.Init("Ultimate SocCar", 1280, 720, new TestScene());
            App.Instance.Run();
        }
    }
#endif
}
