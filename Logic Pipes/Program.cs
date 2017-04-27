using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_Pipes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Dialogue.Message("Hello world! I, " + Environment.MachineName + ", will be your narrator for tonight.");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Fileselect());
                if (Global.Continue) Application.Run(new Prompt());
                
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
