namespace Funcy.Test

open Funcy
open Persimmon
open UseTestNameByReflection

module NonEmptyListTest =
    // constructor function
    let ``NonEmptyList.Singleton(1) is type of Singleton<int>`` = test {
        let singleton = NonEmptyList.Singleton(1)
        do! assertEquals typeof<Singleton<int>> <| singleton.GetType()
        do! assertEquals singleton.Value 1
    }
    let ``NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(2)) is type of ConsNEL<int>`` = test {
        let consNEL = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(2))
        do! assertEquals typeof<ConsNEL<int>> <| consNEL.GetType()
        do! assertEquals consNEL.Head 1
        let tail = consNEL.Tail;
        do! assertEquals typeof<Singleton<int>> <| tail.GetType()
        do! assertEquals <|| (tail.ToSingleton().Value, 2)
    }
    let ``NonEmptyList<string>.Construct(null) throws ArgumentException`` = test {
        let! e = trap { NonEmptyList<string>.Construct(null) |> ignore }
        do! assertEquals typeof<System.ArgumentException> <| e.GetType()
    }
    let ``NonEmptyList<float>.Construct({}) throws ArgumentException`` = test {
        let! e = trap { NonEmptyList<float>.Construct([||]) |> ignore }
        do! assertEquals typeof<System.ArgumentException> <| e.GetType()
    }
    let ``NonEmptyList<int>.Construct({1, 2, 3}) = ConsNEL<int>(1, ConsNEL<int>(2, Singleton<int>(3)))`` = test {
        let nel = NonEmptyList.Construct([1; 2; 3])
        do! assertEquals typeof<ConsNEL<int>> <| nel.GetType()
        do! assertEquals nel <| (NonEmptyList.ConsNEL(1, NonEmptyList.ConsNEL(2, NonEmptyList.Singleton(3))) :> NonEmptyList<int>)
        do! assertNotEquals nel <| (NonEmptyList.ConsNEL(1, NonEmptyList.ConsNEL(2, NonEmptyList.Singleton(4))) :> NonEmptyList<int>)
        do! assertNotEquals nel <| (NonEmptyList.ConsNEL(1, NonEmptyList.ConsNEL(4, NonEmptyList.Singleton(3))) :> NonEmptyList<int>)
    }
    // ConsNEL
    let ``ConsNEL<T>.IsConsNEL should be true`` = test {
        let consNEL = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(0))
        do! assertPred <| consNEL.IsConsNEL
    }
    let ``ConsNEL<T>.IsSingleton should be false`` = test {
        let consNEL = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(2))
        do! assertPred <| not consNEL.IsSingleton
    }
    // Singleton
    let ``Singleton<T>.IsConsNEL should be false`` = test {
        let singleton = NonEmptyList.Singleton(3)
        do! assertPred <| not singleton.IsConsNEL
    }
    let ``Nil<T>.IsNil should be true`` = test {
        let singleton = NonEmptyList.Singleton(5)
        do! assertPred <| singleton.IsSingleton
    }
    // head(ConsNEL(h, t)) = h
    let ``NonEmptyList.ConsNEL(h, _).Head = h`` = test {
        let nel = NonEmptyList<int>.ConsNEL(1, NonEmptyList.Singleton(2))
        let head = nel.Head;
        do! assertEquals head 1
    }
    // tail(ConsNEL(h, t)) = t
    let ``NonEmptyList<T>.Cons(_, t).Tail = t`` = test {
        let nel = NonEmptyList.ConsNEL(1, NonEmptyList.ConsNEL(2, NonEmptyList.Singleton(3)))
        let tail = nel.Tail;
        do! assertEquals tail (NonEmptyList.ConsNEL(2, NonEmptyList.Singleton(3)) :> NonEmptyList<int>)
    }
    // equality on Singleton
    let ``equality on Singleton<T> depends on its contents`` = test {
        let lhs = NonEmptyList.Singleton("hoge")
        let rhs = NonEmptyList.Singleton("hoge")
        do! assertEquals lhs rhs
        do! assertPred (lhs = rhs)
        do! assertEquals lhs.Value rhs.Value
        do! assertEquals <|| (lhs.GetHashCode(), rhs.GetHashCode())
    }
    let ``when values differ, Singleton<T> should differ`` = test {
        let lhs = NonEmptyList.Singleton("hoge")
        let rhs = NonEmptyList.Singleton("fuga")
        do! assertNotEquals lhs rhs
        do! assertPred (lhs <> rhs)
    }
    let ``content types matter on equality of Singleton<T>`` = test {
        let lhs = NonEmptyList.Singleton(3.14)
        let rhs = NonEmptyList.Singleton(3.14f)
        do! (not >> assertPred) <| lhs.Equals(rhs)
    }
    // equality on ConsNEL
    let ``equality on ConsNEL<T> depends on its contents`` = test {
        let lhs = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(2))
        let rhs = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(2))
        do! assertEquals lhs rhs
        do! assertPred (lhs = rhs)
        do! assertEquals <|| (lhs.GetHashCode(), rhs.GetHashCode())
    }
    let ``when headers differ, ConsNEL<T> should differ`` = test {
        let lhs = NonEmptyList.ConsNEL("hoge", NonEmptyList.Singleton("bar"))
        let rhs = NonEmptyList.ConsNEL("fuga", NonEmptyList.Singleton("bar"))
        do! assertNotEquals lhs rhs
        do! assertPred (lhs <> rhs)
    }
    let ``when tails differ, ConsNEL<T> should differ`` = test {
        let lhs = NonEmptyList.ConsNEL(1, NonEmptyList.ConsNEL(2, NonEmptyList.Singleton(3)))
        let rhs = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(2))
        do! assertNotEquals lhs rhs
        do! assertPred (lhs <> rhs)
    }
    let ``content types matter on equality of ConsNEL<T>`` = test {
        let lhs = NonEmptyList.ConsNEL(3.14, NonEmptyList.Singleton(2.718))
        let rhs = NonEmptyList.ConsNEL(3.14m, NonEmptyList.Singleton(2.718m))
        do! (not >> assertPred) <| lhs.Equals(rhs)
    }
    // equality between ConsNEL and Singleton
    let ``ConsNEL<T> never equals to Singleton<T>`` = test {
        let consNEL = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(2))
        let singleton = NonEmptyList.Singleton(2)
        do! assertNotEquals (consNEL :> NonEmptyList<int>) (singleton :> NonEmptyList<int>)
    }
    // Singleton<T> constructor is monotone
    let ``Singleton<T> reserve ordering`` = test {
        let singleton1 = NonEmptyList.Singleton(1)
        let singleton2 = NonEmptyList.Singleton(2)
        do! assertPred (singleton1 < singleton2)
    }
    // ConsNEL<T> constructor is monotone
    let ``ConsNEL<T> reserve ordering`` = test {
        let consNEL1 = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(3))
        let consNEL2 = NonEmptyList.ConsNEL(2, NonEmptyList.Singleton(3))
        do! assertPred (consNEL1 < consNEL2)
    }
    // compareto, dictionary order
    let ``order on NonEmptyList<T> is dictionary order`` = test {
        let consNEL124 = NonEmptyList.ConsNEL(1, NonEmptyList.ConsNEL(2, NonEmptyList.Singleton(4)))
        let consNEL13 = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(3))
        do! assertPred (consNEL124 < consNEL13)
    }

