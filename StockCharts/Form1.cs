using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Trading;
using LiteDB;
using System.IO;

namespace StockCharts
{
    public partial class Form1 : Form
    {
        public List<Company> cList;
        public Database db;
        public LiteCollection<CupHandle> colch;
        public Form1()
        {
            InitializeComponent();


            comboBoxDuration.Items.Add("1 year");
            comboBoxDuration.Items.Add("2 year");
            comboBoxDuration.Items.Add("5 year");
            comboBoxDuration.SelectedIndex = 0;

            chartValue.ChartAreas.Clear();
            ChartArea area = new ChartArea("0");
            area.AxisX.LabelStyle.Format = "MM/dd/yyyy";
            area.AxisX.LabelStyle.Angle = -90;
            area.AxisX.IntervalType = DateTimeIntervalType.Days;
            area.AxisX.ScaleView.Zoomable = true;
            area.AxisY.ScaleView.Zoomable = true;
            area.AxisY.LabelStyle.Format = "{0:0.00}";
            chartValue.ChartAreas.Add(area);


            chartVolume.ChartAreas.Clear();
            area = new ChartArea("0");
            area.AxisX.LabelStyle.Format = "MM/dd/yyyy";
            area.AxisX.LabelStyle.Angle = -90;
            area.AxisX.IntervalType = DateTimeIntervalType.Months;
            area.AxisX.ScaleView.Zoomable = true;
            area.AxisY.ScaleView.Zoomable = true;
            chartVolume.ChartAreas.Add(area);

            chartValue.MouseWheel += chart1_MouseWheel;

            string dbfile = @"C:\Users\Jared\AppData\Local\TradeData\Stocks2RBC_9-19-18.db";
            string cupDbFile = Path.Combine(Path.GetDirectoryName(dbfile), Path.GetFileNameWithoutExtension(dbfile) + "_ch.db");

            bool overwrite = false;
            db = new Trading.Database(dbfile, overwrite);
            cList = new List<Company>();
            var col = db.DB.GetCollection<Trading.Company>("data");


            //Get cuphandle collection
            LiteDatabase chdb = new LiteDatabase(cupDbFile);
            colch = chdb.GetCollection<CupHandle>("CupHandle");

            int num = colch.Count();
            for (int i = 1; i < 50; i++)
            {
                Company c = col.FindById(i);
                cList.Add(c);
            }

            foreach (Company c in cList)
            {
                if (c != null)
                {
                    TreeNode cNode = new TreeNode(c.Symbol);

                    int cnt = 0;
                    //List<CupHandle> listch = colch.FindAll().ToList();
                    List<CupHandle> listch = colch.Find(Query.EQ("Symbol", c.Symbol)).ToList();
                    foreach (CupHandle ch in listch)
                    {
                        cnt++;
                        TreeNode n = new TreeNode(cnt.ToString() + ", " + ch.GetRank().ToString());
                        cNode.Nodes.Add(n);
                    }
                    treeView.Nodes.Add(cNode);
                }
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            string[] symbols = textBoxSymbols.Text.Split(',');
            List<Trading.Company> list = new List<Trading.Company>();

            Trading.IEXData.HistoryType ht = Trading.IEXData.HistoryType.OneYear;
            if (comboBoxDuration.SelectedIndex == 0) { ht = Trading.IEXData.HistoryType.OneYear; }
            if (comboBoxDuration.SelectedIndex == 1) { ht = Trading.IEXData.HistoryType.TwoYear; }
            if (comboBoxDuration.SelectedIndex == 2) { ht = Trading.IEXData.HistoryType.FiveYear; }
            foreach (string sym in symbols)
            {

                Trading.Company c = Trading.IEXData.DownloadSymbol(sym, ht);
                list.Add(c);

                Color[] colors = new Color[list.Count];
                Random r = new Random();
                for (int i = 0; i < list.Count; i++ )
                {
                    colors[i] = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                }

                DrawPriceChart(list, colors);
                DrawVolumeChart(list[0]);
            }
        }

        private void DrawChart(Company c)
        {
            List<Company> list = new List<Company>();
            list.Add(c);

            Color[] colors = new Color[list.Count];
            Random r = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                colors[i] = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
            }

            DrawPriceChart(list, colors);
            DrawVolumeChart(list[0]);
        }

        private void DrawPriceChart(Trading.Company c, Color color)
        {
            List<Trading.Company> cList = new List<Company>();
            cList.Add(c);
            Color[] colors = new Color[1];
            colors[0] = color;

            DrawPriceChart(cList, colors);
        }

