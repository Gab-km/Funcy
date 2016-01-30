namespace Funcy.Test

open System
open System.Collections.Generic
open System.Linq
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module FuncyListApplicativeTest =
    open Funcy.Extensions

    let ``Apply: FuncyList<int> -> FuncyList<int -> int> -> FuncyList<int>`` = test {
        let flistX = FuncyList.Construct([|1; 2; 3|])
        let flistF = FuncyList.Construct
                        ([|
                            Func<int, int>(fun x -> x + 3)
                            Func<int, int>(fun x -> x * 3)
                            Func<int, int>(fun x -> x / 3)
                        |])
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<Cons<int>> <| sut.GetType()
        do! assertPred sut.IsCons
        let expected = FuncyList.Construct([|4; 5; 6; 3; 6; 9; 0; 0; 1|])
        do! assertEquals expected <| sut
    }

    let ``Apply: FuncyList<DateTime> -> FuncyList<DateTime -> string> -> FuncyList<string>`` = test {
        let flistX = FuncyList.Construct([|DateTime(2015, 10, 23); DateTime(2015, 10, 24)|])
        let flistF = FuncyList.Construct
                        ([|
                            Func<DateTime, string>(fun d1 -> d1.ToString("yyyy/MM/dd"))
                            Func<DateTime, string>(fun d2 -> d2.ToString("yyyy-MM-dd"))
                            Func<DateTime, string>(fun d3 -> d3.ToString("yyyyMMdd"))
                        |])
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<Cons<string>> <| sut.GetType()
        do! assertPred sut.IsCons
        let expected = FuncyList.Construct
                        ([|
                            "2015/10/23"; "2015/10/24"
                            "2015-10-23"; "2015-10-24"
                            "20151023"; "20151024"
                        |])
        do! assertEquals expected <| sut
    }

    let ``Cons<float -> bool> <*> Nil<float> = Nil<bool>`` = test {
        let flistX = FuncyList.Nil()
        let flistF = FuncyList.Cons(Func<float, bool>(fun f -> f > 3.14), FuncyList.Nil())
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<Nil<bool>> <| sut.GetType()
        do! assertPred sut.IsNil
    }

    let ``Nil<string -> decimal> <*> Cons<string> = Nil<decimal>`` = test {
        let flistX = FuncyList.Cons("hoge", FuncyList.Nil())
        let flistF = FuncyList<Func<string, decimal>>.Nil()
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<Nil<decimal>> <| sut.GetType()
        do! assertPred sut.IsNil
    }

    let ``Cons<int> <* Cons<string> = Cons<int>`` = test {
        let sut = FuncyList.Construct([|1; 1; 2; 3|]).ApplyLeft(FuncyList.Construct([|"hoge"; "fuga"; "bar"|]))
        do! assertEquals typeof<Cons<int>> <| sut.GetType()
        do! assertEquals sut <| FuncyList.Construct([|1; 1; 2; 3|])
    }

    let ``Cons<float> *> Cons<decimal> = Cons<decimal>`` = test {
        let sut = FuncyList.Construct([|3.14; 2.718|]).ApplyRight(FuncyList.Construct([|2.718m; 3.14m|]))
        do! assertEquals typeof<Cons<decimal>> <| sut.GetType()
        do! assertEquals sut <| FuncyList.Construct([|2.718m; 3.14m|])
    }

    let ``Nil<bool> <* Cons<long> = Nil<bool>`` = test {
        let nil = FuncyList<bool>.Nil()
        let sut = nil.ApplyLeft(FuncyList.Cons(20L, FuncyList.Nil()))
        do! assertEquals typeof<Nil<bool>> <| sut.GetType()
        do! assertEquals sut <| (FuncyList.Nil() :> FuncyList<bool>)
    }

    let ``Nil<string> *> Cons<byte> = Cons<byte>`` = test {
        let nil = FuncyList.Nil()
        let sut = nil.ApplyRight(FuncyList.Construct([|0xCAuy; 0xFEuy|]))
        do! assertEquals typeof<Cons<byte>> <| sut.GetType()
        do! assertEquals sut <| FuncyList.Cons(0xCAuy, FuncyList.Cons(0xFEuy, FuncyList.Nil()))
    }

    let ``Cons(+ 5) <*> Cons(3, 4, 10) = Cons(8, 9, 15)`` = test {
        let add = Func<int, int, int>(fun x y -> x + y)
        let flistAdd5 = FuncyList.Cons(add.Curry().Invoke(5), FuncyList.Nil())
        let flistX = FuncyList.Construct([|3; 4; 10|])
        let sut = flistX.Apply(flistAdd5)
        do! assertPred sut.IsCons
        let expected = FuncyList.Construct([|8; 9; 15|])
        do! assertEquals expected <| sut
    }

    let ``Cons(+, *, -) <*> Cons(5, 2) = Cons(+ 5, + 2, * 5, * 2, - 5, - 2)`` = test {
        let add = Func<int, int, int>(fun x y -> x + y)
        let mul = Func<int, int, int>(fun x y -> x * y)
        let sub = Func<int, int, int>(fun x y -> x - y)
        let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)   // for implicit conversion in F#
        let flistOP = FuncyList.Construct([|!> add.Curry(); !> mul.Curry(); !> sub.Curry()|])
        let flistX = FuncyList.Cons(5, FuncyList.Cons(2, FuncyList.Nil()))
        let sut = flistX.Apply(flistOP)
        let target = FuncyList.Construct([|0; 1; 2|])
        let expected = FuncyList.Construct([|5; 6; 7; 2; 3; 4; 0; 5; 10; 0; 2; 4; 5; 4; 3; 2; 1; 0|])
        do! assertEquals expected <| target.Apply(sut)
    }

    let ``Cons<T>.Point returns Cons<T>`` = test {
        let cons = FuncyList.Cons("z", FuncyList.Nil())
        let actual = cons.Point("a")
        do! assertEquals typeof<Cons<string>> <| actual.GetType()
        do! assertEquals "a" <| actual.ToCons().Head
    }

    let ``Nil<T>.Point returns Cons<T>`` = test {
        let nil = FuncyList<decimal>.Nil()
        let actual = nil.Point(3.14m)
        do! assertEquals typeof<Cons<decimal>> <| actual.GetType()
        do! assertEquals 3.14m <| actual.ToCons().Head
    }
