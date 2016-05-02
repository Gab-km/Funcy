using System;

namespace Funcy.Future.Computations
{
    public interface IComputableTC<TComputable, TSource> : IApplicativeTC<TComputable, TSource> where TComputable : IPointed<TComputable>
    {
        IComputableTC<TComputable, TReturn> Compute<TReturn>(Func<TSource, TReturn> f);
        IComputableTC<TComputable, TReturn> ComputeWith<TReturn>(Func<TSource, IComputableTC<TComputable, TReturn>> f);
    }
}
