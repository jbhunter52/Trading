using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using NLog;

namespace Trading
{
    public static class IEXData
    {
        public static Company DownloadSymbol(string symbol, HistoryType ht, SymbolData symbolData = null)
        {
            List<HistoricalDataResponse> data = new List<HistoricalDataResponse>();
            

            #region GetEarningsData
            //EarningsData earnings = new EarningsData();
            //using (HttpClient client = new HttpClient())
            //{
            //    try
            //    {
            //        string IEXTrading_API_PATH = GetEarningsDataURL(symbol);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //        //For IP-API
            //        client.BaseAddress = new Uri(IEXTrading_API_PATH);
            //        HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
            //        if (response.IsSuccessStatusCode)
            //        {
            //            earnings = response.Content.ReadAsAsync<EarningsData>().GetAwaiter().GetResult();
            //        }
            //        else { }
            //    }
            //    catch (Exception ex)
            //    {
            //        return new Company();
            //        Debug.Nlog(symbol + "\n" + ex.Message);
            //    }
            //}

            //if (earnings == null)
            //{
            //    return new Company();
            //}
            #endregion

            #region GetHistoricalData
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string IEXTrading_API_PATH = GetHistoricalDataURL(symbol, ht);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    //For IP-API
                    client.BaseAddress = new Uri(IEXTrading_API_PATH);
                    HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var historicalDataList = response.Content.ReadAsAsync<List<HistoricalDataResponse>>().GetAwaiter().GetResult();

                        if (historicalDataList.Count > 0)
                        {
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
                                else { return new Company(); }
                            }
                        }
                        else { return new Company(); }
                    }
                    else { return new Company(); }
                }
                catch (Exception ex)
                {
                    Debug.Nlog(symbol + "\n" + ex.Message);
                }
            }
            #endregion



            return new Company(symbol, data, symbolData);
        }
        //public static List<Company> DownloadSymbols(List<string> symbols, HistoryType ht)
        //{
        //    string years = "";
        //    if (ht == HistoryType.OneYear) { years = "1y"; }
        //    if (ht == HistoryType.TwoYear) { years = "2y"; }
        //    if (ht == HistoryType.FiveYear) { years = "5y"; }
        //    var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/{1}";
        //    List<string> requestsBatch = new List<string>();
        //    foreach (string symbol in symbols)
        //    {
        //        string request = string.Format(IEXTrading_API_PATH, symbol, years);
        //        requestsBatch.Add(request);
        //    }

        //    var httpClient = new HttpClient {Timeout = TimeSpan.FromMilliseconds(5)};
        //    var taskList = new List<Task<Company>>();
        //    for (int i = 0; i < requestsBatch.Count; i++ )
        //    {
        //        string myRequest = requestsBatch[i];
        //        string sym = symbols[i];
        //        // by virtue of not awaiting each call, you've already acheived parallelism
        //        taskList.Add(GetResponseAsync(sym, myRequest));
        //    }

        //    try
        //    {
        //        // asynchronously wait until all tasks are complete
        //        await Task.WhenAll(taskList.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        //public async Task<Company> GetResponseAsync(string symbol, string myRequest)
        //{
        //    HttpClient httpClient = new HttpClient();
        //    // no Task.Run here!
        //    var response = await httpClient.GetAsync(myRequest);
        //    List<HistoricalDataResponse> historicalDataList = await response.Content.ReadAsAsync<List<HistoricalDataResponse>>();

        //    if (historicalDataList.Count > 0)
        //        return new Company()
        //}

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
                            if (symbolData.type.Equals("cs"))
                            {
                                symbols.Add(symbolData);
                            }
                        }
                    }
                }
            }

            return symbols;

        }

        //public static void ProcessSymbolResults(Database db, SymbolData sym, List<HistoricalDataResponse> data)
        //{
        //    Company c = new Company(sym.symbol, data);
        //    db.AddSymbol(c);
        //}
        private static string GetHistoricalDataURL(string symbol, HistoryType ht)
        {
            string years = "";
            if (ht == HistoryType.OneYear) { years = "1y"; }
            if (ht == HistoryType.TwoYear) { years = "2y"; }
            if (ht == HistoryType.FiveYear) { years = "5y"; }
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/{1}";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol, years);
            return IEXTrading_API_PATH;
        }
        private static string GetEarningsDataURL(string symbol)
        {
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/earnings";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol);
            return IEXTrading_API_PATH;
        }

        public enum HistoryType
        {
            OneYear,
            TwoYear,
            FiveYear
        };
    }
}
