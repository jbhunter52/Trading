using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public static class MathHelpers
    {
        public static double[] MovingAverage(double[] data, int windowSize)
        {
            double[] buffer = new double[windowSize];
            for (int i = 0; i < buffer.Length; i++) { buffer[i] = data[0] / windowSize; }
            double[] output = new double[data.Length];
            int current_index = 0;
            for (int i=0; i< data.Length; i++)
            {
                buffer[current_index] = data[i] / windowSize;
                double ma = 0;
                for (int j = 0; j < windowSize; j++)
                {
                    ma += buffer[j];
                }
                output[i] = ma;
                current_index = (current_index + 1) % windowSize;
            }
            return output;
        }
        public static int[] MovingAverage(int[] data, int windowSize)
        {
            int[] buffer = new int[windowSize];
            for (int i = 0; i < buffer.Length; i++) { buffer[i] = data[0] / windowSize; }
            int[] output = new int[data.Length];
            int current_index = 0;
            for (int i = 0; i < data.Length; i++)
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
            return output;
        }

    }
}
