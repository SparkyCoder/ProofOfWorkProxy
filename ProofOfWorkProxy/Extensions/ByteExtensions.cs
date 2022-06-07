using System;
using System.Text;

namespace ProofOfWorkProxy.Extensions
{
    public static class ByteExtensions
    {
        public static void Display(this byte[] messageData, ConsoleColor color)
        {
            var requestAscii = Encoding.ASCII.GetString(messageData);
            requestAscii.Display(color);
        }
    }
}
