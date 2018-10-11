namespace MarketSim
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxMinSingleRank = new System.Windows.Forms.TextBox();
            this.textBoxStopLoss = new System.Windows.Forms.TextBox();
            this.textBoxStopGain = new System.Windows.Forms.TextBox();
            this.textBoxTotalRank = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCurrentValue = new System.Windows.Forms.Label();
            this.labelStockCount = new System.Windows.Forms.Label();
            this.labelCurrentDate = new System.Windows.Forms.Label();
            this.buttonRun = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBoxDbFile = new System.Windows.Forms.TextBox();
            this.buttonOpenDb = new System.Windows.Forms.Button();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.textBoxMinEpsGrowth = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBoxMinEpsGrowth);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxMinSingleRank);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxStopLoss);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxStopGain);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxTotalRank);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.labelCurrentValue);
            this.splitContainer1.Panel1.Controls.Add(this.labelStockCount);
            this.splitContainer1.Panel1.Controls.Add(this.labelCurrentDate);
            this.splitContainer1.Panel1.Controls.Add(this.buttonRun);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxDbFile);
            this.splitContainer1.Panel1.Controls.Add(this.buttonOpenDb);
            this.splitContainer1.Panel1.Controls.Add(this.buttonFilter);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxStatus);
            this.splitContainer1.Size = new System.Drawing.Size(698, 622);
            this.splitContainer1.SplitterDistance = 152;
            this.splitContainer1.TabIndex = 0;
            // 
            // textBoxMinSingleRank
            // 
            this.textBoxMinSingleRank.Location = new System.Drawing.Point(302, 84);
            this.textBoxMinSingleRank.Name = "textBoxMinSingleRank";
            this.textBoxMinSingleRank.Size = new System.Drawing.Size(53, 20);
            this.textBoxMinSingleRank.TabIndex = 16;
            // 
            // textBoxStopLoss
            // 
            this.textBoxStopLoss.Location = new System.Drawing.Point(430, 84);
            this.textBoxStopLoss.Name = "textBoxStopLoss";
            this.textBoxStopLoss.Size = new System.Drawing.Size(53, 20);
            this.textBoxStopLoss.TabIndex = 15;
            // 
            // textBoxStopGain
            // 
            this.textBoxStopGain.Location = new System.Drawing.Point(430, 60);
            this.textBoxStopGain.Name = "textBoxStopGain";
            this.textBoxStopGain.Size = new System.Drawing.Size(53, 20);
            this.textBoxStopGain.TabIndex = 14;
            // 
            // textBoxTotalRank
            // 
            this.textBoxTotalRank.Location = new System.Drawing.Point(302, 58);
            this.textBoxTotalRank.Name = "textBoxTotalRank";
            this.textBoxTotalRank.Size = new System.Drawing.Size(53, 20);
            this.textBoxTotalRank.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Min Eps Growth";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(370, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Stop Loss";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Stop Gain";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Min Single Rank";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Min Total Rank";
            // 
            // labelCurrentValue
            // 
            this.labelCurrentValue.AutoSize = true;
            this.labelCurrentValue.Location = new System.Drawing.Point(534, 62);
            this.labelCurrentValue.Name = "labelCurrentValue";
            this.labelCurrentValue.Size = new System.Drawing.Size(74, 13);
            this.labelCurrentValue.TabIndex = 7;
            this.labelCurrentValue.Text = "Current Value:";
            // 
            // labelStockCount
            // 
            this.labelStockCount.AutoSize = true;
            this.labelStockCount.Location = new System.Drawing.Point(93, 43);
            this.labelStockCount.Name = "labelStockCount";
            this.labelStockCount.Size = new System.Drawing.Size(73, 13);
            this.labelStockCount.TabIndex = 6;
            this.labelStockCount.Text = "Total Stocks: ";
            // 
            // labelCurrentDate
            // 
            this.labelCurrentDate.AutoSize = true;
            this.labelCurrentDate.Location = new System.Drawing.Point(534, 36);
            this.labelCurrentDate.Name = "labelCurrentDate";
            this.labelCurrentDate.Size = new System.Drawing.Size(70, 13);
            this.labelCurrentDate.TabIndex = 5;
            this.labelCurrentDate.Text = "Current Date:";
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(12, 67);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 4;
            this.buttonRun.Text = "Run Simulation";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(484, 9);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(202, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // textBoxDbFile
            // 
            this.textBoxDbFile.Location = new System.Drawing.Point(12, 12);
            this.textBoxDbFile.Name = "textBoxDbFile";
            this.textBoxDbFile.Size = new System.Drawing.Size(370, 20);
            this.textBoxDbFile.TabIndex = 2;
            // 
            // buttonOpenDb
            // 
            this.buttonOpenDb.Location = new System.Drawing.Point(388, 10);
            this.buttonOpenDb.Name = "buttonOpenDb";
            this.buttonOpenDb.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenDb.TabIndex = 1;
            this.buttonOpenDb.Text = "Open Db";
            this.buttonOpenDb.UseVisualStyleBackColor = true;
            this.buttonOpenDb.Click += new System.EventHandler(this.buttonOpenDb_Click);
            // 
            // buttonFilter
            // 
            this.buttonFilter.Location = new System.Drawing.Point(12, 38);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonFilter.TabIndex = 0;
            this.buttonFilter.Text = "Filter";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.Size = new System.Drawing.Size(698, 466);
            this.textBoxStatus.TabIndex = 0;
            // 
            // textBoxMinEpsGrowth
            // 
            this.textBoxMinEpsGrowth.Location = new System.Drawing.Point(302, 110);
            this.textBoxMinEpsGrowth.Name = "textBoxMinEpsGrowth";
            this.textBoxMinEpsGrowth.Size = new System.Drawing.Size(53, 20);
            this.textBoxMinEpsGrowth.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 622);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxDbFile;
        private System.Windows.Forms.Button buttonOpenDb;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Label labelCurrentDate;
        private System.Windows.Forms.Label labelStockCount;
        private System.Windows.Forms.Label labelCurrentValue;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMinSingleRank;
        private System.Windows.Forms.TextBox textBoxStopLoss;
        private System.Windows.Forms.TextBox textBoxStopGain;
        private System.Windows.Forms.TextBox textBoxTotalRank;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxMinEpsGrowth;
    }
}

