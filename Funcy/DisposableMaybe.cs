using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public abstract class DisposableMaybe<T> : IMaybe<T>, IDisposable where T : IDisposable
    {
        public abstract bool IsSome { get; }
        bool IMaybe<T>.IsSome
        {
            get { return this.IsSome; }
        }
        bool IMaybe<T>.IsNone { get { return !this.IsSome; } }

        public static DisposableSome<T> Some(T value)
        {
            return new DisposableSome<T>(value);
        }

        public static DisposableNone<T> None()
        {
            return new DisposableNone<T>();
        }

        public abstract void Dispose();
        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        ISome<T> IMaybe<T>.ToSome()
        {
            return (ISome<T>)this;
        }

        INone<T> IMaybe<T>.ToNone()
        {
            return (INone<T>)this;
        }
    }

    public class DisposableSome<T> : DisposableMaybe<T>, ISome<T> where T : IDisposable
    {
        private T value;
        T ISome<T>.Value
        {
            get { return this.value; }
        }

        public DisposableSome(T value)
        {
            this.value = value;
        }

        public override bool IsSome
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
                this.value = default(T);
            }
        }
    }

    public class DisposableNone<T> : DisposableMaybe<T>, INone<T> where T : IDisposable
    {
        public DisposableNone()
        {
        }

        public override bool IsSome
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
}
