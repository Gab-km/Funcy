# クイックスタート

使い方を覚えるために、まずは簡単なところから始めてみましょう。

```csharp
using Funcy;
```

Funcy はここから始まります。

## パターンマッチ

`Funcy.Patterns` 名前空間を使うことで、パターンマッチを利用できます。

```csharp
using Funcy.Patterns;
```

パターンマッチというと、構造を分解して変数に割り当てたり、条件の網羅性をチェックしてくれたりするイメージを持ちますが、Funcy のパターンマッチは軽装備です。例えば網羅性のチェックはできないので、自分で気をつける必要があります。

定数パターンは `Case<T>.Of` を使って書きます。

```csharp
Matcher.Match(value).With(
	Case<int>.Of(1).Then(() => Console.WriteLine("No. 1!")),
    Case<int>.Of(2).Then(() => Console.WriteLine("2位じゃダメなんですか？"))
    Case<int>.Else().Then(() => Console.WriteLine("その他大勢"))
);
```

`IExtractor<T>` を実装しているクラスに対しては、 `Case<T>.From<IExtractor<T>>` を用いて識別子パターンを書くこともできます。

```csharp
return Matcher.ReturnMatch<string>(maybe).With(
    Case<string>.From<Some<string>>().Then(s => s),
    Case<string>.Else().Then(() => "何も得られませんでした")
);
```

なお、値を返さないパターンマッチは `Matcher.Match` メソッドを、値を返すパターンマッチは `Matcher.ReturnMatch<T>` メソッドを使用します。

## カリー化

Funcy では、多引数の `Action` や `Func` をカリー化し、1引数関数の列のように扱うことができます。

```csharp
// add : (int, int) -> int
var add = new Func<int, int, int>((x, y) => x + y);
// curriedAdd : int -> int -> int
var curriedAdd = Currying.Curry(add);
var curriedAdd3 = curriedAdd.Invoke(3);
curriedAdd3.Invoke(5);	// 8
```

カリー化した関数は `CurriedAction` や `CurriedFunction` というクラスのインスタンスとして扱われ、これらはまた元の `Action` や `Func` に戻すことができます。

```csharp
// uncurriedAdd : (int, int) -> int
var uncurriedAdd = Currying.Uncurry(curriedAdd);
uncurriedAdd(3, 5);		// 8
curriedAdd.Uncurry()(2, 7);	// 9
```

## Maybe と Either

### Maybe

値がないかもしれないことを表すのに `Maybe<T>` 型を使うことができます。

```csharp
var some = Maybe<string>.Some("Hello, world!");
var none = Maybe<string>.None();
Console.WriteLine(some.Value);	// Hello, world!
```

Maybe なのに Some / None なのは、VB だと 'Option' と 'Nothing' が予約語だからです。Option スタイルと Maybe スタイルのどちらに倒してもキーワードのエスケープが必要になるため、あえて混在させています。

### Either

処理の成功と失敗を表すのに `Either<TLeft, TRight>` 型を用いることができます。

```csharp
var right = Either<string, int>.Right(42);
var left = Either<string, int>.Left("Something wrong");
Console.WriteLine(right.Value);	// 42
Console.WriteLine(left.Value);	// Something wrong
```

## ファンクター/アプリカティヴ/モナド

Funcy では、他の関数型言語でもよく見かける計算スタイルを、ある程度使えるようになっています。

```csharp
var either3 = Either<Exeption, int>.Right(3);
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

もちろん、これらを使わなくとも何の問題もありませんが、 `Maybe` や `Either` を使っているうちに常用するようになる場合があります。