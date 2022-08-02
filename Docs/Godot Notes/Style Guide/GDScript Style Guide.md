# GDScript style guide[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#gdscript-style-guide "Permalink to this headline")
## Table of Contents
- [[Cheatsheet]]
- [[Style Guide - Code Order]]
- [[Style Guide - Naming Conventions]]
- [[Style Guide - Inferred Types]]
- [[Style Guide - Static Typing]]
- [[Style Guide - Numbers]]
- Formatting
	- [[Style Guide - Formatting]]
	- [[Style Guide - Indentation]]
	- [[Style Guide - Trailing Commas]]
	- [[Style Guide - Blank lines]]
	- [[Style Guide - Format multiline statements for readability]]
	- Line length
	- One statement per line
	- Avoid unnecessary parentheses
	- Boolean operators
	- Comment spacing
	- Whitespace
	- Quotes
- Members
	- Member variables
	- Local variables
	- methods and static functions

---
# Formatting
see [[Style Guide - Formatting]]
see [[Style Guide - Format multiline statements for readability]]

### Line length[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#line-length "Permalink to this headline")

Keep individual lines of code under 100 characters.

If you can, try to keep lines under 80 characters. This helps to read the code on small displays and with two scripts opened side-by-side in an external text editor. For example, when looking at a differential revision.

### One statement per line[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#one-statement-per-line "Permalink to this headline")

Never combine multiple statements on a single line. No, C programmers, not even with a single line conditional statement.

**Good**:
```
if position.x > width:
    position.x = 0

if flag:
    print("flagged")
```

**Bad**:
```
if position.x > width: position.x = 0

if flag: print("flagged")
```
The only exception to that rule is the ternary operator:

```
next_state = "fall" if not is_on_floor() else "idle"
```



### Avoid unnecessary parentheses[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#avoid-unnecessary-parentheses "Permalink to this headline")

Avoid parentheses in expressions and conditional statements. Unless necessary for order of operations or wrapping over multiple lines, they only reduce readability.

**Good**:
```
if is_colliding():
    queue_free()
```
**Bad**:
```
if (is_colliding()):
    queue_free()
```

### Boolean operators[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#boolean-operators "Permalink to this headline")

Prefer the plain English versions of boolean operators, as they are the most accessible:

-   Use `and` instead of `&&`.
-   Use `or` instead of `||`.

You may also use parentheses around boolean operators to clear any ambiguity. This can make long expressions easier to read.

**Good**:
```
if (foo and bar) or baz:
    print("condition is true")
```
**Bad**:
```
if foo && bar || baz:
    print("condition is true")
```

### Comment spacing[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#comment-spacing "Permalink to this headline")

Regular comments should start with a space, but not code that you comment out. This helps differentiate text comments from disabled code.

**Good**:
```
#This is a comment.
#print("This is disabled code")
```
**Bad**:
```
#This is a comment.
#print("This is disabled code")
```

### Whitespace[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#whitespace "Permalink to this headline")

Always use one space around operators and after commas. Also, avoid extra spaces in dictionary references and function calls.

**Good**:
```
position.x = 5
position.y = target_position.y + 10
dict["key"] = 5
my_array = [4, 5, 6]
print("foo")
```
**Bad**:
```
position.x=5
position.y = mpos.y+10
dict ["key"] = 5
myarray = [4,5,6]
print ("foo")
```
Don't use spaces to align expressions vertically:

```
x        = 100
y        = 100
velocity = 500
```
### Quotes[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#quotes "Permalink to this headline")

Use double quotes unless single quotes make it possible to escape fewer characters in a given string. See the examples below:
```
# Normal string.
print("hello world")

# Use double quotes as usual to avoid escapes.
print("hello 'world'")

# Use single quotes as an exception to the rule to avoid escapes.
print('hello "world"')

# Both quote styles would require 2 escapes; prefer double quotes if it's a tie.
print("'hello' \"world\"")
```



# Members

see [[Style Guide - Code Order]]
see [[Style Guide - Class Declaration]]
see [[Style Guide - Signals and Properties]]

### Member variables[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#member-variables "Permalink to this headline")

Don't declare member variables if they are only used locally in a method, as it makes the code more difficult to follow. Instead, declare them as local variables in the method's body.

### Local variables[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#local-variables "Permalink to this headline")

Declare local variables as close as possible to their first use. This makes it easier to follow the code, without having to scroll too much to find where the variable was declared.

### Methods and static functions[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#methods-and-static-functions "Permalink to this headline")

After the class's properties come the methods.

Start with the `_init()` callback method, that the engine will call upon creating the object in memory. Follow with the `_ready()` callback, that Godot calls when it adds a node to the scene tree.

These functions should come first because they show how the object is initialized.

Other built-in virtual callbacks, like `_unhandled_input()` and `_physics_process`, should come next. These control the object's main loop and interactions with the game engine.

The rest of the class's interface, public and private methods, come after that, in that order.
```
func _init():
    add_to_group("state_machine")

func _ready():
    connect("state_changed", self, "_on_state_changed")
    _state.enter()

func _unhandled_input(event):
    _state.unhandled_input(event)

func transition_to(target_state_path, msg={}):
    if not has_node(target_state_path):
        return

    var target_state = get_node(target_state_path)
    assert(target_state.is_composite == false)

    _state.exit()
    self._state = target_state
    _state.enter(msg)
    Events.emit_signal("player_state_changed", _state.name)

func _on_state_changed(previous, new):
    print("state changed")
    emit_signal("state_changed")
```