        private void DrawPriceChart(List<Trading.Company> list, Color[] colors)
        {
            ChartArea area = new ChartArea("0");

            for (int i = 0; i < list.Count; i++)
            {
                Trading.Company c = list[i];
                //Close price
                Series s = new Series(c.Symbol + " Close Price");
                s.ChartType = SeriesChartType.Line;
                s.Color = colors[i];
                s.ChartArea = "0";
                s.XValueType = ChartValueType.DateTime;
                for (int j = 0; j < c.Count; j++ )
                {
                    DateTime date = DateTime.Parse(c.label[j]);
                    float val = c.close[j];
                    s.Points.AddXY(date, val);
                }
                chartValue.Series.Add(s);

                //MovingAverageClose price
                Series maClose = new Series(c.Symbol + " MA Close Price");
                maClose.ChartType = SeriesChartType.Line;
                maClose.BorderDashStyle = ChartDashStyle.Dot;
                maClose.Color = colors[i];
                maClose.ChartArea = "0";
                maClose.XValueType = ChartValueType.DateTime;
                for (int j = 0; j < c.Count; j++)
                {
                    DateTime date = DateTime.Parse(c.label[j]);
                    float val = c.MovingAverageClose[j];
                    maClose.Points.AddXY(date, val);
                }
                chartValue.Series.Add(maClose);
            }

            chartValue.Show();
            chartValue.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chartValue.ChartAreas[0].AxisY.ScaleView.ZoomReset();
        }

        private void DrawVolumeChart(Trading.Company c)
        {
            //Volume
            Series s = new Series(c.Symbol + " Volume");
            s.ChartType = SeriesChartType.Column;
            s.ChartArea = "0";
            s.XValueType = ChartValueType.DateTime;

            for (int i = 0; i < c.Count; i++)
            {
                DateTime date = DateTime.Parse(c.label[i]);
                float val = c.volume[i];
                s.Points.AddXY(date, val);
            }
            s["PixelPointWidth"] = "1";
            chartVolume.Series.Add(s);


            //MovingAverageVolume
            Series maVol = new Series(c.Symbol + " MA Volume");
            maVol.ChartType = SeriesChartType.Line;
            maVol.BorderDashStyle = ChartDashStyle.Solid;
            maVol.ChartArea = "0";
            maVol.XValueType = ChartValueType.DateTime;
            for (int i = 0; i < c.Count; i++)
            {
                DateTime date = DateTime.Parse(c.label[i]);
                float val = c.MovingAverageVolume[i];
                maVol.Points.AddXY(date, val);
            }
            chartVolume.Series.Add(maVol);
            chartVolume.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chartVolume.ChartAreas[0].AxisY.ScaleView.ZoomReset();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            chartValue.Series.Clear();
            chartVolume.Series.Clear();
            string sym;

            if (node.Parent == null) // if a root node
                sym = node.Text;
            else //must be selection of a cup with handle
                sym = node.Parent.Text;

            //Draw value and volume chart
            var col = db.DB.GetCollection<Trading.Company>("data");
            Company c = col.FindOne(x => x.Symbol.Equals(sym));
            DrawChart(c);

            if (node.Parent != null)
            {
                //DrawCupHandles
                if (ModifierKeys.HasFlag(Keys.Control))
                {
                    DrawCupHandles(c, -1);
                }
                else
                {
                    int index = Int16.Parse(node.Text.Split(',')[0]) - 1;
                    DrawCupHandles(c, index);
                }
            }


        }
        private void DrawCupHandles(Company c, int index)
        {
            //if index is < 0 then show all cup handles for the symbol
            List<CupHandle> chs = colch.Find(Query.EQ("Symbol", c.Symbol)).ToList();
            if (index < 0)
            {
                foreach (CupHandle ch in chs)
                {
                    chartValue.Series.Add(GetCupHandleSeries(ch, c));
                }
            }
            else
            {
                chartValue.Series.Add(GetCupHandleSeries(chs[index], c));
            }
        }
        private Series GetCupHandleSeries(CupHandle ch, Company c)
        {
            Random r = new Random();
            Series s = new Series();
            Color randColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));

