#csharp #scripting
## String[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#string "Permalink to this headline")

Use `System.String` (`string`). Most of Godot's String methods are provided by the `StringExtensions` class as extension methods.

```cs
string upper = "I LIKE SALAD FORKS";
string lower = upper.ToLower();
```

There are a few differences, though:

`erase`: Strings are immutable in C#, so we cannot modify the string passed to the extension method. For this reason, `Erase` was added as an extension method of `StringBuilder` instead of string. Alternatively, you can use `string.Remove`.

`IsSubsequenceOf`/`IsSubsequenceOfi`: An additional method is provided, which is an overload of `IsSubsequenceOf`, allowing you to explicitly specify case sensitivity:

```cs
str.IsSubsequenceOf("ok"); // Case sensitive
str.IsSubsequenceOf("ok", true); // Case sensitive
str.IsSubsequenceOfi("ok"); // Case insensitive
str.IsSubsequenceOf("ok", false); // Case insensitive
```
`Match`/`Matchn`/`ExprMatch`: An additional method is provided besides Match and Matchn, which allows you to explicitly specify case sensitivity:

str.Match("*.txt"); // Case sensitive
str.ExprMatch("*.txt", true); // Case sensitive
str.Matchn("*.txt"); // Case insensitive
str.ExprMatch("*.txt", false); // Case insensitive