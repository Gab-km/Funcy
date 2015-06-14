# Funcy

Funcy is the one of the functional libraries for C#/VB.

## Usage

```c#
using System;
using Funcy;
using Funcy.Computations;

namespace Hello
{
    class Program
    {
        static void Main(string[] args)
        {
            var maybe = Maybe.Some("Hello").ComputeWith(hello =>
                            Maybe.Some("World").Compute(world =>
                                hello + " " + world + "!"
                            )
                        );
            if (maybe.IsSome)
            {
                Console.WriteLine(maybe.ToSome().Value);
            }
            else
            {
                Console.WriteLine("Fmm... Is there any troubles?");
            }
        }
    }
}
```
