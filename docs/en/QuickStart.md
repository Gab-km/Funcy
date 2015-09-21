# Quick Start

To understand how to use Funcy, write easy examples.

```csharp
using Funcy;
```

Funcy begins here.

## Pattern Matching

You can use pattern matching by using `Funcy.Patterns` namespace.

```csharp
using Funcy.Patterns;
```

Funcy's patterns has limited power. For example, you should take care that patterns are exhaustive - Funcy cannot check it.

Write `Case<T>.Of` to use constant patterns.

```charp
Matcher.Match(value).With(
    Case<int>.Of(1).Then(() => Console.WriteLine("No. 1!")),
    Case<int>.Of(2).Then(() => Console.WriteLine("Everyone knows the first one, not the second.")),
    Case<int>.Else().Then(() => Console.WriteLine("The crowd"))
);
```

You can also write identifier patterns by using `Case<T>.From<IExtractor<T>>` with the class which implements `IExtractor<T>`.

```csharp
return Matcher.ReturnMatch<string>(maybe).With(
    Case<string>.From<Some<string>>().Then(s => s),
    Case<string>.Else().Then(() => "You got nothing.")
);
```

To write pattern matches, use `Matcher.Match` not to return any values, and `Matcher.ReturnMatch<T>` to return `T` value.

## Currying

In Funcy, you can curry poly-parameterized `Action` or `Func` and treat them as mono-parameterized ones.

```csharp
// add : (int, int) -> int
var add = new Func<int, int, int>((x, y) => x + y);
// curriedAdd : int -> int -> int
var curriedAdd = Currying.Curry(add);
var curriedAdd3 = curriedAdd.Invoke(3);
curriedAdd3.Invoke(5);	// 8
```

When you curry delegates, you get the instance of `CurriedAction` or `CurriedFunction` class, and you can uncurry them to return to the original `Action` or `Func`.

```csharp
// uncurriedAdd : (int, int) -> int
var uncurriedAdd = Currying.Uncurry(curriedAdd);
uncurriedAdd(3, 5);		// 8
curriedAdd.Uncurry()(2, 7);	// 9
```

## Maybe and Either

### Maybe

You can use `Maybe<T>` type to represent that there may or may not be a value.

```csharp
var some = Maybe<string>.Some("Hello, world!");
var none = Maybe<string>.None();
Console.WriteLine(some.Value);	// Hello, world!
```

Because 'Option' and 'Nothing' are both reserved keywords in VB, Funcy uses the name Some / None with Maybe. You need to escape the names whether in Option style or Maybe style, so these are merged.

### Either

You can use `Either<TLeft, TRight>` type to represent values which is either correct or error.

```csharp
var right = Either<string, int>.Right(42);
var left = Either<string, int>.Left("Something wrong");
Console.WriteLine(right.Value);	// 42
Console.WriteLine(left.Value);	// Something wrong
```

## Functor / Applicative / Monad

You can use some functional computation idioms which is also seen in other functional programming languages.

```csharp
var either3 = Either<Exception, int>.Right(3);
var either6 = either3.FMap(x => x * 2);	// Right(6)
```

```csharp
var maybe3 = Maybe<int>.Some(3);
// add : int -> int -> int
var add = Currying.Curry(new Func<int, int, int>((x, y) => x + y));
// maybe3plus : Maybe<int -> int>
var maybe3plus = maybe3.FMap(add.Invoke);
var maybe5 = Maybe<int>.Some(5);
maybe5.Apply(maybe3plus);	// Some(8)
```

```csharp
// f : 'a -> Either<Exception, string>
var f = (x) => Either<Exception, string>.Right(x.ToString());
var right = Either<Exception, int>.Right(2);
right.Compute(f);	// Right("2") : Either<Exception, string>
var ex = new Exception("error");
var left = Either<Exception, int>.Left(ex);
left.Compute(f);	// Left(ex) : Either<Exception, string>
```

Of cource you don&apos;t have to use these idioms, but you may use them frequently while you deal with `Maybe` or `Either` types.