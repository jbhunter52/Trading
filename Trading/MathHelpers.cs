using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public static class MathHelpers
    {
        public static List<float> MovingAverage(List<float> data, int windowSize)
        {
            try
            {
                float[] buffer = new float[windowSize];
                for (int i = 0; i < buffer.Length; i++) { buffer[i] = data[0] / windowSize; }
                float[] output = new float[data.Count];
                int current_index = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    buffer[current_index] = data[i] / windowSize;
                    float ma = 0;
                    for (int j = 0; j < windowSize; j++)
                    {
                        ma += buffer[j];
                    }
                    output[i] = ma;
                    current_index = (current_index + 1) % windowSize;
                }
                return output.ToList();
            }
            catch
            { return new List<float>(); }
        }
        public static List<int> MovingAverage(List<int> data, int windowSize)
        {
            try
            {
                int[] buffer = new int[windowSize];
                for (int i = 0; i < buffer.Length; i++) { buffer[i] = data[0] / windowSize; }
                int[] output = new int[data.Count];
                int current_index = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    buffer[current_index] = data[i] / windowSize;
                    int ma = 0;
                    for (int j = 0; j < windowSize; j++)
                    {
                        ma += buffer[j];
                    }
                    output[i] = ma;
                    current_index = (current_index + 1) % windowSize;
                }
                return output.ToList();
            }
            catch { return new List<int>(); }
        }
        public static string DateToString(DateTime dt)
        {
            int month = dt.Month;
            string monthStr = month.ToString();
            if (month < 10)
                monthStr = "0" + monthStr;

            int day = dt.Day;
            string dayStr = day.ToString();
            if (day < 10)
                dayStr = "0" + dayStr;



            return dt.Year + "-" + monthStr + "-" + dayStr;
        }
        public static DateTime StringToDate(string date)
        {
            //Need To Test
            string[] v = date.Split('-');
            int year = Int16.Parse(v[0]);
            int month = Int16.Parse(v[1]);
            int day = Int16.Parse(v[2]);
            return new DateTime(year, month, day);
        }
    }
}
