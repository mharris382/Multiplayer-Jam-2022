# Coroutines & signals[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#coroutines-signals "Permalink to this headline")
The real strength of using `yield` is when combined with [[Signals]]. `yield` can accept two arguments, an object and a signal. When the signal is received, execution will recommence. Here are some examples:

```
# Resume execution the next frame.
yield(get_tree(), "idle_frame")

# Resume execution when animation is done playing.
yield(get_node("AnimationPlayer"), "animation_finished")

# Wait 5 seconds, then resume execution.
yield(get_tree().create_timer(5.0), "timeout")
```


----

Coroutines themselves use the `completed` signal when they transition into an invalid state, for example:
```
func my_func():
    yield(button_func(), "completed")
    print("All buttons were pressed, hurray!")

func button_func():
    yield($Button0, "pressed")
    yield($Button1, "pressed")
```

`my_func` will only continue execution once both buttons have been pressed.


---- 


You can also get the signal's argument once it's emitted by an object:
```
# Wait for when any node is added to the scene tree.
var node = yield(get_tree(), "node_added")
```

----


If there is more than one argument, `yield` returns an [[array]] containing the arguments:

```
signal done(input, processed)

func process_input(input):
    print("Processing initialized")
    yield(get_tree(), "idle_frame")
    print("Waiting")
    yield(get_tree(), "idle_frame")
    emit_signal("done", input, "Processed " + input)

func _ready():
    process_input("Test") # Prints: Processing initialized
    var data = yield(self, "done") # Prints: waiting
    print(data[1]) # Prints: Processed Test
```


---


If you're unsure whether a function may yield or not, or whether it may yield multiple times, you can yield to the `completed` signal conditionally:

```
func generate():
    var result = rand_range(-1.0, 1.0)

    if result < 0.0:
        yield(get_tree(), "idle_frame")

    return result

func make():
    var result = generate()

    if result is GDScriptFunctionState: # Still working.
        result = yield(result, "completed")

    return result
```

This ensures that the function returns whatever it was supposed to return regardless of whether coroutines were used internally. Note that using `while` would be redundant here as the `completed` signal is only emitted when the function didn't yield anymore