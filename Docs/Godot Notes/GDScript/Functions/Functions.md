### Functions[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#functions "Permalink to this headline")
(see [[Classes]])
Functions always belong to a [class](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#classes). The scope priority for variable look-up is: local → class member → global. The `self` variable is always available and is provided as an option for accessing class members, but is not always required (and should _not_ be sent as the function's first argument, unlike Python).

```
func my_function(a, b):
    print(a)
    print(b)
    return a + b  # Return is optional; without it 'null' is returned.
```
A function can `return` at any point. The default return value is `null`.

Functions can also have type specification for the arguments and for the return value. Types for arguments can be added in a similar way to variables:

```
func my_function(a: int, b: String):
    pass
```

If a function argument has a default value, it's possible to infer the type:

```
func my_function(int_arg := 42, String_arg := "string"):
    pass
```

The return type of the function can be specified after the arguments list using the arrow token (`->`):

```
func my_int_function() -> int:
    return 0
```

Functions that have a return type **must** return a proper value. Setting the type as `void` means the function doesn't return anything. Void functions can return early with the `return` keyword, but they can't return any value.

```
func void_function() -> void:
    return # Can't return a value
```