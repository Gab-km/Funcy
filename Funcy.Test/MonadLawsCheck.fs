namespace Funcy.Test

open System
open Funcy
open Funcy.Computations
open Persimmon
open Persimmon.Dried
open UseTestNameByReflection

module MonadLawsCheck =
    module MonadLawsInMaybe =
        let returnMaybe<'T> x = (Maybe.Some(x) :> Maybe<'T>)

        let ``Left identity in Maybe<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f a ->
            let f_ = Func<int, Maybe<int>>(fun x -> Maybe.Some(f.Invoke(x)) :> Maybe<int>)
            // return a >>= f ≡ f a
            (returnMaybe a).ComputeWith(f_) = f_.Invoke(a)
        )
        
        let ``Left identity in Maybe<T> 2`` = Prop.forAll(Arb.int)(fun a ->
            let f_ = Func<int, Maybe<int>>(fun _ -> Maybe.None() :> Maybe<int>)
            // return a >>= f ≡ f a
            (returnMaybe a).ComputeWith(f_) = f_.Invoke(a)
        )

        let ``Right identity in Maybe<T> 1`` = Prop.forAll(Arb.int)(fun i ->
            let m = Maybe.Some(i)
            // m >>= return ≡ m
            m.ComputeWith(Func<int, Maybe<int>>(fun x -> returnMaybe x)) = (m :> Maybe<int>)
        )

        let ``Right identity in Maybe<T> 2`` = Prop.forAll(Arb.int)(fun _ ->
            let m = Maybe.None()
            // m >>= return ≡ m
            m.ComputeWith(Func<int, Maybe<int>>(fun x -> returnMaybe x)) = (m :> Maybe<int>)
        )

        let ``Associativity in Maybe<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let m = Maybe.Some(i)
            let f_ = Func<int, Maybe<int>>(fun x -> Maybe.Some(f.Invoke(x)) :> Maybe<int>)
            let g_ = Func<int, Maybe<int>>(fun x -> Maybe.Some(g.Invoke(x)) :> Maybe<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Maybe<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int))(fun f g ->
            let m = Maybe.None()
            let f_ = Func<int, Maybe<int>>(fun x -> Maybe.Some(f.Invoke(x)) :> Maybe<int>)
            let g_ = Func<int, Maybe<int>>(fun x -> Maybe.Some(g.Invoke(x)) :> Maybe<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Maybe<T> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun g i ->
            let m = Maybe.Some(i)
            let f_ = Func<int, Maybe<int>>(fun _ -> Maybe.None() :> Maybe<int>)
            let g_ = Func<int, Maybe<int>>(fun x -> Maybe.Some(g.Invoke(x)) :> Maybe<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Maybe<T> 4`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f i ->
            let m = Maybe.Some(i)
            let f_ = Func<int, Maybe<int>>(fun x -> Maybe.Some(f.Invoke(x)) :> Maybe<int>)
            let g_ = Func<int, Maybe<int>>(fun _ -> Maybe.None() :> Maybe<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Monad laws`` = property {
            apply ``Left identity in Maybe<T> 1``
            apply ``Left identity in Maybe<T> 2``
            apply ``Right identity in Maybe<T> 1``
            apply ``Right identity in Maybe<T> 2``
            apply ``Associativity in Maybe<T> 1``
            apply ``Associativity in Maybe<T> 2``
            apply ``Associativity in Maybe<T> 3``
            apply ``Associativity in Maybe<T> 4``
        }

    module MonadLawsInEither =
        let returnEither<'TLeft, 'TRight> x = (Either<'TLeft, 'TRight>.Right(x) :> Either<'TLeft, 'TRight>)

        let ``Left identity in Either<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f a ->
            let f_ = Func<int, Either<exn, int>>(fun x -> Either<exn, int>.Right(f.Invoke(x)) :> Either<exn, int>)
            // return a >>= f ≡ f a
            (returnEither a).ComputeWith(f_) = f_.Invoke(a)
        )
        
        let ``Left identity in Either<TLeft, TRight> 2`` = Prop.forAll(Arb.string, Arb.int)(fun s a ->
            let e = exn(s)
            let f_ = Func<int, Either<exn, int>>(fun _ -> Either<exn, int>.Left(e) :> Either<exn, int>)
            // return a >>= f ≡ f a
            (returnEither a).ComputeWith(f_) = f_.Invoke(a)
        )

        let ``Right identity in Either<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f i ->
            let m = Either<exn, int>.Right(i)
            // m >>= return ≡ m
            m.ComputeWith(Func<int, Either<exn, int>>(fun x -> returnEither x)) = (m :> Either<exn, int>)
        )

        let ``Right identity in Either<TLeft, TRight> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.string.NonNull)(fun f s ->
            let m = Either<exn, int>.Left(exn(s))
            // m >>= return ≡ m
            m.ComputeWith(Func<int, Either<exn, int>>(fun x -> returnEither x)) = (m :> Either<exn, int>)
        )

        let ``Associativity in Either<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f g i ->
            let m = Either<exn, int>.Right(i)
            let f_ = Func<int, Either<exn, int>>(fun x -> Either<exn, int>.Right(f.Invoke(x)) :> Either<exn, int>)
            let g_ = Func<int, Either<exn, int>>(fun x -> Either<exn, int>.Right(g.Invoke(x)) :> Either<exn, int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Either<TLeft, TRight> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.string.NonNull)(fun f g s ->
            let m = Either<exn, int>.Left(exn(s))
            let f_ = Func<int, Either<exn, int>>(fun x -> Either<exn, int>.Right(f.Invoke(x)) :> Either<exn, int>)
            let g_ = Func<int, Either<exn, int>>(fun x -> Either<exn, int>.Right(g.Invoke(x)) :> Either<exn, int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Either<TLeft, TRight> 3`` = Prop.forAll(Arb.string, Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun s g i ->
            let m = Either<exn, int>.Right(i)
            let e = exn(s)
            let f_ = Func<int, Either<exn, int>>(fun _ -> Either<exn, int>.Left(e) :> Either<exn, int>)
            let g_ = Func<int, Either<exn, int>>(fun x -> Either<exn, int>.Right(g.Invoke(x)) :> Either<exn, int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Either<TLeft, TRight> 4`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.string.NonNull, Arb.int)(fun f s i ->
            let m = Either<exn, int>.Right(i)
            let f_ = Func<int, Either<exn, int>>(fun x -> Either<exn, int>.Right(f.Invoke(x)) :> Either<exn, int>)
            let e = exn(s)
            let g_ = Func<int, Either<exn, int>>(fun _ -> Either<exn, int>.Left(e) :> Either<exn, int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Monad laws`` = property {
            apply ``Left identity in Either<TLeft, TRight> 1``
            apply ``Left identity in Either<TLeft, TRight> 2``
            apply ``Right identity in Either<TLeft, TRight> 1``
            apply ``Right identity in Either<TLeft, TRight> 2``
            apply ``Associativity in Either<TLeft, TRight> 1``
            apply ``Associativity in Either<TLeft, TRight> 2``
            apply ``Associativity in Either<TLeft, TRight> 3``
            apply ``Associativity in Either<TLeft, TRight> 4``
        }

    module MonadLawsInFuncyList =
        let returnFList<'T> x = FuncyList.Construct([|x|])

        let ``Left identity in FuncyList<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f a ->
            let f_ = Func<int, FuncyList<int>>(fun x -> FuncyList.Cons(f.Invoke(x), FuncyList.Nil()) :> FuncyList<int>)
            // return a >>= f ≡ f a
            (returnFList a).ComputeWith(f_) = f_.Invoke(a)
        )
        
        let ``Left identity in FuncyList<T> 2`` = Prop.forAll(Arb.int)(fun a ->
            let f_ = Func<int, FuncyList<int>>(fun _ -> FuncyList.Nil() :> FuncyList<int>)
            // return a >>= f ≡ f a
            (returnFList a).ComputeWith(f_) = f_.Invoke(a)
        )

        let ``Right identity in FuncyList<T> 1`` = Prop.forAll(Arb.array(Arb.int).NonNull)(fun a ->
            let m = FuncyList.Construct(a)
            // m >>= return ≡ m
            m.ComputeWith(Func<int, FuncyList<int>>(fun x -> returnFList x)) = m
        )

        let ``Right identity in FuncyList<T> 2`` = Prop.forAll(Arb.int)(fun _ ->
            let m = FuncyList.Nil()
            // m >>= return ≡ m
            m.ComputeWith(Func<int, FuncyList<int>>(fun x -> returnFList x)) = (m :> FuncyList<int>)
        )

        let ``Associativity in FuncyList<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.array(Arb.int).NonNull)(fun f g a ->
            let m = FuncyList.Construct(a)
            let f_ = Func<int, FuncyList<int>>(fun x -> FuncyList.Cons(f.Invoke(x), FuncyList.Nil()) :> FuncyList<int>)
            let g_ = Func<int, FuncyList<int>>(fun x -> FuncyList.Construct([|g.Invoke(x)|]))
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in FuncyList<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int))(fun f g ->
            let m = FuncyList.Nil()
            let f_ = Func<int, FuncyList<int>>(fun x -> FuncyList.Construct([|f.Invoke(x)|]))
            let g_ = Func<int, FuncyList<int>>(fun x -> FuncyList.Cons(g.Invoke(x), FuncyList.Nil()) :> FuncyList<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in FuncyList<T> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun g i ->
            let m = FuncyList.Cons(i, FuncyList.Nil())
            let f_ = Func<int, FuncyList<int>>(fun _ -> FuncyList.Nil() :> FuncyList<int>)
            let g_ = Func<int, FuncyList<int>>(fun x -> FuncyList.Construct([|g.Invoke(x)|]))
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in FuncyList<T> 4`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.array(Arb.int).NonNull)(fun f a ->
            let m = FuncyList.Construct(a)
            let f_ = Func<int, FuncyList<int>>(fun x -> FuncyList.Cons(f.Invoke(x), FuncyList.Nil()) :> FuncyList<int>)
            let g_ = Func<int, FuncyList<int>>(fun _ -> FuncyList.Nil() :> FuncyList<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Monad laws`` = property {
            apply ``Left identity in FuncyList<T> 1``
            apply ``Left identity in FuncyList<T> 2``
            apply ``Right identity in FuncyList<T> 1``
            apply ``Right identity in FuncyList<T> 2``
            apply ``Associativity in FuncyList<T> 1``
            apply ``Associativity in FuncyList<T> 2``
            apply ``Associativity in FuncyList<T> 3``
            apply ``Associativity in FuncyList<T> 4``
        }

    module MonadLawsInNonEmptyList =
        let returnNEL x = NonEmptyList.Construct([x])

        let ``Left identity in NonEmptyList<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f a ->
            let f_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.Singleton(f.Invoke(x)) :> NonEmptyList<int>)
            // return a >>= f ≡ f a
            (returnNEL a).ComputeWith(f_) = f_.Invoke(a)
        )
        
        let ``Left identity in NonEmptyList<T> 2`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun f a ->
            let f_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.ConsNEL(f.Invoke(x), NonEmptyList.Singleton(f.Invoke(x) * 2)) :> NonEmptyList<int>)
            // return a >>= f ≡ f a
            (returnNEL a).ComputeWith(f_) = f_.Invoke(a)
        )

        let ``Right identity in NonEmptyList<T>`` = Prop.forAll(Arb.nonEmptyList(Arb.int))(fun ls ->
            let m = NonEmptyList.Construct(ls)
            // m >>= return ≡ m
            m.ComputeWith(Func<int, NonEmptyList<int>>(fun x -> returnNEL x)) = m
        )

        let arbFs = {
          Gen = Gen.listOfLength 3 <| Arb.systemFunc(CoArb.int, Arb.int).Gen
          Shrinker = Shrink.shrinkList <| Arb.systemFunc(CoArb.int, Arb.int).Shrinker
          PrettyPrinter = Pretty.prettyList
        }

        let ``Associativity in NonEmptyList<T> 1`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.nonEmptyList(Arb.int))(fun f1 f2 g ls ->
            let m = NonEmptyList.Construct(ls)
            let f_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.ConsNEL(f1.Invoke(x), NonEmptyList.Singleton(f2.Invoke(x))) :> NonEmptyList<int>)
            let g_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.Singleton(g.Invoke(x)) :> NonEmptyList<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in NonEmptyList<T> 2`` = Prop.forAll(Arb.nonEmptyList(Arb.systemFunc(CoArb.int, Arb.int)), Arb.systemFunc(CoArb.int, Arb.int), Arb.systemFunc(CoArb.int, Arb.int), Arb.int)(fun fs g1 g2 a ->
            let m = NonEmptyList.Singleton(a)
            let f_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.Construct(List.map (fun (f: Func<int, int>) -> f.Invoke(x)) fs))
            let g_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.ConsNEL(g1.Invoke(x), NonEmptyList.Singleton(g2.Invoke(x))) :> NonEmptyList<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in NonEmptyList<T> 3`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.nonEmptyList(Arb.systemFunc(CoArb.int, Arb.int)), Arb.int, Arb.int)(fun f gs i j ->
            let m = NonEmptyList.ConsNEL(i, NonEmptyList.Singleton(j))
            let f_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.Singleton(f.Invoke(x)) :> NonEmptyList<int>)
            let g_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.Construct(List.map (fun (g: Func<int, int>) -> g.Invoke(x)) gs))
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in NonEmptyList<T> 4`` = Prop.forAll(arbFs, arbFs, Arb.nonEmptyList(Arb.int))(fun fs gs ls ->
            let m = NonEmptyList.Construct(ls)
            let f_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.Construct(List.map (fun (f: Func<int, int>) -> f.Invoke(x)) fs))
            let g_ = Func<int, NonEmptyList<int>>(fun x -> NonEmptyList.Construct(List.map (fun (g: Func<int, int>) -> g.Invoke(x)) gs))
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Monad laws`` = property {
            apply ``Left identity in NonEmptyList<T> 1``
            apply ``Left identity in NonEmptyList<T> 2``
            apply ``Right identity in NonEmptyList<T>``
            apply ``Associativity in NonEmptyList<T> 1``
            apply ``Associativity in NonEmptyList<T> 2``
            apply ``Associativity in NonEmptyList<T> 3``
            apply ``Associativity in NonEmptyList<T> 4``
        }
