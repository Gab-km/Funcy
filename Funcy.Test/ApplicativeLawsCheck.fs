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
            v.Apply(pureMaybe funcId) = v
        )

        let ``Identity in None<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = Maybe<int>.None()
            // pure id <*> v = v
            v.Apply(pureMaybe funcId) = v
        )

        let ``Composition in Maybe<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let u = Maybe.Some(f)
            let v = Maybe.Some(g)
            let w = Maybe.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in Maybe<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun g i ->
            let u = Maybe.None()
            let v = Maybe.Some(g)
            let w = Maybe.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in Maybe<T> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f i ->
            let u = Maybe.Some(f)
            let v = Maybe.None()
            let w = Maybe.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in Maybe<T> 4`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int))(fun f g ->
            let u = Maybe.Some(f)
            let v = Maybe.Some(g)
            let w = Maybe.None()
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in Maybe<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureMaybe x).Apply(pureMaybe f) = pureMaybe(f.Invoke(x))
        )

        let ``Interchange in Maybe<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f y ->
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

    module ApplicativeLawsInMaybeTC =
        open Funcy.Future
        let pureMaybe = MaybeTC.Some
        
        let ``Identity in SomeTC<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = MaybeTC.Some(i)
            // pure id <*> v = v
            v.Apply(pureMaybe funcId) = v
        )

        let ``Identity in NoneTC<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = MaybeTC.None<int>()
            // pure id <*> v = v
            v.Apply(pureMaybe funcId) = v
        )

        let ``Composition in MaybeTC<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let u = MaybeTC.Some(f)
            let v = MaybeTC.Some(g)
            let w = MaybeTC.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in MaybeTC<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun g i ->
            let u = MaybeTC.None()
            let v = MaybeTC.Some(g)
            let w = MaybeTC.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in MaybeTC<T> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f i ->
            let u = MaybeTC.Some(f)
            let v = MaybeTC.None()
            let w = MaybeTC.Some(i)
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in MaybeTC<T> 4`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int))(fun f g ->
            let u = MaybeTC.Some(f)
            let v = MaybeTC.Some(g)
            let w = MaybeTC.None()
            let pointed = pureMaybe <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in MaybeTC<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureMaybe x).Apply(pureMaybe f) = pureMaybe(f.Invoke(x))
        )

        let ``Interchange in MaybeTC<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f y ->
            // u <*> pure y = pure ($ y) <*> u
            let u = MaybeTC.Some(f)
            (pureMaybe y).Apply(u) = u.Apply(pureMaybe <| Func<Func<int, int>, int>(fun f_ -> f_.Invoke(y)))
        )

        let ``Applicative laws`` = property {
            apply ``Identity in SomeTC<T>``
            apply ``Identity in NoneTC<T>``
            apply ``Composition in MaybeTC<T> 1``
            apply ``Composition in MaybeTC<T> 2``
            apply ``Composition in MaybeTC<T> 3``
            apply ``Composition in MaybeTC<T> 4``
            apply ``Homomorphism in MaybeTC<T>``
            apply ``Interchange in MaybeTC<T>``
        }

    module ApplicativeLawsInEither =
        let pureEither<'TLeft, 'TRight> = Either<'TLeft, 'TRight>.Right
        
        let ``Identity in Right<TLeft, TRight>`` = Prop.forAll(Arb.int)(fun i ->
            let v = Either.Right(i)
            // pure id <*> v = v
            v.Apply(pureEither funcId) = v
        )
        
        let ``Identity in Left<TLeft, TRight>`` = Prop.forAll(Arb.string.NonNull)(fun s ->
            let v = Either.Left(exn(s))
            // pure id <*> v = v
            v.Apply(pureEither funcId) = v
        )

        let ``Composition in Either<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let u = Either.Right(f)
            let v = Either.Right(g)
            let w = Either.Right(i)
            let pointed = pureEither <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in Either<TLeft, TRight> 2`` = Prop.forAll(Arb.string.NonNull, Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun s g i ->
            let u = Either.Left(exn(s))
            let v = Either.Right(g)
            let w = Either.Right(i)
            let pointed = pureEither <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in Either<TLeft, TRight> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.string.NonNull, Arb.int)(fun f s i ->
            let u = Either.Right(f)
            let v = Either.Left(exn(s))
            let w = Either.Right(i)
            let pointed = pureEither <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in Either<TLeft, TRight> 4`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.string)(fun f g s ->
            let u = Either.Right(f)
            let v = Either.Right(g)
            let w = Either.Left(exn(s))
            let pointed = pureEither <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in Either<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureEither x).Apply(pureEither f) = (pureEither(f.Invoke(x)) :> Either<exn, int>)
        )

        let ``Interchange in Either<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f y ->
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

    module ApplicativeLawsInEitherTC =
        open Funcy.Future
        let pureEitherTC<'TLeft, 'TRight> x = EitherTC<'TLeft>.Right<'TRight>(x)
        
        let ``Identity in RightTC<TLeft, TRight>`` = Prop.forAll(Arb.int)(fun i ->
            let v = EitherTC<string>.Right(i)
            // pure id <*> v = v
            v.Apply(pureEitherTC funcId) = v
        )
        
        let ``Identity in LeftTC<TLeft, TRight>`` = Prop.forAll(Arb.string.NonNull)(fun s ->
            let v = EitherTC<exn>.Left<int>(exn(s))
            // pure id <*> v = v
            v.Apply(pureEitherTC funcId) = v
        )

        let ``Composition in EitherTC<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let u = EitherTC<string>.Right(f)
            let v = EitherTC<string>.Right(g)
            let w = EitherTC<string>.Right(i)
            let pointed = pureEitherTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in EitherTC<TLeft, TRight> 2`` = Prop.forAll(Arb.string.NonNull, Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun s g i ->
            let u = EitherTC<exn>.Left(exn(s))
            let v = EitherTC<exn>.Right(g)
            let w = EitherTC<exn>.Right(i)
            let pointed = pureEitherTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in EitherTC<TLeft, TRight> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.string.NonNull, Arb.int)(fun f s i ->
            let u = EitherTC<exn>.Right(f)
            let v = EitherTC<exn>.Left(exn(s))
            let w = EitherTC<exn>.Right(i)
            let pointed = pureEitherTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in EitherTC<TLeft, TRight> 4`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.string)(fun f g s ->
            let u = EitherTC<exn>.Right(f)
            let v = EitherTC<exn>.Right(g)
            let w = EitherTC<exn>.Left(exn(s))
            let pointed = pureEitherTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in EitherTC<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureEitherTC x).Apply(pureEitherTC f) = pureEitherTC(f.Invoke(x))
        )

        let ``Interchange in EitherTC<TLeft, TRight>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f y ->
            // u <*> pure y = pure ($ y) <*> u
            let u = EitherTC<string>.Right(f)
            (pureEitherTC y).Apply(u) = u.Apply(pureEitherTC <| Func<Func<int, int>, int>(fun f_ -> f_.Invoke(y)))
        )

        let ``Applicative laws`` = property {
            apply ``Identity in RightTC<TLeft, TRight>``
            apply ``Identity in LeftTC<TLeft, TRight>``
            apply ``Composition in EitherTC<TLeft, TRight> 1``
            apply ``Composition in EitherTC<TLeft, TRight> 2``
            apply ``Composition in EitherTC<TLeft, TRight> 3``
            apply ``Composition in EitherTC<TLeft, TRight> 4``
            apply ``Homomorphism in EitherTC<TLeft, TRight>``
            apply ``Interchange in EitherTC<TLeft, TRight>``
        }

    module ApplicativeLawsInFuncyList =
        let pureFList x = FuncyList.Construct([|x|])
        
        let ``Identity in Cons<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = FuncyList.Cons(i, FuncyList.Nil())
            // pure id <*> v = v
            v.Apply(pureFList funcId) = v
        )

        let ``Identity in Nil<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = FuncyList<int>.Nil()
            // pure id <*> v = v
            v.Apply(pureFList funcId) = v
        )

        let ``Composition in FuncyList<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let u = FuncyList.Cons(f, FuncyList.Nil())
            let v = FuncyList.Construct([|g|])
            let w = FuncyList.Cons(i, FuncyList.Nil())
            let pointed = pureFList <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in FuncyList<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.array(Arb.int).NonNull)(fun g a ->
            let u = FuncyList.Nil()
            let v = FuncyList.Cons(g, FuncyList.Nil())
            let w = FuncyList.Construct(a)
            let pointed = pureFList <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in FuncyList<T> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f i ->
            let u = FuncyList.Cons(f, FuncyList.Nil())
            let v = FuncyList.Nil()
            let w = FuncyList.Cons(i, FuncyList.Nil())
            let pointed = pureFList <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in FuncyList<T> 4`` = Prop.forAll(Arb.array(Arb.systemFunc(CoArb.int, Arb.int)).NonNull, Arb.systemFunc(CoArb.int, Arb.int))(fun fs g ->
            let u = FuncyList.Construct(fs)
            let v = FuncyList.Construct([|g|])
            let w = FuncyList.Nil()
            let pointed = pureFList <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in FuncyList<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureFList x).Apply(pureFList f) = pureFList(f.Invoke(x))
        )

        let ``Interchange in FuncyList<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f y ->
            // u <*> pure y = pure ($ y) <*> u
            let u = FuncyList.Cons(f, FuncyList.Nil())
            (pureFList y).Apply(u) = u.Apply(pureFList <| Func<Func<int, int>, int>(fun f_ -> f_.Invoke(y)))
        )

        let ``Applicative laws`` = property {
            apply ``Identity in Cons<T>``
            apply ``Identity in Nil<T>``
            apply ``Composition in FuncyList<T> 1``
            apply ``Composition in FuncyList<T> 2``
            apply ``Composition in FuncyList<T> 3``
            apply ``Composition in FuncyList<T> 4``
            apply ``Homomorphism in FuncyList<T>``
            apply ``Interchange in FuncyList<T>``
        }

    module ApplicativeLawsInFuncyListTC =
        open Funcy.Future

        let pureFListTC x = FuncyListTC.Construct([|x|])
        
        let ``Identity in ConsTC<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = FuncyListTC.Cons(i, FuncyListTC.Nil())
            // pure id <*> v = v
            v.Apply(pureFListTC funcId) = v
        )

        let ``Identity in NilTC<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = FuncyListTC.Nil<int>()
            // pure id <*> v = v
            v.Apply(pureFListTC funcId) = v
        )

        let ``Composition in FuncyListTC<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let u = FuncyListTC.Cons(f, FuncyListTC.Nil())
            let v = FuncyListTC.Construct([|g|])
            let w = FuncyListTC.Cons(i, FuncyListTC.Nil())
            let pointed = pureFListTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in FuncyListTC<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.array(Arb.int).NonNull)(fun g a ->
            let u = FuncyListTC.Nil()
            let v = FuncyListTC.Cons(g, FuncyListTC.Nil())
            let w = FuncyListTC.Construct(a)
            let pointed = pureFListTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in FuncyListTC<T> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f i ->
            let u = FuncyListTC.Cons(f, FuncyListTC.Nil())
            let v = FuncyListTC.Nil()
            let w = FuncyListTC.Cons(i, FuncyListTC.Nil())
            let pointed = pureFListTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in FuncyListTC<T> 4`` = Prop.forAll(Arb.array(Arb.systemFunc(CoArb.int, Arb.int)).NonNull, Arb.systemFunc(CoArb.int, Arb.int))(fun fs g ->
            let u = FuncyListTC.Construct(fs)
            let v = FuncyListTC.Construct([|g|])
            let w = FuncyListTC.Nil()
            let pointed = pureFListTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in FuncyListTC<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureFListTC x).Apply(pureFListTC f) = pureFListTC(f.Invoke(x))
        )

        let ``Interchange in FuncyListTC<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f y ->
            // u <*> pure y = pure ($ y) <*> u
            let u = FuncyListTC.Cons(f, FuncyListTC.Nil())
            (pureFListTC y).Apply(u) = u.Apply(pureFListTC <| Func<Func<int, int>, int>(fun f_ -> f_.Invoke(y)))
        )

        let ``Applicative laws`` = property {
            apply ``Identity in ConsTC<T>``
            apply ``Identity in NilTC<T>``
            apply ``Composition in FuncyListTC<T> 1``
            apply ``Composition in FuncyListTC<T> 2``
            apply ``Composition in FuncyListTC<T> 3``
            apply ``Composition in FuncyListTC<T> 4``
            apply ``Homomorphism in FuncyListTC<T>``
            apply ``Interchange in FuncyListTC<T>``
        }

    module ApplicativeLawsInNonEmptyList =
        let pureNEL x = NonEmptyList.Construct([x])
        
        let ``Identity in NonEmptyList<T>`` = Prop.forAll(Arb.nonEmpty(Arb.list Arb.int))(fun ls ->
            let v = NonEmptyList.Construct(ls)
            // pure id <*> v = v
            v.Apply(pureNEL funcId) = v
        )
        
        let ``Identity in ConsNEL<T>`` = Prop.forAll(Arb.int, Arb.int)(fun i j ->
            let v = NonEmptyList.ConsNEL(i, NonEmptyList.Singleton(j))
            // pure id <*> v = v
            v.Apply(pureNEL funcId) = v
        )

        let ``Identity in Singleton<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = NonEmptyList.Singleton(i)
            // pure id <*> v = v
            v.Apply(pureNEL funcId) = v
        )

        let ``Composition in NonEmptyList<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.nonEmpty(Arb.list <| Arb.systemFunc(CoArb.int, Arb.int)), Arb.int, Arb.int)(fun f gs i j ->
            let u = NonEmptyList.Singleton(f)
            let v = NonEmptyList.Construct(gs)
            let w = NonEmptyList.ConsNEL(i, NonEmptyList.Singleton(j))
            let pointed = pureNEL <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )
        
        let ``Composition in NonEmptyList<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.nonEmpty(Arb.list Arb.int))(fun f g h ls ->
            let u = NonEmptyList.ConsNEL(f, NonEmptyList.Singleton(g))
            let v = NonEmptyList.Singleton(h)
            let w = NonEmptyList.Construct(ls)
            let pointed = pureNEL <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in NonEmptyList<T> 3`` = Prop.forAll(Arb.nonEmpty(Arb.list <| Arb.systemFunc(CoArb.int, Arb.int)), Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun fs g h i ->
            let u = NonEmptyList.Construct(fs)
            let v = NonEmptyList.ConsNEL(g, NonEmptyList.Singleton(h))
            let w = NonEmptyList.Singleton(i)
            let pointed = pureNEL <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let arbFs = {
          Gen = Gen.listOfLength 3 <| Arb.systemFunc(CoArb.int, Arb.int).Gen
          Shrinker = Shrink.shrinkList <| Arb.systemFunc(CoArb.int, Arb.int).Shrinker
          PrettyPrinter = Pretty.prettyList
        }

        let arbLs = {
          Gen = Gen.listOfLength 3 <| Arb.int.Gen
          Shrinker = Shrink.shrinkList <| Arb.int.Shrinker
          PrettyPrinter = Pretty.prettyList
        }

        let ``Composition in NonEmptyList<T> 4`` = Prop.forAll(arbFs, arbFs, arbLs)(fun fs gs ls ->
            let u = NonEmptyList.Construct(fs)
            let v = NonEmptyList.Construct(gs)
            let w = NonEmptyList.Construct(ls)
            let pointed = pureNEL <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in NonEmptyList<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureNEL x).Apply(pureNEL f) = pureNEL(f.Invoke(x))
        )

        let ``Interchange in NonEmptyList<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g y ->
            // u <*> pure y = pure ($ y) <*> u
            let u = NonEmptyList.ConsNEL(f, NonEmptyList.Singleton(g))
            (pureNEL y).Apply(u) = u.Apply(pureNEL <| Func<Func<int, int>, int>(fun f_ -> f_.Invoke(y)))
        )

        let ``Applicative laws`` = property {
            apply ``Identity in NonEmptyList<T>``
            apply ``Identity in ConsNEL<T>``
            apply ``Identity in Singleton<T>``
            apply ``Composition in NonEmptyList<T> 1``
            apply ``Composition in NonEmptyList<T> 2``
            apply ``Composition in NonEmptyList<T> 3``
            apply ``Composition in NonEmptyList<T> 4``
            apply ``Homomorphism in NonEmptyList<T>``
            apply ``Interchange in NonEmptyList<T>``
        }

    module ApplicativeLawsInNonEmptyListTC =
        open Funcy.Future

        let pureNELTC x = NonEmptyListTC.Construct([x])
        
        let ``Identity in NonEmptyListTC<T>`` = Prop.forAll(Arb.nonEmpty(Arb.list Arb.int))(fun ls ->
            let v = NonEmptyListTC.Construct(ls)
            // pure id <*> v = v
            v.Apply(pureNELTC funcId) = v
        )
        
        let ``Identity in ConsNELTC<T>`` = Prop.forAll(Arb.int, Arb.int)(fun i j ->
            let v = NonEmptyListTC.ConsNEL(i, NonEmptyListTC.Singleton(j))
            // pure id <*> v = v
            v.Apply(pureNELTC funcId) = v
        )

        let ``Identity in SingletonTC<T>`` = Prop.forAll(Arb.int)(fun i ->
            let v = NonEmptyListTC.Singleton(i)
            // pure id <*> v = v
            v.Apply(pureNELTC funcId) = v
        )

        let ``Composition in NonEmptyListTC<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.nonEmpty(Arb.list <| Arb.systemFunc(CoArb.int, Arb.int)), Arb.int, Arb.int)(fun f gs i j ->
            let u = NonEmptyListTC.Singleton(f)
            let v = NonEmptyListTC.Construct(gs)
            let w = NonEmptyListTC.ConsNEL(i, NonEmptyListTC.Singleton(j))
            let pointed = pureNELTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )
        
        let ``Composition in NonEmptyListTC<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.nonEmpty(Arb.list Arb.int))(fun f g h ls ->
            let u = NonEmptyListTC.ConsNEL(f, NonEmptyListTC.Singleton(g))
            let v = NonEmptyListTC.Singleton(h)
            let w = NonEmptyListTC.Construct(ls)
            let pointed = pureNELTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Composition in NonEmptyListTC<T> 3`` = Prop.forAll(Arb.nonEmpty(Arb.list <| Arb.systemFunc(CoArb.int, Arb.int)), Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun fs g h i ->
            let u = NonEmptyListTC.Construct(fs)
            let v = NonEmptyListTC.ConsNEL(g, NonEmptyListTC.Singleton(h))
            let w = NonEmptyListTC.Singleton(i)
            let pointed = pureNELTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let arbFs = {
          Gen = Gen.listOfLength 3 <| Arb.systemFunc(CoArb.int, Arb.int).Gen
          Shrinker = Shrink.shrinkList <| Arb.systemFunc(CoArb.int, Arb.int).Shrinker
          PrettyPrinter = Pretty.prettyList
        }

        let arbLs = {
          Gen = Gen.listOfLength 3 <| Arb.int.Gen
          Shrinker = Shrink.shrinkList <| Arb.int.Shrinker
          PrettyPrinter = Pretty.prettyList
        }

        let ``Composition in NonEmptyListTC<T> 4`` = Prop.forAll(arbFs, arbFs, arbLs)(fun fs gs ls ->
            let u = NonEmptyListTC.Construct(fs)
            let v = NonEmptyListTC.Construct(gs)
            let w = NonEmptyListTC.Construct(ls)
            let pointed = pureNELTC <|
                            (!> Currying.Curry(Func<Func<int, int>, Func<int, int>, Func<int, int>>(fun f_ g_ -> Composition.Compose(f_, g_))))
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
            w.Apply(v.Apply(u.Apply(pointed))) = w.Apply(v).Apply(u)
        )

        let ``Homomorphism in NonEmptyListTC<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f x ->
            // pure f <*> pure x = pure (f x)
            (pureNELTC x).Apply(pureNELTC f) = pureNELTC(f.Invoke(x))
        )

        let ``Interchange in NonEmptyListTC<T>`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g y ->
            // u <*> pure y = pure ($ y) <*> u
            let u = NonEmptyListTC.ConsNEL(f, NonEmptyListTC.Singleton(g))
            (pureNELTC y).Apply(u) = u.Apply(pureNELTC <| Func<Func<int, int>, int>(fun f_ -> f_.Invoke(y)))
        )

        let ``Applicative laws`` = property {
            apply ``Identity in NonEmptyListTC<T>``
            apply ``Identity in ConsNELTC<T>``
            apply ``Identity in SingletonTC<T>``
            apply ``Composition in NonEmptyListTC<T> 1``
            apply ``Composition in NonEmptyListTC<T> 2``
            apply ``Composition in NonEmptyListTC<T> 3``
            apply ``Composition in NonEmptyListTC<T> 4``
            apply ``Homomorphism in NonEmptyListTC<T>``
            apply ``Interchange in NonEmptyListTC<T>``
        }
