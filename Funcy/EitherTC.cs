using System;
using System.Collections;
using System.Collections.Generic;
using Funcy.Computations;
using Funcy.Patterns;

namespace Funcy
{
    public abstract class EitherTC<TLeft, TRight> : IStructuralEquatable, IStructuralComparable, IComputableTC<EitherTC<TLeft>, TRight>
    {
        public abstract bool IsRight { get; }
        public abstract bool IsLeft { get; }

        public RightTC<TLeft, TRight> ToRight()
        {
            return (RightTC<TLeft, TRight>)this;
        }

        public LeftTC<TLeft, TRight> ToLeft()
        {
            return (LeftTC<TLeft, TRight>)this;
        }

        public IPointed<EitherTC<TLeft>> Pointed { get { return new EitherTC<TLeft>(); } }

        IFunctorTC<EitherTC<TLeft>, TReturn> IFunctorTC<EitherTC<TLeft>, TRight>.FMap<TReturn>(Func<TRight, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }
        public abstract EitherTC<TLeft, TReturn> FMap<TReturn>(Func<TRight, TReturn> f);

        IApplicativeTC<EitherTC<TLeft>, TReturn> IApplicativeTC<EitherTC<TLeft>, TRight>.FMapA<TReturn>(Func<TRight, TReturn> f)
        {
            return this.FMapA(f);
        }
        public EitherTC<TLeft, TReturn> FMapA<TReturn>(Func<TRight, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }

        IApplicativeTC<EitherTC<TLeft>, TReturn> IApplicativeTC<EitherTC<TLeft>, TRight>.Apply<TReturn>(IApplicativeTC<EitherTC<TLeft>, Func<TRight, TReturn>> f)
        {
            return this.Apply((EitherTC<TLeft, Func<TRight, TReturn>>)f);
        }
        public EitherTC<TLeft, TReturn> Apply<TReturn>(EitherTC<TLeft, Func<TRight, TReturn>> f)
        {
            if (f.IsRight)
            {
                return this.FMap<TReturn>(f.ToRight().Value);
            }
            else
            {
                return EitherTC<TLeft>.Left<TReturn>(f.ToLeft().Value);
            }
        }

        IApplicativeTC<EitherTC<TLeft>, TRight> IApplicativeTC<EitherTC<TLeft>, TRight>.ApplyLeft<TReturn>(IApplicativeTC<EitherTC<TLeft>, TReturn> other)
        {
            return this.ApplyLeft<TReturn>((EitherTC<TLeft, TReturn>)other);
        }
        public EitherTC<TLeft, TRight> ApplyLeft<TReturn>(EitherTC<TLeft, TReturn> other)
        {
            return this;
        }

        IApplicativeTC<EitherTC<TLeft>, TReturn> IApplicativeTC<EitherTC<TLeft>, TRight>.ApplyRight<TReturn>(IApplicativeTC<EitherTC<TLeft>, TReturn> other)
        {
            return this.ApplyRight<TReturn>((EitherTC<TLeft, TReturn>)other);
        }
        public EitherTC<TLeft, TReturn> ApplyRight<TReturn>(EitherTC<TLeft, TReturn> other)
        {
            return other;
        }

        IComputableTC<EitherTC<TLeft>, TReturn> IComputableTC<EitherTC<TLeft>, TRight>.Compute<TReturn>(Func<TRight, TReturn> f)
        {
            return this.Compute(f);
        }
        public EitherTC<TLeft, TReturn> Compute<TReturn>(Func<TRight, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }

        IComputableTC<EitherTC<TLeft>, TReturn> IComputableTC<EitherTC<TLeft>, TRight>.ComputeWith<TReturn>(Func<TRight, IComputableTC<EitherTC<TLeft>, TReturn>> f)
        {
            return this.ComputeWith((Func<TRight, EitherTC<TLeft, TReturn>>)f);
        }
        public abstract EitherTC<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, EitherTC<TLeft, TReturn>> f);

