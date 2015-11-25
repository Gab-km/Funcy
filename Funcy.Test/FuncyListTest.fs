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

    let ``Cons<T>.GetEnumerator<T>() returns IEnumerator<T>`` = test {
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

    let ``Nil<T>.GetEnumerator<T>() returns IEnumerator<T>`` = test {
        let sut = FuncyList<string>.Nil()
        let enumerator = sut.GetEnumerator()
        do! assertPred <| (not <| enumerator.MoveNext())
    }

module FuncyListFunctorTest =
    open System

    let ``fmap FuncyList<int> (int -> int) = FuncyList<int>`` = test {
        let list = FuncyList.Construct(1, 2, 3)
        let sut = list.FMap(Func<int, int>(fun x -> x * 4))
        do! assertPred sut.IsCons
        do! assertEquals sut <| FuncyList.Construct(4, 8, 12)
    }

    let ``fmap FuncyList<decimal> (decimal -> string) = FuncyList<string>`` = test {
        let sut = FuncyList.Construct(2.718m).FMap(Func<decimal, string>(fun d -> d.ToString()))
        do! assertPred sut.IsCons
        do! assertEquals sut <| FuncyList.Construct("2.718")
    }

    let ``fmap Nil<byte []> (byte [] -> byte) = byte) = Nil<byte>`` = test {
        let nil = FuncyList<byte []>.Nil()
        let sut = nil.FMap(Func<byte [], byte>(Array.sum))
        do! assertPred sut.IsNil
        do! assertEquals sut <| (FuncyList<byte>.Nil() :> FuncyList<byte>)
    }

module FuncyListApplicativeTest =
    open System
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
        do! assertPred sut.IsNil
    }

    let ``Nil<string -> decimal> <*> Cons<string> = Nil<decimal>`` = test {
        let flistX = FuncyList.Cons("hoge", FuncyList.Nil())
        let flistF = FuncyList<Func<string, decimal>>.Nil()
        let sut = flistX.Apply(flistF)
        do! assertPred sut.IsNil
    }

    let ``Cons<int> <* Cons<string> = Cons<int>`` = test {
        let sut = FuncyList.Construct([|1; 1; 2; 3|]).ApplyLeft(FuncyList.Construct([|"hoge"; "fuga"; "bar"|]))
        do! assertPred sut.IsCons
        do! assertEquals sut <| FuncyList.Construct([|1; 1; 2; 3|])
    }

    let ``Cons<float> *> Cons<decimal> = Cons<decimal>`` = test {
        let sut = FuncyList.Construct([|3.14; 2.718|]).ApplyRight(FuncyList.Construct([|2.718m; 3.14m|]))
        do! assertPred sut.IsCons
        do! assertEquals sut <| FuncyList.Construct([|2.718m; 3.14m|])
    }

    let ``Nil<bool> <* Cons<long> = Nil<bool>`` = test {
        let nil = FuncyList<bool>.Nil()
        let sut = nil.ApplyLeft(FuncyList.Cons(20L, FuncyList.Nil()))
        do! assertPred sut.IsNil
        do! assertEquals sut <| (FuncyList.Nil() :> FuncyList<bool>)
    }

    let ``Nil<string> *> Cons<byte> = Cons<byte>`` = test {
        let nil = FuncyList.Nil()
        let sut = nil.ApplyRight(FuncyList.Construct([|0xCAuy; 0xFEuy|]))
        do! assertPred sut.IsCons
        do! assertEquals sut <| (FuncyList.Cons(0xCAuy, FuncyList.Cons(0xFEuy, FuncyList.Nil())) :> FuncyList<byte>)
    }

    let ``Cons(+ 5) <*> Cons(3, 4, 10) = Cons(8, 9, 15)`` = test {
        let add = Func<int, int, int>((+))
        let flistAdd5 = FuncyList.Cons(add.Curry().Invoke(5), FuncyList.Nil())
        let flistX = FuncyList.Construct([|3; 4; 10|])
        let sut = flistX.Apply(flistAdd5)
        do! assertPred sut.IsCons
        let expected = FuncyList.Construct([|8; 9; 15|])
        do! assertEquals expected <| sut
    }

    let ``Cons(+, *, -) <*> Cons(5, 2) = Cons(+ 5, + 2, * 5, * 2, - 5, - 2)`` = test {
        let add = Func<int, int, int>((+))
        let mul = Func<int, int, int>((*))
        let sub = Func<int, int, int>((-))
        let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)   // for implicit conversion in F#
        let flistOP = FuncyList.Construct([|!> add.Curry(); !> mul.Curry(); !> sub.Curry()|])
        let flistX = FuncyList.Cons(5, FuncyList.Cons(2, FuncyList.Nil()))
        let sut = flistX.Apply(flistOP)
        let target = FuncyList.Construct([|0; 1; 2|])
        let expected = FuncyList.Construct([|5; 6; 7; 2; 3; 4; 0; 5; 10; 0; 2; 4; 5; 4; 3; 2; 1; 0|])
        do! assertEquals expected <| target.Apply(sut)
    }

