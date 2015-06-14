namespace Funcy.Test

open Funcy
open Funcy.Computations
open Persimmon

module MaybeComputationTest =
    let t1 = test "Some + Some should return Some value" {
        let sut = Maybe.Some(1).ComputeWith (fun x ->
                    Maybe.Some(2).Compute (fun y ->
                        x + y) :> IComputable<int>)
        do! assertEquals typeof<Some<int>> <| sut.GetType()
        do! assertEquals 3 <| sut.ToSome().Value
    }

    let t2 = test "Some + None should return None value" {
        let maybeX = Maybe.Some(3)
        let maybeY = Maybe.None()
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.Compute (fun y ->
                        x + y) :> IComputable<int>)
        do! assertEquals typeof<None<int>> <| sut.GetType()
        do! assertPred <| sut.ToNone().IsNone
    }

    let t3 = test "None + Some should return None value" {
        let maybeX = Maybe.None()
        let maybeY = Maybe.Some(4)
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.Compute (fun y ->
                        x + y) :> IComputable<int>)
        do! assertEquals typeof<None<int>> <| sut.GetType()
        do! assertPred <| sut.ToNone().IsNone
    }

    let t4 = test "None + None should return None value" {
        let maybeX = Maybe.None()
        let maybeY = Maybe.None()
        let sut = maybeX.ComputeWith (fun x ->
                    maybeY.Compute (fun y ->
                        x + y) :> IComputable<int>)
        do! assertEquals typeof<None<int>> <| sut.GetType()
        do! assertPred <| sut.ToNone().IsNone
    }

    let t5 = test "Hello world!" {
        let sut = Maybe.Some("Hello").ComputeWith (fun hello ->
                    Maybe.Some("world").Compute (fun world ->
                        hello + " " + world + "!") :> IComputable<string>)
        do! assertEquals typeof<Some<string>> <| sut.GetType()
        do! assertEquals "Hello world!" <| sut.ToSome().Value
    }
