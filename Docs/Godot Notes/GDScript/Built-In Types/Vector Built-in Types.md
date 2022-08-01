### Vector built-in types[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#vector-built-in-types "Permalink to this headline")

#### [Vector2](https://docs.godotengine.org/en/stable/classes/class_vector2.html#class-vector2)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#vector2 "Permalink to this headline")

2D vector type containing `x` and `y` fields. Can also be accessed as an array.

#### [Rect2](https://docs.godotengine.org/en/stable/classes/class_rect2.html#class-rect2)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#rect2 "Permalink to this headline")

2D Rectangle type containing two vectors fields: `position` and `size`. Also contains an `end` field which is `position + size`.

#### [Vector3](https://docs.godotengine.org/en/stable/classes/class_vector3.html#class-vector3)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#vector3 "Permalink to this headline")

3D vector type containing `x`, `y` and `z` fields. This can also be accessed as an array.

#### [Transform2D](https://docs.godotengine.org/en/stable/classes/class_transform2d.html#class-transform2d)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#transform2d "Permalink to this headline")

3×2 matrix used for 2D transforms.

#### [Plane](https://docs.godotengine.org/en/stable/classes/class_plane.html#class-plane)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#plane "Permalink to this headline")

3D Plane type in normalized form that contains a `normal` vector field and a `d` scalar distance.

#### [Quat](https://docs.godotengine.org/en/stable/classes/class_quat.html#class-quat)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#quat "Permalink to this headline")

Quaternion is a datatype used for representing a 3D rotation. It's useful for interpolating rotations.

#### [AABB](https://docs.godotengine.org/en/stable/classes/class_aabb.html#class-aabb)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#aabb "Permalink to this headline")

Axis-aligned bounding box (or 3D box) contains 2 vectors fields: `position` and `size`. Also contains an `end` field which is `position + size`.

#### [Basis](https://docs.godotengine.org/en/stable/classes/class_basis.html#class-basis)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#basis "Permalink to this headline")

3x3 matrix used for 3D rotation and scale. It contains 3 vector fields (`x`, `y` and `z`) and can also be accessed as an array of 3D vectors.

#### [Transform](https://docs.godotengine.org/en/stable/classes/class_transform.html#class-transform)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#transform "Permalink to this headline")

3D Transform contains a Basis field `basis` and a Vector3 field `origin`.