using System;
using System.Linq;
using ProofOfWorkProxy.Exceptions;

namespace ProofOfWorkProxy.Extensions
{
    public static class StringExtensions
    {
        public static bool IsBoolean(this string valueType)
        {
            return valueType == typeof(bool).ToString();
        }

        public static void Display(this string message, ConsoleColor color, bool displayWithNewline = true)
        {
            Console.ForegroundColor = color;
            var addedText = displayWithNewline ? Environment.NewLine : string.Empty;
            Console.WriteLine($"{message} {addedText}");
        }

        public static string GetClassName(this string type)
        {
            return type.Split('.').Last();
        }
    }
}
