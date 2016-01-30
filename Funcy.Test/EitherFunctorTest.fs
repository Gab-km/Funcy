namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module EitherFunctorTest =
    let ``fmap Right<exn, int> (int -> string) = Right<exn, string>`` = test {
        let right = Either<exn, int>.Right(42)
        let sut = right.FMap(Func<int, string>(fun x -> x.ToString()));
        do! assertEquals typeof<Right<exn, string>> <| sut.GetType()
        do! assertEquals sut <| Either<exn, string>.Right("42")
    }

    let ``fmap Right<exn, float> (float -> float) = Right<exn, float>`` = test {
        let sut = Either<exn, float>.Right(-1.4142).FMap(Func<float, float>(fun f -> Math.Abs(f)))
        do! assertEquals typeof<Right<exn, float>> <| sut.GetType()
        do! assertPred (sut = Either<exn, float>.Right(1.4142))
    }

    let ``fmap Left<exn, string> (string -> int) = Left<exn, string>`` = test {
        let err = Exception("forty-two")
        let left = Either<exn, string>.Left(err)
        let sut = left.FMap(Func<string, int>(fun s -> System.Int32.Parse(s)))
        do! assertEquals typeof<Left<exn, int>> <| sut.GetType()
        do! assertEquals sut <| Either<exn, int>.Left(err)
    }
