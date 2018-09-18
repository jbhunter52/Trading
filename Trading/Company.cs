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



        public Company(string symbol, List<HistoricalDataResponse> data)
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

            this.Symbol = symbol;
            this.Name = "";

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
    }
}
