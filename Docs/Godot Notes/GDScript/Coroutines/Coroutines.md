# Coroutines with yield[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#coroutines-with-yield "Permalink to this headline")

GDScript offers support for [coroutines](https://en.wikipedia.org/wiki/Coroutine) via the [yield](https://docs.godotengine.org/en/stable/classes/class_%40gdscript.html#class-gdscript-method-yield) built-in function. Calling `yield()` will immediately return from the current function, with the current frozen state of the same function as the return value. Calling `resume()` on this resulting object will continue execution and return whatever the function returns. Once resumed, the state object becomes invalid. 


---

## Examples

```
func my_func():
    print("Hello")
    yield()
    print("world")

func _ready():
    var y = my_func()
    # Function state saved in 'y'.
    print("my dear")
    y.resume()
    # 'y' resumed and is now an invalid state.
```
**Output**
```
Hello
my dear
world
```


---


It is also possible to pass values between `yield()` and `resume()`, for example:
```
func my_func():
    print("Hello")
    print(yield())
    return "cheers!"

func _ready():
    var y = my_func()
    # Function state saved in 'y'.
    print(y.resume("world"))
    # 'y' resumed and is now an invalid state.
```
**Output**
```
Hello
world
cheers!
```


---


**Remember to save the new function state, when using multiple `yield`s:**

```
func co_func():
    for i in range(1, 5):
        print("Turn %d" % i)
        yield();

func _ready():
    var co = co_func();
    while co is GDScriptFunctionState && co.is_valid():
        co = co.resume();
```

