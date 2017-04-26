using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_Pipes
{
    static class Dialogue
    {
        public static void Message(string message) { Console.WriteLine(message); }
        public static void Error(string message) { Message("Uh oh, " + message); Environment.Exit(1); }
    }
}
