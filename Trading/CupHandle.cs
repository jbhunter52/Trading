using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    public class CupHandle
    {
        public List<double> Close;
        public List<int> Volume;
        public List<DateTime> Date;

        public Point K;
        public Point A;
        public Point B;
        public Point C;
        public Point D;
        public double Gamma;
        public CupHandleParameters Parameters;

        public CupHandle(CupHandleParameters parameters)
        {
            Close = new List<double>();
            Volume = new List<int>();
            Date = new List<DateTime>();
            K = new Point();
            A = new Point();
            B = new Point();
            C = new Point();
            D = new Point();
            Gamma = 0;
            Parameters = parameters;
        }
    }

    public class CupHandleParameters
    {
        public Range<int> Setup; //K-A
        public Range<int> CupLeft; //A-B
        public Range<int> CupRight; //B-C
        public Range<int> Handle; //C-D
        public Range<double> PivotRatio; //Pc/Pa

        public CupHandleParameters(Range<int> setup, Range<int> cupLeft, Range<int> cupRight, Range<int> handle, Range<double> pivotRatio)
        {
            Setup = setup;
            CupLeft = cupLeft;
            CupRight = cupRight;
            Handle = handle;
        }

        public CupHandleParameters(CupHandleDefinition ch)
        {
            if (ch == CupHandleDefinition.Haiku1)
            {
                Setup = new Range<int>(2, 30);
                CupLeft = new Range<int>(20, 120);
                CupRight = new Range<int>(3, 25);
                Handle = new Range<int>(2, 30);
            }
            if (ch == CupHandleDefinition.Haiku3)
            {
                Setup = new Range<int>(2, 50);
                CupLeft = new Range<int>(20, 147);
                CupRight = new Range<int>(3, 25);
                Handle = new Range<int>(2, 30);
                PivotRatio = new Range<double>(0.78, 1.1);
            }

        }
    }

    public enum CupHandleDefinition
    {
        Haiku1,
        Haiku3
    };

    public class Point
    {
        public double Close;
        public int Volume;
        public DateTime Date;
        public int Index;


        public Point(int index, double close, int volume, DateTime date)
        {
            Index = index;
            Close = close;
            Volume = volume;
            Date = date;
        }
        public Point()
        {
            Index = -1;
        }
    }

    /// <summary>The Range class.</summary>
    /// <typeparam name="T">Generic parameter.</typeparam>
    public class Range<T> where T : IComparable<T>
    {
        public Range(T min, T max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }
        /// <summary>Minimum value of the range.</summary>
        public T Minimum { get; set; }

        /// <summary>Maximum value of the range.</summary>
        public T Maximum { get; set; }

        /// <summary>Presents the Range in readable format.</summary>
        /// <returns>String representation of the Range</returns>
        public override string ToString()
        {
            return string.Format("[{0} - {1}]", this.Minimum, this.Maximum);
        }

        /// <summary>Determines if the range is valid.</summary>
        /// <returns>True if range is valid, else false</returns>
        public bool IsValid()
        {
            return this.Minimum.CompareTo(this.Maximum) <= 0;
        }

        /// <summary>Determines if the provided value is inside the range.</summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is inside Range, else false</returns>
        public bool ContainsValue(T value)
        {
            return (this.Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);
        }

        /// <summary>Determines if this Range is inside the bounds of another range.</summary>
        /// <param name="Range">The parent range to test on</param>
        /// <returns>True if range is inclusive, else false</returns>
        public bool IsInsideRange(Range<T> range)
        {
            return this.IsValid() && range.IsValid() && range.ContainsValue(this.Minimum) && range.ContainsValue(this.Maximum);
        }

        /// <summary>Determines if another range is inside the bounds of this range.</summary>
        /// <param name="Range">The child range to test</param>
        /// <returns>True if range is inside, else false</returns>
        public bool ContainsRange(Range<T> range)
        {
            return this.IsValid() && range.IsValid() && this.ContainsValue(range.Minimum) && this.ContainsValue(range.Maximum);
        }
    }

}
