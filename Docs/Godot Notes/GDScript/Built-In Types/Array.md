#gdscript

---

#### [Array](https://docs.godotengine.org/en/stable/classes/class_array.html#class-array)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#array "Permalink to this headline")

Generic sequence of arbitrary object types, including other arrays or dictionaries (see below). The array can resize dynamically. Arrays are indexed starting from index `0`. Negative indices count from the end.

```
var arr = []
arr = [1, 2, 3]
var b = arr[1] # This is 2.
var c = arr[arr.size() - 1] # This is 3.
var d = arr[-1] # Same as the previous line, but shorter.
arr[0] = "Hi!" # Replacing value 1 with "Hi!".
arr.append(4) # Array is now ["Hi!", 2, 3, 4].
```

GDScript arrays are allocated linearly in memory for speed. Large arrays (more than tens of thousands of elements) may however cause memory fragmentation. If this is a concern, special types of arrays are available. These only accept a single data type. They avoid memory fragmentation and use less memory, but are atomic and tend to run slower than generic arrays. They are therefore only recommended to use for large data sets:

-   [PoolByteArray](https://docs.godotengine.org/en/stable/classes/class_poolbytearray.html#class-poolbytearray): An array of bytes (integers from 0 to 255).
-   [PoolIntArray](https://docs.godotengine.org/en/stable/classes/class_poolintarray.html#class-poolintarray): An array of integers.
-   [PoolRealArray](https://docs.godotengine.org/en/stable/classes/class_poolrealarray.html#class-poolrealarray): An array of floats.
-   [PoolStringArray](https://docs.godotengine.org/en/stable/classes/class_poolstringarray.html#class-poolstringarray): An array of strings.
-   [PoolVector2Array](https://docs.godotengine.org/en/stable/classes/class_poolvector2array.html#class-poolvector2array): An array of [[Vector2]] objects.
-   [PoolVector3Array](https://docs.godotengine.org/en/stable/classes/class_poolvector3array.html#class-poolvector3array): An array of [Vector3](https://docs.godotengine.org/en/stable/classes/class_vector3.html#class-vector3) objects.
-   [PoolColorArray](https://docs.godotengine.org/en/stable/classes/class_poolcolorarray.html#class-poolcolorarray): An array of [Color](https://docs.godotengine.org/en/stable/classes/class_color.html#class-color) objects.

---

seealso: [[Advanced Exports]]

