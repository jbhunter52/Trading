using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public class Portfolio
    {
        public List<Asset> Assets;
        public float Cash;
        public float StopGain;
        public float StopLoss;

        public Portfolio(float cash, float stopGain, float stopLoss)
        {
            Assets = new List<Asset>();
            Cash = cash;
            StopGain = stopGain;
            StopLoss = stopLoss;
        }

        public void Buy(Asset asset)
        {
            if (!Assets.Any(x => x.Symbol.Equals(asset.Symbol)))
            {
                //Stock not currently owned
                Assets.Add(asset);
                Cash = Cash - asset.TotalValue;
            }
        }
        public void Sell(Asset asset, float sellValue)
        {
            Assets.Remove(asset);
            Cash = Cash + asset.SetCurrentValue(sellValue);
        }
        public bool OwnAsset(string sym)
        {
            foreach (Asset a in Assets)
            {
                if (a.Symbol.Equals("sym"))
                    return true;
            }
            return false;
        }

        public float GetTotalValue()
        {
            float val = Cash;
            foreach (Asset a in Assets)
            {
                val += a.CurrentTotalValue;
            }
            return val;
        }
    }
}
