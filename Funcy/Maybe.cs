using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public abstract class Maybe<T> : IMaybe<T>
    {
        public abstract bool IsSome { get; }
        bool IMaybe<T>.IsSome { get { return this.IsSome; } }
        public bool IsNone { get { return !this.IsSome; } }
        bool IMaybe<T>.IsNone { get { return this.IsNone; } }

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
            return (ISome<T>)this;
        }

        INone<T> IMaybe<T>.ToNone()
        {
            return (INone<T>)this;
        }
    }

    public class Some<T> : Maybe<T>, ISome<T>
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

        public override bool IsSome
        {
            get { return true; }
        }
    }

    public class None<T> : Maybe<T>, INone<T>
    {
        public None()
        {
        }

        public override bool IsSome
        {
            get { return false; }
        }
    }
}
