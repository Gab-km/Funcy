#nowarn "67"
namespace Funcy.Test

open Funcy
open Persimmon

module DisposableMaybeTest =
    let ``DisposableMaybe.Some creates Some<T> instance`` = test "DisposableMaybe.Some creates Some<T> instance" {
        let md = new MyDisposable<int>(1)
        let some = DisposableMaybe.Some(md)
        do! assertEquals typeof<DisposableSome<MyDisposable<int>>> <| some.GetType()
    }
    let ``DisposableMaybe.None creates None<T> instance`` = test "DisposableMaybe.None creates None<T> instance" {
        let none = DisposableMaybe<MyDisposable<string>>.None()
        do! assertEquals typeof<DisposableNone<MyDisposable<string>>> <| none.GetType()
    }
    let ``DisposableSome<T> as ISome<T> then Value should return its value`` = test "DisposableSome<T> as ISome<T> then Value should return its value" {
        let md = new MyDisposable<float>(2.5)
        let some = DisposableMaybe.Some(md) :> ISome<MyDisposable<float>>
        let expected = new MyDisposable<float>(2.5)
        do! assertEquals expected some.Value
    }
    let ``DisposableSome<T>.IsSome should return true`` = test "DisposableSome<T>.IsSome should return true" {
        let some = DisposableMaybe.Some(new MyDisposable<string>("hoge"))
        do! assertPred some.IsSome
    }
    let ``DisposableSome<T>.IsNone should return false`` = test "DisposableSome<T>.IsNone should return false" {
        let some = DisposableMaybe.Some(new MyDisposable<string>("hoge"))
        do! assertPred <| not some.IsNone
    }
    let ``When DisposableSome<T> as IMaybe<T> then IsSome should return true`` = test "When DisposableSome<T> as IMaybe<T> then IsSome should return true" {
        let some = DisposableMaybe.Some(new MyDisposable<float32>(-1.0f)) :> IMaybe<MyDisposable<float32>>
        do! assertPred some.IsSome
    }
    let ``When DisposableSome<T> as IMaybe<T> then IsNone should return false`` = test "When DisposableSome<T> as IMaybe<T> then IsNone should return false" {
        let some = DisposableMaybe.Some(new MyDisposable<obj>(obj())) :> IMaybe<MyDisposable<obj>>
        do! assertPred <| not some.IsNone
    }
    let ``When DisposableSome<T> as IMaybe<T> then ToSome should return ISome<T> instance`` = test "When DisposableSome<T> as IMaybe<T> then ToSome should return ISome<T> instance" {
        let some = DisposableMaybe.Some(new MyDisposable<int>(-1)) :> IMaybe<MyDisposable<int>>
        do! assertPred (some.ToSome() :? ISome<MyDisposable<int>>)
    }
    let ``When DisposableSome<T> as IMaybe<T> then ToNone should raise InvalidCastException`` = test "When DisposableSome<T> as IMaybe<T> then ToNone should raise InvalidCastException" {
        let some = DisposableMaybe.Some(new MyDisposable<string>("egg")) :> IMaybe<MyDisposable<string>>
        let! e = trap { some.ToNone() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``DisposableNone<T>.IsSome should return false`` = test "DisposableNone<T>.IsSome should return false" {
        let none = DisposableMaybe<MyDisposable<int>>.None()
        do! assertPred <| not none.IsSome
    }
    let ``DisposableNone<T>.IsNone should return true`` = test "DisposableNone<T>.IsNone should return true" {
        let none = DisposableMaybe<MyDisposable<int>>.None()
        do! assertPred none.IsNone
    }
    let ``When DisposableNone<T> as IMaybe<T> then IsSome should return false`` = test "When DisposableNone<T> as IMaybe<T> then IsSome should return false" {
        let none = DisposableMaybe<MyDisposable<float>>.None() :> IMaybe<MyDisposable<float>>
        do! assertPred <| not none.IsSome
    }
    let ``When DisposableNone<T> as IMaybe<T> then IsNone should return true`` = test "When DisposableNone<T> as IMaybe<T> then IsNone should return true" {
        let none = DisposableMaybe<MyDisposable<decimal>>.None() :> IMaybe<MyDisposable<decimal>>
        do! assertPred none.IsNone
    }
    let ``When DisposableNone<T> as IMaybe<T> then ToSome should raise InvalidCastException`` = test "When DisposableNone<T> as IMaybe<T> then ToSome should raise InvalidCastException" {
        let none = DisposableMaybe<MyDisposable<int list>>.None() :> IMaybe<MyDisposable<int list>>
        let! e = trap { none.ToSome() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``When DisposableNone<T> as IMaybe<T> then ToNone should return INone<T> instance`` = test "When DisposableNone<T> as IMaybe<T> then ToNone should return INone<T> instance" {
        let none = DisposableMaybe<MyDisposable<bool>>.None() :> IMaybe<MyDisposable<bool>>
        do! assertPred (none.ToNone() :? INone<MyDisposable<bool>>)
    }
    let ``Given target as MyDisposable<T> and sut as DisposableSome<MyDisposable<T>> When sut.Dispose() Then sut.Disposed is true`` =
        test "Given target as MyDisposable<T> and sut as DisposableSome<MyDisposable<T>> When sut.Dispose() Then sut.Disposed is true" {
            let target = new MyDisposable<string>("hoge")
            do! assertPred <| not target.Disposed
            let sut = DisposableMaybe<MyDisposable<string>>.Some(target)
            do! assertPred sut.IsSome
            sut.Dispose()
            do! assertPred <| target.Disposed
        }
