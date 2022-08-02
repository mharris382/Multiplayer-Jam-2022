
### Inferred types[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#inferred-types "Permalink to this headline")

In most cases you can let the compiler infer the type, using `:=`:

```
var health := 0  # The compiler will use the int type.
```

However, in a few cases when context is missing, the compiler falls back to the function's return type. For example, `get_node()` cannot infer a type unless the scene or file of the node is loaded in memory. In this case, you should set the type explicitly.



**Good**: (see [[Casting]])
```
onready var health_bar: ProgressBar = get_node("UI/LifeBar")

Alternatively, you can use the `as` keyword to cast the return type, and that type will be used to infer the type of the var.

onready var health_bar := get_node("UI/LifeBar") as ProgressBar
# health_bar will be typed as ProgressBar
```
This option is also considered more [type-safe](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/static_typing.html#doc-gdscript-static-typing-safe-lines) than the first.

**Bad**:
```
# The compiler can't infer the exact type and will use Node
# instead of ProgressBar.
onready var health_bar := get_node("UI/LifeBar")
```