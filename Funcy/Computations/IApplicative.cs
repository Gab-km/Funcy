using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy.Computations
{
    public interface IApplicative<TSource> : IFunctor<TSource>
    {
        IApplicative<TReturn> Apply<TReturn>(IApplicative<Func<TSource, TReturn>> f);
        IApplicative<TSource> ApplyLeft<TReturn>(IApplicative<TReturn> other);
        IApplicative<TReturn> ApplyRight<TReturn>(IApplicative<TReturn> other);
        IApplicative<TSource> Point(TSource value);
    }
}
