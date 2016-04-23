using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcy.Computations
{
    public interface IComputableTC<TComputable, TSource> : IApplicativeTC<TComputable, TSource> where TComputable : IPointed<TComputable>
    {
        IComputableTC<TComputable, TReturn> Compute<TReturn>(Func<TSource, TReturn> f);
        IComputableTC<TComputable, TReturn> ComputeWith<TReturn>(Func<TSource, IComputableTC<TComputable, TReturn>> f);
    }
}
