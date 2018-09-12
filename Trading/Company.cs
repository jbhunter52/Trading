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

            this.Data.OrderBy(x => DateTime.Parse(x.date));

            foreach (List)
        }
    }
}
