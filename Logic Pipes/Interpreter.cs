using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_Pipes
{
    class Pipe
    {
        public string Name;
        public bool System = false;
        public Pipe(string name) { Name = name; }
    }
    class Container
    {
        public string Path;
        public bool System = false;
        public List<Pipe> Attatched;
        public Container(string path, params Pipe[] attatch) { Path = path; Attatched = attatch.ToList(); }
    }
    class Engine //todo
    {
        Container Attached;
        public string Action;
        public Engine(string action, Container attatch) { Action = action; Attached = attatch; }
        public string Run() //todo
        {
            return System.IO.File.ReadAllText(Attached.Path);
        }
    }
    static class Interpreter
    {
        static void Output(string message)
        {
            Program.Prompt.Output.Text += message + Environment.NewLine;
        }
        public static void Interpret(string line)
        {
            string[] words = line.Split(' ');
        }
    }
}
