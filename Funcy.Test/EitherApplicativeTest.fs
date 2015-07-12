namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module EitherApplicativeTest =
    let ``Apply: Either<exn, int> -> Either<int -> int> -> Either<exn, int>`` = test {
        let eitherX = Either<exn, int>.Right(2)
        let eitherF = Either<exn, Func<int, int>>.Right(Func<int, int>(fun x -> x * 3))
        let sut = eitherX.Apply(eitherF)
        do! assertEquals typeof<Right<exn, int>> <| sut.GetType()
        do! assertEquals 6 <| sut.ToRight().Value
    }

    let ``Apply: Either<exn, float> -> Either<exn, float -> string> -> Either<exn, string>`` = test {
        let sut = Either<exn, float>.Right(3.14).Apply(
                    Either<exn, Func<float, string>>.Right(Func<float, string>(fun f -> f.ToString())))
        do! assertEquals typeof<Right<exn, string>> <| sut.GetType()
        do! assertEquals "3.14" <| sut.ToRight().Value
    }

    let ``Right<exn, decimal -> bool> <*> Left<exn, decimal> = Left<exn, bool>`` = test {
        let err = Exception("hoge")
        let eitherX = Either<exn, decimal>.Left(err)
        let eitherF = Either<exn, Func<decimal, bool>>.Right(Func<decimal, bool>(fun d -> d > 2.718m))
        let sut = eitherX.Apply(eitherF)
        do! assertEquals typeof<Left<exn, bool>> <| sut.GetType()
        do! assertEquals err <| sut.ToLeft().Value
    }

    let ``Left<exn, string -> int> <*> Right<exn, string> = Left<exn, int>`` = test {
        let err = Exception("fuga")
        let eitherX = Either<exn, string>.Right("F#!F#!")
        let eitherF = Either<exn, Func<string, int>>.Left(err)
        let sut = eitherX.Apply(eitherF)
        do! assertEquals typeof<Left<exn, int>> <| sut.GetType()
        do! assertEquals err <| sut.ToLeft().Value
    }

    let ``Right<exn, int> <* Right<exn, string> = Right<exn, int>`` = test {
        let sut = Either<exn, int>.Right(4).ApplyLeft(Either<exn, string>.Right("fuga"))
        do! assertEquals typeof<Right<exn, int>> <| sut.GetType()
        do! assertEquals sut <| (Either<exn, int>.Right(4) :> IEither<exn, int>)
    }
