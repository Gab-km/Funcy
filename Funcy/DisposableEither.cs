using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public abstract class DisposableEither1<TLeft, TRight> : IEither<TLeft, TRight>, IDisposable where TLeft : IDisposable
    {
        public abstract bool IsLeft { get; }
        bool IEither<TLeft, TRight>.IsLeft { get { return this.IsLeft; } }
        bool IEither<TLeft, TRight>.IsRight { get { return !this.IsLeft; } }

        public abstract void Dispose();
        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        ILeft<TLeft, TRight> IEither<TLeft, TRight>.ToLeft()
        {
            return (ILeft<TLeft, TRight>)this;
        }

        IRight<TLeft, TRight> IEither<TLeft, TRight>.ToRight()
        {
            return (IRight<TLeft, TRight>)this;
        }

        public static DisposableEither1<TLeft, TRight> Left(TLeft left)
        {
            return new DisposableLeft1<TLeft, TRight>(left);
        }

        public static DisposableEither1<TLeft, TRight> Right(TRight right)
        {
            return new DisposableRight1<TLeft, TRight>(right);
        }
    }

    public class DisposableLeft1<TLeft, TRight> : DisposableEither1<TLeft, TRight>, ILeft<TLeft, TRight> where TLeft : IDisposable
    {
        private TLeft value;
        TLeft ILeft<TLeft, TRight>.Value
        {
            get { return this.value; }
        }

        public DisposableLeft1(TLeft left)
        {
            this.value = left;
        }

        public override bool IsLeft
        {
            get
            {
                return true;
            }
        }

        public override void Dispose()
        {
            if (this.value != null)
            {
                this.value.Dispose();
                this.value = default(TLeft);
            }
        }
    }

    public class DisposableRight1<TLeft, TRight> : DisposableEither1<TLeft, TRight>, IRight<TLeft, TRight> where TLeft : IDisposable
    {
        private TRight value;
        TRight IRight<TLeft, TRight>.Value
        {
            get { return this.value; }
        }

        public DisposableRight1(TRight right)
        {
            this.value = right;
        }

        public override bool IsLeft
        {
            get
            {
                return false;
            }
        }

        public override void Dispose()
        {
        }
    }


    public abstract class DisposableEither2<TLeft, TRight> : IEither<TLeft, TRight>, IDisposable where TRight : IDisposable
    {
        public abstract bool IsLeft { get; }
        bool IEither<TLeft, TRight>.IsLeft { get { return this.IsLeft; } }
        bool IEither<TLeft, TRight>.IsRight { get { return !this.IsLeft; } }

        public abstract void Dispose();
        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        ILeft<TLeft, TRight> IEither<TLeft, TRight>.ToLeft()
        {
            return (ILeft<TLeft, TRight>)this;
        }

        IRight<TLeft, TRight> IEither<TLeft, TRight>.ToRight()
        {
            return (IRight<TLeft, TRight>)this;
        }

        public static DisposableEither2<TLeft, TRight> Left(TLeft left)
        {
            return new DisposableLeft2<TLeft, TRight>(left);
        }

        public static DisposableEither2<TLeft, TRight> Right(TRight right)
        {
            return new DisposableRight2<TLeft, TRight>(right);
        }
    }

    public class DisposableLeft2<TLeft, TRight> : DisposableEither2<TLeft, TRight>, ILeft<TLeft, TRight> where TRight : IDisposable
    {
        private TLeft value;
        TLeft ILeft<TLeft, TRight>.Value
        {
            get { return this.value; }
        }

        public DisposableLeft2(TLeft left)
        {
            this.value = left;
        }

        public override bool IsLeft
        {
            get
            {
                return true;
            }
        }

        public override void Dispose()
        {
        }
    }

    public class DisposableRight2<TLeft, TRight> : DisposableEither2<TLeft, TRight>, IRight<TLeft, TRight> where TRight : IDisposable
    {
        private TRight value;
        TRight IRight<TLeft, TRight>.Value
        {
            get { return this.value; }
        }

        public DisposableRight2(TRight right)
        {
            this.value = right;
        }

        public override bool IsLeft
        {
            get
            {
                return false;
            }
        }

        public override void Dispose()
        {
            if (this.value != null)
            {
                this.value.Dispose();
                this.value = default(TRight);
            }
        }
    }


    public abstract class DisposableEither3<TLeft, TRight> : IEither<TLeft, TRight>, IDisposable
        where TLeft : IDisposable
        where TRight : IDisposable
    {
        public abstract bool IsLeft { get; }
        bool IEither<TLeft, TRight>.IsLeft { get { return this.IsLeft; } }
        bool IEither<TLeft, TRight>.IsRight { get { return !this.IsLeft; } }

        public abstract void Dispose();
        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        ILeft<TLeft, TRight> IEither<TLeft, TRight>.ToLeft()
        {
            return (ILeft<TLeft, TRight>)this;
        }

        IRight<TLeft, TRight> IEither<TLeft, TRight>.ToRight()
        {
            return (IRight<TLeft, TRight>)this;
        }

        public static DisposableEither3<TLeft, TRight> Left(TLeft left)
        {
            return new DisposableLeft3<TLeft, TRight>(left);
        }

        public static DisposableEither3<TLeft, TRight> Right(TRight right)
        {
            return new DisposableRight3<TLeft, TRight>(right);
        }
    }

    public class DisposableLeft3<TLeft, TRight> : DisposableEither3<TLeft, TRight>, ILeft<TLeft, TRight>
        where TLeft : IDisposable
        where TRight : IDisposable
    {
        private TLeft value;
        TLeft ILeft<TLeft, TRight>.Value
        {
            get { return this.value; }
        }

        public DisposableLeft3(TLeft left)
        {
            this.value = left;
        }

        public override bool IsLeft
        {
            get
            {
                return true;
            }
        }

        public override void Dispose()
        {
            if (this.value != null)
            {
                this.value.Dispose();
                this.value = default(TLeft);
            }
        }
    }

    public class DisposableRight3<TLeft, TRight> : DisposableEither3<TLeft, TRight>, IRight<TLeft, TRight>
        where TLeft : IDisposable
        where TRight : IDisposable
    {
        private TRight value;
        TRight IRight<TLeft, TRight>.Value
        {
            get { return this.value; }
        }

        public DisposableRight3(TRight right)
        {
            this.value = right;
        }

        public override bool IsLeft
        {
            get
            {
                return false;
            }
        }

        public override void Dispose()
        {
            if (this.value != null)
            {
                this.value.Dispose();
                this.value = default(TRight);
            }
        }
    }
}
