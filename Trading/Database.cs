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
        public Database(string filename, bool newdb)
        {
            if (newdb)
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
            DB = new LiteDatabase(@filename);

        }
        public void AddSymbol(Company c)
        {
            var col = DB.GetCollection<Trading.Company>("data");
            col.Insert(c);
        }
        public Company GetSymbolFromDB(string symbol)
        {
            var col = DB.GetCollection<Trading.Company>("data");

            var c = col.FindOne(x => x.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase));
            return (Company)c;
        }

        public void GetAllData(IEXData.HistoryType ht, int num)
        {
           List<Trading.SymbolData> symbols = Trading.IEXData.DownloadSymbolList();

           int cnt = symbols.Count;
            if (num > 0)
                cnt = num;


            for (int i = 0; i < cnt; i++)
            {
                Trading.SymbolData symData = symbols[i];
                Console.WriteLine((i+1).ToString() + "/" + symbols.Count.ToString() + "\t" + symData.symbol  );
                Trading.Company c = Trading.IEXData.DownloadSymbol(symData.symbol, ht);
                if (c.Id != -1)
                    this.AddSymbol(c);
                //System.Threading.Thread.Sleep(20);
            }
        }
    }
}
