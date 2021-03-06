﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using LiteDB;
using NodaTime;

namespace Trading
{
    public class CupHandle
    {
        [BsonId]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public Point K { get; set; }
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
        public Point D { get; set; }
        public Point Buy { get; set; }
        public bool BuyTrigger { get; set; }
        public double Gamma { get; set; }
        public float R1 { get; set; }
        public float R2 { get; set; }
        public float R3 { get; set; }
        
        public CupHandle()
        {
            K = new Point();
            A = new Point();
            B = new Point();
            C = new Point();
            D = new Point();
            Buy = new Point();
            BuyTrigger = false;
            Gamma = 0;
            R1 = 0;
            R2 = 0;
            R3 = 0;
        }

        public void SetC(Point c)
        {
            C = c;
            D = new Point();
            Buy = new Point();
            BuyTrigger = false;
            R1 = 0;
            R2 = 0;
            R3 = 0;
        }
        public void SetA(Point a)
        {
            A = a;
            B = new Point();
            C = new Point();
            D = new Point();
            K = new Point();
            Buy = new Point();
            BuyTrigger = false;
            R1 = 0;
            R2 = 0;
            R3 = 0;
        }
        public void SetB(Point b)
        {
            B = b;
            D = new Point();
            Buy = new Point();
            BuyTrigger = false;
        }

        public string Serialize()
        {
            byte[] bytes = ZeroFormatterSerializer.Serialize(this);
            return Encoding.UTF8.GetString(bytes);
        }
        public CupHandle(byte[] bytes)
        {
            if (bytes == null)
            {
                K = new Point();
                A = new Point();
                B = new Point();
                C = new Point();
                D = new Point();
                Buy = new Point();
                BuyTrigger = false;
                Gamma = 0;
                R1 = 0;
                R2 = 0;
                R3 = 0;
            }
            else
            {
                //byte[] bytes = Encoding.UTF8.GetBytes(data);
                CupHandle ch = ZeroFormatterSerializer.Deserialize<CupHandle>(bytes);
                K = ch.K;
                A = ch.A;
                B = ch.B;
                C = ch.C;
                D = ch.D;
                Gamma = ch.Gamma;
                R1 = ch.R1;
                R2 = ch.R2;
                R3 = ch.R3;
            }
        }
        public float GetRank()
        {
            return R1 + R2 + R3;
        }

        public float GetMinimumRank()
        {
            float min = R1;
            if (R2 < min)
                min = R2;
            if (R3 < min)
                min = R3;
            return min;
        }
    }

    [ZeroFormattable]
    public class CupHandleParameters
    {
        public Range<int> Setup { get; set; } //K-A
        public Range<int> CupLeft { get; set; } //A-B
        public Range<int> CupRight { get; set; } //B-C
        public Range<int> Handle { get; set; } //C-D
        public Range<int> AC { get; set; }
        public Range<float> PivotRatio { get; set; } //Pc/Pa
        public float BuyWait { get; set; }
        public float MinRank { get; set; }
        public float MinSingleRank { get; set; }
        public float CBdiffMin { get; set; }

        //public CupHandleParameters(Range<int> setup, Range<int> cupLeft, Range<int> cupRight, Range<int> handle, Range<float> pivotRatio, float minRank)
        //{
        //    Setup = setup;
        //    CupLeft = cupLeft;
        //    CupRight = cupRight;
        //    Handle = handle;
        //    AC = new Range<int>(cupLeft.Minimum + cupRight.Minimum, cupLeft.Maximum + cupRight.Maximum);
        //    BuyWait = 0.5f;
        //    MinRank = minRank;
        //    CBdiffMin = 0.5f;
        //    MinSingleRank = 0.0f;
        //}
        public string Serialize()
        {
            byte[] bytes = ZeroFormatterSerializer.Serialize(this);
            return Encoding.UTF8.GetString(bytes);
        }
        public CupHandleParameters(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            CupHandleParameters chp = ZeroFormatterSerializer.Deserialize<CupHandleParameters>(bytes);
            Setup = chp.Setup;
            CupLeft = chp.CupLeft;
            CupRight = chp.CupRight;
            Handle = chp.Handle;
            AC = chp.AC;
            PivotRatio = chp.PivotRatio;
            BuyWait = chp.BuyWait;
            CBdiffMin = chp.CBdiffMin;
            MinSingleRank = chp.MinSingleRank;
        }

        public CupHandleParameters(CupHandleDefinition ch)
        {
            if (ch == CupHandleDefinition.Haiku1)
            {
                Setup = new Range<int>(2, 30);
                CupLeft = new Range<int>(20, 120);
                CupRight = new Range<int>(3, 25);
                Handle = new Range<int>(2, 30);
                PivotRatio = new Range<float>(0.78f, 1.1f);
                AC = new Range<int>(CupLeft.Minimum + CupRight.Minimum, CupLeft.Maximum + CupRight.Maximum);
                BuyWait = 0.5f;
                MinRank = 8;
                MinSingleRank = 0.0f;
            }
            if (ch == CupHandleDefinition.Haiku3)
            {
                Setup = new Range<int>(2, 50);
                CupLeft = new Range<int>(20, 147);
                CupRight = new Range<int>(3, 25);
                Handle = new Range<int>(2, 30);
                PivotRatio = new Range<float>(0.78f, 1.1f);
                AC = new Range<int>(CupLeft.Minimum + CupRight.Minimum, CupLeft.Maximum + CupRight.Maximum);
                MinRank = 8;
                MinSingleRank = 0.0f;
            }
        }
    }

    public enum CupHandleDefinition
    {
        Haiku1,
        Haiku3
    };

    [ZeroFormattable]
    public class Point
    {
        [Index(0)]
        public virtual float Close { get; set; }
        [Index(1)]
        public virtual int Volume { get; set; }
        [Index(2)]
        public virtual LocalDate Date { get; set; }
        [Index(3)]
        public virtual int Index { get; set; }


        public Point(int index, float close, int volume, LocalDate date)
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
        public string Serialize()
        {
            byte[] bytes = ZeroFormatterSerializer.Serialize(this);
            return Encoding.UTF8.GetString(bytes);
        }
        public Point(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            Point p = ZeroFormatterSerializer.Deserialize<Point>(bytes);
            Close = p.Close;
            Volume = p.Volume;
            Date = p.Date;
            Index = p.Index;
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
