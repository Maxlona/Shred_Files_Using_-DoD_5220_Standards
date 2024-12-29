namespace Stronghold.Forms
{
    partial class FileShredder
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            FileBackGroundWorker = new System.ComponentModel.BackgroundWorker();
            RunProgress = new Label();
            start = new Button();
            selectFileBtn = new Button();
            FileName = new Label();
            label2 = new Label();
            toolTip1 = new ToolTip(components);
            cyclesGroupBox = new GroupBox();
            singleCycle = new RadioButton();
            sevenCycles = new RadioButton();
            fiveCycles = new RadioButton();
            threeCycles = new RadioButton();
            SelectDirectBtn = new Button();
            DirectoryBackGroundWorker = new System.ComponentModel.BackgroundWorker();
            PrgsBar = new ProgressBar();
            Stop = new Button();
            cyclesGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.RoyalBlue;
            label1.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.HighlightText;
            label1.ImageAlign = ContentAlignment.TopCenter;
            label1.Location = new Point(15, 8);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(786, 32);
            label1.TabIndex = 12;
            label1.Text = "File Shredder:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FileBackGroundWorker
            // 
            FileBackGroundWorker.WorkerReportsProgress = true;
            FileBackGroundWorker.DoWork += shredderWorker_DoWork;
            FileBackGroundWorker.ProgressChanged += shredderWorker_ProgressChanged;
            FileBackGroundWorker.RunWorkerCompleted += shredderWorker_RunWorkerCompleted;
            // 
            // RunProgress
            // 
            RunProgress.BackColor = Color.WhiteSmoke;
            RunProgress.Font = new Font("Microsoft Sans Serif", 10F);
            RunProgress.Location = new Point(19, 450);
            RunProgress.Name = "RunProgress";
            RunProgress.Size = new Size(778, 28);
            RunProgress.TabIndex = 14;
            RunProgress.Text = "Progress:";
            RunProgress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // start
            // 
            start.BackColor = Color.AliceBlue;
            start.FlatStyle = FlatStyle.Flat;
            start.Font = new Font("Microsoft Sans Serif", 10F);
            start.ForeColor = SystemColors.InfoText;
            start.Location = new Point(691, 536);
            start.Margin = new Padding(3, 2, 3, 2);
            start.Name = "start";
            start.Size = new Size(106, 40);
            start.TabIndex = 15;
            start.Text = "Start";
            start.UseVisualStyleBackColor = false;
            start.Click += start_Click;
            // 
            // selectFileBtn
            // 
            selectFileBtn.BackColor = Color.AliceBlue;
            selectFileBtn.FlatStyle = FlatStyle.Flat;
            selectFileBtn.Font = new Font("Microsoft Sans Serif", 10F);
            selectFileBtn.ForeColor = SystemColors.InfoText;
            selectFileBtn.Location = new Point(634, 212);
            selectFileBtn.Margin = new Padding(0);
            selectFileBtn.Name = "selectFileBtn";
            selectFileBtn.Size = new Size(163, 40);
            selectFileBtn.TabIndex = 17;
            selectFileBtn.Text = "Select a file";
            selectFileBtn.UseVisualStyleBackColor = false;
            selectFileBtn.Click += browse_Click;
            // 
            // FileName
            // 
            FileName.BackColor = Color.WhiteSmoke;
            FileName.Font = new Font("Microsoft Sans Serif", 10F);
            FileName.Location = new Point(19, 409);
            FileName.Name = "FileName";
            FileName.Size = new Size(778, 28);
            FileName.TabIndex = 19;
            FileName.Text = "File Name:";
            FileName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.BackColor = Color.WhiteSmoke;
            label2.Font = new Font("Microsoft Sans Serif", 10F);
            label2.Location = new Point(19, 56);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(778, 140);
            label2.TabIndex = 20;
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // toolTip1
            // 
            toolTip1.IsBalloon = true;
            // 
            // cyclesGroupBox
            // 
            cyclesGroupBox.Controls.Add(singleCycle);
            cyclesGroupBox.Controls.Add(sevenCycles);
            cyclesGroupBox.Controls.Add(fiveCycles);
            cyclesGroupBox.Controls.Add(threeCycles);
            cyclesGroupBox.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cyclesGroupBox.ForeColor = SystemColors.ControlText;
            cyclesGroupBox.Location = new Point(19, 212);
            cyclesGroupBox.Margin = new Padding(3, 2, 3, 2);
            cyclesGroupBox.Name = "cyclesGroupBox";
            cyclesGroupBox.Padding = new Padding(3, 2, 3, 2);
            cyclesGroupBox.Size = new Size(402, 185);
            cyclesGroupBox.TabIndex = 25;
            cyclesGroupBox.TabStop = false;
            cyclesGroupBox.Text = "Encryption Cycles:";
            // 
            // singleCycle
            // 
            singleCycle.AutoSize = true;
            singleCycle.Location = new Point(22, 28);
            singleCycle.Margin = new Padding(0);
            singleCycle.Name = "singleCycle";
            singleCycle.Size = new Size(165, 19);
            singleCycle.TabIndex = 28;
            singleCycle.Text = "Fast: Secue Single Cycle  ";
            singleCycle.UseVisualStyleBackColor = true;
            // 
            // sevenCycles
            // 
            sevenCycles.AutoSize = true;
            sevenCycles.Location = new Point(22, 124);
            sevenCycles.Margin = new Padding(0);
            sevenCycles.Name = "sevenCycles";
            sevenCycles.Size = new Size(166, 19);
            sevenCycles.TabIndex = 27;
            sevenCycles.Text = "Strongest: Secue 7 Cycle  ";
            sevenCycles.UseVisualStyleBackColor = true;
            // 
            // fiveCycles
            // 
            fiveCycles.AutoSize = true;
            fiveCycles.Location = new Point(22, 91);
            fiveCycles.Margin = new Padding(0);
            fiveCycles.Name = "fiveCycles";
            fiveCycles.Size = new Size(150, 19);
            fiveCycles.TabIndex = 26;
            fiveCycles.Text = "Strong: Secue 5 Cycle  ";
            fiveCycles.UseVisualStyleBackColor = true;
            // 
            // threeCycles
            // 
            threeCycles.AutoSize = true;
            threeCycles.Checked = true;
            threeCycles.Location = new Point(22, 59);
            threeCycles.Margin = new Padding(0);
            threeCycles.Name = "threeCycles";
            threeCycles.Size = new Size(139, 19);
            threeCycles.TabIndex = 25;
            threeCycles.TabStop = true;
            threeCycles.Text = "Safe: Secue 3 Cycles";
            threeCycles.UseVisualStyleBackColor = true;
            // 
            // SelectDirectBtn
            // 
            SelectDirectBtn.BackColor = Color.AliceBlue;
            SelectDirectBtn.FlatStyle = FlatStyle.Flat;
            SelectDirectBtn.Font = new Font("Microsoft Sans Serif", 10F);
            SelectDirectBtn.ForeColor = SystemColors.InfoText;
            SelectDirectBtn.Location = new Point(634, 262);
            SelectDirectBtn.Margin = new Padding(0);
            SelectDirectBtn.Name = "SelectDirectBtn";
            SelectDirectBtn.Size = new Size(163, 40);
            SelectDirectBtn.TabIndex = 26;
            SelectDirectBtn.Text = "Select a folder";
            SelectDirectBtn.UseVisualStyleBackColor = false;
            SelectDirectBtn.Click += SelectDirectBtn_Click;
            // 
            // DirectoryBackGroundWorker
            // 
            DirectoryBackGroundWorker.WorkerReportsProgress = true;
            DirectoryBackGroundWorker.DoWork += DirectoryBackGroundWorker_DoWork;
            DirectoryBackGroundWorker.ProgressChanged += DirectoryBackGroundWorker_ProgressChanged;
            DirectoryBackGroundWorker.RunWorkerCompleted += DirectoryBackGroundWorker_RunWorkerCompleted;
            // 
            // PrgsBar
            // 
            PrgsBar.Location = new Point(19, 491);
            PrgsBar.Name = "PrgsBar";
            PrgsBar.Size = new Size(778, 28);
            PrgsBar.TabIndex = 27;
            // 
            // Stop
            // 
            Stop.BackColor = Color.AliceBlue;
            Stop.FlatStyle = FlatStyle.Flat;
            Stop.Font = new Font("Microsoft Sans Serif", 10F);
            Stop.ForeColor = SystemColors.InfoText;
            Stop.Location = new Point(19, 536);
            Stop.Margin = new Padding(3, 2, 3, 2);
            Stop.Name = "Stop";
            Stop.Size = new Size(106, 40);
            Stop.TabIndex = 28;
            Stop.Text = "Stop";
            Stop.UseVisualStyleBackColor = false;
            Stop.Click += Stop_Click;
            // 
            // FileShredder
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(809, 587);
            Controls.Add(Stop);
            Controls.Add(PrgsBar);
            Controls.Add(SelectDirectBtn);
            Controls.Add(cyclesGroupBox);
            Controls.Add(label2);
            Controls.Add(FileName);
            Controls.Add(selectFileBtn);
            Controls.Add(start);
            Controls.Add(RunProgress);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FileShredder";
            Load += FileShredder_Load;
            cyclesGroupBox.ResumeLayout(false);
            cyclesGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker FileBackGroundWorker;
        private System.Windows.Forms.Label RunProgress;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button selectFileBtn;
        private System.Windows.Forms.Label FileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox cyclesGroupBox;
        private System.Windows.Forms.RadioButton sevenCycles;
        private System.Windows.Forms.RadioButton fiveCycles;
        private System.Windows.Forms.RadioButton threeCycles;
        private System.Windows.Forms.RadioButton singleCycle;
        private System.Windows.Forms.Button SelectDirectBtn;
        private System.ComponentModel.BackgroundWorker DirectoryBackGroundWorker;
        private System.Windows.Forms.ProgressBar PrgsBar;
        private System.Windows.Forms.Button Stop;
    }
}