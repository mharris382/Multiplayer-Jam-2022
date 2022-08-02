# Matrices and transforms[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#matrices-and-transforms "Permalink to this headline")

## API
- [[Transform2D]]
- [[API/Vector2]]

As mentioned in the previous tutorial, it is important to remember that **in Godot, the Y axis points _down_ in 2D.** This is the opposite of how most schools teach linear algebra, with the Y axis pointing up.

![[Pasted image 20220728220309.png]]

### Scaling the transformation matrix[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#scaling-the-transformation-matrix "Permalink to this headline")

```
var t = Transform2D()
# Scale
t.x *= 2
t.y *= 2
transform = t # Change the node's transform to what we just calculated.
```

To calculate the object's scale from an existing transformation matrix, you can use length() on each of the column vectors.

```
scale.x = t.x.length
scale.y = t.y.length
```

### Rotating the transformation matrix[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#rotating-the-transformation-matrix "Permalink to this headline")

calculating rotation from code
![](https://docs.godotengine.org/en/stable/_images/rotate2.png)

Note: Godot represents all rotations with radians, not degrees. A full turn is TAU or PI*2 radians, and a quarter turn of 90 degrees is TAU/4 or PI/2 radians. Working with TAU usually results in more readable code.

```
var rot = 0.5 # The rotation to apply in radians
var t = Transform2D()
t.x.x = cos(rot)
t.y.y = cos(rot)
t.x.y = sin(rot)
t.y.x = -sin(rot)
transform = t # Change the node's transform to what we just calculated.
```


### Basis of the transformation matrix[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#basis-of-the-transformation-matrix "Permalink to this headline")
The X and Y vectors are together called the _basis_ of the transformation matrix. The terms "basis" and "basis vectors" are important to know.

You might have noticed that [[Transform2D]] actually has three [[API/Vector2]] values: x, y, and origin. The origin value is not part of the basis, but it is part of the transform, and we need it to represent position.

### Translating the transformation matrix[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#translating-the-transformation-matrix "Permalink to this headline")
![](https://docs.godotengine.org/en/stable/_images/identity-origin.png)
If we want the object to move to a position of (1, 2), we simply need to set its origin vector to (1, 2):
![](https://docs.godotengine.org/en/stable/_images/translate.png)


### Putting it all together[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#putting-it-all-together "Permalink to this headline")
```
var t = Transform2D()
# Translation
t.origin = Vector2(350, 150)
# Rotation
var rot = -0.5 # The rotation to apply.
t.x.x = cos(rot)
t.y.y = cos(rot)
t.x.y = sin(rot)
t.y.x = -sin(rot)
# Scale
t.x *= 3
t.y *= 3
transform = t # Change the node's transform to what we just calculated.
```

## Practical applications of transforms[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#practical-applications-of-transforms "Permalink to this headline")

In actual projects, you will usually be working with transforms inside transforms by having multiple [[Node2D]] or [Spatial](https://docs.godotengine.org/en/stable/classes/class_spatial.html#class-spatial) nodes parented to each other.

### Converting positions between transforms[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#converting-positions-between-transforms "Permalink to this headline")
There are many cases where you'd want to convert a position in and out of a transform. 

For example:
1.  if you have a position relative to the player and would like to find the world (parent-relative) position
1. if you have a world position and want to know where it is relative to the player. 
	
	
	**use `transform.xform(relative_offset)` to go from local to world
	use `transform.xform_inv(world_offset)` to go from world to local**

 find what a vector relative to the player would be defined in world space as using the "xform" method:
```
# World space vector 100 units below the player.
print(transform.xform(Vector2(0, 100)))
```
use the "xform_inv" method to find a what world space position would be if it was instead defined relative to the player:
```
# Where is (0, 100) relative to the player?
print(transform.xform_inv(Vector2(0, 100)))
```


### Moving an object relative to itself[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#moving-an-object-relative-to-itself "Permalink to this headline")

```
#moves the object 100 units to the right
transform.origin += transform.x * 100
```
In actual projects, you can use `translate_object_local` in 3D or `move_local_x` and `move_local_y` in 2D to do this

### Applying transforms onto transforms[¶](https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html#applying-transforms-onto-transforms "Permalink to this headline")

