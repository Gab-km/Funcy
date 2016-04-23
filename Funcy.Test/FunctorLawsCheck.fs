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
            maybe.FMap(funcId) = maybe
        )

        let ``Identity in None<T>`` = Prop.forAll(Arb.int)(fun i ->
            let maybe = Maybe<int>.None()
            // fmap id == id
            maybe.FMap(funcId) = maybe
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

    module FunctorLawsInMaybeTC =
        let ``Identity in SomeTC<T>`` = Prop.forAll(Arb.int)(fun i ->
            let maybe = MaybeTC.Some(i)
            // fmap id == id
            maybe.FMap(funcId) = maybe
        )

        let ``Identity in NoneTC<T>`` = Prop.forAll(Arb.int)(fun i ->
            let maybe = MaybeTC.None<int>()
            // fmap id == id
            maybe.FMap(funcId) = maybe
        )

        let ``Composition in SomeTC<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let maybe = MaybeTC.Some(i)
            // fmap (f . g) == fmap f . fmap g
            maybe.FMap(Composition.Compose(g, f)) = maybe.FMap(f).FMap(g)
        )

        let ``Composition in NoneTC<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int))(fun f g ->
            let maybe = MaybeTC.None<int>()
            // fmap (f . g) == fmap f . fmap g
            maybe.FMap(Composition.Compose(g, f)) = maybe.FMap(f).FMap(g)
        )

        let ``Functor laws`` = property {
            apply ``Identity in SomeTC<T>``
            apply ``Identity in NoneTC<T>``
            apply ``Composition in SomeTC<T>``
            apply ``Composition in NoneTC<T>``
        }
        
    module FunctorLawsInEither =
        let ``Identity in Right<TLeft, TRight>`` = Prop.forAll(Arb.int)(fun i ->
            let either = Either<exn, int>.Right(i)
            // fmap id == id
            either.FMap(funcId) = either
        )

        let ``Identity in Left<TLeft, TRight>`` = Prop.forAll(Arb.string.NonNull)(fun s ->
            let either = Either<exn, int>.Left(exn(s))
            // fmap id == id
            either.FMap(funcId) = either
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

    module FunctorLawsInEitherTC =
        let ``Identity in RightTC<TLeft, TRight>`` = Prop.forAll(Arb.int)(fun i ->
            let either = EitherTC<exn>.Right(i)
            // fmap id == id
            either.FMap(funcId) = either
        )

        let ``Identity in LeftTC<TLeft, TRight>`` = Prop.forAll(Arb.string.NonNull)(fun s ->
            let either = EitherTC<exn>.Left<int>(exn(s))
            // fmap id == id
            either.FMap(funcId) = either
        )

        let ``Composition in RightTC<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let either = EitherTC<exn>.Right(i)
            // fmap (f . g) == fmap f . fmap g
            either.FMap(Composition.Compose(g, f)) = either.FMap(f).FMap(g)
        )

        let ``Composition in LeftTC<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.string.NonNull)(fun f g s ->
            let either = EitherTC<exn>.Left<int>(exn(s))
            // fmap (f . g) == fmap f . fmap g
            either.FMap(Composition.Compose(g, f)) = either.FMap(f).FMap(g)
        )

        let ``Functor laws`` = property {
            apply ``Identity in RightTC<TLeft, TRight>``
            apply ``Identity in LeftTC<TLeft, TRight>``
            apply ``Composition in RightTC<TLeft, TRight>``
            apply ``Composition in LeftTC<TLeft, TRight>``
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
            fList.FMap(funcId) = fList
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

    module FunctorLawsInNonEmptyList =
        let ``Identity in NonEmptyList<T>`` = Prop.forAll(Arb.nonEmpty(Arb.list Arb.int))(fun ls ->
            let nel = NonEmptyList.Construct(ls)
            // fmap id == id
            nel.FMap(funcId) = nel
        )
        
        let ``Identity in ConsNEL<T>`` = Prop.forAll(Arb.int, Arb.int)(fun x y ->
            let consNEL = NonEmptyList.ConsNEL(x, NonEmptyList.Singleton(y))
            // fmap id == id
            consNEL.FMap(funcId) = consNEL
        )

        let ``Identity in Singleton<T>`` = Prop.forAll(Arb.int)(fun i ->
            let singleton = NonEmptyList<int>.Singleton(i)
            // fmap id == id
            singleton.FMap(funcId) = singleton
        )

        let ``Composition in NonEmptyList<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.nonEmpty(Arb.list Arb.int))(fun f g ls ->
            let nel = NonEmptyList.Construct(ls)
            // fmap (f . g) == fmap f . fmap g
            nel.FMap(Composition.Compose(g, f)) = nel.FMap(f).FMap(g)
        )

        let ``Composition in ConsNEL<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int, Arb.int)(fun f g i j ->
            let consNEL = NonEmptyList.ConsNEL(i, NonEmptyList.Singleton(j))
            // fmap (f . g) == fmap f . fmap g
            consNEL.FMap(Composition.Compose(g, f)) = consNEL.FMap(f).FMap(g)
        )

        let ``Composition in Singleton<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let singleton = NonEmptyList.Singleton(i)
            // fmap (f . g) == fmap f . fmap g
            singleton.FMap(Composition.Compose(g, f)) = singleton.FMap(f).FMap(g)
        )

        let ``Functor laws`` = property {
            apply ``Identity in NonEmptyList<T>``
            apply ``Identity in ConsNEL<T>``
            apply ``Identity in Singleton<T>``
            apply ``Composition in NonEmptyList<T>``
            apply ``Composition in ConsNEL<T>``
            apply ``Composition in Singleton<T>``
        }
