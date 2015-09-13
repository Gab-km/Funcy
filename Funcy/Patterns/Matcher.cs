using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funcy;

namespace Funcy.Patterns
{
    public interface IMatcher
    {
        object Target { get; }
    }

    public static class Matcher
    {
        public static SimpleMatcher Match(object target)
        {
            return new SimpleMatcher(target);
        }

        public static ReturnableMatcher<TReturn> ReturnMatch<TReturn>(object target)
        {
            return new ReturnableMatcher<TReturn>(target);
        }
    }

    public class SimpleMatcher : IMatcher
    {
        public object Target { get; private set; }

        internal SimpleMatcher(object target)
        {
            this.Target = target;
        }

        public void With(IPattern pattern, params IPattern[] patterns)
        {
            var matched = pattern.Matching(this);
            if (matched.IsLeft)
            {
                if (patterns.Length > 0)
                {
                    this.With(patterns[0], patterns.Skip(1).ToArray());
                }
                else
                {
                    throw matched.ToLeft().Value;
                }
            }
        }
    }

    public class ReturnableMatcher<TReturn> : IMatcher
    {
        public object Target { get; private set; }

        internal ReturnableMatcher(object target)
        {
            this.Target = target;
        }

        public TReturn With(IReturnablePattern<TReturn> pattern, params IReturnablePattern<TReturn>[] patterns)
        {
            var matched = pattern.Matching(this);
            if (matched.IsLeft)
            {
                if (patterns.Length > 0)
                {
                    return this.With(patterns[0], patterns.Skip(1).ToArray());
                }
                else
                {
                    throw matched.ToLeft().Value;
                }
            }
            else
            {
                return matched.ToRight().Value;
            }
        }
    }
}
