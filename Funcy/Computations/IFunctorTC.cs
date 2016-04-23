using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcy.Computations
{
    public interface IFunctorTC<TFunctor, TSource>
    {
        IFunctorTC<TFunctor,TReturn> FMap<TReturn>(Func<TSource, TReturn> f);
        IPointed<TFunctor> Pointed { get; }
    }

    public interface IPointed<TFunctor>
    {
        IFunctorTC<TFunctor, TSource> Point<TSource>(TSource value);
    }
}
