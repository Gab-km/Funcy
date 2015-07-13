namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module MaybeApplicativeTest =
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
