using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy.Computations
{
    public interface IFunctor<TSource>
    {
        IFunctor<TReturn> FMap<TReturn>(Func<TSource, TReturn> f);
    }
}
