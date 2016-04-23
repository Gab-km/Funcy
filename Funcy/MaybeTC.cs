using System;
using System.Collections;
using System.Collections.Generic;
using Funcy.Computations;
using Funcy.Patterns;

namespace Funcy
{
    public abstract class MaybeTC<T> : IStructuralEquatable, IStructuralComparable, IComputableTC<MaybeTC, T>
    {
        public abstract bool IsSome { get; }
        public abstract bool IsNone { get; }

        public SomeTC<T> ToSome()
        {
            return (SomeTC<T>)this;
        }

        public NoneTC<T> ToNone()
        {
            return (NoneTC<T>)this;
        }

        IPointed<MaybeTC> IFunctorTC<MaybeTC, T>.Pointed { get { return this.Pointed; } }
        public MaybeTC Pointed { get { return new MaybeTC(); } }

        IFunctorTC<MaybeTC, TReturn> IFunctorTC<MaybeTC, T>.FMap<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }
        public abstract MaybeTC<TReturn> FMap<TReturn>(Func<T, TReturn> f);

        IApplicativeTC<MaybeTC, TReturn> IApplicativeTC<MaybeTC, T>.Apply<TReturn>(IApplicativeTC<MaybeTC, Func<T, TReturn>> f)
        {
            return this.Apply<TReturn>((MaybeTC<Func<T, TReturn >>)f);
        }
        public MaybeTC<TReturn>Apply<TReturn>(MaybeTC<Func<T,TReturn>>f)
        {
            if (f.IsSome)
            {
                return this.FMap<TReturn>(f.ToSome().Value);
            }
            else
            {
                return MaybeTC.None<TReturn>();
            }
        }

        IApplicativeTC<MaybeTC, T> IApplicativeTC<MaybeTC, T>.ApplyLeft<TReturn>(IApplicativeTC<MaybeTC, TReturn> other)
        {
            return this.ApplyLeft<TReturn>((MaybeTC<TReturn >)other);
        }
        public MaybeTC<T> ApplyLeft<TReturn>(MaybeTC<TReturn> other)
        {
            return this;
        }

        IApplicativeTC<MaybeTC, TReturn> IApplicativeTC<MaybeTC, T>.ApplyRight<TReturn>(IApplicativeTC<MaybeTC, TReturn> other)
        {
            return this.ApplyRight<TReturn>((MaybeTC<TReturn>)other);
        }
        public MaybeTC<TReturn> ApplyRight<TReturn>(MaybeTC<TReturn> other)
        {
            return other;
        }

        IApplicativeTC<MaybeTC, TReturn> IApplicativeTC<MaybeTC, T>.FMapA<TReturn>(Func<T, TReturn> f)
        {
            return this.FMapA(f);
        }
        public MaybeTC<TReturn> FMapA<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }

        IComputableTC<MaybeTC, TReturn> IComputableTC<MaybeTC, T>.Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.Compute(f);
        }
        public MaybeTC<TReturn> Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }

        IComputableTC<MaybeTC, TReturn> IComputableTC<MaybeTC, T>.ComputeWith<TReturn>(Func<T, IComputableTC<MaybeTC, TReturn>> f)
        {
            return this.ComputeWith<TReturn>(x => (MaybeTC<TReturn>)(f(x)));
        }
        public abstract MaybeTC<TReturn> ComputeWith<TReturn>(Func<T, MaybeTC<TReturn>> f);

        public abstract bool Equals(object other, IEqualityComparer comparer);
        public abstract int GetHashCode(IEqualityComparer comparer);
        public abstract int CompareTo(object other, IComparer comparer);
    }

    public class MaybeTC : IPointed<MaybeTC>
    {
        public static MaybeTC<TSource> Some<TSource>(TSource value)
        {
            return new SomeTC<TSource>(value);
        }

        public static MaybeTC<TSource> None<TSource>()
        {
            return new NoneTC<TSource>();
        }

        IFunctorTC<MaybeTC, TSource> IPointed<MaybeTC>.Point<TSource>(TSource value)
        {
            return this.Point<TSource>(value);
        }
        public MaybeTC<TSource> Point<TSource>(TSource value)
        {
            return MaybeTC.Some<TSource>(value);
        }
    }

    public sealed class SomeTC<T> : MaybeTC<T>, IExtractor<T>
    {
        public T Value { get; private set; }

        public SomeTC(T value)
        {
            this.Value = value;
        }

        public override bool IsNone { get { return !this.IsSome; } }
        public override bool IsSome { get { return true; } }

        public override MaybeTC<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return new SomeTC<TReturn>(f(this.Value));
        }

        public override MaybeTC<TReturn> ComputeWith<TReturn>(Func<T, MaybeTC<TReturn>> f)
        {
            return f(this.Value);
        }

        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is SomeTC<T>)
            {
                var some = (SomeTC<T>)other;
                return comparer.Equals(this.Value, some.Value);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<T>.Default);
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this.Value);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 1;
            if (other is SomeTC<T>)
            {
                var some = (SomeTC<T>)other;
                return comparer.Compare(this.Value, some.Value);
            }
            else if (other is NoneTC<T>)
            {
                return 1;
            }
            else
            {
                throw new ArgumentException("'other' is neither instance of SomeTC nor NoneTC.");
            }
        }

        T IExtractor<T>.Extract()
        {
            return this.Value;
        }
    }

    public sealed class NoneTC<T> : MaybeTC<T>
    {
        public override bool IsNone { get { return true; } }
        public override bool IsSome { get { return !this.IsNone; } }

        public override MaybeTC<TReturn> ComputeWith<TReturn>(Func<T, MaybeTC<TReturn>> f)
        {
            return MaybeTC.None<TReturn>();
        }

        public override MaybeTC<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return MaybeTC.None<TReturn>();
        }

        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is NoneTC<T>)
            {
                var none = (NoneTC<T>)other;
                return comparer.Equals(this.GetType().DeclaringType, none.GetType().DeclaringType);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<T>.Default);
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 1;
            if (other is SomeTC<T>)
            {
                return -1;
            }
            else if (other is NoneTC<T>)
            {
                var none = (None<T>)other;
                return comparer.Compare(this.GetType().DeclaringType, none.GetType().DeclaringType);
            }
            else
            {
                throw new ArgumentException("'other' is neither instance of SomeTC nor NoneTC.");
            }
        }
    }
}
