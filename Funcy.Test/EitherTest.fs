#nowarn "67"
namespace Funcy.Test

open System
open Funcy
open Persimmon
open UseTestNameByReflection

module EitherTest =

    let ``Either<TLeft, TRight>.Right creates Right<TLeft, TRight> instance`` = test {
        let right = Either<exn, int>.Right(1)
        do! assertEquals typeof<Right<exn, int>> <| right.GetType()
    }
    let ``Either<TLeft, TRight>.Left creates Left<TLeft, TRight> instance`` = test {
        let left = Either<exn, string>.Left(Exception("hoge"))
        do! assertEquals typeof<Left<exn, string>> <| left.GetType()
    }
    let ``Right<TLeft, TRight>.Value should return its right value`` = test {
        let right = Either<exn, float>.Right(2.5)
        do! assertEquals 2.5 right.Value
    }
    let ``Right<TLeft, TRight>.IsRight should return true`` = test {
        let right = Either<exn, string>.Right("hoge")
        do! assertPred right.IsRight
    }
    let ``Right<TLeft, TRight>.IsLeft should return false`` = test {
        let right = Either<exn, string>.Right("hoge")
        do! assertPred <| not right.IsLeft
    }
    let ``When Right<TLeft, TRight> as Either<TLeft, TRight> then IsRight should return true`` = test {
        let right = Either<exn, float32>.Right(-1.0f) :> Either<exn, float32>
        do! assertPred right.IsRight
    }
    let ``When Right<TLeft, TRight> as Either<TLeft, TRight> then IsLeft should return false`` = test {
        let right = Either<exn, obj>.Right(obj()) :> Either<exn, obj>
        do! assertPred <| not right.IsLeft
    }
    let ``When Right<TLeft, TRight> as Either<TLeft, TRight> then ToRight should return IRight<TLeft, TRight> instance`` = test {
        let right = Either<exn, int>.Right(-1) :> Either<exn, int>
        do! assertPred (right.ToRight() :? Right<exn, int>)
    }
    let ``When Right<TLeft, TRight> as Either<TLeft, TRight> then ToLeft should raise InvalidCastException`` = test {
        let right = Either<exn, string>.Right("egg") :> Either<exn, string>
        let! e = trap { right.ToLeft() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``Left<TLeft, TRight>.Value should return its left value`` = test {
        let err = Exception("Left")
        let left = Either<exn, float>.Left(err)
        do! assertEquals err left.Value
    }
    let ``Left<TLeft, TRight>.IsRight should return false`` = test {
        let left = Either<exn, int>.Left(Exception("fuga"))
        do! assertPred <| not left.IsRight
    }
    let ``Left<TLeft, TRight>.IsLeft should return true`` = test {
        let left = Either<exn, int>.Left(Exception("fuga"))
        do! assertPred left.IsLeft
    }
    let ``When Left<TLeft, TRight> as Either<TLeft, TRight> Then IsRight should return false`` = test {
        let left = Either<exn, int>.Left(Exception("fuga")) :> Either<exn, int>
        do! assertPred <| not left.IsRight
    }
    let ``When Left<TLeft, TRight> as Either<TLeft, TRight> Then IsLeft should return true`` = test {
        let left = Either<exn, int>.Left(Exception("fuga")) :> Either<exn, int>
        do! assertPred left.IsLeft
    }
    let ``When Left<TLeft, TRight> as Either<TLeft, TRight> then ToRight should raise InvalidCastException`` = test {
        let left = Either<exn, int list>.Left(Exception("Not List")) :> Either<exn, int list>
        let! e = trap { left.ToRight() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``When Left<TLeft, TRight> as Either<TLeft, TRight> then ToLeft should return ILeft<TLeft, TRight> instance`` = test {
        let left = Either<exn, bool>.Left(Exception("ToLeft")) :> Either<exn, bool>
        do! assertPred (left.ToLeft() :? Left<exn, bool>)
    }
    let ``Right<TLeft, TRight> should have equality1`` = test {
        let right = Either<exn, string>.Right("seven")
        let other = Either<exn, string>.Right("seven")
        do! assertEquals right other
        do! assertPred (right = other)
        do! assertEquals <|| (right.GetHashCode(), other.GetHashCode())
    }
    let ``Right<TLeft, TRight> should have equality2`` = test {
        let right = Either<exn, int>.Right(7)
        let other = Either<exn, int>.Right(8)
        do! assertNotEquals right other
    }
    let ``Right<TLeft, TRight> should have equality3`` = test {
        let right = Either<exn, float>.Right(3.14)
        let other = Either<exn, float32>.Right(3.14f)
        do! (not >> assertPred) <| right.Equals(other)
    }
    let ``Left<TLeft, TRight> should have equality1`` = test {
        let err = Exception("error")
        let left = Either<exn, float>.Left(err)
        let other = Either<exn, float>.Left(err)
        do! assertEquals left other
        do! assertPred (left = other)
    }
    let ``Left<TLeft, TRight> should have equality2`` = test {
        let left = Either<exn, decimal>.Left(Exception("hoge"))
        let other = Either<exn, decimal>.Left(Exception("fuga"))
        do! assertNotEquals left other
    }
    let ``Left<TLeft, TRight> should have equality3`` = test {
        let left = Either<System.IO.IOException, int>.Left(System.IO.IOException("Exception"))
        let other = Either<System.ApplicationException, int>.Left(System.ApplicationException("Exception"))
        do! (not >> assertPred) <| left.Equals(other)
    }
    let ``Either<TLeft, TRight> should have equality`` = test {
        let right1 = Either<exn, System.DateTime>.Right(System.DateTime(2015, 7, 12)) :> Either<exn, System.DateTime>
        let right2 = Either<exn, System.DateTime>.Right(System.DateTime(2015, 7, 12)) :> Either<exn, System.DateTime>
        let right3 = Either<exn, System.DateTime>.Right(System.DateTime(2015, 7, 13)) :> Either<exn, System.DateTime>
        let err = Exception("hoge")
        let left1 = Either<exn, System.DateTime>.Left(err) :> Either<exn, System.DateTime>
        let left2 = Either<exn, System.DateTime>.Left(err) :> Either<exn, System.DateTime>
        let left3 = Either<exn, System.DateTime>.Left(Exception("hoge")) :> Either<exn, System.DateTime>
        do! assertEquals right1 right2
        do! assertNotEquals right1 right3
        do! assertEquals left1 left2
        do! assertNotEquals left1 left3
        do! assertNotEquals right1 left1
        do! assertNotEquals right1 left3
        do! assertNotEquals right3 left1
    }
