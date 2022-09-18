## General differences[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#general-differences "Permalink to this headline")

As explained in the [C# basics](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#doc-c-sharp), C# generally uses `PascalCase` instead of the `snake_case` used in GDScript and C++.

## Global scope[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#global-scope "Permalink to this headline")

Global functions and some constants had to be moved to classes, since C# does not allow declaring them in namespaces. Most global constants were moved to their own enums.

### Math functions[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#math-functions "Permalink to this headline")

Math global functions, like `abs`, `acos`, `asin`, `atan` and `atan2`, are located under `Mathf` as `Abs`, `Acos`, `Asin`, `Atan` and `Atan2`. The `PI` constant can be found as `Mathf.Pi`.


### Other functions[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#other-functions "Permalink to this headline")

Many other global functions like `print` and `var2str` are located under `GD`. Example: `GD.Print` and `GD.Var2Str`.

| GDScript       | C#                    |
| -------------- | --------------------- |
| `weakref(obj)` | `Object.WeakRef(obj)` |
|`is_instance_valid(obj)`                |     `Object.IsInstanceValid(obj)`                  |


### Tips[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#tips "Permalink to this headline")

Sometimes it can be useful to use the `using static` directive. This directive allows to access the members and nested types of a class without specifying the class name.

```cs
using static Godot.GD;

public class Test
{
    static Test()
    {
        Print("Hello"); // Instead of GD.Print("Hello");
    }
}
```


[[CSharp Exports]]
