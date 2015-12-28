using Funcy.Computations;
using Funcy.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcy
{
    public abstract class NonEmptyList<T> : IStructuralEquatable, IStructuralComparable, IEnumerable<T>, IComputable<T>
    {
        public static ConsNEL<T> ConsNEL(T head, NonEmptyList<T> tail)
        {
            return new ConsNEL<T>(head, tail);
        }

        public static Singleton<T> Singleton(T value)
        {
            return new Singleton<T>(value);
        }

        public static NonEmptyList<T> Construct(IEnumerable<T> sequence)
        {
            if (sequence == null || !sequence.Any())
                throw new ArgumentException("Argument 'sequence' is null or empty.");

            var enumerator = sequence.GetEnumerator();
            enumerator.MoveNext();
            return ConstructRecursively(enumerator);
        }

        private static NonEmptyList<T> ConstructRecursively(IEnumerator<T> enumerator)
        {
            var current = enumerator.Current;
            if (enumerator.MoveNext())
                return NonEmptyList<T>.ConsNEL(current, ConstructRecursively(enumerator));
            else
                return NonEmptyList<T>.Singleton(current);
        }

        public ConsNEL<T> ToConsNEL()
        {
            return (ConsNEL<T>)this;
        }

        public Singleton<T> ToSingleton()
        {
            return (Singleton<T>)this;
        }

        public abstract bool IsConsNEL { get; }
        public abstract bool IsSingleton { get; }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            return this.Equals(other, comparer);
        }
        public abstract bool Equals(object other, System.Collections.IEqualityComparer comparer);

        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<T>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return this.GetHashCode(comparer);
        }
        public abstract int GetHashCode(System.Collections.IEqualityComparer comparer);
        
        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return this.CompareTo(other, comparer);
        }
        public abstract int CompareTo(object other, IComparer comparer);

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

        IFunctor<TReturn> IFunctor<T>.FMap<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }
        public abstract NonEmptyList<TReturn> FMap<TReturn>(Func<T, TReturn> f);

        IApplicative<TReturn> IApplicative<T>.Apply<TReturn>(IApplicative<Func<T, TReturn>> f)
        {
            return this.Apply<TReturn>((NonEmptyList<Func<T, TReturn>>)f);
        }
        public NonEmptyList<TReturn> Apply<TReturn>(NonEmptyList<Func<T, TReturn>> f)
        {
            return NonEmptyList<TReturn>.Construct(f.SelectMany(fElem => this.FMap(fElem)));
        }

        IApplicative<T> IApplicative<T>.ApplyLeft<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyLeft<TReturn>((NonEmptyList<TReturn>)other);
        }
        public NonEmptyList<T> ApplyLeft<TReturn>(NonEmptyList<TReturn> other)
        {
            return this;
        }

        IApplicative<TReturn> IApplicative<T>.ApplyRight<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyRight<TReturn>((NonEmptyList<TReturn>)other);
        }
        public NonEmptyList<TReturn> ApplyRight<TReturn>(NonEmptyList<TReturn> other)
        {
            return other;
        }

        IApplicative<T> IApplicative<T>.Point(T value)
        {
            return this.Point(value);
        }
        public NonEmptyList<T> Point(T value)
        {
            return NonEmptyList<T>.Singleton(value);
        }

        [Obsolete("This method is deprecated. Use FMap method.")]
        IComputable<TReturn> IComputable<T>.Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }

        IComputable<TReturn> IComputable<T>.ComputeWith<TReturn>(Func<T, IComputable<TReturn>> f)
        {
            return this.ComputeWith((Func<T, NonEmptyList<TReturn>>)f);
        }
        public NonEmptyList<TReturn> ComputeWith<TReturn>(Func<T, NonEmptyList<TReturn>> f)
        {
            return NonEmptyList<TReturn>.Construct(this.SelectMany(v => f(v)));
        }
    }

    public class ConsNEL<T> : NonEmptyList<T>, IExtractor<Tuple<T, NonEmptyList<T>>>
    {
        private T head;
        public T Head { get { return this.head; } }

        private NonEmptyList<T> tail;
        public NonEmptyList<T> Tail { get { return this.tail; } }

        public ConsNEL(T head, NonEmptyList<T> tail)
        {
            this.head = head;
            this.tail = tail;
        }

        public override bool IsConsNEL { get { return true; } }

        public override bool IsSingleton { get { return false; } }

        public override bool Equals(object obj, IEqualityComparer comparer)
        {
            if (obj == null) return false;
            if (obj is ConsNEL<T>)
            {
                var other = (ConsNEL<T>)obj;
                return comparer.Equals(this.head, other.head) && this.tail.Equals(other.tail, comparer);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            int hash = 17;
            hash = hash * 31 + comparer.GetHashCode(this.head);
            hash = hash * 31 + this.tail.GetHashCode(comparer);
            return hash;
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 0;
            if (other is ConsNEL<T>)
            {
                var cons = (ConsNEL<T>)other;
                int headComparisonResult = comparer.Compare(this.head, cons.head);
                if (headComparisonResult != 0)
                {
                    return headComparisonResult;
                }
                else
                {
                    return this.tail.CompareTo(cons.tail, comparer);
                }
            }
            if (other is Singleton<T>)
            {
                var singleton = (Singleton<T>)other;
                int headComparisonResult = comparer.Compare(this.head, singleton.Value);
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
                return 0;
            }
        }

        public override NonEmptyList<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return NonEmptyList<TReturn>.ConsNEL(f(this.head), this.tail.FMap(f));
        }

        public Tuple<T, NonEmptyList<T>> Extract()
        {
            return Tuple.Create<T, NonEmptyList<T>>(this.head, this.tail);
        }
    }

    public class Singleton<T> : NonEmptyList<T>, IExtractor<T>
    {
        private T value;
        public T Value { get { return this.value; } }

        public Singleton(T value)
        {
            this.value = value;
        }

        public override bool IsConsNEL { get { return false; } }

        public override bool IsSingleton { get { return true; } }

        public override bool Equals(object obj, IEqualityComparer comparer)
        {
            if (obj == null) return false;
            if (obj is Singleton<T>)
            {
                var other = (Singleton<T>)obj;
                return comparer.Equals(this.value, other.value);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            int hash = 17;
            return hash * 31 + comparer.GetHashCode(this.value);
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 0;
            if (other is ConsNEL<T>)
            {
                var cons = (ConsNEL<T>)other;
                int headComparisonResult = comparer.Compare(this.value, cons.Head);
                if (headComparisonResult != 0)
                {
                    return headComparisonResult;
                }
                else
                {
                    return -1;
                }
            }
            else if (other is Singleton<T>)
            {
                var singleton = (Singleton<T>)other;
                return comparer.Compare(this.value, singleton.value);
            }
            else
            {
                return comparer.Compare(this, other);
            }
        }

        public override NonEmptyList<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return NonEmptyList<TReturn>.Singleton(f(this.value));
        }

        public T Extract()
        {
            return this.value;
        }
    }

    public static class NonEmptyListNT
    {
        public static FuncyList<T> ToFuncyList<T>(this NonEmptyList<T> self)
        {
            return FuncyList<T>.Construct(self.ToArray());
        }

        public static FuncyList<T> Take<T>(this NonEmptyList<T> self, int length)
        {
            return NonEmptyListNT.ToFuncyList(self).Take(length);
        }

        public static T TakeFirst<T>(this NonEmptyList<T> self)
        {
            var enumerator = self.GetEnumerator();
            enumerator.MoveNext();

            return enumerator.Current;
        }
    }
}
