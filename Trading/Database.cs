using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LiteDB;
using System.IO;


namespace Trading
{
    public class Database
    {
        private int current = 0;
        public System.Diagnostics.Stopwatch sw;
        public LiteDatabase DB;
        public Database(string filename, bool newdb)
        {
            if (newdb)
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
            DB = new LiteDatabase(@filename);

            DB.Mapper.RegisterType<NodaTime.LocalDate>(value => new BsonValue(LocalDateToInt(value)), bson => LocalDateFromString(bson));
            //DB.Mapper.RegisterType<Point>
            //(
            //    serialize: (Point) => Point.Serialize(),
            //    deserialize: (bson) => new Point(bson.AsString)
            //);
            //DB.Mapper.RegisterType<CupHandle>
            //(
            //    serialize: (CupHandle) => CupHandle.Serialize(),
            //    deserialize: (bson) => new CupHandle(bson.AsBinary)
            //);
            //DB.Mapper.RegisterType<CupHandleParameters>
            //(
            //    serialize: (CupHandleParameters) => CupHandleParameters.Serialize(),
            //    deserialize: (bson) => new CupHandleParameters(bson.AsString)
            //);
        }
        public void AddSymbol(Company c)
        {
            var col = DB.GetCollection<Trading.Company>("data");
            col.Insert(c);
        }
        public Company GetSymbolFromDB(string symbol)
        {
            var col = DB.GetCollection<Trading.Company>("data");

            var c = col.FindOne(x => x.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase));
            return (Company)c;
        }

        public void GetAllData(IEXData.HistoryType ht, int num)
        {
           List<Trading.SymbolData> symbols = Trading.IEXData.DownloadSymbolList();

           int cnt = symbols.Count;
            if (num > 0)
                cnt = num;

            current = 0;
            for (int i = 0; i < cnt; i++)
            {
                Trading.SymbolData symData = symbols[i];
                Debug.Nlog((i+1).ToString() + "/" + symbols.Count.ToString() + "\t" + symData.symbol  );
                Trading.Company c = Trading.IEXData.DownloadSymbol(symData.symbol, ht);
                if (c.Id != -1)
                    this.AddSymbol(c);
            }
        }

        public void GetAllData2(IEXData.HistoryType ht, int num)
        {
            List<Trading.SymbolData> symbols = Trading.IEXData.DownloadSymbolList();

            int cnt = symbols.Count;
            if (num > 0)
                cnt = num;
            List<Task> tasks = new List<Task>();
            int total = symbols.Count;
            foreach (var sym in symbols)
            {
                var t = new Task(() => ProcessSymbol(total, sym, ht));
                tasks.Add(t);
                if (tasks.Count == cnt)
                    break;
            }
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            StartAndWaitAllThrottled(tasks,6, 20000000);
        }

        public void GetYChartsEpsData(List<Company> cList)
        {
            List<Task> tasks = new List<Task>();
            current = 0;
            System.Net.IPAddress[] ips = System.Net.Dns.GetHostEntry("proxy-nl.privateinternetaccess.com").AddressList;

            List<com.LandonKey.SocksWebProxy.SocksWebProxy> proxies = new List<com.LandonKey.SocksWebProxy.SocksWebProxy>(ips.Length);
            foreach (System.Net.IPAddress ip in ips)
            {
                com.LandonKey.SocksWebProxy.Proxy.ProxyConfig pc = new com.LandonKey.SocksWebProxy.Proxy.ProxyConfig(System.Net.IPAddress.Parse("127.0.0.1"), 1080, ip, 1080, com.LandonKey.SocksWebProxy.Proxy.ProxyConfig.SocksVersion.Five, "x7212591", "c4gkjs4rSg");
                var proxy = new com.LandonKey.SocksWebProxy.SocksWebProxy(pc);
                proxies.Add(proxy);
            }

            int proxyCnt = 0;
            for (int i = 0; i < cList.Count; i++ )
            {
                Company c = cList[i];
                var t = new Task(() => ProcessSymbolYChartEps(cList.Count, c, proxies[proxyCnt]));
                tasks.Add(t);

                proxyCnt++;
                if (proxyCnt >= proxies.Count)
                    proxyCnt = 0;
            }
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            StartAndWaitAllThrottled(tasks, proxies.Count, 20000000);
        }

