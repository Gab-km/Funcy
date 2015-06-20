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

    [Serializable]
    public class MatchFailureException : Exception
    {
      public MatchFailureException() { }
      public MatchFailureException( string message ) : base( message ) { }
      public MatchFailureException( string message, Exception inner ) : base( message, inner ) { }
      protected MatchFailureException( 
	    System.Runtime.Serialization.SerializationInfo info, 
	    System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
