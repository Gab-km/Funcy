using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcy;

namespace Funcy.Patterns
{
    public class Pattern<T>
    {
        private Case<T> _case;
        private Action action;

        public Pattern(Case<T> @case, Action action)
        {
            this._case = @case;
            this.action = action;
        }

        public IMaybe<Matcher> Matching(Matcher matcher)
        {
            if (this._case.Test(matcher))
            {
                this.action.Invoke();
                return Maybe<Matcher>.None();
            }
            else
            {
                return Maybe<Matcher>.Some(matcher);
            }
        }
    }
}
