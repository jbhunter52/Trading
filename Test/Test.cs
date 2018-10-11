using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Trading;
using LiteDB;
using System.Diagnostics;
using NodaTime;

namespace Test
{
    class Test
    {
        static void Main(string[] args)
        {
            string dbfile = @"C:\Users\Jared\AppData\Local\TradeData\Stocks-10-5-18_update.db";
            //string dbfile = @"C:\Users\Jared\AppData\Local\TradeData\Stocks-10-5-18_update.db";
            //GetNewDb(dbfile, true, 200);

            //GetYChartsDb(dbfile);

            //Trading.Database db = new Trading.Database(dbfile, false);
            //List<Company> list = new List<Company>();

            //var col = db.DB.GetCollection<Trading.Company>("data");
            //list = col.FindAll().ToList();
            ////list = col.Find(Query.Where("EarningsData", earnings => earnings.AsArray.Count > 0)).ToList();
            //foreach (Company c in list)
            //{
            //    c.SetHighToDateSeries();
            //    col.Update(c.Id, c);
            //}

            //Correct gap in dates
            Trading.Database db = new Trading.Database(dbfile, false);
            List<Company> list = new List<Company>();
            var col = db.DB.GetCollection<Trading.Company>("data");
            list = col.FindAll().ToList();
            foreach (Company c in list)
            {
                LocalDate prev = c.date[0];
                for (int i = 1; i < c.Count; i++)
                {
                    LocalDate thisDay = c.date[i];
                    int days = Period.Between(prev, thisDay).Days;
                    if (days > 14)
                    {
                        Console.WriteLine(c.Symbol, days.ToString());
                        col.Delete(c.Id);
                        break;
                    }
                    prev = thisDay;
                }
            }
            db.DB.Dispose();

            //Simulation sim = new Simulation();
            //Console.WriteLine("Initializing...");
            //sim.Dbfile = dbfile;
            //sim.SetDefault();
            //List<Company> list = sim.GetTradeList();

            //sim.Run(list);
            //Console.WriteLine(sim.Portfolio.GetTotalValue().ToString());

            //Optimization ga = new Optimization(dbfile);
            //ga.Optimize();

            //s.SetDefault();
            //s.Dbfile = dbfile;
            //List<Company> list = s.GetTradeList();

            //for (int i = 0; i < 10; i++)
            //{
            //    s.Run(list);
            //    Console.WriteLine(s.Values[s.Values.Count - 1].ToString());
            //}
            Console.ReadKey();
        }

        static void GetNewDb(string dbfile, bool newdb, int num = -1)
        {
            Trading.Database db = new Trading.Database(dbfile, newdb);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            db.GetAllData2(Trading.IEXData.HistoryType.FiveYear, num);
            Trading.Debug.Nlog("Get all took " + ((float)sw.ElapsedMilliseconds / 1000 /60).ToString() + " minutes");
            db.DB.Dispose();
            
        }

        static void GetYChartsDb(string dbfile)
        {
            Trading.Database db = new Trading.Database(dbfile, false);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var col = db.DB.GetCollection<Trading.Company>("data");
            List<Company> cList = col.FindAll().ToList();
            db.GetYChartsEpsData(cList);
            Trading.Debug.Nlog("Get all Ycharts took " + ((float)sw.ElapsedMilliseconds / 1000 / 60).ToString() + " minutes");
            db.DB.Dispose();
        }
    }


}
