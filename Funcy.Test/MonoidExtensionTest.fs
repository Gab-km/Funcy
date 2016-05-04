namespace Funcy.Test

open Funcy.Future.Computations
open Persimmon
open UseTestNameByReflection

module MonoidExtensionTest =
    // for comparison of floating point number
    let inline isNeighborhood (expected: ^a) (actual: ^a) (error: ^a) = expected - error <= actual && actual <= expected + error

    module MonoidalSumTest =
        let ``MonoidalSumSByte#MEmpty is 0y`` = test {
            let monoid = 1y.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0y
            do! assertEquals expected actual
        }
        let ``MonoidalSumSByte#MAappend is (+)`` = test {
            let monoid = 2y.ToMonoidalSum()
            let actual = monoid.MAppend(3y)
            let expected = 5y
            do! assertEquals expected actual
        }
        let ``MonoidalSumInt16#MEmpty is 0s`` = test {
            let monoid = 4s.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0s
            do! assertEquals expected actual
        }
        let ``MonoidalSumInt16#MAppend is (+)`` = test {
            let monoid = 5s.ToMonoidalSum()
            let actual = monoid.MAppend(6s)
            let expected = 11s
            do! assertEquals expected actual
        }
        let ``MonoidalSumInt32#MEmpty is 0`` = test {
            let monoid = (7).ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0
            do! assertEquals expected actual
        }
        let ``MonoidalSumInt32#MAppend is (+)`` = test {
            let monoid = (8).ToMonoidalSum()
            let actual = monoid.MAppend(9)
            let expected = 17
            do! assertEquals expected actual
        }
        let ``MonoidalSumInt64#MEmpty is 0L`` = test {
            let monoid = 10L.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0L
            do! assertEquals expected actual
        }
        let ``MonoidalSumInt64#MAppend is (+)`` = test {
            let monoid = 11L.ToMonoidalSum()
            let actual = monoid.MAppend(12L)
            let expected = 23L
            do! assertEquals expected actual
        }
        let ``MonoidalSumByte#MEmpty is 0uy`` = test {
            let monoid = 13uy.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0uy
            do! assertEquals expected actual
        }
        let ``MonoidalSumByte#MAppend is (+)`` = test {
            let monoid = 14uy.ToMonoidalSum()
            let actual = monoid.MAppend(15uy)
            let expected = 29uy
            do! assertEquals expected actual
        }
        let ``MonoidalSumUInt16#MEmpty is 0us`` = test {
            let monoid = 16us.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0us
            do! assertEquals expected actual
        }
        let ``MonoidalSumUInt16#MAppend is (+)`` = test {
            let monoid = 17us.ToMonoidalSum()
            let actual = monoid.MAppend(18us)
            let expected = 35us
            do! assertEquals expected actual
        }
        let ``MonoidalSumUInt32#MEmpty is 0u`` = test {
            let monoid = 19u.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0u
            do! assertEquals expected actual
        }
        let ``MonoidalSumUInt32#MAppend is (+)`` = test {
            let monoid = 20u.ToMonoidalSum()
            let actual = monoid.MAppend(21u)
            let expected = 41u
            do! assertEquals expected actual
        }
        let ``MonoidalSumUInt64#MEmpty is 0uL`` = test {
            let monoid = 22uL.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0uL
            do! assertEquals expected actual
        }
        let ``MonoidalSumUInt64#MAppend is (+)`` = test {
            let monoid = 23uL.ToMonoidalSum()
            let actual = monoid.MAppend(24uL)
            let expected = 47uL
            do! assertEquals expected actual
        }
        let ``MonoidalSumSingle#MEmpty is 0.0f`` = test {
            let monoid = -1.2f.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0.0f
            do! assertEquals expected actual
        }

        let ``MonoidalSumSingle#MAppend is (+)`` = test {
            let monoid = -2.3f.ToMonoidalSum()
            let actual = monoid.MAppend(3.4f)
            let expected = 1.1f
            do! assertPred <| isNeighborhood expected actual 0.00001f
        }
        let ``MonoidalSumDouble#MEmpty is 0.0`` = test {
            let monoid = -4.5.ToMonoidalSum()
            let actual = monoid.MEmpty
            let expected = 0.0
            do! assertEquals expected actual
        }
        let ``MonoidalSumDouble#MAppend is (+)`` = test {
            let monoid = -5.6.ToMonoidalSum()
            let actual = monoid.MAppend(6.7)
            let expected = 1.1
            do! assertPred <| isNeighborhood expected actual 0.00001
        }

    module MonoidalProductTest =
        let ``MonoidalProductSByte#MEmpty is 1y`` = test {
            let monoid = -1y.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1y
            do! assertEquals expected actual
        }
        let ``MonoidalProductSByte#MAppend is (*)`` = test {
            let monoid = -2y.ToMonoidalProduct()
            let actual = monoid.MAppend(3y)
            let expected = -6y
            do! assertEquals expected actual
        }
        let ``MonoidalProductInt16#MEmpty is 1s`` = test {
            let monoid = -3s.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1s
            do! assertEquals expected actual
        }
        let ``MonoidalProductInt16#MAppend is (*)`` = test {
            let monoid = -4s.ToMonoidalProduct()
            let actual = monoid.MAppend(5s)
            let expected= -20s
            do! assertEquals expected actual
        }
        let ``MonoidalProductInt32#MEmpty is 1`` = test {
            let monoid = (-6).ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1
            do! assertEquals expected actual
        }
        let ``MonoidalProductInt32#MAppend is (*)`` = test {
            let monoid = (-7).ToMonoidalProduct()
            let actual = monoid.MAppend(8)
            let expected = -56
            do! assertEquals expected actual
        }
        let ``MonoidalProductInt64#MEmpty is 1L`` = test {
            let monoid = -9L.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1L
            do! assertEquals expected actual
        }
        let ``MonoidalProductInt64#MAppend is (*)`` = test {
            let monoid = -10L.ToMonoidalProduct()
            let actual = monoid.MAppend(11L)
            let expected = -110L
            do! assertEquals expected actual
        }
        let ``MonoidalProductByte#MEmpty is 1uy`` = test {
            let monoid = 12uy.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1uy
            do! assertEquals expected actual
        }
        let ``MonoidalProductByte#MAppend is (*)`` = test {
            let monoid = 13uy.ToMonoidalProduct()
            let actual = monoid.MAppend(14uy)
            let expected = 182uy
            do! assertEquals expected actual
        }
        let ``MonoidalProductUInt16#MEmpty is 1us`` = test {
            let monoid = 15us.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1us
            do! assertEquals expected actual
        }
        let ``MonoidalProductUInt16#MAppend is (*)`` = test {
            let monoid = 16us.ToMonoidalProduct()
            let actual = monoid.MAppend(17us)
            let expected = 272us
            do! assertEquals expected actual
        }
        let ``MonoidalProductUInt32#MEmpty is 1u`` = test {
            let monoid = 18u.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1u
            do! assertEquals expected actual
        }
        let ``MonoidalProductUInt32#MAppend is (*)`` = test {
            let monoid = 19u.ToMonoidalProduct()
            let actual = monoid.MAppend(20u)
            let expected = 380u
            do! assertEquals expected actual
        }
        let ``MonoidalProductUInt64#MEmpty is 1uL`` = test {
            let monoid = 21uL.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1uL
            do! assertEquals expected actual
        }
        let ``MonoidalProductUInt64#MAppend is (*)`` = test {
            let monoid = 22uL.ToMonoidalProduct()
            let actual = monoid.MAppend(23uL)
            let expected = 506uL
            do! assertEquals expected actual
        }
        let ``MonoidalProductSingle#MEmpty is 1.0f`` = test {
            let monoid = -1.2f.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1.0f
            do! assertEquals expected actual
        }
        let ``MonoidalProductSingle#MAppend is (*)`` = test {
            let monoid = -2.3f.ToMonoidalProduct()
            let actual = monoid.MAppend(3.4f)
            let expected = -7.82f
            do! assertEquals expected actual
        }
        let ``MonoidalProductDouble#MEmpty is 1.0`` = test {
            let monoid = -4.5.ToMonoidalProduct()
            let actual = monoid.MEmpty
            let expected = 1.0
            do! assertEquals expected actual
        }
        let ``MonoidalProductDouble#MAppend is (*)`` = test {
            let monoid = -5.6.ToMonoidalProduct()
            let actual = monoid.MAppend(6.7)
            let expected = -37.52
            do! assertPred <| isNeighborhood actual expected 0.00001
        }
