using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace Trading
{
    public class Asset
    {
        public string Symbol;
        public float TotalValue;
        public float PerShareInitial;
        public float CurrentPerShare;
        public float CurrentTotalValue;
        public LocalDate BuyDate;

        public Asset(string sym, float perShareInital, float totalValue, LocalDate buyDate)
        {
            Symbol = sym;
            TotalValue = totalValue;
            PerShareInitial = perShareInital;
            CurrentTotalValue = totalValue;
            BuyDate = buyDate;
        }

        public float SetCurrentValue(float currentPerShare)
        {
            CurrentPerShare = currentPerShare;
            CurrentTotalValue = TotalValue * (currentPerShare / PerShareInitial);
            return CurrentTotalValue;
        }
    }
}
