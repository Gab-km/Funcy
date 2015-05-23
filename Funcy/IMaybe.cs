using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public interface IMaybe<T>
    {
        bool IsSome { get; }
        bool IsNone { get; }
        ISome<T> ToSome();
        INone<T> ToNone();
    }

    public interface ISome<T> : IMaybe<T>
    {
        T Value { get; }
    }

    public interface INone<T> : IMaybe<T>
    {
    }
}
