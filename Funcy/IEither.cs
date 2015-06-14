using Funcy.Computations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public interface IEither<TLeft, TRight> : IComputable<TRight>
    {
        bool IsLeft { get; }
        bool IsRight { get; }
        ILeft<TLeft, TRight> ToLeft();
        IRight<TLeft, TRight> ToRight();
    }

    public interface ILeft<TLeft, TRight> : IEither<TLeft, TRight>
    {
        TLeft Value { get; }
    }

    public interface IRight<TLeft, TRight> : IEither<TLeft, TRight>
    {
        TRight Value { get; }
    }
}
