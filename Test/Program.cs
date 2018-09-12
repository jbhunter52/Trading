using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Trading.Database db = new Trading.Database(@"C:\Users\Jared\AppData\Local\TradeData\test.db");
            db.GetSymbol("aapl");

            Trading.Company appl = db.LoadSymbol("appl");


        }
    }
}
