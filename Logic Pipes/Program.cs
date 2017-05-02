using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using FileOptions2;

namespace Logic_Pipes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Prompt Prompt;
        public static List<FileList> Containers = new List<FileList>();
        public static List<OptionSet> Vars = new List<OptionSet>();
        [STAThread]
        static void Main()
        {
            try
            {
                Dialogue.Message("Hello world! I, " + Environment.MachineName + ", will be your narrator for tonight.");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Fileselect());
                if (Global.Continue)
                {
                    Prompt = new Prompt();
                    var Folder = new FileInfo(Global.Path).Directory;
                    foreach (FileInfo f in Folder.GetFiles())
                    {
                        if (f.FullName.EndsWith(".lpc")) Containers.Add(new FileList(f.FullName));
                        else if (f.FullName.EndsWith(".lpv")) Vars.Add(new OptionSet(f.FullName));
                    }
                    Interpreter.Sysinit();
                    foreach (OptionSet S in Vars) Memory.Init(S);
                    Application.Run(Prompt);
                    Containers.Clear();
                    foreach(Container C in Interpreter.Containers)
                        if (!C.Sys)
                        {
                            File.WriteAllText(Folder.FullName + "\\" + C.Name + ".lpc", null);
                            FileList f = new FileList(Folder.FullName + "\\" + C.Name + ".lpc");
                            f.newElement("%%" + C.Path);
                            if (C.AttatchedEngine() != null) f.newElement("##" + C.AttatchedEngine().Name + "," + C.AttatchedEngine().Action);
                            foreach (Pipe p in C.Attatched) f.newElement(p.Name);
                            Containers.Add(f);
                        }
                    foreach (FileList F in Containers) F.reload();
                    OptionSet s = new OptionSet(Folder.FullName + "\\vars.lpv");
                    foreach (Memory.Var v in Memory.Vars) s.NewEntry(v.name, v.value);
                }
                
            } catch(Exception e)
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
