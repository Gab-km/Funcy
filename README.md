# Funcy

Funcy is the one of the functional libraries for C#/VB.

## Usage

```c#
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
