using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public static class Composition
    {
        public static Func<T1, T3> Compose<T1, T2, T3>(Func<T2, T3> g, Func<T1, T2> f)
        {
            return x => g(f(x));
        }
    }
}
