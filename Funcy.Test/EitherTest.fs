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
    let ``Right<TLeft, TRight>.ToRight() should return Right<TLeft, TRight> instance`` = test {
        let right = Either<exn, int>.Right(-1) :> Either<exn, int>
        let sut = right.ToRight()
        do! assertPred sut.IsRight
    }
    let ``Right<TLeft, TRight>.ToLeft() should raise InvalidCastException`` = test {
        let right = Either<exn, string>.Right("egg")
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
    let ``Left<TLeft, TRight>.ToRight() should raise InvalidCastException`` = test {
        let left = Either<exn, int list>.Left(Exception("Not List"))
        let! e = trap { left.ToRight() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``Left<TLeft, TRight>.ToLeft() should return Left<TLeft, TRight> instance`` = test {
        let left = Either<exn, bool>.Left(Exception("ToLeft"))
        let sut = left.ToLeft()
        do! assertPred sut.IsLeft
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

module EitherFunctorTest =
    let ``fmap Right<exn, int> (int -> string) = Right<exn, string>`` = test {
        let right = Either<exn, int>.Right(42)
        let sut = right.FMap(Func<int, string>(fun x -> x.ToString()));
        do! assertPred sut.IsRight
        do! assertEquals sut <| (Either<exn, string>.Right("42") :> Either<exn, string>)
    }

    let ``fmap Right<exn, float> (float -> float) = Right<exn, float>`` = test {
        let sut = Either<exn, float>.Right(-1.4142).FMap(Func<float, float>(fun f -> Math.Abs(f)))
        do! assertPred sut.IsRight
        do! assertPred (sut = (Either<exn, float>.Right(1.4142) :> Either<exn, float>))
    }

    let ``fmap Left<exn, string> (string -> int) = Left<exn, string>`` = test {
        let err = Exception("forty-two")
        let left = Either<exn, string>.Left(err)
        let sut = left.FMap(Func<string, int>(fun s -> System.Int32.Parse(s)))
        do! assertPred sut.IsLeft
        do! assertEquals sut <| (Either<exn, int>.Left(err) :> Either<exn, int>)
    }

module EitherApplicativeTest =
    let ``Apply: Either<exn, float> -> Either<exn, float -> string> -> Either<exn, string>`` = test {
        let sut = Either<exn, float>.Right(3.14).Apply(
                    Either<exn, Func<float, string>>.Right(Func<float, string>(fun f -> f.ToString())))
        do! assertPred sut.IsRight
        do! assertEquals "3.14" <| sut.ToRight().Value
    }

    let ``Right<exn, decimal -> bool> <*> Left<exn, decimal> = Left<exn, bool>`` = test {
        let err = Exception("hoge")
        let eitherX = Either<exn, decimal>.Left(err)
        let eitherF = Either<exn, Func<decimal, bool>>.Right(Func<decimal, bool>(fun d -> d > 2.718m))
        let sut = eitherX.Apply(eitherF)
        do! assertPred sut.IsLeft
        do! assertEquals err <| sut.ToLeft().Value
    }

    let ``Left<exn, string -> int> <*> Right<exn, string> = Left<exn, int>`` = test {
        let err = Exception("fuga")
        let eitherX = Either<exn, string>.Right("F#!F#!")
        let eitherF = Either<exn, Func<string, int>>.Left(err)
        let sut = eitherX.Apply(eitherF)
        do! assertPred sut.IsLeft
        do! assertEquals err <| sut.ToLeft().Value
    }

    let ``Right<exn, int> <* Right<exn, string> = Right<exn, int>`` = test {
        let sut = Either<exn, int>.Right(4).ApplyLeft(Either<exn, string>.Right("fuga"))
        do! assertPred sut.IsRight
        do! assertEquals sut <| (Either<exn, int>.Right(4) :> Either<exn, int>)
    }

module EitherComputationTest =
    let ``Right + Right should return Right value`` = test {
        let eitherX = Either<exn, int>.Right(1)
        let eitherY = Either<exn, int>.Right(2)
        let sut = eitherX.ComputeWith(fun x ->
                    eitherY.FMap(fun y ->
                        x + y))
        do! assertPred sut.IsRight
        do! assertEquals 3 <| sut.ToRight().Value
    }

    let ``Right + Left should return Left value`` = test {
        let eitherX = Either<exn, int>.Right(3)
        let eitherY = Either<exn, int>.Left(Exception("fuga"))
        let sut = eitherX.ComputeWith (fun x ->
                    eitherY.FMap (fun y ->
                        x + y))
        do! assertPred <| sut.IsLeft
    }

    let ``Left + Right should return Left value`` = test {
        let sut = Either<exn, int>.Left(Exception("bar")).ComputeWith (fun x ->
                    Either<exn, int>.Right(4).FMap (fun y ->
                        x + y))
        do! assertPred <| sut.IsLeft
    }

    let ``Left + Left should return Left value`` = test {
        let eitherX = Either<exn, int>.Left(Exception("exn1"))
        let eitherY = Either<exn, int>.Left(Exception("exn2"))
        let sut = eitherX.ComputeWith (fun x ->
                    eitherY.FMap (fun y ->
                        x + y))
        do! assertPred <| sut.IsLeft
    }
