#nowarn "67"
namespace Funcy.Test

open System
open Funcy
open Persimmon

module EitherTest =

    let t1 = test "Either<TLeft, TRight>.Right creates Right<TLeft, TRight> instance" {
        let right = Either<exn, int>.Right(1)
        do! assertEquals typeof<Right<exn, int>> <| right.GetType()
    }
    let t2 = test "Either<TLeft, TRight>.Left creates Left<TLeft, TRight> instance" {
        let left = Either<exn, string>.Left(Exception("hoge"))
        do! assertEquals typeof<Left<exn, string>> <| left.GetType()
    }
    let t3 = test "Right<TLeft, TRight> as IRight<TLeft, TRight> then Value should return its right value" {
        let right = Either<exn, float>.Right(2.5) :> IRight<exn, float>
        do! assertEquals 2.5 right.Value
    }
    let t4 = test "Right<TLeft, TRight>.IsRight should return true" {
        let right = Either<exn, string>.Right("hoge")
        do! assertPred right.IsRight
    }
    let t5 = test "Right<TLeft, TRight>.IsLeft should return false" {
        let right = Either<exn, string>.Right("hoge")
        do! assertPred <| not right.IsLeft
    }
    let t6 = test "When Right<TLeft, TRight> as IEither<TLeft, TRight> then IsRight should return true" {
        let right = Either<exn, float32>.Right(-1.0f) :> IEither<exn, float32>
        do! assertPred right.IsRight
    }
    let t7 = test "When Right<TLeft, TRight> as IEither<TLeft, TRight> then IsLeft should return false" {
        let right = Either<exn, obj>.Right(obj()) :> IEither<exn, obj>
        do! assertPred <| not right.IsLeft
    }
    let t8 = test "When Right<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should return IRight<TLeft, TRight> instance" {
        let right = Either<exn, int>.Right(-1) :> IEither<exn, int>
        do! assertPred (right.ToRight() :? IRight<exn, int>)
    }
    let t9 = test "When Right<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should raise InvalidCastException" {
        let right = Either<exn, string>.Right("egg") :> IEither<exn, string>
        let! e = trap { right.ToLeft() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let t10 = test "Left<TLeft, TRight> as ILeft<TLeft, TRight> then Value should return its left value" {
        let err = Exception("Left")
        let left = Either<exn, float>.Left(err) :> ILeft<exn, float>
        do! assertEquals err left.Value
    }
    let t11 = test "Left<TLeft, TRight>.IsRight should return false" {
        let left = Either<exn, int>.Left(Exception("fuga"))
        do! assertPred <| not left.IsRight
    }
    let t12 = test "Left<TLeft, TRight>.IsLeft should return true" {
        let left = Either<exn, int>.Left(Exception("fuga"))
        do! assertPred left.IsLeft
    }
    let t13 = test "When Left<TLeft, TRight> as IEither<TLeft, TRight> Then IsRight should return false" {
        let left = Either<exn, int>.Left(Exception("fuga")) :> IEither<exn, int>
        do! assertPred <| not left.IsRight
    }
    let t14 = test "When Left<TLeft, TRight> as IEither<TLeft, TRight> Then IsLeft should return true" {
        let left = Either<exn, int>.Left(Exception("fuga")) :> IEither<exn, int>
        do! assertPred left.IsLeft
    }
    let t15 = test "When Left<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should raise InvalidCastException" {
        let left = Either<exn, int list>.Left(Exception("Not List")) :> IEither<exn, int list>
        let! e = trap { left.ToRight() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let t16 = test "When Left<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should return ILeft<TLeft, TRight> instance" {
        let left = Either<exn, bool>.Left(Exception("ToLeft")) :> IEither<exn, bool>
        do! assertPred (left.ToLeft() :? ILeft<exn, bool>)
    }
