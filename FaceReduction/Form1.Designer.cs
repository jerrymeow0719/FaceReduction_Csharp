namespace FaceReduction
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.File_Before = new System.Windows.Forms.PictureBox();
            this.File_After = new System.Windows.Forms.PictureBox();
            this.Group_PictureBox = new System.Windows.Forms.GroupBox();
            this.btm_Detect = new System.Windows.Forms.Button();
            this.ClassifierPanel = new System.Windows.Forms.Panel();
            this.radioButton_YOLO = new System.Windows.Forms.RadioButton();
            this.radioButton_Cascade = new System.Windows.Forms.RadioButton();
            this.btm_Pause = new System.Windows.Forms.Button();
            this.btm_Play = new System.Windows.Forms.Button();
            this.btm_Start = new System.Windows.Forms.Button();
            this.btm_Load = new System.Windows.Forms.Button();
            this.Text_FilePath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.File_Before)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.File_After)).BeginInit();
            this.Group_PictureBox.SuspendLayout();
            this.ClassifierPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // File_Before
            // 
            this.File_Before.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.File_Before.Location = new System.Drawing.Point(6, 21);
            this.File_Before.Name = "File_Before";
            this.File_Before.Size = new System.Drawing.Size(410, 350);
            this.File_Before.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.File_Before.TabIndex = 0;
            this.File_Before.TabStop = false;
            // 
            // File_After
            // 
            this.File_After.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.File_After.Location = new System.Drawing.Point(429, 21);
            this.File_After.Name = "File_After";
            this.File_After.Size = new System.Drawing.Size(410, 350);
            this.File_After.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.File_After.TabIndex = 1;
            this.File_After.TabStop = false;
            // 
            // Group_PictureBox
            // 
            this.Group_PictureBox.Controls.Add(this.btm_Detect);
            this.Group_PictureBox.Controls.Add(this.ClassifierPanel);
            this.Group_PictureBox.Controls.Add(this.btm_Pause);
            this.Group_PictureBox.Controls.Add(this.btm_Play);
            this.Group_PictureBox.Controls.Add(this.btm_Start);
            this.Group_PictureBox.Controls.Add(this.btm_Load);
            this.Group_PictureBox.Controls.Add(this.Text_FilePath);
            this.Group_PictureBox.Controls.Add(this.File_After);
            this.Group_PictureBox.Controls.Add(this.File_Before);
            this.Group_PictureBox.Location = new System.Drawing.Point(12, 12);
            this.Group_PictureBox.Name = "Group_PictureBox";
            this.Group_PictureBox.Size = new System.Drawing.Size(845, 435);
            this.Group_PictureBox.TabIndex = 2;
            this.Group_PictureBox.TabStop = false;
            // 
            // btm_Detect
            // 
            this.btm_Detect.Enabled = false;
            this.btm_Detect.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btm_Detect.Location = new System.Drawing.Point(619, 403);
            this.btm_Detect.Name = "btm_Detect";
            this.btm_Detect.Size = new System.Drawing.Size(107, 23);
            this.btm_Detect.TabIndex = 9;
            this.btm_Detect.Text = "Detect";
            this.btm_Detect.UseVisualStyleBackColor = true;
            this.btm_Detect.Click += new System.EventHandler(this.btm_Detect_Click);
            // 
            // ClassifierPanel
            // 
            this.ClassifierPanel.Controls.Add(this.radioButton_YOLO);
            this.ClassifierPanel.Controls.Add(this.radioButton_Cascade);
            this.ClassifierPanel.Location = new System.Drawing.Point(429, 406);
            this.ClassifierPanel.Name = "ClassifierPanel";
            this.ClassifierPanel.Size = new System.Drawing.Size(145, 25);
            this.ClassifierPanel.TabIndex = 8;
            // 
            // radioButton_YOLO
            // 
            this.radioButton_YOLO.AutoSize = true;
            this.radioButton_YOLO.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_YOLO.Location = new System.Drawing.Point(84, 3);
            this.radioButton_YOLO.Name = "radioButton_YOLO";
            this.radioButton_YOLO.Size = new System.Drawing.Size(59, 20);
            this.radioButton_YOLO.TabIndex = 1;
            this.radioButton_YOLO.TabStop = true;
            this.radioButton_YOLO.Text = "YOLO";
            this.radioButton_YOLO.UseVisualStyleBackColor = true;
            // 
            // radioButton_Cascade
            // 
            this.radioButton_Cascade.AutoSize = true;
            this.radioButton_Cascade.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_Cascade.Location = new System.Drawing.Point(3, 3);
            this.radioButton_Cascade.Name = "radioButton_Cascade";
            this.radioButton_Cascade.Size = new System.Drawing.Size(75, 20);
            this.radioButton_Cascade.TabIndex = 0;
            this.radioButton_Cascade.TabStop = true;
            this.radioButton_Cascade.Text = "Cascade";
            this.radioButton_Cascade.UseVisualStyleBackColor = true;
            // 
            // btm_Pause
            // 
            this.btm_Pause.Enabled = false;
            this.btm_Pause.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btm_Pause.Location = new System.Drawing.Point(119, 403);
            this.btm_Pause.Name = "btm_Pause";
            this.btm_Pause.Size = new System.Drawing.Size(107, 23);
            this.btm_Pause.TabIndex = 7;
            this.btm_Pause.Text = "Pause";
            this.btm_Pause.UseVisualStyleBackColor = true;
            this.btm_Pause.Click += new System.EventHandler(this.btm_Pause_Click);
            // 
            // btm_Play
            // 
            this.btm_Play.Enabled = false;
            this.btm_Play.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btm_Play.Location = new System.Drawing.Point(6, 403);
            this.btm_Play.Name = "btm_Play";
            this.btm_Play.Size = new System.Drawing.Size(107, 23);
            this.btm_Play.TabIndex = 6;
            this.btm_Play.Text = "Play";
            this.btm_Play.UseVisualStyleBackColor = true;
            this.btm_Play.Click += new System.EventHandler(this.btm_Play_Click);
            // 
            // btm_Start
            // 
            this.btm_Start.Enabled = false;
            this.btm_Start.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btm_Start.Location = new System.Drawing.Point(732, 403);
            this.btm_Start.Name = "btm_Start";
            this.btm_Start.Size = new System.Drawing.Size(107, 23);
            this.btm_Start.TabIndex = 5;
            this.btm_Start.Text = "Start";
            this.btm_Start.UseVisualStyleBackColor = true;
            this.btm_Start.Click += new System.EventHandler(this.btm_Start_Click);
            // 
            // btm_Load
            // 
            this.btm_Load.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btm_Load.Location = new System.Drawing.Point(732, 377);
            this.btm_Load.Name = "btm_Load";
            this.btm_Load.Size = new System.Drawing.Size(107, 21);
            this.btm_Load.TabIndex = 4;
            this.btm_Load.Text = "Load ...";
            this.btm_Load.UseVisualStyleBackColor = true;
            this.btm_Load.Click += new System.EventHandler(this.btm_Load_Click);
            // 
            // Text_FilePath
            // 
            this.Text_FilePath.Enabled = false;
            this.Text_FilePath.Location = new System.Drawing.Point(6, 378);
            this.Text_FilePath.Name = "Text_FilePath";
            this.Text_FilePath.Size = new System.Drawing.Size(720, 22);
            this.Text_FilePath.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 450);
            this.Controls.Add(this.Group_PictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.File_Before)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.File_After)).EndInit();
            this.Group_PictureBox.ResumeLayout(false);
            this.Group_PictureBox.PerformLayout();
            this.ClassifierPanel.ResumeLayout(false);
            this.ClassifierPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox File_Before;
        private System.Windows.Forms.PictureBox File_After;
        private System.Windows.Forms.GroupBox Group_PictureBox;
        private System.Windows.Forms.Button btm_Start;
        private System.Windows.Forms.Button btm_Load;
        private System.Windows.Forms.TextBox Text_FilePath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btm_Pause;
        private System.Windows.Forms.Button btm_Play;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel ClassifierPanel;
        private System.Windows.Forms.RadioButton radioButton_YOLO;
        private System.Windows.Forms.RadioButton radioButton_Cascade;
        private System.Windows.Forms.Button btm_Detect;
    }
}

