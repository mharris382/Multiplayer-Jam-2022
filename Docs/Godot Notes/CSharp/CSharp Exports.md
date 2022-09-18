
seealso: [[Exports]], [[Advanced Exports]]

## [Export keyword](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#export-keyword "Permalink to this headline")

Use the `[Export]` attribute instead of the GDScript `export` keyword. This attribute can also be provided with optional [PropertyHint](https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html#enum-globalscope-propertyhint) and `hintString` parameters. Default values can be set by assigning a value.

## Example:

```cs
using Godot;

public class MyNode : Node
{
    [Export]
    private NodePath _nodePath;

    [Export]
    private string _name = "default";

    [Export(PropertyHint.Range, "0,100000,1000,or_greater")]
    private int _income;

    [Export(PropertyHint.File, "*.png,*.jpg")]
    private string _icon;
}
```



# [PropertyHint](https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html#enum-globalscope-propertyhint)
enum **PropertyHint**:


-   **PROPERTY_HINT_NONE** = **0** --- No hint for the edited property.

## Number Ranges
-   **PROPERTY_HINT_RANGE** = **1** --- Hints that an integer or float property should be within a range specified via the hint string `"min,max"` or `"min,max,step"`. 
	- The hint string can optionally include `"or_greater"` and/or `"or_lesser"` to allow manual input going respectively above the max or below the min values. Example: `"-360,360,1,or_greater,or_lesser"`.
-   **PROPERTY_HINT_EXP_RANGE** = **2** --- Hints that a float property should be within an exponential range specified via the hint string `"min,max"` or `"min,max,step"`. 
	- The hint string can optionally include `"or_greater"` and/or `"or_lesser"` to allow manual input going respectively above the max or below the min values. Example: `"0.01,100,0.01,or_greater"`.

### Numerical
Unlike [PROPERTY_HINT_ENUM](https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html#class-globalscope-constant-property-hint-enum) a property with this hint still accepts arbitrary values and can be empty. The list of values serves to suggest possible values.
-   **PROPERTY_HINT_EXP_EASING** = **4** --- Hints that a float property should be edited via an exponential easing function. 
	- The hint string can include `"attenuation"` to flip the curve horizontally and/or `"inout"` to also include in/out easing.
-   **PROPERTY_HINT_LENGTH** = **5** --- Deprecated hint, unused.
-   **PROPERTY_HINT_KEY_ACCEL** = **7** --- Deprecated hint, unused.


## Enumurations
-   **PROPERTY_HINT_ENUM** = **3** --- Hints that an integer, float or string property is an enumerated value to pick in a list specified via a hint string.
	- The hint string is a comma separated list of names such as `"Hello,Something,Else"`. 
	- For integer and float properties, the first name in the list has value 0, the next 1, and so on. Explicit values can also be specified by appending `:integer` to the name, e.g. `"Zero,One,Three:3,Four,Six:6"`.

-   **PROPERTY_HINT_ENUM_SUGGESTION** = **39** --- Hints that a string property can be an enumerated value to pick in a list specified via a hint string such as `"Hello,Something,Else"`.

-   **PROPERTY_HINT_FLAGS** = **8** --- Hints that an integer property is a bitmask with named bit flags. For example, to allow toggling bits 0, 1, 2 and 4, the hint could be something like `"Bit0,Bit1,Bit2,,Bit4"`.



## Layers
### Layers 2D
-   **PROPERTY_HINT_LAYERS_2D_RENDER** = **9** --- Hints that an integer property is a bitmask using the optionally named 2D render layers.
-   **PROPERTY_HINT_LAYERS_2D_PHYSICS** = **10** --- Hints that an integer property is a bitmask using the optionally named 2D physics layers.
-   **PROPERTY_HINT_LAYERS_2D_NAVIGATION** = **11** --- Hints that an integer property is a bitmask using the optionally named 2D navigation layers.
- 
### Layers 3D
-   **PROPERTY_HINT_LAYERS_3D_RENDER** = **12** --- Hints that an integer property is a bitmask using the optionally named 3D render layers.
-   **PROPERTY_HINT_LAYERS_3D_PHYSICS** = **13** --- Hints that an integer property is a bitmask using the optionally named 3D physics layers.
-   **PROPERTY_HINT_LAYERS_3D_NAVIGATION** = **14** --- Hints that an integer property is a bitmask using the optionally named 3D navigation layers.

## I/O
-   **PROPERTY_HINT_FILE** = **15** --- Hints that a string property is a path to a file. Editing it will show a file dialog for picking the path. The hint string can be a set of filters with wildcards like `"*.png,*.jpg"`.
-   **PROPERTY_HINT_DIR** = **16** --- Hints that a string property is a path to a directory. Editing it will show a file dialog for picking the path.
-   **PROPERTY_HINT_GLOBAL_FILE** = **17** --- Hints that a string property is an absolute path to a file outside the project folder. Editing it will show a file dialog for picking the path. The hint string can be a set of filters with wildcards like `"*.png,*.jpg"`.
-   **PROPERTY_HINT_GLOBAL_DIR** = **18** --- Hints that a string property is an absolute path to a directory outside the project folder. Editing it will show a file dialog for picking the path.

## Resource
-   **PROPERTY_HINT_RESURCE_TYPE** = **19** --- Hints that a property is an instance of a [Resource](https://docs.godotengine.org/en/stable/classes/class_resource.html#class-resource)-derived type, optionally specified via the hint string (e.g. `"Texture"`). Editing it will show a popup menu of valid resource types to instantiate.

## Text Elements
-   **PROPERTY_HINT_MULTILINE_TEXT** = **20** --- Hints that a string property is text with line breaks. Editing it will show a text input field where line breaks can be typed.
-   **PROPERTY_HINT_PLACEHOLDER_TEXT** = **21** --- Hints that a string property should have a placeholder text visible on its input field, whenever the property is empty. The hint string is the placeholder text to use.

## Color
-   **PROPERTY_HINT_COLOR_NO_ALPHA** = **22** --- Hints that a color property should be edited without changing its alpha component, i.e. only R, G and B channels are edited.
## Image
-   **PROPERTY_HINT_IMAGE_COMPRESS_LOSSY** = **23** --- Hints that an image is compressed using lossy compression.
-   **PROPERTY_HINT_IMAGE_COMPRESS_LOSSLESS** = **24** --- Hints that an image is compressed using lossless compression.