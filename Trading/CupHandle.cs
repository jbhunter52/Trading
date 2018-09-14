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

        public CupHandle(CupHandleParameters params)
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
        }
    }

    public class CupHandleParameters
    {
        public Range<int> Setup;
        public Range<int> CupLeft;
        public Range<int> CupRight;
        public Range<int> Handle;

        public CupHandleParameters(Range<int> setup, Range<int> cupLeft, Range<int> cupRight, Range<int> handle)
        {
            this.Setup = setup;
            this.CupLeft = cupLeft;
            this.CupRight = cupRight;
            this.Handle = handle;
        }
    }

    public class Point
    {
        public double Close;
        public int Volume;
        public DateTime Date;
        public int Index;


        public Point(int index, double close, int volume, DateTime date)
        {
            this.Index = index;
            this.Close = close;
            this.Volume = volume;
            this.Date = date;
        }
        public Point()
        {
            this.Index = -1;
        }
    }

    /// <summary>The Range class.</summary>
    /// <typeparam name="T">Generic parameter.</typeparam>
    public class Range<T> where T : IComparable<T>
    {
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
