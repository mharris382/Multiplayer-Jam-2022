## Coordinate systems (2D)[¶](https://docs.godotengine.org/en/stable/tutorials/math/vector_math.html#coordinate-systems-2d "Permalink to this headline")

In 2D space, coordinates are defined using a horizontal axis (`x`) and a vertical axis (`y`). A particular position in 2D space is written as a pair of values such as `(4, 3)`.

![../../_images/vector_axis1.png](https://docs.godotengine.org/en/stable/_images/vector_axis1.png)

## Vector operations[¶](https://docs.godotengine.org/en/stable/tutorials/math/vector_math.html#vector-operations "Permalink to this headline")

You can use either method (x and y coordinates or angle and magnitude) to refer to a vector, but for convenience, programmers typically use the coordinate notation. For example, in Godot, the origin is the top-left corner of the screen, so to place a 2D node named `Node2D` 400 pixels to the right and 300 pixels down, use the following code:

```
$Node2D.position = Vector2(400, 300)
a = a.normalized()
```

### Reflection[¶](https://docs.godotengine.org/en/stable/tutorials/math/vector_math.html#reflection "Permalink to this headline")
```
var collision = move_and_collide(velocity * delta)
if collision:
    var reflect = collision.remainder.bounce(collision.normal)
    velocity = velocity.bounce(collision.normal)
    move_and_collide(reflect)
```

## Dot product[¶](https://docs.godotengine.org/en/stable/tutorials/math/vector_math.html#dot-product "Permalink to this headline") and Cross product[¶](https://docs.godotengine.org/en/stable/tutorials/math/vector_math.html#cross-product "Permalink to this headline")
```
#dot product
var c = a.dot(b)
var d = b.dot(a)

#cross product
var c = a.cross(b)
```
### Facing[¶](https://docs.godotengine.org/en/stable/tutorials/math/vector_math.html#facing "Permalink to this headline")

```
var AP = A.direction_to(P)
if AP.dot(fA) > 0:
    print("A sees P!")
```


# Advanced Math

### Distance to plane[¶](https://docs.godotengine.org/en/stable/tutorials/math/vectors_advanced.html#distance-to-plane "Permalink to this headline")
```
var distance = normal.dot(point)
```


# [[Matrices and Transforms]]