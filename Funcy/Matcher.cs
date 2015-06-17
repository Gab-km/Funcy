using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public static class Matcher
    {
        public static Matcher<T> Match<T>(T target) where T : IEquatable<T>
        {
            return new Matcher<T>(target, false);
        }
    }

    public class Matcher<T> where T : IEquatable<T>
    {
        private T target;
        private bool matched;

        public Matcher(T target, bool matched)
        {
            this.target = target;
            this.matched = matched;
        }

        public Matcher<T> Case(T expected, Action action)
        {
            if (!this.matched && this.target.Equals(expected))
            {
                action.Invoke();
                return new Matcher<T>(target, true);
            }
            else
            {
                return this;
            }
        }

        public void Else(Action action)
        {
            if (!this.matched)
            {
                action.Invoke();
            }
        }
    }
}
