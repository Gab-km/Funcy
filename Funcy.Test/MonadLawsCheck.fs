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

        let ``Left identity in Maybe<T> 1`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f a ->
            let f_ = Func<int, IMaybe<int>>(fun x -> Maybe.Some(f.Invoke(x)) :> IMaybe<int>)
            // return a >>= f ≡ f a
            (returnMaybe a).ComputeWith(f_) = f_.Invoke(a)
        )
        
        let ``Left identity in Maybe<T> 2`` = Prop.forAll(Arb.int)(fun a ->
            let f_ = Func<int, IMaybe<int>>(fun _ -> Maybe.None() :> IMaybe<int>)
            // return a >>= f ≡ f a
            (returnMaybe a).ComputeWith(f_) = f_.Invoke(a)
        )

        let ``Right identity in Maybe<T> 1`` = Prop.forAll(Arb.int)(fun i ->
            let m = Maybe.Some(i) :> Maybe<int>
            // m >>= return ≡ m
            m.ComputeWith(Func<int, IMaybe<int>>(fun x -> returnMaybe x :> IMaybe<int>)) = (m :> IMaybe<int>)
        )

        let ``Right identity in Maybe<T> 2`` = Prop.forAll(Arb.int)(fun _ ->
            let m = Maybe.None() :> Maybe<int>
            // m >>= return ≡ m
            m.ComputeWith(Func<int, IMaybe<int>>(fun x -> returnMaybe x :> IMaybe<int>)) = (m :> IMaybe<int>)
        )

        let ``Associativity in Maybe<T> 1`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f g i ->
            let m = Maybe.Some(i) :> IMaybe<int>
            let f_ = Func<int, IComputable<int>>(fun x -> Maybe.Some(f.Invoke(x)) :> IComputable<int>)
            let g_ = Func<int, IComputable<int>>(fun x -> Maybe.Some(g.Invoke(x)) :> IComputable<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Maybe<T> 2`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int))(fun f g ->
            let m = Maybe.None() :> IMaybe<int>
            let f_ = Func<int, IComputable<int>>(fun x -> Maybe.Some(f.Invoke(x)) :> IComputable<int>)
            let g_ = Func<int, IComputable<int>>(fun x -> Maybe.Some(g.Invoke(x)) :> IComputable<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Maybe<T> 3`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun g i ->
            let m = Maybe.Some(i) :> IMaybe<int>
            let f_ = Func<int, IComputable<int>>(fun _ -> Maybe.None() :> IComputable<int>)
            let g_ = Func<int, IComputable<int>>(fun x -> Maybe.Some(g.Invoke(x)) :> IComputable<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Maybe<T> 4`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f i ->
            let m = Maybe.Some(i) :> IMaybe<int>
            let f_ = Func<int, IComputable<int>>(fun x -> Maybe.Some(f.Invoke(x)) :> IComputable<int>)
            let g_ = Func<int, IComputable<int>>(fun _ -> Maybe.None() :> IComputable<int>)
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

        let ``Left identity in Either<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f a ->
            let f_ = Func<int, IEither<exn, int>>(fun x -> Either<exn, int>.Right(f.Invoke(x)) :> IEither<exn, int>)
            // return a >>= f ≡ f a
            (returnEither a).ComputeWith(f_) = f_.Invoke(a)
        )
        
        let ``Left identity in Either<TLeft, TRight> 2`` = Prop.forAll(Arb.string, Arb.int)(fun s a ->
            let e = exn(s)
            let f_ = Func<int, IEither<exn, int>>(fun _ -> Either<exn, int>.Left(e) :> IEither<exn, int>)
            // return a >>= f ≡ f a
            (returnEither a).ComputeWith(f_) = f_.Invoke(a)
        )

        let ``Right identity in Either<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f i ->
            let m = Either<exn, int>.Right(i) :> Either<exn, int>
            // m >>= return ≡ m
            m.ComputeWith(Func<int, IEither<exn, int>>(fun x -> returnEither x :> IEither<exn, int>)) = (m :> IEither<exn, int>)
        )

        let ``Right identity in Either<TLeft, TRight> 2`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.string)(fun f s ->
            let m = Either<exn, int>.Left(exn(s)) :> Either<exn, int>
            // m >>= return ≡ m
            m.ComputeWith(Func<int, IEither<exn, int>>(fun x -> returnEither x :> IEither<exn, int>)) = (m :> IEither<exn, int>)
        )

        let ``Associativity in Either<TLeft, TRight> 1`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun f g i ->
            let m = Either<exn, int>.Right(i) :> IEither<exn, int>
            let f_ = Func<int, IComputable<int>>(fun x -> Either<exn, int>.Right(f.Invoke(x)) :> IComputable<int>)
            let g_ = Func<int, IComputable<int>>(fun x -> Either<exn, int>.Right(g.Invoke(x)) :> IComputable<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Either<TLeft, TRight> 2`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.string)(fun f g s ->
            let m = Either<exn, int>.Left(exn(s)) :> IEither<exn, int>
            let f_ = Func<int, IComputable<int>>(fun x -> Either<exn, int>.Right(f.Invoke(x)) :> IComputable<int>)
            let g_ = Func<int, IComputable<int>>(fun x -> Either<exn, int>.Right(g.Invoke(x)) :> IComputable<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Either<TLeft, TRight> 3`` = Prop.forAll(Arb.string, Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.int)(fun s g i ->
            let m = Either<exn, int>.Right(i) :> IEither<exn, int>
            let e = exn(s)
            let f_ = Func<int, IComputable<int>>(fun _ -> Either<exn, int>.Left(e) :> IComputable<int>)
            let g_ = Func<int, IComputable<int>>(fun x -> Either<exn, int>.Right(g.Invoke(x)) :> IComputable<int>)
            // (m >>= f) >>= g ≡ m >>= (\x -> f x >>= g)
            m.ComputeWith(f_).ComputeWith(g_) = m.ComputeWith(fun x -> f_.Invoke(x).ComputeWith(g_))
        )

        let ``Associativity in Either<TLeft, TRight> 4`` = Prop.forAll(Arb.systemFunc(CoArbitrary.int, Arb.int), Arb.string, Arb.int)(fun f s i ->
            let m = Either<exn, int>.Right(i) :> IEither<exn, int>
            let f_ = Func<int, IComputable<int>>(fun x -> Either<exn, int>.Right(f.Invoke(x)) :> IComputable<int>)
            let e = exn(s)
            let g_ = Func<int, IComputable<int>>(fun _ -> Either<exn, int>.Left(e) :> IComputable<int>)
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