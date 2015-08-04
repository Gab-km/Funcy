namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open Persimmon.Dried
open UseTestNameByReflection

module ApplicativeLawsCheck =
    let funcId<'T> = Func<'T, 'T>(id)
    let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)   // for implicit conversion in F#

    module ApplicativeLawsInMaybe =
        let pureMaybe = Maybe.Some
        
        let ``Identity in Some<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = Maybe.Some(i)
            // pure id <*> v = v
            v.Apply(pureMaybe funcId) = (v :> IMaybe<int>)
        )

        let ``Identity in None<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = Maybe<int>.None()
            // pure id <*> v = v
            v.Apply(pureMaybe funcId) = (v :> IMaybe<int>)
        )

        let ``Composition in Maybe<T> 1`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f g i ->
            let u = Maybe.Some(f)
            let v = Maybe.Some(g)
            let w = Maybe.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) :> IApplicative<int> = w.Apply(v).Apply(u)
        )

        let ``Composition in Maybe<T> 2`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun g i ->
            let u = Maybe.None()
            let v = Maybe.Some(g)
            let w = Maybe.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) :> IApplicative<int> = w.Apply(v).Apply(u)
        )

        let ``Composition in Maybe<T> 3`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f i ->
            let u = Maybe.Some(f)
            let v = Maybe.None()
            let w = Maybe.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) :> IApplicative<int> = w.Apply(v).Apply(u)
        )

        let ``Composition in Maybe<T> 4`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int))(fun f g ->
            let u = Maybe.Some(f)
            let v = Maybe.Some(g)
            let w = Maybe.None()
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) :> IApplicative<int> = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in Maybe<T>`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureMaybe x).Apply(pureMaybe f) = (pureMaybe(f.Invoke(x)) :> IMaybe<int>)
        )

        let ``Interchange in Maybe<T>`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f y ->
            // u <*> pure y = pure ($ y) <*> u
            let u = Maybe.Some(f)
            (pureMaybe y).Apply(u) = u.Apply(pureMaybe <| Func<Func<int, int>, int>(fun f_ -> f_.Invoke(y)))
        )

        let ``Applicative laws`` = property {
            apply ``Identity in Some<T>``
            apply ``Identity in None<T>``
            apply ``Composition in Maybe<T> 1``
            apply ``Composition in Maybe<T> 2``
            apply ``Composition in Maybe<T> 3``
            apply ``Composition in Maybe<T> 4``
            apply ``Homomorphism in Maybe<T>``
            apply ``Interchange in Maybe<T>``
        }

    module ApplicativeLawsInEither =
        let pureEither<'TLeft, 'TRight> = Either<'TLeft, 'TRight>.Right
        
        let ``Identity in Right<TLeft, TRight>`` = Prop.forAll(Arb.int)(fun i ->
            let v = Either.Right(i)
            // pure id <*> v = v
            v.Apply(pureEither funcId) = (v :> IEither<exn, int>)
        )
        
        let ``Identity in Left<TLeft, TRight>`` = Prop.forAll(Arb.string)(fun s ->
            let v = Either.Left(exn(s))
            // pure id <*> v = v
            v.Apply(pureEither funcId) = (v :> IEither<exn, int>)
        )

        let ``Composition in Either<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f g i ->
            let u = Either.Right(f)
            let v = Either.Right(g)
            let w = Either.Right(i)
            let pointed = pureEither <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) :> IApplicative<int> = w.Apply(v).Apply(u)
        )

        let ``Composition in Either<TLeft, TRight> 2`` = Prop.forAll(Arb.string, Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun s g i ->
            let u = Either.Left(exn(s))
            let v = Either.Right(g)
            let w = Either.Right(i)
            let pointed = pureEither <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) :> IApplicative<int> = w.Apply(v).Apply(u)
        )

        let ``Composition in Either<TLeft, TRight> 3`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.string, Arb.int)(fun f s i ->
            let u = Either.Right(f)
            let v = Either.Left(exn(s))
            let w = Either.Right(i)
            let pointed = pureEither <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) :> IApplicative<int> = w.Apply(v).Apply(u)
        )

        let ``Composition in Either<TLeft, TRight> 4`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.string)(fun f g s ->
            let u = Either.Right(f)
            let v = Either.Right(g)
            let w = Either.Left(exn(s))
            let pointed = pureEither <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) :> IApplicative<int> = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in Either<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureEither x).Apply(pureEither f) = (pureEither(f.Invoke(x)) :> IEither<exn, int>)
        )

        let ``Interchange in Either<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f y ->
            // u <*> pure y = pure ($ y) <*> u
            let u = Either.Right(f)
            (pureEither y).Apply(u) = u.Apply(pureEither <| Func<Func<int, int>, int>(fun f_ -> f_.Invoke(y)))
        )

        let ``Applicative laws`` = property {
            apply ``Identity in Right<TLeft, TRight>``
            apply ``Identity in Left<TLeft, TRight>``
            apply ``Composition in Either<TLeft, TRight> 1``
            apply ``Composition in Either<TLeft, TRight> 2``
            apply ``Composition in Either<TLeft, TRight> 3``
            apply ``Composition in Either<TLeft, TRight> 4``
            apply ``Homomorphism in Either<TLeft, TRight>``
            apply ``Interchange in Either<TLeft, TRight>``
        }
