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
            // fmap id == id
            maybe.FMap(funcId) = (maybe :> Maybe<int>)
        )

        let ``Identity in None<T>`` = Prop.forAll(Arb.int)(fun i ->
            let maybe = Maybe<int>.None()
            // fmap id == id
            maybe.FMap(funcId) = (maybe :> Maybe<int>)
        )

        let ``Composition in Some<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let maybe = Maybe.Some(i)
            // fmap (f . g) == fmap f . fmap g
            maybe.FMap(Composition.Compose(g, f)) = maybe.FMap(f).FMap(g)
        )

        let ``Composition in None<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int))(fun f g ->
            let maybe = Maybe<int>.None()
            // fmap (f . g) == fmap f . fmap g
            maybe.FMap(Composition.Compose(g, f)) = maybe.FMap(f).FMap(g)
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
            // fmap id == id
            either.FMap(funcId) = (either :> Either<exn, int>)
        )

        let ``Identity in Left<TLeft, TRight>`` = Prop.forAll(Arb.string.NonNull)(fun s ->
            let either = Either<exn, int>.Left(exn(s))
            // fmap id == id
            either.FMap(funcId) = (either :> Either<exn, int>)
        )

        let ``Composition in Right<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let either = Either<exn, int>.Right(i)
            // fmap (f . g) == fmap f . fmap g
            either.FMap(Composition.Compose(g, f)) = either.FMap(f).FMap(g)
        )

        let ``Composition in Left<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.string.NonNull)(fun f g s ->
            let either = Either<exn, int>.Left(exn(s))
            // fmap (f . g) == fmap f . fmap g
            either.FMap(Composition.Compose(g, f)) = either.FMap(f).FMap(g)
        )

        let ``Functor laws`` = property {
            apply ``Identity in Right<TLeft, TRight>``
            apply ``Identity in Left<TLeft, TRight>``
            apply ``Composition in Right<TLeft, TRight>``
            apply ``Composition in Left<TLeft, TRight>``
        }

    module FunctorLawsInFuncyList =
        let ``Identity in Cons<T>`` = Prop.forAll(Arb.array(Arb.int).NonNull)(fun a ->
            let fList = FuncyList.Construct(a)
            // fmap id == id
            fList.FMap(funcId) = fList
        )

        let ``Identity in Nil<T>`` = Prop.forAll(Arb.int)(fun i ->
            let fList = FuncyList<int>.Nil()
            // fmap id == id
            fList.FMap(funcId) = (fList :> FuncyList<int>)
        )

        let ``Composition in Cons<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let fList = FuncyList.Cons(i, FuncyList.Nil())
            // fmap (f . g) == fmap f . fmap g
            fList.FMap(Composition.Compose(g, f)) = fList.FMap(f).FMap(g)
        )

        let ``Composition in Nil<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int))(fun f g ->
            let fList = FuncyList<int>.Nil()
            // fmap (f . g) == fmap f . fmap g
            fList.FMap(Composition.Compose(g, f)) = fList.FMap(f).FMap(g)
        )

        let ``Functor laws`` = property {
            apply ``Identity in Cons<T>``
            apply ``Identity in Nil<T>``
            apply ``Composition in Cons<T>``
            apply ``Composition in Nil<T>``
        }