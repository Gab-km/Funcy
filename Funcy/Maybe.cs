using Funcy.Computations;
using Funcy.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public abstract class Maybe<T> : IMaybe<T>
    {
        public static Some<T> Some(T value)
        {
            return new Some<T>(value);
        }

        public static None<T> None()
        {
            return new None<T>();
        }
        
        ISome<T> IMaybe<T>.ToSome()
        {
            return this.ToSome();
        }
        public abstract ISome<T> ToSome();
        
        INone<T> IMaybe<T>.ToNone()
        {
            return this.ToNone();
        }
        public abstract INone<T> ToNone();

        bool IMaybe<T>.IsSome
        {
            get { return this.IsSome; }
        }
        public abstract bool IsSome { get; }
        
        bool IMaybe<T>.IsNone
        {
            get { return this.IsNone; }
        }
        public abstract bool IsNone { get; }

        IComputable<TReturn> IComputable<T>.Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.Compute(f);
        }
        public abstract IMaybe<TReturn> Compute<TReturn>(Func<T, TReturn> f);

        IComputable<TReturn> IComputable<T>.ComputeWith<TReturn>(Func<T, IComputable<TReturn>> f)
        {
            return this.ComputeWith((Func<T, IMaybe<TReturn>>)f);
        }
        public abstract IMaybe<TReturn> ComputeWith<TReturn>(Func<T, IMaybe<TReturn>> f);

        IApplicative<TReturn> IApplicative<T>.Apply<TReturn>(IApplicative<Func<T, TReturn>> f)
        {
            return this.Apply<TReturn>((IMaybe<Func<T, TReturn>>)f);
        }
        public abstract IMaybe<TReturn> Apply<TReturn>(IMaybe<Func<T, TReturn>> f);

        IApplicative<T> IApplicative<T>.ApplyLeft<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyLeft<TReturn>((IMaybe<TReturn>)other);
        }
        public IMaybe<T> ApplyLeft<TReturn>(IMaybe<TReturn> other)
        {
            return this;
        }

        IApplicative<TReturn> IApplicative<T>.ApplyRight<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyRight<TReturn>((IMaybe<TReturn>)other);
        }
        public IMaybe<TReturn> ApplyRight<TReturn>(IMaybe<TReturn> other)
        {
            return other;
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
        public abstract IMaybe<TReturn> FMap<TReturn>(Func<T, TReturn> f);
    }

    public class Some<T> : Maybe<T>, ISome<T>, IExtractor<T>
    {
        private T value;
        T ISome<T>.Value
        {
            get { return this.value; }
        }

        public Some(T value)
        {
            this.value = value;
        }

        public override bool IsSome { get { return true; } }

        public override bool IsNone { get { return !this.IsSome; } }

        public override ISome<T> ToSome()
        {
            return this;
        }

        public override INone<T> ToNone()
        {
            return (INone<T>)this;
        }

        public override IMaybe<TReturn> Compute<TReturn>(Func<T, TReturn> f)
        {
            return Maybe<TReturn>.Some(f(this.value));
        }

        public override IMaybe<TReturn> ComputeWith<TReturn>(Func<T, IMaybe<TReturn>> f)
        {
            return f(this.value);
        }

        public T Extract()
        {
            return this.value;
        }

        public override IMaybe<TReturn> Apply<TReturn>(IMaybe<Func<T, TReturn>> f)
        {
            if (f.IsSome)
            {
                return Maybe<TReturn>.Some(f.ToSome().Value(this.value));
            }
            else
            {
                return Maybe<TReturn>.None();
            }
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

        public override IMaybe<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return Maybe<TReturn>.Some(f(this.value));
        }
    }

    public class None<T> : Maybe<T>, INone<T>
    {
        public None()
        {
        }

        public override bool IsSome { get { return false; } }

        public override bool IsNone { get { return !this.IsSome; } }

        public override ISome<T> ToSome()
        {
            return (ISome<T>)this;
        }

        public override INone<T> ToNone()
        {
            return this;
        }

        public override IMaybe<TReturn> Compute<TReturn>(Func<T, TReturn> f)
        {
            return Maybe<TReturn>.None();
        }

        public override IMaybe<TReturn> ComputeWith<TReturn>(Func<T, IMaybe<TReturn>> f)
        {
            return Maybe<TReturn>.None();
        }

        public override IMaybe<TReturn> Apply<TReturn>(IMaybe<Func<T, TReturn>> f)
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

        public override IMaybe<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return Maybe<TReturn>.None();
        }
    }
}
