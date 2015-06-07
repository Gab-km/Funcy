#nowarn "67"
namespace Funcy.Test

open System
open Funcy
open Persimmon

module DisposableEither1Test =
    let t1 = test "DisposableEither1<TLeft, TRight>.Right creates DisposableRight1<TLeft, TRight> instance" {
        let right = DisposableEither1<MyDisposable<exn>, int>.Right(1)
        do! assertEquals typeof<DisposableRight1<MyDisposable<exn>, int>> <| right.GetType()
    }
    let t2 = test "DisposableEither1<TLeft, TRight>.Left creates DisposableLeft1<TLeft, TRight> instance" {
        let left = DisposableEither1<MyDisposable<string>, string>.Left(new MyDisposable<string>("hoge"))
        do! assertEquals typeof<DisposableLeft1<MyDisposable<string>, string>> <| left.GetType()
    }
    let t3 = test "DisposableRight1<TLeft, TRight> as IRight<TLeft, TRight> then Value should return its right value" {
        let right = DisposableEither1<MyDisposable<exn>, float>.Right(2.5) :> IRight<MyDisposable<exn>, float>
        do! assertEquals 2.5 right.Value
    }
    let t4 = test "DisposableRight1<TLeft, TRight>.IsRight should return true" {
        let right = DisposableEither1<MyDisposable<exn>, string>.Right("hoge")
        do! assertPred right.IsRight
    }
    let t5 = test "DisposableRight1<TLeft, TRight>.IsLeft should return false" {
        let right = DisposableEither1<MyDisposable<exn>, string>.Right("hoge")
        do! assertPred <| not right.IsLeft
    }
    let t6 = test "When DisposableRight1<TLeft, TRight> as IEither<TLeft, TRight> then IsRight should return true" {
        let right = DisposableEither1<MyDisposable<exn>, float32>.Right(-1.0f) :> IEither<MyDisposable<exn>, float32>
        do! assertPred right.IsRight
    }
    let t7 = test "When DisposableRight1<TLeft, TRight> as IEither<TLeft, TRight> then IsLeft should return false" {
        let right = DisposableEither1<MyDisposable<exn>, obj>.Right(obj()) :> IEither<MyDisposable<exn>, obj>
        do! assertPred <| not right.IsLeft
    }
    let t8 = test "When DisposableRight1<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should return IRight<TLeft, TRight> instance" {
        let right = DisposableEither1<MyDisposable<exn>, int>.Right(-1) :> IEither<MyDisposable<exn>, int>
        do! assertPred (right.ToRight() :? IRight<MyDisposable<exn>, int>)
    }
    let t9 = test "When DisposableRight1<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should raise InvalidCastException" {
        let right = DisposableEither1<MyDisposable<exn>, string>.Right("egg") :> IEither<MyDisposable<exn>, string>
        let! e = trap { right.ToLeft() |> ignore }
        do! assertEquals typeof<InvalidCastException> <| e.GetType()
    }
    let t10 = test "DisposableLeft1<TLeft, TRight> as ILeft<TLeft, TRight> then Value should return its left value" {
        let err = new MyDisposable<exn>(Exception("Left"))
        let left = DisposableEither1<MyDisposable<exn>, float>.Left(err) :> ILeft<MyDisposable<exn>, float>
        do! assertEquals err left.Value
    }
    let t11 = test "DisposableLeft1<TLeft, TRight>.IsRight should return false" {
        let left = DisposableEither1<MyDisposable<exn>, int>.Left(new MyDisposable<exn>(Exception("fuga")))
        do! assertPred <| not left.IsRight
    }
    let t12 = test "DisposableLeft1<TLeft, TRight>.IsLeft should return true" {
        let left = DisposableEither1<MyDisposable<exn>, int>.Left(new MyDisposable<exn>(Exception("fuga")))
        do! assertPred left.IsLeft
    }
    let t13 = test "When DisposableLeft1<TLeft, TRight> as IEither<TLeft, TRight> Then IsRight should return false" {
        let left = DisposableEither1<MyDisposable<exn>, int>.Left(new MyDisposable<exn>(Exception("fuga"))) :> IEither<MyDisposable<exn>, int>
        do! assertPred <| not left.IsRight
    }
    let t14 = test "When DisposableLeft1<TLeft, TRight> as IEither<TLeft, TRight> Then IsLeft should return true" {
        let left = DisposableEither1<MyDisposable<exn>, int>.Left(new MyDisposable<exn>(Exception("fuga"))) :> IEither<MyDisposable<exn>, int>
        do! assertPred left.IsLeft
    }
    let t15 = test "When DisposableLeft1<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should raise InvalidCastException" {
        let left = DisposableEither1<MyDisposable<exn>, int list>.Left(new MyDisposable<exn>(Exception("Not List"))) :> IEither<MyDisposable<exn>, int list>
        let! e = trap { left.ToRight() |> ignore }
        do! assertEquals typeof<InvalidCastException> <| e.GetType()
    }
    let t16 = test "When DisposableLeft1<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should return ILeft<TLeft, TRight> instance" {
        let left = DisposableEither1<MyDisposable<exn>, bool>.Left(new MyDisposable<exn>(Exception("ToLeft"))) :> IEither<MyDisposable<exn>, bool>
        do! assertPred (left.ToLeft() :? ILeft<MyDisposable<exn>, bool>)
    }
    let t17 = test "Given target as MyDisposable<TLeft> and sut as DisposableLeft1<MyDisposable<TLeft>, TRight> When sut.Dispose() Then sut.Disposed is true" {
        let target = new MyDisposable<string>("hoge")
        do! assertPred <| not target.Disposed
        let sut = DisposableEither1<MyDisposable<string>, int>.Left(target)
        do! assertPred sut.IsLeft
        sut.Dispose()
        do! assertPred <| target.Disposed
    }

