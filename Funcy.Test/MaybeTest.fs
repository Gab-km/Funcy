#nowarn "67"
namespace Funcy.Test

open Funcy
open Persimmon
open UseTestNameByReflection

module MaybeTest =
    let ``Maybe.Some creates Some<T> instance`` = test {
        let some = Maybe.Some(1)
        do! assertEquals typeof<Some<int>> <| some.GetType()
    }
    let ``Maybe.None creates None<T> instance`` = test {
        let none = Maybe<string>.None()
        do! assertEquals typeof<None<string>> <| none.GetType()
    }
    let ``Some<T> as ISome<T> then Value should return its value`` = test {
        let some = Maybe.Some(2.5) :> ISome<float>
        do! assertEquals 2.5 some.Value
    }
    let ``Some<T>.IsSome should return true`` = test {
        let some = Maybe.Some("hoge")
        do! assertPred some.IsSome
    }
    let ``Some<T>.IsNone should return false`` = test {
        let some = Maybe.Some(3.14m)
        do! assertPred <| not some.IsNone
    }
    let ``When Some<T> as IMaybe<T> then IsSome should return true`` = test {
        let some = Maybe.Some(-1.0f) :> IMaybe<float32>
        do! assertPred some.IsSome
    }
    let ``When Some<T> as IMaybe<T> then IsNone should return false`` = test {
        let some = Maybe.Some(obj()) :> IMaybe<obj>
        do! assertPred <| not some.IsNone
    }
    let ``When Some<T> as IMaybe<T> then ToSome should return ISome<T> instance`` = test {
        let some = Maybe.Some(-1) :> IMaybe<int>
        do! assertPred (some.ToSome() :? ISome<int>)
    }
    let ``When Some<T> as IMaybe<T> then ToNone should raise InvalidCastException`` = test {
        let some = Maybe.Some("egg") :> IMaybe<string>
        let! e = trap { some.ToNone() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``None<T>.IsSome should return false`` = test {
        let none = Maybe<int>.None()
        do! assertPred <| not none.IsSome
    }
    let ``None<T>.IsNone should return true`` = test {
        let none = Maybe<int>.None()
        do! assertPred none.IsNone
    }
    let ``When None<T> as IMaybe<T> then IsSome should return false`` = test {
        let none = Maybe<float>.None() :> IMaybe<float>
        do! assertPred <| not none.IsSome
    }
    let ``When None<T> as IMaybe<T> then IsNone should return true`` = test {
        let none = Maybe<decimal>.None() :> IMaybe<decimal>
        do! assertPred none.IsNone
    }
    let ``When None<T> as IMaybe<T> then ToSome should raise InvalidCastException`` = test {
        let none = Maybe<int list>.None() :> IMaybe<int list>
        let! e = trap { none.ToSome() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``When None<T> as IMaybe<T> then ToNone should return INone<T> instance`` = test {
        let none = Maybe<bool>.None() :> IMaybe<bool>
        do! assertPred (none.ToNone() :? INone<bool>)
    }
