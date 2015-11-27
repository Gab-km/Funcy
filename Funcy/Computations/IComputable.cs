using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy.Computations
{
    public interface IComputable<TSource> : IApplicative<TSource>
    {
        [Obsolete("This method is deprecated. Use FMap method from IFunctor interface.")]
        IComputable<TReturn> Compute<TReturn>(Func<TSource, TReturn> f);
        IComputable<TReturn> ComputeWith<TReturn>(Func<TSource, IComputable<TReturn>> f);
    }
}
