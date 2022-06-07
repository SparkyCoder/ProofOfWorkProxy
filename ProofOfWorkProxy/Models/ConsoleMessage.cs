using System;
using ProofOfWorkProxy.Extensions;

namespace ProofOfWorkProxy.Models
{
    public class ConsoleMessage
    {
        private ConsoleColor Color { get; }
        private string Message { get; }

        public ConsoleMessage(string message, ConsoleColor color)
        {
            Color = color;
            Message = message;
        }

        public void DisplayMessage()
        {
            Message.Display(Color);
        }
    }
}
