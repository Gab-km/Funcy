using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funcy;

namespace Funcy.Patterns
{
    public class Matcher
    {
        public static Matcher Match(object target)
        {
            return new Matcher(target);
        }

        public object Target { get; private set; }

        private Matcher(object target)
        {
            this.Target = target;
        }

        public void With<T>(Pattern<T> pattern, params Pattern<T>[] patterns)
        {
            var maybeMatched = pattern.Matching(this);
            if (maybeMatched.IsSome)
            {
                if (patterns.Length > 0)
                {
                    this.With(patterns[0], patterns.Skip(1).ToArray());
                }
            }
        }
    }
}
