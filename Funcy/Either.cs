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
    public abstract class Either<TLeft, TRight> : IStructuralEquatable, IComputable<TRight>
    {
        public static Left<TLeft, TRight> Left(TLeft left)
        {
            return new Left<TLeft, TRight>(left);
        }

        public static Right<TLeft, TRight> Right(TRight right)
        {
            return new Right<TLeft, TRight>(right);
        }

        public Left<TLeft, TRight> ToLeft()
        {
            return (Left<TLeft, TRight>)this;
        }

        public Right<TLeft, TRight> ToRight()
        {
            return (Right<TLeft, TRight>)this;
        }
        
        public abstract bool IsRight { get; }

        public abstract bool IsLeft { get; }

        [Obsolete("This method is deprecated. Use FMap method.")]
        IComputable<TReturn> IComputable<TRight>.Compute<TReturn>(Func<TRight, TReturn> f)
        {
            return this.Compute<TReturn>(f);
        }
        [Obsolete("This method is deprecated. Use FMap method.")]
        public Either<TLeft, TReturn> Compute<TReturn>(Func<TRight, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }

        IComputable<TReturn> IComputable<TRight>.ComputeWith<TReturn>(Func<TRight, IComputable<TReturn>> f)
        {
            return this.ComputeWith(x => (Either<TLeft, TReturn>)f(x));
        }
        public abstract Either<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, Either<TLeft, TReturn>> f);

        IApplicative<TReturn> IApplicative<TRight>.Apply<TReturn>(IApplicative<Func<TRight, TReturn>> f)
        {
            return this.Apply<TReturn>((Either<TLeft, Func<TRight, TReturn>>)f);
        }
        public Either<TLeft, TReturn> Apply<TReturn>(Either<TLeft, Func<TRight, TReturn>> f)
        {
            if (f.IsRight)
            {
                return this.FMap<TReturn>(f.ToRight().Value);
            }
            else
            {
                return Either<TLeft, TReturn>.Left(f.ToLeft().Value);
            }
        }

        IApplicative<TRight> IApplicative<TRight>.ApplyLeft<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyLeft<TReturn>((Either<TLeft, TReturn>)other);
        }
        public Either<TLeft, TRight> ApplyLeft<TReturn>(Either<TLeft, TReturn> other)
        {
            return this;
        }

        IApplicative<TReturn> IApplicative<TRight>.ApplyRight<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyRight<TReturn>((Either<TLeft, TReturn>)other);
        }
        public Either<TLeft, TReturn> ApplyRight<TReturn>(Either<TLeft, TReturn> other)
        {
            return other;
        }

        IApplicative<TRight> IApplicative<TRight>.Point(TRight value)
        {
            return this.Point(value);
        }
        public Either<TLeft, TRight> Point(TRight value)
        {
            return Either<TLeft, TRight>.Right(value);
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

        IFunctor<TReturn> IFunctor<TRight>.FMap<TReturn>(Func<TRight, TReturn> f)
        {
            return this.FMap(f);
        }
        public abstract Either<TLeft, TReturn> FMap<TReturn>(Func<TRight, TReturn> f);
    }

    public class Left<TLeft, TRight> : Either<TLeft, TRight>, IExtractor<TLeft>
    {
        private TLeft value;
        public TLeft Value
        {
            get { return this.value; }
        }

        public Left(TLeft left)
        {
            this.value = left;
        }

        public override bool IsRight { get { return false; } }

        public override bool IsLeft { get { return !this.IsRight; } }

        public override Either<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, Either<TLeft, TReturn>> f)
        {
            return Either<TLeft, TReturn>.Left(this.value);
        }

        public TLeft Extract()
        {
            return this.value;
        }

        public override bool Equals(object other, System.Collections.IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is Left<TLeft, TRight>)
            {
                var left = (Left<TLeft, TRight>)other;
                return comparer.Equals(this.value, left.value);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this.value);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<TRight>.Default);
        }

        public override Either<TLeft, TReturn> FMap<TReturn>(Func<TRight, TReturn> f)
        {
            return Either<TLeft, TReturn>.Left(this.value);
        }
    }

    public class Right<TLeft, TRight> : Either<TLeft, TRight>, IExtractor<TRight>
    {
        private TRight value;
        public TRight Value
        {
            get { return this.value; }
        }

        public Right(TRight right)
        {
            this.value = right;
        }

        public override bool IsRight { get { return true; } }
        public override bool IsLeft { get { return !this.IsRight; } }

        public override Either<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, Either<TLeft, TReturn>> f)
        {
            return f(this.value);
        }

        public TRight Extract()
        {
            return this.value;
        }

        public override bool Equals(object other, System.Collections.IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is Right<TLeft, TRight>)
            {
                var right = (Right<TLeft, TRight>)other;
                return comparer.Equals(this.value, right.value);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this.value);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<TRight>.Default);
        }

        public override Either<TLeft, TReturn> FMap<TReturn>(Func<TRight, TReturn> f)
        {
            return Either<TLeft, TReturn>.Right(f(this.value));
        }
    }
}