module DisposableEither2Test =
    let t1 = test "DisposableEither2<TLeft, TRight>.Right creates DisposableRight2<TLeft, TRight> instance" {
        let right = DisposableEither2<exn, MyDisposable<int>>.Right(new MyDisposable<int>(1))
        do! assertEquals typeof<DisposableRight2<exn, MyDisposable<int>>> <| right.GetType()
    }
    let t2 = test "DisposableEither2<TLeft, TRight>.Left creates DisposableLeft2<TLeft, TRight> instance" {
        let left = DisposableEither2<exn, MyDisposable<string>>.Left(Exception("hoge"))
        do! assertEquals typeof<DisposableLeft2<exn, MyDisposable<string>>> <| left.GetType()
    }
    let t3 = test "DisposableRight2<TLeft, TRight> as IRight<TLeft, TRight> then Value should return its right value" {
        let value = new MyDisposable<float>(2.5)
        let right = DisposableEither2<exn, MyDisposable<float>>.Right(value) :> IRight<exn, MyDisposable<float>>
        do! assertEquals value right.Value
    }
    let t4 = test "DisposableRight2<TLeft, TRight>.IsRight should return true" {
        let right = DisposableEither2<exn, MyDisposable<string>>.Right(new MyDisposable<string>("hoge"))
        do! assertPred right.IsRight
    }
    let t5 = test "DisposableRight2<TLeft, TRight>.IsLeft should return false" {
        let right = DisposableEither2<exn, MyDisposable<string>>.Right(new MyDisposable<string>("hoge"))
        do! assertPred <| not right.IsLeft
    }
    let t6 = test "When DisposableRight2<TLeft, TRight> as IEither<TLeft, TRight> then IsRight should return true" {
        let right = DisposableEither2<exn, MyDisposable<float32>>.Right(new MyDisposable<float32>(-1.0f)) :> IEither<exn, MyDisposable<float32>>
        do! assertPred right.IsRight
    }
    let t7 = test "When DisposableRight2<TLeft, TRight> as IEither<TLeft, TRight> then IsLeft should return false" {
        let right = DisposableEither2<exn, MyDisposable<obj>>.Right(new MyDisposable<obj>(obj())) :> IEither<exn, MyDisposable<obj>>
        do! assertPred <| not right.IsLeft
    }
    let t8 = test "When DisposableRight2<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should return IRight<TLeft, TRight> instance" {
        let right = DisposableEither2<exn, MyDisposable<int>>.Right(new MyDisposable<int>(-1)) :> IEither<exn, MyDisposable<int>>
        do! assertPred (right.ToRight() :? IRight<exn, MyDisposable<int>>)
    }
    let t9 = test "When DisposableRight2<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should raise InvalidCastException" {
        let right = DisposableEither2<exn, MyDisposable<string>>.Right(new MyDisposable<string>("egg")) :> IEither<exn, MyDisposable<string>>
        let! e = trap { right.ToLeft() |> ignore }
        do! assertEquals typeof<InvalidCastException> <| e.GetType()
    }
    let t10 = test "DisposableLeft2<TLeft, TRight> as ILeft<TLeft, TRight> then Value should return its left value" {
        let err = Exception("Left")
        let left = DisposableEither2<exn, MyDisposable<float>>.Left(err) :> ILeft<exn, MyDisposable<float>>
        do! assertEquals err left.Value
    }
    let t11 = test "DisposableLeft2<TLeft, TRight>.IsRight should return false" {
        let left = DisposableEither2<exn, MyDisposable<int>>.Left(Exception("fuga"))
        do! assertPred <| not left.IsRight
    }
    let t12 = test "DisposableLeft2<TLeft, TRight>.IsLeft should return true" {
        let left = DisposableEither2<exn, MyDisposable<int>>.Left(Exception("fuga"))
        do! assertPred left.IsLeft
    }
    let t13 = test "When DisposableLeft2<TLeft, TRight> as IEither<TLeft, TRight> Then IsRight should return false" {
        let left = DisposableEither2<exn, MyDisposable<int>>.Left(Exception("fuga")) :> IEither<exn, MyDisposable<int>>
        do! assertPred <| not left.IsRight
    }
    let t14 = test "When DisposableLeft2<TLeft, TRight> as IEither<TLeft, TRight> Then IsLeft should return true" {
        let left = DisposableEither2<exn, MyDisposable<int>>.Left(Exception("fuga")) :> IEither<exn, MyDisposable<int>>
        do! assertPred left.IsLeft
    }
    let t15 = test "When DisposableLeft2<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should raise InvalidCastException" {
        let left = DisposableEither2<exn, MyDisposable<int list>>.Left(Exception("Not List")) :> IEither<exn, MyDisposable<int list>>
        let! e = trap { left.ToRight() |> ignore }
        do! assertEquals typeof<InvalidCastException> <| e.GetType()
    }
    let t16 = test "When DisposableLeft2<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should return ILeft<TLeft, TRight> instance" {
        let left = DisposableEither2<exn, MyDisposable<bool>>.Left(Exception("ToLeft")) :> IEither<exn, MyDisposable<bool>>
        do! assertPred (left.ToLeft() :? ILeft<exn, MyDisposable<bool>>)
    }
    let t17 = test "Given target as MyDisposable<TRight> and sut as DisposableRight2<TLeft, MyDisposable<TRight>> When sut.Dispose() Then sut.Disposed is true" {
        let target = new MyDisposable<int>(1)
        do! assertPred <| not target.Disposed
        let sut = DisposableEither2<exn, MyDisposable<int>>.Right(target)
        do! assertPred sut.IsRight
        sut.Dispose()
        do! assertPred <| target.Disposed
    }

