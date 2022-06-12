using System;
using ProofOfWorkProxy.Extensions;

namespace ProofOfWorkProxy.Models
{
    public class ConsoleMessage
    {
        private ConsoleColor color { get; }
        private string message { get; }


        public ConsoleMessage(string message, ConsoleColor color = ConsoleColor.Magenta)
        {
            this.color = color;
            this.message = message;
        }

        public void DisplayMessage(bool addNewLine = true)
        {
            message.Display(color, addNewLine);
        }
    }
}
