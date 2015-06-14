#nowarn "67"
namespace Funcy.Test

open Funcy
open Persimmon

module MaybeTest =
    let t1 = test "Maybe.Some creates Some<T> instance" {
        let some = Maybe.Some(1)
        do! assertEquals typeof<Some<int>> <| some.GetType()
    }
    let t2 = test "Maybe.None creates None<T> instance" {
        let none = Maybe<string>.None()
        do! assertEquals typeof<None<string>> <| none.GetType()
    }
    let t3 = test "Some<T> as ISome<T> then Value should return its value" {
        let some = Maybe.Some(2.5) :> ISome<float>
        do! assertEquals 2.5 some.Value
    }
    let t4 = test "Some<T>.IsSome should return true" {
        let some = Maybe.Some("hoge")
        do! assertPred some.IsSome
    }
    let t5 = test "Some<T>.IsNone should return false" {
        let some = Maybe.Some(3.14m)
        do! assertPred <| not some.IsNone
    }
    let t6 = test "When Some<T> as IMaybe<T> then IsSome should return true" {
        let some = Maybe.Some(-1.0f) :> IMaybe<float32>
        do! assertPred some.IsSome
    }
    let t7 = test "When Some<T> as IMaybe<T> then IsNone should return false" {
        let some = Maybe.Some(obj()) :> IMaybe<obj>
        do! assertPred <| not some.IsNone
    }
    let t8 = test "When Some<T> as IMaybe<T> then ToSome should return ISome<T> instance" {
        let some = Maybe.Some(-1) :> IMaybe<int>
        do! assertPred (some.ToSome() :? ISome<int>)
    }
    let t9 = test "When Some<T> as IMaybe<T> then ToNone should raise InvalidCastException" {
        let some = Maybe.Some("egg") :> IMaybe<string>
        let! e = trap { some.ToNone() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let t10 = test "None<T>.IsSome should return false" {
        let none = Maybe<int>.None()
        do! assertPred <| not none.IsSome
    }
    let t11 = test "None<T>.IsNone should return true" {
        let none = Maybe<int>.None()
        do! assertPred none.IsNone
    }
    let t12 = test "When None<T> as IMaybe<T> then IsSome should return false" {
        let none = Maybe<float>.None() :> IMaybe<float>
        do! assertPred <| not none.IsSome
    }
    let t13 = test "When None<T> as IMaybe<T> then IsNone should return true" {
        let none = Maybe<decimal>.None() :> IMaybe<decimal>
        do! assertPred none.IsNone
    }
    let t14 = test "When None<T> as IMaybe<T> then ToSome should raise InvalidCastException" {
        let none = Maybe<int list>.None() :> IMaybe<int list>
        let! e = trap { none.ToSome() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let t15 = test "When None<T> as IMaybe<T> then ToNone should return INone<T> instance" {
        let none = Maybe<bool>.None() :> IMaybe<bool>
        do! assertPred (none.ToNone() :? INone<bool>)
    }
