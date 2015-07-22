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

        let ``Curry ((T1, T2, T3, T4, T5) -> TReturn) = T1 -> T2 -> T3 -> T4 -> T5 -> TReturn`` = test {
            let convNconv = Func<byte, int, float, decimal, string, string>(fun b i f d s ->
                                Convert.ToString(Convert.ToDecimal(Convert.ToDouble(Convert.ToInt32(b) + i) + f) + d) + s)
            let sut = Currying.Curry(convNconv)
            do! assertEquals typeof<CurriedFunction<byte, int, float, decimal, string, string>> <| sut.GetType()
            let convNconv01 = sut.Invoke(0x01uy)
            do! assertEquals typeof<CurriedFunction<int, float, decimal, string, string>> <| convNconv01.GetType()
            do! assertEquals "3.14~pi" <| convNconv01.Invoke(2).Invoke(0.1).Invoke(0.04m).Invoke("~pi")
        }

        let ``Curry ((T1, T2, T3, T4, T5, T6) -> TReturn) = T1 -> T2 -> T3 -> T4 -> T5 -> T6 -> TReturn`` = test {
            let convNconv = Func<byte, int16, int, float, decimal, string, string>(fun b s i f d t ->
                                Convert.ToString(Convert.ToDecimal(Convert.ToDouble(Convert.ToInt32(Convert.ToInt16(b) + s) + i) + f) + d) + t)
            let sut = Currying.Curry(convNconv)
            do! assertEquals typeof<CurriedFunction<byte, int16, int, float, decimal, string, string>> <| sut.GetType()
            let convNconv00 = sut.Invoke(0x00uy)
            do! assertEquals typeof<CurriedFunction<int16, int, float, decimal, string, string>> <| convNconv00.GetType()
            do! assertEquals "3.14 : pi" <| convNconv00.Invoke(1s).Invoke(2).Invoke(0.1).Invoke(0.04m).Invoke(" : pi")
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
            let m16 = m15.Compute(fun f -> f.Invoke("9876543210.1234"))
            do! assertEquals "9876543210.1234 = 9876543210.1234" !result
        }
