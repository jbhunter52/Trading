using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public class HistoricalDataResponse
    {
        public string date { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public int volume { get; set; }
        public int unadjustedVolume { get; set; }
        public float change { get; set; }
        public float changePercent { get; set; }
        public float vwap { get; set; }
        public string label { get; set; }
        public float changeOverTime { get; set; }
        public float MovingAverageClose { get; set; }
        public int MovingAverageVolume { get; set; }
        public float RelativePriceVolume { get; set; }
        public float MovingRelativePriceVolume { get; set; }
    }

   
}
