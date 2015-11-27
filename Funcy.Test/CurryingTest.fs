namespace Funcy.Test

open System
open Funcy
open Persimmon
open UseTestNameByReflection

module CurryingTest =
    module CurriedFunctionTest =
        let ``Curry ((T1, T2) -> TReturn) = T1 -> T2 -> TReturn`` = test {
            let add = Func<int, int, int>(fun x y -> x + y)
            let sut = Currying.Curry(add)
            do! assertEquals typeof<CurriedFunction<int, int, int>> <| sut.GetType()
            let add2 = sut.Invoke(2)
            do! assertEquals typeof<Func<int, int>> <| add2.GetType()
            do! assertEquals 5 <| add2.Invoke(3)
        }

        let ``Curry ((T1, T2, T3) -> TReturn) = T1 -> T2 -> T3 -> TReturn`` = test {
            let myMax = Func<int, int, int, int>(fun x y z -> Math.Max(Math.Max(x, y), z))
            let sut = Currying.Curry(myMax)
            do! assertEquals typeof<CurriedFunction<int, int, int, int>> <| sut.GetType()
            let myMax1 = sut.Invoke(1)
            do! assertEquals typeof<CurriedFunction<int, int, int>> <| myMax1.GetType()
            do! assertEquals 3 <| myMax1.Invoke(3).Invoke(2)
        }

        let ``Curry ((T1, T2, T3, T4) -> TReturn) = T1 -> T2 -> T3 -> T4 -> TReturn`` = test {
            let dateText = Func<int, int, int, string, string>(fun year month day fmt -> DateTime(year, month, day).ToString(fmt))
            let sut = Currying.Curry(dateText)
            do! assertEquals typeof<CurriedFunction<int, int, int, string, string>> <| sut.GetType()
            let dateText2015 = sut.Invoke(2015)
            do! assertEquals typeof<CurriedFunction<int, int, string, string>> <| dateText2015.GetType()
            do! assertEquals "2015-07-15" <| dateText2015.Invoke(7).Invoke(15).Invoke("yyyy-MM-dd")
        }

        let ``Curry ((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> TReturn) = T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> TReturn`` = test {
            let convNconv = Func<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2)
            let sut = Currying.Curry(convNconv)
            do! assertEquals typeof<CurriedFunction<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| sut.GetType()
            let convNconv0 = sut.Invoke(0uy)
            do! assertEquals typeof<CurriedFunction<byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv0.GetType()
            let convNconv1 = convNconv0.Invoke(10uy)
            do! assertEquals typeof<CurriedFunction<int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv1.GetType()
            let convNconv2 = convNconv1.Invoke(200s)
            do! assertEquals typeof<CurriedFunction<int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv2.GetType()
            let convNconv3 = convNconv2.Invoke(3000s)
            do! assertEquals typeof<CurriedFunction<int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv3.GetType()
            let convNconv4 = convNconv3.Invoke(40000)
            do! assertEquals typeof<CurriedFunction<int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv4.GetType()
            let convNconv5 = convNconv4.Invoke(500000)
            do! assertEquals typeof<CurriedFunction<int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv5.GetType()
            let convNconv6 = convNconv5.Invoke(6000000)
            do! assertEquals typeof<CurriedFunction<int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv6.GetType()
            let convNconv7 = convNconv6.Invoke(70000000L)
            do! assertEquals typeof<CurriedFunction<int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv7.GetType()
            let convNconv8 = convNconv7.Invoke(800000000L)
            do! assertEquals typeof<CurriedFunction<int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv8.GetType()
            let convNconv9 = convNconv8.Invoke(9000000000L)
            do! assertEquals typeof<CurriedFunction<decimal, decimal, decimal, decimal, string, string, string>>
                <| convNconv9.GetType()
            let convNconvA = convNconv9.Invoke(0.1m)
            do! assertEquals typeof<CurriedFunction<decimal, decimal, decimal, string, string, string>> <| convNconvA.GetType()
            let convNconvB = convNconvA.Invoke(0.02m)
            do! assertEquals typeof<CurriedFunction<decimal, decimal, string, string, string>> <| convNconvB.GetType()
            let convNconvC = convNconvB.Invoke(0.003m)
            do! assertEquals typeof<CurriedFunction<decimal, string, string, string>> <| convNconvC.GetType()
            let convNconvD = convNconvC.Invoke(0.0004m)
            do! assertEquals typeof<CurriedFunction<string, string, string>> <| convNconvD.GetType()
            let convNconvE = convNconvD.Invoke(" = ")
            do! assertEquals typeof<Func<string, string>> <| convNconvE.GetType()
            let convNconvF = convNconvE.Invoke("9876543210.1234")
            do! assertEquals "9876543210.1234 = 9876543210.1234" convNconvF
        }

    module CurriedActionTest =
        let ``Curry ((T1, T2) -> void) = T1 -> T2 -> void`` = test {
            let result = ref 0
            let add = Action<int, int>(fun x y -> result := x + y)
            let sut = Currying.Curry(add)
            do! assertEquals typeof<CurriedAction<int, int>> <| sut.GetType()
            let add2 = sut.Invoke(2)
            do! assertEquals typeof<Action<int>> <| add2.GetType()
            do add2.Invoke(3)
            do! assertEquals 5 !result
        }

        let ``Curry ((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> void) = T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> void`` = test {
            let result = ref ""
            let convNconv = Action<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    result := Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2)
            let sut = Currying.Curry(convNconv)
            do! assertEquals typeof<CurriedAction<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| sut.GetType()
            let convNconv0 = sut.Invoke(0uy)
            do! assertEquals typeof<CurriedAction<byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv0.GetType()
            let convNconv1 = convNconv0.Invoke(10uy)
            do! assertEquals typeof<CurriedAction<int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv1.GetType()
            let convNconv2 = convNconv1.Invoke(200s)
            do! assertEquals typeof<CurriedAction<int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv2.GetType()
            let convNconv3 = convNconv2.Invoke(3000s)
            do! assertEquals typeof<CurriedAction<int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv3.GetType()
            let convNconv4 = convNconv3.Invoke(40000)
            do! assertEquals typeof<CurriedAction<int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv4.GetType()
            let convNconv5 = convNconv4.Invoke(500000)
            do! assertEquals typeof<CurriedAction<int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv5.GetType()
            let convNconv6 = convNconv5.Invoke(6000000)
            do! assertEquals typeof<CurriedAction<int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv6.GetType()
            let convNconv7 = convNconv6.Invoke(70000000L)
            do! assertEquals typeof<CurriedAction<int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv7.GetType()
            let convNconv8 = convNconv7.Invoke(800000000L)
            do! assertEquals typeof<CurriedAction<int64, decimal, decimal, decimal, decimal, string, string>>
                <| convNconv8.GetType()
            let convNconv9 = convNconv8.Invoke(9000000000L)
            do! assertEquals typeof<CurriedAction<decimal, decimal, decimal, decimal, string, string>> <| convNconv9.GetType()
            let convNconvA = convNconv9.Invoke(0.1m)
            do! assertEquals typeof<CurriedAction<decimal, decimal, decimal, string, string>> <| convNconvA.GetType()
            let convNconvB = convNconvA.Invoke(0.02m)
            do! assertEquals typeof<CurriedAction<decimal, decimal, string, string>> <| convNconvB.GetType()
            let convNconvC = convNconvB.Invoke(0.003m)
            do! assertEquals typeof<CurriedAction<decimal, string, string>> <| convNconvC.GetType()
            let convNconvD = convNconvC.Invoke(0.0004m)
            do! assertEquals typeof<CurriedAction<string, string>> <| convNconvD.GetType()
            let convNconvE = convNconvD.Invoke(" = ")
            do! assertEquals typeof<Action<string>> <| convNconvE.GetType()
            do convNconvE.Invoke("9876543210.1234")
            do! assertEquals "9876543210.1234 = 9876543210.1234" !result
        }

    module CurryingExtensionTest =
        open Funcy.Extensions

        let ``Curry ((T1, T2) -> TReturn) = T1 -> T2 -> TReturn`` = test {
            let add = Func<int, int, int>(fun x y -> x + y)
            let sut = add.Curry()
            do! assertEquals typeof<CurriedFunction<int, int, int>> <| sut.GetType()
            let add2 = sut.Invoke(2)
            do! assertEquals typeof<Func<int, int>> <| add2.GetType()
            do! assertEquals 5 <| add2.Invoke(3)
        }

        let ``Curry ((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> TReturn) = T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> TReturn`` = test {
            let convNconv = Func<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2)
            let sut = convNconv.Curry()
            do! assertEquals "9876543210.1234 = 9876543210.1234"
                <| sut.Invoke(0uy)
                        .Invoke(10uy)
                        .Invoke(200s)
                        .Invoke(3000s)
                        .Invoke(40000)
                        .Invoke(500000)
                        .Invoke(6000000)
                        .Invoke(70000000L)
                        .Invoke(800000000L)
                        .Invoke(9000000000L)
                        .Invoke(0.1m)
                        .Invoke(0.02m)
                        .Invoke(0.003m)
                        .Invoke(0.0004m)
                        .Invoke(" = ")
                        .Invoke("9876543210.1234")
        }

        let ``Curry ((T1, T2) -> void) = T1 -> T2 -> void`` = test {
            let result = ref 0
            let add = Action<int, int>(fun x y -> result := x + y)
            let sut = add.Curry()
            do! assertEquals typeof<CurriedAction<int, int>> <| sut.GetType()
            let add2 = sut.Invoke(2)
            do! assertEquals typeof<Action<int>> <| add2.GetType()
            do add2.Invoke(3)
            do! assertEquals 5 !result
        }

        let ``Curry ((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> void) = T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> void`` = test {
            let result = ref ""
            let convNconv = Action<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    result := Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2)
            let sut = convNconv.Curry()
            do sut.Invoke(0uy)
                    .Invoke(10uy)
                    .Invoke(200s)
                    .Invoke(3000s)
                    .Invoke(40000)
                    .Invoke(500000)
                    .Invoke(6000000)
                    .Invoke(70000000L)
                    .Invoke(800000000L)
                    .Invoke(9000000000L)
                    .Invoke(0.1m)
                    .Invoke(0.02m)
                    .Invoke(0.003m)
                    .Invoke(0.0004m)
                    .Invoke(" = ")
                    .Invoke("9876543210.1234")
            do! assertEquals "9876543210.1234 = 9876543210.1234" !result
        }

    module ImplicitConversionTest =
        open Funcy.Computations
        open Funcy.Extensions

        let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)   // for implicit conversion in F#

        let ``Curry ((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> TReturn) = T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> TReturn`` = test {
            let convNconv = Func<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2)
            let sut = convNconv.Curry()
            let m0 = Maybe.Some(sut)
            let m1 = Maybe.Some(0uy).FMap(!> m0.ToSome().Value)
            let m2 = Maybe.Some(10uy).FMap(!> m1.ToSome().Value)
            let m3 = Maybe.Some(200s).FMap(!> m2.ToSome().Value)
            let m4 = Maybe.Some(3000s).FMap(!> m3.ToSome().Value)
            let m5 = Maybe.Some(40000).FMap(!> m4.ToSome().Value)
            let m6 = Maybe.Some(500000).FMap(!> m5.ToSome().Value)
            let m7 = Maybe.Some(6000000).FMap(!> m6.ToSome().Value)
            let m8 = Maybe.Some(70000000L).FMap(!> m7.ToSome().Value)
            let m9 = Maybe.Some(800000000L).FMap(!> m8.ToSome().Value)
            let m10 = Maybe.Some(9000000000L).FMap(!> m9.ToSome().Value)
            let m11 = Maybe.Some(0.1m).FMap(!> m10.ToSome().Value)
            let m12 = Maybe.Some(0.02m).FMap(!> m11.ToSome().Value)
            let m13 = Maybe.Some(0.003m).FMap(!> m12.ToSome().Value)
            let m14 = Maybe.Some(0.0004m).FMap(!> m13.ToSome().Value)
            let m15 = Maybe.Some(" = ").FMap(!> m14.ToSome().Value)
            let m16 = Maybe.Some("9876543210.1234").Apply(m15)
            do! assertEquals "9876543210.1234 = 9876543210.1234" <| m16.ToSome().Value
        }

        let ``Curry ((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> void) = T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> void`` = test {
            let result = ref ""
            let convNconv = Action<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    result := Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2)
            let sut = convNconv.Curry()
            let m0 = Maybe.Some(sut)
            let m1 = Maybe.Some(0uy).FMap(!> m0.ToSome().Value)
            let m2 = Maybe.Some(10uy).FMap(!> m1.ToSome().Value)
            let m3 = Maybe.Some(200s).FMap(!> m2.ToSome().Value)
            let m4 = Maybe.Some(3000s).FMap(!> m3.ToSome().Value)
            let m5 = Maybe.Some(40000).FMap(!> m4.ToSome().Value)
            let m6 = Maybe.Some(500000).FMap(!> m5.ToSome().Value)
            let m7 = Maybe.Some(6000000).FMap(!> m6.ToSome().Value)
            let m8 = Maybe.Some(70000000L).FMap(!> m7.ToSome().Value)
            let m9 = Maybe.Some(800000000L).FMap(!> m8.ToSome().Value)
            let m10 = Maybe.Some(9000000000L).FMap(!> m9.ToSome().Value)
            let m11 = Maybe.Some(0.1m).FMap(!> m10.ToSome().Value)
            let m12 = Maybe.Some(0.02m).FMap(!> m11.ToSome().Value)
            let m13 = Maybe.Some(0.003m).FMap(!> m12.ToSome().Value)
            let m14 = Maybe.Some(0.0004m).FMap(!> m13.ToSome().Value)
            let m15 = Maybe.Some(" = ").FMap(!> m14.ToSome().Value)
            let m16 = m15.FMap(fun f -> f.Invoke("9876543210.1234"))
            do! assertEquals "9876543210.1234 = 9876543210.1234" !result
        }

    module UncurryTest =

        let ``Uncurry (T1 -> T2 -> TReturn) = (T1, T2) -> TReturn`` = test {
            let curried = Currying.Curry(Func<int, int, int>(fun x y -> x + y))
            let uncurried = Currying.Uncurry(curried)
            do! assertEquals typeof<Func<int, int, int>> <| uncurried.GetType()
            do! assertEquals 5 <| uncurried.Invoke(2, 3)
        }

        let ``Uncurry (T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> TReturn) = (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> TReturn`` = test {
            let convNconv = Func<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2)
            let curried1 = Currying.Curry(convNconv)
            let uncurried1 = Currying.Uncurry(curried1)
            do! assertEquals typeof<Func<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried1.GetType()
            do! assertEquals "9876543210.1234 = 9876543210.1234"
                <| uncurried1.Invoke(0uy, 10uy, 200s, 3000s, 40000, 500000, 6000000, 70000000L, 800000000L, 9000000000L, 0.1m, 0.02m, 0.003m, 0.0004m, " = ", "9876543210.1234")
            let curried2 = curried1.Invoke(0uy)
            let uncurried2 = Currying.Uncurry(curried2)
            do! assertEquals typeof<Func<byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried2.GetType()
            let curried3 = curried2.Invoke(10uy)
            let uncurried3 = Currying.Uncurry(curried3)
            do! assertEquals typeof<Func<int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried3.GetType()
            let curried4 = curried3.Invoke(200s)
            let uncurried4 = Currying.Uncurry(curried4)
            do! assertEquals typeof<Func<int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried4.GetType()
            let curried5 = curried4.Invoke(3000s)
            let uncurried5 = Currying.Uncurry(curried5)
            do! assertEquals typeof<Func<int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried5.GetType()
            let curried6 = curried5.Invoke(40000)
            let uncurried6 = Currying.Uncurry(curried6)
            do! assertEquals typeof<Func<int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried6.GetType()
            let curried7 = curried6.Invoke(500000)
            let uncurried7 = Currying.Uncurry(curried7)
            do! assertEquals typeof<Func<int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried7.GetType()
            let curried8 = curried7.Invoke(6000000)
            let uncurried8 = Currying.Uncurry(curried8)
            do! assertEquals typeof<Func<int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried8.GetType()
            let curried9 = curried8.Invoke(70000000L)
            let uncurried9 = Currying.Uncurry(curried9)
            do! assertEquals typeof<Func<int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried9.GetType()
            let curried10 = curried9.Invoke(800000000L)
            let uncurried10 = Currying.Uncurry(curried10)
            do! assertEquals typeof<Func<int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried10.GetType()
            let curried11 = curried10.Invoke(9000000000L)
            let uncurried11 = Currying.Uncurry(curried11)
            do! assertEquals typeof<Func<decimal, decimal, decimal, decimal, string, string, string>> <| uncurried11.GetType()
            let curried12 = curried11.Invoke(0.1m)
            let uncurried12 = Currying.Uncurry(curried12)
            do! assertEquals typeof<Func<decimal, decimal, decimal, string, string, string>> <| uncurried12.GetType()
            let curried13 = curried12.Invoke(0.02m)
            let uncurried13 = Currying.Uncurry(curried13)
            do! assertEquals typeof<Func<decimal, decimal, string, string, string>> <| uncurried13.GetType()
            let curried14 = curried13.Invoke(0.003m)
            let uncurried14 = Currying.Uncurry(curried14)
            do! assertEquals typeof<Func<decimal, string, string, string>> <| uncurried14.GetType()
            let curried15 = curried14.Invoke(0.0004m)
            let uncurried15 = Currying.Uncurry(curried15)
            do! assertEquals typeof<Func<string, string, string>> <| uncurried15.GetType()
        }

        let ``(T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> TReturn).Uncurry() = (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> TReturn`` = test {
            let convNconv = Func<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2)
            let curried1 = Currying.Curry(convNconv)
            let uncurried1 = curried1.Uncurry()
            do! assertEquals typeof<Func<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried1.GetType()
            do! assertEquals "9876543210.1234 = 9876543210.1234"
                <| uncurried1.Invoke(0uy, 10uy, 200s, 3000s, 40000, 500000, 6000000, 70000000L, 800000000L, 9000000000L, 0.1m, 0.02m, 0.003m, 0.0004m, " = ", "9876543210.1234")
            let curried2 = curried1.Invoke(0uy)
            let uncurried2 = curried2.Uncurry()
            do! assertEquals typeof<Func<byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried2.GetType()
            let curried3 = curried2.Invoke(10uy)
            let uncurried3 = curried3.Uncurry()
            do! assertEquals typeof<Func<int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried3.GetType()
            let curried4 = curried3.Invoke(200s)
            let uncurried4 = curried4.Uncurry()
            do! assertEquals typeof<Func<int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried4.GetType()
            let curried5 = curried4.Invoke(3000s)
            let uncurried5 = curried5.Uncurry()
            do! assertEquals typeof<Func<int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried5.GetType()
            let curried6 = curried5.Invoke(40000)
            let uncurried6 = curried6.Uncurry()
            do! assertEquals typeof<Func<int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried6.GetType()
            let curried7 = curried6.Invoke(500000)
            let uncurried7 = curried7.Uncurry()
            do! assertEquals typeof<Func<int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried7.GetType()
            let curried8 = curried7.Invoke(6000000)
            let uncurried8 = curried8.Uncurry()
            do! assertEquals typeof<Func<int64, int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried8.GetType()
            let curried9 = curried8.Invoke(70000000L)
            let uncurried9 = curried9.Uncurry()
            do! assertEquals typeof<Func<int64, int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried9.GetType()
            let curried10 = curried9.Invoke(800000000L)
            let uncurried10 = curried10.Uncurry()
            do! assertEquals typeof<Func<int64, decimal, decimal, decimal, decimal, string, string, string>>
                <| uncurried10.GetType()
            let curried11 = curried10.Invoke(9000000000L)
            let uncurried11 = curried11.Uncurry()
            do! assertEquals typeof<Func<decimal, decimal, decimal, decimal, string, string, string>> <| uncurried11.GetType()
            let curried12 = curried11.Invoke(0.1m)
            let uncurried12 = curried12.Uncurry()
            do! assertEquals typeof<Func<decimal, decimal, decimal, string, string, string>> <| uncurried12.GetType()
            let curried13 = curried12.Invoke(0.02m)
            let uncurried13 = curried13.Uncurry()
            do! assertEquals typeof<Func<decimal, decimal, string, string, string>> <| uncurried13.GetType()
            let curried14 = curried13.Invoke(0.003m)
            let uncurried14 = curried14.Uncurry()
            do! assertEquals typeof<Func<decimal, string, string, string>> <| uncurried14.GetType()
            let curried15 = curried14.Invoke(0.0004m)
            let uncurried15 = curried15.Uncurry()
            do! assertEquals typeof<Func<string, string, string>> <| uncurried15.GetType()
        }

        let ``Curry (T1 -> T2 -> void) = (T1, T2) -> void`` = test {
            let result = ref 0
            let curried = Currying.Curry(Action<int, int>(fun x y -> result := x + y))
            let sut = Currying.Uncurry(curried)
            do! assertEquals typeof<Action<int, int>> <| sut.GetType()
            sut.Invoke(2, 3)
            do! assertEquals 5 !result
        }

        let ``Uncurry (T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> void) = (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> void`` = test {
            let result = ref ""
            let curried1 = Currying.Curry(
                            Action<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    result := Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2))
            let uncurried1 = Currying.Uncurry(curried1)
            do! assertEquals typeof<Action<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried1.GetType()
            uncurried1.Invoke(0uy, 10uy, 200s, 3000s, 40000, 500000, 6000000, 70000000L, 800000000L, 9000000000L, 0.1m, 0.02m, 0.003m, 0.0004m, " = ", "9876543210.1234")
            do! assertEquals "9876543210.1234 = 9876543210.1234" !result
            let curried2 = curried1.Invoke(0uy)
            let uncurried2 = Currying.Uncurry(curried2)
            do! assertEquals typeof<Action<byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried2.GetType()
            let curried3 = curried2.Invoke(10uy)
            let uncurried3 = Currying.Uncurry(curried3)
            do! assertEquals typeof<Action<int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried3.GetType()
            let curried4 = curried3.Invoke(200s)
            let uncurried4 = Currying.Uncurry(curried4)
            do! assertEquals typeof<Action<int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried4.GetType()
            let curried5 = curried4.Invoke(3000s)
            let uncurried5 = Currying.Uncurry(curried5)
            do! assertEquals typeof<Action<int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried5.GetType()
            let curried6 = curried5.Invoke(40000)
            let uncurried6 = Currying.Uncurry(curried6)
            do! assertEquals typeof<Action<int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried6.GetType()
            let curried7 = curried6.Invoke(500000)
            let uncurried7 = Currying.Uncurry(curried7)
            do! assertEquals typeof<Action<int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried7.GetType()
            let curried8 = curried7.Invoke(6000000)
            let uncurried8 = Currying.Uncurry(curried8)
            do! assertEquals typeof<Action<int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried8.GetType()
            let curried9 = curried8.Invoke(70000000L)
            let uncurried9 = Currying.Uncurry(curried9)
            do! assertEquals typeof<Action<int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried9.GetType()
            let curried10 = curried9.Invoke(800000000L)
            let uncurried10 = Currying.Uncurry(curried10)
            do! assertEquals typeof<Action<int64, decimal, decimal, decimal, decimal, string, string>> <| uncurried10.GetType()
            let curried11 = curried10.Invoke(9000000000L)
            let uncurried11 = Currying.Uncurry(curried11)
            do! assertEquals typeof<Action<decimal, decimal, decimal, decimal, string, string>> <| uncurried11.GetType()
            let curried12 = curried11.Invoke(0.1m)
            let uncurried12 = Currying.Uncurry(curried12)
            do! assertEquals typeof<Action<decimal, decimal, decimal, string, string>> <| uncurried12.GetType()
            let curried13 = curried12.Invoke(0.02m)
            let uncurried13 = Currying.Uncurry(curried13)
            do! assertEquals typeof<Action<decimal, decimal, string, string>> <| uncurried13.GetType()
            let curried14 = curried13.Invoke(0.003m)
            let uncurried14 = Currying.Uncurry(curried14)
            do! assertEquals typeof<Action<decimal, string, string>> <| uncurried14.GetType()
            let curried15 = curried14.Invoke(0.0004m)
            let uncurried15 = Currying.Uncurry(curried15)
            do! assertEquals typeof<Action<string, string>> <| uncurried15.GetType()
        }

        let ``(T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> T7 -> T8 -> T9 -> T10 -> T11 -> T12 -> T13 -> T14 -> T15 -> T16 -> void).Uncurry() = (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) -> void`` = test {
            let result = ref ""
            let curried1 = Currying.Curry(
                            Action<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>(
                                fun b1 b2 s1 s2 i1 i2 i3 l1 l2 l3 m1 m2 m3 m4 t1 t2 ->
                                    result := Convert.ToString(Convert.ToDecimal(Convert.ToInt64(Convert.ToInt32(Convert.ToInt16(b1 + b2) + s1 + s2) + i1 + i2 + i3) + l1 + l2 + l3) + m1 + m2 + m3 + m4) + t1 + t2))
            let uncurried1 = curried1.Uncurry()
            do! assertEquals typeof<Action<byte, byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried1.GetType()
            uncurried1.Invoke(0uy, 10uy, 200s, 3000s, 40000, 500000, 6000000, 70000000L, 800000000L, 9000000000L, 0.1m, 0.02m, 0.003m, 0.0004m, " = ", "9876543210.1234")
            do! assertEquals "9876543210.1234 = 9876543210.1234" !result
            let curried2 = curried1.Invoke(0uy)
            let uncurried2 = curried2.Uncurry()
            do! assertEquals typeof<Action<byte, int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried2.GetType()
            let curried3 = curried2.Invoke(10uy)
            let uncurried3 = curried3.Uncurry()
            do! assertEquals typeof<Action<int16, int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried3.GetType()
            let curried4 = curried3.Invoke(200s)
            let uncurried4 = curried4.Uncurry()
            do! assertEquals typeof<Action<int16, int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried4.GetType()
            let curried5 = curried4.Invoke(3000s)
            let uncurried5 = curried5.Uncurry()
            do! assertEquals typeof<Action<int, int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried5.GetType()
            let curried6 = curried5.Invoke(40000)
            let uncurried6 = curried6.Uncurry()
            do! assertEquals typeof<Action<int, int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried6.GetType()
            let curried7 = curried6.Invoke(500000)
            let uncurried7 = curried7.Uncurry()
            do! assertEquals typeof<Action<int, int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried7.GetType()
            let curried8 = curried7.Invoke(6000000)
            let uncurried8 = curried8.Uncurry()
            do! assertEquals typeof<Action<int64, int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried8.GetType()
            let curried9 = curried8.Invoke(70000000L)
            let uncurried9 = curried9.Uncurry()
            do! assertEquals typeof<Action<int64, int64, decimal, decimal, decimal, decimal, string, string>>
                <| uncurried9.GetType()
            let curried10 = curried9.Invoke(800000000L)
            let uncurried10 = curried10.Uncurry()
            do! assertEquals typeof<Action<int64, decimal, decimal, decimal, decimal, string, string>> <| uncurried10.GetType()
            let curried11 = curried10.Invoke(9000000000L)
            let uncurried11 = curried11.Uncurry()
            do! assertEquals typeof<Action<decimal, decimal, decimal, decimal, string, string>> <| uncurried11.GetType()
            let curried12 = curried11.Invoke(0.1m)
            let uncurried12 = curried12.Uncurry()
            do! assertEquals typeof<Action<decimal, decimal, decimal, string, string>> <| uncurried12.GetType()
            let curried13 = curried12.Invoke(0.02m)
            let uncurried13 = curried13.Uncurry()
            do! assertEquals typeof<Action<decimal, decimal, string, string>> <| uncurried13.GetType()
            let curried14 = curried13.Invoke(0.003m)
            let uncurried14 = curried14.Uncurry()
            do! assertEquals typeof<Action<decimal, string, string>> <| uncurried14.GetType()
            let curried15 = curried14.Invoke(0.0004m)
            let uncurried15 = curried15.Uncurry()
            do! assertEquals typeof<Action<string, string>> <| uncurried15.GetType()
        }
