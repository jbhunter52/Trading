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

namespace StockCharts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            comboBoxDuration.Items.Add("1 year");
            comboBoxDuration.Items.Add("2 year");
            comboBoxDuration.Items.Add("5 year");
            comboBoxDuration.SelectedIndex = 0;
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

        private void DrawPriceChart(List<Trading.Company> list, Color[] colors)
        {
            chartValue.ChartAreas.Clear();
            chartValue.Series.Clear();
            ChartArea area = new ChartArea("0");
            area.AxisX.LabelStyle.Format = "MM/dd/yyyy";
            area.AxisX.LabelStyle.Angle = -90;
            area.AxisX.IntervalType = DateTimeIntervalType.Months;
            chartValue.ChartAreas.Add(area);

            for (int i = 0; i < list.Count; i++)
            {
                Trading.Company c = list[i];
                //Close price
                Series s = new Series(c.Symbol + " Close Price");
                s.ChartType = SeriesChartType.Line;
                s.Color = colors[i];
                s.ChartArea = "0";
                s.XValueType = ChartValueType.DateTime;
                foreach (Trading.HistoricalDataResponse data in c.Data)
                {
                    DateTime date = DateTime.Parse(data.label);
                    float val = (float)data.close;
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
                foreach (Trading.HistoricalDataResponse data in c.Data)
                {
                    DateTime date = DateTime.Parse(data.label);
                    float val = (float)data.MovingAverageClose;
                    maClose.Points.AddXY(date, val);
                }
                chartValue.Series.Add(maClose);
            }

            chartValue.Show();
        }

        private void DrawVolumeChart(Trading.Company c)
        {
            
            chartVolume.ChartAreas.Clear();
            chartVolume.Series.Clear();
            ChartArea area = new ChartArea("0");
            area.AxisX.LabelStyle.Format = "MM/dd/yyyy";
            area.AxisX.LabelStyle.Angle = -90;
            area.AxisX.IntervalType = DateTimeIntervalType.Months;
            chartVolume.ChartAreas.Add(area);

            //Volume
            Series s = new Series(c.Symbol + " Volume");
            s.ChartType = SeriesChartType.Column;
            s.ChartArea = "0";
            s.XValueType = ChartValueType.DateTime;
            
            foreach (Trading.HistoricalDataResponse data in c.Data)
            {
                DateTime date = DateTime.Parse(data.label);
                float val = (float)data.volume;
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
            foreach (Trading.HistoricalDataResponse data in c.Data)
            {
                DateTime date = DateTime.Parse(data.label);
                float val = (float)data.MovingAverageVolume;
                maVol.Points.AddXY(date, val);
            }
            chartVolume.Series.Add(maVol);

            chartVolume.Show();
        }



    }
    
}
