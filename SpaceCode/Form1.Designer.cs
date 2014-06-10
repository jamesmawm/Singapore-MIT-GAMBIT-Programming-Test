namespace SpaceCode
{
    partial class Form1
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
            this.spaceControl1 = new SpaceCode.SpaceControl();
            this.SuspendLayout();
            // 
            // spaceControl1
            // 
            this.spaceControl1.BackColor = System.Drawing.Color.Transparent;
            this.spaceControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spaceControl1.Location = new System.Drawing.Point(0, 0);
            this.spaceControl1.Name = "spaceControl1";
            this.spaceControl1.Size = new System.Drawing.Size(1099, 726);
            this.spaceControl1.TabIndex = 0;
            this.spaceControl1.Load += new System.EventHandler(this.spaceControl1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1099, 726);
            this.Controls.Add(this.spaceControl1);
            this.Name = "Form1";
            this.Text = "Space";
            this.ResumeLayout(false);

        }

        #endregion

        private SpaceControl spaceControl1;
    }
}

