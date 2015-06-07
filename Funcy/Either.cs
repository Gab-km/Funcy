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

        public abstract bool IsRight { get; }
        bool IEither<TLeft, TRight>.IsRight { get { return this.IsRight; } }

        public bool IsLeft { get { return !this.IsRight; } }
        bool IEither<TLeft, TRight>.IsLeft { get { return this.IsLeft; } }

        ILeft<TLeft, TRight> IEither<TLeft, TRight>.ToLeft()
        {
            return (ILeft<TLeft, TRight>)this;
        }

        IRight<TLeft, TRight> IEither<TLeft, TRight>.ToRight()
        {
            return (IRight<TLeft, TRight>)this;
        }
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

        public override bool IsRight
        {
            get
            {
                return false;
            }
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

        public override bool IsRight
        {
            get
            {
                return true;
            }
        }
    }
}