module NonEmptyListIterableTest =
    open System.Collections.Generic
    open System.Linq

    let ``ConsNEL<T>.GetEnumerator() returns IEnumerator<T>`` = test {
        let sut = NonEmptyList.Construct(["hoge"; "fuga"; "bar"])
        let enumerator = sut.GetEnumerator()
        do! assertPred <| enumerator.MoveNext()
        do! assertEquals "hoge" enumerator.Current
        do! assertPred <| enumerator.MoveNext()
        do! assertEquals "fuga" enumerator.Current
        do! assertPred <| enumerator.MoveNext()
        do! assertEquals "bar" enumerator.Current
        do! assertPred <| (not <| enumerator.MoveNext())
    }

    let ``Singleton<T>.GetEnumerator() returns IEnumerator<T>`` = test {
        let sut = NonEmptyList.Singleton(42)
        let enumerator = sut.GetEnumerator()
        do! assertPred <| enumerator.MoveNext()
        do! assertEquals 42 enumerator.Current
        do! assertPred <| (not <| enumerator.MoveNext())
    }
    
module NonEmptyListFunctorTest =
    open System

    let ``fmap NonEmptyList<int> (int -> int) = NonEmptyList<int>`` = test {
        let nel = NonEmptyList.Construct([1; 2; 3])
        let sut = nel.FMap(Func<int, int>(fun x -> x * 4))
        do! assertEquals typeof<ConsNEL<int>> <| sut.GetType()
        do! assertEquals sut <| NonEmptyList.Construct([4; 8; 12])
    }
    
    let ``fmap NonEmptyList<decimal> (decimal -> string) = NonEmptyList<string>`` = test {
        let sut = NonEmptyList.Construct([2.718m; 3.14m]).FMap(Func<decimal, string>(fun d -> d.ToString()))
        do! assertEquals typeof<ConsNEL<string>> <| sut.GetType()
        do! assertEquals sut <| NonEmptyList.Construct(["2.718"; "3.14"])
    }

    let ``fmap Singleton<byte []> (byte [] -> byte) = byte) = Singleton<byte>`` = test {
        let singleton = NonEmptyList.Singleton([|0xCAuy; 0xFEuy; 0xBAuy;  0xBEuy|])
        let sut = singleton.FMap(Func<byte [], byte>(fun arr -> Array.max arr))
        do! assertEquals typeof<Singleton<byte>> <| sut.GetType()
        do! assertEquals sut <| (NonEmptyList.Singleton(0xFEuy) :> NonEmptyList<byte>)
    }
    
