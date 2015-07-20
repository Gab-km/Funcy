using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy.Extensions
{
    public static class CurryingExtension
    {
        public static CurriedFunction<T1, T2, TReturn> Curry<T1, T2, TReturn>(this Func<T1, T2, TReturn> f)
        {
            return Currying.Curry<T1, T2, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, TReturn> Curry<T1, T2, T3, TReturn>(this Func<T1, T2, T3, TReturn> f)
        {
            return Currying.Curry<T1, T2, T3, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, TReturn>
            Curry<T1, T2, T3, T4, TReturn>(this Func<T1, T2, T3, T4, TReturn> f)
        {
            return Currying.Curry<T1, T2, T3, T4, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, TReturn>
            Curry<T1, T2, T3, T4, T5, TReturn>(this Func<T1, T2, T3, T4, T5, TReturn> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, TReturn>(this Func<T1, T2, T3, T4, T5, T6, TReturn> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, TReturn>(this Func<T1, T2, T3, T4, T5, T6, T7, TReturn> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>(
                this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>(
                this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>(
                this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>(
                this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>(
                this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>(
                this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>(f);
        }

        public static CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>(
                this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>(f);
        }

        public static CurriedAction<T1, T2> Curry<T1, T2>(this Action<T1, T2> f)
        {
            return Currying.Curry<T1, T2>(f);
        }

        public static CurriedAction<T1, T2, T3> Curry<T1, T2, T3>(this Action<T1, T2, T3> f)
        {
            return Currying.Curry<T1, T2, T3>(f);
        }

        public static CurriedAction<T1, T2, T3, T4> Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> f)
        {
            return Currying.Curry<T1, T2, T3, T4>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5> Curry<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6>
            Curry<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7>
            Curry<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> f)
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(f);
        }

        public static CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
                this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> f
            )
        {
            return Currying.Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(f);
        }
    }
}
