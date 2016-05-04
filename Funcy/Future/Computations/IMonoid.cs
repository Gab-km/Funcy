using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcy.Future.Computations
{
    public interface IMonoid<TSource>
    {
        TSource MEmpty { get; }
        TSource MAppend(TSource other);
    }

    public abstract class MonoidalSByte : IMonoid<sbyte>
    {
        public sbyte Value { get; private set; }

        public MonoidalSByte(sbyte value)
        {
            this.Value = value;
        }

        sbyte IMonoid<sbyte>.MEmpty { get { return this.MEmpty; } }
        public abstract sbyte MEmpty { get; }

        sbyte IMonoid<sbyte>.MAppend(sbyte other)
        {
            return this.MAppend(other);
        }
        public abstract sbyte MAppend(sbyte other);
    }

    public class MonoidalSumSByte : MonoidalSByte
    {
        public MonoidalSumSByte(sbyte value) : base(value) { }

        public override sbyte MEmpty { get { return 0; } }

        public override sbyte MAppend(sbyte other)
        {
            return (sbyte)(this.Value + other);
        }
    }

    public class MonoidalProductSByte : MonoidalSByte
    {
        public MonoidalProductSByte(sbyte value) : base(value) { }

        public override sbyte MEmpty { get { return 1; } }

        public override sbyte MAppend(sbyte other)
        {
            return (sbyte)(this.Value * other);
        }
    }

    public abstract class MonoidalInt16 : IMonoid<short>
    {
        public short Value { get; private set; }

        public MonoidalInt16(short value)
        {
            this.Value = value;
        }

        short IMonoid<short>.MEmpty { get { return this.MEmpty; } }
        public abstract short MEmpty { get; }

        short IMonoid<short>.MAppend(short other)
        {
            return this.MAppend(other);
        }
        public abstract short MAppend(short other);
    }

    public class MonoidalSumInt16 : MonoidalInt16
    {
        public MonoidalSumInt16(short value) : base(value) { }

        public override short MEmpty { get { return 0; } }

        public override short MAppend(short other)
        {
            return (short)(this.Value + other);
        }
    }

    public class MonoidalProductInt16 : MonoidalInt16
    {
        public MonoidalProductInt16(short value) : base(value) { }

        public override short MEmpty { get { return 1; } }

        public override short MAppend(short other)
        {
            return (short)(this.Value * other);
        }
    }

    public abstract class MonoidalInt32 : IMonoid<int>
    {
        public int Value { get; private set; }

        public MonoidalInt32(int value)
        {
            this.Value = value;
        }

        int IMonoid<int>.MEmpty { get { return this.MEmpty; } }
        public abstract int MEmpty { get; }

        int IMonoid<int>.MAppend(int other)
        {
            return this.MAppend(other);
        }
        public abstract int MAppend(int other);
    }

    public class MonoidalSumInt32 : MonoidalInt32
    {
        public MonoidalSumInt32(int value) : base(value) { }

        public override int MEmpty { get { return 0; } }

        public override int MAppend(int other)
        {
            return this.Value + other;
        }
    }

    public class MonoidalProductInt32 : MonoidalInt32
    {
        public MonoidalProductInt32(int value) : base(value) { }

        public override int MEmpty { get { return 1; } }

        public override int MAppend(int other)
        {
            return this.Value * other;
        }
    }

    public abstract class MonoidalInt64 : IMonoid<long>
    {
        public long Value { get; private set; }

        public MonoidalInt64(long value)
        {
            this.Value = value;
        }

        long IMonoid<long>.MEmpty { get { return this.MEmpty; } }
        public abstract long MEmpty{ get; }

        long IMonoid<long>.MAppend(long other)
        {
            return this.MAppend(other);
        }
        public abstract long MAppend(long other);
    }

    public class MonoidalSumInt64 : MonoidalInt64
    {
        public MonoidalSumInt64(long value) : base(value) { }

        public override long MEmpty { get { return 0L; } }

        public override long MAppend(long other)
        {
            return this.Value + other;
        }
    }

    public class MonoidalProductInt64 : MonoidalInt64
    {
        public MonoidalProductInt64(long value) : base(value) { }

        public override long MEmpty { get { return 1L; } }

        public override long MAppend(long other)
        {
            return this.Value * other;
        }
    }

    public abstract class MonoidalByte : IMonoid<byte>
    {
        public byte Value { get; private set; }

        public MonoidalByte(byte value)
        {
            this.Value = value;
        }

        byte IMonoid<byte>.MEmpty { get { return this.MEmpty; } }
        public abstract byte MEmpty { get; }

        byte IMonoid<byte>.MAppend(byte other)
        {
            return this.MAppend(other);
        }
        public abstract byte MAppend(byte other);
    }

    public class MonoidalSumByte : MonoidalByte
    {
        public MonoidalSumByte(byte value) : base(value) { }

        public override byte MEmpty { get { return 0; } }

        public override byte MAppend(byte other)
        {
            return (byte)(Value + other);
        }
    }

    public class MonoidalProductByte : MonoidalByte
    {
        public MonoidalProductByte(byte value) : base(value) { }

        public override byte MEmpty { get { return 1; } }

        public override byte MAppend(byte other)
        {
            return (byte)(this.Value * other);
        }
    }

    public abstract class MonoidalUInt16 : IMonoid<ushort>
    {
        public ushort Value { get; private set; }

        public MonoidalUInt16(ushort value)
        {
            this.Value = value;
        }

        ushort IMonoid<ushort>.MEmpty { get { return this.MEmpty; } }
        public abstract ushort MEmpty { get; }

        ushort IMonoid<ushort>.MAppend(ushort other)
        {
            return this.MAppend(other);
        }
        public abstract ushort MAppend(ushort other);
    }

    public class MonoidalSumUInt16 : MonoidalUInt16
    {
        public MonoidalSumUInt16(ushort value) : base(value) { }

        public override ushort MEmpty { get { return 0; } }

        public override ushort MAppend(ushort other)
        {
            return (ushort)(this.Value + other);
        }
    }

    public class MonoidalProductUInt16 : MonoidalUInt16
    {
        public MonoidalProductUInt16(ushort value) : base(value) { }

        public override ushort MEmpty { get { return 1; } }

        public override ushort MAppend(ushort other)
        {
            return (ushort)(this.Value * other);
        }
    }

    public abstract class MonoidalUInt32 : IMonoid<uint>
    {
        public uint Value { get; private set; }

        public MonoidalUInt32(uint value)
        {
            this.Value = value;
        }

        uint IMonoid<uint>.MEmpty { get { return this.MEmpty; } }
        public abstract uint MEmpty { get; }

        uint IMonoid<uint>.MAppend(uint other)
        {
            return this.MAppend(other);
        }
        public abstract uint MAppend(uint other);
    }

    public class MonoidalSumUInt32 : MonoidalUInt32
    {
        public MonoidalSumUInt32(uint value) : base(value) { }

        public override uint MEmpty { get { return 0U; } }

        public override uint MAppend(uint other)
        {
            return Value + other;
        }
    }

    public class MonoidalProductUInt32 : MonoidalUInt32
    {
        public MonoidalProductUInt32(uint value) : base(value) { }

        public override uint MEmpty { get { return 1U; } }

        public override uint MAppend(uint other)
        {
            return this.Value * other;
        }
    }

    public abstract class MonoidalUInt64 : IMonoid<ulong>
    {
        public ulong Value { get; private set; }

        public MonoidalUInt64(ulong value)
        {
            this.Value = value;
        }

        ulong IMonoid<ulong>.MEmpty { get { return this.MEmpty; } }
        public abstract ulong MEmpty { get; }

        ulong IMonoid<ulong>.MAppend(ulong other)
        {
            return this.MAppend(other);
        }
        public abstract ulong MAppend(ulong other);
    }

    public class MonoidalSumUInt64 : MonoidalUInt64
    {
        public MonoidalSumUInt64(ulong value) : base(value) { }

        public override ulong MEmpty { get { return 0UL; } }

        public override ulong MAppend(ulong other)
        {
            return this.Value + other;
        }
    }

    public class MonoidalProductUInt64 : MonoidalUInt64
    {
        public MonoidalProductUInt64(ulong value) : base(value) { }

        public override ulong MEmpty { get { return 1UL; } }

        public override ulong MAppend(ulong other)
        {
            return this.Value * other;
        }
    }

    public abstract class MonoidalSingle : IMonoid<float>
    {
        public float Value { get; private set; }

        public MonoidalSingle(float value)
        {
            this.Value = value;
        }

        float IMonoid<float>.MEmpty { get { return this.MEmpty; } }
        public abstract float MEmpty { get; }

        float IMonoid<float>.MAppend(float other)
        {
            return this.MAppend(other);
        }
        public abstract float MAppend(float other);
    }

    public class MonoidalSumSingle : MonoidalSingle
    {
        public MonoidalSumSingle(float value) : base(value) { }

        public override float MEmpty { get { return 0.0F; } }

        public override float MAppend(float other)
        {
            return this.Value + other;
        }
    }

    public class MonoidalProductSingle : MonoidalSingle
    {
        public MonoidalProductSingle(float value) : base(value) { }

        public override float MEmpty { get { return 1.0F; } }

        public override float MAppend(float other)
        {
            return this.Value * other;
        }
    }

    public abstract class MonoidalDouble : IMonoid<double>
    {
        public double Value { get; private set; }

        public MonoidalDouble(double value)
        {
            this.Value = value;
        }

        double IMonoid<double>.MEmpty { get { return this.MEmpty; } }
        public abstract double MEmpty { get; }

        double IMonoid<double>.MAppend(double other)
        {
            return this.MAppend(other);
        }
        public abstract double MAppend(double other);
    }

    public class MonoidalSumDouble : MonoidalDouble
    {
        public MonoidalSumDouble(double value) : base(value) { }

        public override double MEmpty { get { return 0.0; } }

        public override double MAppend(double other)
        {
            return this.Value + other;
        }
    }

    public class MonoidalProductDouble : MonoidalDouble
    {
        public MonoidalProductDouble(double value) : base(value) { }

        public override double MEmpty { get { return 1.0; } }

        public override double MAppend(double other)
        {
            return this.Value * other;
        }
    }

    public static class MonoidalExtensions
    {
        public static MonoidalSumSByte ToMonoidalSum(this sbyte value)
        {
            return new MonoidalSumSByte(value);
        }

        public static MonoidalSumInt16 ToMonoidalSum(this short value)
        {
            return new MonoidalSumInt16(value);
        }

        public static MonoidalSumInt32 ToMonoidalSum(this int value)
        {
            return new MonoidalSumInt32(value);
        }

        public static MonoidalSumInt64 ToMonoidalSum(this long value)
        {
            return new MonoidalSumInt64(value);
        }

        public static MonoidalSumByte ToMonoidalSum(this byte value)
        {
            return new MonoidalSumByte(value);
        }

        public static MonoidalSumUInt16 ToMonoidalSum(this ushort value)
        {
            return new MonoidalSumUInt16(value);
        }

        public static MonoidalSumUInt32 ToMonoidalSum(this uint value)
        {
            return new MonoidalSumUInt32(value);
        }

        public static MonoidalSumUInt64 ToMonoidalSum(this ulong value)
        {
            return new MonoidalSumUInt64(value);
        }

        public static MonoidalSumSingle ToMonoidalSum(this float value)
        {
            return new MonoidalSumSingle(value);
        }

        public static MonoidalSumDouble ToMonoidalSum(this double value)
        {
            return new MonoidalSumDouble(value);
        }

        public static MonoidalProductSByte ToMonoidalProduct(this sbyte value)
        {
            return new MonoidalProductSByte(value);
        }

        public static MonoidalProductInt16 ToMonoidalProduct(this short value)
        {
            return new MonoidalProductInt16(value);
        }

        public static MonoidalProductInt32 ToMonoidalProduct(this int value)
        {
            return new MonoidalProductInt32(value);
        }

        public static MonoidalProductInt64 ToMonoidalProduct(this long value)
        {
            return new MonoidalProductInt64(value);
        }

        public static MonoidalProductByte ToMonoidalProduct(this byte value)
        {
            return new MonoidalProductByte(value);
        }

        public static MonoidalProductUInt16 ToMonoidalProduct(this ushort value)
        {
            return new MonoidalProductUInt16(value);
        }

        public static MonoidalProductUInt32 ToMonoidalProduct(this uint value)
        {
            return new MonoidalProductUInt32(value);
        }

        public static MonoidalProductUInt64 ToMonoidalProduct(this ulong value)
        {
            return new MonoidalProductUInt64(value);
        }

        public static MonoidalProductSingle ToMonoidalProduct(this float value)
        {
            return new MonoidalProductSingle(value);
        }

        public static MonoidalProductDouble ToMonoidalProduct(this double value)
        {
            return new MonoidalProductDouble(value);
        }
    }
}
