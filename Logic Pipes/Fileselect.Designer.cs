namespace Logic_Pipes
{
    partial class Fileselect
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
            this.label1 = new System.Windows.Forms.Label();
            this.fileBox = new System.Windows.Forms.TextBox();
            this.select = new System.Windows.Forms.Button();
            this.openLppDialogue = new System.Windows.Forms.OpenFileDialog();
            this.create = new System.Windows.Forms.Button();
            this.createLppDialogue = new System.Windows.Forms.SaveFileDialog();
            this.confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(137, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(394, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose a .lpp file";
            // 
            // fileBox
            // 
            this.fileBox.Location = new System.Drawing.Point(12, 170);
            this.fileBox.Name = "fileBox";
            this.fileBox.Size = new System.Drawing.Size(636, 31);
            this.fileBox.TabIndex = 1;
            // 
            // select
            // 
            this.select.Location = new System.Drawing.Point(221, 250);
            this.select.Name = "select";
            this.select.Size = new System.Drawing.Size(222, 78);
            this.select.TabIndex = 2;
            this.select.Text = "Browse...";
            this.select.UseVisualStyleBackColor = true;
            this.select.Click += new System.EventHandler(this.button1_Click);
            // 
            // openLppDialogue
            // 
            this.openLppDialogue.FileName = "openFileDialog1";
            // 
            // create
            // 
            this.create.Location = new System.Drawing.Point(221, 350);
            this.create.Name = "create";
            this.create.Size = new System.Drawing.Size(222, 78);
            this.create.TabIndex = 3;
            this.create.Text = "Create New...";
            this.create.UseVisualStyleBackColor = true;
            this.create.Click += new System.EventHandler(this.button2_Click);
            // 
            // confirm
            // 
            this.confirm.Location = new System.Drawing.Point(13, 448);
            this.confirm.Name = "confirm";
            this.confirm.Size = new System.Drawing.Size(636, 70);
            this.confirm.TabIndex = 4;
            this.confirm.Text = "Confirm";
            this.confirm.UseVisualStyleBackColor = true;
            this.confirm.Click += new System.EventHandler(this.confirm_Click);
            // 
            // Fileselect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 530);
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.create);
            this.Controls.Add(this.select);
            this.Controls.Add(this.fileBox);
            this.Controls.Add(this.label1);
            this.Name = "Fileselect";
            this.Text = "Fileselect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileBox;
        private System.Windows.Forms.Button select;
        private System.Windows.Forms.OpenFileDialog openLppDialogue;
        private System.Windows.Forms.Button create;
        private System.Windows.Forms.SaveFileDialog createLppDialogue;
        private System.Windows.Forms.Button confirm;
    }
}