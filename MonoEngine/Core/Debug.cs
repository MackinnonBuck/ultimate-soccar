using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public static class Debug
    {
        /// <summary>
        /// Used for defining the severity of the log.
        /// </summary>
        public enum LogSeverity
        {
            MESSAGE = ConsoleColor.Cyan,
            WARNING = ConsoleColor.Yellow,
            ERROR = ConsoleColor.Red
        }

        /// <summary>
        /// Logs an object to the console.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="severity"></param>
        public static void Log(object o, LogSeverity severity = LogSeverity.MESSAGE)
        {
#if DEBUG
            Console.ForegroundColor = (ConsoleColor)severity;
            Console.WriteLine(o.ToString());
#endif
        }

        /// <summary>
        /// Clears the console of all existing messages.
        /// </summary>
        public static void Clear()
        {
#if DEBUG
            Console.Clear();
#endif
        }
    }
}
