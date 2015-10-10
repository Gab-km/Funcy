namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module ListFunctorTest =
    let ``fmap List<int> (int -> int) = List<int>`` = test {
        let list = List.Construct(1, 2, 3)
        let sut = list.FMap(Func<int, int>(fun x -> x * 4))
        do! assertEquals typeof<Cons<int>> <| sut.GetType()
        do! assertEquals sut <| List.Construct(4, 8, 12)
    }

    let ``fmap List<decimal> (decimal -> string) = List<string>`` = test {
        let sut = List.Construct(2.718m).FMap(Func<decimal, string>(fun d -> d.ToString()))
        do! assertEquals typeof<Cons<string>> <| sut.GetType()
        do! assertEquals sut <| List.Construct("2.718")
    }

    let ``fmap Nil<byte []> (byte [] -> byte) = byte) = Nil<byte>`` = test {
        let nil = List<byte []>.Nil()
        let sut = nil.FMap(Func<byte [], byte>(fun arr -> Array.sum arr))
        do! assertEquals typeof<Nil<byte>> <| sut.GetType()
        do! assertEquals sut <| (List<byte>.Nil() :> List<byte>)
    }
