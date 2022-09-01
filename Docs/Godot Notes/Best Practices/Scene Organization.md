#godot

---
# Scene organization[¶](https://docs.godotengine.org/en/stable/tutorials/best_practices/scene_organization.html#scene-organization "Permalink to this headline")


## Options for Dependency Injection 

```
# Parent
$Child.connect("signal_name", object_with_method, "method_on_the_object")

# Child
emit_signal("signal_name") # Triggers parent-defined behavior.
```

```
# Parent
$Child.method_name = "do"

# Child, assuming it has String property 'method_name' and method 'do'.
call(method_name) # Call parent-defined method (which child must own).
```

```
# Parent
$Child.func_property = funcref(object_with_method, "method_on_the_object")

# Child
func_property.call_func() # Call parent-defined method (can come from anywhere).
```

```
# Parent
$Child.target = self

# Child
print(target) # Use parent-defined node.
```

```
# Parent
$Child.target_path = ".."

# Child
get_node(target_path) # Use parent-defined NodePath.
```


## Choosing a node tree structure[¶](https://docs.godotengine.org/en/stable/tutorials/best_practices/scene_organization.html#choosing-a-node-tree-structure "Permalink to this headline")


# When to use scenes versus scripts[¶](https://docs.godotengine.org/en/stable/tutorials/best_practices/scenes_versus_scripts.html#when-to-use-scenes-versus-scripts "Permalink to this headline")

## Anonymous types[¶](https://docs.godotengine.org/en/stable/tutorials/best_practices/scenes_versus_scripts.html#anonymous-types "Permalink to this headline")

## Named types[¶](https://docs.godotengine.org/en/stable/tutorials/best_practices/scenes_versus_scripts.html#named-types "Permalink to this headline")

## Performance of Script vs PackedScene[¶](https://docs.godotengine.org/en/stable/tutorials/best_practices/scenes_versus_scripts.html#performance-of-script-vs-packedscene "Permalink to this headline")
## Conclusion[¶](https://docs.godotengine.org/en/stable/tutorials/best_practices/scenes_versus_scripts.html#conclusion "Permalink to this headline")