        public static void StartAndWaitAllThrottled(IEnumerable<Task> tasksToRun, int maxActionsToRunInParallel, int timeoutInMilliseconds, CancellationToken cancellationToken = new CancellationToken())
        {
            // Convert to a list of tasks so that we don't enumerate over it multiple times needlessly.
            var tasks = tasksToRun.ToList();

            using (var throttler = new SemaphoreSlim(maxActionsToRunInParallel))
            {
                var postTaskTasks = new List<Task>();

                // Have each task notify the throttler when it completes so that it decrements the number of tasks currently running.
                tasks.ForEach(t => postTaskTasks.Add(t.ContinueWith(tsk => throttler.Release())));

                // Start running each task.
                foreach (var task in tasks)
                {
                    // Increment the number of tasks currently running and wait if too many are running.
                    throttler.Wait(timeoutInMilliseconds, cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();
                    task.Start();
                }

                // Wait for all of the provided tasks to complete.
                // We wait on the list of "post" tasks instead of the original tasks, otherwise there is a potential race condition where the throttler&#39;s using block is exited before some Tasks have had their "post" action completed, which references the throttler, resulting in an exception due to accessing a disposed object.
                Task.WaitAll(postTaskTasks.ToArray(), cancellationToken);
            }
        }
        public void ProcessSymbolYChartEps(int total, Company c, com.LandonKey.SocksWebProxy.SocksWebProxy proxy)
        {
            Random r = new Random();
            System.Threading.Thread.Sleep(500 + r.Next(1000));  //Sleep between 500-1500 ms
            EpsData data = YChartsData.GetEpsData(c.Symbol, proxy);
            var col = DB.GetCollection<Trading.Company>("data");

            if (data.Quarters.Count > 0)
            {
                c.EarningsQuarters = data.Quarters;
                c.EarningsData = data.Eps;
                col.Update(c);
            }
            else
            {
                col.Delete(c.Id);
            }

            current++;
            int left = total - current;
            double avgSec = sw.Elapsed.TotalSeconds / current;
            TimeSpan ts = new TimeSpan(0, 0, (int)(avgSec * left));
            Debug.Nlog("YCharts Eps, " + current.ToString() + "/" + total.ToString() + "\t" + c.Symbol + ", " + ts.ToString() + " left");
        }
        public void ProcessSymbol(int total, SymbolData symData, IEXData.HistoryType ht)
        {
            Trading.Company c = Trading.IEXData.DownloadSymbol(symData.symbol, ht, symData);
            if (c.Id != -1)
                this.AddSymbol(c);

            current++;
            int left = total - current;
            double avgSec = sw.Elapsed.TotalSeconds / current;
            TimeSpan ts = new TimeSpan(0, 0, (int)(avgSec * left));
            Debug.Nlog(current.ToString() + "/" + total.ToString() + "\t" + symData.symbol + ", " + ts.ToString() + " left");
            
        }

        public string LocalDateToInt(NodaTime.LocalDate ld)
        {
            string year = ld.Year.ToString("D4");
            string month = ld.Month.ToString("D2");
            string day = ld.Day.ToString("D2");
            return year + month + day;
        }
        public NodaTime.LocalDate LocalDateFromString(string s)
        {
            //string s = val.ToString();
            int year = Int16.Parse(s.Substring(0,4));
            int month = Int16.Parse(s.Substring(4,2));
            int day = Int16.Parse(s.Substring(6,2));
            return new NodaTime.LocalDate(year, month, day);
        }

    }
}
