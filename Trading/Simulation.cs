using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Trading;
using LiteDB;
using System.Diagnostics;
using NodaTime;
using System.Data;

namespace Trading
{
    public class Simulation
    {
        public string Dbfile;
        public LocalDate StartDay;
        public LocalDate EndDay;
        public Portfolio Portfolio;
        public float MinEpsGrowth;
        public int MinNumPrevQuarters;
        public CupHandleParameters Chp;
        public List<LocalDate> Dates;
        public List<float> Values;
        public List<Company> List;

        public Simulation()
        {
        }

        public void SetDefault()
        {
            StartDay = new LocalDate(2013, 9, 19);
            EndDay = new LocalDate(2018, 9, 27);
            Portfolio = new Portfolio(10000.0f, 1.20f, 0.92f, 1000.0f);
            Chp = new CupHandleParameters(CupHandleDefinition.Haiku1);
            MinEpsGrowth = 0.25f;
            MinNumPrevQuarters = 3;
        }

        public List<Company> GetTradeList()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Trading.Database db = new Trading.Database(Dbfile, false);
            List<Company> list = new List<Company>();

            var col = db.DB.GetCollection<Trading.Company>("data");
            Trading.Debug.Nlog("Sim Start, total in collection, " + col.Count());
            //list = col.FindAll().ToList();
            list = col.Find(Query.Where("EarningsData", earnings => earnings.AsArray.Count > 0)).ToList();
            //list = col.Find(Query.And(Query.LT("Id", 500),Query.Where("EarningsData", earnings => earnings.AsArray.Count > 0))).ToList();
            Trading.Debug.Nlog("Sim Start, total with earnings data, " + list.Count);
            sw.Stop();

            for (int i = 0; i < list.Count; i++)
            {
                Company c = list[i];
            }
            Trading.Debug.Nlog("Database query time, " + sw.Elapsed.TotalMinutes.ToString() + " minutes");
            sw.Reset();


            //list = col.Find(Query.And(Query.GT("MinClose", 2.0f), Query.GT("MinVolume", 1000), Query.LT("maxDayChangePerc", 1.5))).ToList();
            Trading.Debug.Nlog("After filter " + list.Count.ToString() + " stocks");
            Trading.Debug.Nlog("Filter " + ((float)sw.ElapsedMilliseconds / 1000).ToString() + " seconds");

            List = list;
            return list;
        }

