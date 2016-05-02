using System;

namespace Funcy.Future.Computations
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
