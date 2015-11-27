namespace Funcy.Test

open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module MaybeComputationTest =
    let ``Some + Some should return Some value`` = test {
        let sut = Maybe.Some(1).ComputeWith (fun x ->
                    Maybe.Some(2).FMap (fun y ->
                        x + y))
        do! assertEquals typeof<Some<int>> <| sut.GetType()
        do! assertEquals 3 <| sut.ToSome().Value
    }

    let ``Some + None should return None value`` = test {
        let maybeX = Maybe.Some(3)
        let maybeY = Maybe.None()
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.FMap (fun y ->
                        x + y))
        do! assertEquals typeof<None<int>> <| sut.GetType()
        do! assertPred <| sut.ToNone().IsNone
    }

    let ``None + Some should return None value`` = test {
        let maybeX = Maybe.None()
        let maybeY = Maybe.Some(4)
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.FMap (fun y ->
                        x + y))
        do! assertEquals typeof<None<int>> <| sut.GetType()
        do! assertPred <| sut.ToNone().IsNone
    }

    let ``None + None should return None value`` = test {
        let maybeX = Maybe.None()
        let maybeY = Maybe.None()
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.FMap (fun y ->
                        x + y))
        do! assertEquals typeof<None<int>> <| sut.GetType()
        do! assertPred <| sut.ToNone().IsNone
    }

    let ``Hello world!`` = test {
        let sut = Maybe.Some("Hello").ComputeWith (fun hello ->
                    Maybe.Some("world").FMap (fun world ->
                        hello + " " + world + "!"))
        do! assertEquals typeof<Some<string>> <| sut.GetType()
        do! assertEquals "Hello world!" <| sut.ToSome().Value
    }
