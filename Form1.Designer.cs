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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCleanAllProcess = new System.Windows.Forms.Button();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chbLogError = new System.Windows.Forms.CheckBox();
            this.txtActionSleep = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtManageSiteUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtManualLoad = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnterLoad = new System.Windows.Forms.TextBox();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPageLoad = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(380, 55);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(315, 57);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnCleanAllProcess
            // 
            this.btnCleanAllProcess.Location = new System.Drawing.Point(380, 385);
            this.btnCleanAllProcess.Name = "btnCleanAllProcess";
            this.btnCleanAllProcess.Size = new System.Drawing.Size(315, 63);
            this.btnCleanAllProcess.TabIndex = 1;
            this.btnCleanAllProcess.Text = "Dọn sạch các tiến trình chạy ngầm";
            this.btnCleanAllProcess.UseVisualStyleBackColor = true;
            this.btnCleanAllProcess.Click += new System.EventHandler(this.BtnCleanAllProcess_Click);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(985, 528);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(123, 20);
            this.linkLabel2.TabIndex = 3;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Ngô Thành Vinh";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel2_LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1100, 498);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnStart);
            this.tabPage1.Controls.Add(this.btnCleanAllProcess);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1092, 465);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Trang chủ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chbLogError);
            this.tabPage2.Controls.Add(this.txtActionSleep);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtManageSiteUrl);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtManualLoad);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtEnterLoad);
            this.tabPage2.Controls.Add(this.btnSaveConfig);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.txtPageLoad);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1092, 465);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cấu hình";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chbLogError
            // 
            this.chbLogError.AutoSize = true;
            this.chbLogError.Location = new System.Drawing.Point(27, 247);
            this.chbLogError.Name = "chbLogError";
            this.chbLogError.Size = new System.Drawing.Size(132, 24);
            this.chbLogError.TabIndex = 13;
            this.chbLogError.Text = "Ghi lỗi vào file";
            this.chbLogError.UseVisualStyleBackColor = true;
            // 
            // txtActionSleep
            // 
            this.txtActionSleep.Location = new System.Drawing.Point(487, 181);
            this.txtActionSleep.Name = "txtActionSleep";
            this.txtActionSleep.Size = new System.Drawing.Size(107, 26);
            this.txtActionSleep.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(458, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Thời gian nghỉ sau mỗi hành động click button, nhập text... (giây)";
            // 
            // txtManageSiteUrl
            // 
            this.txtManageSiteUrl.Location = new System.Drawing.Point(176, 121);
            this.txtManageSiteUrl.Name = "txtManageSiteUrl";
            this.txtManageSiteUrl.Size = new System.Drawing.Size(347, 26);
            this.txtManageSiteUrl.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "URL trang quản lý";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(333, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Thời gian tối đa mỗi hành động thủ công (giây)";
            // 
            // txtManualLoad
            // 
            this.txtManualLoad.Location = new System.Drawing.Point(362, 74);
            this.txtManualLoad.Name = "txtManualLoad";
            this.txtManualLoad.Size = new System.Drawing.Size(106, 26);
            this.txtManualLoad.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(412, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(329, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Thời gian chờ tối đa sau khi nhấn Enter  (giây)";
            // 
            // txtEnterLoad
            // 
            this.txtEnterLoad.Location = new System.Drawing.Point(747, 25);
            this.txtEnterLoad.Name = "txtEnterLoad";
            this.txtEnterLoad.Size = new System.Drawing.Size(106, 26);
            this.txtEnterLoad.TabIndex = 3;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(477, 350);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(150, 60);
            this.btnSaveConfig.TabIndex = 2;
            this.btnSaveConfig.Text = "Lưu cấu hình";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.BtnSaveConfig_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Thời gian tối đa tải trang (giây)";
            // 
            // txtPageLoad
            // 
            this.txtPageLoad.Location = new System.Drawing.Point(249, 27);
            this.txtPageLoad.Name = "txtPageLoad";
            this.txtPageLoad.Size = new System.Drawing.Size(106, 26);
            this.txtPageLoad.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 557);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.linkLabel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MTookit";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCleanAllProcess;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPageLoad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEnterLoad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtManualLoad;
        private System.Windows.Forms.TextBox txtManageSiteUrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtActionSleep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chbLogError;
    }
}

