using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Funcy.Future.Computations;
using Funcy.Patterns;

namespace Funcy.Future
{
    public abstract class NonEmptyListTC<T> : IStructuralEquatable, IStructuralComparable, IEnumerable<T>, IComputableTC<NonEmptyListTC, T>
    {
        public abstract bool IsConsNEL { get; }
        public abstract bool IsSingleton { get; }

        public NonEmptyListTC()
        {
            this.Pointed = new NonEmptyListTC();
        }

        public ConsNELTC<T> ToConsNEL()
        {
            return (ConsNELTC<T>)this;
        }

        public SingletonTC<T> ToSingleton()
        {
            return (SingletonTC<T>)this;
        }

        IPointed<NonEmptyListTC> IFunctorTC<NonEmptyListTC, T>.Pointed { get { return this.Pointed; } }
        public NonEmptyListTC Pointed { get; private set; }

        IFunctorTC<NonEmptyListTC, TReturn> IFunctorTC<NonEmptyListTC, T>.FMap<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }
        public abstract NonEmptyListTC<TReturn> FMap<TReturn>(Func<T, TReturn> f);

        IApplicativeTC<NonEmptyListTC, TReturn> IApplicativeTC<NonEmptyListTC, T>.Apply<TReturn>(IApplicativeTC<NonEmptyListTC, Func<T, TReturn>> f)
        {
            return this.Apply((NonEmptyListTC<Func<T, TReturn>>)f);
        }
        public NonEmptyListTC<TReturn> Apply<TReturn>(NonEmptyListTC<Func<T, TReturn>> f)
        {
            return NonEmptyListTC.Construct(f.SelectMany(fElem => this.FMap(fElem)));
        }

        IApplicativeTC<NonEmptyListTC, T> IApplicativeTC<NonEmptyListTC, T>.ApplyLeft<TReturn>(IApplicativeTC<NonEmptyListTC, TReturn> other)
        {
            return ApplyLeft((NonEmptyListTC<TReturn>)other);
        }
        public NonEmptyListTC<T> ApplyLeft<TReturn>(NonEmptyListTC<TReturn> other)
        {
            return this;
        }

        IApplicativeTC<NonEmptyListTC, TReturn> IApplicativeTC<NonEmptyListTC, T>.ApplyRight<TReturn>(IApplicativeTC<NonEmptyListTC, TReturn> other)
        {
            return ApplyRight((NonEmptyListTC<TReturn>)other);
        }
        public NonEmptyListTC<TReturn> ApplyRight<TReturn>(NonEmptyListTC<TReturn> other)
        {
            return other;
        }

