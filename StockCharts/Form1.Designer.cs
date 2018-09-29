namespace StockCharts
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.comboBoxDuration = new System.Windows.Forms.ComboBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.textBoxSymbols = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chartValue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartVolume = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxDuration);
            this.splitContainer1.Panel1.Controls.Add(this.buttonGo);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxSymbols);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(729, 726);
            this.splitContainer1.SplitterDistance = 127;
            this.splitContainer1.TabIndex = 0;
            // 
            // comboBoxDuration
            // 
            this.comboBoxDuration.FormattingEnabled = true;
            this.comboBoxDuration.Location = new System.Drawing.Point(452, 13);
            this.comboBoxDuration.Name = "comboBoxDuration";
            this.comboBoxDuration.Size = new System.Drawing.Size(106, 21);
            this.comboBoxDuration.TabIndex = 4;
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(363, 30);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(39, 23);
            this.buttonGo.TabIndex = 3;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // textBoxSymbols
            // 
            this.textBoxSymbols.Location = new System.Drawing.Point(59, 34);
            this.textBoxSymbols.Name = "textBoxSymbols";
            this.textBoxSymbols.Size = new System.Drawing.Size(297, 20);
            this.textBoxSymbols.TabIndex = 0;
            this.textBoxSymbols.Text = "aapl";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chartValue);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chartVolume);
            this.splitContainer2.Size = new System.Drawing.Size(729, 595);
            this.splitContainer2.SplitterDistance = 425;
            this.splitContainer2.TabIndex = 0;
            // 
            // chartValue
            // 
            this.chartValue.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.chartValue.Legends.Add(legend1);
            this.chartValue.Location = new System.Drawing.Point(0, 0);
            this.chartValue.Name = "chartValue";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartValue.Series.Add(series1);
            this.chartValue.Size = new System.Drawing.Size(729, 425);
            this.chartValue.TabIndex = 0;
            this.chartValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartValue_MouseDown);
            this.chartValue.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartValue_MouseMove);
            this.chartValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chartValue_MouseUp);
            // 
            // chartVolume
            // 
            this.chartVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.chartVolume.Legends.Add(legend2);
            this.chartVolume.Location = new System.Drawing.Point(0, 0);
            this.chartVolume.Name = "chartVolume";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartVolume.Series.Add(series2);
            this.chartVolume.Size = new System.Drawing.Size(729, 166);
            this.chartVolume.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer3.Size = new System.Drawing.Size(894, 726);
            this.splitContainer3.SplitterDistance = 161;
            this.splitContainer3.TabIndex = 1;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(161, 726);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 726);
            this.Controls.Add(this.splitContainer3);
            this.Name = "Form1";
            this.Text = "StockCharts";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartVolume)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TextBox textBoxSymbols;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartValue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartVolume;
        private System.Windows.Forms.ComboBox comboBoxDuration;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TreeView treeView;
    }
}

