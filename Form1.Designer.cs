namespace MToolkit
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCleanAllProcess = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(403, 56);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(315, 57);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnCleanAllProcess
            // 
            this.btnCleanAllProcess.Location = new System.Drawing.Point(403, 410);
            this.btnCleanAllProcess.Name = "btnCleanAllProcess";
            this.btnCleanAllProcess.Size = new System.Drawing.Size(315, 63);
            this.btnCleanAllProcess.TabIndex = 1;
            this.btnCleanAllProcess.Text = "Dọn sạch các tiến trình chạy ngầm";
            this.btnCleanAllProcess.UseVisualStyleBackColor = true;
            this.btnCleanAllProcess.Click += new System.EventHandler(this.BtnCleanAllProcess_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 497);
            this.Controls.Add(this.btnCleanAllProcess);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "MTookit";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCleanAllProcess;
    }
}

