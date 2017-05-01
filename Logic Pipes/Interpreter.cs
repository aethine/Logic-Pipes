using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
        public void Add(string contents)
        {
            string o = contents.Trim();
            if (o == "^c") System.IO.File.WriteAllText(Path, null);
            else System.IO.File.AppendAllText(Path, o + Environment.NewLine);
        }
        public Container(string name, string path, params Pipe[] attatch) { Name = name; Path = path; Attatched = attatch.ToList(); }
    }
    class Engine
    {
        public string Name;
        public Container Attached;
        public string Action;
        public Engine(string name, string action, Container attatch) { Name = name; Action = action; Attached = attatch; }
        string[] awords;
        bool runfirst = true;
        public void Run()
        {
            if (!string.IsNullOrEmpty(Action))
            {
                if (runfirst) awords = Action.Trim().Split(' ');
                for (int x = 0; x < awords.Length; x++)
                {
                    if (awords[x].StartsWith("@"))
                    {
                        string place = awords[x].Split('@')[1];
                        int pl = -1;
                        if (int.TryParse(place, out pl)) awords[x] = System.IO.File.ReadAllLines(Attached.Path)[pl];
                        else if (place == "*") //this wil work with only one reference, gotta fix!
                        {
                            runfirst = false;
                            foreach (string S in System.IO.File.ReadAllLines(Attached.Path)) //overflow exception happening here!!!!!!
                            {
                                awords[x] = S;
                                Run();
                            }
                            runfirst = true;
                            return;
                        }
                        else Interpreter.SendDownPipe(Interpreter.FindPipeByName("Output"),
                        "[SYS] Line identifier " + place + " was not recognized");
                    }
                }
                if (awords[0] == "pipe")
                {
                    if (awords.Length >= 3)
                    {
                        string send = null;
                        for (int x = 2; x < awords.Length; x++) send += awords[x];
                        string[] outputs;
                        if (awords[1].Contains(',')) outputs = awords[1].Split(',');
                        else outputs = new string[] { awords[1] };
                        foreach (string o in outputs) Interpreter.SendDownPipe(Interpreter.FindPipeByName(o), send);
                    }
                    else Interpreter.SendDownPipe(Interpreter.FindPipeByName(awords[1]), "[SYS] Not enough parameters");
                }
                else Interpreter.SendDownPipe(Interpreter.FindPipeByName(awords[1]),
                            "[SYS] Command " + awords[0] + " was not recognized");
            }
            else Interpreter.SendDownPipe(
                Interpreter.FindPipeByName("Output"),
                "[SYS] An angine attatched to container " + Attached.Name + "'s Action is blank");

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
        public static Engine FindEngineByName(string name)
        {
            foreach (Engine e in Engines)
                if (e.Name == name) return e;
            throw new ArgumentException();
        }
        public static Engine AttatchedEngine(this Container C)
        {
            foreach (Engine e in Engines)
                if (e.Attached == C) return e;
            return null;
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
                        else if (c.Name == "$input") Interpret(contents);
                        else c.Add(contents);
                        break;
                    }
                }
        }
        public static void Sysinit()
        {
            {
                Pipes.Add(new Pipe("Output"));
                Pipes.Add(new Pipe("Input"));
                Containers.Add(new Container("$output", null));
                Containers.Add(new Container("$input", null));
                Pipes[0].Sys = true;
                Pipes[1].Sys = true;
                Containers[0].Sys = true;
                Containers[1].Sys = true;
                Containers[0].Attatched.Add(Pipes[0]);
                Containers[1].Attatched.Add(Pipes[1]);
            } //system containers/pipes
            foreach (FileList F in Program.Containers)
            {
                string name = null, path = null; Engine te = null; List<Pipe> ps = new List<Pipe>();
                name = new System.IO.FileInfo(F.path).Name.Remove(new System.IO.FileInfo(F.path).Name.Length - 4);
                //Console.WriteLine(name);
                foreach (string L in F.getAllElements())
                {
                    if (L.StartsWith("%%")) path = L.Split('%')[2]; //finding the container path
                    else if (L.StartsWith("##")) te = new Engine(L.Split('#')[2].Split(',')[0], L.Split('#')[2].Split(',')[1], null); //attatched engine
                    else
                    { //doesnt handle for unattatched pipes!!
                        try { ps.Add(FindPipeByName(L)); }
                        catch (ArgumentException) { Pipes.Add(new Pipe(L)); ps.Add(FindPipeByName(L)); }
                    }
                }
                Container c = new Container(name, path, ps.ToArray());
                Containers.Add(c);
                if (te != null) { te.Attached = c; Engines.Add(te); }
            }
        }

        public static void Interpret(string line)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    words = line.Trim().Split(' ');
                    if (words[0] == "pipe")
                    {
                        if (words.Length >= 3) //if the command entered has at least the required amount of parameters
                        {
                            string[] pipes; //pipes to send down
                            if (words[1].Contains(',')) //if youre sending down more than one pipe
                                pipes = words[1].Split(','); //splits into each pipe
                            else pipes = new string[] { words[1] }; //for just single pipe
                            foreach (string s in pipes) //sending the information down each pipe given
                                try { SendDownPipe(FindPipeByName(s), getAllAfter(1)); } //getallafter is getting each word after the second place
                                catch (ArgumentException) { Output("[SYS] Could not find pipe " + s); } //if findpipebyname cant find the pipe
                        }
                        else Output("[SYS] Not enough parameters");
                    } //send information down a declared pipe (view this for an annotated command)
                    else if (words[0] == "mktainer")
                    {
                        if (words.Length >= 3)
                        {
                            bool f = true;
                            foreach (Container c in Containers)
                            {
                                if (c.Name == words[1]) { Output("[SYS] Container already exists with given name"); f = false; break; }
                                if (c.Path == words[2]) { Output("[SYS] Container already exists on given path"); f = false; break; }
                            }
                            if (f)
                            {
                                if (System.IO.File.Exists(words[2]))
                                {
                                    Containers.Add(new Container(words[1], words[2]));
                                    Output("[SYS] Created a container named " + words[1] + " at " + words[2]);
                                }
                                else Output("[SYS] Could not find file given");
                            }
                        }
                        else Output("[SYS] Not enough parameters");

                    } //make a container
                    else if (words[0] == "mkpipe")
                    {
                        if (words.Length >= 2)
                        {
                            bool f = true;
                            foreach (Pipe p in Pipes)
                                if (p.Name == words[1]) { Output("[SYS] Pipe already exists with given name"); f = false; break; }
                            if (f) { Pipes.Add(new Pipe(words[1])); Output("[SYS] Created a pipe named " + words[1]); }
                        }
                        else Output("[SYS] Not enough parameters");
                    } //make a pipe
                    else if (words[0] == "attatch")
                    {
                        if (words.Length >= 3)
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
                    } //attatch a pipe to a container
                    else if (words[0] == "mkfile")
                    {
                        if (words.Length >= 2)
                        {
                            if (!System.IO.File.Exists(words[1]))
                            {
                                try { System.IO.File.WriteAllText(words[1], null); Output("[SYS] Created " + words[1]); }
                                catch (System.IO.IOException) { Output("[SYS] Path not valid"); }
                            }
                            else Output("[SYS] File already exists");
                        }
                        else Output("[SYS] Not enough parameters");
                    } //make a physical file
                    else if (words[0] == "mkengine")
                    {
                        if (words.Length >= 4)
                        {
                            bool f = true;
                            foreach (Engine e in Engines)
                                if (e.Name == words[1]) { f = false; Output("[SYS] Engine already exists with given name"); break; }
                            try
                            {
                                if (f)
                                {
                                    Engines.Add(new Engine(words[1], getAllAfter(2), FindContByName(words[2])));
                                    Output
                                        ("[SYS] Created an engine named " + words[1] +
                                        " attatched to container " + words[2] +
                                        " with action: " + getAllAfter(2));
                                }
                            }
                            catch (ArgumentException) { Output("[SYS] Container does not exist under name given"); }
                        }
                        else Output("[SYS] Not enough parameters");
                    } //make an engine and attatch it to a container
                    else if (words[0] == "runengine")
                    {
                        if (words.Length >= 2)
                        {
                            if (words[1].Contains(','))
                            {
                                foreach (string en in words[1].Split(','))
                                    try { FindEngineByName(en).Run(); }
                                    catch (ArgumentException) { Output("[SYS] Could not find engine with name " + en); }

                            }
                            else FindEngineByName(words[1]).Run();
                        }
                        else Output("[SYS] Not enough parameters");
                    } //execute whatever a declared engine does
                    else if (words[0] == "deltainer")
                    {
                        if (words.Length >= 2)
                            Containers.Remove(FindContByName(words[1]));
                        else Output("[SYS] Not enough parameters");
                    } //delete a container (doesn't delete the file)
                    else if (words[0] == "delpipe")
                    {
                        if (words.Length >= 2)
                            Pipes.Remove(FindPipeByName(words[1]));
                        else Output("[SYS] Not enough parameters");
                    } //delete a pipe
                    else if (words[0] == "delengine")
                    {
                        if (words.Length >= 2)
                            Engines.Remove(FindEngineByName(words[1]));
                        else Output("[SYS] Not enough parameters");
                        //not removing it from its container because it cant execute if it doesnt exist in the list
                        //and can just be replaced later
                    } //delete an engine
                    else if (words[0] == "delfile")
                    {
                        if (words.Length >= 2)
                            System.IO.File.Delete(words[1]);
                        else Output("[SYS] Not enough parameters");
                    } //delete a file
                    else if (words[0] == "list")
                    {
                        if (words.Length >= 2)
                        {
                            switch (words[1])
                            {
                                case "container":
                                    foreach (Container c in Containers)
                                    {
                                        Output("-" + c.Name + ":");
                                        Engine e = c.AttatchedEngine();
                                        if (e != null) Output("--Engine: " + e.Name + " (" + e.Action.Trim() + ")");
                                        else Output("--Engine: None");
                                        foreach (Pipe p in c.Attatched) Output("--" + p.Name);
                                    }
                                    break;
                                case "pipe":
                                    foreach (Pipe p in Pipes) Output("-" + p.Name);
                                    break;
                                case "engine":
                                    foreach(Engine e in Engines)
                                    {
                                        Output("-" + e.Name + ":");
                                        Output("--Attatched to: " + e.Attached.Name);
                                        Output("--Action: " + e.Action);
                                    }
                                    break;
                                default:
                                    Output("[SYS] Type " + words[1] + " to list not found");
                                    break;
                            }
                        }
                        else Output("[SYS] Not enough parameters");
                    } //list types and details of those types
                    else if (words[0] == "cls") Program.Prompt.Output.Text = null; //clears console
                    else if (words[0] == "exit") Program.Prompt.Close(); //exits
                    else Output("[SYS] Command not found: " + words[0]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show
                    ("An exception has occurred: " + e.Message, "Error", MessageBoxButtons.OK);
                Dialogue.Error
                    ("An unhandled exception occurred! "
                    + e.GetType()
                    + ": " + e.Message);
            }
        }
    }
}
