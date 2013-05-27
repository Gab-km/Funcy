using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Funcy.Test
{
    public class OptionTest
    {
        [TestFixture]
        public class ForModule
        {
            [TestCase(1)]
            [TestCase(0)]
            public void GivenIntOptionWhenIsSome(int value)
            {
                var opt = Option<int>.Some(value);
                Assert.True(OptionModule.IsSome(opt));
            }

            [TestCase("hoge")]
            [TestCase("")]
            [TestCase(null)]
            public void GivenStringOptionWhenIsSome(string value)
            {
                var opt = Option<string>.Some(value);
                Assert.True(OptionModule.IsSome(opt));
            }

            [TestCase(0)]
            [TestCase("")]
            public void GivenNoneWhenIsSome<T>(T placeholder)
            {
                var none = Option<T>.None();
                Assert.False(OptionModule.IsSome(none));
            }

            [TestCase(1)]
            [TestCase(0)]
            public void GivenIntOptionWhenIsNone(int value)
            {
                var opt = Option<int>.Some(value);
                Assert.False(OptionModule.IsNone(opt));
            }

            [TestCase("hoge")]
            [TestCase("")]
            [TestCase(null)]
            public void GivenStringOptionWhenIsNone(string value)
            {
                var opt = Option<string>.Some(value);
                Assert.False(OptionModule.IsNone(opt));
            }

            [TestCase(0)]
            [TestCase("")]
            public void GivenNoneWhenIsNone<T>(T placeholder)
            {
                var none = Option<T>.None();
                Assert.True(OptionModule.IsNone(none));
            }

            [TestCase(1)]
            [TestCase("hoge")]
            public void GivenSomeWhenGet<T>(T value)
            {
                var some = Option<T>.Some(value);
                Assert.That(OptionModule.Get(some), Is.EqualTo(value));
            }

            [TestCase(0)]
            [TestCase("")]
            public void GivenNoneWhenGetThenThrowArgumentException<T>(T placeholder)
            {
                var none = Option<T>.None();
                Assert.Throws<ArgumentException>(() => OptionModule.Get(none));
            }

            [TestCase(1)]
            [TestCase(0)]
            public void GivenIntOptionWhenBind(int value)
            {
                SatisfyMonadLaw1(value);
                SatisfyMonadLaw2(value);
                SatisfyMonadLaw3(value);
            }

            [TestCase("hoge")]
            [TestCase("")]
            [TestCase(null)]
            public void GivenStringOptionWhenBind(string value)
            {
                SatisfyMonadLaw1(value);
                SatisfyMonadLaw2(value);
                SatisfyMonadLaw3(value);
            }

            private void SatisfyMonadLaw1<T>(T value)
            {
                Func<T, Option<bool>> f = x => Option<bool>.Some(true);
                Assert.That(
                    OptionModule.Bind(f, Option<T>.Some(value)),
                    Is.EqualTo(f(value))
                );
            }

            private void SatisfyMonadLaw2<T>(T value)
            {
                var m = Option<T>.Some(value);
                Assert.That(
                    OptionModule.Bind(Option<T>.Some, m),
                    Is.EqualTo(m)
                );
            }

            private void SatisfyMonadLaw3<T>(T value)
            {
                var m = Option<T>.Some(value);
                Func<T, Option<string>> f = x => Option<string>.Some(string.Format("{0}", x));
                Func<string, Option<int>> g = x => Option<int>.Some(x.Length);
                Assert.That(
                    OptionModule.Bind(g, OptionModule.Bind(f, m)),
                    Is.EqualTo(OptionModule.Bind(x => OptionModule.Bind(g, f(x)), m))
                );
            }
        }

        [TestFixture]
        public class ForType
        {
            [TestCase(1)]
            [TestCase(0)]
            public void GivenIntOptionWhenIsSome(int value)
            {
                var opt = Option<int>.Some(value);
                Assert.True(opt.IsSome);
            }

            [TestCase("hoge")]
            [TestCase("")]
            [TestCase(null)]
            public void GivenStringOptionWhenIsSome(string value)
            {
                var opt = Option<string>.Some(value);
                Assert.True(opt.IsSome);
            }

            [TestCase(0)]
            [TestCase("")]
            public void GivenNoneWhenIsSome<T>(T placeholder)
            {
                var none = Option<T>.None();
                Assert.False(none.IsSome);
            }

            [TestCase(1)]
            [TestCase(0)]
            public void GivenIntOptionWhenIsNone(int value)
            {
                var opt =  Option<int>.Some(value);
                Assert.False(opt.IsNone);
            }

            [TestCase("hoge")]
            [TestCase("")]
            [TestCase(null)]
            public void GivenStringOptionWhenIsNone(string value)
            {
                var opt = Option<string>.Some(value);
                Assert.False(opt.IsNone);
            }

            [TestCase(0)]
            [TestCase("")]
            public void GivenNoneWhenIsNone<T>(T placeholder)
            {
                var none = Option<T>.None();
                Assert.True(none.IsNone);
            }

            [TestCase(1)]
            [TestCase(0)]
            public void GivenIntOptionWhenMatchWith(int value)
            {
                TestOptionMatchWith<int>(() => Option<int>.Some(value), true);
                TestOptionMatchWith2<int>(() => Option<int>.Some(value), true);
            }

            [TestCase("hoge")]
            [TestCase("")]
            [TestCase(null)]
            public void GivenStringOptionWhenMatchWith(string value)
            {
                TestOptionMatchWith<string>(() => Option<string>.Some(value), true);
                TestOptionMatchWith2<string>(() => Option<string>.Some(value), true);
            }

            [TestCase(0)]
            [TestCase("")]
            public void GivenNoneWhenMatchWith<T>(T placeholder)
            {
                TestOptionMatchWith<T>(() => Option<T>.None(), false);
                TestOptionMatchWith2<T>(() => Option<T>.None(), false);
            }

            private void TestOptionMatchWith<T>(Func<Option<T>> creator, bool doneSomeCase)
            {
                var opt = creator();
                bool result = false;
                opt.MatchWith(
                    value => result = true,
                    () => result = false
                );
                Assert.That(result, Is.EqualTo(doneSomeCase));
            }

            private void TestOptionMatchWith2<T>(Func<Option<T>> creator, bool doneSomeCase)
            {
                var opt = creator();
                bool result = opt.MatchWith<bool>(
                    value => true,
                    () => false
                );
                Assert.That(result, Is.EqualTo(doneSomeCase));
            }

            [TestCase(1)]
            [TestCase(0)]
            public void GivenIntOptionWhenBind(int value)
            {
                SatisfyMonadLaw1(value);
                SatisfyMonadLaw2(value);
                SatisfyMonadLaw3(value);
            }

            [TestCase("hoge")]
            [TestCase("")]
            [TestCase(null)]
            public void GivenStringOptionWhenBind(string value)
            {
                SatisfyMonadLaw1(value);
                SatisfyMonadLaw2(value);
                SatisfyMonadLaw3(value);
            }

            private void SatisfyMonadLaw1<T>(T value)
            {
                Func<T, Option<bool>> f = x => Option<bool>.Some(true);
                Assert.That(
                    Option<T>.Some(value).Bind(f),
                    Is.EqualTo(f(value))
                );
            }

            private void SatisfyMonadLaw2<T>(T value)
            {
                var m = Option<T>.Some(value);
                Assert.That(
                    m.Bind(Option<T>.Some),
                    Is.EqualTo(m)
                );
            }

            private void SatisfyMonadLaw3<T>(T value)
            {
                var m = Option<T>.Some(value);
                Func<T, Option<string>> f = x => Option<string>.Some(string.Format("{0}", x));
                Func<string, Option<int>> g = x => Option<int>.Some(x.Length);
                Assert.That(
                    m.Bind(f).Bind(g),
                    Is.EqualTo(m.Bind(x => f(x).Bind(g)))
                );
            }

            [TestCase(0)]
            public void GivenNoneWhenBind<T>(T placeholder)
            {
                Func<T, Option<bool>> f1 = x => Option<bool>.Some(true);
                Assert.That(
                    Option<T>.None().Bind(f1),
                    Is.EqualTo(Option<bool>.None())
                );
            }

            [TestCase(1, 1, true)]
            [TestCase(1, 0, false)]
            [TestCase("hoge", "hoge", true)]
            [TestCase("hoge", "", false)]
            public void GivenSameTypeOptionWhenEquals<T>(T leftValue, T rightValue, bool expected)
            {
                var left = Option<Option<T>>.Some(Option<T>.Some(leftValue));
                var right = Option<Option<T>>.Some(Option<T>.Some(rightValue));
                Assert.That(left.Equals(right), Is.EqualTo(expected));
            }

            [TestCase("hoge", false)]
            [TestCase("", false)]
            [TestCase(null, true)]
            public void GivenNullValueOptionWhenEquals(string value, bool expected)
            {
                var nullOpt = Option<Option<string>>.Some(Option<string>.Some(null));

                var opt = Option<Option<string>>.Some(Option<string>.Some(value));
                Assert.That(opt.Equals(nullOpt), Is.EqualTo(expected));
                Assert.That(nullOpt.Equals(opt), Is.EqualTo(expected));
            }

            [TestCase(1, "hoge")]
            [TestCase("", 0)]
            public void GivenDifferentTypeOptionWhenEquals<T, U>(T leftValue, U rightValue)
            {
                var left = Option<Option<T>>.Some(Option<T>.Some(leftValue));
                var right = Option<Option<U>>.Some(Option<U>.Some(rightValue));
                Assert.False(left.Equals(right));
            }

            [TestCase(1, "hoge")]
            [TestCase("", 0)]
            public void GivenDifferentTypeLevelWhenEquals<T, U>(T leftValue, U rightValue)
            {
                var opt = Option<T>.Some(leftValue);
                var noOpt = rightValue;
                Assert.False(opt.Equals(noOpt));
                Assert.False(noOpt.Equals(opt));
            }

            [TestCase(0)]
            [TestCase("")]
            public void GivenSameTypeOfNoneWhenEquals<T>(T placeholder)
            {
                var none1 = Option<T>.None();
                var none2 = Option<T>.None();
                Assert.True(none1.Equals(none2));
            }

            [Test]
            public void GivenDifferentTypeOfNoneWhenEquals()
            {
                var none1 = Option<int>.None();
                var none2 = Option<string>.None();
                Assert.False(none1.Equals(none2));
            }

            [TestCase(1)]
            [TestCase("hoge")]
            public void GivenSomeAndNoneWhenEquals<T>(T value)
            {
                var some = Option<T>.Some(value);
                var none = Option<T>.None();
                Assert.False(some.Equals(none));
                Assert.False(none.Equals(some));
            }
        }
    }
}
