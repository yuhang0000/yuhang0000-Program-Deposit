namespace FolderIconFix
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BtnFolder = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnStart = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusTextVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTextNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "路径: ";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(62, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(732, 25);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // BtnFolder
            // 
            this.BtnFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnFolder.Location = new System.Drawing.Point(800, 5);
            this.BtnFolder.Name = "BtnFolder";
            this.BtnFolder.Size = new System.Drawing.Size(32, 23);
            this.BtnFolder.TabIndex = 0;
            this.BtnFolder.Text = "..";
            this.BtnFolder.UseVisualStyleBackColor = true;
            this.BtnFolder.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.Location = new System.Drawing.Point(906, 5);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(62, 23);
            this.BtnExit.TabIndex = 3;
            this.BtnExit.Text = "退出";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.button2_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStart.Location = new System.Drawing.Point(838, 5);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(62, 23);
            this.BtnStart.TabIndex = 2;
            this.BtnStart.Text = "搜索";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.button3_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusTextVersion,
            this.StatusTextNum});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(980, 30);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusTextVersion
            // 
            this.StatusTextVersion.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusTextVersion.Name = "StatusTextVersion";
            this.StatusTextVersion.Size = new System.Drawing.Size(69, 24);
            this.StatusTextVersion.Text = "v0.0.0.1";
            this.StatusTextVersion.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // StatusTextNum
            // 
            this.StatusTextNum.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusTextNum.Name = "StatusTextNum";
            this.StatusTextNum.Size = new System.Drawing.Size(37, 24);
            this.StatusTextNum.Text = "0/0";
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox2.Location = new System.Drawing.Point(0, 37);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(980, 383);
            this.textBox2.TabIndex = 5;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 450);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnFolder);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "FolderIconFIx";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button BtnFolder;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusTextVersion;
        private System.Windows.Forms.ToolStripStatusLabel StatusTextNum;
        private System.Windows.Forms.TextBox textBox2;
    }
}

