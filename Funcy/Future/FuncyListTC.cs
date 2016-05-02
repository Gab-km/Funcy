using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Funcy.Future.Computations;
using Funcy.Patterns;

namespace Funcy.Future
{
    public abstract class FuncyListTC<T> : IStructuralEquatable, IStructuralComparable, IEnumerable<T>, IComputableTC<FuncyListTC, T>
    {
        public abstract bool IsCons { get; }
        public abstract bool IsNil { get; }

        public FuncyListTC()
        {
            this.Pointed = new FuncyListTC();
        }

        public ConsTC<T> ToCons()
        {
            return (ConsTC<T>)this;
        }

        public NilTC<T> ToNil()
        {
            return (NilTC<T>)this;
        }

        IPointed<FuncyListTC> IFunctorTC<FuncyListTC, T>.Pointed { get { return this.Pointed; } }
        public FuncyListTC Pointed { get; private set; }

        IFunctorTC<FuncyListTC, TReturn> IFunctorTC<FuncyListTC, T>.FMap<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }
        public abstract FuncyListTC<TReturn> FMap<TReturn>(Func<T, TReturn> f);

        IApplicativeTC<FuncyListTC, TReturn> IApplicativeTC<FuncyListTC, T>.Apply<TReturn>(IApplicativeTC<FuncyListTC, Func<T, TReturn>> f)
        {
            return this.Apply((FuncyListTC<Func<T, TReturn>>)f);
        }
        public FuncyListTC<TReturn> Apply<TReturn>(FuncyListTC<Func<T, TReturn>> f)
        {
            if (f.IsCons)
            {
                return FuncyListTC.Construct(f.SelectMany(fCons => this.FMap(fCons)).ToArray());
            }
            else
            {
                return FuncyListTC.Nil<TReturn>();
            }
        }

        IApplicativeTC<FuncyListTC, T> IApplicativeTC<FuncyListTC, T>.ApplyLeft<TReturn>(IApplicativeTC<FuncyListTC, TReturn> other)
        {
            return this.ApplyLeft((FuncyListTC<TReturn>)other);
        }
        public FuncyListTC<T> ApplyLeft<TReturn>(FuncyListTC<TReturn> other)
        {
            return this;
        }

        IApplicativeTC<FuncyListTC, TReturn> IApplicativeTC<FuncyListTC, T>.ApplyRight<TReturn>(IApplicativeTC<FuncyListTC, TReturn> other)
        {
            return this.ApplyRight((FuncyListTC<TReturn>)other);
        }
        public FuncyListTC<TReturn> ApplyRight<TReturn>(FuncyListTC<TReturn> other)
        {
            return other;
        }

