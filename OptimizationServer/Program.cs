using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading;
using System.IO;

namespace OptimizationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Simulation sim = new Simulation();
            Console.WriteLine("Initializing...");
            if (args.Length == 0)
                return;
            if (!File.Exists(args[0]))
                return;

            string pFile = @"C:\Users\Jared\AppData\Local\TradeData\Optimization\param.txt";
            string rFile = @"C:\Users\Jared\AppData\Local\TradeData\Optimization\result.txt";
            string readyFile = @"C:\Users\Jared\AppData\Local\TradeData\Optimization\ready.txt";
            sim.Dbfile = args[0];
            sim.SetDefault();
            sim.GetTradeList();

            File.Create(readyFile).Dispose();
            while (true)
            {
                if (File.Exists(pFile))
                {
                    while (IsFileReady(pFile) == false)
                    {
                        System.Threading.Thread.Sleep(20);
                    }
                        
                    string parameters = File.ReadAllLines(pFile)[0];
                    string[] p = parameters.Split(',');

                    float minTotalRank = float.Parse(p[0]);
                    float minSingleRank = float.Parse(p[1]);
                    float stopGain = float.Parse(p[2]);
                    float stopLoss = float.Parse(p[3]);
                    float minGrowth = float.Parse(p[4]);

                    sim.Chp.MinRank = minTotalRank;
                    sim.Chp.MinSingleRank = minSingleRank;
                    sim.Portfolio.StopGain = stopGain;
                    sim.Portfolio.StopLoss = stopLoss;
                    sim.MinEpsGrowth = minGrowth;

                    sim.Run();
                    Console.WriteLine(parameters + "," + sim.Portfolio.GetTotalValue().ToString());
                    File.Delete(pFile);

                    using (StreamWriter sr = new StreamWriter(rFile, false))
                    {
                        sr.Write(sim.Portfolio.GetTotalValue().ToString());
                    }

                }
                System.Threading.Thread.Sleep(100);
            }


            

        }

        public static bool IsFileReady(string filename)
        {
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
