# Funcy

Funcy is one of the functional libraries for C#/VB.

## Usage

C#:

```csharp
using System;
using Funcy;
using Funcy.Patterns;

namespace Hello
{
    class Program
    {
        static void Main(string[] args)
        {
            var maybe = Maybe<string>.Some("Hello").ComputeWith(hello =>
                Maybe<string>.Some("World").Compute(world =>
                    hello + " " + world + "!"
                )
            );
            Matcher.Match(maybe).With(
                Case<string>.From<Some<string>>().Then(s => Console.WriteLine(s)),
                Case<string>.Else().Then(() => Console.WriteLine("Fmm... Are there any troubles?"))
            );
        }
    }
}
```

VB:

```vb
Imports Funcy
Imports Funcy.Patterns

Module Module1

    Sub Main()
        Dim maybeHello = Maybe(Of String).Some("Hello").ComputeWith(
            Function(hello) Maybe(Of String).Some("World").Compute(
                Function(world) hello + " " + world + "!"
            )
        )
        Matcher.Match(maybeHello).With(
            [Case](Of String).From(Of Some(Of String))().Then(Sub(s) Console.WriteLine(s)),
            [Case](Of String).Else().Then(Sub() Console.WriteLine("Fmm... Are there any troubles?"))
        )
    End Sub

End Module
```

# Documentation

* [English](https://github.com/Gab-km/Funcy/blob/master/docs/en/index.md)
* [Japanese](https://github.com/Gab-km/Funcy/blob/master/docs/ja/index.md)

# Contribution

Pull Requests are very welcome! You can also open issues freely. The one and only important point is Funcy's tests, which are written in F# and use [Persimmon](https://github.com/persimmon-projects/Persimmon). Try it and familiarize yourself with it!

[![Build status](https://ci.appveyor.com/api/projects/status/6rxw9lmpqbuws9gi/branch/master?svg=true)](https://ci.appveyor.com/project/Gabkm/funcy/branch/master)

# License

The MIT License - see [LICENSE](https://github.com/Gab-km/Funcy/blob/master/LICENSE.txt)
