#csharp #scripting #coroutines 
## Yield[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#yield "Permalink to this headline")

Something similar to GDScript's `yield` with a single parameter can be achieved with C#'s [yield keyword](https://docs.microsoft.com/en-US/dotnet/csharp/language-reference/keywords/yield).

The equivalent of yield on signal can be achieved with async/await and `Godot.Object.ToSignal`.

Example:

```cs
await ToSignal(timer, "timeout");
GD.Print("After timeout");
```