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
            bool overwrite = false;
            Trading.Database db = new Trading.Database(@"C:\Users\Jared\AppData\Local\TradeData\Stocks2RBC_9-19-18.db", overwrite);

            Stopwatch sw = new Stopwatch();
            sw.Start();;
            //db.GetAllData2(Trading.IEXData.HistoryType.FiveYear, 0);
            Console.WriteLine("Get all " + ((float)sw.ElapsedMilliseconds / 1000).ToString() + " seconds");
            
            sw.Restart();
            List<Company> list = new List<Company>();
            
            var col = db.DB.GetCollection<Trading.Company>("data");
            int num = 200;
            for (int i = 1; i < num; i++ )
            {
                Company c = col.FindById(i);
                c.CurrentCupHandles = new CupHandle();
                c.FullCupHandles = new List<CupHandle>();
                list.Add(c);
            }
            //list = col.Find(Query.And(Query.GT("MinClose", 2.0f), Query.GT("MinVolume", 1000), Query.LT("maxDayChangePerc", 1.5))).ToList();
            Console.WriteLine("After filter " + list.Count.ToString() + " stocks");
            Console.WriteLine("Filter " + ((float)sw.ElapsedMilliseconds / 1000).ToString() + " seconds");

            CupHandleParameters chp = new CupHandleParameters(CupHandleDefinition.Haiku1);
            
            DateTime startDay = new DateTime(2013, 9, 19);
            int runDays = 365 * 5;
            for (int i = 0; i < runDays;i++ )
            {
                DateTime thisDay = startDay.AddDays(i);
                Console.WriteLine(thisDay.ToString());
                foreach (Company c in list)
                {
                    int ind = -1;
                    bool found = c.FindIndexByDate(thisDay, out ind);
                    if (found)
                    {
                        if (c.IsHighToDate(ind)) //Search for A
                        {
                            Console.WriteLine(c.Symbol + ", New A found");
                            c.CurrentCupHandles.A = c.GetPoint(ind, thisDay); //Set new A
                        }
                        else if (c.CurrentCupHandles.A != null)
                        {
                            int lastA = ind - c.CurrentCupHandles.A.Index;
                            
                            if (chp.AC.ContainsValue(lastA)) //Last A in correct Range, search for C
                            {
                                Point possibleC = c.GetPoint(ind, thisDay);
                                if (chp.PivotRatio.ContainsValue(possibleC.Close/c.CurrentCupHandles.A.Close ))
                                {
                                    // Pc/Pa inside pivot ratio

                                    //Now check that no Pj exceeds line connection Pc and Pa
                                    if (c.EnsurePointJ(possibleC)) //EnsurePointJ needs work!!!
                                    {
                                        //Do I need to require more cup filters?
                                        //What about volume fileters?
                                        c.CurrentCupHandles.C = possibleC;
                                        //Now searching for handle
                                        Console.WriteLine(c.Symbol + ", found C");
                                    }
                                }
                            }
                        }

                    }
                }
            }

                Console.ReadKey();
        }

        
    }
}
