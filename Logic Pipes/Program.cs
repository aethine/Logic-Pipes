using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Logic_Pipes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Prompt Prompt;
        public static List<FileList> Containers = new List<FileList>();
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
                        if (f.FullName.EndsWith(".lpc")) Containers.Add(new FileList(f.FullName));
                    Interpreter.Sysinit();
                    Application.Run(Prompt);
                    Containers.Clear();
                    foreach(Container C in Interpreter.Containers)
                        if (!C.Sys)
                        {
                            FileList f = new FileList(Folder.FullName + C.Name + ".lpc");
                            f.newElement("%%" + C.Path);
                            if (C.AttatchedEngine() != null) f.newElement("##" + C.AttatchedEngine().Name + "," + C.AttatchedEngine().Action);
                            foreach (Pipe p in C.Attatched) f.newElement(p.Name);
                            Containers.Add(f);
                        }
                    foreach (FileList F in Containers) F.reload();
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
