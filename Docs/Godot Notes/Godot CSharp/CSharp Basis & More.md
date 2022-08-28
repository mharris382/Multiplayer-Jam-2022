#csharp #scripting

## Basis[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#basis "Permalink to this headline")

Structs cannot have parameterless constructors in C#. Therefore, `new Basis()` initializes all primitive members to their default value. Use `Basis.Identity` for the equivalent of `Basis()` in GDScript and C++.

The following method was converted to a property with a different name:

GDScript | C#
--|--
get_scale() | Scale

## Transform2D[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#transform2d "Permalink to this headline")

Structs cannot have parameterless constructors in C#. Therefore, `new Transform2D()` initializes all primitive members to their default value. Please use `Transform2D.Identity` for the equivalent of `Transform2D()` in GDScript and C++.

The following methods were converted to properties with their respective names changed:

GDScript | C#
--|--
`get_rotation()` | `Rotation`
`get_scale()` | `Scale`

**## Plane[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#plane "Permalink to this headline")

The following method was converted to a property with a _slightly_ different name:

GDScript | C#
--|--
`center()` | `Center`

## Rect2[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#rect2 "Permalink to this headline")

The following field was converted to a property with a _slightly_ different name:

GDScript
GDScript | C#
--|--
`end` | `End`

## Dictionary[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#dictionary "Permalink to this headline")
## Array[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#array "Permalink to this headline")
'## Variant[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#variant "Permalink to this headline")'
`System.Object` (`object`) is used instead of `Variant`.

