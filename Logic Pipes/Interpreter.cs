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
        public bool Sys = false;
        public Pipe(string name) { Name = name; }
    }
    class Container
    {
        public string Path;
        public bool Sys = false;
        public List<Pipe> Attatched;
        public void Add(string contents) { System.IO.File.AppendAllText(Path, contents + Environment.NewLine); }
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
        static string[] words;
        public static List<Pipe> Pipes = new List<Pipe>();
        public static List<Container> Containers = new List<Container>();
        public static List<Engine> Engines = new List<Engine>();
        
        static string getAllAfter(int after)
        {
            string o = null;
            for (int x = after + 1; x < words.Length; x++) o += words[x] + " ";
            return o;
        }
        static void Output(string message)
        {
            Program.Prompt.Output.Text += message + Environment.NewLine;
        }
        public static Pipe FindPipeByName(string name)
        {
            foreach (Pipe p in Pipes)
                if (p.Name == name) return p;
            throw new ArgumentException();
        }
        public static void SendDownPipe(Pipe pipe, string contents)
        {
            List<Container> cs = new List<Container>();
            foreach (Container c in Containers)
                foreach (Pipe p in c.Attatched)
                {
                    if (p == pipe)
                    {
                        if (c.Path == "$output") Output(contents);
                        else c.Add(contents);
                    }
                }
        }
        public static void Sysinit()
        {
            Pipes.Add(new Pipe("Output"));
            Containers.Add(new Container("$output"));
            Pipes[0].Sys = true;
            Containers[0].Sys = true;
            Containers[0].Attatched.Add(Pipes[0]);
        }
        public static void Interpret(string line)
        {
            words = line.Split(' ');
            if(words[0] == "pipe")
            {
                if (words.Length >= 3)
                {
                    string[] pipes;
                    if (words[1].Contains(','))
                        pipes = words[1].Split(',');
                    else pipes = new string[] { words[1]};
                    foreach (string s in pipes)
                        try { SendDownPipe(FindPipeByName(s), getAllAfter(1)); }
                        catch (ArgumentException) { Output("[SYS] Could not find pipe " + s); }
                }
                else Output("[SYS] Not enough parameters");
            }
        }
    }
}
