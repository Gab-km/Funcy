namespace Funcy.Test

open Funcy
open Persimmon
open UseTestNameByReflection
open Funcy.Patterns

module MatcherTest =
    module SimpleMatcherTest =
        let ``Matcher should perform the value pattern matching 1`` = test {
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
    
        let ``Matcher should perform the value pattern matching 2`` = test {
            let target = System.DateTime(2015, 6, 17)
            let is20150617 = ref false
            let is20150618 = ref false
            let isElse = ref false
            do Matcher.Match(target).With(
                Case.Of(System.DateTime(2015, 6, 17)).Then(fun () -> is20150617 := true),
                Case.Of(System.DateTime(2015, 6, 18)).Then(fun () -> is20150618 := true),
                Case.Else().Then(fun () -> isElse := true))
            do! assertPred !is20150617
            do! assertPred <| not !is20150618
            do! assertPred <| not !isElse
        }

        let ``Matcher should perform the value pattern matching 3`` = test {
            let target = "hoge"
            let isCase = ref false
            let isElse = ref false
            let matcher = Matcher.Match(target).With(
                                    Case.Of("fuga").Then(fun () -> isCase := true),
                                    Case.Else().Then(fun () -> isElse := true))
            do! assertPred <| not !isCase
            do! assertPred !isElse
        }

        let ``Matcher should throw MatchFailureException when any patterns are unmatched`` = test {
            let target = 3.14159265358979m
            let isMatched = ref false
            let! e = trap { Matcher.Match(target).With(
                                    Case.Of(3.141592653589m).Then(fun () -> isMatched := true))
                            |> ignore }
            do! assertEquals typeof<Funcy.Patterns.MatchFailureException> <| e.GetType()
            do! assertPred <| not !isMatched
        }

        let ``Matcher should perform the when pattern matching`` = test {
            let target = 20
            let isGreaterThan10 = ref false
            let isElse = ref false
            let matcher = Matcher.Match(target).With(
                                    Case.When(fun v -> v > 10).Then(fun () -> isGreaterThan10 := true),
                                    Case.Else().Then(fun () -> isElse := true))
            do! assertPred !isGreaterThan10
            do! assertPred <| not !isElse
        }

        let ``Matcher should perform the from pattern matching when Some<T> is matched`` = test {
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

        let ``Matcher should perform the from pattern matching when Some<T> isn't matched`` = test {
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

        let ``Matcher should perform the from pattern matching when Right<TLeft, TRight> is matched`` = test {
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

        let ``Matcher should perform the from pattern matching when Left<TLeft, TRight> is matched`` = test {
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

        let ``Matcher should perform the from pattern matching when Cons<T> is matched`` = test {
            let target = Cons(1, Cons(2, Nil()))
            let valueHead = ref 0
            let isCons = ref false
            let isNil = ref false
            let action = System.Action<int * FuncyList<int>>(fun (head, tail) -> valueHead := head; isCons := true)
            Matcher.Match(target).With(
                Case.From<Cons<int>>().Then(action),
                Case.Else().Then(fun _ -> isNil := true)
            ) |> ignore
            do! assertEquals 1 !valueHead
            do! assertPred !isCons
            do! assertPred <| not !isNil
        }
        
    module ReturnableMatcherTest =
        let ``Matcher should perform the value pattern matching 1`` = test {
            let target = 10
            let actual = Matcher.ReturnMatch(target).With(
                                    Case.Of(9).Then(fun () -> "9"),
                                    Case.Of(10).Then(fun () -> "10"),
                                    Case.Else().Then(fun () -> System.String.Empty))
            do! assertEquals "10" actual
        }
    
        let ``Matcher should perform the value pattern matching 2`` = test {
            let target = System.DateTime(2015, 6, 17)
            let actual = Matcher.ReturnMatch(target).With(
                            Case.Of(System.DateTime(2015, 6, 17)).Then(fun () -> "20150617"),
                            Case.Of(System.DateTime(2015, 6, 18)).Then(fun () -> "20150618"),
                            Case.Else().Then(fun () -> "Else"))
            do! assertEquals "20150617" actual
        }

        let ``Matcher should perform the value pattern matching 3`` = test {
            let target = "hoge"
            let actual = Matcher.ReturnMatch(target).With(
                                    Case.Of("fuga").Then(fun () -> true),
                                    Case.Else().Then(fun () -> false))
            do! assertPred <| not actual
        }

        let ``Matcher should throw MatchFailureException when any patterns are unmatched`` = test {
            let target = 3.14159265358979m
            let isMatched = ref false
            let! e = trap { isMatched := Matcher.ReturnMatch(target).With(
                                    Case.Of(3.141592653589m).Then(fun () -> true))}
            do! assertEquals typeof<Funcy.Patterns.MatchFailureException> <| e.GetType()
            do! assertPred <| not !isMatched
        }

        let ``Matcher should perform the when pattern matching`` = test {
            let target = 20
            let actual = Matcher.ReturnMatch(target).With(
                                    Case.When(fun v -> v > 10).Then(fun () -> true),
                                    Case.Else().Then(fun () -> false))
            do! assertPred actual
        }

        let ``Matcher should perform the from pattern matching when Some<T> is matched`` = test {
            let target = Maybe<int>.Some(4)
            let actual = Matcher.ReturnMatch(target).With(
                                    Case.From<Some<int>>().Then<int>(id),
                                    Case.Else().Then(fun () -> -1))
            do! assertEquals (target.ToSome().Value) actual
        }

        let ``Matcher should perform the from pattern matching when Some<T> isn't matched`` = test {
            let target = Maybe<string>.None()
            let actual = Matcher.ReturnMatch(target).With(
                                    Case.From<Some<string>>().Then<string>(id),
                                    Case.Else().Then(fun () -> ""))
            do! assertEquals "" actual
        }

        let ``Matcher should perform the from pattern matching when Right<TLeft, TRight> is matched`` = test {
            let target = Either<exn, int>.Right(3)
            let actual = Matcher.ReturnMatch(target).With(
                                    Case.From<Right<exn, int>>().Then<int>(id),
                                    Case.From<Left<exn, int>>().Then(fun () -> 0))
            do! assertEquals 3 actual
        }

        let ``Matcher should perform the from pattern matching when Left<TLeft, TRight> is matched`` = test {
            let err = System.Exception("hoge")
            let target = Either<exn, int>.Left(err)
            let actual = Matcher.ReturnMatch(target).With(
                                    Case.From<Right<exn, int>>().Then(fun () -> null),
                                    Case.From<Left<exn, int>>().Then<exn>(id))
            do! assertEquals err actual
        }

        let ``Matcher should perform the from pattern matching when Cons<T> is matched`` = test {
            let target = Cons(1, Cons(2, Nil()))
            let func = System.Func<int * FuncyList<int>, FuncyList<int>>(snd)
            let actual = Matcher.ReturnMatch(target).With(
                            Case.From<Cons<int>>().Then(func),
                            Case.Else().Then(fun _ -> null))
            let expected = FuncyList.Construct([| 2 |])
            do! assertEquals expected actual
        }
