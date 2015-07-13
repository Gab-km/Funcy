namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open UseTestNameByReflection

module EitherComputationTest =
    let ``Right + Right should return Right value`` = test {
        let eitherX = Either<exn, int>.Right(1)
        let eitherY = Either<exn, int>.Right(2)
        let sut = eitherX.ComputeWith(fun x ->
                    eitherY.Compute(fun y ->
                        x + y))
        do! assertEquals typeof<Right<exn, int>> <| sut.GetType()
        do! assertEquals 3 <| sut.ToRight().Value
    }

    let ``Right + Left should return Left value`` = test {
        let eitherX = Either<exn, int>.Right(3)
        let eitherY = Either<exn, int>.Left(Exception("fuga"))
        let sut = eitherX.ComputeWith (fun x ->
                    eitherY.Compute (fun y ->
                        x + y))
        do! assertEquals typeof<Left<exn, int>> <| sut.GetType()
        do! assertPred <| sut.ToLeft().IsLeft
    }

    let ``Left + Right should return Left value`` = test {
        let sut = Either<exn, int>.Left(Exception("bar")).ComputeWith (fun x ->
                    Either<exn, int>.Right(4).Compute (fun y ->
                        x + y))
        do! assertEquals typeof<Left<exn, int>> <| sut.GetType()
        do! assertPred <| sut.ToLeft().IsLeft
    }

    let ``Left + Left should return Left value`` = test {
        let eitherX = Either<exn, int>.Left(Exception("exn1"))
        let eitherY = Either<exn, int>.Left(Exception("exn2"))
        let sut = eitherX.ComputeWith (fun x ->
                    eitherY.Compute (fun y ->
                        x + y))
        do! assertEquals typeof<Left<exn, int>> <| sut.GetType()
        do! assertPred <| sut.ToLeft().IsLeft
    }
