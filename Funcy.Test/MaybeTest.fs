namespace Funcy.Test

open Funcy
open Persimmon

module MaybeTest =
    let ``Maybe.Some creates Some<T> instance`` = test "Maybe.Some creates Some<T> instance" {
        let some = Maybe.Some(1)
        do! assertEquals typeof<Some<int>> <| some.GetType()
    }
    let ``Maybe.None creates None<T> instance`` = test "Maybe.None creates None<T> instance" {
        let none = Maybe<string>.None()
        do! assertEquals typeof<None<string>> <| none.GetType()
    }
    let ``Some<T> as ISome<T> then Value should return its value`` = test "Some<T> as ISome<T> then Value should return its value" {
        let some = Maybe.Some(2.5)
        do! assertEquals 2.5 (some :> ISome<float>).Value
    }
    let ``Some<T>.IsSome should return true`` = test "Some<T>.IsSome should return true" {
        let some = Maybe.Some("hoge")
        do! assertPred some.IsSome
    }
    let ``When Some<T> as IMaybe<T> then IsSome should return true`` = test "When Some<T> as IMaybe<T> then IsSome should return true" {
        let some = Maybe.Some(-1.0f)
        do! assertPred (some :> IMaybe<float32>).IsSome
    }
    let ``When Some<T> as IMaybe<T> then IsNone should return false`` = test "When Some<T> as IMaybe<T> then IsNone should return false" {
        let some = Maybe.Some(obj())
        do! assertPred <| not (some :> IMaybe<obj>).IsNone
    }
    let ``When Some<T> as IMaybe<T> then ToSome should return ISome<T> instance`` = test "When Some<T> as IMaybe<T> then ToSome should return ISome<T> instance" {
        let some = Maybe.Some(-1)
        do! assertPred ((some :> IMaybe<int>).ToSome() :? ISome<int>)
    }
    let ``When Some<T> as IMaybe<T> then ToNone should raise InvalidCastException`` = test "When Some<T> as IMaybe<T> then ToNone should raise InvalidCastException" {
        let some = Maybe.Some("egg")
        let! e = trap { (some :> IMaybe<string>).ToNone() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``None<T>.IsSome should return false`` = test "None<T>.IsSome should return false" {
        let none = Maybe<int>.None()
        do! assertPred <| not none.IsSome
    }
    let ``When None<T> as IMaybe<T> then IsSome should return false`` = test "When None<T> as IMaybe<T> then IsSome should return false" {
        let none = Maybe<float>.None()
        do! assertPred <| not (none :> IMaybe<float>).IsSome
    }
    let ``When None<T> as IMaybe<T> then IsNone should return true`` = test "When None<T> as IMaybe<T> then IsNone should return true" {
        let none = Maybe<decimal>.None()
        do! assertPred (none :> IMaybe<decimal>).IsNone
    }
    let ``When None<T> as IMaybe<T> then ToSome should raise InvalidCastException`` = test "When None<T> as IMaybe<T> then ToSome should raise InvalidCastException" {
        let none = Maybe<int list>.None()
        let! e = trap { (none :> IMaybe<int list>).ToSome() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``When None<T> as IMaybe<T> then ToNone should return INone<T> instance`` = test "When None<T> as IMaybe<T> then ToNone should return INone<T> instance" {
        let none = Maybe<bool>.None()
        do! assertPred ((none :> IMaybe<bool>).ToNone() :? INone<bool>)
    }
