namespace Logic_Pipes
{
    partial class Prompt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Input = new System.Windows.Forms.TextBox();
            this.Enter = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Input
            // 
            this.Input.AcceptsReturn = true;
            this.Input.Location = new System.Drawing.Point(13, 721);
            this.Input.Multiline = true;
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(827, 31);
            this.Input.TabIndex = 0;
            // 
            // Enter
            // 
            this.Enter.Location = new System.Drawing.Point(587, 774);
            this.Enter.Name = "Enter";
            this.Enter.Size = new System.Drawing.Size(253, 41);
            this.Enter.TabIndex = 1;
            this.Enter.Text = "Enter";
            this.Enter.UseVisualStyleBackColor = true;
            this.Enter.Click += new System.EventHandler(this.Enter_Click);
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(13, 12);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Output.Size = new System.Drawing.Size(827, 669);
            this.Output.TabIndex = 2;
            // 
            // Prompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 830);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Enter);
            this.Controls.Add(this.Input);
            this.Name = "Prompt";
            this.Text = "Logic Prompt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Enter;
        public System.Windows.Forms.TextBox Input;
        public System.Windows.Forms.TextBox Output;
    }
}

