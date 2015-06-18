using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy.Patterns
{
    public class Case<T>
    {
        private IMaybe<T> pattern;

        private Case(T pattern)
        {
            this.pattern = Maybe<T>.Some(pattern);
        }

        private Case()
        {
            this.pattern = Maybe<T>.None();
        }

        public static Case<T> Of(T pattern)
        {
            return new Case<T>(pattern);
        }

        public static Case<T> Else()
        {
            return new Case<T>();
        }

        public Pattern<T> Then(Action action)
        {
            return new Pattern<T>(this, action);
        }

        internal bool Test(Matcher matcher)
        {
            if (this.pattern.IsSome)
            {
                return matcher.Target.Equals(this.pattern.ToSome().Value);
            }
            else
            {
                return true;
            }
        }
    }
}
