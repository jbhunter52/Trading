using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trading;
using System.Threading.Tasks;
using NodaTime;

namespace MarketSim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<Company> List = new List<Company>();
        public Simulation Sim = new Simulation();

        private void buttonOpenDb_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = @"C:\Users\Jared\AppData\Local\TradeData";
            ofd.Filter = "Database files (*.db) | *.db";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxDbFile.Text = ofd.FileName;
            }


        }

        private async void buttonFilter_Click(object sender, EventArgs e)
        {
            if (textBoxDbFile.Text.Equals(""))
                MessageBox.Show("Select a database file");
            else
            {
                Sim = new Simulation();
                Sim.SetDefault();
                Sim.Dbfile = textBoxDbFile.Text;
                progressBar1.Style = ProgressBarStyle.Marquee;
                progressBar1.MarqueeAnimationSpeed = 30;

                var result = await Task.Run(() =>
                {
                    return Sim.GetTradeList();
                });
                List = result;
                labelStockCount.Text = "Total Stocks: " + List.Count.ToString();
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.MarqueeAnimationSpeed = 0;
            }
        }

        private async void buttonRun_Click(object sender, EventArgs e)
        {
            if (List.Count == 0)
                MessageBox.Show("First filter stocks");
            else
            {
                //Simulation.OnProgressUpdate += Simulation_OnProgressUpdate;
                var progress = new Progress<Result>(r =>
                {
                    labelCurrentDate.Text = "Current date: " + r.Date.Month.ToString() + "/" + r.Date.Day.ToString() + "/" + r.Date.Year.ToString();
                    progressBar1.Value = (int)(r.PercComp * 100);

                    labelCurrentValue.Text = "Current value: " + ((int)r.Portfolio.GetTotalValue()).ToString();
                    //dataGridView1.DataSource = r.Dt;
                    textBoxStatus.Text = r.Status;

                });
                Sim = new Simulation();
                Sim.SetDefault();
                Sim.Dbfile = textBoxDbFile.Text;
                progressBar1.Value = 0;

                await Task.Run(() =>
                {
                    Sim.Run(List, progress);
                });

                progressBar1.Value = 0;
            }

        }

        void Simulation_OnProgressUpdate(object sender, ProgressUpdateArgs arg)
        {
            Result r = arg.Result;
            labelCurrentDate.Text = "Current date: " + r.Date.Month.ToString() + "//" + r.Date.Day.ToString() + "//" + r.Date.Year.ToString();
            progressBar1.Value = (int)(r.PercComp * 100);
            
        }

    }
}
