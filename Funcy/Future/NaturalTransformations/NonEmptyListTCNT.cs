using System.Linq;

namespace Funcy.Future.NaturalTransformations
{
    public static class NonEmptyListTCNT
    {
        public static FuncyListTC<T> ToFuncyList<T>(this NonEmptyListTC<T> self)
        {
            return FuncyListTC.Construct<T>(self.ToArray());
        }

        public static FuncyListTC<T> Take<T>(this NonEmptyListTC<T> self, int length)
        {
            return ToFuncyList(self).Take(length);
        }

        public static T TakeFirst<T>(this NonEmptyListTC<T> self)
        {
            var enumerator = self.GetEnumerator();
            enumerator.MoveNext();

            return enumerator.Current;
        }
    }
}
