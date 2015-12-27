namespace Funcy.Test

open System
open Funcy
open Persimmon
open Persimmon.Dried
open UseTestNameByReflection

module NaturalTransformationLawsCheck =

    (* commutative diagram
    
      C:
        a -> b

      D:
        F(a) -> F(b)
        |       |
        v       v
        G(a) -> G(b)

     *)

    // FuncyList
    module ElementAtForFuncyList =

        let ``FuncyListNT.ElementAt is natural`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.nonNull(Arb.array Arb.int), Arb.int)(fun f a l ->
            let Fa = FuncyList.Construct(a)
            Fa.ElementAt(l).FMap(f) = Fa.FMap(f).ElementAt(l)
        )

        let ``NaturalTransformation laws`` = property {
            apply ``FuncyListNT.ElementAt is natural``
        }

    module TakeForFuncyList =

        let ``FuncyListNT.Take is natural`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.nonNull(Arb.array Arb.int), Arb.int)(fun f a l ->
            let Fa = FuncyList.Construct(a)
            Fa.Take(l).FMap(f) = Fa.FMap(f).Take(l)
        )

        let ``NaturalTransformation laws`` = property {
            apply ``FuncyListNT.Take is natural``
        }

    module FirstForFuncyList =

        let ``FuncyListNT.First is natural`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.nonNull(Arb.array Arb.int))(fun f a ->
            let Fa = FuncyList.Construct(a)
            Fa.First().FMap(f) = Fa.FMap(f).First()
        )

        let ``NaturalTransformation laws`` = property {
            apply ``FuncyListNT.First is natural``
        }

    // NonEmptyList
    module ToFuncyListForNonEmptyList =

        let ``NonEmptyListNT.ToFuncyList is natural`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.list(Arb.int).NonEmpty)(fun f a ->
            let Fa = NonEmptyList.Construct(a)
            Fa.ToFuncyList().FMap(f) = Fa.FMap(f).ToFuncyList()
        )

        let ``NaturalTransformation laws`` = property {
            apply ``NonEmptyListNT.ToFuncyList is natural``
        }

    module TakeForNonEmptyList =

        let ``NonEmptyListNT.Take is natural`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.list(Arb.int).NonEmpty, Arb.int)(fun f a l ->
            let Fa = NonEmptyList.Construct(a)
            Fa.Take(l).FMap(f) = Fa.FMap(f).Take(l)
        )

        let ``NaturalTransformation laws`` = property {
            apply ``NonEmptyListNT.Take is natural``
        }

    module TakeFirstForNonEmptyList =

        let ``NonEmptyListNT.TakeFirst is natural`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.list(Arb.int).NonEmpty)(fun f a ->
            let Fa = NonEmptyList.Construct(a)
            f.Invoke((Fa.TakeFirst())) = Fa.FMap(f).TakeFirst()
        )

        let ``NaturalTransformation laws`` = property {
            apply ``NonEmptyListNT.TakeFirst is natural``
        }
