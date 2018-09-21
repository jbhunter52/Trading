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
            List<CupHandle> cupHandles = new List<CupHandle>();
            List<string> chSymbols = new List<string>();
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
                            //Console.WriteLine(c.Symbol + ", New A found");
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
                                    // Pc/Pa is now verified to be within the pivot ratio range

                                    // How to establish B???


                                    if (c.EnsurePointJ(possibleC)) //Now check that no Pj exceeds line connection Pc and Pa
                                    {
                                        c.CurrentCupHandles.C = possibleC;
                                        //Now searching for handle
                                        //Console.WriteLine(c.Symbol + ", found C");

                                        //Use URPV3 to find B???
                                        //BC range, or Frame 3 span, or RightCup
                                        int rangeBC = chp.CupRight.Maximum-chp.CupRight.Minimum;
                                        int startBsearch = c.CurrentCupHandles.C.Index-rangeBC;

                                        float urpvSum = 0;
                                        float drpvSum = 0;
                                        int urpvCnt = 0;
                                        int drpvCnt = 0;
                                        List<float> urpv3Arr = new List<float>();
                                        List<float> R1arr = new List<float>();
                                        List<float> R3arr = new List<float>();
                                        for (int j = startBsearch; j < c.CurrentCupHandles.C.Index; j++)
                                        {
                                            if (c.close[j] > c.close[j-1])
                                            {
                                                //Up day
                                                urpvSum += c.RelativePriceVolume[j];
                                                urpvCnt++;
                                            }
                                            else
                                            {
                                                //down day
                                                drpvSum += c.RelativePriceVolume[j];
                                                drpvCnt++;
                                            }

                                            if (c.MovingRelativePriceVolume[j] == 0)
                                            {
                                                //this is bad
                                                bool stop = true;
                                            }

                                            //Check RPV values
                                            float urpv = urpvSum / urpvCnt;
                                            urpv3Arr.Add(urpv);
                                            float drpv = drpvSum / drpvCnt;

                                            float R1;
                                            if (drpv == 0)
                                                R1 = (float)Math.Log10(urpv / drpv);
                                            else
                                                R1 = (float)Math.Log10(urpv / c.MovingRelativePriceVolume[j]);

                                            R1arr.Add(R1);
                                            float R3 = (float)Math.Log10(urpv / c.MovingRelativePriceVolume[j]);
                                            R3arr.Add(R3);
                                        }

                                        List<float> R1R3sum = new List<float>();
                                        //Now with data find the best PointB based on R1 and R2
                                        for (int j = 0; j < R1arr.Count; j++)
                                        {
                                            R1R3sum.Add(R1arr[j] + R3arr[j]);
                                        }

                                        float bestR1R3 = R1R3sum.Max();
                                        ind = R1R3sum.IndexOf(bestR1R3);
                                        int bestR1R3Index = startBsearch + ind;
                                        float urpv3 = urpv3Arr[ind];
                                        if (bestR1R3 > 0)
                                        {
                                            //B Found!!!
                                            //Console.WriteLine("B found!!!");
                                            c.CurrentCupHandles.R1 = R1arr[ind];
                                            c.CurrentCupHandles.R3 = R3arr[ind];
                                            DateTime bDate = MathHelpers.StringToDate(c.date[bestR1R3Index]);
                                            c.CurrentCupHandles.B = c.GetPoint(bestR1R3Index, bDate);

                                            //Now continue and find PointD the handle
                                            int rangeCD = chp.Handle.Maximum - chp.Handle.Minimum;
                                            int startDsearch = c.CurrentCupHandles.C.Index;
                                            int endDsearch = startDsearch + rangeCD;

                                            List<float> R2arr = new List<float>();
                                            urpvSum = 0;
                                            drpvSum = 0;
                                            urpvCnt = 0;
                                            drpvCnt = 0;
                                            for (int j = startDsearch; j < endDsearch; j++)
                                            {
                                                if (c.close[j] > c.close[j - 1])
                                                {
                                                    //Up day
                                                    urpvSum += c.RelativePriceVolume[j];
                                                    urpvCnt++;
                                                }
                                                else
                                                {
                                                    //down day
                                                    drpvSum += c.RelativePriceVolume[j];
                                                    drpvCnt++;
                                                }

                                                if (c.MovingRelativePriceVolume[j] == 0)
                                                {
                                                    //this is bad
                                                    bool stop = true;
                                                }

                                                float urpv = urpvSum / urpvCnt;
                                                float drpv = drpvSum / drpvCnt;
                                                float R2;
                                                if (drpv == 0)
                                                    R2 = (float)Math.Log10(urpv3 / drpv);
                                                else
                                                    R2 = (float)Math.Log10(urpv3 / c.MovingRelativePriceVolume[j]);
                                                R2arr.Add(R2);
                                            }


                                            float bestR2 = R2arr.Max();
                                            ind = R2arr.IndexOf(bestR2);
                                            int bestR2Index = startDsearch + ind;

                                            if (bestR2 > 0)
                                            {
                                                //D found !!!
                                                //Console.WriteLine("D found!!!");
                                                c.CurrentCupHandles.R2 = R2arr[ind];
                                                DateTime dDate = MathHelpers.StringToDate(c.date[bestR2Index]);
                                                c.CurrentCupHandles.D = c.GetPoint(bestR2Index, dDate);

                                               // Console.WriteLine(c.Symbol + " Rank " + c.CurrentCupHandles.GetRank().ToString());
                                                cupHandles.Add(c.CurrentCupHandles);
                                                chSymbols.Add(c.Symbol);
                                                bool stop = true;

                                            }
                                        }

                                        
                                    }
                                }
                            }
                        }

                    }
                }

                //end of trading day loop
                for (int j = 0; j < cupHandles.Count; j++)
                {
                    Console.WriteLine(chSymbols[j] + ", " + cupHandles[j].GetRank().ToString());
                }
            }

                Console.ReadKey();
        }

        
    }
}
