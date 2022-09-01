#gdscript #godot #scripting 

see also: [[Exports]], [[Exporting Properties]]

----


# Advanced exports[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_exports.html#advanced-exports "Permalink to this headline")
Not every type of export can be provided on the level of the language itself to avoid unnecessary design complexity. The following describes some more or less common exporting features which can be implemented with a low-level API.

Before reading further, you should get familiar with the way [[Properties]] are handled and how they can be customized with [_set()](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object-method-get-property-list), [_get()](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object-method-get-property-list), and [_get_property_list()](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object-method-get-property-list) methods as described in [Accessing data or logic from an object](https://docs.godotengine.org/en/stable/tutorials/best_practices/godot_interfaces.html#doc-accessing-data-or-logic-from-object).

**See also**
For binding properties using the above methods in C++, see [Binding properties using _set/_get/_get_property_list](https://docs.godotengine.org/en/stable/development/cpp/object_class.html#doc-binding-properties-using-set-get-property-list).

**Warning**
The script must operate in the [[Tool Mode]] so the above methods can work from within the editor.

----


(see [[Properties]])

![[Exporting Properties]]