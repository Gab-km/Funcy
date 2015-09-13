using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcy;

namespace Funcy.Patterns
{
    public interface IPattern
    {
        Either<MatchFailureException, IMatcher> Matching(IMatcher matcher);
    }

    public class Pattern<T> : IPattern
    {
        private ICase<T> _case;
        private Either<Action, Action<T>> action;

        internal Pattern(ICase<T> @Case, Action action)
        {
            this._case = @Case;
            this.action = Either<Action, Action<T>>.Left(action);
        }

        internal Pattern(ICase<T> @case, Action<T> action)
        {
            this._case = @case;
            this.action = Either<Action, Action<T>>.Right(action);
        }

        public Either<MatchFailureException, IMatcher> Matching(IMatcher matcher)
        {
            if (this._case.Test(matcher))
            {
                if (this.action.IsRight)
                {
                    var value = this._case.GetValue(matcher);
                    this.action.ToRight().Value.Invoke(value);
                }
                else
                {
                    this.action.ToLeft().Value.Invoke();
                }
                return Either<MatchFailureException, IMatcher>.Right(matcher);
            }
            else
            {
                return Either<MatchFailureException, IMatcher>.Left(new MatchFailureException());
            }
        }
    }

    public class ExtractPattern<T, U> : IPattern where U : IExtractor<T>
    {
        private FromCase<T, U> _case;
        private Action<T> action;

        internal ExtractPattern(FromCase<T, U> @case, Action<T> action)
        {
            this._case = @case;
            this.action = action;
        }

        public Either<MatchFailureException, IMatcher> Matching(IMatcher matcher)
        {
            if (this._case.Test(matcher))
            {
                var value = this._case.GetInnerValue(matcher);
                this.action.Invoke(value);
                return Either<MatchFailureException, IMatcher>.Right(matcher);
            }
            else
            {
                return Either<MatchFailureException, IMatcher>.Left(new MatchFailureException());
            }
        }
    }

    public interface IReturnablePattern<TReturn>
    {
        Either<MatchFailureException, TReturn> Matching(ReturnableMatcher<TReturn> matcher);
    }

    public class ReturnablePattern<T, TReturn> : IReturnablePattern<TReturn>
    {
        private ICase<T> _case;
        private Either<Func<TReturn>, Func<T, TReturn>> func;

        internal ReturnablePattern(ICase<T> @Case, Func<TReturn> func)
        {
            this._case = @Case;
            this.func = Either<Func<TReturn>, Func<T, TReturn>>.Left(func);
        }

        internal ReturnablePattern(ICase<T> @case, Func<T, TReturn> func)
        {
            this._case = @case;
            this.func = Either<Func<TReturn>, Func<T, TReturn>>.Right(func);
        }

        public Either<MatchFailureException, TReturn> Matching(ReturnableMatcher<TReturn> matcher)
        {
            if (this._case.Test(matcher))
            {
                if (this.func.IsRight)
                {
                    var value = this._case.GetValue(matcher);
                    return Either<MatchFailureException, TReturn>.Right(this.func.ToRight().Value.Invoke(value));
                }
                else
                {
                    return Either<MatchFailureException, TReturn>.Right(this.func.ToLeft().Value.Invoke());
                }
            }
            else
            {
                return Either<MatchFailureException, TReturn>.Left(new MatchFailureException());
            }
        }
    }

    public class ReturnableExtractPattern<T, U, TReturn> : IReturnablePattern<TReturn> where U : IExtractor<T>
    {
        private FromCase<T, U> _case;
        private Func<T, TReturn> func;

        internal ReturnableExtractPattern(FromCase<T, U> @case, Func<T, TReturn> func)
        {
            this._case = @case;
            this.func = func;
        }

        public Either<MatchFailureException, TReturn> Matching(ReturnableMatcher<TReturn> matcher)
        {
            if (this._case.Test(matcher))
            {
                var value = this._case.GetInnerValue(matcher);
                return Either<MatchFailureException, TReturn>.Right(this.func.Invoke(value));
            }
            else
            {
                return Either<MatchFailureException, TReturn>.Left(new MatchFailureException());
            }
        }
    }

    [Serializable]
    public class MatchFailureException : Exception
    {
        public MatchFailureException() { }
        public MatchFailureException(string message) : base(message) { }
        public MatchFailureException(string message, Exception inner) : base(message, inner) { }
        protected MatchFailureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
