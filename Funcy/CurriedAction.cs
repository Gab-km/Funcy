using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcy
{
    public class CurriedAction<T1, T2>
    {
        private Func<T1, Action<T2>> curriedA;
        private Action<T1, T2> action;

        public static implicit operator Func<T1, Action<T2>>(CurriedAction<T1, T2> ca)
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2> f)
        {
            this.action = f;
            this.curriedA = (t1) => new Action<T2>(t2 => f(t1, t2));
        }

        public Action<T2> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2> Uncurry()
        {
            return this.action;
        }
    }

    public class CurriedAction<T1, T2, T3>
    {
        private Func<T1, CurriedAction<T2, T3>> curriedA;
        private Action<T1, T2, T3> f;

        public static implicit operator Func<T1, CurriedAction<T2, T3>>(CurriedAction<T1, T2, T3> ca)
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3> f)
        {
            this.f = f;
            this.curriedA = (t1) => new CurriedAction<T2, T3>((t2, t3) => f(t1, t2, t3));
        }

        public CurriedAction<T2, T3> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4>
    {
        private Func<T1, CurriedAction<T2, T3, T4>> curriedA;
        private Action<T1, T2, T3, T4> f;

        public static implicit operator Func<T1, CurriedAction<T2, T3, T4>>(CurriedAction<T1, T2, T3, T4> ca)
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4> f)
        {
            this.f = f;
            this.curriedA = (t1) => new CurriedAction<T2, T3, T4>((t2, t3, t4) => f(t1, t2, t3, t4));
        }

        public CurriedAction<T2, T3, T4> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5>> curriedA;
        private Action<T1, T2, T3, T4, T5> f;

        public static implicit operator Func<T1, CurriedAction<T2, T3, T4, T5>>(CurriedAction<T1, T2, T3, T4, T5> ca)
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5> f)
        {
            this.f = f;
            this.curriedA = (t1) => new CurriedAction<T2, T3, T4, T5>((t2, t3, t4, t5) => f(t1, t2, t3, t4, t5));
        }

        public CurriedAction<T2, T3, T4, T5> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6> f;

        public static implicit operator Func<T1, CurriedAction<T2, T3, T4, T5, T6>>(CurriedAction<T1, T2, T3, T4, T5, T6> ca)
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6> f)
        {
            this.f = f;
            this.curriedA = (t1) => new CurriedAction<T2, T3, T4, T5, T6>((t2, t3, t4, t5, t6) => f(t1, t2, t3, t4, t5, t6));
        }

        public CurriedAction<T2, T3, T4, T5, T6> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7>>(CurriedAction<T1, T2, T3, T4, T5, T6, T7> ca)
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7>((t2, t3, t4, t5, t6, t7) => f(t1, t2, t3, t4, t5, t6, t7));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8>>(CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8> ca)
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8>((t2, t3, t4, t5, t6, t7, t8) =>
                    f(t1, t2, t3, t4, t5, t6, t7, t8));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9>>(CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> ca)
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9>((t2, t3, t4, t5, t6, t7, t8, t9) =>
                    f(t1, t2, t3, t4, t5, t6, t7, t8, t9));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10>>(
                CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ca
            )
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10>((t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                    f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(
                CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ca
            )
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>((t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                    f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(
                CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ca
            )
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                    (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(
                CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ca
            )
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                    (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(
                CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ca
            )
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                    (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>(
                CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ca
            )
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                    (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Uncurry()
        {
            return this.f;
        }
    }

    public class CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {
        private Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> curriedA;
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> f;

        public static implicit operator
            Func<T1, CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>(
                CurriedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ca
            )
        {
            return ca.curriedA;
        }

        public CurriedAction(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> f)
        {
            this.f = f;
            this.curriedA = (t1) =>
                new CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
                    (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16));
        }

        public CurriedAction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Invoke(T1 arg)
        {
            return this.curriedA(arg);
        }

        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Uncurry()
        {
            return this.f;
        }
    }
}
