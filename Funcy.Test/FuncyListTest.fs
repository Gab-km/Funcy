namespace Funcy.Test

open Funcy
open Persimmon
open UseTestNameByReflection

module FuncyListTest =
    // constructor function
    let ``FuncyList.Cons(1, FuncyList.Nil()) is type of Cons<int>`` = test {
        let cons = FuncyList.Cons(1, FuncyList.Nil())
        do! assertEquals typeof<Cons<int>> <| cons.GetType()
    }
    let ``FuncyList<int>.Nil() is type of Nil<int>`` = test {
        let nil = FuncyList<int>.Nil()
        do! assertEquals typeof<Nil<int>> <| nil.GetType()
    }
    let ``FuncyList<string>.Construct() is type of Nil<string>`` = test {
        let empty = FuncyList<string>.Construct()
        do! assertEquals typeof<Nil<string>> <| empty.GetType()
    }
    let ``FuncyList<int>.Construct(1, 2, 3) = Cons<int>(1, Cons<int>(2, Cons<int>(3, Nil<int>())))`` = test {
        let list = FuncyList.Construct(1, 2, 3)
        do! assertEquals typeof<Cons<int>> <| list.GetType()
        do! assertEquals list <| (FuncyList.Cons(1, FuncyList.Cons(2, FuncyList.Cons(3, FuncyList.Nil()))) :> FuncyList<int>)
    }
    // cons
    let ``Cons<T>.IsCons should be true`` = test {
        let cons = FuncyList.Cons(1, FuncyList.Nil())
        do! assertPred <| cons.IsCons
    }
    let ``Cons<T>.IsNil should be false`` = test {
        let cons = FuncyList.Cons(1, FuncyList.Nil())
        do! assertPred <| not cons.IsNil
    }
    // nil
    let ``Nil<T>.IsCons should be false`` = test {
        let cons = FuncyList<int>.Nil()
        do! assertPred <| not cons.IsCons
    }
    let ``Nil<T>.IsNil should be true`` = test {
        let cons = FuncyList<int>.Nil()
        do! assertPred <| cons.IsNil
    }
    // head(cons(h, t)) = h
    let ``FuncyList.Cons(h, _).Head = h`` = test {
        let list = FuncyList<int>.Cons(1, FuncyList<int>.Nil())
        let head = list.Head;
        do! assertEquals head <| 1
    }
    // tail(cons(h, t)) = t
    let ``FuncyList<int>.Cons(_, t).Tail = t`` = test {
        let list = FuncyList.Cons(1, FuncyList.Cons(2, FuncyList.Nil()))
        let tail = list.Tail;
        do! assertEquals tail <| (FuncyList.Cons(2, FuncyList<int>.Nil()) :> FuncyList<int>)
    }
    // equality on Cons
    let ``equality on Cons<T> depends on its contents`` = test {
        let lhs = FuncyList.Cons(1, FuncyList.Nil())
        let rhs = FuncyList.Cons(1, FuncyList.Nil())
        do! assertEquals lhs rhs
        do! assertPred (lhs = rhs)
        do! assertEquals <|| (lhs.GetHashCode(), rhs.GetHashCode())
    }
    let ``when headers differ, Cons<T> should differ`` = test {
        let lhs = FuncyList.Cons("hoge", FuncyList.Nil())
        let rhs = FuncyList.Cons("fuga", FuncyList.Nil())
        do! assertNotEquals lhs rhs
    }
    let ``content types matter on equality of Cons<T>`` = test {
        let lhs = FuncyList.Cons(3.14, FuncyList.Nil())
        let rhs = FuncyList.Cons(3.14f, FuncyList.Nil())
        do! (not >> assertPred) <| lhs.Equals(rhs)
    }
    let ``when tails differ, Cons<T> should differ`` = test {
        let lhs = FuncyList.Cons(1, FuncyList.Cons(2, FuncyList.Nil()))
        let rhs = FuncyList.Cons(1, FuncyList.Nil())
        do! assertNotEquals lhs rhs
    }
    // equality on Nil
    let ``equality on Nil<T> depends on its type argument`` = test {
        let lhs = FuncyList<int>.Nil()
        let rhs = FuncyList<int>.Nil()
        do! assertEquals lhs rhs
        do! assertPred (lhs = rhs)
        do! assertEquals <|| (lhs.GetHashCode(), rhs.GetHashCode())
    }
    let ``when type arguments differ, Nil<T> should differ`` = test {
        let lhs = FuncyList<int>.Nil()
        let rhs = FuncyList<string>.Nil()
        do! (not >> assertPred) <| lhs.Equals(rhs)
    }
    // equality between Cons and Nil
    let ``Cons<T> never equals to Nil<T>`` = test {
        let cons = FuncyList.Cons(1, FuncyList.Nil())
        let nil = FuncyList<int>.Nil()
        do! assertNotEquals (cons :> FuncyList<int>) (nil :> FuncyList<int>)
    }
    // Cons<T> constructor is monotone
    let ``Cons<T> reserve ordering`` = test {
        let cons1 = FuncyList.Cons(1, FuncyList.Nil())
        let cons2 = FuncyList.Cons(2, FuncyList.Nil())
        do! assertPred (cons1 < cons2)
    }
    // compareto, dictionary order
    let ``order on FuncyList<T> is dictionary order`` = test {
        let cons124 = FuncyList.Cons(1, FuncyList.Cons(2, FuncyList.Cons(4, FuncyList.Nil())))
        let cons13 = FuncyList.Cons(1, FuncyList.Cons(3, FuncyList.Nil()))
        do! assertPred (cons124 < cons13)
    }
    // comparison between Cons and Nil
    let ``Cons<T> is larger than Nil<T>`` = test {
        let cons = FuncyList.Cons(1, FuncyList.Nil())
        let nil = FuncyList<int>.Nil()
        do! assertPred ((cons :> FuncyList<int>) > (nil :> FuncyList<int>))
    }

module FuncyListIterableTest =
    open System.Collections.Generic
    open System.Linq

    let ``FuncyList<T>.GetEnumerator<T>() returns IEnumerator<T>`` = test {
        let sut = FuncyList.Construct([|"hoge"; "fuga"; "bar"|])
        let enumerator = sut.GetEnumerator()
        do! assertPred <| enumerator.MoveNext()
        do! assertEquals "hoge" enumerator.Current
        do! assertPred <| enumerator.MoveNext()
        do! assertEquals "fuga" enumerator.Current
        do! assertPred <| enumerator.MoveNext()
        do! assertEquals "bar" enumerator.Current
        do! assertPred <| (not <| enumerator.MoveNext())
    }
