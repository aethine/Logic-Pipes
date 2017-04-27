using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_Pipes
{
    static class Interpreter
    {
        static void Output(string message)
        {
            Program.Prompt.Output.Text += message + Environment.NewLine;
        }
    }
}