        IApplicativeTC<NonEmptyListTC, TReturn> IApplicativeTC<NonEmptyListTC, T>.FMapA<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }

        IComputableTC<NonEmptyListTC, TReturn> IComputableTC<NonEmptyListTC, T>.Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }

        IComputableTC<NonEmptyListTC, TReturn> IComputableTC<NonEmptyListTC, T>.ComputeWith<TReturn>(Func<T, IComputableTC<NonEmptyListTC, TReturn>> f)
        {
            return this.ComputeWith((Func<T, NonEmptyListTC<TReturn>>)f);
        }
        public NonEmptyListTC<TReturn> ComputeWith<TReturn>(Func<T, NonEmptyListTC<TReturn>> f)
        {
            return NonEmptyListTC.Construct(this.SelectMany(v => f(v)));
        }

        public IEnumerator<T> GetEnumerator()
        {
            var target = this;
            while (target.IsConsNEL)
            {
                var cons = target.ToConsNEL();
                yield return cons.Head;
                target = cons.Tail;
            }

            var singleton = target.ToSingleton();
            yield return singleton.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            return this.Equals(other, comparer);
        }
        public abstract bool Equals(object other, IEqualityComparer comparer);
        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<T>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return this.GetHashCode(comparer);
        }
        public abstract int GetHashCode(IEqualityComparer comparer);
        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return this.CompareTo(other, comparer);
        }
        public abstract int CompareTo(object other, IComparer comparer);
    }

    public class NonEmptyListTC : IPointed<NonEmptyListTC>
    {
        public static NonEmptyListTC<T> ConsNEL<T>(T head, NonEmptyListTC<T> tail)
        {
            return new ConsNELTC<T>(head, tail);
        }

        public static NonEmptyListTC<T> Singleton<T>(T value)
        {
            return new SingletonTC<T>(value);
        }

        public static NonEmptyListTC<T> Construct<T>(IEnumerable<T> sequence)
        {
            if (sequence == null || !sequence.Any())
                throw new ArgumentException("Argument 'sequence' is null or empty.");

            var enumerator = sequence.GetEnumerator();
            enumerator.MoveNext();
            return ConstructRecursively(enumerator);
        }

        private static NonEmptyListTC<T> ConstructRecursively<T>(IEnumerator<T> enumerator)
        {
            var current = enumerator.Current;
            if (enumerator.MoveNext())
                return ConsNEL(current, ConstructRecursively(enumerator));
            else
                return Singleton(current);
        }

        IFunctorTC<NonEmptyListTC, TSource> IPointed<NonEmptyListTC>.Point<TSource>(TSource value)
        {
            return Point(value);
        }
        public NonEmptyListTC<TSource>Point<TSource>(TSource value)
        {
            return Singleton(value);
        }
    }

    public sealed class ConsNELTC<T> : NonEmptyListTC<T>, IExtractor<Tuple<T,NonEmptyListTC<T>>>
    {
        public T Head { get; private set; }
        public NonEmptyListTC<T> Tail { get; private set; }

        public override bool IsConsNEL { get { return true; } }
        public override bool IsSingleton { get { return !this.IsConsNEL; } }

        public ConsNELTC(T head, NonEmptyListTC<T> tail)
        {
            this.Head = head;
            this.Tail = tail;
        }

        public override NonEmptyListTC<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return NonEmptyListTC.ConsNEL(f(this.Head), this.Tail.FMap(f));
        }

        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is ConsNELTC<T>)
            {
                var consNEL = (ConsNELTC<T>)other;
                return comparer.Equals(this.Head, consNEL.Head) && this.Tail.Equals(consNEL.Tail, comparer);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.Head);
            var h2 = this.Tail.GetHashCode(comparer);
            return ((h1 << 5) + h1) ^ h2;
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 0;
            if (other is ConsNELTC<T>)
            {
                var consNEL = (ConsNELTC<T>)other;
                int headComparisonResult = comparer.Compare(this.Head, consNEL.Head);
                if (headComparisonResult != 0)
                {
                    return headComparisonResult;
                }
                else
                {
                    return this.Tail.CompareTo(consNEL.Tail, comparer);
                }
            }
            else if (other is SingletonTC<T>)
            {
                var singleton = (SingletonTC<T>)other;
                int headComparisonResult = comparer.Compare(this.Head, singleton.Value);
                if (headComparisonResult != 0)
                {
                    return headComparisonResult;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                throw new ArgumentException("'other' is not NonEmptyListTC.");
            }
        }

        Tuple<T, NonEmptyListTC<T>> IExtractor<Tuple<T, NonEmptyListTC<T>>>.Extract()
        {
            return Tuple.Create(this.Head, this.Tail);
        }
    }

    public sealed class SingletonTC<T> : NonEmptyListTC<T>, IExtractor<T>
    {
        public T Value { get; private set; }

        public override bool IsConsNEL { get { return !this.IsSingleton; } }
        public override bool IsSingleton { get { return true; } }

        public SingletonTC(T value)
        {
            this.Value = value;
        }

        public override NonEmptyListTC<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return NonEmptyListTC.Singleton(f(this.Value));
        }

        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is SingletonTC<T>)
            {
                var singleton = (SingletonTC<T>)other;
                return comparer.Equals(this.Value, singleton.Value);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.Value);
            var h2 = 17;
            return ((h1 << 5) + h1) ^ h2;
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 0;
            if (other is ConsNELTC<T>)
            {
                var consNEL = (ConsNELTC<T>)other;
                int headComparisonResult = comparer.Compare(this.Value, consNEL.Head);
                if (headComparisonResult != 0)
                {
                    return headComparisonResult;
                }
                else
                {
                    return -1;
                }
            }
            else if (other is SingletonTC<T>)
            {
                var singleton = (SingletonTC<T>)other;
                return comparer.Compare(this.Value, singleton.Value);
            }
            else
            {
                throw new ArgumentException("'other' is not NonEmptyListTC.");
            }
        }

        T IExtractor<T>.Extract()
        {
            return this.Value;
        }
    }
}
