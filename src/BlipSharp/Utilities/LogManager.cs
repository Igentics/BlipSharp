using System.Diagnostics;

namespace BlipSharp.Utilities
{
    public class LogManager
    {
        public static void Log(string message)
        {
            Log(message, null);
        }

        public static void Log(string message, params object[] args)
        {
            if(args == null)
                Debug.WriteLine(message);
            else
                Debug.WriteLine(message, args);
        }
    }
}