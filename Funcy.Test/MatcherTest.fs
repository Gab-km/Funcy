namespace Funcy.Test

open Funcy
open Persimmon

module MatcherTest =
    open Funcy.Patterns

    let t1 = test "Matcher should perform the value pattern matching 1" {
        let target = 10
        let is9 = ref false
        let is10 = ref false
        let isElse = ref false
        let matcher = Matcher.Match(target).With(
                                Case.Of(9).Then(fun () -> is9 := true),
                                Case.Of(10).Then(fun () -> is10 := true),
                                Case.Else().Then(fun () -> isElse := true))
        do! assertPred <| not !is9
        do! assertPred !is10
        do! assertPred <| not !isElse
    }
    
    let t2 = test "Matcher should perform the value pattern matching 2" {
        let target = System.DateTime(2015, 6, 17)
        let is20150617 = ref false
        let is20150618 = ref false
        let isElse = ref false
        let matcher = Matcher.Match(target).With(
                                Case.Of(System.DateTime(2015, 6, 17)).Then(fun () -> is20150617 := true),
                                Case.Of(System.DateTime(2015, 6, 18)).Then(fun () -> is20150618 := true),
                                Case.Else().Then(fun () -> isElse := true))
        do! assertPred !is20150617
        do! assertPred <| not !is20150618
        do! assertPred <| not !isElse
    }

    let t3 = test "Matcher should perform the value pattern matching 3" {
        let target = "hoge"
        let isCase = ref false
        let isElse = ref false
        let matcher = Matcher.Match(target).With(
                                Case.Of("fuga").Then(fun () -> isCase := true),
                                Case.Else().Then(fun () -> isElse := true))
        do! assertPred <| not !isCase
        do! assertPred !isElse
    }