        public abstract bool Equals(object other, IEqualityComparer comparer);
        public abstract int GetHashCode(IEqualityComparer comparer);
        public abstract int CompareTo(object other, IComparer comparer);
    }

    public class EitherTC<TLeft> : IPointed<EitherTC<TLeft>>
    {
        public static EitherTC<TLeft, TRight> Right<TRight>(TRight right)
        {
            return new RightTC<TLeft, TRight>(right);
        }

        public static EitherTC<TLeft, TRight> Left<TRight>(TLeft left)
        {
            return new LeftTC<TLeft, TRight>(left);
        }

        IFunctorTC<EitherTC<TLeft>, TSource> IPointed<EitherTC<TLeft>>.Point<TSource>(TSource value)
        {
            return this.Point<TSource>(value);
        }
        public EitherTC<TLeft, TSource> Point<TSource>(TSource value)
        {
            return EitherTC<TLeft>.Right<TSource>(value);
        }
    }

    public sealed class RightTC<TLeft, TRight> : EitherTC<TLeft, TRight>, IExtractor<TRight>
    {
        public TRight Value { get; private set; }

        public RightTC(TRight value)
        {
            this.Value = value;
        }

        public override bool IsRight { get { return true; } }
        public override bool IsLeft { get { return !this.IsRight; } }

        public override EitherTC<TLeft, TReturn> FMap<TReturn>(Func<TRight, TReturn> f)
        {
            return new RightTC<TLeft, TReturn>(f(this.Value));
        }

        public override EitherTC<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, EitherTC<TLeft, TReturn>> f)
        {
            return f(this.Value);
        }

        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is RightTC<TLeft, TRight>)
            {
                var right = (RightTC<TLeft, TRight>)other;
                return comparer.Equals(this.Value, right.Value);
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<TRight>.Default);
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(typeof(TLeft));
            var h2 = comparer.GetHashCode(this.Value);
            return ((h1 << 5) + h1) ^ h2;
        }
        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 1;
            if (other is RightTC<TLeft, TRight>)
            {
                var right = (RightTC<TLeft, TRight>)other;
                return comparer.Compare(this.Value, right.Value);
            }
            else if (other is LeftTC<TLeft, TRight>)
            {
                return 1;
            }
            else
            {
                throw new ArgumentException("'other' is neither instance of RightTC nor LeftTC.");
            }
        }

        TRight IExtractor<TRight>.Extract()
        {
            return this.Value;
        }
    }

    public sealed class LeftTC<TLeft, TRight> : EitherTC<TLeft, TRight>, IExtractor<TLeft>
    {
        public TLeft Value { get; private set; }

        public override bool IsRight { get { return !this.IsLeft; } }
        public override bool IsLeft { get { return true; } }

        public LeftTC(TLeft value)
        {
            this.Value = value;
        }

        public override EitherTC<TLeft, TReturn> FMap<TReturn>(Func<TRight, TReturn> f)
        {
            return new LeftTC<TLeft, TReturn>(this.Value);
        }

        public override EitherTC<TLeft, TReturn> ComputeWith<TReturn>(Func<TRight, EitherTC<TLeft, TReturn>> f)
        {
            return EitherTC<TLeft>.Left<TReturn>(this.Value);
        }

        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (other is LeftTC<TLeft, TRight>)
            {
                var left = (LeftTC<TLeft, TRight>)other;
                return comparer.Equals(this.Value, left.Value);
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<TLeft>.Default);
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.Value);
            var h2 = comparer.GetHashCode(typeof(TRight));
            return ((h1 << 5) + h1) ^ h2;
        }
        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }

        public override int CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 1;
            if (other is LeftTC<TLeft, TRight>)
            {
                var left = (LeftTC<TLeft, TRight>)other;
                return comparer.Compare(this.Value, left.Value);
            }
            else if (other is RightTC<TLeft, TRight>)
            {
                return -1;
            }
            else
            {
                throw new ArgumentException("'other' is neither instance of LeftTC nor RightTC.");
            }
        }

        TLeft IExtractor<TLeft>.Extract()
        {
            return this.Value;
        }
    }
}
