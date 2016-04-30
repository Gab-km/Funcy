namespace Funcy.Test

open Funcy.Future
open Persimmon
open UseTestNameByReflection

module EitherTCTest =
    module EitherTCAsPointedTest =
        open Funcy.Computations

        let ``EitherTC<TLeft>.Right<TRight> creates RightTC<TLeft, TRight> instance`` = test {
            let right = EitherTC<string>.Right(1)
            do! assertEquals typeof<RightTC<string, int>> <| right.GetType()
        }
        let ``EitherTC<TLeft>.Left<TRight> creates LeftTC<TLeft, TRight> instance`` = test {
            let left = EitherTC<string>.Left<int>("left")
            do! assertEquals typeof<LeftTC<string, int>> <| left.GetType()
        }
        let ``EitherTC<TLeft, TRight>.Point creates RightTC<TLeft, TRight> instance`` = test {
            let left = EitherTC<string>.Left<string>("left")
            let point = left.Pointed.Point(10)
            do! assertEquals typeof<RightTC<string, int>> <| point.GetType()
        }

    module GeneralTest =
        open System

        let ``RightTC<TLeft, TRight>.IsRight should return true`` = test {
            let right = EitherTC<exn>.Right("hoge")
            do! assertPred right.IsRight
        }
        let ``RightTC<TLeft, TRight>.Value should return its right value`` = test {
            let right = EitherTC<exn>.Right(2.5).ToRight()
            do! assertEquals 2.5 right.Value
        }
        let ``RightTC<TLeft, TRight>.IsLeft should return false`` = test {
            let right = EitherTC<exn>.Right("hoge")
            do! assertPred <| not right.IsLeft
        }
        let ``When RightTC<TLeft, TRight> then ToLeft should raise InvalidCastException`` = test {
            let right = EitherTC<exn>.Right("egg")
            let! e = trap { right.ToLeft() |> ignore }
            do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
        }
        let ``LeftTC<TLeft, TRight>.IsRight should return false`` = test {
            let left = EitherTC<exn>.Left<string>(Exception("fuga"))
            do! assertPred <| not left.IsRight
        }
        let ``LeftTC<TLeft, TRight>.Value should return its left value`` = test {
            let err = Exception("Left")
            let left = EitherTC<exn>.Left<int>(err).ToLeft()
            do! assertEquals err left.Value
        }
        let ``LeftTC<TLeft, TRight>.IsLeft should return true`` = test {
            let left = EitherTC<exn>.Left<float>(Exception("fuga"))
            do! assertPred left.IsLeft
        }
        let ``When LeftTC<TLeft, TRight> then ToRight should raise InvalidCastException`` = test {
            let left = EitherTC<exn>.Left<int list>(Exception("Not List"))
            let! e = trap { left.ToRight() |> ignore }
            do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
        }
        let ``RightTC<TLeft, TRight> should have equality1`` = test {
            let right = EitherTC<exn>.Right("seven")
            let other = EitherTC<exn>.Right("seven")
            do! assertEquals right other
            do! assertPred (right = other)
            do! assertEquals <|| (right.GetHashCode(), other.GetHashCode())
        }
        let ``RightTC<TLeft, TRight> should have equality2`` = test {
            let right = EitherTC<exn>.Right(7)
            let other = EitherTC<exn>.Right(8)
            do! assertNotEquals right other
        }
        let ``RightTC<TLeft, TRight> should have equality3`` = test {
            let right = EitherTC<exn>.Right(3.14)
            let other = EitherTC<exn>.Right(3.14f)
            do! (not >> assertPred) <| right.Equals(other)
        }
        let ``LeftTC<TLeft, TRight> should have equality1`` = test {
            let err = Exception("error")
            let left = EitherTC<exn>.Left<float>(err)
            let other = EitherTC<exn>.Left<float>(err)
            do! assertEquals left other
            do! assertPred (left = other)
        }
        let ``LeftTC<TLeft, TRight> should have equality2`` = test {
            let left = EitherTC<exn>.Left<decimal>(Exception("hoge"))
            let other = EitherTC<exn>.Left<decimal>(Exception("fuga"))
            do! assertNotEquals left other
        }
        let ``LeftTC<TLeft, TRight> should have equality3`` = test {
            let left = EitherTC<System.IO.IOException>.Left<int>(System.IO.IOException("Exception"))
            let other = EitherTC<System.ApplicationException>.Left<int>(System.ApplicationException("Exception"))
            do! (not >> assertPred) <| left.Equals(other)
        }
        let ``EitherTC<TLeft, TRight> should have equality`` = test {
            let right1 = EitherTC<exn>.Right(System.DateTime(2015, 7, 12))
            let right2 = EitherTC<exn>.Right(System.DateTime(2015, 7, 12))
            let right3 = EitherTC<exn>.Right(System.DateTime(2015, 7, 13))
            let err = Exception("hoge")
            let left1 = EitherTC<exn>.Left<System.DateTime>(err)
            let left2 = EitherTC<exn>.Left<System.DateTime>(err)
            let left3 = EitherTC<exn>.Left<System.DateTime>(Exception("hoge"))
            do! assertEquals right1 right2
            do! assertNotEquals right1 right3
            do! assertEquals left1 left2
            do! assertNotEquals left1 left3
            do! assertNotEquals right1 left1
            do! assertNotEquals right1 left3
            do! assertNotEquals right3 left1
        }

    module FunctorTest =
        open System

        let ``fmap RightTC<exn, int> (int -> string) = RightTC<exn, string>`` = test {
            let right = EitherTC<exn>.Right(42)
            let sut = right.FMap(Func<int, string>(fun x -> x.ToString()));
            do! assertEquals typeof<RightTC<exn, string>> <| sut.GetType()
            do! assertEquals sut <| EitherTC<exn>.Right("42")
        }

        let ``fmap RightTC<exn, float> (float -> float) = RightTC<exn, float>`` = test {
            let sut = EitherTC<exn>.Right(-1.4142).FMap(Func<float, float>(fun f -> Math.Abs(f)))
            do! assertEquals typeof<RightTC<exn, float>> <| sut.GetType()
            do! assertPred (sut = EitherTC<exn>.Right(1.4142))
        }

        let ``fmap LeftTC<exn, string> (string -> int) = LeftTC<exn, string>`` = test {
            let err = Exception("forty-two")
            let left = EitherTC<exn>.Left<string>(err)
            let sut = left.FMap(Func<string, int>(fun s -> System.Int32.Parse(s)))
            do! assertEquals typeof<LeftTC<exn, int>> <| sut.GetType()
            do! assertEquals sut <| EitherTC<exn>.Left<int>(err)
        }

    module ApplicativeTest =
        open System
        open Funcy.Extensions

        let ``Apply: EitherTC<exn, int> -> EitherTC<int -> int> -> EitherTC<exn, int>`` = test {
            let eitherX = EitherTC<exn>.Right(2)
            let eitherF = EitherTC<exn>.Right(Func<int, int>(fun x -> x * 3))
            let sut = eitherX.Apply(eitherF)
            do! assertEquals typeof<RightTC<exn, int>> <| sut.GetType()
            do! assertEquals 6 <| sut.ToRight().Value
        }

        let ``Apply: EitherTC<exn, float> -> EitherTC<exn, float -> string> -> EitherTC<exn, string>`` = test {
            let sut = EitherTC<exn>.Right(3.14).Apply(
                        EitherTC<exn>.Right(Func<float, string>(fun f -> f.ToString())))
            do! assertEquals typeof<RightTC<exn, string>> <| sut.GetType()
            do! assertEquals "3.14" <| sut.ToRight().Value
        }

        let ``RightTC<exn, decimal -> bool> <*> LeftTC<exn, decimal> = LeftTC<exn, bool>`` = test {
            let err = Exception("hoge")
            let eitherX = EitherTC<exn>.Left(err)
            let eitherF = EitherTC<exn>.Right(Func<decimal, bool>(fun d -> d > 2.718m))
            let sut = eitherX.Apply(eitherF)
            do! assertEquals typeof<LeftTC<exn, bool>> <| sut.GetType()
            do! assertEquals err <| sut.ToLeft().Value
        }

        let ``LeftTC<exn, string -> int> <*> RightTC<exn, string> = LeftTC<exn, int>`` = test {
            let err = Exception("fuga")
            let eitherX = EitherTC<exn>.Right("F#!F#!")
            let eitherF = EitherTC<exn>.Left<Func<string, int>>(err)
            let sut = eitherX.Apply(eitherF)
            do! assertEquals typeof<LeftTC<exn, int>> <| sut.GetType()
            do! assertEquals err <| sut.ToLeft().Value
        }

        let ``RightTC<exn, int> <* RightTC<exn, string> = RightTC<exn, int>`` = test {
            let sut = EitherTC<exn>.Right(4).ApplyLeft(EitherTC<exn>.Right("fuga"))
            do! assertEquals typeof<RightTC<exn, int>> <| sut.GetType()
            do! assertEquals sut <| EitherTC<exn>.Right(4)
        }

        let ``(int -> int) <$> RightTC<exn, int> = RightTC<int>`` = test {
            let right = EitherTC<exn>.Right(3)
            let sut = right.FMapA(Func<int, int>(fun x -> x * 4))
            do! assertEquals typeof<RightTC<exn, int>> <| sut.GetType()
            do! assertEquals sut <| EitherTC<exn>.Right(12)
        }

        let ``(double -> string) <$> LeftTC<string, double> = LeftTC<string, string>`` = test {
            let sut = EitherTC<string>.Left<double>("left").FMapA(Func<double, string>(fun d -> d.ToString()))
            do! assertPred sut.IsLeft
        }

    module ComputationTest =
        open System

        let ``RightTC + RightTC should return RightTC value`` = test {
            let eitherX = EitherTC<exn>.Right(1)
            let eitherY = EitherTC<exn>.Right(2)
            let sut = eitherX.ComputeWith(fun x ->
                        eitherY.FMap(fun y ->
                            x + y))
            do! assertEquals typeof<RightTC<exn, int>> <| sut.GetType()
            do! assertEquals 3 <| sut.ToRight().Value
        }

        let ``RightTC + LeftTC should return LeftTC value`` = test {
            let ex = Exception("fuga")
            let eitherX = EitherTC<exn>.Right(3)
            let eitherY = EitherTC<exn>.Left<int>(ex)
            let sut = eitherX.ComputeWith (fun x ->
                        eitherY.FMap (fun y ->
                            x + y))
            do! assertEquals typeof<LeftTC<exn, int>> <| sut.GetType()
            do! assertPred <| sut.IsLeft
            do! assertEquals ex <| sut.ToLeft().Value
        }

        let ``LeftTC + RightTC should return LeftTC value`` = test {
            let sut = EitherTC<exn>.Left<int>(Exception("bar")).ComputeWith (fun x ->
                        EitherTC<exn>.Right(4).FMap (fun y ->
                            x + y))
            do! assertEquals typeof<LeftTC<exn, int>> <| sut.GetType()
            do! assertPred <| sut.IsLeft
        }

        let ``LeftTC + LeftTC should return Left value`` = test {
            let ex1 = Exception("exn1")
            let ex2 = Exception("exn2")
            let eitherX = EitherTC<exn>.Left<int>(ex1)
            let eitherY = EitherTC<exn>.Left<int>(ex2)
            let sut = eitherX.ComputeWith (fun x ->
                        eitherY.FMap (fun y ->
                            x + y))
            do! assertEquals typeof<LeftTC<exn, int>> <| sut.GetType()
            do! assertPred <| sut.IsLeft
            do! assertEquals ex1 <| sut.ToLeft().Value
        }
