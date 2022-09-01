#gdscript 
#### Casting[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#casting "Permalink to this headline")

Values assigned to typed variables must have a compatible type. If it's needed to coerce a value to be of a certain type, in particular for object types, you can use the casting operator `as`.

Casting between object types results in the same object if the value is of the same type or a subtype of the cast type.

```python
var my_node2D: Node2D
my_node2D = $Sprite as Node2D # Works since Sprite is a subtype of Node2D.
```

If the value is not a subtype, the casting operation will result in a `null` value.

```python
var my_node2D: Node2D
my_node2D = $Button as Node2D # Results in 'null' since a Button is not a subtype of Node2D.
```

For built-in types, they will be forcibly converted if possible, otherwise the engine will raise an error.

```python
var my_int: int
my_int = "123" as int # The string can be converted to int.
my_int = Vector2() as int # A Vector2 can't be converted to int, this will cause an error.
```

Casting is also useful to have better type-safe variables when interacting with the scene tree:

```python
# Will infer the variable to be of type Sprite.
var my_sprite := $Character as Sprite

($AnimPlayer as AnimationPlayer).play("walk")
```
Will fail if $AnimPlayer is not an AnimationPlayer, even if it has the method 'play()'.