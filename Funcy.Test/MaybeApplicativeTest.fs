namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module MaybeApplicativeTest =
    open Funcy.Extensions

    let ``Apply: Maybe<int> -> Maybe<int -> int> -> Maybe<int>`` = test {
        let maybeX = Maybe.Some(1)
        let maybeF = Maybe.Some(Func<int, int>(fun x -> x + 3))
        let sut = maybeX.Apply(maybeF)
        do! assertEquals typeof<Some<int>> <| sut.GetType()
        do! assertPred sut.IsSome
        do! assertEquals  4 <| sut.ToSome().Value
    }

    let ``Apply: Maybe<DateTime> -> Maybe<DateTime -> string> -> Maybe<string>`` = test {
        let sut = Maybe.Some(DateTime(2015, 7, 9)).Apply(
                    Maybe.Some(Func<DateTime, string>(fun dt -> dt.ToString("yyyy/MM/dd"))))
        do! assertEquals typeof<Some<string>> <| sut.GetType()
        do! assertPred sut.IsSome
        do! assertEquals "2015/07/09" <| sut.ToSome().Value
    }

    let ``Some<float -> bool> <*> None<float> = None<bool>`` = test {
        let maybeX = Maybe.None()
        let maybeF = Maybe.Some(Func<float, bool>(fun f -> f > 3.14))
        let sut = maybeX.Apply(maybeF)
        do! assertEquals typeof<None<bool>> <| sut.GetType()
        do! assertPred sut.IsNone
    }

    let ``None<string -> decimal> <*> Some<string> = None<decimal>`` = test {
        let maybeX = Maybe.Some("hoge")
        let maybeF = Maybe<Func<string, decimal>>.None()
        let sut = maybeX.Apply(maybeF)
        do! assertEquals typeof<None<decimal>> <| sut.GetType()
        do! assertPred sut.IsNone
    }

    let ``Some<int> <* Some<string> = Some<int>`` = test {
        let sut = Maybe.Some(2).ApplyLeft(Maybe.Some("hoge"))
        do! assertEquals typeof<Some<int>> <| sut.GetType()
        do! assertEquals sut <| (Maybe.Some(2) :> IMaybe<int>)
    }

    let ``Some<float> *> Some<decimal> = Some<decimal>`` = test {
        let sut = Maybe.Some(3.14).ApplyRight(Maybe.Some(3.14m))
        do! assertEquals typeof<Some<decimal>> <| sut.GetType()
        do! assertEquals sut <| (Maybe.Some(3.14m) :> IMaybe<decimal>)
    }

    let ``None<bool> <* Some<long> = None<bool>`` = test {
        let none : None<bool> = Maybe.None()
        let sut = none.ApplyLeft(Maybe.Some(20L))
        do! assertEquals typeof<None<bool>> <| sut.GetType()
        do! assertEquals sut <| (Maybe.None() :> IMaybe<bool>)
    }

    let ``None<string> *> Some<byte> = Some<byte>`` = test {
        let none : None<string> = Maybe.None()
        let sut = none.ApplyRight(Maybe.Some(0xCAuy))
        do! assertEquals typeof<Some<byte>> <| sut.GetType()
        do! assertEquals sut <| (Maybe.Some(0xCAuy) :> IMaybe<byte>)
    }

    let ``Some(+ 5) <*> Some(3) = Some(8)`` = test {
        let add = Func<int, int, int>(fun x y -> x + y)
        let maybeAdd5 = Maybe.Some(add.Curry().Invoke(5))
        let maybe3 = Maybe.Some(3)
        let sut = maybe3.Apply(maybeAdd5)
        do! assertPred sut.IsSome
        do! assertEquals 8 <| sut.ToSome().Value
    }

    let ``Some(+) <*> Some(5) = Some(+ 5)`` = test {
        let add = Func<int, int, int>(fun x y -> x + y)
        let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)   // for implicit conversion in F#
        let maybeAdd = Maybe.Some(!> add.Curry())
        let maybe5 = Maybe.Some(5)
        let sut = maybe5.Apply(maybeAdd)
        do! assertEquals 8 <| Maybe.Some(3).Apply(sut).ToSome().Value
    }
