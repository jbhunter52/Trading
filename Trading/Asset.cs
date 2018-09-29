using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public class Asset
    {
        public string Symbol;
        public float TotalValue;
        public float PerShareInitial;
        public float CurrentTotalValue;

        public Asset(string sym, float perShareInital, float totalValue)
        {
            Symbol = sym;
            TotalValue = totalValue;
            PerShareInitial = perShareInital;
            CurrentTotalValue = totalValue;
        }

        public float SetCurrentValue(float currentPerShare)
        {
            CurrentTotalValue = TotalValue * (currentPerShare / PerShareInitial);
            return CurrentTotalValue;
        }
    }
}
