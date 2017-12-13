using System;

namespace FCS.Common
{
    public class Logger
    {
        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Error(Exception ex, string additionalMessage = "")
        {
            Console.WriteLine(ex.Message + " " + additionalMessage);

        }
    }
}
