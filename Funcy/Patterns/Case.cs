using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funcy;

namespace Funcy.Patterns
{
    public interface ICase<T>
    {
        IPattern Then(Action action);
        bool Test(Matcher matcher);
        T GetValue(Matcher matcher);
    }

    public static class Case<T>
    {
        public static ICase<T> Of(T pattern)
        {
            return new OfCase<T>(pattern);
        }

        public static ICase<T> When(Func<T, bool> predicate)
        {
            return new WhenCase<T>(predicate);
        }

        public static FromCase<T, U> From<U>() where U : IExtractor<T>
        {
            return new FromCase<T, U>();
        }

        public static ICase<T> Else()
        {
            return new ElseCase<T>();
        }
    }

    public class OfCase<T> : ICase<T>
    {
        private T pattern;

        internal OfCase(T pattern)
        {
            this.pattern = pattern;
        }

        public IPattern Then(Action action)
        {
            return new Pattern<T>(this, action);
        }

        public IPattern Then(Action<T> action)
        {
            return new Pattern<T>(this, action);
        }

        public bool Test(Matcher matcher)
        {
            return matcher.Target.Equals(this.pattern);
        }

        public T GetValue(Matcher matcher)
        {
            return this.pattern;
        }
    }

    public class WhenCase<T> : ICase<T>
    {
        private Func<T, bool> predicate;

        internal WhenCase(Func<T, bool> predicate)
        {
            this.predicate = predicate;
        }

        public IPattern Then(Action action)
        {
            return new Pattern<T>(this, action);
        }

        public IPattern Then(Action<T> action)
        {
            return new Pattern<T>(this, action);
        }

        public bool Test(Matcher matcher)
        {
            try
            {
                var target = (T)matcher.Target;
                return this.predicate(target);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public T GetValue(Matcher matcher)
        {
            return (T)matcher.Target;
        }
    }

    public class FromCase<T, U> : ICase<U> where U : IExtractor<T>
    {
        internal FromCase()
        {
        }

        public IPattern Then(Action action)
        {
            return new Pattern<U>(this, action);
        }

        public ExtractPattern<T, U> Then(Action<T> action)
        {
            return new ExtractPattern<T, U>(this, action);
        }

        public bool Test(Matcher matcher)
        {
            try
            {
                var extractor = (U)matcher.Target;
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public U GetValue(Matcher matcher)
        {
            return (U)matcher.Target;
        }

        internal T GetInnerValue(Matcher matcher)
        {
            IExtractor<T> target = (IExtractor<T>)matcher.Target;
            return target.Extract();
        }
    }

    public class ElseCase<T> : ICase<T>
    {
        internal ElseCase()
        {
        }

        public IPattern Then(Action action)
        {
            return new Pattern<T>(this, action);
        }

        public bool Test(Matcher matcher)
        {
            return true;
        }

        public T GetValue(Matcher matcher)
        {
            return (T)matcher.Target;
        }
    }
}
