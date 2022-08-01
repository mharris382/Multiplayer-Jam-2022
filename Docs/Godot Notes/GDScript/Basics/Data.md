## Data[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#data "Permalink to this headline")
(see [[Exports]], [[Properties]], [[Built-in types]], [[Array]], [[Dictionary]])
(seealso [[Cheatsheet]])

### Variables[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#variables "Permalink to this headline")

Variables can exist as class members or local to functions. They are created with the `var` keyword and may, optionally, be assigned a value upon initialization.

```
var a # Data type is 'null' by default.
var b = 5
var c = 3.8
var d = b + c # Variables are always initialized in order.
```


## Type Hints

Variables can optionally have a type specification. When a type is specified, the variable will be forced to have always that same type, and trying to assign an incompatible value will raise an error.

Types are specified in the variable declaration using a `:` (colon) symbol after the variable name, followed by the type.
```
var my_vector2: Vector2
var my_node: Node = Sprite.new()
```

## Initialization

If the variable is initialized within the declaration, the type can be inferred, so it's possible to omit the type name:

```
var my_vector2 := Vector2() # 'my_vector2' is of type 'Vector2'.
var my_node := Sprite.new() # 'my_node' is of type 'Sprite'.
```

Valid types are:

-   Built-in types (Array, Vector2, int, String, etc.).
-   Engine classes (Node, Resource, Reference, etc.).
-   Constant names if they contain a script resource (`MyScript` if you declared `const MyScript = preload("res://my_script.gd")`).
-   Other classes in the same script, respecting scope (`InnerClass.NestedClass` if you declared `class NestedClass` inside the `class InnerClass` in the same scope).
-   Script classes declared with the `class_name` keyword.