namespace Funcy.Test

open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module FuncyListComputationTest =
    let ``Cons + Cons should return Cons value`` = test {
        let sut = FuncyList.Construct([|1|]).ComputeWith (fun x ->
                    FuncyList.Construct([|2|]).FMap (fun y ->
                        x + y))
        do! assertEquals typeof<Cons<int>> <| sut.GetType()
        let expected = FuncyList.Construct([|3|])
        do! assertEquals expected sut
    }

    let ``[0, 1] >>= (\x -> [2, 4] >>= (\y -> return $ x + y)) should return [2, 4, 3, 5]`` = test {
        let sut = FuncyList.Construct([|0; 1|]).ComputeWith (fun x ->
                    FuncyList.Construct([|2; 4|]).FMap (fun y ->
                        x + y))
        do! assertEquals typeof<Cons<int>> <| sut.GetType()
        let expected = FuncyList.Construct([|2; 4; 3; 5|])
        do! assertEquals expected sut
    }

    let ``Cons + Nil should return Nil value`` = test {
        let cons = FuncyList.Construct([|1|])
        let nil = FuncyList.Nil()
        let sut = cons.ComputeWith(fun x -> nil.FMap(fun y -> x + y))
        do! assertEquals typeof<Nil<int>> <| sut.GetType()
        let expected = FuncyList.Nil() :> FuncyList<int>
        do! assertEquals expected sut
    }

    let ``Nil + Cons should return Nil value`` = test {
        let nil = FuncyList.Nil()
        let cons = FuncyList.Construct([|2|])
        let sut = nil.ComputeWith(fun x -> cons.FMap(fun y -> x + y))
        do! assertEquals typeof<Nil<int>> <| sut.GetType()
        let expected = FuncyList.Nil() :> FuncyList<int>
        do! assertEquals expected sut
    }

    let ``Nil + Nil should return Nil value`` = test {
        let nil1 = FuncyList.Nil()
        let nil2 = FuncyList.Nil()
        let sut = nil1.ComputeWith(fun x -> nil2.FMap(fun y -> x + y))
        do! assertEquals typeof<Nil<int>> <| sut.GetType()
        let expected = FuncyList.Nil() :> FuncyList<int>
        do! assertEquals expected sut
    }
