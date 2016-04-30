namespace Funcy.Test

open Funcy.Future
open Persimmon
open UseTestNameByReflection

module NonEmptyListTCTest =
    module NonEmptyListTCAsPointedTest =
        // constructor function
        let ``NonEmptyListTC.Singleton(1) is type of SingletonTC<int>`` = test {
            let singleton = NonEmptyListTC.Singleton(1)
            do! assertEquals typeof<SingletonTC<int>> <| singleton.GetType()
            do! assertEquals 1 <| singleton.ToSingleton().Value 
        }
        let ``NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(2)) is type of ConsNELTC<int>`` = test {
            let nel = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(2))
            do! assertEquals typeof<ConsNELTC<int>> <| nel.GetType()
            let consNEL = nel.ToConsNEL()
            do! assertEquals 1 <| consNEL.Head
            let tail = consNEL.Tail;
            do! assertEquals typeof<SingletonTC<int>> <| tail.GetType()
            do! assertEquals <|| (tail.ToSingleton().Value, 2)
        }
        let ``NonEmptyListTC.Construct<string>(null) throws ArgumentException`` = test {
            let! e = trap { NonEmptyListTC.Construct<string>(null) |> ignore }
            do! assertEquals typeof<System.ArgumentException> <| e.GetType()
        }
        let ``NonEmptyListTC.Construct<float>({}) throws ArgumentException`` = test {
            let! e = trap { NonEmptyListTC.Construct<float>([||]) |> ignore }
            do! assertEquals typeof<System.ArgumentException> <| e.GetType()
        }
        let ``NonEmptyListTC.Construct({1, 2, 3}) = ConsNELTC<int>(1, ConsNELTC<int>(2, SingletonTC<int>(3)))`` = test {
            let nel = NonEmptyListTC.Construct([1; 2; 3])
            do! assertEquals typeof<ConsNELTC<int>> <| nel.GetType()
            do! assertEquals nel <| (NonEmptyListTC.ConsNEL(1, NonEmptyListTC.ConsNEL(2, NonEmptyListTC.Singleton(3))))
            do! assertNotEquals nel <| (NonEmptyListTC.ConsNEL(1, NonEmptyListTC.ConsNEL(2, NonEmptyListTC.Singleton(4))))
            do! assertNotEquals nel <| (NonEmptyListTC.ConsNEL(1, NonEmptyListTC.ConsNEL(4, NonEmptyListTC.Singleton(3))))
        }

    module GeneralTest =
        // ConsNEL
        let ``ConsNELTC<T>.IsConsNEL should be true`` = test {
            let consNEL = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(0))
            do! assertPred <| consNEL.IsConsNEL
        }
        let ``ConsNELTC<T>.IsSingleton should be false`` = test {
            let consNEL = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(2))
            do! assertPred <| not consNEL.IsSingleton
        }
        // Singleton
        let ``SingletonTC<T>.IsConsNEL should be false`` = test {
            let singleton = NonEmptyListTC.Singleton(3)
            do! assertPred <| not singleton.IsConsNEL
        }
        let ``SingletonTC<T>.IsSingleton should be true`` = test {
            let singleton = NonEmptyListTC.Singleton(5)
            do! assertPred <| singleton.IsSingleton
        }
        // head(ConsNEL(h, t)) = h
        let ``NonEmptyListTC.ConsNEL(h, _).Head = h`` = test {
            let nel = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(2))
            let head = nel.ToConsNEL().Head;
            do! assertEquals head 1
        }
        // tail(ConsNEL(h, t)) = t
        let ``NonEmptyListTC.Cons(_, t).Tail = t`` = test {
            let nel = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.ConsNEL(2, NonEmptyListTC.Singleton(3)))
            let tail = nel.ToConsNEL().Tail;
            do! assertEquals tail <| NonEmptyListTC.ConsNEL(2, NonEmptyListTC.Singleton(3))
        }
        // equality on Singleton
        let ``equality on SingletonTC<T> depends on its contents`` = test {
            let lhs = NonEmptyListTC.Singleton("hoge")
            let rhs = NonEmptyListTC.Singleton("hoge")
            do! assertEquals lhs rhs
            do! assertPred (lhs = rhs)
            do! assertEquals <|| (lhs.ToSingleton().Value, rhs.ToSingleton().Value)
            do! assertEquals <|| (lhs.GetHashCode(), rhs.GetHashCode())
        }
        let ``when values differ, SingletonTC<T> should differ`` = test {
            let lhs = NonEmptyListTC.Singleton("hoge")
            let rhs = NonEmptyListTC.Singleton("fuga")
            do! assertNotEquals lhs rhs
            do! assertPred (lhs <> rhs)
        }
        let ``content types matter on equality of SingletonTC<T>`` = test {
            let lhs = NonEmptyListTC.Singleton(3.14)
            let rhs = NonEmptyListTC.Singleton(3.14f)
            do! (not >> assertPred) <| lhs.Equals(rhs)
        }
        // equality on ConsNEL
        let ``equality on ConsNELTC<T> depends on its contents`` = test {
            let lhs = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(2))
            let rhs = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(2))
            do! assertEquals lhs rhs
            do! assertPred (lhs = rhs)
            do! assertEquals <|| (lhs.GetHashCode(), rhs.GetHashCode())
        }
        let ``when headers differ, ConsNELTC<T> should differ`` = test {
            let lhs = NonEmptyListTC.ConsNEL("hoge", NonEmptyListTC.Singleton("bar"))
            let rhs = NonEmptyListTC.ConsNEL("fuga", NonEmptyListTC.Singleton("bar"))
            do! assertNotEquals lhs rhs
            do! assertPred (lhs <> rhs)
        }
        let ``when tails differ, ConsNELTC<T> should differ`` = test {
            let lhs = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.ConsNEL(2, NonEmptyListTC.Singleton(3)))
            let rhs = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(2))
            do! assertNotEquals lhs rhs
            do! assertPred (lhs <> rhs)
        }
        let ``content types matter on equality of ConsNELTC<T>`` = test {
            let lhs = NonEmptyListTC.ConsNEL(3.14, NonEmptyListTC.Singleton(2.718))
            let rhs = NonEmptyListTC.ConsNEL(3.14m, NonEmptyListTC.Singleton(2.718m))
            do! (not >> assertPred) <| lhs.Equals(rhs)
        }
        // equality between ConsNEL and Singleton
        let ``ConsNELTC<T> never equals to SingletonTC<T>`` = test {
            let consNEL = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(2))
            let singleton = NonEmptyListTC.Singleton(2)
            do! assertNotEquals consNEL singleton
        }
        // SingletonTC<T> constructor is monotone
        let ``SingletonTC<T> reserve ordering`` = test {
            let singleton1 = NonEmptyListTC.Singleton(1)
            let singleton2 = NonEmptyListTC.Singleton(2)
            do! assertPred (singleton1 < singleton2)
        }
        // ConsNELTC<T> constructor is monotone
        let ``ConsNELTC<T> reserve ordering`` = test {
            let consNEL1 = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(3))
            let consNEL2 = NonEmptyListTC.ConsNEL(2, NonEmptyListTC.Singleton(3))
            do! assertPred (consNEL1 < consNEL2)
        }
        // compareto, dictionary order
        let ``order on NonEmptyListTC<T> is dictionary order`` = test {
            let consNEL124 = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.ConsNEL(2, NonEmptyListTC.Singleton(4)))
            let consNEL13 = NonEmptyListTC.ConsNEL(1, NonEmptyListTC.Singleton(3))
            do! assertPred (consNEL124 < consNEL13)
        }

    module NonEmptyListTCNTTest =
        open System
        open Funcy.Future.NaturalTransformations

        // ToFuncyList
        let ``Original NonEmptyListTC and resulted FuncyListTC have a same type of elements`` = test {
            let nel = NonEmptyListTC.Construct([| "my"; "elements"; "are"; "type"; "of"; "string"|])
            let sut = nel.ToFuncyList()
            do! assertEquals typeof<ConsTC<string>> <| sut.GetType()
        }
        let ``Original NonEmptyListTC and resulted FuncyListTC have a equivalent elements`` = test {
            let elements = [| "my"; "elements"; "are"; "type"; "of"; "string"|]
            let nel = NonEmptyListTC.Construct(elements)
            let fl = FuncyListTC.Construct(elements)
            let sut = nel.ToFuncyList()
            do! assertEquals sut fl
        }

        // Take
        let ``NonEmptyListTCNT.Take(cons, 3) returns a value of type Cons`` = test {
            let nel = NonEmptyListTC.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
            let sut = nel.Take(3)
            do! assertEquals typeof<ConsTC<string>> <| sut.GetType()
        }

        let ``NonEmptyListTCNT.Take(list, 3) takes first 3 elements`` = test {
            let nel = NonEmptyListTC.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
            let sut = nel.Take(3)
            do! assertEquals sut <| FuncyListTC.Construct([| "my"; "name"; "is" |])
        }
        let ``NonEmptyListTCNT.Take(list, 3) returns list.ToFuncyList() when the length of list is less than or equal to 3`` = test {
            let nel = NonEmptyListTC.Construct([| "NonEmptyList" |])
            let sut = nel.Take(3)
            do! assertEquals sut <| nel.ToFuncyList()
        }
        let ``NonEmptyListTCNT.Take(list, 0) returns Nil`` = test {
            let nel = NonEmptyListTC.Construct([| "NonEmptyList" |])
            let sut = nel.Take(0)
            do! assertEquals sut <| FuncyListTC.Nil<string>()
        }
    
        let ``NonEmptyListTCNT.Take commutes with Length function`` = test {
            let nel = NonEmptyListTC.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
            let func = Func<string, int>(fun s -> s.Length)
            do! assertEquals <|| (nel.Take(3).FMap(func), nel.FMap(func).Take(3))
        }

        // TakeFirst
        let ``NonEmptyListTCNT.TakeFirst<T>(cons) returns a value of type T`` = test {
            let nel = NonEmptyListTC.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
            let sut = nel.TakeFirst()
            do! assertEquals typeof<string> <| sut.GetType()
        }

        let ``NonEmptyListTCNT.TakeFirst(list) takes a first element`` = test {
            let nel = NonEmptyListTC.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
            let sut = nel.TakeFirst()
            do! assertEquals sut <| "my"
        }

        let ``NonEmptyListTCNT.TakeFirst commutes with Length function`` = test {
            let nel = NonEmptyListTC.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
            let func = Func<string, int>(fun s -> s.Length)
            do! assertEquals <|| (func.Invoke(nel.TakeFirst()), nel.FMap(func).TakeFirst())
        }