module NonEmptyListApplicativeTest =
    open System
    open Funcy.Extensions

    let ``Apply: NonEmptyList<int> -> NonEmptyList<int -> int> -> NonEmptyList<int>`` = test {
        let flistX = NonEmptyList.Construct([1; 2; 3])
        let flistF = NonEmptyList.Construct
                        ([
                            Func<int, int>(fun x -> x + 3)
                            Func<int, int>(fun x -> x * 3)
                            Func<int, int>(fun x -> x / 3)
                        ])
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<ConsNEL<int>> <| sut.GetType()
        do! assertPred sut.IsConsNEL
        let expected = NonEmptyList.Construct([4; 5; 6; 3; 6; 9; 0; 0; 1])
        do! assertEquals expected sut
    }

    let ``Apply: NonEmptyList<DateTime> -> NonEmptyList<DateTime -> string> -> NonEmptyList<string>`` = test {
        let flistX = NonEmptyList.Construct([DateTime(2015, 11, 17); DateTime(2015, 11, 18)])
        let flistF = NonEmptyList.Construct
                        ([
                            Func<DateTime, string>(fun d1 -> d1.ToString("yyyy/MM/dd"))
                            Func<DateTime, string>(fun d2 -> d2.ToString("yyyy-MM-dd"))
                            Func<DateTime, string>(fun d3 -> d3.ToString("yyyyMMdd"))
                        ])
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<ConsNEL<string>> <| sut.GetType()
        do! assertPred sut.IsConsNEL
        let expected = NonEmptyList.Construct
                        ([
                            "2015/11/17"; "2015/11/18"
                            "2015-11-17"; "2015-11-18"
                            "20151117"; "20151118"
                        ])
        do! assertEquals expected sut
    }
    
    let ``ConsNEL<float -> bool> <*> Singleton<float> = ConsNEL<bool>`` = test {
        let flistX = NonEmptyList.Singleton(2.718)
        let flistF = NonEmptyList.ConsNEL(Func<float, bool>(fun f -> f > 3.14), NonEmptyList.Singleton(Func<float, bool>(fun f -> f < 3.14)))
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<ConsNEL<bool>> <| sut.GetType()
        do! assertPred sut.IsConsNEL
        let expected = NonEmptyList.Construct([false; true])
        do! assertEquals expected sut
    }
    
    let ``Singleton<string -> int> <*> ConsNEL<string> = ConsNEL<decimal>`` = test {
        let flistX = NonEmptyList.ConsNEL("hoge", NonEmptyList.Singleton("bar"))
        let flistF = NonEmptyList.Singleton(Func<string, int>(fun s -> s.Length))
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<ConsNEL<int>> <| sut.GetType()
        do! assertPred sut.IsConsNEL
        let expected = NonEmptyList.Construct([4; 3])
        do! assertEquals expected sut
    }
    
    let ``Singleton<int -> float> <*> Singleton<int> = Singleton<float>`` = test {
        let flistX = NonEmptyList.Singleton(10)
        let flistF = NonEmptyList.Singleton(Func<int, float>(fun i -> float(i)))
        let sut = flistX.Apply(flistF)
        do! assertEquals typeof<Singleton<float>> <| sut.GetType()
        do! assertPred sut.IsSingleton
        let expected = NonEmptyList.Construct([10.0])
        do! assertEquals expected sut
    }
    
    let ``ConsNEL<int> <* ConsNEL<string> = ConsNEL<int>`` = test {
        let sut = NonEmptyList.Construct([1; 1; 2; 3]).ApplyLeft(NonEmptyList.Construct(["hoge"; "fuga"; "bar"]))
        do! assertEquals typeof<ConsNEL<int>> <| sut.GetType()
        do! assertEquals sut <| NonEmptyList.Construct([1; 1; 2; 3])
    }
    
    let ``ConsNEL<float> *> ConsNEL<decimal> = ConsNEL<decimal>`` = test {
        let sut = NonEmptyList.Construct([3.14; 2.718]).ApplyRight(NonEmptyList.Construct([2.718m; 3.14m]))
        do! assertEquals typeof<ConsNEL<decimal>> <| sut.GetType()
        do! assertEquals sut <| NonEmptyList.Construct([2.718m; 3.14m])
    }
    
    let ``Singleton<bool> <* ConsNEL<long> = Singleton<bool>`` = test {
        let sut = NonEmptyList.Singleton(true).ApplyLeft(NonEmptyList.ConsNEL(33L, NonEmptyList.Singleton(4L)))
        do! assertEquals typeof<Singleton<bool>> <| sut.GetType()
        do! assertEquals sut <| (NonEmptyList.Singleton(true) :> NonEmptyList<bool>)
    }
    
    let ``Singleton<string> *> ConsNEL<byte> = ConsNEL<byte>`` = test {
        let sut = NonEmptyList.Singleton("CAFE").ApplyRight(NonEmptyList.Construct([0xCAuy; 0xFEuy]))
        do! assertEquals typeof<ConsNEL<byte>> <| sut.GetType()
        do! assertEquals sut <| (NonEmptyList.ConsNEL(0xCAuy, NonEmptyList.Singleton(0xFEuy)) :> NonEmptyList<byte>)
    }

    let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)   // for implicit conversion in F#
    
    let ``Singleton(+) <$> Singleton(5) <*> ConsNEL(3, 4, 10) = ConsNEL(8, 9, 15)`` = test {
        let add = Func<int, int, int>(fun x y -> x + y)
        // (+) <$> NEL(5) <*> NEL(3, 4, 10) ... Funcy has the backward applicative style
        let sut = NonEmptyList.Construct([3; 4; 10]).Apply(NonEmptyList.Singleton(5).FMap(!> add.Curry()))
        do! assertPred sut.IsConsNEL
        let expected = NonEmptyList.Construct([8; 9; 15])
        do! assertEquals expected <| sut
    }    

    let ``ConsNEL(+, *, -) <*> ConsNEL(5, 2) = ConsNEL(+ 5, + 2, * 5, * 2, - 5, - 2)`` = test {
        let add = Func<int, int, int>(fun x y -> x + y)
        let mul = Func<int, int, int>(fun x y -> x * y)
        let sub = Func<int, int, int>(fun x y -> x - y)
        let flistOP = NonEmptyList.Construct([!> add.Curry(); !> mul.Curry(); !> sub.Curry()])
        let flistX = NonEmptyList.ConsNEL(5, NonEmptyList.Singleton(2))
        let sut = flistX.Apply(flistOP)
        let target = NonEmptyList.Construct([0; 1; 2])
        let expected = NonEmptyList.Construct([5; 6; 7; 2; 3; 4; 0; 5; 10; 0; 2; 4; 5; 4; 3; 2; 1; 0])
        do! assertEquals expected <| target.Apply(sut)
    }
    
