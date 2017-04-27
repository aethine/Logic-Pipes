using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_Pipes
{
    public partial class Prompt : Form
    {
        public Prompt()
        {
            InitializeComponent();
            Input.KeyPress += new KeyPressEventHandler(CheckEnterKeyPress);
        }
        private void CheckEnterKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                Enter.PerformClick();
        }
        private void Enter_Click(object sender, EventArgs e)
        {
            Interpreter.Interpret(Input.Text);
            Input.Text = null;
        }
    }
}
