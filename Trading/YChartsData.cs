using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Core;
using ScrapySharp.Extensions;
using NodaTime;
using System.Net;
using System.Net.Sockets;
using com.LandonKey.SocksWebProxy;
using com.LandonKey.SocksWebProxy.Proxy;

namespace Trading
{
    public static class YChartsData
    {
        public static EpsData GetEpsData(string symbol, SocksWebProxy proxy)
        {
            //ProxyConfig pc = new ProxyConfig(IPAddress.Parse("127.0.0.1"), 1080, ip, 1080, ProxyConfig.SocksVersion.Five, "x7212591", "c4gkjs4rSg");

            //var proxy = new SocksWebProxy(pc);
            WebClient client = new WebClient();
            client.Headers.Add("Cache-Control", "no-cache");
            client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
            client.Proxy = proxy;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            string html = "";
            int tryCount = 0;
            while (html.Equals("") && tryCount < 6)
            {
                try 
                {
                    html = client.DownloadString("https://ycharts.com/companies/" + symbol + "/eps");
                }
                catch (Exception ex)
                {
                    Trading.Debug.Nlog(symbol + "\n\t" + ex.Message + " \n\t" + ex.InnerException.Message);
                    tryCount++;
                    System.Threading.Thread.Sleep(500);
                    continue;
                }
                break;
            }

            if (html.Equals(""))
                return new EpsData();


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var page = doc.DocumentNode.SelectSingleNode("//body");
            List<HtmlNode> dateNodes = doc.DocumentNode.CssSelect("td.col1").ToList();
            List<HtmlNode> epsNodes = doc.DocumentNode.CssSelect("td.col2").ToList();


            List<string> dateStrings = dateNodes.Select(x => x.InnerHtml).ToList();
            List<string> epsStrings = epsNodes.Select(x => x.InnerHtml.Replace(" ", "").Replace("\n", "")).ToList();

            EpsData epsData = new EpsData(dateStrings, epsStrings);
            return epsData;

        }
    }

    public class EpsData
    {
        public List<LocalDate> Quarters;
        public List<float> Eps;

        public EpsData()
        {
            Quarters = new List<LocalDate>();
            Eps = new List<float>();
        }
        public EpsData(List<string> dates, List<string> values)
        {
            Quarters = new List<LocalDate>();
            Eps = new List<float>();

            for (int i = 0; i < dates.Count; i++)
            {
                string[] v = dates[i].Replace(",", "").Split(' ');

                if (v.Length == 3)
                {
                    int month = -1;
                    if (v[0].StartsWith("Jan"))
                        month = 1;
                    else if (v[0].StartsWith("Feb"))
                        month = 2;
                    else if (v[0].StartsWith("Mar"))
                        month = 3;
                    else if (v[0].StartsWith("Apr"))
                        month = 4;
                    else if (v[0].StartsWith("May"))
                        month = 5;
                    else if (v[0].StartsWith("Jun"))
                        month = 6;
                    else if (v[0].StartsWith("Jul"))
                        month = 7;
                    else if (v[0].StartsWith("Aug"))
                        month = 8;
                    else if (v[0].StartsWith("Sep"))
                        month = 9;
                    else if (v[0].StartsWith("Oct"))
                        month = 10;
                    else if (v[0].StartsWith("Nov"))
                        month = 11;
                    else if (v[0].StartsWith("Dec"))
                        month = 12;

                    if (month >= 0)
                    {
                        int day = int.Parse(v[1]);
                        int year = int.Parse(v[2]);

                        try
                        {
                            float val;
                            if (float.TryParse(values[i], out val))
                            {
                                Quarters.Add(new LocalDate(year, month, day));
                                Eps.Add(val);
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}
