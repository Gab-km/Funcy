namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module MaybeFunctorTest =
    let ``fmap Some<int> (int -> int) = Some<int>`` = test {
        let some = Maybe.Some(3)
        let sut = some.FMap(Func<int, int>(fun x -> x * 4))
        do! assertEquals typeof<Some<int>> <| sut.GetType()
        do! assertEquals sut <| (Maybe.Some(12) :> Maybe<int>)
    }

    let ``fmap Some<decimal> (decimal -> string) = Some<decimal>`` = test {
        let sut = Maybe.Some(2.718m).FMap(Func<decimal, string>(fun d -> d.ToString()))
        do! assertEquals typeof<Some<string>> <| sut.GetType()
        do! assertEquals sut <| (Maybe.Some("2.718") :> Maybe<string>)
    }

    let ``fmap None<byte []> (byte [] -> byte) = None<byte>`` = test {
        let none = Maybe<byte []>.None()
        let sut = none.FMap(Func<byte [], byte>(fun arr -> Array.sum arr))
        do! assertEquals typeof<None<byte>> <| sut.GetType()
        do! assertEquals sut <| (Maybe<byte>.None() :> Maybe<byte>)
    }
