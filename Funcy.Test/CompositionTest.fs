namespace Funcy.Test

open System
open Funcy
open Persimmon
open UseTestNameByReflection

module CompositionTest =
    let ``(* 10) . abs = fun x -> (* 10) <| abs x`` = test {
        let x10 x = x * 10
        let sut = Composition.Compose(Func<int, int>(x10), Func<int, int>(abs))
        do! assertEquals 10 (sut.Invoke(1))
        do! assertEquals 130 (sut.Invoke(-13))
    }

    let ``(:: 0) . List.rev = fun ls -> (:: 0) <| List.rev ls`` = test {
        let cons0 ls = 0 :: ls
        let sut = Composition.Compose(Func<int list, int list>(cons0), Func<int list, int list>(List.rev))
        do! assertEquals [0; 4; 3; 3] (sut.Invoke([3; 3; 4]))
        do! assertEquals [0] (sut.Invoke([]))
    }

    let ``(int -> string) . (string -> int) = (string -> string)`` = test {
        let sut = Composition.Compose(
                    Func<int, string>(fun i -> String.Format("{0} is number.", i)),
                    Func<string, int>(fun h -> Convert.ToInt32(h, 16)))
        do! assertEquals "42 is number." (sut.Invoke("0x2A"))
    }