        IApplicativeTC<FuncyListTC, TReturn> IApplicativeTC<FuncyListTC, T>.FMapA<TReturn>(Func<T, TReturn> f)
        {
            return this.FMapA(f);
        }
        public FuncyListTC<TReturn> FMapA<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }

        IComputableTC<FuncyListTC, TReturn> IComputableTC<FuncyListTC, T>.Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.Compute(f);
        }
        public FuncyListTC<TReturn> Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }

        IComputableTC<FuncyListTC, TReturn> IComputableTC<FuncyListTC, T>.ComputeWith<TReturn>(Func<T, IComputableTC<FuncyListTC, TReturn>> f)
        {
            return this.ComputeWith((Func<T, FuncyListTC<TReturn>>)f);
        }
        public abstract FuncyListTC<TReturn> ComputeWith<TReturn>(Func<T, FuncyListTC<TReturn>> f);
        public abstract bool Equals(object other, IEqualityComparer comparer);
        public abstract int GetHashCode(IEqualityComparer comparer);
        public abstract int CompareTo(object other, IComparer comparer);

        public IEnumerator<T> GetEnumerator()
        {
            if (this.IsCons)
            {
                var target = this;
                do
                {
                    var cons = target.ToCons();
                    yield return cons.Head;
                    target = cons.Tail;
                } while (target.IsCons);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class FuncyListTC : IPointed<FuncyListTC>
    {
        public static FuncyListTC<TSource> Cons<TSource>(TSource head, FuncyListTC<TSource> tail)
        {
            return new ConsTC<TSource>(head, tail);
        }

        public static FuncyListTC<TSource> Nil<TSource>()
        {
            return new NilTC<TSource>();
        }

        public static FuncyListTC<TSource> Construct<TSource>(params TSource[] args)
        {
            FuncyListTC<TSource> result = FuncyListTC.Nil<TSource>();

            for (int i = args.Length - 1; i >= 0; i--)
            {
                result = Cons(args[i], result);
            }

            return result;
        }

        IFunctorTC<FuncyListTC, TSource> IPointed<FuncyListTC>.Point<TSource>(TSource value)
        {
            return this.Point(value);
        }
        public FuncyListTC<TSource> Point<TSource>(TSource value)
        {
            return Cons(value, Nil<TSource>());
        }
    }

    public sealed class ConsTC<T> : FuncyListTC<T>, IExtractor<Tuple<T, FuncyListTC<T>>>
    {
        public T Head { get; private set; }
        public FuncyListTC<T> Tail { get; private set; }

        public override bool IsCons { get { return true; } }
        public override bool IsNil { get { return !this.IsCons; } }

        public ConsTC(T head, FuncyListTC<T> tail)
        {
            this.Head = head;
            this.Tail = tail;
        }

        public override FuncyListTC<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return FuncyListTC.Cons(f(this.Head), this.Tail.FMap(f));
        }

        public override FuncyListTC<TReturn> ComputeWith<TReturn>(Func<T, FuncyListTC<TReturn>> f)
        {
            return FuncyListTC.Construct(this.SelectMany(h => f(h)).ToArray());
        }

        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is ConsTC<T>)
            {
                var cons = (ConsTC<T>)other;
                return comparer.Equals(this.Head, cons.Head) && this.Tail.Equals(cons.Tail, comparer);
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
            var h1 = comparer.GetHashCode(this.Head);
            var h2 = this.Tail.GetHashCode(comparer);
            return ((h1 << 5) + h1) ^ h2;
        }
        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 0;
            if (other is ConsTC<T>)
            {
                var cons = (ConsTC<T>)other;
                int headComparisonResult = comparer.Compare(this.Head, cons.Head);
                if (headComparisonResult != 0)
                {
                    return headComparisonResult;
                }
                else
                {
                    return this.Tail.CompareTo(cons.Tail, comparer);
                }
            }
            if (other is NilTC<T>)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        Tuple<T, FuncyListTC<T>> IExtractor<Tuple<T, FuncyListTC<T>>>.Extract()
        {
            return Tuple.Create<T, FuncyListTC<T>>(this.Head, this.Tail);
        }
    }

    public sealed class NilTC<T> : FuncyListTC<T>
    {
        public override bool IsCons { get { return !this.IsNil; } }
        public override bool IsNil { get { return true; } }

        public override FuncyListTC<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return FuncyListTC.Nil<TReturn>();
        }

        public override FuncyListTC<TReturn> ComputeWith<TReturn>(Func<T, FuncyListTC<TReturn>> f)
        {
            return FuncyListTC.Nil<TReturn>();
        }

        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is NilTC<T>)
            {
                var nil = (NilTC<T>)other;
                return comparer.Equals(this.GetType().DeclaringType, nil.GetType().DeclaringType);
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
            if (other == null) return 0;
            if (other is ConsTC<T>)
            {
                return -1;
            }
            else if (other is NilTC<T>)
            {
                var nil = (NilTC<T>)other;
                return comparer.Compare(this.GetType().DeclaringType, nil.GetType().DeclaringType);
            }
            else
            {
                return comparer.Compare(this, other);
            }
        }
    }
}
