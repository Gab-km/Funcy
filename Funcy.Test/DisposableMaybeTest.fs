namespace Funcy.Test

open Funcy
open Persimmon

module DisposableMaybeTest =
    type MyDisposable<'T when 'T: equality>(value : 'T) =
        member self.Value = value
        member val Disposed = false with get, set
        override self.Equals (other: obj) =
            if (other.GetType() <> typeof<MyDisposable<'T>>) then false
            else
                self.Value = (other :?> MyDisposable<'T>).Value
        override self.GetHashCode() = value.GetHashCode()
        interface System.IDisposable with
            member self.Dispose() = self.Disposed <- true
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
        let some = DisposableMaybe.Some(md)
        let expected = new MyDisposable<float>(2.5)
        do! assertEquals expected (some :> ISome<MyDisposable<float>>).Value
    }
    let ``DisposableSome<T>.IsSome should return true`` = test "DisposableSome<T>.IsSome should return true" {
        let some = Maybe.Some(new MyDisposable<string>("hoge"))
        do! assertPred some.IsSome
    }
    let ``When DisposableSome<T> as IMaybe<T> then IsSome should return true`` = test "When DisposableSome<T> as IMaybe<T> then IsSome should return true" {
        let some = DisposableMaybe.Some(new MyDisposable<float32>(-1.0f))
        do! assertPred (some :> IMaybe<MyDisposable<float32>>).IsSome
    }
    let ``When DisposableSome<T> as IMaybe<T> then IsNone should return false`` = test "When DisposableSome<T> as IMaybe<T> then IsNone should return false" {
        let some = DisposableMaybe.Some(new MyDisposable<obj>(obj()))
        do! assertPred <| not (some :> IMaybe<MyDisposable<obj>>).IsNone
    }
    let ``When DisposableSome<T> as IMaybe<T> then ToSome should return ISome<T> instance`` = test "When DisposableSome<T> as IMaybe<T> then ToSome should return ISome<T> instance" {
        let some = DisposableMaybe.Some(new MyDisposable<int>(-1))
        do! assertPred ((some :> IMaybe<MyDisposable<int>>).ToSome() :? ISome<MyDisposable<int>>)
    }
    let ``When DisposableSome<T> as IMaybe<T> then ToNone should raise InvalidCastException`` = test "When DisposableSome<T> as IMaybe<T> then ToNone should raise InvalidCastException" {
        let some = DisposableMaybe.Some(new MyDisposable<string>("egg"))
        let! e = trap { (some :> IMaybe<MyDisposable<string>>).ToNone() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``DisposableNone<T>.IsSome should return false`` = test "DisposableNone<T>.IsSome should return false" {
        let none = DisposableMaybe<MyDisposable<int>>.None()
        do! assertPred <| not none.IsSome
    }
    let ``When DisposableNone<T> as IMaybe<T> then IsSome should return false`` = test "When DisposableNone<T> as IMaybe<T> then IsSome should return false" {
        let none = DisposableMaybe<MyDisposable<float>>.None()
        do! assertPred <| not (none :> IMaybe<MyDisposable<float>>).IsSome
    }
    let ``When DisposableNone<T> as IMaybe<T> then IsNone should return true`` = test "When DisposableNone<T> as IMaybe<T> then IsNone should return true" {
        let none = DisposableMaybe<MyDisposable<decimal>>.None()
        do! assertPred (none :> IMaybe<MyDisposable<decimal>>).IsNone
    }
    let ``When DisposableNone<T> as IMaybe<T> then ToSome should raise InvalidCastException`` = test "When DisposableNone<T> as IMaybe<T> then ToSome should raise InvalidCastException" {
        let none = DisposableMaybe<MyDisposable<int list>>.None()
        let! e = trap { (none :> IMaybe<MyDisposable<int list>>).ToSome() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``When DisposableNone<T> as IMaybe<T> then ToNone should return INone<T> instance`` = test "When DisposableNone<T> as IMaybe<T> then ToNone should return INone<T> instance" {
        let none = DisposableMaybe<MyDisposable<bool>>.None()
        do! assertPred ((none :> IMaybe<MyDisposable<bool>>).ToNone() :? INone<MyDisposable<bool>>)
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