            s.Name = "CupHandle " + ch.Id.ToString();
            s.ChartType = SeriesChartType.Line;
            s.Color = randColor;
            s.ChartArea = "0";
            s.XValueType = ChartValueType.DateTime;
            s.BorderWidth = 3;
            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 20;
            s.Points.AddXY(DateTime.Parse(c.label[ch.A.Index]), ch.A.Close);
            s.Points.AddXY(DateTime.Parse(c.label[ch.B.Index]), ch.B.Close);
            s.Points.AddXY(DateTime.Parse(c.label[ch.C.Index]), ch.C.Close);
            s.Points.AddXY(DateTime.Parse(c.label[ch.D.Index]), ch.D.Close);
            s.Points.AddXY(DateTime.Parse(c.label[ch.Buy.Index]), ch.Buy.Close);
            return s;
        }
        private void RemoveAllCupHandleSeries()
        {
            List<Series> prevCHSeries = chartValue.Series.Where(x => x.Name.Contains("CupHandle")).ToList();
            foreach (Series s in prevCHSeries)
            {
                chartValue.Series.Remove(s);
            }
        }
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            var xAxisVol = chartVolume.ChartAreas[0].AxisX;
            var yAxisVol = chartVolume.ChartAreas[0].AxisY;

            try
            {
                double mult = 0;
                if (e.Delta < 0) // Scrolled down.
                {
                    //xAxis.ScaleView.ZoomReset();
                    //yAxis.ScaleView.ZoomReset();
                    mult = 1/1.25;
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    mult = 1.25;
                }

                if (Math.Abs(e.Delta) > 0)
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    var yMin = yAxis.ScaleView.ViewMinimum;
                    var yMax = yAxis.ScaleView.ViewMaximum;

                    var xSpan = (xMax - xMin) / mult;
                    var ySpan = (yMax - yMin) / mult;

                    var xPos = xAxis.PixelPositionToValue(e.Location.X);
                    var yPos = yAxis.PixelPositionToValue(e.Location.Y);

                    var u = (xPos - xMin) / (xMax - xMin);
                    var v = (yPos - yMin) / (yMax - yMin);

                    var posXStart = xPos - u * xSpan;
                    var posXFinish = xPos + (1-u) * xSpan;
                    var posYStart = yPos - v * ySpan;
                    var posYFinish = yPos + (1-v) * ySpan;

                    if (ModifierKeys.HasFlag(Keys.Control))
                    {
                        yAxis.ScaleView.Zoom(posYStart, posYFinish);
                    }
                    else if (ModifierKeys.HasFlag(Keys.Shift))
                    {
                        xAxis.ScaleView.Zoom(posXStart, posXFinish);
                        xAxisVol.ScaleView.Zoom(posXStart, posXFinish);
                    }
                    else
                    {
                        xAxis.ScaleView.Zoom(posXStart, posXFinish);
                        yAxis.ScaleView.Zoom(posYStart, posYFinish);
                        xAxisVol.ScaleView.Zoom(posXStart, posXFinish);
                    }
                }
                //chartVolume.ChartAreas[0].AxisY.Minimum = 0;
                //chartVolume.ChartAreas[0].AxisY.Minimum = Double.NaN;
                //chartVolume.ChartAreas[0].RecalculateAxesScale();

            }
            catch { }
        }

        private double prevMouseX = 0;
        private double prevMouseY = 0;

        private void chartValue_MouseMove(object sender, MouseEventArgs e)
        {
            if (rightMouseDown)
            {
                var xAxis = chartValue.ChartAreas[0].AxisX;
                var yAxis = chartValue.ChartAreas[0].AxisY;

                var xAxisVol = chartVolume.ChartAreas[0].AxisX;
                var yAxisVol = chartVolume.ChartAreas[0].AxisY;

                double xPos = xAxis.PixelPositionToValue(e.Location.X);
                double yPos = yAxis.PixelPositionToValue(e.Location.Y);
                double xPosPrev = xAxis.PixelPositionToValue(prevMouseX);
                double yPosPrev = yAxis.PixelPositionToValue(prevMouseY);

                double xDiff = xPos - xPosPrev;
                double yDiff = yPos - yPosPrev;

                xAxis.ScaleView.Zoom(xAxis.ScaleView.ViewMinimum + xDiff, xAxis.ScaleView.ViewMaximum + xDiff);
                yAxis.ScaleView.Zoom(yAxis.ScaleView.ViewMinimum - yDiff, yAxis.ScaleView.ViewMaximum - yDiff);

                xAxisVol.ScaleView.Zoom(xAxis.ScaleView.ViewMinimum + xDiff, xAxis.ScaleView.ViewMaximum + xDiff);

                //chartVolume.ChartAreas[0].AxisY.Minimum = 0;
                //chartVolume.ChartAreas[0].AxisY.Minimum = Double.NaN;
                //chartVolume.ChartAreas[0].RecalculateAxesScale();

                prevMouseX = e.Location.X;
                prevMouseY = e.Location.Y;
            }
        }

        bool rightMouseDown = false;
        private void chartValue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            { 
                rightMouseDown = true;
                prevMouseX = e.Location.X;
                prevMouseY = e.Location.Y;
            }
        }

        private void chartValue_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                rightMouseDown = false;
        }




    }
    
}