module DisposableEither3Test =
    let t1 = test "DisposableEither3<TLeft, TRight>.Right creates DisposableRight3<TLeft, TRight> instance" {
        let right = DisposableEither3<MyDisposable<exn>, MyDisposable<int>>.Right(new MyDisposable<int>(1))
        do! assertEquals typeof<DisposableRight3<MyDisposable<exn>, MyDisposable<int>>> <| right.GetType()
    }
    let t2 = test "DisposableEither3<TLeft, TRight>.Left creates DisposableLeft3<TLeft, TRight> instance" {
        let left = DisposableEither3<MyDisposable<exn>, MyDisposable<string>>.Left(new MyDisposable<exn>(Exception("hoge")))
        do! assertEquals typeof<DisposableLeft3<MyDisposable<exn>, MyDisposable<string>>> <| left.GetType()
    }
    let t3 = test "DisposableRight3<TLeft, TRight> as IRight<TLeft, TRight> then Value should return its right value" {
        let value = new MyDisposable<float>(2.5)
        let right = DisposableEither3<MyDisposable<exn>, MyDisposable<float>>.Right(value) :> IRight<MyDisposable<exn>, MyDisposable<float>>
        do! assertEquals value right.Value
    }
    let t4 = test "DisposableRight3<TLeft, TRight>.IsRight should return true" {
        let right = DisposableEither3<MyDisposable<exn>, MyDisposable<string>>.Right(new MyDisposable<string>("hoge"))
        do! assertPred right.IsRight
    }
    let t5 = test "DisposableRight3<TLeft, TRight>.IsLeft should return false" {
        let right = DisposableEither3<MyDisposable<exn>, MyDisposable<string>>.Right(new MyDisposable<string>("hoge"))
        do! assertPred <| not right.IsLeft
    }
    let t6 = test "When DisposableRight3<TLeft, TRight> as IEither<TLeft, TRight> then IsRight should return true" {
        let right =
            DisposableEither3<MyDisposable<exn>, MyDisposable<float32>>.Right(new MyDisposable<float32>(-1.0f)) :>
                IEither<MyDisposable<exn>, MyDisposable<float32>>
        do! assertPred right.IsRight
    }
    let t7 = test "When DisposableRight3<TLeft, TRight> as IEither<TLeft, TRight> then IsLeft should return false" {
        let right =
            DisposableEither3<MyDisposable<exn>, MyDisposable<obj>>.Right(new MyDisposable<obj>(obj())) :>
                IEither<MyDisposable<exn>, MyDisposable<obj>>
        do! assertPred <| not right.IsLeft
    }
    let t8 = test "When DisposableRight3<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should return IRight<TLeft, TRight> instance" {
        let right =
            DisposableEither3<MyDisposable<exn>, MyDisposable<int>>.Right(new MyDisposable<int>(-1)) :>
                IEither<MyDisposable<exn>, MyDisposable<int>>
        do! assertPred (right.ToRight() :? IRight<MyDisposable<exn>, MyDisposable<int>>)
    }
    let t9 = test "When DisposableRight3<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should raise InvalidCastException" {
        let right =
            DisposableEither3<MyDisposable<exn>, MyDisposable<string>>.Right(new MyDisposable<string>("egg")) :>
                IEither<MyDisposable<exn>, MyDisposable<string>>
        let! e = trap { right.ToLeft() |> ignore }
        do! assertEquals typeof<InvalidCastException> <| e.GetType()
    }
    let t10 = test "DisposableLeft3<TLeft, TRight> as ILeft<TLeft, TRight> then Value should return its left value" {
        let err = new MyDisposable<exn>(Exception("Left"))
        let left = DisposableEither3<MyDisposable<exn>, MyDisposable<float>>.Left(err) :> ILeft<MyDisposable<exn>, MyDisposable<float>>
        do! assertEquals err left.Value
    }
    let t11 = test "DisposableLeft3<TLeft, TRight>.IsRight should return false" {
        let left = DisposableEither3<MyDisposable<exn>, MyDisposable<int>>.Left(new MyDisposable<exn>(Exception("fuga")))
        do! assertPred <| not left.IsRight
    }
    let t12 = test "DisposableLeft3<TLeft, TRight>.IsLeft should return true" {
        let left = DisposableEither3<MyDisposable<exn>, MyDisposable<int>>.Left(new MyDisposable<exn>(Exception("fuga")))
        do! assertPred left.IsLeft
    }
    let t13 = test "When DisposableLeft3<TLeft, TRight> as IEither<TLeft, TRight> Then IsRight should return false" {
        let left =
            DisposableEither3<MyDisposable<exn>, MyDisposable<int>>.Left(new MyDisposable<exn>(Exception("fuga"))) :>
                IEither<MyDisposable<exn>, MyDisposable<int>>
        do! assertPred <| not left.IsRight
    }
    let t14 = test "When DisposableLeft3<TLeft, TRight> as IEither<TLeft, TRight> Then IsLeft should return true" {
        let left =
            DisposableEither3<MyDisposable<exn>, MyDisposable<int>>.Left(new MyDisposable<exn>(Exception("fuga"))) :>
                IEither<MyDisposable<exn>, MyDisposable<int>>
        do! assertPred left.IsLeft
    }
    let t15 = test "When DisposableLeft3<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should raise InvalidCastException" {
        let left =
            DisposableEither3<MyDisposable<exn>, MyDisposable<int list>>.Left(new MyDisposable<exn>(Exception("Not List"))) :>
                IEither<MyDisposable<exn>, MyDisposable<int list>>
        let! e = trap { left.ToRight() |> ignore }
        do! assertEquals typeof<InvalidCastException> <| e.GetType()
    }
    let t16 = test "When DisposableLeft3<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should return ILeft<TLeft, TRight> instance" {
        let left =
            DisposableEither3<MyDisposable<exn>, MyDisposable<bool>>.Left(new MyDisposable<exn>(Exception("ToLeft"))) :>
                IEither<MyDisposable<exn>, MyDisposable<bool>>
        do! assertPred (left.ToLeft() :? ILeft<MyDisposable<exn>, MyDisposable<bool>>)
    }
    let t17 = test "Given target as MyDisposable<TLeft> and sut as DisposableLeft3<MyDisposable<TLeft>, TRight> When sut.Dispose() Then sut.Disposed is true" {
        let target = new MyDisposable<exn>(Exception("hoge"))
        do! assertPred <| not target.Disposed
        let sut = DisposableEither3<MyDisposable<exn>, MyDisposable<int>>.Left(target)
        do! assertPred sut.IsLeft
        sut.Dispose()
        do! assertPred <| target.Disposed
    }
    let t18 = test "Given target as MyDisposable<TRight> and sut as DisposableRight3<TLeft, MyDisposable<TRight>> When sut.Dispose() Then sut.Disposed is true" {
        let target = new MyDisposable<decimal>(3.14m)
        do! assertPred <| not target.Disposed
        let sut = DisposableEither3<MyDisposable<exn>, MyDisposable<decimal>>.Right(target)
        do! assertPred sut.IsRight
        sut.Dispose()
        do! assertPred <| target.Disposed
    }
