using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Trading;
using LiteDB;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Trading.Database db = new Trading.Database(@"C:\Users\Jared\AppData\Local\TradeData\Stocks2RBC_9-17-18.db", false);
            List<Trading.SymbolData> data = Trading.IEXData.DownloadSymbolList();

            StreamWriter streamWriter = new StreamWriter(@"C:\users\jared\appdata\local\TradeData\data.txt", false);
            foreach (var sym in data)
            {
                streamWriter.WriteLine(sym.symbol + "," + sym.name + "," + sym.isEnabled);
            }

            db.GetAllData(Trading.IEXData.HistoryType.FiveYear, 50);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var col = db.DB.GetCollection<Trading.Company>("data");
            List<Company> list = col.FindAll().ToList();
            
            Console.WriteLine("Get all " + ((float)sw.ElapsedMilliseconds/1000).ToString() + " seconds");
            sw.Restart();

            Console.WriteLine("Before filter " + list.Count.ToString() + " stocks");

            //Filter
            list = (from c in list
                        where c.AllPriceAboveValue(2.0f)
                        where c.AllVolumeAboveValue(1000)
                        select c).ToList();

            Console.WriteLine("After filter " + list.Count.ToString() + " stocks");
            Console.WriteLine("Filter " + ((float)sw.ElapsedMilliseconds / 1000).ToString() + " seconds");

            Console.ReadKey();
        }

        
    }
}
