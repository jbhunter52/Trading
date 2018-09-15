using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public class SymbolData
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string isEnabled { get; set; }
        public string type { get; set; }
        public string iexId { get; set; }
    }
}
