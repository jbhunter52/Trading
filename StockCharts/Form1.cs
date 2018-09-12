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
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            string[] symbols = textBoxSymbols.Text.Split(',');
            List<Trading.Company> list = new List<Trading.Company>();
            foreach (string sym in symbols)
            {
                Trading.Database db = new Trading.Database(@"C:\Users\Jared\AppData\Local\TradeData\test.db");
                Trading.Company c = db.DownloadSymbol(sym, false);
                list.Add(c);

                DrawPriceChart(list);
            }
        }

        private void DrawPriceChart(List<Trading.Company> list)
        {
            chartValue.ChartAreas.Clear();
            chartValue.Series.Clear();
            ChartArea area = new ChartArea("0");
            area.AxisX.LabelStyle.Format = "MM/dd/yyyy";
            area.AxisX.LabelStyle.Angle = -90;
            area.AxisX.IntervalType = DateTimeIntervalType.Months;
            chartValue.ChartAreas.Add(area);

            foreach (Trading.Company c in list)
            {
                Series s = new Series(c.Symbol);
                s.ChartType = SeriesChartType.Line;
                s.ChartArea = "0";
                s.XValueType = ChartValueType.DateTime;

                foreach (Trading.HistoricalDataResponse data in c.Data)
                {
                    DateTime date = DateTime.Parse(data.label);
                    float val = (float)data.close;
                    s.Points.AddXY(date, val);
                }

                chartValue.Series.Add(s);                  
            }

            chartValue.Show();
        }

    }
    
}
