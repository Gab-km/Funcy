using Funcy.Computations;
using Funcy.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public abstract class Either<TLeft, TRight> : IEither<TLeft, TRight>
    {
        public static Left<TLeft, TRight> Left(TLeft left)
        {
            return new Left<TLeft, TRight>(left);
        }

        public static Right<TLeft, TRight> Right(TRight right)
        {
            return new Right<TLeft, TRight>(right);
        }

        public abstract ILeft<TLeft, TRight> ToLeft();

        public abstract IRight<TLeft, TRight> ToRight();

        public abstract bool IsRight { get; }

        public abstract bool IsLeft { get; }

        IComputable<TReturn> IComputable<TRight>.Compute<TReturn>(Func<TRight, TReturn> f)
        {
            return this.Compute(f);
        }
        public abstract IEither<TLeft, TReturn> Compute<TReturn>(Func<TRight, TReturn> f);

        IComputable<TReturn> IComputable<TRight>.ComputeWith<TReturn>(Func<TRight, IComputable<TReturn>> f)
        {
            return this.ComputeWith((Func<TRight, IEither<TLeft, TReturn>>)f);
        }
        public abstract IEither<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, IEither<TLeft, TReturn>> f);

        IApplicative<TReturn> IApplicative<TRight>.Apply<TReturn>(IApplicative<Func<TRight, TReturn>> f)
        {
            return this.Apply<TReturn>((IEither<TLeft, Func<TRight, TReturn>>)f);
        }
        public abstract IEither<TLeft, TReturn> Apply<TReturn>(IEither<TLeft, Func<TRight, TReturn>> f);

        IApplicative<TRight> IApplicative<TRight>.ApplyLeft<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyLeft<TReturn>((IEither<TLeft, TReturn>)other);
        }
        public IEither<TLeft, TRight> ApplyLeft<TReturn>(IEither<TLeft, TReturn> other)
        {
            return this;
        }

        IApplicative<TReturn> IApplicative<TRight>.ApplyRight<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyRight<TReturn>((IEither<TLeft, TReturn>)other);
        }
        public IEither<TLeft, TReturn> ApplyRight<TReturn>(IEither<TLeft, TReturn> other)
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
    }

    public class Left<TLeft, TRight> : Either<TLeft, TRight>, ILeft<TLeft, TRight>, IExtractor<TLeft>
    {
        private TLeft value;
        TLeft ILeft<TLeft, TRight>.Value
        {
            get { return this.value; }
        }

        public Left(TLeft left)
        {
            this.value = left;
        }

        public override bool IsRight { get { return false; } }

        public override bool IsLeft { get { return !this.IsRight; } }

        public override ILeft<TLeft, TRight> ToLeft()
        {
            return this;
        }

        public override IRight<TLeft, TRight> ToRight()
        {
            return (IRight<TLeft, TRight>)this;
        }
        
        public override IEither<TLeft, TReturn> Compute<TReturn>(Func<TRight, TReturn> f)
        {
            return Either<TLeft, TReturn>.Left(this.value);
        }

        public override IEither<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, IEither<TLeft, TReturn>> f)
        {
            return Either<TLeft, TReturn>.Left(this.value);
        }

        public TLeft Extract()
        {
            return this.value;
        }

        public override IEither<TLeft, TReturn> Apply<TReturn>(IEither<TLeft, Func<TRight, TReturn>> f)
        {
            return Either<TLeft, TReturn>.Left(this.value);
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
    }

    public class Right<TLeft, TRight> : Either<TLeft, TRight>, IRight<TLeft, TRight>, IExtractor<TRight>
    {
        private TRight value;
        TRight IRight<TLeft, TRight>.Value
        {
            get { return this.value; }
        }

        public Right(TRight right)
        {
            this.value = right;
        }

        public override bool IsRight { get { return true; } }
        public override bool IsLeft { get { return !this.IsRight; } }

        public override IRight<TLeft, TRight> ToRight()
        {
            return this;
        }

        public override ILeft<TLeft, TRight> ToLeft()
        {
            return (ILeft<TLeft, TRight>)this;
        }

        public override IEither<TLeft, TReturn> Compute<TReturn>(Func<TRight, TReturn> f)
        {
            return Either<TLeft, TReturn>.Right(f(this.value));
        }

        public override IEither<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, IEither<TLeft, TReturn>> f)
        {
            return f(this.value);
        }

        public TRight Extract()
        {
            return this.value;
        }

        public override IEither<TLeft, TReturn> Apply<TReturn>(IEither<TLeft, Func<TRight, TReturn>> f)
        {
            if (f.IsRight)
            {
                return Either<TLeft, TReturn>.Right(f.ToRight().Value(this.value));
            }
            else
            {
                return Either<TLeft, TReturn>.Left(f.ToLeft().Value);
            }
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
    }
}
