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
            this.labelCurrentValue = new System.Windows.Forms.Label();
            this.labelStockCount = new System.Windows.Forms.Label();
            this.labelCurrentDate = new System.Windows.Forms.Label();
            this.buttonRun = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBoxDbFile = new System.Windows.Forms.TextBox();
            this.buttonOpenDb = new System.Windows.Forms.Button();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
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
            this.splitContainer1.Size = new System.Drawing.Size(698, 522);
            this.splitContainer1.SplitterDistance = 110;
            this.splitContainer1.TabIndex = 0;
            // 
            // labelCurrentValue
            // 
            this.labelCurrentValue.AutoSize = true;
            this.labelCurrentValue.Location = new System.Drawing.Point(490, 64);
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
            this.labelCurrentDate.Location = new System.Drawing.Point(490, 38);
            this.labelCurrentDate.Name = "labelCurrentDate";
            this.labelCurrentDate.Size = new System.Drawing.Size(70, 13);
            this.labelCurrentDate.TabIndex = 5;
            this.labelCurrentDate.Text = "Current Date:";
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(306, 64);
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
            this.textBoxStatus.Size = new System.Drawing.Size(698, 408);
            this.textBoxStatus.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 522);
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
    }
}

