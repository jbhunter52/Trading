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
        public List<HistoricalDataResponse> Data { get; set; }

        public Company(string symbol, List<HistoricalDataResponse> data)
        {
            this.Symbol = symbol;
            this.Data = data;
            this.Name = "";

            //Maybe it would be faster to reverse???
            this.Data.OrderBy(x => DateTime.Parse(x.date));
            
            double[] close = (from dp in this.Data
                             select dp.close).ToArray();
            double[] maClose = MathHelpers.MovingAverage(close, 50);
            int[] vol = (from dp in this.Data
                              select dp.volume).ToArray();
            int[] maVol = MathHelpers.MovingAverage(vol, 50);

            double[] rpv = new double[close.Length];
            double[] marpv = new double[close.Length];

            rpv[0] = 0;
            for (int i = 1; i < close.Length; i++)
            {
                double y = (close[i] - close[i-1])/close[i-1];
                rpv[i] = vol[i] * y;
            }
            marpv = MathHelpers.MovingAverage(rpv, 50);

            for (int i = 0; i < this.Data.Count; i++)
            {
                this.Data[i].MovingAverageClose = maClose[i];
                this.Data[i].MovingAverageVolume = maVol[i];
                this.Data[i].RelativePriceVolume = rpv[i];
                this.Data[i].MovingRelativePriceVolume = marpv[i];
            }

            
        }
    }
}
