namespace Funcy.Test

open Funcy
open Funcy.Computations
open Persimmon
open Persimmon.Dried
open UseTestNameByReflection

module FunctorLawsCheck =
    let funcId<'T> = System.Func<'T, 'T>(id)

    module FunctorLawsInMaybe =
        let ``Identity in Some<T>`` = Prop.forAll(Arb.int)(fun i ->
            let maybe = Maybe.Some(i)
            maybe.FMap(funcId) = (maybe :> IMaybe<int>)
        )

        let ``Identity in None<T>`` = Prop.forAll(Arb.int)(fun i ->
            let maybe = Maybe<int>.None()
            maybe.FMap(funcId) = (maybe :> IMaybe<int>)
        )

        let ``Composition in Some<T>`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f g i ->
            let maybe = Maybe.Some(i)
            maybe.FMap(f).FMap(g) = (maybe.FMap(Composition.Compose(g, f)) :> IFunctor<int>)
        )

        let ``Composition in None<T>`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int))(fun f g ->
            let maybe = Maybe<int>.None()
            maybe.FMap(f).FMap(g) = (maybe.FMap(Composition.Compose(g, f)) :> IFunctor<int>)
        )

        let ``Functor laws`` = property {
            apply ``Identity in Some<T>``
            apply ``Identity in None<T>``
            apply ``Composition in Some<T>``
            apply ``Composition in None<T>``
        }
        
    module FunctorLawsInEither =
        let ``Identity in Right<TLeft, TRight>`` = Prop.forAll(Arb.int)(fun i ->
            let either = Either<exn, int>.Right(i)
            either.FMap(funcId) = (either :> IEither<exn, int>)
        )

        let ``Identity in Left<TLeft, TRight>`` = Prop.forAll(Arb.int)(fun i ->
            let either = Either<exn, int>.Left(exn("hoge"))
            either.FMap(funcId) = (either :> IEither<exn, int>)
        )

        let ``Composition in Right<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f g i ->
            let either = Either<exn, int>.Right(i)
            either.FMap(f).FMap(g) = (either.FMap(Composition.Compose(g, f)) :> IFunctor<int>)
        )

        let ``Composition in Left<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int))(fun f g ->
            let either = Either<exn, int>.Left(exn("fuga"))
            either.FMap(f).FMap(g) = (either.FMap(Composition.Compose(g, f)) :> IFunctor<int>)
        )

        let ``Functor laws`` = property {
            apply ``Identity in Right<TLeft, TRight>``
            apply ``Identity in Left<TLeft, TRight>``
            apply ``Composition in Right<TLeft, TRight>``
            apply ``Composition in Left<TLeft, TRight>``
        }
