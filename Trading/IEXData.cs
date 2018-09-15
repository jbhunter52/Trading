using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace Trading
{
    public static class IEXData
    {
        public static Company DownloadSymbol(string symbol, HistoryType ht)
        {
            string years = "";
            if (ht == HistoryType.OneYear) { years = "1y"; }
            if (ht == HistoryType.TwoYear) { years = "2y"; }
            if (ht == HistoryType.FiveYear) { years = "5y"; }
            List<HistoricalDataResponse> data = new List<HistoricalDataResponse>();
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/" + years;

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

            return new Company(symbol, data);
        }
        public static List<SymbolData> DownloadSymbolList()
        {

            List<SymbolData> symbols = new List<SymbolData>();
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/ref-data/symbols";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var symbolList = response.Content.ReadAsAsync<List<SymbolData>>().GetAwaiter().GetResult();
                    foreach (var symbolData in symbolList)
                    {
                        if (symbolData != null)
                        {
                            symbols.Add(symbolData);
                        }
                    }
                }
            }

            return symbols;

        }
        public enum HistoryType
        {
            OneYear,
            TwoYear,
            FiveYear
        };
    }
}
