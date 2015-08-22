using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcy;

namespace Funcy.Patterns
{
    public interface IPattern
    {
        Either<MatchFailureException, Matcher> Matching(Matcher matcher);
    }

    public class Pattern<T> : IPattern
    {
        private ICase<T> _case;
        private Maybe<Action> actionVoid;
        private Maybe<Action<T>> actionT;

        internal Pattern(ICase<T> @Case, Action action)
        {
            this._case = @Case;
            this.actionVoid = Maybe<Action>.Some(action);
            this.actionT = Maybe<Action<T>>.None();
        }

        internal Pattern(ICase<T> @case, Action<T> action)
        {
            this._case = @case;
            this.actionT = Maybe<Action<T>>.Some(action);
            this.actionVoid = Maybe<Action>.None();
        }

        public Either<MatchFailureException, Matcher> Matching(Matcher matcher)
        {
            if (this._case.Test(matcher))
            {
                if (this.actionT.IsSome)
                {
                    var value = this._case.GetValue(matcher);
                    this.actionT.ToSome().Value.Invoke(value);
                }
                else
                {
                    this.actionVoid.ToSome().Value.Invoke();
                }
                return Either<MatchFailureException, Matcher>.Right(matcher);
            }
            else
            {
                return Either<MatchFailureException, Matcher>.Left(new MatchFailureException());
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

        public Either<MatchFailureException, Matcher> Matching(Matcher matcher)
        {
            if (this._case.Test(matcher))
            {
                var value = this._case.GetInnerValue(matcher);
                this.action.Invoke(value);
                return Either<MatchFailureException, Matcher>.Right(matcher);
            }
            else
            {
                return Either<MatchFailureException, Matcher>.Left(new MatchFailureException());
            }
        }

    }
}
