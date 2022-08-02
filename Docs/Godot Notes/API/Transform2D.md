# Transform2D[¶](https://docs.godotengine.org/en/stable/classes/class_transform2d.html#transform2d "Permalink to this headline")

## Description[¶](https://docs.godotengine.org/en/stable/classes/class_transform2d.html#description "Permalink to this headline")

2×3 matrix (2 rows, 3 columns) used for 2D linear transformations. It can represent transformations such as translation, rotation, or scaling. It consists of three [Vector2](https://docs.godotengine.org/en/stable/classes/class_vector2.html#class-vector2) values: [x](https://docs.godotengine.org/en/stable/classes/class_transform2d.html#class-transform2d-property-x), [y](https://docs.godotengine.org/en/stable/classes/class_transform2d.html#class-transform2d-property-y), and the [origin](https://docs.godotengine.org/en/stable/classes/class_transform2d.html#class-transform2d-property-origin).

For more information, read the [[Matrices and Transforms]] documentation article.

	**use `transform.xform(relative_offset)` to go from local to world
	use `transform.xform_inv(world_offset)` to go from world to local**

note that `transform.xform` and `transform.xform_inv` accept  [[API/Vector2]], [[Rect2]], or [PoolVector2Array](https://docs.godotengine.org/en/stable/classes/class_poolvector2array.html#class-poolvector2array)