module FuncyListComputationTest =
    let ``Cons + Cons should return Cons value`` = test {
        let sut = FuncyList.Construct([|1|]).ComputeWith (fun x ->
                    FuncyList.Construct([|2|]).FMap (fun y ->
                        x + y))
        do! assertPred sut.IsCons
        let expected = FuncyList.Construct([|3|])
        do! assertEquals expected sut
    }

    let ``[0, 1] >>= (\x -> [2, 4] >>= (\y -> return $ x + y)) should return [2, 4, 3, 5]`` = test {
        let sut = FuncyList.Construct([|0; 1|]).ComputeWith (fun x ->
                    FuncyList.Construct([|2; 4|]).FMap (fun y ->
                        x + y))
        do! assertPred sut.IsCons
        let expected = FuncyList.Construct([|2; 4; 3; 5|])
        do! assertEquals expected sut
    }

    let ``Cons + Nil should return Nil value`` = test {
        let cons = FuncyList.Construct([|1|])
        let nil = FuncyList.Nil()
        let sut = cons.ComputeWith(fun x -> nil.FMap(fun y -> x + y))
        do! assertPred sut.IsNil
        let expected = FuncyList.Nil() :> FuncyList<int>
        do! assertEquals expected sut
    }

    let ``Nil + Cons should return Nil value`` = test {
        let nil = FuncyList.Nil()
        let cons = FuncyList.Construct([|2|])
        let sut = nil.ComputeWith(fun x -> cons.FMap(fun y -> x + y))
        do! assertPred sut.IsNil
        let expected = FuncyList.Nil() :> FuncyList<int>
        do! assertEquals expected sut
    }

    let ``Nil + Nil should return Nil value`` = test {
        let nil1 = FuncyList.Nil()
        let nil2 = FuncyList.Nil()
        let sut = nil1.ComputeWith(fun x -> nil2.FMap(fun y -> x + y))
        do! assertPred sut.IsNil
        let expected = FuncyList.Nil() :> FuncyList<int>
        do! assertEquals expected sut
    }


module FuncyListNTTest =
    open System

    // Take
    let ``FuncyListNT.Take(cons, 3) returns a value of type Cons`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.Take(3)
        do! assertEquals typeof<Cons<string>> <| sut.GetType()
    }
    let ``FuncyListNT.Take(nil, 3) returns a value of type Nil`` = test {
        let flist = FuncyList<string>.Construct([||])
        let sut = flist.Take(3)
        do! assertEquals typeof<Nil<string>> <| sut.GetType()
    }

    let ``FuncyListNT.Take(list, 3) takes first 3 elements`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.Take(3)
        do! assertEquals sut <| FuncyList.Construct([| "my"; "name"; "is" |])
    }
    let ``FuncyListNT.Take(list, 3) returns list when the length of list is less than or equal to 3`` = test {
        let flist = FuncyList.Construct([| "FuncyList" |])
        let sut = flist.Take(3)
        do! assertEquals sut <| flist
    }
    let ``FuncyListNT.Take(list, 0) returns Nil`` = test {
        let flist = FuncyList.Construct([| "FuncyList" |])
        let sut = flist.Take(0)
        do! assertEquals sut <| (FuncyList<string>.Nil() :> FuncyList<string>)
    }

    let ``FuncyListNT.Take commutes with Length function`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let func = Func<string, int>(fun s -> s.Length)
        do! assertEquals <|| (flist.Take(3).FMap(func), flist.FMap(func).Take(3))
    }

    // TakeFirst
    let ``FuncyListNT.TakeFirst(cons) returns a value of type Cons`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.TakeFirst()
        do! assertEquals typeof<Some<string>> <| sut.GetType()
    }
    let ``FuncyListNT.TakeFirst(nil) returns a value of type Nil`` = test {
        let flist = FuncyList<string>.Construct([||])
        let sut = flist.TakeFirst()
        do! assertEquals typeof<None<string>> <| sut.GetType()
    }

    let ``FuncyListNT.TakeFirst(list) takes a first element`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.TakeFirst()
        do! assertEquals sut <| (Maybe.Some("my") :> Maybe<string>)
    }

    let ``FuncyListNT.TakeFirst commutes with Length function`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let func = Func<string, int>(fun s -> s.Length)
        do! assertEquals <|| (flist.TakeFirst().FMap(func), flist.FMap(func).TakeFirst())
    }
