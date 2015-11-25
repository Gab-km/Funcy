namespace Funcy.Test

open Funcy
open Persimmon
open UseTestNameByReflection

module MaybeTest =
    let ``Maybe.Some creates Some<T> instance`` = test {
        let some = Maybe.Some(1)
        do! assertEquals typeof<Some<int>> <| some.GetType()
    }
    let ``Maybe.None creates None<T> instance`` = test {
        let none = Maybe<string>.None()
        do! assertEquals typeof<None<string>> <| none.GetType()
    }
    let ``Some<T>.Value should return its value`` = test {
        let some = Maybe.Some(2.5)
        do! assertEquals 2.5 some.Value
    }
    let ``Some<T>.IsSome should return true`` = test {
        let some = Maybe.Some("hoge")
        do! assertPred some.IsSome
    }
    let ``Some<T>.IsNone should return false`` = test {
        let some = Maybe.Some(3.14m)
        do! assertPred <| not some.IsNone
    }
    let ``Some<T>.ToSome() should return Some<T> instance`` = test {
        let some = Maybe.Some(-1)
        let sut = some.ToSome()
        do! assertPred sut.IsSome
    }
    let ``Some<T>.ToNone() should raise InvalidCastException`` = test {
        let some = Maybe.Some("egg")
        let! e = trap { some.ToNone() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``None<T>.IsSome should return false`` = test {
        let none = Maybe<int>.None()
        do! assertPred <| not none.IsSome
    }
    let ``None<T>.IsNone should return true`` = test {
        let none = Maybe<int>.None()
        do! assertPred none.IsNone
    }
    let ``None<T>.ToSome() should raise InvalidCastException`` = test {
        let none = Maybe<int list>.None()
        let! e = trap { none.ToSome() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``None<T>.ToNone() should return None<T> instance`` = test {
        let none = Maybe<bool>.None()
        let sut = none.ToNone()
        do! assertPred none.IsNone
    }
    let ``Some<T> should have equality1`` = test {
        let some = Maybe.Some(5)
        let other = Maybe.Some(5)
        do! assertEquals some other
        do! assertPred (some = other)
        do! assertEquals <|| (some.GetHashCode(), other.GetHashCode())
    }
    let ``Some<T> should have equality2`` = test {
        let some = Maybe.Some("hoge")
        let other = Maybe.Some("fuga")
        do! assertNotEquals some other
    }
    let ``Some<T> should have equality3`` = test {
        let some = Maybe.Some(3.14)
        let other = Maybe.Some(3.14f)
        do! (not >> assertPred) <| some.Equals(other)
    }
    let ``None<T> should have equality1`` = test {
        let none : None<float> = Maybe.None()
        let other : None<float> = Maybe.None()
        do! assertEquals none other
        do! assertEquals <|| (none.GetHashCode(), other.GetHashCode())
    }
    let ``None<T> should have equality2`` = test {
        let none : None<float32> = Maybe.None()
        let other : None<decimal> = Maybe.None()
        do! (not >> assertPred) <| none.Equals(other)
    }
    let ``Maybe<T> should have equality`` = test {
        let some1 = Maybe.Some(System.DateTime(2015, 7, 10)) :> Maybe<System.DateTime>
        let some2 = Maybe.Some(System.DateTime(2015, 7, 10)) :> Maybe<System.DateTime>
        let some3 = Maybe.Some(System.DateTime(2015, 7, 11)) :> Maybe<System.DateTime>
        let none = Maybe.None() :> Maybe<System.DateTime>
        do! assertEquals some1 some2
        do! assertEquals <|| (some1.GetHashCode(), some2.GetHashCode())
        do! assertNotEquals some1 some3
        do! assertNotEquals some1 none
        do! assertNotEquals some3 none
    }
    let ``Maybe<T> should have comparability`` = test {
        let some1 = Maybe.Some(1) :> Maybe<int>
        let some2 = Maybe.Some(2) :> Maybe<int>
        let none = Maybe.None() :> Maybe<int>
        do! assertPred (some1 < some2)
        do! assertPred (some2 > some1)
        do! assertPred (none < some1)
        do! assertPred (none < some2)
    }

module MaybeFunctorTest =
    open System

    let ``fmap Some<int> (int -> int) = Some<int>`` = test {
        let some = Maybe.Some(3)
        let sut = some.FMap(Func<int, int>(fun x -> x * 4))
        do! assertPred sut.IsSome
        do! assertEquals sut <| (Maybe.Some(12) :> Maybe<int>)
    }

    let ``fmap Some<decimal> (decimal -> string) = Some<decimal>`` = test {
        let sut = Maybe.Some(2.718m).FMap(Func<decimal, string>(fun d -> d.ToString()))
        do! assertPred sut.IsSome
        do! assertEquals sut <| (Maybe.Some("2.718") :> Maybe<string>)
    }

    let ``fmap None<byte []> (byte [] -> byte) = None<byte>`` = test {
        let none = Maybe<byte []>.None()
        let sut = none.FMap(Func<byte [], byte>(Array.sum))
        do! assertPred sut.IsNone
    }

module MaybeApplicativeTest =
    open System
    open Funcy.Extensions

    let ``Apply: Maybe<int> -> Maybe<int -> int> -> Maybe<int>`` = test {
        let maybeX = Maybe.Some(1)
        let maybeF = Maybe.Some(Func<int, int>(fun x -> x + 3))
        let sut = maybeX.Apply(maybeF)
        do! assertPred sut.IsSome
        do! assertEquals  4 <| sut.ToSome().Value
    }

    let ``Apply: Maybe<DateTime> -> Maybe<DateTime -> string> -> Maybe<string>`` = test {
        let sut = Maybe.Some(DateTime(2015, 7, 9)).Apply(
                    Maybe.Some(Func<DateTime, string>(fun dt -> dt.ToString("yyyy/MM/dd"))))
        do! assertPred sut.IsSome
        do! assertEquals "2015/07/09" <| sut.ToSome().Value
    }

    let ``Some<float -> bool> <*> None<float> = None<bool>`` = test {
        let maybeX = Maybe.None()
        let maybeF = Maybe.Some(Func<float, bool>(fun f -> f > 3.14))
        let sut = maybeX.Apply(maybeF)
        do! assertPred sut.IsNone
    }

    let ``None<string -> decimal> <*> Some<string> = None<decimal>`` = test {
        let maybeX = Maybe.Some("hoge")
        let maybeF = Maybe<Func<string, decimal>>.None()
        let sut = maybeX.Apply(maybeF)
        do! assertPred sut.IsNone
    }

    let ``Some<int> <* Some<string> = Some<int>`` = test {
        let sut = Maybe.Some(2).ApplyLeft(Maybe.Some("hoge"))
        do! assertPred sut.IsSome
        do! assertEquals sut <| (Maybe.Some(2) :> Maybe<int>)
    }

    let ``Some<float> *> Some<decimal> = Some<decimal>`` = test {
        let sut = Maybe.Some(3.14).ApplyRight(Maybe.Some(3.14m))
        do! assertPred sut.IsSome
        do! assertEquals sut <| (Maybe.Some(3.14m) :> Maybe<decimal>)
    }

    let ``None<bool> <* Some<long> = None<bool>`` = test {
        let none : None<bool> = Maybe.None()
        let sut = none.ApplyLeft(Maybe.Some(20L))
        do! assertPred sut.IsNone
        do! assertEquals sut <| (Maybe.None() :> Maybe<bool>)
    }

    let ``None<string> *> Some<byte> = Some<byte>`` = test {
        let none : None<string> = Maybe.None()
        let sut = none.ApplyRight(Maybe.Some(0xCAuy))
        do! assertPred sut.IsSome
        do! assertEquals sut <| (Maybe.Some(0xCAuy) :> Maybe<byte>)
    }

    let ``Some(+ 5) <*> Some(3) = Some(8)`` = test {
        let add = Func<int, int, int>((+))
        let maybeAdd5 = Maybe.Some(add.Curry().Invoke(5))
        let maybe3 = Maybe.Some(3)
        let sut = maybe3.Apply(maybeAdd5)
        do! assertPred sut.IsSome
        do! assertEquals 8 <| sut.ToSome().Value
    }

    let ``Some(+) <*> Some(5) = Some(+ 5)`` = test {
        let add = Func<int, int, int>((+))
        let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)   // for implicit conversion in F#
        let maybeAdd = Maybe.Some(!> add.Curry())
        let maybe5 = Maybe.Some(5)
        let sut = maybe5.Apply(maybeAdd)
        do! assertEquals 8 <| Maybe.Some(3).Apply(sut).ToSome().Value
    }

module MaybeComputationTest =
    let ``Some + Some should return Some value`` = test {
        let sut = Maybe.Some(1).ComputeWith (fun x ->
                    Maybe.Some(2).FMap (fun y ->
                        x + y))
        do! assertPred sut.IsSome
        do! assertEquals 3 <| sut.ToSome().Value
    }

    let ``Some + None should return None value`` = test {
        let maybeX = Maybe.Some(3)
        let maybeY = Maybe.None()
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.FMap (fun y ->
                        x + y))
        do! assertPred <| sut.IsNone
    }

    let ``None + Some should return None value`` = test {
        let maybeX = Maybe.None()
        let maybeY = Maybe.Some(4)
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.FMap (fun y ->
                        x + y))
        do! assertPred <| sut.IsNone
    }

    let ``None + None should return None value`` = test {
        let maybeX = Maybe.None()
        let maybeY = Maybe.None()
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.FMap (fun y ->
                        x + y))
        do! assertPred <| sut.IsNone
    }

    let ``Hello world!`` = test {
        let sut = Maybe.Some("Hello").ComputeWith (fun hello ->
                    Maybe.Some("world").FMap (fun world ->
                        hello + " " + world + "!"))
        do! assertPred sut.IsSome
        do! assertEquals "Hello world!" <| sut.ToSome().Value
    }
