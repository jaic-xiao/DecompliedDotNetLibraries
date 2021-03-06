﻿namespace System.Data.Common
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Xml;

    internal sealed class DoubleStorage : DataStorage
    {
        private const double defaultValue = 0.0;
        private double[] values;

        internal DoubleStorage(DataColumn column) : base(column, typeof(double), 0.0)
        {
        }

        public override object Aggregate(int[] records, AggregateType kind)
        {
            bool flag = false;
            try
            {
                double num;
                double num2;
                int num3;
                int num4;
                int num5;
                int num6;
                int num7;
                int num8;
                int num9;
                double minValue;
                double maxValue;
                double num12;
                int num13;
                double num14;
                double num15;
                int num16;
                int num17;
                int[] numArray;
                double num18;
                int num19;
                int[] numArray2;
                int num20;
                int[] numArray3;
                switch (kind)
                {
                    case AggregateType.Sum:
                        num15 = 0.0;
                        numArray3 = records;
                        num9 = 0;
                        goto Label_006D;

                    case AggregateType.Mean:
                        num14 = 0.0;
                        num13 = 0;
                        numArray2 = records;
                        num8 = 0;
                        goto Label_00D5;

                    case AggregateType.Min:
                        maxValue = double.MaxValue;
                        num5 = 0;
                        goto Label_0232;

                    case AggregateType.Max:
                        minValue = double.MinValue;
                        num4 = 0;
                        goto Label_028F;

                    case AggregateType.First:
                        if (records.Length <= 0)
                        {
                            return null;
                        }
                        return this.values[records[0]];

                    case AggregateType.Count:
                        return base.Aggregate(records, kind);

                    case AggregateType.Var:
                    case AggregateType.StDev:
                        num3 = 0;
                        num = 0.0;
                        num18 = 0.0;
                        num2 = 0.0;
                        num12 = 0.0;
                        numArray = records;
                        num7 = 0;
                        goto Label_017B;

                    default:
                        goto Label_02E8;
                }
            Label_0046:
                num20 = numArray3[num9];
                if (!this.IsNull(num20))
                {
                    num15 += this.values[num20];
                    flag = true;
                }
                num9++;
            Label_006D:
                if (num9 < numArray3.Length)
                {
                    goto Label_0046;
                }
                if (flag)
                {
                    return num15;
                }
                return base.NullValue;
            Label_00A7:
                num19 = numArray2[num8];
                if (!this.IsNull(num19))
                {
                    num14 += this.values[num19];
                    num13++;
                    flag = true;
                }
                num8++;
            Label_00D5:
                if (num8 < numArray2.Length)
                {
                    goto Label_00A7;
                }
                if (flag)
                {
                    return (num14 / ((double) num13));
                }
                return base.NullValue;
            Label_0137:
                num6 = numArray[num7];
                if (!this.IsNull(num6))
                {
                    num2 += this.values[num6];
                    num12 += this.values[num6] * this.values[num6];
                    num3++;
                }
                num7++;
            Label_017B:
                if (num7 < numArray.Length)
                {
                    goto Label_0137;
                }
                if (num3 <= 1)
                {
                    return base.NullValue;
                }
                num = (num3 * num12) - (num2 * num2);
                num18 = num / (num2 * num2);
                if ((num18 < 1E-15) || (num < 0.0))
                {
                    num = 0.0;
                }
                else
                {
                    num /= (double) (num3 * (num3 - 1));
                }
                if (kind == AggregateType.StDev)
                {
                    return Math.Sqrt(num);
                }
                return num;
            Label_0208:
                num17 = records[num5];
                if (!this.IsNull(num17))
                {
                    maxValue = Math.Min(this.values[num17], maxValue);
                    flag = true;
                }
                num5++;
            Label_0232:
                if (num5 < records.Length)
                {
                    goto Label_0208;
                }
                if (flag)
                {
                    return maxValue;
                }
                return base.NullValue;
            Label_0265:
                num16 = records[num4];
                if (!this.IsNull(num16))
                {
                    minValue = Math.Max(this.values[num16], minValue);
                    flag = true;
                }
                num4++;
            Label_028F:
                if (num4 < records.Length)
                {
                    goto Label_0265;
                }
                if (flag)
                {
                    return minValue;
                }
                return base.NullValue;
            }
            catch (OverflowException)
            {
                throw ExprException.Overflow(typeof(double));
            }
        Label_02E8:
            throw ExceptionBuilder.AggregateException(kind, base.DataType);
        }

        public override int Compare(int recordNo1, int recordNo2)
        {
            double num3 = this.values[recordNo1];
            double num2 = this.values[recordNo2];
            if ((num3 == 0.0) || (num2 == 0.0))
            {
                int num = base.CompareBits(recordNo1, recordNo2);
                if (num != 0)
                {
                    return num;
                }
            }
            return num3.CompareTo(num2);
        }

        public override int CompareValueTo(int recordNo, object value)
        {
            if (base.NullValue == value)
            {
                if (this.IsNull(recordNo))
                {
                    return 0;
                }
                return 1;
            }
            double num = this.values[recordNo];
            if ((0.0 == num) && this.IsNull(recordNo))
            {
                return -1;
            }
            return num.CompareTo((double) value);
        }

        public override string ConvertObjectToXml(object value)
        {
            return XmlConvert.ToString((double) value);
        }

        public override object ConvertValue(object value)
        {
            if (base.NullValue != value)
            {
                if (value != null)
                {
                    value = ((IConvertible) value).ToDouble(base.FormatProvider);
                    return value;
                }
                value = base.NullValue;
            }
            return value;
        }

        public override object ConvertXmlToObject(string s)
        {
            return XmlConvert.ToDouble(s);
        }

        public override void Copy(int recordNo1, int recordNo2)
        {
            base.CopyBits(recordNo1, recordNo2);
            this.values[recordNo2] = this.values[recordNo1];
        }

        protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
        {
            double[] numArray = (double[]) store;
            numArray[storeIndex] = this.values[record];
            nullbits.Set(storeIndex, this.IsNull(record));
        }

        public override object Get(int record)
        {
            double num = this.values[record];
            if (num != 0.0)
            {
                return num;
            }
            return base.GetBits(record);
        }

        protected override object GetEmptyStorage(int recordCount)
        {
            return new double[recordCount];
        }

        public override void Set(int record, object value)
        {
            if (base.NullValue == value)
            {
                this.values[record] = 0.0;
                base.SetNullBit(record, true);
            }
            else
            {
                this.values[record] = ((IConvertible) value).ToDouble(base.FormatProvider);
                base.SetNullBit(record, false);
            }
        }

        public override void SetCapacity(int capacity)
        {
            double[] destinationArray = new double[capacity];
            if (this.values != null)
            {
                Array.Copy(this.values, 0, destinationArray, 0, Math.Min(capacity, this.values.Length));
            }
            this.values = destinationArray;
            base.SetCapacity(capacity);
        }

        protected override void SetStorage(object store, BitArray nullbits)
        {
            this.values = (double[]) store;
            base.SetNullStorage(nullbits);
        }
    }
}

