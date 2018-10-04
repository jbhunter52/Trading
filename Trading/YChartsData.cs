using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Core;
using ScrapySharp.Extensions;
using NodaTime;

namespace Trading
{
    public static class YChartsData
    {
        public static EpsData GetEpsData(string symbol)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://ycharts.com/companies/MSFT/eps");

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
                        Quarters.Add(new LocalDate(year, month, day));
                        Eps.Add(float.Parse(values[i]));
                    }
                }
            }
        }
    }
}
