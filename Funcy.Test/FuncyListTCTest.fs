namespace Funcy.Test

open Funcy.Future
open Persimmon
open UseTestNameByReflection

module FuncyListTCTest =
    module FuncyListTCAsPointedTest =
        // constructor function
        let ``FuncyListTC.Cons(1, FuncyListTC.Nil<int>()) is type of ConsTC<int>`` = test {
            let cons = FuncyListTC.Cons(1, FuncyListTC.Nil())
            do! assertEquals typeof<ConsTC<int>> <| cons.GetType()
        }
        let ``FuncyListTC.Nil<int>() is type of NilTC<int>`` = test {
            let nil = FuncyListTC.Nil<int>()
            do! assertEquals typeof<NilTC<int>> <| nil.GetType()
        }
        let ``FuncyListTC.Construct<string>() is type of NilTC<string>`` = test {
            let empty = FuncyListTC.Construct<string>()
            do! assertEquals typeof<NilTC<string>> <| empty.GetType()
        }
        let ``FuncyListTC.Construct(1, 2, 3) = ConsTC<int>(1, ConsTC<int>(2, ConsTC<int>(3, NilTC<int>())))`` = test {
            let list = FuncyListTC.Construct(1, 2, 3)
            do! assertEquals typeof<ConsTC<int>> <| list.GetType()
            do! assertEquals list <| FuncyListTC.Cons(1, FuncyListTC.Cons(2, FuncyListTC.Cons(3, FuncyListTC.Nil())))
        }

    module GeneralTest =
        // cons
        let ``ConsTC<T>.IsCons should be true`` = test {
            let cons = FuncyListTC.Cons(1, FuncyListTC.Nil())
            do! assertPred <| cons.IsCons
        }
        let ``ConsTC<T>.IsNil should be false`` = test {
            let cons = FuncyListTC.Cons(1, FuncyListTC.Nil())
            do! assertPred <| not cons.IsNil
        }
        // nil
        let ``NilTC<T>.IsCons should be false`` = test {
            let nil = FuncyListTC.Nil<int>()
            do! assertPred <| not nil.IsCons
        }
        let ``NilTC<T>.IsNil should be true`` = test {
            let nil = FuncyListTC.Nil<int>()
            do! assertPred <| nil.IsNil
        }
        // head(cons(h, t)) = h
        let ``FuncyListTC.Cons(h, _).Head = h`` = test {
            let list = FuncyListTC.Cons(1, FuncyListTC.Nil())
            let head = list.ToCons().Head;
            do! assertEquals head <| 1
        }
        // tail(cons(h, t)) = t
        let ``FuncyListTC.Cons(_, t).Tail = t`` = test {
            let list = FuncyListTC.Cons(1, FuncyListTC.Cons(2, FuncyListTC.Nil()))
            let tail = list.ToCons().Tail;
            do! assertEquals tail <| FuncyListTC.Cons(2, FuncyListTC.Nil())
        }
        // equality on Cons
        let ``equality on ConsTC<T> depends on its contents`` = test {
            let lhs = FuncyListTC.Cons(1, FuncyListTC.Nil())
            let rhs = FuncyListTC.Cons(1, FuncyListTC.Nil())
            do! assertEquals lhs rhs
            do! assertPred (lhs = rhs)
            do! assertEquals <|| (lhs.GetHashCode(), rhs.GetHashCode())
        }
        let ``when headers differ, ConsTC<T> should differ`` = test {
            let lhs = FuncyListTC.Cons("hoge", FuncyListTC.Nil())
            let rhs = FuncyListTC.Cons("fuga", FuncyListTC.Nil())
            do! assertNotEquals lhs rhs
        }
        let ``content types matter on equality of ConsTC<T>`` = test {
            let lhs = FuncyListTC.Cons(3.14, FuncyListTC.Nil())
            let rhs = FuncyListTC.Cons(3.14f, FuncyListTC.Nil())
            do! (not >> assertPred) <| lhs.Equals(rhs)
        }
        let ``when tails differ, ConsTC<T> should differ`` = test {
            let lhs = FuncyListTC.Cons(1, FuncyListTC.Cons(2, FuncyListTC.Nil()))
            let rhs = FuncyListTC.Cons(1, FuncyListTC.Nil())
            do! assertNotEquals lhs rhs
        }
        // equality on Nil
        let ``equality on NilTC<T> depends on its type argument`` = test {
            let lhs = FuncyListTC.Nil<int>()
            let rhs = FuncyListTC.Nil<int>()
            do! assertEquals lhs rhs
            do! assertPred (lhs = rhs)
            do! assertEquals <|| (lhs.GetHashCode(), rhs.GetHashCode())
        }
        let ``when type arguments differ, NilTC<T> should differ`` = test {
            let lhs = FuncyListTC.Nil<int>()
            let rhs = FuncyListTC.Nil<string>()
            do! (not >> assertPred) <| lhs.Equals(rhs)
        }
        // equality between Cons and Nil
        let ``ConsTC<T> never equals to Nil<T>`` = test {
            let cons = FuncyListTC.Cons(1, FuncyListTC.Nil())
            let nil = FuncyListTC.Nil<int>()
            do! assertNotEquals cons nil
        }
        // Cons<T> constructor is monotone
        let ``ConsTC<T> reserve ordering`` = test {
            let cons1 = FuncyListTC.Cons(1, FuncyListTC.Nil())
            let cons2 = FuncyListTC.Cons(2, FuncyListTC.Nil())
            do! assertPred (cons1 < cons2)
        }
        // compareto, dictionary order
        let ``order on FuncyListTC<T> is dictionary order`` = test {
            let cons124 = FuncyListTC.Cons(1, FuncyListTC.Cons(2, FuncyListTC.Cons(4, FuncyListTC.Nil())))
            let cons13 = FuncyListTC.Cons(1, FuncyListTC.Cons(3, FuncyListTC.Nil()))
            do! assertPred (cons124 < cons13)
        }
        // comparison between Cons and Nil
        let ``ConsTC<T> is larger than NilTC<T>`` = test {
            let cons = FuncyListTC.Cons(1, FuncyListTC.Nil())
            let nil = FuncyListTC.Nil<int>()
            do! assertPred (cons > nil)
        }

    module FuncyListTCIterableTest =
        open System.Collections.Generic
        open System.Linq

        let ``ConsTC<T>.GetEnumerator<T>() returns IEnumerator<T>`` = test {
            let sut = FuncyListTC.Construct([|"hoge"; "fuga"; "bar"|])
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
            let sut = FuncyListTC.Nil<string>()
            let enumerator = sut.GetEnumerator()
            do! assertPred <| (not <| enumerator.MoveNext())
        }

    module NaturalTransformationTest =
        open System
        open Funcy.Future.NaturalTransformations

        // ElementAt
        let ``FuncyListTCNT.ElementAt(cons, 0) returns a value of type SomeTC<int>`` = test {
            let cons = FuncyListTC.Construct([|1|])
            let sut = cons.ElementAt(0)
            do! assertEquals typeof<SomeTC<int>> <| sut.GetType()
        }
        let ``FuncyListTCNT.ElementAt(cons, 0) returns SomeTC(cons.head)`` = test {
            let cons = FuncyListTC.Construct([|1|])
            let sut = cons.ElementAt(0)
            do! assertEquals sut <| MaybeTC.Some(1)
        }
        let ``FuncyListTCNT.ElementAt([1, 2, 3], 1) returns a value of type SomeTC<int>`` = test {
            let cons = FuncyListTC.Construct([|1; 2; 3|])
            let sut = cons.ElementAt(1)
            do! assertEquals typeof<SomeTC<int>> <| sut.GetType()
        }
        let ``FuncyListTCNT.ElementAt([1, 2, 3], 1) returns SomeTC(2)`` = test {
            let cons = FuncyListTC.Construct([|1; 2; 3|])
            let sut = cons.ElementAt(1)
            do! assertEquals sut <| MaybeTC.Some(2)
        }
        let ``FuncyListTCNT.ElementAt(nil, 0) returns a value of a type NoneTC`` = test {
            let nil = FuncyListTC.Construct<int>([||])
            let sut = nil.ElementAt(0)
            do! assertEquals typeof<NoneTC<int>> <| sut.GetType()
        }
        let ``FuncyListTCNT.ElementAt(nil, 0) returns NoneTC`` = test {
            let nil = FuncyListTC.Construct([||])
            let sut = nil.ElementAt(0)
            do! assertEquals sut <| MaybeTC.None()
        }

        let ``FuncyListTCNT.ElementAt(cons, -1) returns a value of type NoneTC`` = test {
            let cons = FuncyListTC.Construct([|1|])
            let sut = cons.ElementAt(-1)
            do! assertEquals typeof<NoneTC<int>> <| sut.GetType()
        }
        let ``FuncyListTCNT.ElementAt(cons, -1) returns NoneTC`` = test {
            let cons = FuncyListTC.Construct([|1|])
            let sut = cons.ElementAt(-1)
            do! assertEquals sut <| MaybeTC.None()
        }
        let ``FuncyListTCNT.ElementAt(nil, -1) returns a value of type NoneTC`` = test {
            let nil = FuncyListTC.Construct<int>([||])
            let sut = nil.ElementAt(-1)
            do! assertEquals typeof<NoneTC<int>> <| sut.GetType()
        }
        let ``FuncyListTCNT.ElementAt(nil, -1) returns NoneTC`` = test {
            let nil = FuncyListTC.Construct<int>([||])
            let sut = nil.ElementAt(-1)
            do! assertEquals sut <| MaybeTC.None()
        }

        let ``FuncyListTCNT.ElementAt(cons, 3) returns a value of type NoneTC when the length of cons is less than 3`` = test {
            let cons = FuncyListTC.Construct([|1|])
            let sut = cons.ElementAt(3)
            do! assertEquals sut <| MaybeTC.None()
        }
        let ``FuncyListTCNT.ElementAt(cons, 3) returns NoneTC when the length of cons is less than 3`` = test {
            let cons = FuncyListTC.Construct([|1|])
            let sut = cons.ElementAt(3)
            do! assertEquals typeof<NoneTC<int>> <| sut.GetType()
        }
        let ``FuncyListTCNT.ElementAt(nil, 11) returns a value of type NoneTC`` = test {
            let nil = FuncyListTC.Construct<int>([||])
            let sut = nil.ElementAt(11)
            do! assertEquals typeof<NoneTC<int>> <| sut.GetType()
        }
        let ``FuncyListTCNT.ElementAt(nil, 11) returns NoneTC`` = test {
            let nil = FuncyListTC.Construct<int>([||])
            let sut = nil.ElementAt(11)
            do! assertEquals sut <| MaybeTC.None()
        }

        // Last
        let ``FuncyListTCNT.Last(cons) returns a value of type ConsTC`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let sut = flist.Last()
            do! assertEquals typeof<SomeTC<string>> <| sut.GetType()
        }
        let ``FuncyListTCNT.Last(nil) returns a value of type Nil`` = test {
            let flist = FuncyListTC.Construct<string>([||])
            let sut = flist.Last()
            do! assertEquals typeof<NoneTC<string>> <| sut.GetType()
        }

        let ``FuncyListTCNT.Last(list) takes a first element`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let sut = flist.Last()
            do! assertEquals sut <| MaybeTC.Some("FuncyList")
        }

        let ``FuncyListTCNT.Last commutes with Length function`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let func = Func<string, int>(fun s -> s.Length)
            do! assertEquals <|| (flist.Last().FMap(func), flist.FMap(func).Last())
        }

        // Take
        let ``FuncyListTCNT.Take(cons, 3) returns a value of type ConsTC`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let sut = flist.Take(3)
            do! assertEquals typeof<ConsTC<string>> <| sut.GetType()
        }
        let ``FuncyListTCNT.Take(nil, 3) returns a value of type NilTC`` = test {
            let flist = FuncyListTC.Construct<string>([||])
            let sut = flist.Take(3)
            do! assertEquals typeof<NilTC<string>> <| sut.GetType()
        }

        let ``FuncyListTCNT.Take(list, 3) takes first 3 elements`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let sut = flist.Take(3)
            do! assertEquals sut <| FuncyListTC.Construct([| "my"; "name"; "is" |])
        }
        let ``FuncyListTCNT.Take(list, 3) returns `list` when the length of list is less than or equal to 3`` = test {
            let flist = FuncyListTC.Construct([| "FuncyList" |])
            let sut = flist.Take(3)
            do! assertEquals sut <| flist
        }
        let ``FuncyListNT.Take(list, 0) returns NilTC`` = test {
            let flist = FuncyListTC.Construct([| "FuncyList" |])
            let sut = flist.Take(0)
            do! assertEquals sut <| FuncyListTC.Nil()
        }
    
        let ``FuncyListTCNT.Take commutes with Length function`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let func = Func<string, int>(fun s -> s.Length)
            do! assertEquals <|| (flist.Take(3).FMap(func), flist.FMap(func).Take(3))
        }

        // First
        let ``FuncyListTCNT.First(cons) returns a value of type ConsTC`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let sut = flist.First()
            do! assertEquals typeof<SomeTC<string>> <| sut.GetType()
        }
        let ``FuncyListTCNT.First(nil) returns a value of type NilTC`` = test {
            let flist = FuncyListTC.Construct<string>([||])
            let sut = flist.First()
            do! assertEquals typeof<NoneTC<string>> <| sut.GetType()
        }

        let ``FuncyListTCNT.First(list) takes a first element`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let sut = flist.First()
            do! assertEquals sut <| MaybeTC.Some("my")
        }

        let ``FuncyListTCNT.First commutes with Length function`` = test {
            let flist = FuncyListTC.Construct([| "my"; "name"; "is"; "FuncyList" |])
            let func = Func<string, int>(fun s -> s.Length)
            do! assertEquals <|| (flist.First().FMap(func), flist.FMap(func).First())
        }