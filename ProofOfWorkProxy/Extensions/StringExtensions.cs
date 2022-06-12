using System;
using System.Linq;
using ProofOfWorkProxy.Exceptions;

namespace ProofOfWorkProxy.Extensions
{
    public static class StringExtensions
    {
        public static int ToInteger(this string configurationValue)
        {
            var couldParse = int.TryParse(configurationValue, out var intValue);
            
            if(couldParse) return intValue;

            throw new SettingIsNotANumberException(configurationValue);
        }

        public static bool ToBool(this string configurationValue)
        {
            var couldParse = bool.TryParse(configurationValue, out var boolValue);
            
            if(couldParse) return boolValue;

            throw new ToggleValueIsNotValidException(configurationValue);
        }

        public static bool IsBoolean(this string valueType)
        {
            return valueType == typeof(bool).ToString();
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