module NonEmptyListComputationTest =
    let returnNEL x = NonEmptyList.Construct([x])

    let ``ConsNEL + ConsNEL should return ConsNEL value`` = test {
        let sut = NonEmptyList.Construct([0; 10]).ComputeWith (fun x ->
                    NonEmptyList.Construct([2; 3]).FMap (fun y ->
                        x + y))
        do! assertEquals typeof<ConsNEL<int>> <| sut.GetType()
        let expected = NonEmptyList.Construct([2; 3; 12; 13])
        do! assertEquals expected sut
    }
    
    let ``[0, 1] >>= (\x -> [2, 4] >>= (\y -> return $ x + y)) should return [2, 4, 3, 5]`` = test {
        let sut = NonEmptyList.Construct([0; 1]).ComputeWith (fun x ->
                    NonEmptyList.Construct([2; 4]).ComputeWith (fun y ->
                        returnNEL (x + y)))
        do! assertEquals typeof<ConsNEL<int>> <| sut.GetType()
        let expected = NonEmptyList.Construct([2; 4; 3; 5])
        do! assertEquals expected sut
    }
    
    let ``ConsNEL + Singleton should return Singleton value`` = test {
        let consNEL = NonEmptyList.ConsNEL(1, NonEmptyList.Singleton(2))
        let singleton = NonEmptyList.Singleton(3)
        let sut = consNEL.ComputeWith(fun x -> singleton.FMap(fun y -> x + y))
        do! assertEquals typeof<ConsNEL<int>> <| sut.GetType()
        let expected = NonEmptyList.Construct([4; 5])
        do! assertEquals expected sut
    }
    
    let ``Singleton + ConsNEL should return Singleton value`` = test {
        let singleton = NonEmptyList.Singleton(100)
        let consNEL = NonEmptyList.Construct([2; 3])
        let sut = singleton.ComputeWith(fun x -> consNEL.FMap(fun y -> x + y))
        do! assertEquals typeof<ConsNEL<int>> <| sut.GetType()
        let expected = NonEmptyList.Construct([102; 103])
        do! assertEquals expected sut
    }
    
    let ``Singleton + Singleton should return Singleton value`` = test {
        let singleton1 = NonEmptyList.Singleton(33)
        let singleton2 = NonEmptyList.Singleton(4)
        let sut = singleton1.ComputeWith(fun x -> singleton2.FMap(fun y -> x + y))
        do! assertEquals typeof<Singleton<int>> <| sut.GetType()
        let expected = NonEmptyList.Singleton(37) :> NonEmptyList<int>
        do! assertEquals expected sut
    }
