using System.Collections.Generic;
using System.Linq;

namespace Funcy.Future.NaturalTransformations
{
    public static class FuncyListTCNT
    {
        public static MaybeTC<T> ElementAt<T>(this FuncyListTC<T> source, int index)
        {
            if (source.IsNil)
            {
                return MaybeTC.None<T>();
            }

            if (index < 0)
            {
                return MaybeTC.None<T>();
            }

            if (index >= source.Count())
            {
                return MaybeTC.None<T>();
            }

            return MaybeTC.Some(((IEnumerable<T>)source).ElementAt(index));
        }

        public static MaybeTC<T> Last<T>(this FuncyListTC<T> source)
        {
            if (source.IsNil)
            {
                return MaybeTC.None<T>();
            }
            else
            {
                return MaybeTC.Some(((IEnumerable<T>)source).Last());
            }
        }

        public static FuncyListTC<T> Take<T>(this FuncyListTC<T> source, int count)
        {
            if (source.IsNil)
            {
                return FuncyListTC.Nil<T>();
            }

            return FuncyListTC.Construct(((IEnumerable<T>)source).Take<T>(count).ToArray());
        }

        public static MaybeTC<T> First<T>(this FuncyListTC<T> source)
        {
            if (source.IsNil)
            {
                return MaybeTC.None<T>();
            }
            else
            {
                return MaybeTC.Some(source.ToCons().Head);
            }
        }
    }
}
