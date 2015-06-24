namespace Funcy.Test

open Funcy

module MatcherTest =
    open Persimmon
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

    let t4 = test "Matcher should throw MatchFailureException when any patterns are unmatched" {
        let target = 3.14159265358979m
        let isMatched = ref false
        let! e = trap { Matcher.Match(target).With(
                                Case.Of(3.141592653589m).Then(fun () -> isMatched := true))
                        |> ignore }
        do! assertEquals typeof<Funcy.Patterns.MatchFailureException> <| e.GetType()
        do! assertPred <| not !isMatched
    }

    let t5 = test "Matcher should perform the when pattern matching" {
        let target = 20
        let isGreaterThan10 = ref false
        let isElse = ref false
        let matcher = Matcher.Match(target).With(
                                Case.When(fun v -> v > 10).Then(fun () -> isGreaterThan10 := true),
                                Case.Else().Then(fun () -> isElse := true))
        do! assertPred !isGreaterThan10
        do! assertPred <| not !isElse
    }

    let t6 = test "Matcher should perform the from pattern matching 1" {
        let target = Maybe<int>.Some(4)
        let value = ref 0
        let isSome = ref false
        let isElse = ref false
        let matcher = Matcher.Match(target).With(
                                Case.From<Some<int>>().Then(fun v -> value := v; isSome := true),
                                Case.Else().Then(fun () -> isElse := true))
        do! assertPred !isSome
        do! assertEquals (target.ToSome().Value) !value
        do! assertPred <| not !isElse
    }

    let t7 = test "Matcher should perform the from pattern matching 2" {
        let target = Maybe<string>.None()
        let value = ref ""
        let isSome = ref false
        let isElse = ref false
        let matcher = Matcher.Match(target).With(
                                Case.From<Some<string>>().Then(fun s -> value := s; isSome := true),
                                Case.Else().Then(fun () -> isElse := true))
        do! assertPred <| not !isSome
        do! assertEquals "" !value
        do! assertPred !isElse
    }

    let t8 = test "Matcher should perform the from pattern matching 3" {
        let target = Either<exn, int>.Right(3)
        let value = ref 0
        let isRight = ref false
        let isLeft = ref false
        let matcher = Matcher.Match(target).With(
                                Case.From<Right<exn, int>>().Then(fun i -> value := i; isRight := true),
                                Case.From<Left<exn, int>>().Then(fun () -> isLeft := true))
        do! assertPred !isRight
        do! assertEquals 3 !value
        do! assertPred <| not !isLeft
    }

    let t9 = test "Matcher should perform the from pattern matching 4" {
        let err = System.Exception("hoge")
        let target = Either<exn, int>.Left(err)
        let valueRight = ref 0
        let valueLeft = ref null : exn ref
        let isRight = ref false
        let isLeft = ref false
        let matcher = Matcher.Match(target).With(
                                Case.From<Right<exn, int>>().Then(fun i -> valueRight := i; isRight := true),
                                Case.From<Left<exn, int>>().Then(fun e ->valueLeft := e; isLeft := true))
        do! assertPred <| not !isRight
        do! assertEquals 0 !valueRight
        do! assertPred !isLeft
        do! assertEquals err !valueLeft
    }
