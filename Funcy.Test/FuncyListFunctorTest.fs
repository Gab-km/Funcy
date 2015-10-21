namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module FuncyListFunctorTest =
    let ``fmap FuncyList<int> (int -> int) = FuncyList<int>`` = test {
        let list = FuncyList.Construct(1, 2, 3)
        let sut = list.FMap(Func<int, int>(fun x -> x * 4))
        do! assertEquals typeof<Cons<int>> <| sut.GetType()
        do! assertEquals sut <| FuncyList.Construct(4, 8, 12)
    }

    let ``fmap FuncyList<decimal> (decimal -> string) = FuncyList<string>`` = test {
        let sut = FuncyList.Construct(2.718m).FMap(Func<decimal, string>(fun d -> d.ToString()))
        do! assertEquals typeof<Cons<string>> <| sut.GetType()
        do! assertEquals sut <| FuncyList.Construct("2.718")
    }

    let ``fmap Nil<byte []> (byte [] -> byte) = byte) = Nil<byte>`` = test {
        let nil = FuncyList<byte []>.Nil()
        let sut = nil.FMap(Func<byte [], byte>(fun arr -> Array.sum arr))
        do! assertEquals typeof<Nil<byte>> <| sut.GetType()
        do! assertEquals sut <| (FuncyList<byte>.Nil() :> FuncyList<byte>)
    }
