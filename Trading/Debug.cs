using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public static class Debug
    {
        public static void Nlog(string mess)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(mess);
        }
    }
}
