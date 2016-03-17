namespace Funcy.Test

open Funcy
open Persimmon
open UseTestNameByReflection

module MaybeTCTest =
    module MaybeTCAsPointedTest =
        open Funcy.Computations

        let ``MaybeTC.Some creates SomeTC<T> instance`` = test {
            let some = MaybeTC.Some(1)
            do! assertEquals typeof<SomeTC<int>> <| some.GetType()
        }
        let ``MaybeTC.None creates NoneTC<T> instance`` = test {
            let none = MaybeTC.None<string>()
            do! assertEquals typeof<NoneTC<string>> <| none.GetType()
        }
        let ``MaybeTC<T>.Point creates SomeTC<T> instance`` = test {
            let none = MaybeTC.None<int>()
            let point = none.Pointed.Point(10)
            do! assertEquals typeof<SomeTC<int>> <| point.GetType()
        }

    module GeneralTest =
        let ``SomeTC<T>.IsSome should return true`` = test {
            let some = MaybeTC.Some("hoge")
            do! assertPred some.IsSome
        }
        let ``SomeTC<T>.Value should return its value`` = test {
            let some = MaybeTC.Some(2.5).ToSome()
            do! assertEquals 2.5 some.Value
        }
        let ``SomeTC<T>.IsNone should return false`` = test {
            let some = MaybeTC.Some(3.14m)
            do! assertPred <| not some.IsNone
        }
        let ``When SomeTC<T> as MaybeTC<T> then ToNone should raise InvalidCastException`` = test {
            let some = MaybeTC.Some("egg")
            let! e = trap { some.ToNone() |> ignore }
            do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
        }
        let ``NoneTC<T>.IsSome should return false`` = test {
            let none = MaybeTC.None<int>()
            do! assertPred <| not none.IsSome
        }
        let ``NoneTC<T>.IsNone should return true`` = test {
            let none = MaybeTC.None<int>()
            do! assertPred none.IsNone
        }
        let ``When NoneTC<T> as Maybe<T> then ToSome should raise InvalidCastException`` = test {
            let none = MaybeTC.None<int list>()
            let! e = trap { none.ToSome() |> ignore }
            do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
        }
        let ``SomeTC<T> should have equality1`` = test {
            let some = MaybeTC.Some(5)
            let other = MaybeTC.Some(5)
            do! assertEquals some other
            do! assertPred (some = other)
            do! assertEquals <|| (some.GetHashCode(), other.GetHashCode())
        }
        let ``SomeTC<T> should have equality2`` = test {
            let some = MaybeTC.Some("hoge")
            let other = MaybeTC.Some("fuga")
            do! assertNotEquals some other
        }
        let ``SomeTC<T> should have equality3`` = test {
            let some = MaybeTC.Some(3.14)
            let other = MaybeTC.Some(3.14f)
            do! (not >> assertPred) <| some.Equals(other)
        }
        let ``NoneTC<T> should have equality1`` = test {
            let none = MaybeTC.None<float>()
            let other = MaybeTC.None<float>()
            do! assertEquals none other
            do! assertEquals <|| (none.GetHashCode(), other.GetHashCode())
        }
        let ``NoneTC<T> should have equality2`` = test {
            let none = MaybeTC.None<float32>()
            let other = MaybeTC.None<decimal>()
            do! (not >> assertPred) <| none.Equals(other)
        }
        let ``MaybeTC<T> should have equality`` = test {
            let some1 = MaybeTC.Some(System.DateTime(2015, 7, 10))
            let some2 = MaybeTC.Some(System.DateTime(2015, 7, 10))
            let some3 = MaybeTC.Some(System.DateTime(2015, 7, 11))
            let none = MaybeTC.None() :> MaybeTC<System.DateTime>
            do! assertEquals some1 some2
            do! assertEquals <|| (some1.GetHashCode(), some2.GetHashCode())
            do! assertNotEquals some1 some3
            do! assertNotEquals some1 none
            do! assertNotEquals some3 none
        }
        let ``MaybeTC<T> should have comparability`` = test {
            let some1 = MaybeTC.Some(1)
            let some2 = MaybeTC.Some(2)
            let none = MaybeTC.None<int>()
            do! assertPred (some1 < some2)
            do! assertPred (some2 > some1)
            do! assertPred (none < some1)
            do! assertPred (none < some2)
            do! assertPred (some1 > null)
            do! assertPred (none > null)
    }

    module FunctorTest =
        open System

        let ``fmap SomeTC<int> (int -> int) = SomeTC<int>`` = test {
            let some = MaybeTC.Some(3)
            let sut = some.FMap(Func<int, int>(fun x -> x * 4))
            do! assertEquals typeof<SomeTC<int>> <| sut.GetType()
            do! assertEquals sut <| MaybeTC.Some(12)
        }

        let ``fmap SomeTC<decimal> (decimal -> string) = SomeTC<decimal>`` = test {
            let sut = MaybeTC.Some(2.718m).FMap(Func<decimal, string>(fun d -> d.ToString()))
            do! assertEquals typeof<SomeTC<string>> <| sut.GetType()
            do! assertEquals sut <| MaybeTC.Some("2.718")
        }

        let ``fmap NoneTC<byte []> (byte [] -> byte) = NoneTC<byte>`` = test {
            let none = MaybeTC.None<byte []>()
            let sut = none.FMap(Func<byte [], byte>(fun arr -> Array.sum arr))
            do! assertEquals typeof<NoneTC<byte>> <| sut.GetType()
            do! assertEquals sut <| MaybeTC.None<byte>()
        }

        let ``SomeTC<T>.Pointed returns MaybeTC`` = test {
            let some = MaybeTC.Some("ham")
            let actual = some.Pointed
            do! assertEquals typeof<MaybeTC> <| actual.GetType()
        }

    module ApplicativeTest =
        open System
        open Funcy.Extensions

        let ``Apply: MaybeTC<int> -> MaybeTC<int -> int> -> MaybeTC<int>`` = test {
            let maybeX = MaybeTC.Some(1)
            let maybeF = MaybeTC.Some(Func<int, int>(fun x -> x + 3))
            let sut = maybeX.Apply(maybeF)
            do! assertEquals typeof<SomeTC<int>> <| sut.GetType()
            do! assertPred sut.IsSome
            do! assertEquals  4 <| sut.ToSome().Value
        }

        let ``Apply: MaybeTC<DateTime> -> MaybeTC<DateTime -> string> -> MaybeTC<string>`` = test {
            let sut = MaybeTC.Some(DateTime(2015, 7, 9)).Apply(
                        MaybeTC.Some(Func<DateTime, string>(fun dt -> dt.ToString("yyyy/MM/dd"))))
            do! assertEquals typeof<SomeTC<string>> <| sut.GetType()
            do! assertPred sut.IsSome
            do! assertEquals "2015/07/09" <| sut.ToSome().Value
        }

        let ``SomeTC<float -> bool> <*> NoneTC<float> = NoneTC<bool>`` = test {
            let maybeX = MaybeTC.None()
            let maybeF = MaybeTC.Some(Func<float, bool>(fun f -> f > 3.14))
            let sut = maybeX.Apply(maybeF)
            do! assertEquals typeof<NoneTC<bool>> <| sut.GetType()
            do! assertPred sut.IsNone
        }

        let ``NoneTC<string -> decimal> <*> SomeTC<string> = NoneTC<decimal>`` = test {
            let maybeX = MaybeTC.Some("hoge")
            let maybeF = MaybeTC.None<Func<string, decimal>>()
            let sut = maybeX.Apply(maybeF)
            do! assertEquals typeof<NoneTC<decimal>> <| sut.GetType()
            do! assertPred sut.IsNone
        }

        let ``SomeTC<int> <* SomeTC<string> = SomeTC<int>`` = test {
            let sut = MaybeTC.Some(2).ApplyLeft(MaybeTC.Some("hoge"))
            do! assertEquals typeof<SomeTC<int>> <| sut.GetType()
            do! assertEquals sut <| MaybeTC.Some(2)
        }

        let ``SomeTC<float> *> SomeTC<decimal> = SomeTC<decimal>`` = test {
            let sut = MaybeTC.Some(3.14).ApplyRight(MaybeTC.Some(3.14m))
            do! assertEquals typeof<SomeTC<decimal>> <| sut.GetType()
            do! assertEquals sut <| MaybeTC.Some(3.14m)
        }

        let ``NoneTC<bool> <* SomeTC<long> = NoneTC<bool>`` = test {
            let none = MaybeTC.None<bool>()
            let sut = none.ApplyLeft(MaybeTC.Some(20L))
            do! assertEquals typeof<NoneTC<bool>> <| sut.GetType()
            do! assertEquals sut <| (MaybeTC.None() :> MaybeTC<bool>)
        }

        let ``NoneTC<string> *> SomeTC<byte> = SomeTC<byte>`` = test {
            let none = MaybeTC.None<string>()
            let sut = none.ApplyRight(MaybeTC.Some(0xCAuy))
            do! assertEquals typeof<SomeTC<byte>> <| sut.GetType()
            do! assertEquals sut <| MaybeTC.Some(0xCAuy)
        }

        let ``SomeTC(+ 5) <*> SomeTC(3) = SomeTC(8)`` = test {
            let add = Func<int, int, int>(fun x y -> x + y)
            let maybeAdd5 = MaybeTC.Some(add.Curry().Invoke(5))
            let maybe3 = MaybeTC.Some(3)
            let sut = maybe3.Apply(maybeAdd5)
            do! assertPred sut.IsSome
            do! assertEquals 8 <| sut.ToSome().Value
        }

        let ``SomeTC(+) <*> SomeTC(5) = SomeTC(+ 5)`` = test {
            let add = Func<int, int, int>(fun x y -> x + y)
            let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)   // for implicit conversion in F#
            let maybeAdd = MaybeTC.Some(!> add.Curry())
            let maybe5 = MaybeTC.Some(5)
            let sut = maybe5.Apply(maybeAdd)
            do! assertEquals 8 <| MaybeTC.Some(3).Apply(sut).ToSome().Value
        }

        let ``(int -> int) <$> SomeTC<int> = SomeTC<int>`` = test {
            let some = MaybeTC.Some(3)
            let sut = some.FMapA(Func<int, int>(fun x -> x * 4))
            do! assertEquals typeof<SomeTC<int>> <| sut.GetType()
            do! assertEquals sut <| MaybeTC.Some(12)
        }

        let ``(double -> string) <$> NoneTC<double> = NoneTC<string>`` = test {
            let sut = MaybeTC.None().FMapA(Func<double, string>(fun d -> d.ToString()))
            do! assertPred sut.IsNone
        }

    module ComputationTest =
        let ``SomeTC + SomeTC should return SomeTC value`` = test {
            let sut = MaybeTC.Some(1).ComputeWith (fun x ->
                        MaybeTC.Some(2).Compute (fun y ->
                            x + y))
            do! assertEquals typeof<SomeTC<int>> <| sut.GetType()
            do! assertEquals 3 <| sut.ToSome().Value
        }

        let ``SomeTC + NoneTC should return NoneTC value`` = test {
            let maybeX = MaybeTC.Some(3)
            let maybeY = MaybeTC.None()
            let sut = maybeX.ComputeWith (fun x ->
                        maybeY.Compute (fun y ->
                            x + y))
            do! assertEquals typeof<NoneTC<int>> <| sut.GetType()
            do! assertPred <| sut.ToNone().IsNone
        }

        let ``NoneTC + SomeTC should return NoneTC value`` = test {
            let maybeX = MaybeTC.None()
            let maybeY = MaybeTC.Some(4)
            let sut = maybeX.ComputeWith (fun x ->
                        maybeY.Compute (fun y ->
                            x + y))
            do! assertEquals typeof<NoneTC<int>> <| sut.GetType()
            do! assertPred <| sut.ToNone().IsNone
        }

        let ``NoneTC + NoneTC should return NoneTC value`` = test {
            let maybeX = MaybeTC.None()
            let maybeY = MaybeTC.None()
            let sut = maybeX.ComputeWith (fun x ->
                        maybeY.Compute (fun y ->
                            x + y))
            do! assertEquals typeof<NoneTC<int>> <| sut.GetType()
            do! assertPred <| sut.ToNone().IsNone
        }

        let ``Hello world!`` = test {
            let sut = MaybeTC.Some("Hello").ComputeWith (fun hello ->
                        MaybeTC.Some("world").Compute (fun world ->
                            hello + " " + world + "!"))
            do! assertEquals typeof<SomeTC<string>> <| sut.GetType()
            do! assertEquals "Hello world!" <| sut.ToSome().Value
        }
