namespace SpaceCode
{
    partial class SpaceControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerTick = new System.Windows.Forms.Timer(this.components);
            this.labelScore = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelAI = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timerTick
            // 
            this.timerTick.Enabled = true;
            this.timerTick.Interval = 10;
            this.timerTick.Tick += new System.EventHandler(this.timerTick_Tick);
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.ForeColor = System.Drawing.Color.White;
            this.labelScore.Location = new System.Drawing.Point(12, 13);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(13, 13);
            this.labelScore.TabIndex = 2;
            this.labelScore.Text = "0";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.ForeColor = System.Drawing.Color.White;
            this.labelTime.Location = new System.Drawing.Point(264, 12);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(28, 13);
            this.labelTime.TabIndex = 3;
            this.labelTime.Text = "1:00";
            // 
            // labelAI
            // 
            this.labelAI.AutoSize = true;
            this.labelAI.ForeColor = System.Drawing.Color.White;
            this.labelAI.Location = new System.Drawing.Point(625, 12);
            this.labelAI.Name = "labelAI";
            this.labelAI.Size = new System.Drawing.Size(28, 13);
            this.labelAI.TabIndex = 4;
            this.labelAI.Text = "";
            // 
            // SpaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.labelAI);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelScore);
            this.Name = "SpaceControl";
            this.Size = new System.Drawing.Size(665, 472);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerTick;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Label labelTime;
        public System.Windows.Forms.Label labelAI;
    }
}
