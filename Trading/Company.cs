using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol {get;set;}
        //public List<HistoricalDataResponse> Data { get; set; }

        public int Count = 0;
        public List<string> date { get; set; }
        public List<float> open { get; set; }
        public List<float> high { get; set; }
        public List<float> low  { get; set; }
        public List<float> close  { get; set; }
        public List<int> volume { get; set; }
        public List<int> unadjustedVolume { get; set; }
        public List<float> change  { get; set; }
        public List<float> changePercent  { get; set; }
        public List<float> vwap  { get; set; }
        public List<string> label { get; set; }
        public List<float> changeOverTime  { get; set; }
        public List<float> MovingAverageClose  { get; set; }
        public List<int> MovingAverageVolume { get; set; }
        public List<float> RelativePriceVolume  { get; set; }
        public List<float> MovingRelativePriceVolume  { get; set; }
        public float MinClose { get; set; }
        public float MinVolume { get; set; }
        public float maxDayChangePerc { get; set; }
        public List<CupHandle> FullCupHandles { get; set; }
        public CupHandle CurrentCupHandles { get; set; }
        public Company(string symbol, List<HistoricalDataResponse> data, SymbolData sym = null)
        {
            date = new List<string>();
            open = new List<float>();
            high = new List<float>();
            low = new List<float>();
            close = new List<float>();
            volume = new List<int>();
            unadjustedVolume = new List<int>();
            change = new List<float>();
            changePercent = new List<float>();
            vwap = new List<float>();
            label = new List<string>();
            changeOverTime = new List<float>();
            MovingAverageClose = new List<float>();
            MovingAverageVolume = new List<int>();
            RelativePriceVolume = new List<float>();
            MovingRelativePriceVolume = new List<float>();
            FullCupHandles = new List<CupHandle>();
            CurrentCupHandles = new CupHandle();

            this.Symbol = symbol;
            if (sym != null)
                Name = sym.name;
            else
                Name = "";

            foreach (HistoricalDataResponse d in data)
            {
                date.Add(d.date);
                open.Add(d.open);
                high.Add(d.high);
                low.Add(d.low);
                close.Add(d.close);
                volume.Add(d.volume);
                unadjustedVolume.Add(d.unadjustedVolume);
                change.Add(d.change);
                changePercent.Add(d.changePercent);
                vwap.Add(d.vwap);
                label.Add(d.label);
                changeOverTime.Add(d.changeOverTime);
                //MovingAverageClose.Add(d.MovingAverageClose);
                //MovingAverageVolume.Add(d.MovingAverageVolume);
                //RelativePriceVolume.Add(d.RelativePriceVolume);
                //MovingRelativePriceVolume.Add(d.MovingRelativePriceVolume);
                Count++;
            }

            MinClose = close.Min();
            MinVolume = close.Max();

            List<float> changeSpread = new List<float>();
            for (int i = 0; i < close.Count;i++ )
            {
                float delta = high[i] - low[i];
                changeSpread.Add(delta / open[i]);
            }
            maxDayChangePerc = changeSpread.Max();


            //Maybe it would be faster to reverse???
            //this.Data.OrderBy(x => DateTime.Parse(x.date));

            MovingAverageClose = MathHelpers.MovingAverage(close, 50);
            MovingAverageVolume = MathHelpers.MovingAverage(volume, 50);

            RelativePriceVolume.Add(0);
            for (int i = 1; i < close.Count; i++)
            {
                float y = (close[i] - close[i-1])/close[i-1];
                RelativePriceVolume.Add(volume[i] * y);
            }
            MovingRelativePriceVolume = MathHelpers.MovingAverage(RelativePriceVolume, 50);           
        }

        public Company()
        {
            Id = -1;
        }
        public bool FindIndexByDate(DateTime dt, out int ind)
        {
            for (int i = 0; i < date.Count; i++)
            {
                if (date[i].Equals(MathHelpers.DateToString(dt)))
                {
                    ind = i;
                    return true;
                }
            }
            ind = -1;
            return false;
        }

        public bool AllPriceAboveValue(float val)
        {
            float min = 0;
            foreach (float v in this.low)
            {
                if (v > min)
                    min = v;
            }
            if (min > val)
                return true;
            else
                return false;
        }
        public bool AllVolumeAboveValue(int val)
        {
            float min = 0;
            foreach (int v in this.volume)
            {
                if (v > min)
                    min = v;
            }
            if (min > val)
                return true;
            else
                return false;
        }

        public bool IsHighToDate(int i)
        {
            List<float> val = close.GetRange(0, i + 1);

            float today = close[i];
            if (today == val.Max())
                return true;
            else
                return false;
        }
        public Point GetPoint(int ind, DateTime thisDay)
        {
            return new Point(ind, close[ind], volume[ind], thisDay);
        }
        public bool EnsurePointJ(Point C)
        {
            bool under = true;
            for (int i = CurrentCupHandles.A.Index+1; i < C.Index; i++)
            {
                float Pj = this.close[i];
                float Pa = CurrentCupHandles.A.Close;
                float Pc = C.Close;

                int span = i - CurrentCupHandles.A.Index;
                float rise = Pc - Pa;
                int run = C.Index - CurrentCupHandles.A.Index;
                float slope = rise / run; //should be negative

                float lineVal = CurrentCupHandles.A.Close + slope * span;

                if (Pj < lineVal)
                    under = true;
                else
                    under = false;
            }
            return under;
        }
    }
}
