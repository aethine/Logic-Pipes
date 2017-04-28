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
        public string Name;
        public string Path;
        public bool Sys = false;
        public List<Pipe> Attatched;
        public void Add(string contents) { System.IO.File.AppendAllText(Path, contents + Environment.NewLine); }
        public Container(string name, string path, params Pipe[] attatch) { Name = name; Path = path; Attatched = attatch.ToList(); }
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
        public static Container FindContByName(string name)
        {
            foreach (Container c in Containers)
                if (c.Name == name) return c;
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
                        if (c.Name == "$output") Output(contents);
                        else c.Add(contents);
                        break;
                    }
                }
        }
        public static void Sysinit()
        {
            Pipes.Add(new Pipe("Output"));
            Containers.Add(new Container("$output", null));
            Pipes[0].Sys = true;
            Containers[0].Sys = true;
            Containers[0].Attatched.Add(Pipes[0]);
        }
        public static void Interpret(string line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                words = line.Split(' ');
                if (words[0] == "pipe")
                {
                    if (words.Length >= 3)
                    {
                        string[] pipes;
                        if (words[1].Contains(','))
                            pipes = words[1].Split(',');
                        else pipes = new string[] { words[1] };
                        foreach (string s in pipes)
                            try { SendDownPipe(FindPipeByName(s), getAllAfter(1)); }
                            catch (ArgumentException) { Output("[SYS] Could not find pipe " + s); }
                    }
                    else Output("[SYS] Not enough parameters");
                }
                else if (words[0] == "mktainer")
                {
                    if (words.Length >= 3)
                    {
                        bool f = true;
                        foreach (Container c in Containers)
                        {
                            if (c.Name == words[1]) { Output("[SYS] container already exists with given name"); f = false; break; }
                            if (c.Path == words[2]) { Output("[SYS] container already exists on given path"); f = false; break; }
                        }
                        if (f)
                        {
                            if (System.IO.File.Exists(words[2]))
                            { Containers.Add(new Container(words[1], words[2]));
                              Output("[SYS] Created a container named " + words[1] + " at " + words[2]); }
                            else Output("[SYS] Could not find file given");
                        }
                    }
                    else Output("[SYS] Not enough parameters");

                }
                else if (words[0] == "mkpipe")
                {
                    if (words.Length >= 2)
                    {
                        bool f = true;
                        foreach(Pipe p in Pipes)
                            if(p.Name == words[1]) { Output("[SYS] pipe already exists with given name"); f = false; break; }
                        if (f) { Pipes.Add(new Pipe(words[1])); Output("[SYS] Created a pipe called " + words[1]); }
                    }
                    else Output("[SYS] Not enough parameters");
                }
                else if (words[0] == "attatch")
                {
                    if(words.Length >= 3)
                    {
                        Container c;
                        Pipe p;
                        try
                        {
                            p = FindPipeByName(words[1]);
                            try
                            {
                                c = FindContByName(words[2]);
                                c.Attatched.Add(p);
                                Output("[SYS] attatched pipe " + words[1] + " to container " + words[2]);
                            }
                            catch (ArgumentException) { Output("[SYS] Container not found"); }
                        }
                        catch (ArgumentException) { Output("[SYS] Pipe not found"); }
                    }
                    else Output("[SYS] Not enough parameters");
                }
                else if (words[0] == "mkfile")
                {
                    if(words.Length >= 2)
                    {
                        if (!System.IO.File.Exists(words[1]))
                        {
                            try { System.IO.File.WriteAllText(words[1], null); Output("[SYS] Created " + words[1]); }
                            catch (System.IO.IOException) { Output("[SYS] Path not valid"); }
                        }
                        else Output("[SYS] File already exists");
                    }
                    else Output("[SYS] Not enough parameters");
                }
                else Output("[SYS] Command " + words[0] + " not found");
            }
        }
    }
}