        public void Run(List<Company> list = null, IProgress<Result> progress = null)
        {
            if (list == null)
                list = List;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Dates = new List<LocalDate>();
            Values = new List<float>();
            Portfolio.Cash = Portfolio.StartCash;
            Portfolio.Assets = new List<Asset>();

            //initialize dynamic Company members
            foreach (Company c in list)
            {
                c.SetEarningsGrowthSeries(MinNumPrevQuarters);
                c.CurrentCupHandle = new CupHandle();
                c.Iterator = -1; //reset iterator
            }

            int runDays = Period.Between(StartDay, EndDay, PeriodUnits.Days).Days;
            List<CupHandle> cupHandles = new List<CupHandle>();
            List<string> chSymbols = new List<string>();

            string cupDbFile = Path.Combine(Path.GetDirectoryName(Dbfile), Path.GetFileNameWithoutExtension(Dbfile) + "_ch.db");
            if (File.Exists(cupDbFile))
                File.Delete(cupDbFile);

            int cupHandleId = 1;

            using (LiteDatabase chDb = new LiteDatabase(cupDbFile))
            {
                Trading.Debug.Nlog("Starting simulation");
                var colch = chDb.GetCollection<CupHandle>("CupHandle");
                for (int i = 0; i < runDays; i++)
                {
                    LocalDate thisDay = StartDay.PlusDays(i);
                    Dates.Add(thisDay);
                    foreach (Company c in list)
                    {
                        //if (c.Iterator >= c.date.Count) //Check if day is > data
                        //    continue;
                        
                        int thisDayInd = -1;

                        //if (c.Iterator < 0)
                        //{
                        //    if (thisDay.CompareTo(c.date[0]) == 0)
                        //    {
                        //        c.Iterator = 0; //Initialize for first day of data
                        //    }
                        //}

                        thisDayInd = c.date.BinarySearch(thisDay);
                        //bool found = c.FindIndexByDate(thisDay, out thisDayInd);
                        if (thisDayInd >= 0)
                        {
                            //Start day
                            //thisDayInd = c.Iterator;
                            //thisDay = c.date[c.Iterator];

                            //float epsGrowth = c.ComputeEarningsGrowth(thisDay, MinNumPrevQuarters);
                            float epsGrowth = c.EarningsGrowth[thisDayInd];
                            if (epsGrowth == float.NaN || epsGrowth < MinEpsGrowth)
                                continue;

                            CupHandle ch = new CupHandle();
                            //Check if cuphandle is established
                            if (c.CurrentCupHandle.D.Index > 0)
                            {
                                int endBuySearch = c.CurrentCupHandle.D.Index + (int)(Chp.BuyWait * (c.CurrentCupHandle.C.Index - c.CurrentCupHandle.A.Index));
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
                                            LocalDate dDate = c.date[j];
                                            c.CurrentCupHandle.Buy = c.GetPoint(j);
                                            c.CurrentCupHandle.BuyTrigger = true;
                                            // Debug.Nlog(c.Symbol + " Rank " + c.CurrentCupHandles.GetRank().ToString());
                                            c.CurrentCupHandle.Symbol = c.Symbol;
                                            //c.CurrentCupHandle.Id = ObjectId.NewObjectId();
                                            //colch.Insert(c.CurrentCupHandle);
                                            colch.Insert(cupHandleId, c.CurrentCupHandle);
                                            cupHandleId++;

                                            //Buy Triggered
                                            if (Portfolio.Cash > Portfolio.BuyAmount)
                                            {
                                                Trading.Debug.Nlog("Bought " + c.Symbol);
                                                Portfolio.Buy(new Asset(c.Symbol, c.CurrentCupHandle.Buy.Close, Portfolio.BuyAmount, thisDay));
                                                ReportProgress(progress, thisDay, i, runDays);
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
                                c.CurrentCupHandle.SetA(c.GetPoint(thisDayInd)); //Set new A
                            }
                            else if (c.CurrentCupHandle.A != null)
                            {
                                int lastA = thisDayInd - c.CurrentCupHandle.A.Index;

                                if (Chp.AC.ContainsValue(lastA)) //Last A in correct Range, search for C
                                {
                                    Point possibleC = c.GetPoint(thisDayInd);
                                    if (Chp.PivotRatio.ContainsValue(possibleC.Close / c.CurrentCupHandle.A.Close))
                                    {
                                        // Pc/Pa is now verified to be within the pivot ratio range
                                        if (c.EnsurePointJ(possibleC, ch)) //Now check that no Pj exceeds line connection Pc and Pa
                                        {
                                            c.CurrentCupHandle.SetC(possibleC);
                                            //Now searching for handle
                                            //Debug.Nlog(c.Symbol + ", found C");

                                            //Use URPV3 to find B???
                                            //BC range, or Frame 3 span, or RightCup
                                            int rangeBC = Chp.CupRight.Maximum - Chp.CupRight.Minimum;
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
                                                LocalDate bDate = c.date[bestR1R3Index];
                                                c.CurrentCupHandle.SetB(c.GetPoint(bestR1R3Index));

                                                //Now continue and find PointD the handle
                                                int rangeCD = Chp.Handle.Maximum - Chp.Handle.Minimum;
                                                int startDsearch = c.CurrentCupHandle.C.Index + Chp.CupRight.Minimum;
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
                                                    LocalDate dDate = c.date[ind];
                                                    float dClose = c.close[ind];
                                                    float CBdiff = c.CurrentCupHandle.C.Close - c.CurrentCupHandle.B.Close;
                                                    if (CBdiff > 0)
                                                    {
                                                        float CDdiff = c.CurrentCupHandle.C.Close - dClose;
                                                        if (dClose < c.CurrentCupHandle.C.Close)
                                                        {
                                                            if (dClose > (c.CurrentCupHandle.B.Close + CBdiff * Chp.CBdiffMin))
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
                                                        if (c.CurrentCupHandle.GetRank() > Chp.MinRank && c.CurrentCupHandle.GetMinimumRank() > Chp.MinSingleRank)
                                                        {
                                                            LocalDate dDate = c.date[bestR2Index];
                                                            c.CurrentCupHandle.D = c.GetPoint(bestR2Index);
                                                        }


                                                    }
                                                }
                                            }


                                        }
                                    }
                                }
                            }

                            int index = Portfolio.Assets.FindIndex(x => x.Symbol.Equals(c.Symbol));
                            if (index >= 0)
                            {
                                float perShare = c.close[thisDayInd];
                                Asset a = Portfolio.Assets[index];
                                a.SetCurrentValue(perShare);
                                float change = perShare/a.PerShareInitial;

                                if (change > (Portfolio.StopGain))
                                {
                                    Trading.Debug.Nlog("Sold " + a.Symbol + " for gain");
                                    Portfolio.Sell(a, perShare); //Set asset for gain
                                    ReportProgress(progress, thisDay, i, runDays);
                                }
                                if (change < Portfolio.StopLoss)
                                {
                                    Trading.Debug.Nlog("Sold " + a.Symbol + " for loss");
                                    Portfolio.Sell(a, perShare); //sell asset for loss
                                    ReportProgress(progress, thisDay, i, runDays);
                                }
                                    
                            }

                        }
                        c.Iterator++;
                        //End of company loop

                    }
                    float endOfDayValue = Portfolio.GetTotalValue();
                    float slope;
                    float rSquared;
                    MathHelpers.LinearRegression(Values.ToArray(), out rSquared, out slope);
                    string mess = thisDay.ToString() + ", " + endOfDayValue.ToString() + ", Total Assets: " + Portfolio.Assets.Count.ToString();
                    Trading.Debug.Nlog(mess);
                    Trading.Debug.Nlog(Portfolio.AllAssetsToString());
                    //end of trading day loop
                }
            }
            Trading.Debug.Nlog("Found " + (cupHandleId + 1).ToString() + " handles");
            sw.Stop();
            Trading.Debug.Nlog("Total sim time, " + sw.Elapsed.TotalMinutes.ToString() + " minutes");
            Trading.Debug.Nlog("Finished");
            //Console.ReadKey();
        }

