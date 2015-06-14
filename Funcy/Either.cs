using Funcy.Computations;
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
            return this.ComputeWith(f);
        }
        public abstract IEither<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, IComputable<TReturn>> f);
    }

    public class Left<TLeft, TRight> : Either<TLeft, TRight>, ILeft<TLeft, TRight>
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

        public override IEither<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, IComputable<TReturn>> f)
        {
            return Either<TLeft, TReturn>.Left(this.value);
        }
    }

    public class Right<TLeft, TRight> : Either<TLeft, TRight>, IRight<TLeft, TRight>
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

        public override IEither<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, IComputable<TReturn>> f)
        {
            return (IEither<TLeft, TReturn>)f(this.value);
        }
    }
}
