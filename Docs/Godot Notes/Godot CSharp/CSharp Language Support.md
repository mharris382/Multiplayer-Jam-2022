#csharp #scripting #godot

C# is a high-level programming language developed by Microsoft. In Godot, it is implemented with the **Mono 6.x .NET framework, including full support for C# 8.0.** Mono is an open source implementation of Microsoft's .NET Framework based on the ECMA standards for C# and the Common Language Runtime. A good starting point for checking its capabilities is the [Compatibility](https://www.mono-project.com/docs/about-mono/compatibility/) page in the Mono documentation.

[[CSharp Features]]
[[CSharp Preprocessor Defines]]
[[CSharp Signals]]
[[CSharp Basis & More]]
[[CSharp Yield]]

[[CSharp Singletons]]
[[CSharp Export keyword]]
[[CSharp onready keyword]]


![](https://docs.godotengine.org/en/stable/_static/docs_logo.png)

- [Official Documentation](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/index.html)
	- [General differences between C# and GDScript](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#general-differences-between-c-and-gdscript)
	- [Current Gotchas and known issues](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#current-gotchas-and-known-issues)
	- [Using NuGet packages in Godot](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#using-nuget-packages-in-godot)

![[Pasted image 20220903183713.png]]

PermaLinks
--
- General differences between C# and GDScript[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#general-differences-between-c-and-gdscript "Permalink to this headline")
- Current gotchas and known issues[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#current-gotchas-and-known-issues "Permalink to this headline")
- Performance of C# in Godot[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#performance-of-c-in-godot "Permalink to this headline")
- Using NuGet packages in Godot[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#using-nuget-packages-in-godot "Permalink to this headline")


## Communicating with other scripting languages[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#communicating-with-other-scripting-languages "Permalink to this headline")

This is explained extensively in [Cross-language scripting](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#doc-cross-language-scripting).

## Other differences[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#other-differences "Permalink to this headline")

`preload`, as it works in GDScript, is not available in C#. Use `GD.Load` or `ResourceLoader.Load` instead.

Other differences:

GDScript | C#
--|--
`Color8` | `Color.Color8`
`is_inf` | `float.IsInfinity`
`is_nan` | `float.IsNaN`
`dict2inst` | TODO
`inst2dict` | TODO
