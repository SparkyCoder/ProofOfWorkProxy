using System;
using System.Linq;

namespace ProofOfWorkProxy.Extensions
{
    public static class StringExtensions
    {
        public static int ToInteger(this string configurationValue)
        {
            return Convert.ToInt32(configurationValue);
        }

        public static bool ToBool(this string configurationValue)
        {
            return Convert.ToBoolean(configurationValue);
        }

        public static void Display(this string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message + Environment.NewLine);
        }

        public static string GetClassName(this string type)
        {
            return type.Split('.').Last();
        }
    }
}
