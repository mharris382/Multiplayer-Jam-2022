
# Properties[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_exports.html#properties "Permalink to this headline")
(see [[Properties]]), [[Exports]]
To understand how to better use the sections below, you should understand how to make properties with [[Advanced Exports]]
```
func _get_property_list():
    var properties = []
    # Same as "export(int) var my_property"
    properties.append({
        name = "my_property",
        type = TYPE_INT
    })
    return properties
```

-   The `_get_property_list()` function gets called by the inspector. You can override it for more advanced exports. You must return an `Array` with the contents of the properties for the function to work.
-   `name` is the name of the property
-   `type` is the type of the property from `Variant.Type`.

## Attaching variables to properties[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_exports.html#attaching-variables-to-properties "Permalink to this headline")

To attach variables to properties (allowing the value of the property to be used in scripts), you need to create a variable with the exact same name as the property or else you may need to override the [_set()](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object-method-get-property-list) and [_get()](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object-method-get-property-list) methods. Attaching a variable to to a property also gives you the ability to give it a default state.

```
# This variable is determined by the function below.
# This variable acts just like a regular gdscript export.
var my_property = 5

func _get_property_list():
    var properties = []
    # Same as "export(int) var my_property"
    properties.append({
        name = "my_property",
        type = TYPE_INT
    })
    return properties
```

## Adding script categories[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_exports.html#adding-script-categories "Permalink to this headline")

For better visual distinguishing of properties, a special script category can be embedded into the inspector to act as a separator. `Script Variables` is one example of a built-in category.

```
func _get_property_list():
    var properties = []
    properties.append({
        name = "Debug",
        type = TYPE_NIL,
        usage = PROPERTY_USAGE_CATEGORY | PROPERTY_USAGE_SCRIPT_VARIABLE
    })

    # Example of adding a property to the script category
    properties.append({
        name = "Logging_Enabled",
        type = TYPE_BOOL
    })
    return properties
```
-   `name` is the name of a category to be added to the inspector;
-   Every following property added after the category definition will be a part of the category
-   `PROPERTY_USAGE_CATEGORY` indicates that the property should be treated as a script category specifically, so the type `TYPE_NIL` can be ignored as it won't be actually used for the scripting logic, yet it must be defined anyway.

## Grouping properties[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_exports.html#grouping-properties "Permalink to this headline")

A list of properties with similar names can be grouped.
```
func _get_property_list():
    var properties = []
    properties.append({
        name = "Rotate",
        type = TYPE_NIL,
        hint_string = "rotate_",
        usage = PROPERTY_USAGE_GROUP | PROPERTY_USAGE_SCRIPT_VARIABLE
    })

    # Example of adding to the group
    properties.append({
        name = "rotate_speed",
        type = TYPE_REAL
    })

    # This property won't get added to the group
    # due to not having the "rotate_" prefix.
    properties.append({
        name = "trail_color",
        type = TYPE_COLOR
    })
    return properties
```
-   `name` is the name of a group which is going to be displayed as collapsible list of properties
-   Every following property added after the group property with the prefix (which determined by `hint_string`) will be shortened. For instance, `rotate_speed` is going to be shortened to `speed` in this case. However, `movement_speed` won't be a part of the group and will not be shortened
-   `PROPERTY_USAGE_GROUP` indicates that the property should be treated as a script group specifically, so the type `TYPE_NIL` can be ignored as it won't be actually used for the scripting logic, yet it must be defined anyway.