## Static typing[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#static-typing "Permalink to this headline")

Since Godot 3.1, GDScript supports [optional static typing](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/static_typing.html#doc-gdscript-static-typing).


To declare a variable's type, use `<variable>: <type>`:

```
var health: int = 0
```

To declare the return type of a function, use `-> <type>`:

```
func heal(amount: int) -> void:
```
