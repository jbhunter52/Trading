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
    class RunSim
    {
        static void Main(string[] args)
        {
            bool overwrite = false;
            string dbfile = @"C:\Users\Jared\AppData\Local\TradeData\Stocks2RBC_9-19-18.db";
            string cupDbFile = Path.Combine(Path.GetDirectoryName(dbfile), Path.GetFileNameWithoutExtension(dbfile) + "_ch.db");
            Trading.Database db = new Trading.Database(dbfile, overwrite);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //IEXData.DownloadSymbol("FTF^#", IEXData.HistoryType.FiveYear);
            //db.GetAllData2(Trading.IEXData.HistoryType.FiveYear, -1);
            Trading.Debug.Nlog("Get all " + ((float)sw.ElapsedMilliseconds / 1000).ToString() + " seconds");
            
            sw.Restart();
            List<Company> list = new List<Company>();
            var col = db.DB.GetCollection<Trading.Company>("data");
            int num = 5000;
            for (int i = 1; i < num; i++ )
            {
                Company c = col.FindById(i);
                c.CurrentCupHandle = new CupHandle();
                list.Add(c);
            }
            //list = col.Find(Query.And(Query.GT("MinClose", 2.0f), Query.GT("MinVolume", 1000), Query.LT("maxDayChangePerc", 1.5))).ToList();
            Trading.Debug.Nlog("After filter " + list.Count.ToString() + " stocks");
            Trading.Debug.Nlog("Filter " + ((float)sw.ElapsedMilliseconds / 1000).ToString() + " seconds");

            CupHandleParameters chp = new CupHandleParameters(CupHandleDefinition.Haiku1);
            
            DateTime startDay = new DateTime(2013, 9, 19);
            DateTime endDay = new DateTime(2018, 9, 27);
            int runDays = endDay.Subtract(startDay).Days;
            List<CupHandle> cupHandles = new List<CupHandle>();
            List<string> chSymbols = new List<string>();

            if (File.Exists(cupDbFile))
                File.Delete(cupDbFile);

            int cupHandleId = 1;

            Portfolio portfolio = new Portfolio(10000.0f, 1.20f, 0.92f);

            using (LiteDatabase chDb = new LiteDatabase(cupDbFile))
            {
                var colch = chDb.GetCollection<CupHandle>("CupHandle");
                for (int i = 0; i < runDays; i++)
                {
                    DateTime thisDay = startDay.AddDays(i);
                    string mess = thisDay.ToShortDateString() + ", " + portfolio.GetTotalValue().ToString() + ", Total Assets: " + portfolio.Assets.Count.ToString();
                    Trading.Debug.Nlog(mess);
                    Trading.Debug.Nlog(mess);
                    foreach (Company c in list)
                    {
                        CupHandle ch = new CupHandle();
                        int thisDayInd = -1;
                        bool found = c.FindIndexByDate(thisDay, out thisDayInd);
                        if (found)
                        {

                            //Check if cuphandle is established
                            if (c.CurrentCupHandle.D.Index > 0)
                            {
                                int endBuySearch = c.CurrentCupHandle.D.Index + (int)(chp.BuyWait * (c.CurrentCupHandle.C.Index - c.CurrentCupHandle.A.Index));
                                if (c.CurrentCupHandle.D.Index + 1 >= c.close.Count)
                                    break;
                                if (endBuySearch >= c.close.Count)
                                    endBuySearch = c.close.Count - 1;
                                for (int j = c.CurrentCupHandle.D.Index + 1; j < endBuySearch; j++)
                                {
                                    if (c.close.Count > j) //check that this day in the for loop is valid for data known
                                    {
                                        float close = c.close[j];
                                        if (close > c.CurrentCupHandle.C.Close)
                                        {
                                            DateTime dDate = MathHelpers.StringToDate(c.date[j]);
                                            c.CurrentCupHandle.Buy = c.GetPoint(j, dDate);
                                            c.CurrentCupHandle.BuyTrigger = true;
                                            // Debug.Nlog(c.Symbol + " Rank " + c.CurrentCupHandles.GetRank().ToString());
                                            c.CurrentCupHandle.Symbol = c.Symbol;
                                            //c.CurrentCupHandle.Id = ObjectId.NewObjectId();
                                            //colch.Insert(c.CurrentCupHandle);
                                            colch.Insert(cupHandleId, c.CurrentCupHandle);
                                            cupHandleId++;

                                            //Buy Triggered
                                            if (portfolio.Cash > 1000)
                                            {
                                                portfolio.Buy(new Asset(c.Symbol, c.CurrentCupHandle.Buy.Close, 1000.0f));
                                            }

                                            c.CurrentCupHandle = new CupHandle(); //restart cuphandle
                                            break;
                                        }
                                    }
                                    else
                                        break;
                                }
                            }


                            if (c.IsHighToDate(thisDayInd)) //Search for A
                            {
                                //Debug.Nlog(c.Symbol + ", New A found");
                                c.CurrentCupHandle = new CupHandle(); //reset cuphandle
                                c.CurrentCupHandle.SetA(c.GetPoint(thisDayInd, thisDay)); //Set new A
                            }
                            else if (c.CurrentCupHandle.A != null)
                            {
                                int lastA = thisDayInd - c.CurrentCupHandle.A.Index;

                                if (chp.AC.ContainsValue(lastA)) //Last A in correct Range, search for C
                                {
                                    Point possibleC = c.GetPoint(thisDayInd, thisDay);
                                    if (chp.PivotRatio.ContainsValue(possibleC.Close / c.CurrentCupHandle.A.Close))
                                    {
                                        // Pc/Pa is now verified to be within the pivot ratio range
                                        if (c.EnsurePointJ(possibleC, ch)) //Now check that no Pj exceeds line connection Pc and Pa
                                        {
                                            c.CurrentCupHandle.SetC(possibleC);
                                            //Now searching for handle
                                            //Debug.Nlog(c.Symbol + ", found C");

                                            //Use URPV3 to find B???
                                            //BC range, or Frame 3 span, or RightCup
                                            int rangeBC = chp.CupRight.Maximum - chp.CupRight.Minimum;
                                            int startBsearch = c.CurrentCupHandle.C.Index - rangeBC;
                                            float urpvSum = 0;
                                            float drpvSum = 0;
                                            int urpvCnt = 0;
                                            int drpvCnt = 0;
                                            List<float> urpv3Arr = new List<float>();
                                            List<float> R1arr = new List<float>();
                                            List<float> R3arr = new List<float>();
                                            for (int j = startBsearch; j < c.CurrentCupHandle.C.Index; j++)
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
                                            int ind = R1R3sum.IndexOf(bestR1R3);
                                            int bestR1R3Index = startBsearch + ind;
                                            float urpv3 = urpv3Arr[ind];
                                            if (bestR1R3 > 0)
                                            {
                                                //B Found!!!
                                                //Debug.Nlog("B found!!!");
                                                c.CurrentCupHandle.R1 = R1arr[ind];
                                                c.CurrentCupHandle.R3 = R3arr[ind];
                                                DateTime bDate = MathHelpers.StringToDate(c.date[bestR1R3Index]);
                                                c.CurrentCupHandle.SetB(c.GetPoint(bestR1R3Index, bDate));

                                                //Now continue and find PointD the handle
                                                int rangeCD = chp.Handle.Maximum - chp.Handle.Minimum;
                                                int startDsearch = c.CurrentCupHandle.C.Index + chp.CupRight.Minimum;
                                                int endDsearch = startDsearch + rangeCD;
                                                if (startDsearch >= c.close.Count)
                                                    break;
                                                if (endDsearch >= c.close.Count)
                                                    endDsearch = c.close.Count - 1;
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

                                                //Enforce D requirements
                                                List<float> newR2arr = new List<float>();
                                                for (int cnt = 0; cnt < R2arr.Count;cnt++)
                                                {
                                                    ind = startDsearch + cnt;
                                                    DateTime dDate = MathHelpers.StringToDate(c.date[ind]);
                                                    float dClose = c.close[ind];
                                                    float CBdiff = c.CurrentCupHandle.C.Close - c.CurrentCupHandle.B.Close;
                                                    if (CBdiff > 0)
                                                    {
                                                        float CDdiff = c.CurrentCupHandle.C.Close - dClose;
                                                        if (dClose < c.CurrentCupHandle.C.Close)
                                                        {
                                                            if (dClose > (c.CurrentCupHandle.B.Close + CBdiff*0.5))
                                                            {
                                                                newR2arr.Add(R2arr[cnt]);
                                                            }
                                                        }
                                                    }
                                                }

                                                if (newR2arr.Count > 0)
                                                {
                                                    R2arr = newR2arr;
                                                    float bestR2 = R2arr.Max();
                                                    ind = R2arr.IndexOf(bestR2);
                                                    int bestR2Index = startDsearch + ind;

                                                    if (bestR2 > 0)
                                                    {
                                                        //D found !!!
                                                        //Debug.Nlog("D found!!!");
                                                        c.CurrentCupHandle.R2 = R2arr[ind];
                                                        if (c.CurrentCupHandle.GetRank() > chp.MinRank)
                                                        {
                                                            DateTime dDate = MathHelpers.StringToDate(c.date[bestR2Index]);

                                                            c.CurrentCupHandle.D = c.GetPoint(bestR2Index, dDate);
                                                        }


                                                    }
                                                }
                                            }


                                        }
                                    }
                                }
                            }

                            int index = portfolio.Assets.FindIndex(x => x.Symbol.Equals(c.Symbol));
                            if (index >= 0)
                            {
                                float perShare = c.close[thisDayInd];
                                Asset a = portfolio.Assets[index];
                                a.SetCurrentValue(perShare);
                                float change = perShare/a.PerShareInitial;

                                if (change > (portfolio.StopGain))
                                {
                                    Trading.Debug.Nlog("Sold " + a.Symbol + " for gain");
                                    portfolio.Sell(a, perShare); //Set asset for gain
                                }
                                if (change < portfolio.StopLoss)
                                {
                                    Trading.Debug.Nlog("Sold " + a.Symbol + " for loss");
                                    portfolio.Sell(a, perShare); //sell asset for loss
                                }
                                    
                            }

                        }

                        //End of company loop

                    }

                    //end of trading day loop



                }
            }
            Trading.Debug.Nlog("Found " + (cupHandleId + 1).ToString() + " handles");
            Trading.Debug.Nlog("Finished");
            Console.ReadKey();
            
        }

        
    }


}
