using Funcy.Computations;
using Funcy.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public abstract class Maybe<T> : IStructuralEquatable, IStructuralComparable, IComputable<T>
    {
        public static Some<T> Some(T value)
        {
            return new Some<T>(value);
        }

        public static None<T> None()
        {
            return new None<T>();
        }

        public Some<T> ToSome()
        {
            return (Some<T>)this;
        }
        public None<T> ToNone()
        {
            return (None<T>)this;
        }

        public abstract bool IsSome { get; }
        public abstract bool IsNone { get; }

        [Obsolete("This method is deprecated. Use FMap method.")]
        IComputable<TReturn> IComputable<T>.Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }

        IComputable<TReturn> IComputable<T>.ComputeWith<TReturn>(Func<T, IComputable<TReturn>> f)
        {
            return this.ComputeWith(x => (Maybe<TReturn>)f(x));
        }
        public abstract Maybe<TReturn> ComputeWith<TReturn>(Func<T, Maybe<TReturn>> f);

        IApplicative<TReturn> IApplicative<T>.Apply<TReturn>(IApplicative<Func<T, TReturn>> f)
        {
            return this.Apply<TReturn>((Maybe<Func<T, TReturn>>)f);
        }
        public Maybe<TReturn> Apply<TReturn>(Maybe<Func<T, TReturn>> f)
        {
            if (f.IsSome)
            {
                return this.FMap<TReturn>(f.ToSome().Value);
            }
            else
            {
                return Maybe<TReturn>.None();
            }
        }

        IApplicative<T> IApplicative<T>.ApplyLeft<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyLeft<TReturn>((Maybe<TReturn>)other);
        }
        public Maybe<T> ApplyLeft<TReturn>(Maybe<TReturn> other)
        {
            return this;
        }

        IApplicative<TReturn> IApplicative<T>.ApplyRight<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyRight<TReturn>((Maybe<TReturn>)other);
        }
        public Maybe<TReturn> ApplyRight<TReturn>(Maybe<TReturn> other)
        {
            return other;
        }

        IApplicative<T> IApplicative<T>.Point(T value)
        {
            return this.Point(value);
        }
        public Maybe<T> Point(T value)
        {
            return Maybe<T>.Some(value);
        }
        
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer)
        {
            return this.Equals(other, comparer);
        }
        public abstract bool Equals(object other, System.Collections.IEqualityComparer comparer);

        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            return this.GetHashCode(comparer);
        }
        public abstract int GetHashCode(System.Collections.IEqualityComparer comparer);

        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer)
        {
            return this.CompareTo(other, comparer);
        }
        public abstract int CompareTo(object other, System.Collections.IComparer comparer);

        IFunctor<TReturn> IFunctor<T>.FMap<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }
        public abstract Maybe<TReturn> FMap<TReturn>(Func<T, TReturn> f);
    }

    public class Some<T> : Maybe<T>, IExtractor<T>
    {
        private T value;
        public T Value
        {
            get { return this.value; }
        }

        public Some(T value)
        {
            this.value = value;
        }

        public override bool IsSome { get { return true; } }

        public override bool IsNone { get { return !this.IsSome; } }

        public override Maybe<TReturn> ComputeWith<TReturn>(Func<T, Maybe<TReturn>> f)
        {
            return f(this.value);
        }

        public T Extract()
        {
            return this.value;
        }

        public override bool Equals(object obj, System.Collections.IEqualityComparer comparer)
        {
            if (obj == null) return false;
            if (obj is Some<T>)
            {
                var other = (Some<T>)obj;
                return comparer.Equals(this.value, other.value);
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

        public override int GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this.value);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        public override int CompareTo(object other, System.Collections.IComparer comparer)
        {
            if (other == null) return 0;
            if (other is Some<T>)
            {
                var some = (Some<T>)other;
                return comparer.Compare(this.value, some.value);
            }
            else if (other is None<T>)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override Maybe<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return Maybe<TReturn>.Some(f(this.value));
        }
    }

    public class None<T> : Maybe<T>
    {
        public None()
        {
        }

        public override bool IsSome { get { return false; } }

        public override bool IsNone { get { return !this.IsSome; } }

        public override Maybe<TReturn> ComputeWith<TReturn>(Func<T, Maybe<TReturn>> f)
        {
            return Maybe<TReturn>.None();
        }

        public override bool Equals(object other, System.Collections.IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is None<T>)
            {
                var none = (None<T>)other;
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

        public override int GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            return comparer.GetHashCode();
        }
        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        public override int CompareTo(object other, System.Collections.IComparer comparer)
        {
            if (other == null) return 0;
            if (other is Some<T>)
            {
                return -1;
            }
            else if (other is None<T>)
            {
                var none = (None<T>)other;
                return comparer.Compare(this.GetType().DeclaringType, none.GetType().DeclaringType);
            }
            else
            {
                return comparer.Compare(this, other);
            }
        }

        public override Maybe<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return Maybe<TReturn>.None();
        }
    }
}
