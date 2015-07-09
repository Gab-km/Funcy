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

        public abstract ISome<T> ToSome();

        public abstract INone<T> ToNone();

        public abstract bool IsSome { get; }

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
            return this.Equals(other);
        }

        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            return this.GetHashCode();
        }
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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Some<T>)
            {
                var other = (Some<T>)obj;
                return this.value.Equals(other.value);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is None<T>)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }
    }
}
