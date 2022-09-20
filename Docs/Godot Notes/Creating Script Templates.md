%%
#scripting #gdscript #csharp #godot 
%%


# Creating script templates[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/creating_script_templates.html#creating-script-templates "Permalink to this headline")

Godot provides a way to use script templates as seen in the `Script Create Dialog` while creating a new script:

![../../_images/script_create_dialog_templates.png](https://docs.godotengine.org/en/stable/_images/script_create_dialog_templates.png)

A set of default script templates is provided by default, but it's also possible to modify existing and create new ones, both per project and the editor.

## Locating the templates[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/creating_script_templates.html#locating-the-templates "Permalink to this headline")

There are two places where templates can be managed.

### Editor-defined templates[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/creating_script_templates.html#editor-defined-templates "Permalink to this headline")
These are available globally throughout any project. The location of these templates are determined per each OS:

-   Windows: `%APPDATA%\Godot\script_templates\`
-   Linux: `$HOME/.config/godot/script_templates/`
-   macOS: `$HOME/Library/Application Support/Godot/script_templates/`

If no `script_templates` is detected, Godot will create a default set of built-in templates automatically, so this logic can be used to reset the default templates in case you've accidentally overwritten them.


### Project-defined templates[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/creating_script_templates.html#project-defined-templates "Permalink to this headline")
The default path to search for templates is the `res://script_templates/` directory. The path can be changed by configuring the `editor/script_templates_search_path` setting in the [ProjectSettings](https://docs.godotengine.org/en/stable/classes/class_projectsettings.html#class-projectsettings), both via code and the editor.
If no `script_templates` directory is found within a project, it is simply ignored.


## List of template placeholders[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/creating_script_templates.html#list-of-template-placeholders "Permalink to this headline")
The following describes the complete list of built-in template placeholders which are currently implemented.

### Base placeholders[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/creating_script_templates.html#base-placeholders "Permalink to this headline")
| Placeholder | Description |
| ----------- | ----------- |
| `%CLASS%`   |   The name of the new class (used in C# only).          |
| `%BASE%`    |  The base type a new script inherits from.           |
| `%TS%`            | Indentation placeholder. The exact type and number of whitespace characters used for indentation is determined by the `text_editor/indent/type` and `text_editor/indent/size` settings in the [EditorSettings](https://docs.godotengine.org/en/stable/classes/class_editorsettings.html#class-editorsettings) respectively.            |

## Type placeholders[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/creating_script_templates.html#type-placeholders "Permalink to this headline")

These are only relevant for GDScript with static typing. Whether these placeholders are actually replaced is determined by the `text_editor/completion/add_type_hints` setting in the [EditorSettings](https://docs.godotengine.org/en/stable/classes/class_editorsettings.html#class-editorsettings).
![[Pasted image 20220918212950.png]]




