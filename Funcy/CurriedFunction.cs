using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcy
{
    public class CurriedFunction<T1, T2, TReturn>
    {
        private Func<T1, Func<T2, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, TReturn> f)
        {
            this.curriedF = (t1) => new Func<T2, TReturn>(t2 => f(t1, t2));
        }

        public Func<T2, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, TReturn>
    {
        private Func<T1, CurriedFunction<T2, T3, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, TReturn> f)
        {
            this.curriedF = (t1) => new CurriedFunction<T2, T3, TReturn>(new Func<T2, T3, TReturn>((t2, t3) => f(t1, t2, t3)));
        }

        public CurriedFunction<T2, T3, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, TReturn>
    {
        private Func<T1, CurriedFunction<T2, T3, T4, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, TReturn>(new Func<T2, T3, T4, TReturn>((t2, t3, t4) => f(t1, t2, t3, t4)));
        }

        public CurriedFunction<T2, T3, T4, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, TReturn>
    {
        private Func<T1, CurriedFunction< T2, T3, T4, T5, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, TReturn>(
                    new Func<T2, T3, T4, T5, TReturn>((t2, t3, t4, t5) => f(t1, t2, t3, t4, t5)));
        }

        public CurriedFunction<T2, T3, T4, T5, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, TReturn>
    {
        private Func<T1, CurriedFunction<T2, T3, T4, T5, T6, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, TReturn>(
                    new Func<T2, T3, T4, T5, T6, TReturn>((t2, t3, t4, t5, t6) => f(t1, t2, t3, t4, t5, t6)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, TReturn>
    {
        private Func<T1, CurriedFunction<T2, T3, T4, T5, T6, T7, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, TReturn>((t2, t3, t4, t5, t6, t7) => f(t1, t2, t3, t4, t5, t6, t7)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>
    {
        private Func<T1,CurriedFunction< T2, T3, T4, T5, T6, T7, T8, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, TReturn>((t2, t3, t4, t5, t6, t7, t8) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>
    {
        private Func<T1,CurriedFunction< T2, T3, T4, T5, T6, T7, T8, T9, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, T9, TReturn>((t2, t3, t4, t5, t6, t7, t8, t9) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8, t9)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>
    {
        private Func<T1,CurriedFunction< T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>((t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>
    {
        private Func<T1,CurriedFunction< T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn>((t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                        f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>
    {
        private Func<T1,CurriedFunction< T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn>(
                        (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                            f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>
    {
        private Func<T1, CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>(
                        (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                            f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>
    {
        private Func<T1, CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>(
                        (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                            f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>
    {
        private Func<T1, CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>(
                        (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                            f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }

    public class CurriedFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>
    {
        private Func<T1, CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>> curriedF;

        public CurriedFunction(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn> f)
        {
            this.curriedF = (t1) =>
                new CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>(
                    new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn>(
                        (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
                            f(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16)));
        }

        public CurriedFunction<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturn> Invoke(T1 arg)
        {
            return this.curriedF(arg);
        }
    }
}
