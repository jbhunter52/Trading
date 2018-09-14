using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.IO;


namespace Trading
{
    public class Database
    {
        public LiteDatabase DB;
        public Database(string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            DB = new LiteDatabase(@filename);

        }
        public Company DownloadSymbol(string symbol)
        {
            return IEXData.DownloadSymbol(symbol, IEXData.HistoryType.FiveYear);
        }
        public Company GetSymbolFromDB(string symbol)
        {
            var col = DB.GetCollection<Trading.Company>("data");

            var c = col.FindOne(x => x.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase));
            return (Company)c;
        }
    }
}
