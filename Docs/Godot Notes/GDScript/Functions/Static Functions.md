#gdscript 

---
#### Static functions[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#static-functions "Permalink to this headline")

A function can be declared static. When a function is static, it has no access to the instance member variables or `self`. This is mainly useful to make libraries of helper functions:

```
static func sum2(a, b):
    return a + b
```