        public delegate void ProgressUpdate(object sender, ProgressUpdateArgs arg);
        public event ProgressUpdate OnProgressUpdate;

        public void ReportProgress(IProgress<Result> progress, LocalDate thisDay, int currentIndex, int runDays)
        {
            if (progress != null)
            {
                float endOfDayValue = Portfolio.GetTotalValue();
                string mess = thisDay.ToString() + ", " + endOfDayValue.ToString() + ", Total Assets: " + Portfolio.Assets.Count.ToString();
                Trading.Debug.Nlog(mess);
                Values.Add(endOfDayValue);

                Result r = new Result();
                r.Date = thisDay;
                r.PercComp = (float)currentIndex / runDays;
                r.Portfolio = Portfolio;

                #region DataTable
                //DataTable dt = new DataTable();
                //dt.Columns.Add("Symbol", typeof(string));
                //dt.Columns.Add("BuyDate", typeof(string));
                //dt.Columns.Add("Initial Value", typeof(float));
                //dt.Columns.Add("Current Value", typeof(float));
                //dt.Columns.Add("GainLoss", typeof(float));

                //foreach (Asset a in r.Portfolio.Assets)
                //{
                //    string date = a.BuyDate.Month.ToString() + "/" + a.BuyDate.Day.ToString() + "/" + a.BuyDate.Year.ToString();
                //    float gainloss = (a.CurrentTotalValue - a.PerShareInitial) / a.PerShareInitial;
                //    dt.Rows.Add(a.Symbol, a.PerShareInitial, a.CurrentTotalValue, gainloss);
                //}
                //r.Dt = dt;
                //ProgressUpdateArgs arg = new ProgressUpdateArgs(r);
                //OnProgressUpdate(this, arg);
                #endregion

                #region PortfolioStatus
                string status = "";
                foreach (Asset a in Portfolio.Assets)
                {
                    string date = a.BuyDate.Month.ToString() + "/" + a.BuyDate.Day.ToString() + "/" + a.BuyDate.Year.ToString();
                    float gainloss = (a.CurrentPerShare - a.PerShareInitial) / a.PerShareInitial;
                    status = status + a.Symbol + ", " + date + ", " + ((int)(gainloss * 100)).ToString() + "%" + "\r\n";
                }
                r.Status = status;
                #endregion

                progress.Report(r);
            }
        }

    }
    public class ProgressUpdateArgs : EventArgs
    {
        public Result Result { get; private set; }

        public ProgressUpdateArgs(Result result)
        {
            Result = result;
        }
    }
    public class Result
    {
        public LocalDate Date;
        public float PercComp;
        public Portfolio Portfolio;
        public DataTable Dt;
        public string Status;

        public Result()
        {

        }
    }
}
