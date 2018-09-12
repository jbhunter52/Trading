using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.Net.Http;
using System.IO;

namespace Trading
{
    public class Database
    {
        public LiteDatabase DB;
        public Database(string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            DB = new LiteDatabase(@filename);

        }
        public Company DownloadSymbol(string symbol, bool addtoDatabase)
        {
            List<HistoricalDataResponse> data = new List<HistoricalDataResponse>();
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/1y";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var historicalDataList = response.Content.ReadAsAsync<List<HistoricalDataResponse>>().GetAwaiter().GetResult();
                    foreach (var historicalData in historicalDataList)
                    {
                        if (historicalData != null)
                        {
                            //Console.WriteLine("Open: " + historicalData.open);
                            //Console.WriteLine("Close: " + historicalData.close);
                            //Console.WriteLine("Low: " + historicalData.low);
                            //Console.WriteLine("High: " + historicalData.high);
                            //Console.WriteLine("Change: " + historicalData.change);
                            //Console.WriteLine("Change Percentage: " + historicalData.changePercent);
                            data.Add(historicalData);
                        }
                    }                    
                }
            }

            Company c = new Company(symbol, data);

            if (addtoDatabase)
            {
                var col = DB.GetCollection<Company>("data");

                col.EnsureIndex(x => x.Symbol, true);
                col.Insert(c);
                col.Update(c);
            }
            return c;
        }
        public Company GetSymbol(string symbol)
        {
            var col = DB.GetCollection<Trading.Company>("data");

            var c = col.FindOne(x => x.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase));
            return (Company)c;
        }
    }
}
